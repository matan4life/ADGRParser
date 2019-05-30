using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Data.DataView;
using System.Data.SqlClient;

namespace ADGRClusteriser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var mlContext = new MLContext(seed: 0);
            var players = DataReader.GetPlayerData();
            IDataView dataView = mlContext.Data.LoadFromEnumerable(players);
            string featuresName = "Features";
            var pipeline = mlContext.Transforms.Concatenate(featuresName, "Age", "Sex", "Country", "Profit", "Cheats")
                .Append(mlContext.Clustering.Trainers.KMeans(featuresName, null, 600));
            var model = pipeline.Fit(dataView);
            var predictor = mlContext.Model.CreatePredictionEngine<PlayerData, ClusteringResult>(model);
            using (var connection = new SqlConnection(DataReader.ConnectionString))
            {
                connection.Open();
                foreach (var player in players)
                {
                    var prediction = predictor.Predict(player);
                    string sql = $"INSERT INTO AgeClusters VALUES (N'{player.UserId}', {prediction.PredictedClusterId})";
                    var command = new SqlCommand(sql, connection);
                    await command.ExecuteNonQueryAsync();
                }
            }
            SingleClusterStatistics.Calculate();
        }
    }
}
