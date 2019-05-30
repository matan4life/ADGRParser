using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ADGRVizualizer.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADGRVizualizer.Controllers
{
    public class HomeController : Controller
    {
        public static DateTime CurrentDate { get; set; } = new DateTime(2018, 1, 1);
        public async Task<IActionResult> ProfitDistribution()
        {
            var data = await GetProfits(CurrentDate.ToString("yyyy-MM-dd"));
            var dates = new List<DateTime>();
            Enumerable.Range(1, 30).ToList().ForEach(x => dates.Add(new DateTime(2018, 1, x)));
            ViewBag.Dates = dates.Select(x => new SelectListItem() { Value = x.ToShortDateString(), Text = x.ToShortDateString() }).ToList();
            return View((new SelectListItem()
            {
                Value = CurrentDate.ToShortDateString(),
                Text = CurrentDate.ToShortDateString()
            }, data));
        }
        public IActionResult Index()
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
            var currency = new List<SimpleReportViewModel>();
            var loseRate = new List<SimpleReportViewModel>();
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                connection.Open();
                string sql = "SELECT [Date], [DAU], [NewUsers], [Revenue], [Items], [ItemsUSDIncome], [Currency], CAST(Loss AS float) / Starts FROM [Statistics]";
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
                        currency.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetDecimal(6)
                        });
                        loseRate.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = Convert.ToDecimal(reader.GetDouble(7))
                        });
                    }
                }

                sql = "SELECT [Date], [DAU], [NewUsers], [Revenue], [Items], [USDItemsIncome] FROM [Predictions]";
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
            return View((DAU, NewUsers, Revenue, Items, USD, DAUPredicted, NewUsersPredicted, RevenuePredicted, ItemsPredicted, USDPredicted, currency, loseRate));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChangeDate(string date, string returnUrl)
        {
            CurrentDate = DateTime.Parse(date);
            return RedirectToAction("ProfitDistribution");
        }

        public async Task<List<SimpleReportViewModel>> GetProfits(string date)
        {
            var result = new List<SimpleReportViewModel>();
            using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                await connection.OpenAsync();
                string sql = $"SELECT ChestName, SUM(Price) FROM Currency WHERE Date = '{date}' GROUP BY ChestName";
                var command = new SqlCommand(sql, connection);
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
    }
}
