using DocumentFormat.OpenXml.Office.Y2022.FeaturePropertyBag;
using GM_DAL.Models.TicketOrder;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp
{
    public static class PrintTemplateHTML
    {
        public static string imgsFolder = ConfigurationManager.AppSettings["ImageLibaryPath"];
        public static string generateHTMLBill(TicketOrderHeaderModel header, long subId, string subCode,bool inGop)
        {

            string folderPath = imgsFolder.Replace('\\', '/');
            string fullImagePathLogo = $"file:///{folderPath}/logo-langbian-land.jpg";

            string itemHtml = "";
            int soluong = (inGop == true ? header.Quanti : 1);
            decimal total = (inGop == true ? header.Total : header.Price);
            if (header.DiscountPercent > 0)
            {
                ;
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
                    "<div style='text-align:center;margin-top:10px;'><span>Loại vé: " + header.TicketCode + "</span></div>" +
                    "<table style='width:430px;text-align:center;font-size:16pt;'>" +
                         "<tr>" +
                            "<th>Đơn Giá</th>" +
                            "<th>Số lượng</th>" +
                            "<th>Thành tiền</th>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>"+ header.Price.ToString("N0") + "</td>" +
                            "<td>"+ soluong + "</td>" +
                            "<td>"+ total.ToString("N0") + "</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td colspan='3'>(Hai trăm tám mươi nghìn đồng)</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td colspan='3'>Ngày: "+ DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td colspan='3'><strong>(Vé chỉ có giá trị sử dụng trong ngày)</strong></td>" +
                        "</tr>" +
                     "</table><br/>" + itemHtml + "<hr/>" +
                      "<table style='width:430px;font-size:16pt;'>" +
                           "<tr>" +
                                "<td><img src='data:image/png;base64," + qrCodeByte64 + "' style='width:35mm;border:1px solid #000;padding:1px;margin:1px;' /></td>" +
                                "<td font-size:16pt;>" +
                                        "<strong>Mã đơn:</strong> " + header.Id + "<br/>" +
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



        public static string generateHTMLSubBill(long orderid, long subId, string subCode)
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
                                "<td><strong>Mã kèm theo combo: <strong>" + subId + "</td>" +
                            "</tr>" +
                             "<tr>" +
                                "<td><strong>Thuộc mã đơn: <strong>" + orderid + "</td>" +
                            "</tr>" +
                      "</table>" +
                "</body>" +
            "</html>";
            return subHtml;
        }

        public static Bitmap CreateQRCode(string ticketId)
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

        public static string BitmapToBase64(Bitmap bitmap)
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
