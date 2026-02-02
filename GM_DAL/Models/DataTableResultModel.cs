using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models
{
    public class DataTableResultModel<T>
    {
        public DataTableResultModel()
        {
            this.data = new List<T>();
            this.recordsTotal = 0;
        }
        public int recordsTotal { get; set; }
        public List<T> data { get; set; }
    }
}
