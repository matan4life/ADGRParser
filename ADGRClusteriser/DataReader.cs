using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ADGRClusteriser
{
    static class DataReader
    {
        public static string ConnectionString { get; set; } = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static List<PlayerData> GetPlayerData()
        {
            var countries = GetAllCountries();
            var result = new List<PlayerData>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT Users.Gender, Users.Age, Users.Country, SUM(Currency.Price), Cheats.Cheat, Users.Id FROM [Users] LEFT OUTER JOIN [Currency] ON Currency.UserId=Users.Id INNER JOIN Cheats ON Cheats.UserId = Users.Id GROUP BY Users.Gender, Users.Age, Users.Country, Cheats.Cheat, Users.Id";
                var command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new PlayerData()
                        {
                            Sex = (reader.GetString(0) == "male") ? 0.0F : 1.0F,
                            Age = reader.GetInt32(1),
                            Country = countries.IndexOf(reader.GetString(2)),
                            Profit = (DBNull.Value.Equals(reader.GetValue(3))) ? 0.0F : decimal.ToSingle(reader.GetDecimal(3)),
                            Cheats = Convert.ToSingle(reader.GetBoolean(4)),
                            UserId = reader.GetString(5)
                        });
                    }
                }
            }
            Console.WriteLine("OK");
            return result;
        }

        public static List<string> GetAllCountries()
        {
            var result = new List<string>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT DISTINCT Country From Users";
                var command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                    return result.OrderBy(x=>x).ToList();
                }
            }
        }
    }
}
