using GM_DAL.IServices;
using GM_DAL.Models.TicketOrder;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                        var newOrder = await ticketOrderService.SaveOrderInfo(objectOrder);
                        if (newOrder.data != null 
                            && !string.IsNullOrEmpty(newOrder.message.exMessage)
                            && newOrder.data.value > 0)
                        {
                            lstItems.Remove(objectOrder);
                            ReBindGridCartAfterAction();
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
    }
}
