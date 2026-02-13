using DinkToPdf;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Drawing.Charts;
using GM_DAL.IServices;
using GM_DAL.Models.TicketOrder;
using GM_DAL.Models.User;
using Microsoft.Office.Interop.Excel;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using PdfiumViewer;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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
      
            InitGrid();
            CalculaTotalCart();
            if (AuthenInfo().userName== "nghiahotro")
            {
                button1.Visible = true;
            }
            else
            {
                button1.Visible = false;
            }
          
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
            decimal khachdua = 0;
            if (!string.IsNullOrEmpty(txtTienKhachDua.Text))
            {
                khachdua= Convert.ToDecimal(txtTienKhachDua.Text);
            }


            decimal totalCart = lstItems.Sum(x => x.TotalAfterDiscount);
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
                        CalculaTotalCart();
                    }
                }

                if (columnName == "colPrint")
                {
                    var value = dataGridCartView.Rows[e.RowIndex].Cells["CartLineId"].Value;

                    if (value != null)
                    {
                        string cartLineId = value.ToString();
                        var objectOrder = lstItems.Where(x => x.CartLineId.ToString() == cartLineId).FirstOrDefault();
                        var newOrder = await ticketOrderService.SaveOrderInfo(objectOrder, AuthenInfo().userName);
                        if (newOrder.data != null
                            && string.IsNullOrEmpty(newOrder.message.exMessage)
                            && newOrder.data.value > 0)
                        {

                            lstItems.Remove(objectOrder);
                            ReBindGridCartAfterAction();
                            CalculaTotalCart();


                            /*========== Call In =============*/


                            using (WaitingForm fWait = new WaitingForm())
                            {
                                fWait.Show();
                                fWait.Refresh(); // Vẽ lại giao diện ngay lập tức

                                try
                                {

                                    await Task.Run(() =>
                                    {
                                        PrintOrder(Convert.ToInt64(newOrder.data.value));
                                        Thread.Sleep(3000);
                                    });
                                }
                                catch (Exception ex)
                                {
                                    fWait.Close();
                                    MessageBox.Show($"Lỗi trong quá trình in: {ex.Message}");
                                }
                                finally
                                {
                                    fWait.Close();
                                }
                            }



                            /*=============END Call in*/

                           
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
                string htmlBill = PrintTemplateHTML.generateHTMLBill(headerOrder.data, subIdFirst, subCodeFirst,true);
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
                        htmlBill = PrintTemplateHTML.generateHTMLBill(headerOrder.data, subITem.SubId, subITem.SubOrderCode,false);

                    }
                    else if (subITem.SubType == "SubChild")
                    {
                        htmlBill = PrintTemplateHTML.generateHTMLSubBill(headerOrder.data.Id, subITem.SubId, subITem.SubOrderCode);

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
            await Task.Run(async () =>
            {
                byte[] pdf = _converter.Convert(doc);
                File.WriteAllBytes(savePath, pdf);

                if (File.Exists(savePath))
                {
                    ThucHienIn(savePath, printerName);
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

        private async void button1_Click(object sender, EventArgs e)
        {
            using (WaitingForm fWait = new WaitingForm())
            {
                fWait.Show();
               // fWait.Refresh(); // Vẽ lại giao diện ngay lập tức

                try
                {
                   
                    await Task.Run(() =>
                    {
                        PrintOrder(205974);
                        Thread.Sleep(3000);
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi trong quá trình in: {ex.Message}");
                }
                finally
                {
                    // 4. Đóng Form Waiting và mở lại nút bấm
                    fWait.Close();
                }
            }



        }



        public void ThucHienIn(string filePath, string printerName)
        {
            try
            {
                // 1. Kiểm tra file tồn tại trước khi xử lý
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Không tìm thấy file tại: {filePath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Nạp file PDF (Sử dụng 'using' declaration của .NET 6 để tự động giải phóng bộ nhớ)
                using var document = PdfDocument.Load(filePath);

                // 3. Tạo đối tượng in với chế độ FitSize (tự động co dãn cho vừa khổ giấy vé)
                using var printDocument = document.CreatePrintDocument(PdfPrintMode.Scale);

                // 4. Cấu hình các thông số in
                printDocument.PrinterSettings.PrinterName = printerName;

                // Ẩn cửa sổ trạng thái "Printing..." của Windows (In ngầm hoàn toàn)
                printDocument.PrintController = new StandardPrintController();

                // Thiết lập lề bằng 0 - cực kỳ quan trọng đối với in vé/in nhiệt
                printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                printDocument.OriginAtMargins = false;

                // 5. Bắn lệnh in xuống máy in
                printDocument.Print();
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi như: sai tên máy in, thiếu file pdfium.dll, hoặc file PDF lỗi
                MessageBox.Show($"Lỗi thực hiện in: {ex.Message}", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }





    }
}
