using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.ItemsModel
{
    public class ProductFilterModel: DataTableDefaultParamModel
    {
        public string category { get; set; }
        public string keyword { get; set; }
        public string trangThaiTon { get; set; }
    }
}
