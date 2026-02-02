using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Room
{
    public class BillOrderItemModel
    {
        public Int64 id { get; set; }
        public Int64 billId { get; set; }
        public int productId { get; set; }
        public int quanti { get; set; }
        public decimal price { get; set; }
        public decimal total { get; set; }
    }
}
