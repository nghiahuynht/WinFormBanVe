using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Room
{
    public class RoomFilterModel: DataTableDefaultParamModel
    {
        public string status { get; set; }
        public string keyword { get; set; }
    }
}
