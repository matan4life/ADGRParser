using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ADGRVizualizer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADGRVizualizer.Controllers
{
    public class MultipleClusterController : Controller
    {
        public static int CurrentClusterId { get; set; } = 1;
        public static DateTime CurrentDate { get; set; } = new DateTime(2018, 1, 1);
        public IActionResult Index()
        {
            var data = GetClusterData(CurrentClusterId);
            ViewBag.Clusters = Enumerable.Range(1, 600).Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
            return View((new SelectListItem { Value = CurrentClusterId.ToString(), Text = CurrentClusterId.ToString() }, data));
        }

        public IActionResult Count()
        {
            var result = GetUserCount().OrderByDescending(x=>x.Quantity).ToList();
            var normalize = NormalizeData(result);
            return View((normalize, result));
        }

        public async Task<IActionResult> ProfitDistribution()
        {
            var data = await GetProfit(CurrentDate.ToString("yyyy-MM-dd"), CurrentClusterId);
            var dates = new List<DateTime>();
            Enumerable.Range(1, 30).ToList().ForEach(x => dates.Add(new DateTime(2018, 1, x)));
            ViewBag.Dates = dates.Select(x => new SelectListItem() { Value = x.ToShortDateString(), Text = x.ToShortDateString() }).ToList();
            ViewBag.Clusters = Enumerable.Range(1, 600).Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
            return View((new SelectListItem() { Value = CurrentClusterId.ToString(), Text = CurrentClusterId.ToString() },
                new SelectListItem() { Value = CurrentDate.ToShortDateString(), Text = CurrentDate.ToShortDateString() },
                data));
        }

        public async Task<List<SimpleReportViewModel>> GetProfit(string dateStart, int clusterId)
        {
            List<SimpleReportViewModel> result = new List<SimpleReportViewModel>();
            using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                await connection.OpenAsync();
                string sql = $"SELECT ChestName, SUM(Price) FROM Currency WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) AND Date = '{dateStart}' GROUP BY ChestName";
                var command = new SqlCommand(sql, connection);
                command.CommandTimeout = 0;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetString(0),
                            Quantity = reader.GetDecimal(1)
                        });
                    }
                    return result;
                }
            }
        }

        public IActionResult ChangeCluster(string clusterId, string returnUrl)
        {
            CurrentClusterId = int.Parse(clusterId);
            return RedirectToAction("Index");
        }

        public IActionResult ChangeDate(string date, string returnUrl)
        {
            CurrentDate = DateTime.Parse(date);
            return RedirectToAction("ProfitDistribution");
        }

        public IActionResult DistributeChangeCluster(string clusterId, string returnUrl)
        {
            CurrentClusterId = int.Parse(clusterId);
            return RedirectToAction("ProfitDistribution");
        }

        [NonAction]
        public List<SimpleReportViewModel> NormalizeData(List<SimpleReportViewModel> source)
        {
            var result = new List<SimpleReportViewModel>();
            source.ForEach(x => result.Add(new SimpleReportViewModel()
            {
                DimensionOne = x.DimensionOne,
                Quantity = x.Quantity * 100 / 812151
            }));
            return result;
        }


        [NonAction]
        public List<SimpleReportViewModel> GetUserCount()
        {
            var result = new List<SimpleReportViewModel>();
            using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                connection.Open();
                string sql = "SELECT ClusterId, COUNT(Id) FROM UserClusters GROUP BY ClusterId";
                var command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetInt32(0).ToString(),
                            Quantity = reader.GetInt32(1)
                        });
                    }
                    return result;
                }
            }
        }
        [NonAction]
        public (List<SimpleReportViewModel>, List<SimpleReportViewModel>, List<SimpleReportViewModel>,
            List<SimpleReportViewModel>, List<SimpleReportViewModel>, List<SimpleReportViewModel>,
            List<SimpleReportViewModel>, List<SimpleReportViewModel>, List<SimpleReportViewModel>,
            List<SimpleReportViewModel>, List<SimpleReportViewModel>, List<SimpleReportViewModel>) GetClusterData(int clusterId)
        {
            List<SimpleReportViewModel> DAU = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> NewUsers = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> Revenue = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> Items = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> USD = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> DAUPredicted = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> NewUsersPredicted = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> RevenuePredicted = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> ItemsPredicted = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> USDPredicted = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> Currency = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> LoseRate = new List<SimpleReportViewModel>();
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                connection.Open();
                string sql = $"SELECT [Date], [DAU], [NewUsers], [Revenue], [Items], [ItemsUSDIncome], [Currency], CAST(Loss AS float) / Starts FROM [ClusterStatistics] WHERE ClusterId = {clusterId}";
                var command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DAU.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[1])
                        });
                        NewUsers.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[2])
                        });
                        Revenue.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[3])
                        });
                        Items.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[4])
                        });
                        USD.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[5])
                        });
                        Currency.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetDecimal(6)
                        });
                        LoseRate.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader.GetDouble(7))
                        });

                    }
                }

                sql = $"SELECT [Date], [DAU], [NewUsers], [Revenue], [Items], [USDItemsIncome] FROM [ClusterPredictions] WHERE ClusterId = {clusterId}";
                command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DAUPredicted.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[1])
                        });
                        NewUsersPredicted.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[2])
                        });
                        RevenuePredicted.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[3])
                        });
                        ItemsPredicted.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[4])
                        });
                        USDPredicted.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = Convert.ToDateTime(reader[0]).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader[5])
                        });
                    }
                }
            }
            return (DAU, NewUsers, Revenue, Items, USD, DAUPredicted, NewUsersPredicted, RevenuePredicted, ItemsPredicted, USDPredicted, Currency, LoseRate);
        }
    }
}