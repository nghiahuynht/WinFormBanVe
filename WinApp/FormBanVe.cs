using GM_DAL;
using GM_DAL.IServices;
using GM_DAL.Models.Customer;
using GM_DAL.Models.CustomerType;
using GM_DAL.Models.Ticket;
using GM_DAL.Models.TicketGroup;
using GM_DAL.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinApp
{
    public partial class FormBanVe : Form
    {
        private readonly ITicketGroupService _ticketGroupService;
        private readonly ITicketService _ticketService;
        private readonly ICustomerTypeService _customerTypeService;
        private readonly ICustomerService _customerService;

        private bool _isBindingLoaiKhach;
        private DataGridView gvMenu;
        private TicketModel _ticketDangChon;
        private sealed class GroupHeaderTag
        {
            public string GroupCode { get; set; }
            public string GroupName { get; set; }
        }

        private class ComboItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        private void LoadDoiTuongComboBox()
        {
            var list = new[]
            {
                new ComboItem { Text = "Người Lớn", Value = "NguoiLon" },
                new ComboItem { Text = "Trẻ Em",    Value = "TreEm" }
            }.ToList();

            cb_doituong.DataSource = list;
            cb_doituong.DisplayMember = "Text";
            cb_doituong.ValueMember = "Value";
            cb_doituong.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_doituong.SelectedIndex = 0;

        }

        private void loadhinhthucthanhtoan()
        {
            var list = new[]
           {
                new ComboItem { Text = "Tiền Mặt", Value = "TM" },
                new ComboItem { Text = "Chuyển Khoản",    Value = "CK" }
            }.ToList();

            cb_hinhthuc.DataSource = list;
            cb_hinhthuc.DisplayMember = "Text";
            cb_hinhthuc.ValueMember = "Value";
            cb_hinhthuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_hinhthuc.SelectedIndex = 0;

        }


        private void loadkieuin()
        {
            var list = new[]
           {
                new ComboItem { Text = "In lẻ", Value = "TM" },
                new ComboItem { Text = "In Gộp",    Value = "CK" },
                new ComboItem { Text = "In QR Code",    Value = "CK" }
            }.ToList();

            cb_kieuin.DataSource = list;
            cb_kieuin.DisplayMember = "Text";
            cb_kieuin.ValueMember = "Value";
            cb_kieuin.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_kieuin.SelectedIndex = 0;

        }
        public FormBanVe()
        {
            InitializeComponent();

            var context = new SQLAdoContext();
            _ticketGroupService = new TicketGroupService(context);
            _ticketService = new TicketService(context);
            _customerTypeService = new CustomerTypeService(context);
            _customerService = new CustomerService(context);

            this.Load += FormBanVe_Load;

            LoadDoiTuongComboBox();
            SetupGiaNumeric();
            loadhinhthucthanhtoan();
            loadkieuin();
        }
        private void SetupGiaNumeric()
        {
            txtdongia.DecimalPlaces = 0;          // giá tiền thường không lẻ
            txtdongia.ThousandsSeparator = true;  // 200,000
            txtdongia.Minimum = 0;
            txtdongia.Maximum = 1000000000;       // 1 tỷ (tuỳ bạn)
            txtdongia.Increment = 1000;
            // bước nhảy

            txtsoluong.DecimalPlaces = 0;
            txtsoluong.ThousandsSeparator = true;
            txtsoluong.Minimum = 0;
            txtsoluong.Maximum = 10000;
            txtsoluong.Increment = 1;



            txtkhachdua.DecimalPlaces = 0;          // giá tiền thường không lẻ
            txtkhachdua.ThousandsSeparator = true;  // 200,000
            txtkhachdua.Minimum = 0;
            txtkhachdua.Maximum = 1000000000;       // 1 tỷ (tuỳ bạn)
            txtkhachdua.Increment = 1000;
        }


        private void GvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //click vào label vé bên trái
            if (e.RowIndex < 0) return;

            var tag = gvMenu.Rows[e.RowIndex].Tag;

            // click group header => bỏ qua
            if (tag is GroupHeaderTag) return;

            if (tag is TicketModel ticket)
            {
                var code = ticket.Code ?? "";

                _ticketDangChon = laythongtinve(code);

                if (_ticketDangChon != null)
                {
                    // ✅ đổ giá vào textbox, format đẹp (không có số lẻ)
                    txtdongia.Text = (_ticketDangChon.Price ?? 0).ToString("N0");
                }
                else
                {
                    txtdongia.Text = "";
                }
                txtkhuyenmai.Enabled = true;
                txtsoluong.Enabled = true;
                cb_hinhthuc.Enabled = true;
                txtkhachdua.Enabled = true;

                //MessageBox.Show(code, "Ticket Code");
            }
        }



        private void FormBanVe_Load(object sender, EventArgs e)
        {
            InitMenuGrid();
            LoadMenuLikeImage();
            LoadLoaiKhachComboBox();

            // tránh gắn event nhiều lần
            cb_loaikhach.SelectionChangeCommitted -= cb_loaikhach_SelectionChangeCommitted;
            cb_loaikhach.SelectionChangeCommitted += cb_loaikhach_SelectionChangeCommitted;

            load_cus_theo_style();
        }

        private void cb_loaikhach_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_isBindingLoaiKhach) return;
            load_cus_theo_style();
        }

        private void load_cus_theo_style()
        {
            // lấy value của cb_loaikhach (ValueMember = Code)
            string cusTypeKey = cb_loaikhach.SelectedValue?.ToString();

            var res = _customerService.TimKiemTheoCusType(cusTypeKey);

            if (res?.data == null)
            {
                MessageBox.Show(res?.message?.exMessage ?? "Không lấy được danh sách khách hàng theo loại!");
                return;
            }

            var list = res.data.ToList();

            cb_khachhang.DataSource = null;
            cb_khachhang.DataSource = list;
            cb_khachhang.DisplayMember = "CustomerName";   // hiển thị tên
            cb_khachhang.ValueMember = "CustomerCode";     // value là mã
            cb_khachhang.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_khachhang.SelectedIndex = 0;
        }

        private void LoadLoaiKhachComboBox()
        {
            var res = _customerTypeService.toanbo();

            if (res?.data == null)
            {
                MessageBox.Show(res?.message?.exMessage ?? "Không lấy được CustomerType!");
                return;
            }

            _isBindingLoaiKhach = true;

            var list = res.data.ToList();

            cb_loaikhach.DataSource = list;
            cb_loaikhach.DisplayMember = "Name";
            cb_loaikhach.ValueMember = "Code";
            cb_loaikhach.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_loaikhach.SelectedIndex = 0;

            _isBindingLoaiKhach = false;
        }

        // ================== TẠO GRID TRONG GROUPBOX1 ==================
        private void InitMenuGrid()
        {
            if (gvMenu != null) return;

            gvMenu = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,

                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.Gainsboro,

                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                EnableHeadersVisualStyles = false
            };

            gvMenu.ColumnHeadersHeight = 36;
            gvMenu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            gvMenu.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            gvMenu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // chặn xanh hệ thống
            gvMenu.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            gvMenu.DefaultCellStyle.SelectionBackColor = Color.WhiteSmoke;
            gvMenu.DefaultCellStyle.SelectionForeColor = Color.Black;
            gvMenu.ColumnHeadersDefaultCellStyle.SelectionBackColor = gvMenu.ColumnHeadersDefaultCellStyle.BackColor;
            gvMenu.ColumnHeadersDefaultCellStyle.SelectionForeColor = gvMenu.ColumnHeadersDefaultCellStyle.ForeColor;

            gvMenu.Columns.Clear();
            gvMenu.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCode",
                HeaderText = "Mã vé",
                Width = 160
            });
            gvMenu.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDesc",
                HeaderText = "Mô tả",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // đặt vị trí grid nằm dưới label2/pictureBox1
            int top = Math.Max(label2.Bottom, pictureBox1.Bottom) + 10;
            gvMenu.Left = 12;
            gvMenu.Top = top;
            gvMenu.Width = groupBox1.ClientSize.Width - 24;
            gvMenu.Height = groupBox1.ClientSize.Height - top - 12;
            gvMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            gvMenu.CellPainting += GvMenu_CellPainting;
            gvMenu.SelectionChanged += GvMenu_SelectionChanged;
            gvMenu.CellClick += GvMenu_CellClick;

            groupBox1.Controls.Add(gvMenu);
            gvMenu.BringToFront();
        }

        // ================== LOAD GIỐNG HÌNH (NHÓM + VÉ) ==================
        private void LoadMenuLikeImage()
        {
            var gRes = _ticketGroupService.toanbo();
            if (gRes?.data == null)
            {
                MessageBox.Show(gRes?.message?.exMessage ?? "Không lấy được TicketGroup!");
                return;
            }

            var tRes = _ticketService.toanbo(); // sp_ListAllTicket
            if (tRes?.data == null)
            {
                MessageBox.Show(tRes?.message?.exMessage ?? "Không lấy được Ticket!");
                return;
            }

            var groups = gRes.data.Where(x => x != null).ToList();
            var tickets = tRes.data.Where(x => x != null).ToList();

            // group vé theo TicketGroup
            var ticketsByGroup = tickets
                .GroupBy(t => (t.TicketGroup ?? "").Trim())
                .ToDictionary(g => g.Key, g => g.OrderBy(x => x.Priority ?? 0).ToList());

            gvMenu.SuspendLayout();
            gvMenu.Rows.Clear();

            foreach (var g in groups)
            {
                var code = (g.GroupCode ?? "").Trim();
                var name = string.IsNullOrWhiteSpace(g.GroupName) ? code : g.GroupName;

                // Dòng tiêu đề group
                int headerRowIndex = gvMenu.Rows.Add(name, "");
                var headerRow = gvMenu.Rows[headerRowIndex];
                headerRow.Tag = new GroupHeaderTag { GroupCode = code, GroupName = name };
                headerRow.Height = 34;
                headerRow.DefaultCellStyle.BackColor = Color.White;
                headerRow.DefaultCellStyle.ForeColor = Color.DodgerBlue;
                headerRow.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                headerRow.DefaultCellStyle.SelectionBackColor = Color.White;
                headerRow.DefaultCellStyle.SelectionForeColor = Color.DodgerBlue;

                // Vé trong group
                if (ticketsByGroup.TryGetValue(code, out var list))
                {
                    foreach (var t in list)
                    {
                        int idx = gvMenu.Rows.Add(t.Code, t.Description);
                        gvMenu.Rows[idx].Tag = t; // lưu ticket để click chọn
                        gvMenu.Rows[idx].Height = 32;
                    }
                }
            }

            gvMenu.ClearSelection();
            gvMenu.ResumeLayout();
        }

        // Vẽ dòng group span ngang (giống kiểu ".... VÉ THAM QUAN ....")
        private void GvMenu_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var row = gvMenu.Rows[e.RowIndex];
            if (!(row.Tag is GroupHeaderTag header)) return;

            // chỉ vẽ ở cột 0, cột 1 bỏ qua để span ngang
            if (e.ColumnIndex != 0)
            {
                e.Handled = true;
                return;
            }

            e.Handled = true;

            Rectangle rowRect = gvMenu.GetRowDisplayRectangle(e.RowIndex, true);
            using (var bg = new SolidBrush(Color.White))
                e.Graphics.FillRectangle(bg, rowRect);

            string text = $"••••  {header.GroupName.ToUpper()}  ••••";
            TextRenderer.DrawText(
                e.Graphics,
                text,
                new Font("Segoe UI", 10F, FontStyle.Bold),
                new Rectangle(rowRect.X + 8, rowRect.Y, rowRect.Width - 16, rowRect.Height),
                Color.DodgerBlue,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
            );

            using (var pen = new Pen(Color.Gainsboro))
                e.Graphics.DrawLine(pen, rowRect.Left, rowRect.Bottom - 1, rowRect.Right, rowRect.Bottom - 1);
        }

        private void GvMenu_SelectionChanged(object sender, EventArgs e)
        {
            // không cho select dòng header
            foreach (DataGridViewRow r in gvMenu.SelectedRows)
            {
                if (r.Tag is GroupHeaderTag)
                    r.Selected = false;
            }
        }



        // ==========================================================
        // ✅ CHỈ THÊM HÀM NÀY (KHÔNG ĐỔI LOGIC HIỆN TẠI)
        // ==========================================================
        private TicketModel laythongtinve(string mave)
        {
            if (string.IsNullOrWhiteSpace(mave))
            {
                MessageBox.Show("Mã vé trống!");
                return null;
            }

            var res = _ticketService.timkiemvetheoten(mave);

            if (res?.data == null)
            {
                MessageBox.Show(res?.message?.exMessage ?? $"Không tìm thấy vé với mã: {mave}");
                return null;
            }

            var t = res.data;

            // tạo object mới và gán full thông tin
            var doituong = new TicketModel
            {
                Id = t.Id,
                Code = t.Code,
                Price = t.Price,
                Description = t.Description,
                TicketGroup = t.TicketGroup,
                Priority = t.Priority,
                VAT = t.VAT,

                LoaiVe = t.LoaiVe,
                LoaiIn = t.LoaiIn,
                GateName = t.GateName,

                BillTemplate = t.BillTemplate,
                EContractTemplate = t.EContractTemplate,

                BranchId = t.BranchId,
                IsKhachNuocNgoai = t.IsKhachNuocNgoai,

                KyHieu = t.KyHieu,
                TieuDeVe = t.TieuDeVe,
                MauSoBienLai = t.MauSoBienLai,

                CreatedBy = t.CreatedBy,
                CreatedDate = t.CreatedDate,
                UpdatedBy = t.UpdatedBy,
                UpdatedDate = t.UpdatedDate,

                IsDeleted = t.IsDeleted
            };

            return doituong;
        }

        private void txtkhuyenmai_ValueChanged(object sender, EventArgs e)
        {
            decimal giaTri = txtdongia.Value;
            if (giaTri <= 0)
            {
                MessageBox.Show("Bạn chọn Vé bên trái nha");
                txtdongia.Value = 0;
                return;
            }
            tinhtienkhuyenmai();

        }

        private void rad_mienphi_CheckedChanged(object sender, EventArgs e)
        {
            rad_binhthuong.Checked = false;
            rad_mienphi.Checked = true;
        }

        private void rad_binhthuong_CheckedChanged(object sender, EventArgs e)
        {
            rad_binhthuong.Checked = true;
            rad_mienphi.Checked = false;
        }

        private void tinhtienkhuyenmai()
        {
            if (txtdongia.Value > 0 && txtsoluong.Value > 0)
            {
                decimal tienKM = txtdongia.Value * (txtkhuyenmai.Value / 100) * txtsoluong.Value;
                decimal roundedDown = Math.Floor(tienKM / 1000) * 1000;
                txttienKM.Text = roundedDown.ToString();
                lblthanhtien.Text = (txtdongia.Value * txtsoluong.Value).ToString();
                lbltongthanhtoan.Text = (txtdongia.Value * txtsoluong.Value - roundedDown).ToString();
                lbltienthoi.Text = (txtkhachdua.Value - (txtdongia.Value * txtsoluong.Value - roundedDown)).ToString();
            }
        }

        private void txtdongia_ValueChanged(object sender, EventArgs e)
        {
            tinhtienkhuyenmai();
        }

        private void txtsoluong_ValueChanged(object sender, EventArgs e)
        {
            tinhtienkhuyenmai();
        }

        private void txtkhachdua_ValueChanged(object sender, EventArgs e)
        {
            tinhtienkhuyenmai();
        }
        private void clicknut(String nut)
        {
            txtsoluong.Value = Convert.ToDecimal(nut);
        }

        private void nut1_Click(object sender, EventArgs e)
        {
            clicknut("1");
        }

        private void nut2_Click(object sender, EventArgs e)
        {
            clicknut("2");
        }

        private void nut3_Click(object sender, EventArgs e)
        {
            clicknut("3");
        }

        private void nut4_Click(object sender, EventArgs e)
        {
            clicknut("4");
        }

        private void nut5_Click(object sender, EventArgs e)
        {
            clicknut("5");
        }

        private void nut6_Click(object sender, EventArgs e)
        {
            clicknut("6");
        }

        private void nut7_Click(object sender, EventArgs e)
        {
            clicknut("7");
        }

        private void nut8_Click(object sender, EventArgs e)
        {
            clicknut("8");
        }

        private void nut9_Click(object sender, EventArgs e)
        {
            clicknut("9");
        }

        private void nut0_Click(object sender, EventArgs e)
        {
            clicknut("0");
        }

        private void nutxoanhaplai_Click(object sender, EventArgs e)
        {
            txtsoluong.Value = 0;
            txttienKM.Text = "0";
            txtkhachdua.Value = 0;
            lblthanhtien.Text = "0";
            lbltongthanhtoan.Text = "0";
            lbltienthoi.Text = "0";
        }
    }
}
