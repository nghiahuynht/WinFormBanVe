using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.User
{
    public class SearchUserFilterModel: DataTableDefaultParamModel
    {
        public string searchType { get; set; }
        public string roleCode { get; set; }
        public string status { get; set; }
        public string keyword { get; set; }
    }
}
