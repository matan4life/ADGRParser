using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADGRPredictor
{
    class Program
    {
        static void Main(string[] args)
        {
            //AgeRegression();
            //CheatRegression();
            CountryRegression();
            SexRegression();
            ProfitRegression();
        }

        static void AgeRegression()
        {
            for (int i = 1; i < 7; i++)
            {
                var DAUTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(i, "DAU", "AgeClusterStatistics", "AgeClusterId"));
                var NewUsersTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(i, "NewUsers", "AgeClusterStatistics", "AgeClusterId"));
                var RevenueTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(i, "Revenue", "AgeClusterStatistics", "AgeClusterId"));
                var ItemsTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(i, "Items", "AgeClusterStatistics", "AgeClusterId"));
                var ItemsUSDIncomeTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(i, "ItemsUSDIncome", "AgeClusterStatistics", "AgeClusterId"));
                DataReader.UploadInfo(i, "AgeClusterPrediction", DAUTuple.Item1, DAUTuple.Item2, NewUsersTuple.Item2, RevenueTuple.Item2, ItemsTuple.Item2, ItemsUSDIncomeTuple.Item2);
            }
        }

        static void ProfitRegression()
        {
            for (int i = 1; i < 5; i++)
            {
                var DAUTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(i, "DAU", "ProfitClusterStatistics", "ProfitClusterId"));
                var NewUsersTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(i, "NewUsers", "ProfitClusterStatistics", "ProfitClusterId"));
                var RevenueTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(i, "Revenue", "ProfitClusterStatistics", "ProfitClusterId"));
                var ItemsTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(i, "Items", "ProfitClusterStatistics", "ProfitClusterId"));
                var ItemsUSDIncomeTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(i, "ItemsUSDIncome", "ProfitClusterStatistics", "ProfitClusterId"));
                DataReader.UploadInfo(i, "ProfitClusterPredictions", DAUTuple.Item1, DAUTuple.Item2, NewUsersTuple.Item2, RevenueTuple.Item2, ItemsTuple.Item2, ItemsUSDIncomeTuple.Item2);
            }
        }

        static void SexRegression()
        {
            List<string> genders = new List<string>() { "male", "female" };
            foreach (var gender in genders)
            {
                var DAUTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(gender, "DAU", "SexClusterStatistics", "Sex"));
                var NewUsersTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(gender, "NewUsers", "SexClusterStatistics", "Sex"));
                var RevenueTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(gender, "Revenue", "SexClusterStatistics", "Sex"));
                var ItemsTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(gender, "Items", "SexClusterStatistics", "Sex"));
                var ItemsUSDIncomeTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(gender, "ItemsUSDIncome", "SexClusterStatistics", "Sex"));
                DataReader.UploadInfo(gender, "SexClusterPredictions", DAUTuple.Item1, DAUTuple.Item2, NewUsersTuple.Item2, RevenueTuple.Item2, ItemsTuple.Item2, ItemsUSDIncomeTuple.Item2);
            }
        }

        static void CountryRegression()
        {
            List<string> countries = new List<string>();
            using (var connection = new SqlConnection(DataReader.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT DISTINCT Country FROM Users";
                var command = new SqlCommand(sql, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countries.Add(reader.GetString(0));
                    }
                }
            }
            foreach (var country in countries)
            {
                var DAUTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(country, "DAU", "CountryClusterStatistics", "Country"));
                var NewUsersTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(country, "NewUsers", "CountryClusterStatistics", "Country"));
                var RevenueTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(country, "Revenue", "CountryClusterStatistics", "Country"));
                var ItemsTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(country, "Items", "CountryClusterStatistics", "Country"));
                var ItemsUSDIncomeTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(country, "ItemsUSDIncome", "CountryClusterStatistics", "Country"));
                DataReader.UploadInfo(country, "CountryClusterPredictions", DAUTuple.Item1, DAUTuple.Item2, NewUsersTuple.Item2, RevenueTuple.Item2, ItemsTuple.Item2, ItemsUSDIncomeTuple.Item2);
            }
        }

        static void CheatRegression()
        {
            List<bool> cheats = new List<bool>() { true, false };
            foreach (var cheat in cheats)
            {
                var DAUTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(cheat, "DAU", "CheatsClusterStatistics", "Cheat"));
                var NewUsersTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(cheat, "NewUsers", "CheatsClusterStatistics", "Cheat"));
                var RevenueTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(cheat, "Revenue", "CheatsClusterStatistics", "Cheat"));
                var ItemsTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions(cheat, "Items", "CheatsClusterStatistics", "Cheat"));
                var ItemsUSDIncomeTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions(cheat, "ItemsUSDIncome", "CheatsClusterStatistics", "Cheat"));
                DataReader.UploadInfo(cheat, "CheatsClusterPredictions", DAUTuple.Item1, DAUTuple.Item2, NewUsersTuple.Item2, RevenueTuple.Item2, ItemsTuple.Item2, ItemsUSDIncomeTuple.Item2);

            }
        }
    }
}
