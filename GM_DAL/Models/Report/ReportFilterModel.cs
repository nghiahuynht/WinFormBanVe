using System;

namespace GM_DAL.Models.Report
{
    public class ReportFilterModel : DataTableDefaultParamModel
    {
        public int roomId { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

        // ✅ thêm 2 field để search khách
        public string cusName { get; set; }   // search theo BillOrder.cus_name
        public string cusPhone { get; set; }  // search theo BillOrder.cus_phone
        public string status { get; set; }
    }
}