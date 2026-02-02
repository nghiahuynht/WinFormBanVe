using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.OrderModel
{
    public class BillItemsGridModel
    {
        public Int64 id { get; set; }
        public int productId { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public int quanti { get; set; }
        public decimal total { get; set; }
    }
}
