using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Ticket
{
    public class TicketPricePolicyModel
    {
        public int Id { get; set; }
        public string TicketCode { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTypeName { get; set; }
        public string CustomerForm { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
