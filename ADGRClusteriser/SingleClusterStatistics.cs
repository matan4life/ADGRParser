using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ADGRClusteriser
{
    public static class SingleClusterStatistics
    {
        public static void Calculate()
        {
            CalculateForGenders();
        }
        public static void CalculateForCheats()
        {
            var dau = AddNotExistingValues(GetDau<bool>("Cheats", "Cheat"), default, new List<bool>() { true, false });
            Console.WriteLine("DAU statistics extracted");
            var newUsers = AddNotExistingValues(NewUsers<bool>("Cheats", "Cheat"), default, new List<bool>() { true, false });
            Console.WriteLine("New users statistics extracted");
            var mau = AddNotExistingValues(GetMau<bool>("Cheats", "Cheat"), default, new List<bool>() { true, false });
            Console.WriteLine("MAU statistics extracted");
            var revenue = AddNotExistingValues(GetRevenue<bool>("Cheats", "Cheat"), default, new List<bool>() { true, false });
            Console.WriteLine("Profit statistics extracted");
            var currency = AddNotExistingValues(GetCurrency<bool>("Cheats", "Cheat"), 1.0M, new List<bool>() { true, false });
            Console.WriteLine("Currency statistics extracted");
            var starts = AddNotExistingValues(GetStarts<bool>("Cheats", "Cheat"), default, new List<bool>() { true, false });
            Console.WriteLine("Starts statistics extracted");
            var ends = AddNotExistingValues(GetEnds<bool>("Cheats", "Cheat"), default, new List<bool>() { true, false });
            Console.WriteLine("Loses statistics extracted");
            var wins = AddNotExistingValues(GetWins<bool>("Cheats", "Cheat"), default, new List<bool>() { true, false });
            Console.WriteLine("Wins statistics extracted");
            var usdincome = AddNotExistingValues(GetStageUSDIncome<bool>("Cheats", "Cheat", new List<bool>() { true, false }), default, new List<bool>() { true, false });
            Console.WriteLine("Income stage statistics extracted");
            var items = AddNotExistingValues(GetItems<bool>("Cheats", "Cheat"), default, new List<bool>() { true, false });
            Console.WriteLine("Items statistics extracted");
            var itemsincome = AddNotExistingValues(GetIncome<bool>("Cheats", "Cheat"), default, new List<bool>() { true, false });
            Console.WriteLine("Items income statistics extracted");
            var usditemsincome = AddNotExistingValues(GetItemsUSDIncome<bool>("Cheats", "Cheat", new List<bool>() { true, false }), default, new List<bool>() { true, false });
            Console.WriteLine("USD Items income statistics extracted");
            var countries = dau.Select(x => x.Item1).OrderBy(y => y).ToList();
            var dates = GetDates();
            using (var connection = new SqlConnection(DataReader.ConnectionString))
            {
                connection.Open();
                for (int i = 0; i < dau.Count; i++)
                {
                    string sql = "INSERT INTO [CheatsClusterStatistics] VALUES(@id, @dat, @dau, @new, @mau, @rev, @cur, @sta, @end, @win, @sti, @ite, @iti, @ius)";
                    var sqlcommand = new SqlCommand(sql, connection);
                    sqlcommand.Parameters.AddWithValue("@id", dau[i].Item1);
                    sqlcommand.Parameters.AddWithValue("@dat", dau[i].Item2);
                    sqlcommand.Parameters.AddWithValue("@dau", dau[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@new", newUsers[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@mau", mau.Where(x => x.Item1 == dau[i].Item1).FirstOrDefault().Item3);
                    sqlcommand.Parameters.AddWithValue("@rev", revenue[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@cur", currency[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sta", starts[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@end", ends[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@win", wins[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sti", usdincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ite", items[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@iti", itemsincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ius", usditemsincome[i].Item3);
                    sqlcommand.ExecuteNonQuery();
                }
            }
        }
        public static void CalculateForProfits()
        {
            var dau = AddNotExistingValues(GetDau<int>("RevenueClusters", "ClusterId"), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("DAU statistics extracted");
            var newUsers = AddNotExistingValues(NewUsers<int>("RevenueClusters", "ClusterId"), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("New users statistics extracted");
            var mau = AddNotExistingValues(GetMau<int>("RevenueClusters", "ClusterId"), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("MAU statistics extracted");
            var revenue = AddNotExistingValues(GetRevenue<int>("RevenueClusters", "ClusterId"), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("Profit statistics extracted");
            var currency = AddNotExistingValues(GetCurrency<int>("RevenueClusters", "ClusterId"), 1.0M, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("Currency statistics extracted");
            var starts = AddNotExistingValues(GetStarts<int>("RevenueClusters", "ClusterId"), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("Starts statistics extracted");
            var ends = AddNotExistingValues(GetEnds<int>("RevenueClusters", "ClusterId"), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("Loses statistics extracted");
            var wins = AddNotExistingValues(GetWins<int>("RevenueClusters", "ClusterId"), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("Wins statistics extracted");
            var usdincome = AddNotExistingValues(GetStageUSDIncome<int>("RevenueClusters", "ClusterId", Enumerable.Range(1, 4).ToList()), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("Income stage statistics extracted");
            var items = AddNotExistingValues(GetItems<int>("RevenueClusters", "ClusterId"), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("Items statistics extracted");
            var itemsincome = AddNotExistingValues(GetIncome<int>("RevenueClusters", "ClusterId"), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("Items income statistics extracted");
            var usditemsincome = AddNotExistingValues(GetItemsUSDIncome<int>("RevenueClusters", "ClusterId", Enumerable.Range(1, 4).ToList()), default, Enumerable.Range(1, 4).ToList());
            Console.WriteLine("USD Items income statistics extracted");
            var countries = dau.Select(x => x.Item1).OrderBy(y => y).ToList();
            var dates = GetDates();
            using (var connection = new SqlConnection(DataReader.ConnectionString))
            {
                connection.Open();
                for (int i = 0; i < dau.Count; i++)
                {
                    string sql = "INSERT INTO [ProfitClusterStatistics] VALUES(@id, @dat, @dau, @new, @mau, @rev, @cur, @sta, @end, @win, @sti, @ite, @iti, @ius)";
                    var sqlcommand = new SqlCommand(sql, connection);
                    sqlcommand.Parameters.AddWithValue("@id", dau[i].Item1);
                    sqlcommand.Parameters.AddWithValue("@dat", dau[i].Item2);
                    sqlcommand.Parameters.AddWithValue("@dau", dau[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@new", newUsers[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@mau", mau.Where(x => x.Item1 == dau[i].Item1).FirstOrDefault().Item3);
                    sqlcommand.Parameters.AddWithValue("@rev", revenue[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@cur", currency[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sta", starts[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@end", ends[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@win", wins[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sti", usdincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ite", items[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@iti", itemsincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ius", usditemsincome[i].Item3);
                    sqlcommand.ExecuteNonQuery();
                }
            }
        }
        public static void CalculateForAges()
        {
            var dau = AddNotExistingValues(GetDau<int>("AgeClusters", "ClusterId"), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("DAU statistics extracted");
            var newUsers = AddNotExistingValues(NewUsers<int>("AgeClusters", "ClusterId"), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("New users statistics extracted");
            var mau = AddNotExistingValues(GetMau<int>("AgeClusters", "ClusterId"), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("MAU statistics extracted");
            var revenue = AddNotExistingValues(GetRevenue<int>("AgeClusters", "ClusterId"), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("Profit statistics extracted");
            var currency = AddNotExistingValues(GetCurrency<int>("AgeClusters", "ClusterId"), 1.0M, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("Currency statistics extracted");
            var starts = AddNotExistingValues(GetStarts<int>("AgeClusters", "ClusterId"), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("Starts statistics extracted");
            var ends = AddNotExistingValues(GetEnds<int>("AgeClusters", "ClusterId"), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("Loses statistics extracted");
            var wins = AddNotExistingValues(GetWins<int>("AgeClusters", "ClusterId"), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("Wins statistics extracted");
            var usdincome = AddNotExistingValues(GetStageUSDIncome<int>("AgeClusters", "ClusterId", Enumerable.Range(1, 6).ToList()), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("Income stage statistics extracted");
            var items = AddNotExistingValues(GetItems<int>("AgeClusters", "ClusterId"), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("Items statistics extracted");
            var itemsincome = AddNotExistingValues(GetIncome<int>("AgeClusters", "ClusterId"), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("Items income statistics extracted");
            var usditemsincome = AddNotExistingValues(GetItemsUSDIncome<int>("AgeClusters", "ClusterId", Enumerable.Range(1, 6).ToList()), default, Enumerable.Range(1, 6).ToList());
            Console.WriteLine("USD Items income statistics extracted");
            var countries = dau.Select(x => x.Item1).OrderBy(y => y).ToList();
            var dates = GetDates();
            using (var connection = new SqlConnection(DataReader.ConnectionString))
            {
                connection.Open();
                for (int i = 0; i < dau.Count; i++)
                {
                    string sql = "INSERT INTO [AgeClusterStatistics] VALUES(@id, @dat, @dau, @new, @mau, @rev, @cur, @sta, @end, @win, @sti, @ite, @iti, @ius)";
                    var sqlcommand = new SqlCommand(sql, connection);
                    sqlcommand.Parameters.AddWithValue("@id", dau[i].Item1);
                    sqlcommand.Parameters.AddWithValue("@dat", dau[i].Item2);
                    sqlcommand.Parameters.AddWithValue("@dau", dau[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@new", newUsers[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@mau", mau.Where(x => x.Item1 == dau[i].Item1).FirstOrDefault().Item3);
                    sqlcommand.Parameters.AddWithValue("@rev", revenue[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@cur", currency[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sta", starts[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@end", ends[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@win", wins[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sti", usdincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ite", items[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@iti", itemsincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ius", usditemsincome[i].Item3);
                    sqlcommand.ExecuteNonQuery();
                }
            }
        }
        public static void CalculateForGenders()
        {
            var dau = AddNotExistingValues(GetDau<string>("Users", "Gender"), default, new List<string>() { "male", "female" });
            Console.WriteLine("DAU statistics extracted");
            var newUsers = AddNotExistingValues(NewUsers<string>("Users", "Gender"), default, new List<string>() { "male", "female" });
            Console.WriteLine("New users statistics extracted");
            var mau = AddNotExistingValues(GetMau<string>("Users", "Gender"), default, new List<string>() { "male", "female" });
            Console.WriteLine("MAU statistics extracted");
            var revenue = AddNotExistingValues(GetRevenue<string>("Users", "Gender"), default, new List<string>() { "male", "female" });
            Console.WriteLine("Profit statistics extracted");
            var currency = AddNotExistingValues(GetCurrency<string>("Users", "Gender"), 1.0M, new List<string>() { "male", "female" });
            Console.WriteLine("Currency statistics extracted");
            var starts = AddNotExistingValues(GetStarts<string>("Users", "Gender"), default, new List<string>() { "male", "female" });
            Console.WriteLine("Starts statistics extracted");
            var ends = AddNotExistingValues(GetEnds<string>("Users", "Gender"), default, new List<string>() { "male", "female" });
            Console.WriteLine("Loses statistics extracted");
            var wins = AddNotExistingValues(GetWins<string>("Users", "Gender"), default, new List<string>() { "male", "female" });
            Console.WriteLine("Wins statistics extracted");
            var usdincome = AddNotExistingValues(GetStageUSDIncome<string>("Users", "Gender", new List<string>() { "male", "female" }), default, new List<string>() { "male", "female" });
            Console.WriteLine("Income stage statistics extracted");
            var items = AddNotExistingValues(GetItems<string>("Users", "Gender"), default, new List<string>() { "male", "female" });
            Console.WriteLine("Items statistics extracted");
            var itemsincome = AddNotExistingValues(GetIncome<string>("Users", "Gender"), default, new List<string>() { "male", "female" });
            Console.WriteLine("Items income statistics extracted");
            var usditemsincome = AddNotExistingValues(GetItemsUSDIncome<string>("Users", "Gender", new List<string>() { "male", "female" }), default, new List<string>() { "male", "female" });
            Console.WriteLine("USD Items income statistics extracted");
            var countries = dau.Select(x => x.Item1).OrderBy(y => y).ToList();
            var dates = GetDates();
            using (var connection = new SqlConnection(DataReader.ConnectionString))
            {
                connection.Open();
                for (int i = 0; i < dau.Count; i++)
                {
                    string sql = "INSERT INTO [SexClusterStatistics] VALUES(@id, @dat, @dau, @new, @mau, @rev, @cur, @sta, @end, @win, @sti, @ite, @iti, @ius)";
                    var sqlcommand = new SqlCommand(sql, connection);
                    sqlcommand.Parameters.AddWithValue("@id", dau[i].Item1);
                    sqlcommand.Parameters.AddWithValue("@dat", dau[i].Item2);
                    sqlcommand.Parameters.AddWithValue("@dau", dau[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@new", newUsers[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@mau", mau.Where(x => x.Item1 == dau[i].Item1).FirstOrDefault().Item3);
                    sqlcommand.Parameters.AddWithValue("@rev", revenue[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@cur", currency[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sta", starts[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@end", ends[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@win", wins[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sti", usdincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ite", items[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@iti", itemsincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ius", usditemsincome[i].Item3);
                    sqlcommand.ExecuteNonQuery();
                }
            }
        }

        public static List<string> GetAllCountries()
        {
            var result = new List<string>();
            using (var connection = new SqlConnection(DataReader.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT DISTINCT Country FROM Users";
                var command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) {
                        result.Add(reader.GetString(0));
                    }
                }
                return result;
            }
        }
        public static void CalculateForCountries()
        {
            var dau = AddNotExistingValues(GetDau<string>("Users", "Country"), default, GetAllCountries());
            Console.WriteLine("DAU statistics extracted");
            var newUsers = AddNotExistingValues(NewUsers<string>("Users", "Country"), default, GetAllCountries());
            Console.WriteLine("New users statistics extracted");
            var mau = AddNotExistingValues(GetMau<string>("Users", "Country"), default, GetAllCountries());
            Console.WriteLine("MAU statistics extracted");
            var revenue = AddNotExistingValues(GetRevenue<string>("Users", "Country"), default, GetAllCountries());
            Console.WriteLine("Profit statistics extracted");
            var currency = AddNotExistingValues(GetCurrency<string>("Users", "Country"), 1.0M, GetAllCountries());
            Console.WriteLine("Currency statistics extracted");
            var starts = AddNotExistingValues(GetStarts<string>("Users", "Country"), default, GetAllCountries());
            Console.WriteLine("Starts statistics extracted");
            var ends = AddNotExistingValues(GetEnds<string>("Users", "Country"), default, GetAllCountries());
            Console.WriteLine("Loses statistics extracted");
            var wins = AddNotExistingValues(GetWins<string>("Users", "Country"), default, GetAllCountries());
            Console.WriteLine("Wins statistics extracted");
            var usdincome = AddNotExistingValues(GetStageUSDIncome<string>("Users", "Country", GetAllCountries()), default, GetAllCountries());
            Console.WriteLine("Income stage statistics extracted");
            var items = AddNotExistingValues(GetItems<string>("Users", "Country"), default, GetAllCountries());
            Console.WriteLine("Items statistics extracted");
            var itemsincome = AddNotExistingValues(GetIncome<string>("Users", "Country"), default, GetAllCountries());
            Console.WriteLine("Items income statistics extracted");
            var usditemsincome = AddNotExistingValues(GetItemsUSDIncome<string>("Users", "Country", GetAllCountries()), default, GetAllCountries());
            Console.WriteLine("USD Items income statistics extracted");
            var countries = dau.Select(x => x.Item1).OrderBy(y => y).ToList();
            var dates = GetDates();
            using (var connection = new SqlConnection(DataReader.ConnectionString))
            {
                connection.Open();
                for (int i=0; i<dau.Count; i++)
                {
                    string sql = "INSERT INTO [CountryClusterStatistics] VALUES(@id, @dat, @dau, @new, @mau, @rev, @cur, @sta, @end, @win, @sti, @ite, @iti, @ius)";
                    var sqlcommand = new SqlCommand(sql, connection);
                    sqlcommand.Parameters.AddWithValue("@id", dau[i].Item1);
                    sqlcommand.Parameters.AddWithValue("@dat", dau[i].Item2);
                    sqlcommand.Parameters.AddWithValue("@dau", dau[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@new", newUsers[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@mau", mau.Where(x=>x.Item1 == dau[i].Item1).FirstOrDefault().Item3);
                    sqlcommand.Parameters.AddWithValue("@rev", revenue[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@cur", currency[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sta", starts[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@end", ends[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@win", wins[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@sti", usdincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ite", items[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@iti", itemsincome[i].Item3);
                    sqlcommand.Parameters.AddWithValue("@ius", usditemsincome[i].Item3);
                    sqlcommand.ExecuteNonQuery();
                }
            }
        }
        public static List<(U, DateTime, V)> AddNotExistingValues<U, V>(List<(U, DateTime, V)> source, V value, List<U> clusters)
        {
            var dates = GetDates();
            var ban = new List<(U, DateTime, V)>();
            foreach (var date in dates)
            {
                foreach (var cluster in clusters)
                {
                    var result = source.Where(x => x.Item1.Equals(cluster) && x.Item2 == date).Count();
                    if (result == 0)
                    {
                        ban.Add((cluster, date, value));
                    }
                }
            }
            source.AddRange(ban);
            return source.OrderBy(x => x.Item1).ThenBy(y => y.Item2).ToList();
        }
        public static List<DateTime> GetDates()
        {
            var result = new List<DateTime>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date FROM Users GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]}");
                        result.Add(Convert.ToDateTime(reader[0]));
                    }
                }
            }
            return result;
        }
        public static List<(T, DateTime, int)> GetDau<T>(string clusterTable, string clusterName)
        {
            var result = new List<(T, DateTime, int)>();
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, Logins.Date, COUNT(DISTINCT Logins.UserId) FROM Logins INNER JOIN {clusterTable} ON Logins.UserId={clusterTable}.{userIdColumn} GROUP BY {clusterTable}.{clusterName}, Logins.Date ORDER BY {clusterTable}.{clusterName}, Logins.Date";
                SqlCommand command = new SqlCommand(sql, con);
                command.CommandTimeout = 0;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetInt32(2)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, int)> NewUsers<T>(string clusterTable, string clusterName)
        {
            var result = new List<(T, DateTime, int)>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = "";
                if (clusterTable != "Users")
                {
                    sql = $"SELECT {clusterTable}.{clusterName}, Users.Date, COUNT(Users.Id) FROM Users INNER JOIN {clusterTable} ON Users.Id={clusterTable}.UserId GROUP BY {clusterTable}.{clusterName}, Date ORDER BY {clusterTable}.{clusterName}, Date";
                }
                else
                {
                    sql = $"SELECT {clusterName}, Date, COUNT(Id) FROM Users GROUP BY {clusterName}, Date ORDER BY {clusterName}, Date";
                }
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetInt32(2)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, int)> GetMau<T>(string clusterTable, string clusterName)
        {
            var result = new List<(T, DateTime, int)>();
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, YEAR(Logins.Date), MONTH(Logins.Date), COUNT(DISTINCT Logins.UserId) FROM Logins INNER JOIN {clusterTable} ON Logins.UserId={clusterTable}.{userIdColumn} GROUP BY {clusterTable}.{clusterName}, YEAR(Logins.Date), MONTH(Logins.Date) ORDER BY {clusterTable}.{clusterName}, YEAR(Logins.Date), MONTH(Logins.Date)";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), new DateTime(reader.GetInt32(1), reader.GetInt32(2), 1), reader.GetInt32(3)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, decimal)> GetRevenue<T>(string clusterTable, string clusterName)
        {
            var result = new List<(T, DateTime, decimal)>();
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, Currency.Date, SUM(Currency.Price) FROM Currency INNER JOIN {clusterTable} ON Currency.UserId = {clusterTable}.{userIdColumn} GROUP BY {clusterTable}.{clusterName}, Currency.Date ORDER BY {clusterTable}.{clusterName}, Currency.Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetDecimal(2)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, decimal)> GetCurrency<T>(string clusterTable, string clusterName)
        {
            var dates = GetDates();
            var result = new List<(T, DateTime, decimal)>();
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, Currency.Date, SUM(Currency.Price)/Sum(Currency.Income) FROM Currency INNER JOIN {clusterTable} ON Currency.UserId = {clusterTable}.{userIdColumn} GROUP BY {clusterTable}.{clusterName}, Currency.Date ORDER BY {clusterTable}.{clusterName}, Currency.Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetDecimal(2)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, int)> GetStarts<T>(string clusterTable, string clusterName)
        {
            var dates = GetDates();
            var result = new List<(T, DateTime, int)>();
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, Starts.Date, COUNT(Starts.Id) FROM Starts INNER JOIN {clusterTable} ON Starts.UserId = {clusterTable}.{userIdColumn} GROUP BY {clusterTable}.{clusterName}, Starts.Date ORDER BY {clusterTable}.{clusterName}, Starts.Date";
                SqlCommand command = new SqlCommand(sql, con);
                command.CommandTimeout = 0;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetInt32(2)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, int)> GetEnds<T>(string clusterTable, string clusterName)
        {
            var dates = GetDates();
            var result = new List<(T, DateTime, int)>();
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, Ends.Date, COUNT(Ends.Id) FROM Ends INNER JOIN {clusterTable} ON Ends.UserId = {clusterTable}.{userIdColumn} WHERE Ends.HasWon='False' GROUP BY {clusterTable}.{clusterName}, Ends.Date ORDER BY {clusterTable}.{clusterName}, Ends.Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetInt32(2)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, int)> GetWins<T>(string clusterTable, string clusterName)
        {
            var dates = GetDates();
            var result = new List<(T, DateTime, int)>();
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, Ends.Date, COUNT(Ends.Id) FROM Ends INNER JOIN {clusterTable} ON Ends.UserId = {clusterTable}.{userIdColumn} WHERE Ends.HasWon='True' GROUP BY {clusterTable}.{clusterName}, Ends.Date ORDER BY {clusterTable}.{clusterName}, Ends.Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetInt32(2)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, decimal)> GetStageUSDIncome<T>(string clusterTable, string clusterName, List<T> clusters)
        {
            var dates = GetDates();
            var currency = AddNotExistingValues(GetCurrency<T>(clusterTable, clusterName), 1.0M, clusters);
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            var result = new List<(T, DateTime, decimal)>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, Ends.Date, SUM(Ends.Income) FROM Ends INNER JOIN {clusterTable} ON Ends.UserId = {clusterTable}.{userIdColumn} GROUP BY {clusterTable}.{clusterName}, Ends.Date ORDER BY {clusterTable}.{clusterName}, Ends.Date";
                SqlCommand command = new SqlCommand(sql, con);
                command.CommandTimeout = 0;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetInt32(2)));
                    }
                }
            }
            result = AddNotExistingValues(result, default, clusters);
            var final = new List<(T, DateTime, decimal)>();
            for (int i=0; i<currency.Count; i++)
            {
                final.Add((result[i].Item1, result[i].Item2, result[i].Item3 * currency[i].Item3));
            }
            return final;
        }

        public static List<(T, DateTime, int)> GetItems<T>(string clusterTable, string clusterName)
        {
            var dates = GetDates();
            var result = new List<(T, DateTime, int)>();
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, Items.Date, COUNT(Items.Id) FROM Items INNER JOIN {clusterTable} ON Items.UserId = {clusterTable}.{userIdColumn} GROUP BY {clusterTable}.{clusterName}, Items.Date ORDER BY {clusterTable}.{clusterName}, Items.Date";
                SqlCommand command = new SqlCommand(sql, con);
                command.CommandTimeout = 0;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetInt32(2)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, int)> GetIncome<T>(string clusterTable, string clusterName)
        {
            var dates = GetDates();
            var result = new List<(T, DateTime, int)>();
            string userIdColumn = (clusterTable == "Users") ? "Id" : "UserId";
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT {clusterTable}.{clusterName}, Items.Date, SUM(Items.Price) FROM Items INNER JOIN {clusterTable} ON Items.UserId = {clusterTable}.{userIdColumn} GROUP BY {clusterTable}.{clusterName}, Items.Date ORDER BY {clusterTable}.{clusterName}, Items.Date";
                SqlCommand command = new SqlCommand(sql, con);
                command.CommandTimeout = 0;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(((T)reader.GetValue(0), reader.GetDateTime(1), reader.GetInt32(2)));
                    }
                }
            }
            return result;
        }

        public static List<(T, DateTime, decimal)> GetItemsUSDIncome<T>(string clusterTable, string clusterName, List<T> clusters)
        {
            var income = AddNotExistingValues(GetIncome<T>(clusterTable, clusterName), default, clusters);
            var currency = AddNotExistingValues(GetCurrency<T>(clusterTable, clusterName), 1.0M, clusters);
            var result = new List<(T, DateTime, decimal)>();
            for (int i=0; i<currency.Count; i++)
            {
                result.Add((income[i].Item1, income[i].Item2, income[i].Item3 * currency[i].Item3));
            }
            return result;
        }
    }
}
