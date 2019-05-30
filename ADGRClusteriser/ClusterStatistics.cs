using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADGRClusteriser
{
    public static class ClusterStatistics
    {
        public static void Calculate()
        {
            for (int i = 1; i < 601; i++)
            {
                var dates = GetDates();
                var dau = GetDau(i);
                var newUsers = NewUsers(i);
                var mau = GetMau(i);
                var revenue = GetRevenue(i);
                var currency = GetCurrency(i);
                var starts = GetStarts(i);
                var ends = GetEnds(i);
                var wins = GetWins(i);
                var usdincome = GetStageUSDIncome(i);
                var items = GetItems(i);
                var itemsincome = GetIncome(i);
                var usditemsincome = GetItemsUSDIncome(i);
                Console.WriteLine("Start uploading statistics...");
                using (SqlConnection connection = new SqlConnection(DataReader.ConnectionString))
                {
                    connection.Open();
                    int counter = 0;
                    foreach (var date in dates)
                    {
                        Console.WriteLine($"Sending info from cluster {i} about {date.ToShortDateString()}");
                        string sql = "INSERT INTO [ClusterStatistics] VALUES(@id, @dat, @dau, @new, @mau, @rev, @cur, @sta, @end, @win, @sti, @ite, @iti, @ius)";
                        var sqlcommand = new SqlCommand(sql, connection);
                        sqlcommand.Parameters.AddWithValue("@id", i);
                        sqlcommand.Parameters.AddWithValue("@dat", date);
                        sqlcommand.Parameters.AddWithValue("@dau", dau[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@new", newUsers[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@mau", mau[new DateTime(date.Year, date.Month, 1)]);
                        sqlcommand.Parameters.AddWithValue("@rev", revenue[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@cur", currency[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@sta", starts[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@end", ends[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@win", wins[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@sti", usdincome[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@ite", items[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@iti", itemsincome[dates.IndexOf(date)]);
                        sqlcommand.Parameters.AddWithValue("@ius", usditemsincome[dates.IndexOf(date)]);
                        sqlcommand.ExecuteNonQuery();
                        counter++;
                        Console.WriteLine($"Progress: {(int)((double)counter / dates.Count * 100)}%");
                    }
                }
                Console.WriteLine("Statistics has been successfully uploaded");
            }
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

        public static List<int> GetDau(int clusterId)
        {
            var dates = GetDates();
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, COUNT(DISTINCT UserId) FROM Logins WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToInt32(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        public static List<int> NewUsers(int clusterId)
        {
            var dates = GetDates();
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, COUNT(Id) FROM Users WHERE Id IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToInt32(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        public static Dictionary<DateTime, int> GetMau(int clusterId)
        {
            var dates = GetDates();
            var result = new Dictionary<DateTime, int>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT YEAR(Date), MONTH(Date), COUNT(DISTINCT UserId) FROM Logins WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY YEAR(Date), MONTH(Date) ORDER BY YEAR(Date), MONTH(Date)";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new DateTime(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]), 1), Convert.ToInt32(reader[2]));
                    }
                }
            }
            return result;
        }

        public static List<decimal> GetRevenue(int clusterId)
        {
            var dates = GetDates();
            var result = new List<decimal>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, SUM(Price) FROM Currency WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToDecimal(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        public static List<decimal> GetCurrency(int clusterId)
        {
            var dates = GetDates();
            var result = new List<decimal>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, SUM(Price)/Sum(Income) FROM Currency WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToDecimal(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        public static List<int> GetStarts(int clusterId)
        {
            var dates = GetDates();
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, COUNT(Id) FROM Starts WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToInt32(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        public static List<int> GetEnds(int clusterId)
        {
            var dates = GetDates();
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, COUNT(Id) FROM Ends WHERE HasWon='False' AND UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToInt32(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        public static List<int> GetWins(int clusterId)
        {
            var dates = GetDates();
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, COUNT(Id) FROM Ends WHERE HasWon='True' AND UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToInt32(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        public static List<decimal> GetStageUSDIncome(int clusterId)
        {
            var dates = GetDates();
            var currency = GetCurrency(clusterId);
            var result = new List<decimal>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, SUM(Income) FROM Ends WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToDecimal(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i] *= currency[i];
            }
            return result;
        }

        public static List<int> GetItems(int clusterId)
        {
            var dates = GetDates();
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, COUNT(Id) FROM Items WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToInt32(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        public static List<int> GetIncome(int clusterId)
        {
            var dates = GetDates();
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, SUM(Price) FROM Items WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToInt32(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            return result;
        }

        public static List<decimal> GetItemsUSDIncome(int clusterId)
        {
            var dates = GetDates();
            var currency = GetCurrency(clusterId);
            var result = new List<decimal>();
            using (SqlConnection con = new SqlConnection(DataReader.ConnectionString))
            {
                con.Open();
                string sql = $"SELECT Date, SUM(Price) FROM Items WHERE UserId IN (SELECT Id FROM UserClusters WHERE ClusterId = {clusterId}) GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read())
                    {
                        if (dates[i] != reader.GetDateTime(0))
                        {
                            while (dates[i] != reader.GetDateTime(0))
                            {
                                result.Add(0);
                                i++;
                            }
                        }
                        result.Add(Convert.ToDecimal(reader[1]));
                        i++;
                    }
                    for (; i < 30; i++)
                    {
                        result.Add(0);
                    }
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i] *= currency[i];
            }
            return result;
        }
    }
}
