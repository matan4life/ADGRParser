using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ADGRVizualizer.Models;
using System.Data.SqlClient;

namespace ADGRVizualizer.Controllers
{
    public class HomeController : Controller
    {
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
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                connection.Open();
                string sql = "SELECT [Date], [DAU], [NewUsers], [Revenue], [Items], [ItemsUSDIncome] FROM [Statistics]";
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
            return View((DAU, NewUsers, Revenue, Items, USD, DAUPredicted, NewUsersPredicted, RevenuePredicted, ItemsPredicted, USDPredicted));
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
    }
}
