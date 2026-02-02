using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models
{
    public class MenuModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parent { get; set; }
        public string? menuIcon { get; set; }
        public string url { get; set; }
        public int priority { get; set; }
        public bool isActive { get; set; }
    }
}
