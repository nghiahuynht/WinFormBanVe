using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.TicketOrder
{
    public class TicketOrderHeaderModel
    {
        public long Id { get; set; }
        public string TicketCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public decimal Price { get; set; }
        public int Quanti { get; set; }
        public decimal Total { get; set; }
        public decimal DiscountedAmount {  get; set; }
        public decimal TotalAfterDiscounted { get; set; }
        public decimal DiscountPercent { get; set; }
        public string PrintType { get; set; }
    }
}
