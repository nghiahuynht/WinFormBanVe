using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Booked
{
    public class BookedFilterModel: DataTableDefaultParamModel
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string keyword {  get; set;  }

        public string tim { get; set; }
    }
}
