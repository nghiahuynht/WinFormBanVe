using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_DAL.Models.Report
{
    public class ReportProductThongKeModel
    {
        public string TenSanPham { get; set; }
        public string unit { get; set; }
        public int SoLuongDaBan { get; set; }
        public decimal TongTien { get; set; }

        public decimal GiaBanTrungBinh { get; set; }

        public int SoLuongNhap { get; set; }

        public decimal TongTienNhap { get; set; }

        public decimal GiaNhapTrungBinh { get; set; }
        public decimal LoiNhuanTrenNhap { get; set; }
        public decimal LoiNhuanTrenBan { get; set; }
    }
}