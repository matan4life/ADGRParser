using ADGRVizualizer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ADGRVizualizer.Controllers
{
    public static class Data
    {
        public static async Task<(List<SimpleReportViewModel>, List<SimpleReportViewModel>, List<SimpleReportViewModel>,
            List<SimpleReportViewModel>, List<SimpleReportViewModel>, List<SimpleReportViewModel>,
            List<SimpleReportViewModel>)> GetSingleClusterStatistics<T>(string tableName, T clusterId, string idColumnName)
        {
            List<SimpleReportViewModel> DAU = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> NewUsers = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> Revenue = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> Items = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> USD = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> Currency = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> LoseRate = new List<SimpleReportViewModel>();
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                await connection.OpenAsync();
                string sql = $"SELECT Date, DAU, NewUsers, Revenue, Items, ItemsUSDIncome, Currency, CAST(Loss AS float) / Starts FROM {tableName} WHERE {idColumnName} = @cid";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@cid", clusterId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        DAU.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetInt32(1)
                        });
                        NewUsers.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetInt32(2)
                        });
                        Revenue.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetDecimal(3)
                        });
                        Items.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetInt32(4)
                        });
                        USD.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetDecimal(5)
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
                    return (DAU, NewUsers, Revenue, Items, USD, Currency, LoseRate);
                }
            }
        }

        public static async Task<(List<SimpleReportViewModel>, List<SimpleReportViewModel>, List<SimpleReportViewModel>,
            List<SimpleReportViewModel>, List<SimpleReportViewModel>)> GetSingleClusterPredictions<T>(string tableName, T clusterId, string idColumnName)
        {
            List<SimpleReportViewModel> DAU = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> NewUsers = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> Revenue = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> Items = new List<SimpleReportViewModel>();
            List<SimpleReportViewModel> USD = new List<SimpleReportViewModel>();
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                await connection.OpenAsync();
                string sql = $"SELECT Date, DAU, NewUsers, Revenue, Items, USDItemsIncome FROM {tableName} WHERE {idColumnName} = @cid";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@cid", clusterId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        DAU.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetInt32(1)
                        });
                        NewUsers.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetInt32(2)
                        });
                        Revenue.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetDecimal(3)
                        });
                        Items.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetInt32(4)
                        });
                        USD.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetDateTime(0).ToShortDateString(),
                            Quantity = reader.GetDecimal(5)
                        });
                    }
                    return (DAU, NewUsers, Revenue, Items, USD);
                }
            }
        }

        public static async Task<List<SimpleReportViewModel>> GetProfits<T>(string tableName, string idColumnName, T clusterId, string clusterColumnName, string date)
        {
            var result = new List<SimpleReportViewModel>();
            using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                await connection.OpenAsync();
                string sql = $"SELECT ChestName, SUM(Price) FROM Currency WHERE UserId IN (SELECT {idColumnName} FROM {tableName} WHERE {clusterColumnName} = @cid) AND Date = '{date}' GROUP BY ChestName";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@cid", clusterId);
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

        public static async Task<List<SimpleReportViewModel>> GetClusterCount(string tableName, string idColumnName, string clusterColumnName)
        {
            var result = new List<SimpleReportViewModel>();
            using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                await connection.OpenAsync();
                string sql = $"SELECT {clusterColumnName}, COUNT({idColumnName}) FROM {tableName} GROUP BY {clusterColumnName}";
                var command = new SqlCommand(sql, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(new SimpleReportViewModel()
                        {
                            DimensionOne = reader.GetValue(0).ToString(),
                            Quantity = reader.GetInt32(1)
                        });
                    }
                    return result;
                }
            }
        }

        public static List<SimpleReportViewModel> NormalizeCount(List<SimpleReportViewModel> source)
        {
            var result = new List<SimpleReportViewModel>();
            source.ForEach(x => result.Add(new SimpleReportViewModel()
            {
                DimensionOne = x.DimensionOne,
                Quantity = x.Quantity * 100 / 812151
            }));
            return result;
        }

        public static async Task<List<string>> GetAllCountries()
        {
            var result = new List<string>();
            using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                await connection.OpenAsync();
                string sql = "SELECT DISTINCT Country FROM Users";
                var command = new SqlCommand(sql, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                    return result.OrderBy(x => x).ToList();
                }
            }
        }
    }
}
