using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.TicketOrder
{
    public class PrintModel
    {
        public long SubId { get; set; }
        public string SubType { get; set; }
        public long OrderId { get; set; }
        public string TicketCode { get; set; }
        public string TicketName { get; set; }
        public string SubOrderCode { get; set; }
        public decimal Price { get; set; }
        public int Quanti { get; set; }
        public decimal TotalAfterVAT { get; set; }
        public string BillTemplate { get; set; }
        public string KyHieu { get; set; }
        public string TotalByText { get; set; }
        public DateTime? CreatedDate { get; set; }


        // Nghia
        public string CustomerTypeName { get; set; }
        public string ObjTypeName { get; set; }
        public string TicketGroup { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountedAmount { get; set; }
        public string PaymentValue { get; set; }
        public decimal PaymentRemain { get; set; }
        public string PaymentType { get; set; }
        public decimal? TotalAfterDiscounted { get; set; }

    }
}
