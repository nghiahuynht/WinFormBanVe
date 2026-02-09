using DinkToPdf;
using DinkToPdf.Contracts;
using GM_DAL.IServices;
using GM_DAL.Models.TicketOrder;
using GM_DAL.Models.User;
using Microsoft.Office.Interop.Excel;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
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
    public partial class CartViewerForm : Form
    {
        public List<PostOrderSaveModel> lstItems;
        ITicketOrderService ticketOrderService;
        string printerName = ConfigurationManager.AppSettings["PrinterName"];
        string imgsFolder = ConfigurationManager.AppSettings["ImageLibaryPath"];
        string billPdfExportPath = ConfigurationManager.AppSettings["BillExportPath"];
        private static readonly IConverter _converter = new SynchronizedConverter(new PdfTools());
        public CartViewerForm(List<PostOrderSaveModel> lstItems, ITicketOrderService ticketOrderService)
        {
            InitializeComponent();
            this.lstItems = lstItems;
            this.ticketOrderService = ticketOrderService;
        }

        private void CartViewerForm_Load(object sender, EventArgs e)
        {
            InitializeWebView2();
            InitGrid();
            CalculaTotalCart();
           
        }

        private void InitGrid()
        {

            Helper.SetupFormatGridView(dataGridCartView);


            if (dataGridCartView.Columns.Count == 0)
            {
                dataGridCartView.AutoGenerateColumns = false;
                dataGridCartView.AllowUserToAddRows = false;
                dataGridCartView.ReadOnly = true;
                dataGridCartView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Hiển thị row header để dễ theo dõi số thứ tự dòng
                dataGridCartView.RowHeadersVisible = true;
                dataGridCartView.RowHeadersWidth = 45;


                dataGridCartView.Columns.Clear();

                dataGridCartView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "ID",
                    DataPropertyName = "CartLineId",
                    Name = "CartLineId",
                    Visible = false // Ẩn cột này
                });
                dataGridCartView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Mã vé",
                    DataPropertyName = "TicketCode",
                    Name = "colTicketCode",
                    Width = 150
                });

                dataGridCartView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Khách hàng",
                    DataPropertyName = "CustomerName",
                    Name = "colCustomerName",
                    Width = 250
                });
                dataGridCartView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Loại KH",
                    DataPropertyName = "CustomerType",
                    Name = "colCustomerType",
                    Width = 140
                });

                dataGridCartView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Đối tượng",
                    DataPropertyName = "ObjType",
                    Name = "colObjType",
                    Width = 150
                });


                dataGridCartView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Giá bán",
                    DataPropertyName = "Price",
                    Name = "colPrice",
                    Width = 150
                });
                dataGridCartView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "SL",
                    DataPropertyName = "Quanti",
                    Name = "colQuanti",
                    Width = 50
                });
                dataGridCartView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Thành tiền",
                    DataPropertyName = "TotalAfterDiscount",
                    Name = "colTotalAfterDiscount",
                    Width = 150
                });


                dataGridCartView.DataSource = lstItems;
                SetupDeleteColumn();
                SetupPrintColumn();
            }
            else
            {
                dataGridCartView.DataSource = lstItems;
                dataGridCartView.Update();
                dataGridCartView.Refresh();
            }


            string[] numericCols = { "colTotalAfterDiscount", "colPrice" };

            foreach (var colName in numericCols)
            {
                if (dataGridCartView.Columns[colName] != null)
                {
                    dataGridCartView.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridCartView.Columns[colName].DefaultCellStyle.Format = "N0";
                }


            }


        }

        private void SetupDeleteColumn()
        {
            if (dataGridCartView.Columns.Contains("colDelete"))
            {
                return;
            }
            string printImagePath = Helper.GetImageFullPath("delete-red-icon.png");
            Image editIcon = null;
            if (File.Exists(printImagePath))
            {
                try
                {
                    // Tải ảnh. Sử dụng using để đảm bảo tài nguyên được giải phóng đúng cách
                    using (var stream = new FileStream(printImagePath, FileMode.Open, FileAccess.Read))
                    {
                        editIcon = Image.FromStream(stream);
                    }

                    DataGridViewImageColumn colDel = new DataGridViewImageColumn
                    {
                        HeaderText = "Xóa",
                        Name = "colDelete",
                        Image = editIcon,
                        ImageLayout = DataGridViewImageCellLayout.Zoom,
                        Width = 60,
                        ToolTipText = "Xóa dòng"
                    };
                    dataGridCartView.Columns.Insert(0, colDel);



                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tải ảnh xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (dataGridCartView.Columns.Contains("colDelete")) dataGridCartView.Columns["colDelete"].ReadOnly = true;


        }


        private void SetupPrintColumn()
        {
            if (dataGridCartView.Columns.Contains("colPrint"))
            {
                return;
            }
            string printImagePath = Helper.GetImageFullPath("cart-icon.png");
            Image editIcon = null;
            if (File.Exists(printImagePath))
            {
                try
                {
                    // Tải ảnh. Sử dụng using để đảm bảo tài nguyên được giải phóng đúng cách
                    using (var stream = new FileStream(printImagePath, FileMode.Open, FileAccess.Read))
                    {
                        editIcon = Image.FromStream(stream);
                    }

                    DataGridViewImageColumn colEdit = new DataGridViewImageColumn
                    {
                        HeaderText = "In",
                        Name = "colPrint",
                        Image = editIcon,
                        ImageLayout = DataGridViewImageCellLayout.Zoom,
                        Width = 60,
                        ToolTipText = "Bán và in vé"
                    };
                    dataGridCartView.Columns.Insert(9, colEdit);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi tải ảnh in: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (dataGridCartView.Columns.Contains("colPrint")) dataGridCartView.Columns["colPrint"].ReadOnly = true;


        }


        private void CalculaTotalCart()
        {
            decimal totalCart = lstItems.Sum(x => x.TotalAfterDiscount);
            decimal khachdua = Convert.ToDecimal(txtTienKhachDua.Text);
            decimal tienthoi = khachdua - totalCart;
            lblTotalCart.Text = totalCart.ToString("N0");
            lblTienThoi.Text = tienthoi.ToString("N0");

        }

        private void txtTienKhachDua_MouseLeave(object sender, EventArgs e)
        {
            CalculaTotalCart();
        }

        private async void dataGridCartView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string columnName = dataGridCartView.Columns[e.ColumnIndex].Name;
                if (columnName == "colDelete")
                {
                    var value = dataGridCartView.Rows[e.RowIndex].Cells["CartLineId"].Value;

                    if (value != null)
                    {
                        string cartLineId = value.ToString();
                        var objectRemove = lstItems.Where(x => x.CartLineId.ToString() == cartLineId).FirstOrDefault();
                        lstItems.Remove(objectRemove);
                        ReBindGridCartAfterAction();
                    }
                }

                if (columnName == "colPrint")
                {
                    var value = dataGridCartView.Rows[e.RowIndex].Cells["CartLineId"].Value;

                    if (value != null)
                    {
                        string cartLineId = value.ToString();
                        var objectOrder = lstItems.Where(x => x.CartLineId.ToString() == cartLineId).FirstOrDefault();
                        var newOrder = await ticketOrderService.SaveOrderInfo(objectOrder,AuthenInfo().userName);
                        if (newOrder.data != null 
                            && string.IsNullOrEmpty(newOrder.message.exMessage)
                            && newOrder.data.value > 0)
                        {
                            
                            lstItems.Remove(objectOrder);
                            ReBindGridCartAfterAction();
                            CalculaTotalCart();
                            PrintOrder(Convert.ToInt64(newOrder.data.value));
                        }
                        else
                        {
                            MessageBox.Show("Lỗi tạo đơn: " + newOrder.message.exMessage);
                        }
                    }
                }
            }
        }

        private void ReBindGridCartAfterAction()
        {
            dataGridCartView.DataSource = null;
            dataGridCartView.DataSource = lstItems;
            dataGridCartView.Update();
            dataGridCartView.Refresh();
        }


        private void dataGridCartView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string columnName = dataGridCartView.Columns[e.ColumnIndex].Name;
                if (columnName == "colDelete" || columnName == "colPrint")
                {
                    dataGridCartView.Cursor = Cursors.Hand;
                }
            }
        }

        private void dataGridCartView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string columnName = dataGridCartView.Columns[e.ColumnIndex].Name;
                if (columnName == "colDelete" || columnName == "colPrint")
                {
                    dataGridCartView.Cursor = Cursors.Default;
                }
            }
        }

        private void dataGridCartView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridCartView.Columns[e.ColumnIndex].Name == "colTotalAfterDiscount" && e.Value != null)
            {
                string status = e.Value.ToString();

                e.CellStyle.ForeColor = Color.Green;
                e.CellStyle.SelectionForeColor = Color.DarkRed; // Màu khi chọn dòng đó
                e.CellStyle.Font = new System.Drawing.Font(e.CellStyle.Font, FontStyle.Bold); //
            }
        }





        private async void PrintOrder(long orderId)
        {
            var headerOrder = await ticketOrderService.GetHeaderOrderById(orderId);
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

            string savePath = Path.Combine(billPdfExportPath, orderId + ".pdf");
            await Task.Run(async () => {
                byte[] pdf = _converter.Convert(doc);
                File.WriteAllBytes(savePath, pdf);

                if (File.Exists(savePath))
                {
                    await PrintSilentWebView2(savePath);
                }

            });




        }




        public static AuthenSuccessModel AuthenInfo()
        {
            string loginFile = ConfigurationManager.AppSettings["LoginFile"];
            AuthenSuccessModel userObject = null;
            if (File.Exists(loginFile))
            {
                using (StreamReader readtext = new StreamReader(loginFile))
                {
                    string result = readtext.ReadLine();
                    if (!string.IsNullOrEmpty(result))
                    {
                        userObject = JsonConvert.DeserializeObject<AuthenSuccessModel>(result);
                    }
                }
            }
            return userObject;
        }


        public string generateHTMLBill(TicketOrderHeaderModel header, long subId, string subCode)
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
                    "<div style='text-align:center;margin-top:10px;'><span>Loại vé: " + header.TicketCode + "</span></div>" +
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



        public string generateHTMLSubBill(long orderid, long subId, string subCode)
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




        private async void InitializeWebView2()
        {
            try
            {
                // Điều này đảm bảo CoreWebView2 được tạo sẵn trên UI thread
                await webView21.EnsureCoreWebView2Async(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo WebView2: " + ex.Message);
            }
        }


        async Task PrintSilentWebView2(string pdfPath)
        {
            // 1. Đảm bảo khởi tạo trên UI Thread
            if (webView21.InvokeRequired)
            {
                webView21.Invoke(new MethodInvoker(async () => await PrintSilentWebView2(pdfPath)));
                return;
            }
            await webView21.EnsureCoreWebView2Async();
            var tcs = new TaskCompletionSource<bool>();

            EventHandler<CoreWebView2NavigationCompletedEventArgs> handler = null;
            handler = (s, e) => {
                webView21.NavigationCompleted -= handler;
                tcs.SetResult(e.IsSuccess);
            };

            webView21.NavigationCompleted += handler;
            webView21.CoreWebView2.Navigate(new Uri(pdfPath).AbsoluteUri);

            // Đợi cho đến khi load xong hoặc timeout
            if (await tcs.Task)
            {
                var settings = webView21.CoreWebView2.Environment.CreatePrintSettings();
                settings.PrinterName = printerName;
                settings.ShouldPrintHeaderAndFooter = false;
                settings.ShouldPrintBackgrounds = false;
                try
                {
                    CoreWebView2PrintStatus status = await webView21.CoreWebView2.PrintAsync(settings);

                    if (status == CoreWebView2PrintStatus.Succeeded)
                    {
                        webView21.CoreWebView2.Navigate("about:blank");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi in: " + ex.Message);
                }
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
