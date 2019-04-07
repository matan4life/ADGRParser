using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADGRPredictor
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<int> a = new List<int>() { 1, 2, 3 };
            //List<int> b = new List<int>() { 4, 5, 6 };
            //var c = a.Zip(b, (x, y) => x * y).ToList();
            //c.ForEach(x => Console.WriteLine(x));
            var DAUTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions("DAU"));
            var NewUsersTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions("NewUsers"));
            var RevenueTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions("Revenue"));
            var ItemsTuple = DataReader.DivideIntData(LinearRegression.GetIntPredictions("Items"));
            var ItemsUSDIncomeTuple = DataReader.DividedecimalData(LinearRegression.GetdecimalPredictions("ItemsUSDIncome"));
            DataReader.UploadInfo(DAUTuple.Item1, DAUTuple.Item2, NewUsersTuple.Item2, RevenueTuple.Item2, ItemsTuple.Item2, ItemsUSDIncomeTuple.Item2);
        }
    }
}
