using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models
{
    public class ModelCommonField
    {
        public string? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string? updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
