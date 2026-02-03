using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Ticket
{
    public class TicketModel
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public decimal? Price { get; set; }

        public string Description { get; set; }
        public string BillTemplate { get; set; }
        public string EContractTemplate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public int? BranchId { get; set; }
        public string LoaiIn { get; set; }
        public string GateName { get; set; }

        public bool? IsKhachNuocNgoai { get; set; }

        public string KyHieu { get; set; }
        public string TieuDeVe { get; set; }
        public string MauSoBienLai { get; set; }

        public int? Priority { get; set; }
        public string LoaiVe { get; set; }

        public string TicketGroup { get; set; }

        public decimal? VAT { get; set; }
    }
}
