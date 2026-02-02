using System;

namespace GM_DAL.Models.CustomerVIP
{
    public class CustomerVIPModel
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }

        public int VipLevel { get; set; }
        public decimal DiscountPercent { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string MST { get; set; }
        
    }
}
