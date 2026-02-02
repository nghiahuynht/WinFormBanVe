using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.NhapKhoModel
{
    public class NhapKhoItemsModel
    {
        public Int64 id { get; set; }
        public Int64 NhapKhoId { get; set; }
        public int productId { get; set; }
        public string unit { get; set; }
        public string productName { get; set; }
        public int quanti { get; set; }
        public decimal price { get; set; }
        public decimal total { get; set; }
        public bool isDelete { get; set; }
    }
}
