using DinkToPdf;
using DinkToPdf.Contracts;
using GM_DAL.IServices;
using GM_DAL.Models.TicketOrder;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Web.WebView2.Core;
using QRCoder;
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

        private ITicketOrderService ticketOrderService;
        

        string billPdfExportPath = ConfigurationManager.AppSettings["BillExportPath"];
        string imgsFolder = ConfigurationManager.AppSettings["ImageLibaryPath"];
        private static readonly IConverter _converter = new SynchronizedConverter(new PdfTools());
        public FormTest(ITicketOrderService ticketOrderService)
        {
            InitializeComponent();
            this.ticketOrderService = ticketOrderService;

            if (!Directory.Exists(billPdfExportPath))
            {
                Directory.CreateDirectory(billPdfExportPath);
            }


        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Int64 giaLapOrderId = 174429;
            var headerOrder = await ticketOrderService.GetHeaderOrderById(giaLapOrderId);
            var lstItems = new List<PrintModel>();
            if (headerOrder.data.Id > 0)
            {
                lstItems = ticketOrderService.ListSubCodeForPrint(headerOrder.data.Id).data;
            }



            // 1. Khởi tạo tài liệu tổng
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = DinkToPdf.Orientation.Portrait,
                    PaperSize = new PechkinPaperSize("80mm", "125mm"),
                    Margins = new MarginSettings { Top = 0.3, Left = 0.3, Right = 0.3, Bottom = 0.3 },
                }
            };





            if (headerOrder.data.PrintType == "InGop")
            {
                long subIdFirst = 0;
                string subCodeFirst = "";
                if (lstItems.Any())
                {
                    var subFirst = lstItems.FirstOrDefault();
                    subIdFirst = subFirst.SubId;
                    subCodeFirst = subFirst.SubOrderCode;
                }
                string htmlBill = generateHTMLBill(headerOrder.data, subIdFirst, subCodeFirst);
                var page = new ObjectSettings()
                {
                    PagesCount = true,
                    HtmlContent = htmlBill, // Hàm trả về chuỗi HTML của 1 vé
                    WebSettings = { DefaultEncoding = "utf-8" }
                };
                doc.GlobalSettings.PaperSize.Height = "150mm";
                doc.Objects.Add(page); // Thêm vé này vào danh sách đối tượng của tài liệu
            }
            else // in lẻ
            {
                foreach (var subITem in lstItems)
                {
                    string htmlBill = "";
                    if (subITem.SubType == "Sub")
                    {
                        htmlBill = generateHTMLBill(headerOrder.data, subITem.SubId, subITem.SubOrderCode);
                        
                    }
                    else if (subITem.SubType == "SubChild")
                    {
                        htmlBill = generateHTMLSubBill(headerOrder.data.Id, subITem.SubId, subITem.SubOrderCode);
                       
                    }
                  
                    var page = new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = htmlBill, // Hàm trả về chuỗi HTML của 1 vé
                        WebSettings = {
                            DefaultEncoding = "utf-8",
                            UserStyleSheet = null, // Đảm bảo không bị ảnh hưởng bởi file CSS bên ngoài
                            LoadImages = true      // Bắt buộc phải True để nó nạp được ảnh Base64
                        },


                    };
                    
                    doc.Objects.Add(page); // Thêm vé này vào danh sách đối tượng của tài liệu

                }
            }

            string savePath = Path.Combine(billPdfExportPath, giaLapOrderId + ".pdf");
            await Task.Run(async () => {
                byte[] pdf = _converter.Convert(doc);
                File.WriteAllBytes(savePath, pdf);

                if (File.Exists(savePath))
                {
                    await PrintSilentWebView2(savePath);
                }

            });







        }




       



        public string generateHTMLBill(TicketOrderHeaderModel header,long subId,string subCode)
        {

            string folderPath = imgsFolder.Replace('\\', '/');
            string fullImagePathLogo = $"file:///{folderPath}/logo-langbian-land.jpg";

            string itemHtml = "";

            if (header.DiscountPercent > 0)
            {
                decimal total = header.Total;
                itemHtml = "<table style='width:430px;border-collapse:collapse;' border='1'  >";
                string phanTramKM = header.DiscountPercent.ToString() + "%";
                string tienKM = header.DiscountedAmount.ToString();
                if (phanTramKM != "0")
                {
                    tienKM = tienKM + " (" + phanTramKM + "%)";
                }
                string tienSauKM = header.TotalAfterDiscounted.ToString();
                itemHtml += "<tr><td colspan='2'><strong>Tổng tiền:</strong><td style='text-align:right;'><strong>" + total.ToString("N0") + "</strong></td></tr>";
                itemHtml += "<tr><td colspan='2'><strong>Khuyến mãi:</strong><td style='text-align:right;'><strong>" + tienKM + "</strong></td></tr>";
                itemHtml += "<tr><td colspan='2'><strong>Tổng cần thanh toán:</strong></td><td style='text-align:right;'><strong>" + tienSauKM + "</strong></td></tr>";
                itemHtml += "</table>";
            }

            Bitmap qrCode = CreateQRCode(subId.ToString());
            string qrCodeByte64 = BitmapToBase64(qrCode);
            string simpleHtml = @"<html>" +
                "<body style='margin:0;padding:0;font-size:16pt;'>" +
                    "<table style='width:430px;border-bottom:1px solid #000;margin-top:10px;'>" +
                        "<tr>" +
                            "<td width='50px' style='text-align:center;font-weight:bold;padding:5px;'><img src='" + fullImagePathLogo + "' style='width:100%;'></td>" +
                             "<td width='150px' style='text-align:left;font-weight:bold'>MST: 5801503332 <br/> Hotline: 0923519519<br/>93A Bidoup, phường Langbiang - Đà Lạt</td>" +
                        "</tr>" +
                    "</table>" +
                    "<div style='text-align:center;margin-top:10px;'><span>Loại vé: "+header.TicketCode+"</span></div>" +
                    "<table style='width:430px;text-align:center;font-size:16pt;'>" +
                         "<tr>" +
                            "<th>Đơn Giá</th>" +
                            "<th>Số lượng</th>" +
                            "<th>Thành tiền</th>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>140,000</td>" +
                            "<td>2</td>" +
                            "<td>280,000</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td colspan='3'>(Hai trăm tám mươi nghìn đồng)</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td colspan='3'>Ngày: 10-02-2026</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td colspan='3'><strong>(Vé chỉ có giá trị sử dụng trong ngày)</strong></td>" +
                        "</tr>" +
                     "</table><br/>" + itemHtml + "<hr/>" +
                      "<table style='width:430px;font-size:16pt;'>" +
                           "<tr>" +
                                "<td><img src='data:image/png;base64," + qrCodeByte64 + "' style='width:35mm;border:1px solid #000;padding:1px;margin:1px;' /></td>" +
                                "<td font-size:16pt;>" +
                                        "<strong>Mã đơn:</strong> " + header.Id+"<br/>"+
                                        "<strong>Số vé:</strong> " + subId + "<br/>" +
                                        "<strong>Mã tra cứu:</strong> " + subCode + "<br/>" +
                                        "<strong>Link:</strong> bit.ly/langbiang" +
                                "</td>" +
                           "</tr>" +
                      "</table>" +
                     "<table style='text-align:center;margin-top:3px;font-size:16pt;'>" +
                           "<tr>" +
                                "<td>Vé đã mua có thể đổi ngày tham quan, nhưng không thể hoàn trả - chính sách đổi ngày vui lòng thông báo trước 24h.<br><i>The purchased ticket date can be change, but it is non refunable. Please notify us before at lease 24h if you want to change the visit date. </i></td>" +
                           "</tr>" +
                      "</table>" +
                     "<div style='text-align:center;margin-top:3px;font-size:16pt;'>KÍNH CHÚC QUÝ KHÁCH VUI CHƠI VUI VẺ.</div>" +
                "</body>" +
            "</html>";

  
            return simpleHtml;
        }



        public string generateHTMLSubBill (long orderid,long subId, string subCode)
        {
            Bitmap qrCode = CreateQRCode(subId.ToString());
            string qrCodeByte64 = BitmapToBase64(qrCode);
            string subHtml = @"<html>" +
                "<body style='margin:0;padding:0;font-size:16pt;'>" +
                      "<table style='width:430px;font-size:16pt;text-align:center'>" +
                           "<tr>" +
                                "<td><img src='data:image/png;base64," + qrCodeByte64 + "' style='width:35mm;border:1px solid #000;padding:1px;margin:1px;' /></td>" +
                           "</tr>" +
                            "<tr>" +
                                "<td><strong>Mã kèm theo combo: <strong>"+ subId + "</td>" +
                            "</tr>" +
                             "<tr>" +
                                "<td><strong>Thuộc mã đơn: <strong>" + orderid + "</td>" +
                            "</tr>" +
                      "</table>" +
                "</body>" +
            "</html>";
            return subHtml;
        }







        async Task PrintSilentWebView2(string pdfPath)
        {
            await webView21.EnsureCoreWebView2Async();
            webView21.CoreWebView2.Navigate(new Uri(pdfPath).AbsoluteUri);
            await Task.Delay(300);
            var settings = webView21.CoreWebView2.Environment.CreatePrintSettings();
            settings.PrinterName = "Xprinter 80C"; // Tên máy in trong hệ thống
            settings.ShouldPrintHeaderAndFooter = false;
            settings.ShouldPrintBackgrounds = false; 

           // settings.PageWidth = 3.15; // 80mm quy đổi ra inches
           // settings.PageHeight = 11.0; // Đặt dài một chút, Driver sẽ tự cắt nếu chọn Short Receipt

            try
            {
                CoreWebView2PrintStatus status = await webView21.CoreWebView2.PrintAsync(settings);

                if (status == CoreWebView2PrintStatus.Succeeded)
                {
                    // In thành công, có thể xóa file PDF tạm hoặc cập nhật DB
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in ngầm: " + ex.Message);
            }
        }


        public Bitmap CreateQRCode(string ticketId)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                // Tạo dữ liệu mã QR với mức độ sửa lỗi M (Medium - 15%) 
                // Mức M là tối ưu nhất cho máy in nhiệt
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(ticketId, QRCodeGenerator.ECCLevel.M);

                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    // Số 20 là kích thước mỗi pixel trong mã QR, bạn có thể chỉnh để QR to/nhỏ
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    return qrCodeImage;
                }
            }
        }

        public string BitmapToBase64(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Lưu bitmap vào bộ nhớ dưới dạng PNG
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                // Chuyển mảng byte thành chuỗi Base64
                return Convert.ToBase64String(byteImage);
            }
        }




    }
}
