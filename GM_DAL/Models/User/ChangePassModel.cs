using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.User
{
    public class ChangePassModel
    {
        public string userName { get; set; }
        public string currentPass { get; set; }
        public string newPass { get; set; }
    }
}
