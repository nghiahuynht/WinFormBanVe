using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.ItemsModel
{
    public class ProductItemModel
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public decimal? price { get; set; }
        public string categoryCode { get; set; }
        public string imgThunail { get; set; }
        public bool isDeleted { get; set; }

        public int tonkho { get; set; }
        public string warning { get; set; }
        public decimal? dongianhap { get; set; }
    }
}
