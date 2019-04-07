using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using static System.Console;

namespace ADGRParser
{
    class Program
    {
        public static string ConnectionString { get; set; } = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        static void Main(string[] args)
        {
            LoadStatistics();
        }

        static void LoadStatistics()
        {
            var dates = GetDates();
            var dau = GetDau();
            var newUsers = NewUsers();
            var mau = GetMau();
            var revenue = GetRevenue();
            var currency = GetCurrency();
            var starts = GetStarts();
            var ends = GetEnds();
            var wins = GetWins();
            var usdincome = GetStageUSDIncome();
            var items = GetItems();
            var itemsincome = GetIncome();
            var usditemsincome = GetItemsUSDIncome();
            WriteLine("Start uploading statistics...");
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                int counter = 0;
                foreach (var date in dates)
                {
                    WriteLine($"Sending info about {date.ToShortDateString()}");
                    string sql = "INSERT INTO [Statistics] VALUES(@dat, @dau, @new, @mau, @rev, @cur, @sta, @end, @win, @sti, @ite, @iti, @ius)";
                    var sqlcommand = new SqlCommand(sql, connection);
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
                    WriteLine($"Progress: {(int)((double)counter / dates.Count * 100)}%");
                }
            }
            WriteLine("Statistics has been successfully uploaded");
        }

        static List<DateTime> GetDates()
        {
            var result = new List<DateTime>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
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

        static List<int> GetDau()
        {
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, COUNT(DISTINCT UserId) FROM Logins GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader[1]));
                    }
                }
            }
            return result;
        }

        static List<int> NewUsers()
        {
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, COUNT(Id) FROM Users GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader[1]));
                    }
                }
            }
            return result;
        }

        static Dictionary<DateTime, int> GetMau()
        {
            var result = new Dictionary<DateTime, int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT YEAR(Date), MONTH(Date), COUNT(DISTINCT UserId) FROM Logins GROUP BY YEAR(Date), MONTH(Date) ORDER BY YEAR(Date), MONTH(Date)";
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

        static List<decimal> GetRevenue()
        {
            var result = new List<decimal>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, SUM(Price) FROM Currency GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]} {reader[1]}");
                        result.Add(Convert.ToDecimal(reader[1]));
                    }
                }
            }
            return result;
        }

        static List<decimal> GetCurrency()
        {
            var result = new List<decimal>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, SUM(Price)/Sum(Income) FROM Currency GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]} {reader[1]}");
                        result.Add(Convert.ToDecimal(reader[1]));
                    }
                }
            }
            return result;
        }

        static List<int> GetStarts()
        {
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, COUNT(Id) FROM Starts GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]} {reader[1]}");
                        result.Add(Convert.ToInt32(reader[1]));
                    }
                }
            }
            return result;
        }

        static List<int> GetEnds()
        {
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, COUNT(Id) FROM Ends WHERE HasWon='False' GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]} {reader[1]}");
                        result.Add(Convert.ToInt32(reader[1]));
                    }
                }
            }
            return result;
        }

        static List<int> GetWins()
        {
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, COUNT(Id) FROM Ends WHERE (HasWon='True') GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]} {reader[1]}");
                        result.Add(Convert.ToInt32(reader[1]));
                    }
                }
            }
            return result;
        }

        static List<decimal> GetStageUSDIncome()
        {
            var currency = GetCurrency();
            var result = new List<decimal>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, SUM(Income) FROM Ends GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]} {reader[1]}");
                        result.Add(Convert.ToDecimal(reader[1]));
                    }
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i] *= currency[i];
            }
            return result;
        }

        static List<int> GetItems()
        {
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, COUNT(Id) FROM Items GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]} {reader[1]}");
                        result.Add(Convert.ToInt32(reader[1]));
                    }
                }
            }
            return result;
        }

        static List<int> GetIncome()
        {
            var result = new List<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, SUM(Price) FROM Items GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]} {reader[1]}");
                        result.Add(Convert.ToInt32(reader[1]));
                    }
                }
            }
            return result;
        }

        static List<decimal> GetItemsUSDIncome()
        {
            var currency = GetCurrency();
            var result = new List<decimal>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sql = "SELECT Date, SUM(Price) FROM Items GROUP BY Date ORDER BY Date";
                SqlCommand command = new SqlCommand(sql, con);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //WriteLine($"{reader[0]} {reader[1]}");
                        result.Add(Convert.ToDecimal(reader[1]));
                    }
                }
            }
            for (int i=0; i<result.Count; i++)
            {
                result[i] *= currency[i];
            }
            return result;
        }

        static void ETL()
        {
            var timer1 = Stopwatch.StartNew();
            WriteLine("Extracting data...");
            string DirectoryPath = @"D:/OIDT";
            WriteLine($"There are {Directory.GetFiles(DirectoryPath).Count()} json files");
            foreach (var path in Directory.GetFiles(DirectoryPath))
            {
                var timer2 = Stopwatch.StartNew();
                WriteLine($"Processing {path}");
                dynamic obj = JsonConvert.DeserializeObject(File.ReadAllText(path));
                WriteLine("JSON data has been extracted!");
                int counter = 0;
                foreach (var item in obj)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        string sql;
                        SqlCommand command = new SqlCommand();
                        switch (item.event_id.Value)
                        {
                            case 1L:
                                sql = "INSERT INTO [Logins] VALUES(@id, @date)";
                                command = new SqlCommand(sql, connection);
                                command.Parameters.AddWithValue("@id", item.udid.Value);
                                command.Parameters.AddWithValue("@date", item.date.Value);
                                break;
                            case 2L:
                                sql = "INSERT INTO [Users] VALUES(@id, @c, @d, @g, @a)";
                                command = new SqlCommand(sql, connection);
                                command.Parameters.AddWithValue("@id", item.udid.Value);
                                command.Parameters.AddWithValue("@c", item.parameters.country.Value);
                                command.Parameters.AddWithValue("@d", item.date.Value);
                                command.Parameters.AddWithValue("@g", item.parameters.gender.Value);
                                command.Parameters.AddWithValue("@a", item.parameters.age.Value);
                                break;
                            case 3L:
                                sql = "INSERT INTO [Starts] VALUES(@id, @d, @l)";
                                command = new SqlCommand(sql, connection);
                                command.Parameters.AddWithValue("@id", item.udid.Value);
                                command.Parameters.AddWithValue("@d", item.date.Value);
                                command.Parameters.AddWithValue("@l", item.parameters.stage.Value);
                                break;
                            case 4L:
                                sql = "INSERT INTO [Ends] VALUES(@id, @d, @l, @w, @t, @i)";
                                command = new SqlCommand(sql, connection);
                                command.Parameters.AddWithValue("@id", item.udid.Value);
                                command.Parameters.AddWithValue("@d", item.date.Value);
                                command.Parameters.AddWithValue("@l", item.parameters.stage.Value);
                                command.Parameters.AddWithValue("@w", item.parameters.win.Value);
                                command.Parameters.AddWithValue("@t", item.parameters.time.Value);
                                command.Parameters.AddWithValue("@i", item.parameters.income.Value);
                                break;
                            case 5L:
                                sql = "INSERT INTO [Items] VALUES(@id, @d, @i, @p)";
                                command = new SqlCommand(sql, connection);
                                command.Parameters.AddWithValue("@id", item.udid.Value);
                                command.Parameters.AddWithValue("@d", item.date.Value);
                                command.Parameters.AddWithValue("@i", item.parameters.item.Value);
                                command.Parameters.AddWithValue("@p", item.parameters.price.Value);
                                break;
                            case 6L:
                                sql = "INSERT INTO [Currency] VALUES(@id, @d, @c, @p, @i)";
                                command = new SqlCommand(sql, connection);
                                command.Parameters.AddWithValue("@id", item.udid.Value);
                                command.Parameters.AddWithValue("@d", item.date.Value);
                                command.Parameters.AddWithValue("@c", item.parameters.name.Value);
                                command.Parameters.AddWithValue("@p", item.parameters.price.Value);
                                command.Parameters.AddWithValue("@i", item.parameters.income.Value);
                                break;
                        }
                        command.ExecuteNonQuery();
                    }
                    counter++;
                    double percent = (int)((double)counter / obj.Count * 100);
                    WriteLine($"Processed {percent}%");
                }
                timer2.Stop();
                WriteLine($"Processing of {path} has taken {timer2.ElapsedMilliseconds} milliseconds.");
            }
            timer1.Stop();
            WriteLine($"ETL process has taken {timer1.ElapsedMilliseconds} milliseconds.");
        }
    }
}
