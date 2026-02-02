using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Room
{
    public class BillOrderModel
    {
        public Int64 id { get; set; }
        public int roomId { get; set; }
        public string roomCode { get; set; }
        public string roomName { get; set; }
        public DateTime? checkin { get; set; }
        public DateTime? checkout { get; set; }
        public string status { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string updatedBy { get; set; }
        public string updatedDate { get; set; }
        public string cus_phone { get; set; }
        public string cus_name { get; set; }
        public decimal? cus_total_before_discount { get; set; }
        public decimal? cus_discount_percent { get; set; }
        public decimal? cus_amount_discount { get; set; }
        public decimal? cus_totalmoney_aftercus_discount { get; set; }
        public string note { get; set; }
    }
}
