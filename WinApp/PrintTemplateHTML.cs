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
        public static string qrFolder = ConfigurationManager.AppSettings["QrFolder"];
        public static string linkTraCuu = ConfigurationManager.AppSettings["LinkTraCuu"];
        public static string generateHTMLBill(TicketOrderHeaderModel header, long subId, string subCode,bool inGop)
        {

            string folderPath = imgsFolder.Replace('\\', '/');

            string itemHtml = "";
            int soluong = (inGop == true ? header.Quanti : 1);
           
            decimal total = (inGop == true ? header.Total : header.Price);
            string priceString = header.Price.ToString("N0");
            string totalString = total.ToString("N0");
            string bangChu = "";

            if (header.CustomerType == "LuHanh")
            {
                priceString = string.Empty;
                totalString = string.Empty;
                bangChu = string.Empty;
            }

            if (header.DiscountPercent > 0)
            {
     
                itemHtml = "<table style='width:430px;border-collapse:collapse;' border='1'  >";
                string phanTramKM = header.DiscountPercent.ToString() + "%";
                string tienKM = header.DiscountedAmount.ToString();
                if (phanTramKM != "0")
                {
                    tienKM = tienKM + " (" + phanTramKM + "%)";
                }
                string tienSauKM = header.TotalAfterDiscounted.ToString();
                bangChu = Helper.TienBangChu(tienSauKM.ToString());
                itemHtml += "<tr><td colspan='2'><strong>Tổng tiền:</strong><td style='text-align:right;'><strong>" + total.ToString("N0") + "</strong></td></tr>";
                itemHtml += "<tr><td colspan='2'><strong>Khuyến mãi:</strong><td style='text-align:right;'><strong>" + tienKM + "</strong></td></tr>";
                itemHtml += "<tr><td colspan='2'><strong>Tổng cần thanh toán:</strong></td><td style='text-align:right;'><strong>" + tienSauKM + "</strong></td></tr>";
                itemHtml += "</table>";
            }

            Bitmap qrCode = CreateQRCode(subId.ToString());
            string qrCodeByte64 = BitmapToBase64(qrCode);
            bangChu = Helper.TienBangChu(total.ToString());
            string simpleHtml = @"<html>" +
                "<body style='margin:5;padding:5;font-size:14pt;font-family:Arial;'>" +
                    "<table style='width:430px;border-bottom:1px solid #000;margin-top:10px;'>" +
                        "<tr>" +
                             "<td width='200px' style='text-align:left;'>" +
                                 "<span style='font-size:14pt;'>MST: 5801503332</span> <br/> " +
                                 "<span style='font-size:14pt;'>Hotline: 0923519519</span><br/>" +
                                 "<span style='font-size:14pt;'>93A Bidoup, phường Langbiang - Đà Lạt</span>" +
                             "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<div style='text-align:center;margin-top:10px;font-size:18pt;'><span>Loại vé: " + header.TicketCode + "</span></div>" +
                    "<table style='width:430px;text-align:center;font-size:14pt;'>" +
                         "<tr>" +
                            "<th>Đơn Giá</th>" +
                            "<th>Số lượng</th>" +
                            "<th>Thành tiền</th>" +
                        "</tr>" +
                        "<tr>" +
                            "<td>"+ priceString + "</td>" +
                            "<td>"+ soluong + "</td>" +
                            "<td>"+ totalString + "</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td colspan='3'>"+ bangChu + "</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td colspan='3'>Ngày: "+ DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td colspan='3'><strong>(Vé chỉ có giá trị sử dụng trong ngày)</strong></td>" +
                        "</tr>" +
                     "</table><br/>" + itemHtml +
                      "<table style='width:430px;font-size:14pt;'>" +
                           "<tr>" +
                                "<td><img src='data:image/png;base64," + qrCodeByte64 + "' style='width:35mm;border:1px solid #000;padding:1px;margin:1px;' /></td>" +
                                "<td font-size:16pt;>" +
                                        "<strong>Mã đơn:</strong> " + header.Id + "<br/>" +
                                        "<strong>Số vé:</strong> " + subId + "<br/>" +
                                        "<strong>Mã tra cứu:</strong> " + subCode + "<br/>" +
                                        "<strong>Link: </strong>" + linkTraCuu+
                                "</td>" +
                           "</tr>" +
                      "</table>" +
                     "<table style='text-align:center;margin-top:3px;font-size:14pt;'>" +
                           "<tr>" +
                                "<td>(Vé sử dụng trong ngày)</td>" +
                           "</tr>" +
                      "</table>" +
                     "<div style='text-align:center;margin-top:3px;font-size:14pt;'>KÍNH CHÚC QUÝ KHÁCH VUI CHƠI VUI VẺ.</div>" +
                "</body>" +
            "</html>";


            return simpleHtml;
        }



        public static string generateHTMLSubBill(long orderid, long subId, string subCode)
        {
            Bitmap qrCode = CreateQRCode(subId.ToString());
            string qrCodeByte64 = BitmapToBase64(qrCode);
            string subHtml = @"<html>" +
                "<body style='margin:5;padding:5;font-size:16pt;'>" +
                      "<table style='width:430px;font-size:16pt;text-align:center;margin-top:5px;'>" +
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
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(ticketId, QRCodeGenerator.ECCLevel.M);

                using (QRCode qrCode = new QRCode(qrCodeData))
                {

                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    return qrCodeImage;
                }
            }
        }


        public static void CreateQRCodeAndSave(string ticketId, string folderPath)
        {

          //  string folderSave = folderPath.Replace('\\', '/');

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(ticketId, QRCodeGenerator.ECCLevel.M);

                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    // Tạo Bitmap
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    // 1. Kiểm tra và tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // 2. Tạo đường dẫn đầy đủ (ví dụ: D:\Tickets\QR_123.jpg)
                    string fullPath = Path.Combine(folderPath, $"{ticketId}.jpg");

                    // 3. Lưu xuống ổ cứng dưới dạng Jpeg
                    qrCodeImage.Save(fullPath, System.Drawing.Imaging.ImageFormat.Jpeg);

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
