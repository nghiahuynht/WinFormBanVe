using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Customer
{
    public class CustomerModel
    {
        public int Id { get; set; }

        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }

        public string TaxCode { get; set; }

        public bool? IsDeleted { get; set; }
        public int? Priority { get; set; }

        public string CustomerType { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int SaleChannelId { get; set; }

        public string AgencyName { get; set; }
        public string TaxAddress { get; set; }
    }
}
