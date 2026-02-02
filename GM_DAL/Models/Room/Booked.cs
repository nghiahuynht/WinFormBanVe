using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Room
{
    public class Booked
    {
        public Int64 id { get; set; }
        public string customerName { get; set; }
        public string phone { get; set; }
        public DateTime? bookTime { get; set; }
        public string note { get; set; }
    }
}
