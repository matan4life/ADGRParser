using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Console;

namespace ADGRCheatAnalyzer
{
    class Program
    {
        public static string ConnectionString { get; set; } = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ADGR;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public static string Path { get; set; } = @"D:\OIDT";

        static void Main(string[] args)
        {
            AnalyzeCheats();
        }

        public static void AnalyzeCheats()
        {
            List<string> Cheaters = new List<string>();
            Dictionary<string, long> Players = new Dictionary<string, long>();
            Stopwatch generalTimer = Stopwatch.StartNew();
            WriteLine("Start of analysis");
            foreach (var file in Directory.GetFiles(Path))
            {
                WriteLine($"Processing file {file}");
                Stopwatch inner = Stopwatch.StartNew();
                dynamic data = JsonConvert.DeserializeObject(File.ReadAllText(file));
                int i = 0;
                foreach (var item in data)
                {
                    if (!Cheaters.Contains(item.udid.Value))
                    {
                        switch (item.event_id.Value)
                        {
                            case 2L:
                                Players.Add(item.udid.Value, 0);
                                break;
                            case 4L:
                                Players[item.udid.Value] += item.parameters.income.Value;
                                break;
                            case 5L:
                                if (Players[item.udid.Value] < item.parameters.price.Value)
                                {
                                    Cheaters.Add(item.udid.Value);
                                    Players.Remove(item.udid.Value);
                                    break;
                                }
                                else
                                {
                                    Players[item.udid.Value] -= item.parameters.price.Value;
                                    break;
                                }
                            case 6L:
                                Players[item.udid.Value] += item.parameters.income.Value;
                                break;
                        }
                    }
                    i++;
                    WriteLine($"Processed {i * 100.0 / data.Count}% of records");
                }
                inner.Stop();
                WriteLine($"Analysis of the {file} has taken {inner.ElapsedMilliseconds} ms");
            }
            generalTimer.Stop();
            WriteLine($"The analysis has successfully ended with the time metrics {generalTimer.ElapsedMilliseconds} ms");
            WriteLine("Start recording to thew database");
            int j = 0;
            WriteLine($"Processed {j * 100.0 / (Cheaters.Count + Players.Keys.Count)}% of records");
            foreach (var cheater in Cheaters)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string sql = $"INSERT INTO Cheats VALUES(N'{cheater}', 'true')";
                    var command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    j++;
                    WriteLine($"Processed {j * 100.0 / (Cheaters.Count + Players.Keys.Count)}% of records");
                }
            }

            foreach (var honest in Players.Keys.ToList())
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string sql = $"INSERT INTO Cheats VALUES(N'{honest}', 'false')";
                    var command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    j++;
                    WriteLine($"Processed {j * 100.0 / (Cheaters.Count + Players.Keys.Count)}% of records");
                }
            }
        }
    }
}
