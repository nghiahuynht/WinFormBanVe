using DinkToPdf;
using DinkToPdf.Contracts;
using GM_DAL.Models.TicketOrder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp
{
    public partial class FormTest : Form
    {
        string billPdfExportPath = ConfigurationManager.AppSettings["BillExportPath"];
        private static readonly IConverter _converter = new SynchronizedConverter(new PdfTools());
        public FormTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string htmlBill = generateHTMLBill(new TicketOrderHeaderModel(),new List<PrintModel>());
            if (!Directory.Exists(billPdfExportPath))
            {
                Directory.CreateDirectory(billPdfExportPath);
            }



            string outputPath = Path.Combine(billPdfExportPath,"1.pdf");
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            bool resPdf = ConvertHtmlToPdf_DinkToPdf(htmlBill, outputPath);
            if (resPdf == true)
            {
                PrintReview formBillReview = new PrintReview(1);
                formBillReview.Show();
            }



        }


        public string generateHTMLBill(TicketOrderHeaderModel header, List<PrintModel> items)
        {

            decimal total = header.Total;
            string itemHtml = "<table style='width:430px;border-collapse:collapse;font-size:20pt;' border='1'  >";

            string phanTramKM = header.DiscountPercent.ToString()+"%";
            string tienKM = header.DiscountedAmount.ToString();
            if (phanTramKM != "0")
            {
                tienKM = tienKM + " (" + phanTramKM + "%)";
            }
            string tienSauKM = header.TotalAfterDiscounted.ToString(); ;



           
            itemHtml += "<tr><td colspan='2'><strong>Tổng tiền:</h5></strong><td style='text-align:right;'><strong>" + total.ToString("N0") + "</strong></td></tr>";
            itemHtml += "<tr><td colspan='2'><strong>Khuyến mãi:</h5></strong><td style='text-align:right;'><strong>" + tienKM + "</strong></td></tr>";
            itemHtml += "<tr><td colspan='2'><strong>Tổng cần thanh toán:</strong></td><td style='text-align:right;'><strong>" + tienSauKM + "</strong></td></tr>";
            itemHtml += "</table>";
            itemHtml += "<br/><span style='text-align:center;font-size:20pt;'>(Giá đã bao gồm VAT)</span><br/>";
    
            string simpleHtml = @"<html>" +
                "<body style='margin:0;padding:0;'>" +
                    "<table style='width:430px'>"+
                        "<tr>"+
                            "<td></td>"+
                        "</tr>"+
                    "</table>" +
                    "<div style='text-align:center'><h2 style='font-size:20pt;'>LANGBIANG LAND</h2></div>" +
                    "<div style='text-align:center'><span style='font-size:20pt;'>SĐT: 0987.262.919</h5></div>" +
                    "<div style='text-align:center'><span style='font-size:20pt;'>ĐC: đường Trần Rịa, thôn Long Bình, xã Tuy An Bắc, tỉnh Đắk Lắk</span></div><br/>" +
                    "<div style='text-align:center'><span style='font-size:20pt;'>Ngày: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "</span></div><br/>" +
                    "<table style='width:430px'>" +
                        "<tr style='font-size:20pt;'><td width='100px'>Phòng:</td><td>LANBIANG LAND</td></tr>" +
                     "</table><br/>" + itemHtml + "<hr/>" +
                     "<div style='text-align:center;margin-top:3px'><strong style='font-size:20pt;'>Cám ơn quý khách đã ủng hộ.</strong></div>" +
                     "<div style='text-align:center;font-size:15pt;margin-top:3px;'>Hỗ trợ kỹ thuật: 0823.885.086 - HT Computer</div>" +
                "</body>" +
            "</html>";

            string test = simpleHtml;

            return simpleHtml;
        }



        public bool ConvertHtmlToPdf_DinkToPdf(string htmlContent, string outputFilePath)
        {

            bool resResult = false;
            // 1. Cấu hình Global (tùy chọn PDF chung)
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = new PechkinPaperSize("80mm", "400mm"),
                Margins = new MarginSettings { Top = 0.1, Left = 0.1, Right = 0.1, Bottom = 0.1 },
                DocumentTitle = "LANGBIANLAND",

            };

            // 2. Cấu hình Object (nội dung)
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf8" },


                // Thêm Header/Footer nếu cần
                // HeaderSettings = { FontSize = 9, Right = "Trang [page] / [toPage]", Line = true }
            };

            // 3. Tạo Document
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            // 4. Chuyển đổi và lưu file
            try
            {
                byte[] pdfBytes = _converter.Convert(pdf);
                File.WriteAllBytes(outputFilePath, pdfBytes);
                resResult = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}\nKiểm tra lại việc thiết lập PATH và quyền truy cập.");
            }
            return resResult;
        }


    }
}
