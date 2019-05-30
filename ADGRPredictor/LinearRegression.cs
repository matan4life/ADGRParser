using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADGRPredictor
{
    static class LinearRegression
    {
        public static (decimal, decimal) GetLinearCoeffs(Dictionary<int, decimal> dict)
        {
            var multiply = dict.Keys.ToList().Zip(dict.Values.ToList(), (x, y) => x * y).ToList();
            var square = dict.Keys.ToList().Select(x => x * x).ToList();
            decimal b = (dict.Count * multiply.Sum() - dict.Keys.ToList().Sum() * dict.Values.ToList().Sum()) / 
                (decimal)(dict.Count*square.Sum() - Math.Pow(dict.Keys.ToList().Sum(), 2));
            decimal a = (dict.Values.ToList().Sum() -  b * dict.Keys.ToList().Sum()) / (dict.Count);
            return (b, a);
        }

        public static Dictionary<int, decimal> GetdecimalPredictions<T>(T clusterId, string column, string tableName, string columnName)
        {
            var data = DataReader.GetData<decimal, T>(clusterId, column, tableName, columnName);
            var result = new Dictionary<int, decimal>();
            var coeffs = GetLinearCoeffs(data);
            for (int i=data.Keys.Max()+1; i<data.Keys.Max()+183; i++)
            {
                result.Add(i, coeffs.Item1 * i + coeffs.Item2);
            }
            return result;
        }

        public static Dictionary<int, int> GetIntPredictions<T>(T clusterId, string column, string tableName, string clusterName)
        {
            var data = DataReader.GetData<int, T>(clusterId, column, tableName, clusterName);
            var result = new Dictionary<int, int>();
            var coeffs = GetLinearCoeffs(DataReader.TransformData(data));
            for (int i = data.Keys.Max() + 1; i < data.Keys.Max() + 183; i++)
            {
                result.Add(i, (int)Math.Round(coeffs.Item1 * i + coeffs.Item2));
            }
            return result;
        }
    }
}
