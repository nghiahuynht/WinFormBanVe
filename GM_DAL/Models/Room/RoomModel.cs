using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Room
{
    public class RoomModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public string area { get; set; }

        public string status { get; set; }
        public DateTime? checkin { get; set; }
        public DateTime? checkout { get; set; }
        public decimal? price { get; set; } // Thêm thuộc tính Price
        public decimal totalOrder { get; set; } // danh cho phong co khach thi hien luoon total o hompage
        public long billId { get; set; }
    }
}
