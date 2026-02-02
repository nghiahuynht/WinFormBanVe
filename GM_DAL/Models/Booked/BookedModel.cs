using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Booked
{
    public class BookedModel
    {
        public Int64 id { get; set; }
        public string bookName { get; set; }
        public string bookPhone { get; set; }
        public DateTime bookDate { get; set; }
        public string note {  get; set; } 
        public string createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public string updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
        public bool isDeleted { get; set; }
        public string xuly { get; set; }
        public string xulyText { get; set; }
    }
}
