using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Graphs.Models
{
    public class SalesOrderDetail
    {
        public int Sr { get; set; }
        public string OrderTrackNumber { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string SpecialOffer { get; set; }
        public double UnitPrice { get; set; }
        public double UnitPriceDiscount { get; set; }
    }
}