using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.NhapKhoModel
{
    public class NhapKhoHeaderModel
    {
        public Int64 id { get; set; }
        public string fullName { get; set; }
        public DateTime? actionDate { get; set; }
        public DateTime? createdDate { get; set; }
        public string createdBy { get; set; }
        public string note { get; set; }
        public int? totalQuanti { get; set; }
        public decimal? totalAmount { get; set; }

    }
}
