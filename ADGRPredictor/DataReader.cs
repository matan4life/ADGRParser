using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADGRPredictor
{
    static class DataReader
    {
        public static string ConnectionString { get; set; } = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static string ConnectionString1 { get; set; } = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static Dictionary<int, T> GetData<T, U>(U clusterId, string column, string tableName, string clusterColumnName)
        {
            var result = new Dictionary<int, T>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT [Date] FROM [Statistics]";
                var command = new SqlCommand(sql, connection);
                var firstDate = Convert.ToDateTime(command.ExecuteScalar());
                sql = $"SELECT [Date], [{column}] FROM [{tableName}] WHERE {clusterColumnName} = @cluster";
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@cluster", clusterId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add((int)(Convert.ToDateTime(reader[0]) - firstDate).TotalDays, (T)reader[1]);
                    }
                }
            }
            return result;
        }

        public static DateTime GetFirstDate()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT [Date] FROM [Statistics]";
                var command = new SqlCommand(sql, connection);
                return Convert.ToDateTime(command.ExecuteScalar());
            }
        }

        public static Dictionary<int, decimal> TransformData(Dictionary<int, int> dict)
        {
            var result = new Dictionary<int, decimal>();
            foreach (var pair in dict)
            {
                result.Add(pair.Key, (decimal)pair.Value);
            }
            return result;
        }

        public static void UploadInfo<T>(T clusterId, string tableName, List<DateTime> dates, List<int> DAUs, List<int> NewUsers, List<decimal> Revenues, List<int> Items, List<decimal> Income)
        {
            using (var connection = new SqlConnection(ConnectionString1))
            {
                connection.Open();
                string sql = $"INSERT INTO [{tableName}] VALUES(@c, @d, @dau, @new, @rev, @it, @inc)";
                foreach (var date in dates)
                {
                    var command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@c", clusterId);
                    command.Parameters.AddWithValue("@d", date);
                    command.Parameters.AddWithValue("@dau", DAUs[dates.IndexOf(date)]);
                    command.Parameters.AddWithValue("@new", NewUsers[dates.IndexOf(date)]);
                    command.Parameters.AddWithValue("@rev", Revenues[dates.IndexOf(date)]);
                    command.Parameters.AddWithValue("@it", Items[dates.IndexOf(date)]);
                    command.Parameters.AddWithValue("@inc", Income[dates.IndexOf(date)]);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static (List<DateTime>, List<int>) DivideIntData(Dictionary<int, int> dict)
        {
            var list1 = new List<DateTime>();
            var list2 = new List<int>();
            var firstDate = DataReader.GetFirstDate();
            foreach (var pair in dict)
            {
                list1.Add(firstDate.AddDays(pair.Key));
                list2.Add(pair.Value);
            }
            return (list1, list2);
        }

        public static (List<DateTime>, List<decimal>) DividedecimalData(Dictionary<int, decimal> dict)
        {
            var list1 = new List<DateTime>();
            var list2 = new List<decimal>();
            var firstDate = DataReader.GetFirstDate();
            foreach (var pair in dict)
            {
                list1.Add(firstDate.AddDays(pair.Key));
                list2.Add((decimal)pair.Value);
            }
            return (list1, list2);
        }
    }
}
