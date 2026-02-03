using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.TicketOrder
{
    public class TicketOrderModel
    {
        public long Id { get; set; }

        public string TicketCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }

        public decimal? Price { get; set; }
        public int? Quanti { get; set; }
        public decimal? Total { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public string MaHDDT { get; set; }
        public string CustomerType { get; set; }
        public string BienSoXe { get; set; }

        public int SaleChannelId { get; set; }

        public int? PaymentStatus { get; set; }
        public string PaymentType { get; set; }
        public long? PaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentValue { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public decimal PaymentFee { get; set; }

        public DateTime? VisitDate { get; set; }
        public bool? IsCopy { get; set; }

        public string GateName { get; set; }
        public string ObjType { get; set; }
        public string PartnerCode { get; set; }

        public decimal? DiscountedAmount { get; set; }
        public decimal? TotalAfterDiscount { get; set; }

        public bool? IsFree { get; set; }
        public string OrderStatus { get; set; }

        public decimal? DiscountPercent { get; set; }
        public decimal? PaymentRemain { get; set; }

        public string PrintType { get; set; }

        public string InvNum { get; set; }
        public string TranId { get; set; }
        public string InvSeri { get; set; }

        public Guid? UUID { get; set; }
    }
}
