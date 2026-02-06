using GM_DAL;
using GM_DAL.IServices;
using GM_DAL.Models.Customer;
using GM_DAL.Models.CustomerType;
using GM_DAL.Models.Ticket;
using GM_DAL.Models.TicketGroup;
using GM_DAL.Services;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public FormBanVe()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            var context = new SQLAdoContext();
            _ticketGroupService = new TicketGroupService(context);
            _ticketService = new TicketService(context);
            _customerTypeService = new CustomerTypeService(context);
            _customerService = new CustomerService(context);

            this.Load += FormBanVe_Load;
            this.Shown += FormBanVe_Shown;

            LoadDoiTuongComboBox();
            SetupGiaNumeric();
            loadhinhthucthanhtoan();
            loadkieuin();
        }

        private void FormBanVe_Shown(object sender, EventArgs e)
        {
            // Clear focus sau khi form hiện hoàn toàn (combo sẽ không còn vệt focus xám)
            BeginInvoke(new Action(() =>
            {
                this.ActiveControl = null;
                this.Focus();
            }));
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
                new ComboItem { Text = "Chuyển Khoản", Value = "CK" }
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
                new ComboItem { Text = "In Gộp", Value = "CK" },
                new ComboItem { Text = "In QR Code", Value = "CK" }
            }.ToList();

            cb_kieuin.DataSource = list;
            cb_kieuin.DisplayMember = "Text";
            cb_kieuin.ValueMember = "Value";
            cb_kieuin.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_kieuin.SelectedIndex = 0;
        }

        private void SetupGiaNumeric()
        {
            txtdongia.DecimalPlaces = 0;
            txtdongia.ThousandsSeparator = true;
            txtdongia.Minimum = 0;
            txtdongia.Maximum = 1000000000;
            txtdongia.Increment = 1000;

            txtsoluong.DecimalPlaces = 0;
            txtsoluong.ThousandsSeparator = true;
            txtsoluong.Minimum = 0;
            txtsoluong.Maximum = 10000;
            txtsoluong.Increment = 1;

            txtkhachdua.DecimalPlaces = 0;
            txtkhachdua.ThousandsSeparator = true;
            txtkhachdua.Minimum = 0;
            txtkhachdua.Maximum = 1000000000;
            txtkhachdua.Increment = 1000;
        }

        private void FormBanVe_Load(object sender, EventArgs e)
        {
            // ✅ init grid + load dữ liệu như cũ
            InitMenuGrid();
            LoadMenuLikeImage();
            LoadLoaiKhachComboBox();

            // tránh gắn event nhiều lần
            cb_loaikhach.SelectionChangeCommitted -= cb_loaikhach_SelectionChangeCommitted;
            cb_loaikhach.SelectionChangeCommitted += cb_loaikhach_SelectionChangeCommitted;

            load_cus_theo_style();

            // ✅ build giao diện card giống mock (UI only)
            BuildModernCardLayout();

            this.ActiveControl = null;
        }

        private void cb_loaikhach_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_isBindingLoaiKhach) return;
            load_cus_theo_style();
        }

        private void load_cus_theo_style()
        {
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
            cb_khachhang.DisplayMember = "CustomerName";
            cb_khachhang.ValueMember = "CustomerCode";
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

            gvMenu.Left = 12;
            gvMenu.Top = 70;
            gvMenu.Width = 500;
            gvMenu.Height = 500;
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

            var ticketsByGroup = tickets
                .GroupBy(t => (t.TicketGroup ?? "").Trim())
                .ToDictionary(g => g.Key, g => g.OrderBy(x => x.Priority ?? 0).ToList());

            gvMenu.SuspendLayout();
            gvMenu.Rows.Clear();

            foreach (var g in groups)
            {
                var code = (g.GroupCode ?? "").Trim();
                var name = string.IsNullOrWhiteSpace(g.GroupName) ? code : g.GroupName;

                int headerRowIndex = gvMenu.Rows.Add(name, "");
                var headerRow = gvMenu.Rows[headerRowIndex];
                headerRow.Tag = new GroupHeaderTag { GroupCode = code, GroupName = name };
                headerRow.Height = 34;
                headerRow.DefaultCellStyle.BackColor = Color.White;
                headerRow.DefaultCellStyle.ForeColor = Color.DodgerBlue;
                headerRow.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                headerRow.DefaultCellStyle.SelectionBackColor = Color.White;
                headerRow.DefaultCellStyle.SelectionForeColor = Color.DodgerBlue;

                if (ticketsByGroup.TryGetValue(code, out var list))
                {
                    foreach (var t in list)
                    {
                        int idx = gvMenu.Rows.Add(t.Code, t.Description);
                        gvMenu.Rows[idx].Tag = t;
                        gvMenu.Rows[idx].Height = 32;
                    }
                }
            }

            gvMenu.ClearSelection();
            gvMenu.ResumeLayout();
        }

        private void GvMenu_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var row = gvMenu.Rows[e.RowIndex];
            if (!(row.Tag is GroupHeaderTag header)) return;

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
            foreach (DataGridViewRow r in gvMenu.SelectedRows)
            {
                if (r.Tag is GroupHeaderTag)
                    r.Selected = false;
            }
        }

        private void GvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var tag = gvMenu.Rows[e.RowIndex].Tag;

            if (tag is GroupHeaderTag) return;

            if (tag is TicketModel ticket)
            {
                var code = ticket.Code ?? "";

                _ticketDangChon = laythongtinve(code);

                if (_ticketDangChon != null)
                {
                    // ✅ giữ đúng như bạn đang làm
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

        private void txtdongia_ValueChanged(object sender, EventArgs e) => tinhtienkhuyenmai();
        private void txtsoluong_ValueChanged(object sender, EventArgs e) => tinhtienkhuyenmai();
        private void txtkhachdua_ValueChanged(object sender, EventArgs e) => tinhtienkhuyenmai();

        private void clicknut(String nut)
        {
            if (txtsoluong.Value.ToString().Length < 1)
            {
                txtsoluong.Value = Convert.ToDecimal(nut);
            }
            else
            {
                if (txtsoluong.Value.ToString().Length < 4)
                {
                    txtsoluong.Value = Convert.ToDecimal(txtsoluong.Value + "" + Convert.ToDecimal(nut));
                }
                else
                {
                    MessageBox.Show("Vượt quá số lượng");
                    return;
                }
            }
        }

        private void nut1_Click(object sender, EventArgs e) => clicknut("1");
        private void nut2_Click(object sender, EventArgs e) => clicknut("2");
        private void nut3_Click(object sender, EventArgs e) => clicknut("3");
        private void nut4_Click(object sender, EventArgs e) => clicknut("4");
        private void nut5_Click(object sender, EventArgs e) => clicknut("5");
        private void nut6_Click(object sender, EventArgs e) => clicknut("6");
        private void nut7_Click(object sender, EventArgs e) => clicknut("7");
        private void nut8_Click(object sender, EventArgs e) => clicknut("8");
        private void nut9_Click(object sender, EventArgs e) => clicknut("9");
        private void nut0_Click(object sender, EventArgs e) => clicknut("0");

        private void nutxoanhaplai_Click(object sender, EventArgs e)
        {
            txtsoluong.Value = 0;
            txttienKM.Text = "0";
            txtkhachdua.Value = 0;
            lblthanhtien.Text = "0";
            lbltongthanhtoan.Text = "0";
            lbltienthoi.Text = "0";
        }

        #region UI CARD LAYOUT (giống mock) - chỉ thay giao diện

        private Panel _header;
        private TableLayoutPanel _mainGrid;

        // ✅ mới: label Kiểu in để dời xuống card NHẬP SỐ
        private Label _lblKieuIn;

        private readonly Color _bg = Color.FromArgb(246, 248, 252);
        private readonly Color _card = Color.White;
        private readonly Color _border = Color.FromArgb(226, 232, 240);
        private readonly Color _text = Color.FromArgb(15, 23, 42);
        private readonly Color _muted = Color.FromArgb(100, 116, 139);
        private readonly Color _blue1 = Color.FromArgb(37, 99, 235);
        private readonly Color _blue2 = Color.FromArgb(29, 78, 216);

        private Font F(float size, bool bold = false)
            => new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);

        private void BuildModernCardLayout()
        {
            if (_mainGrid != null) return;

            this.BackColor = _bg;

            groupBox1.Visible = false;
            groupBox2.Visible = false;

            // ===== HEADER =====
            _header = new GradientPanel(_blue2, _blue1)
            {
                Dock = DockStyle.Top,
                Height = 66,
                Padding = new Padding(18, 10, 18, 10)
            };
            this.Controls.Add(_header);
            _header.BringToFront();

            // ✅ FIX: ĐẨY LABEL9 LÊN (không đổi logic nào khác)
            // Label9 đang nằm trên Form nhưng bị _header che => đưa label9 vào _header
            if (label9 != null)
            {
                label9.Parent = _header;
                label9.AutoSize = true;
                label9.BackColor = Color.Transparent;  // ăn gradient
                label9.ForeColor = Color.White;        // nổi trên nền xanh
                label9.Location = new Point(18, 20);   // góc trái header
                label9.BringToFront();
            }

            // (giữ nguyên như bạn đang comment pictureBox2)
            // pictureBox2.Parent = _header;
            // pictureBox2.Location = new Point(18, 12);
            // pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            // pictureBox2.BackColor = Color.Transparent;

            label1.Parent = _header;
            label1.AutoSize = false;
            label1.TextAlign = ContentAlignment.MiddleRight;
            label1.ForeColor = Color.White;
            label1.BackColor = Color.Transparent;
            label1.Font = F(11, true);
            label1.Location = new Point(220, 0);
            label1.Size = new Size(this.ClientSize.Width - 240, _header.Height);
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // ===== MAIN GRID 2 CỘT =====
            _mainGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = _bg,
                Padding = new Padding(18, 16, 18, 18),
                ColumnCount = 2,
                RowCount = 1
            };
            _mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            _mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            this.Controls.Add(_mainGrid);
            _mainGrid.BringToFront();

            // ===== LEFT CARD =====
            var leftCard = CreateCard("DANH SÁCH VÉ", "Chọn vé bên dưới");
            leftCard.Dock = DockStyle.Fill;
            _mainGrid.Controls.Add(leftCard, 0, 0);

            gvMenu.Parent = leftCard;
            gvMenu.Location = new Point(16, 68);
            gvMenu.Size = new Size(leftCard.ClientSize.Width - 32, leftCard.ClientSize.Height - 84);
            gvMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // ===== RIGHT STACK =====
            var rightStack = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = _bg,
                ColumnCount = 1,
                RowCount = 3
            };

            rightStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 400)); // top cards
            rightStack.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // keypad
            rightStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));  // actions

            _mainGrid.Controls.Add(rightStack, 1, 0);

            // ---- TOP CARDS (3 cột) ----
            var topCards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = _bg,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(0)
            };
            topCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            topCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            topCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30f));
            rightStack.Controls.Add(topCards, 0, 0);

            var cardInfo = CreateCard("THÔNG TIN", "Loại khách / Khách hàng / Đối tượng");
            var cardPrice = CreateCard("BẢNG GIÁ", "Đơn giá / Số lượng / Khuyến mãi");
            var cardPay = CreateCard("THANH TOÁN", "Tổng tiền & tiền thối");

            cardInfo.Dock = DockStyle.Fill;
            cardPrice.Dock = DockStyle.Fill;
            cardPay.Dock = DockStyle.Fill;

            topCards.Controls.Add(cardInfo, 0, 0);
            topCards.Controls.Add(cardPrice, 1, 0);
            topCards.Controls.Add(cardPay, 2, 0);

            // ====== Add controls into cards ======

            // INFO
            PlaceLabel(cardInfo, "Loại khách", 16, 62);
            MoveControl(cb_loaikhach, cardInfo, 16, 88, 10);
            PlaceLabel(cardInfo, "Khách hàng", 16, 128);
            MoveControl(cb_khachhang, cardInfo, 16, 154, 10);
            PlaceLabel(cardInfo, "Đối tượng", 16, 194);
            MoveControl(cb_doituong, cardInfo, 16, 220, 10);

            // PRICE
            PlaceLabel(cardPrice, "Đơn giá", 16, 62);
            MoveControl(txtdongia, cardPrice, 16, 88, 10);
            PlaceLabel(cardPrice, "Số lượng", 16, 128);
            MoveControl(txtsoluong, cardPrice, 16, 154, 10);
            PlaceLabel(cardPrice, "Khuyến mãi (%)", 16, 194);
            MoveControl(txtkhuyenmai, cardPrice, 16, 220, 10);

            PlaceLabel(cardPrice, "Tiền khuyến mãi", 16, 260);
            txttienKM.Parent = cardPrice;
            txttienKM.Location = new Point(16, 286);
            txttienKM.Width = 10;

            // PAY
            PlaceLabel(cardPay, "Thành tiền", 16, 62);
            MoveMoneyLabel(lblthanhtien, cardPay, 16, 90, _blue1, 18);

            PlaceLabel(cardPay, "Tổng thanh toán", 16, 132);
            MoveMoneyLabel(lbltongthanhtoan, cardPay, 16, 160, Color.FromArgb(220, 38, 38), 18);

            PlaceLabel(cardPay, "Tiền thối", 16, 202);
            MoveMoneyLabel(lbltienthoi, cardPay, 16, 230, _text, 16);

            PlaceLabel(cardPay, "Hình thức", 200, 62);
            MoveControl(cb_hinhthuc, cardPay, 200, 88, 10);
            PlaceLabel(cardPay, "Khách đưa", 200, 132);
            MoveControl(txtkhachdua, cardPay, 200, 158, 10);

            // ---- KEYPAD CARD ----
            var keypadCard = CreateCard("NHẬP SỐ", "Bàn phím số lượng");
            keypadCard.Dock = DockStyle.Fill;
            rightStack.Controls.Add(keypadCard, 0, 1);

            groupBox3.Parent = keypadCard;
            groupBox3.Size = new Size(380, 72);

            _lblKieuIn = new Label
            {
                Parent = keypadCard,
                Text = "Kiểu in",
                AutoSize = true,
                ForeColor = _muted,
                Font = F(9, true)
            };

            cb_kieuin.Parent = keypadCard;
            cb_kieuin.DropDownStyle = ComboBoxStyle.DropDownList;

            var keypadPanel = new TableLayoutPanel
            {
                Parent = keypadCard,
                BackColor = _card,
                ColumnCount = 6,
                RowCount = 2,
                Size = new Size(keypadCard.ClientSize.Width - 32, 150),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            for (int i = 0; i < 6; i++) keypadPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66f));
            keypadPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            keypadPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            AddKeypadBtn(keypadPanel, nut1, 0, 0);
            AddKeypadBtn(keypadPanel, nut2, 1, 0);
            AddKeypadBtn(keypadPanel, nut3, 2, 0);
            AddKeypadBtn(keypadPanel, nut4, 3, 0);
            AddKeypadBtn(keypadPanel, nut5, 4, 0);
            AddKeypadBtn(keypadPanel, nut6, 5, 0);

            AddKeypadBtn(keypadPanel, nut7, 0, 1);
            AddKeypadBtn(keypadPanel, nut8, 1, 1);
            AddKeypadBtn(keypadPanel, nut9, 2, 1);
            AddKeypadBtn(keypadPanel, nut0, 3, 1);

            nutxoanhaplai.Parent = keypadPanel;
            keypadPanel.Controls.Add(nutxoanhaplai, 4, 1);
            keypadPanel.SetColumnSpan(nutxoanhaplai, 2);
            nutxoanhaplai.Dock = DockStyle.Fill;
            nutxoanhaplai.Margin = new Padding(10);

            // ---- ACTION BAR ----
            var actionBar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _bg
            };
            rightStack.Controls.Add(actionBar, 0, 2);

            nuthemvaodon.Parent = actionBar;
            nutxemdon.Parent = actionBar;

            void RelayoutAction()
            {
                int h = 68;
                int top = 12;
                int half = actionBar.Width / 2;
                nuthemvaodon.SetBounds(0, top, half - 12, h);
                nutxemdon.SetBounds(half + 12, top, half - 12, h);
            }
            actionBar.Resize += (s, e) => RelayoutAction();
            RelayoutAction();

            void RelayoutCards()
            {
                int wInfo = Math.Max(120, cardInfo.ClientSize.Width - 32);
                cb_loaikhach.Width = wInfo;
                cb_khachhang.Width = wInfo;
                cb_doituong.Width = wInfo;

                int wPrice = Math.Max(120, cardPrice.ClientSize.Width - 32);
                txtdongia.Width = wPrice;
                txtsoluong.Width = wPrice;
                txtkhuyenmai.Width = wPrice;
                txttienKM.Width = wPrice;

                int rightCol = Math.Max(160, cardPay.ClientSize.Width / 2);
                int wRight = Math.Max(120, cardPay.ClientSize.Width - rightCol - 16);

                cb_hinhthuc.Left = rightCol;
                txtkhachdua.Left = rightCol;

                cb_hinhthuc.Width = wRight;
                txtkhachdua.Width = wRight;

                keypadPanel.Width = keypadCard.ClientSize.Width - 32;

                int totalBlockH = groupBox3.Height + 18 + keypadPanel.Height;
                int free = keypadCard.ClientSize.Height - 68 - totalBlockH;
                int offsetY = Math.Max(0, free / 2);

                groupBox3.Left = 16;
                groupBox3.Top = 64 + offsetY;

                keypadPanel.Left = 16;
                keypadPanel.Top = groupBox3.Bottom + 18;

                int xKieuIn = groupBox3.Right + 24;
                int yKieuIn = groupBox3.Top + 6;

                _lblKieuIn.Location = new Point(xKieuIn, yKieuIn);

                int comboTop = yKieuIn + 22;
                int maxW = keypadCard.ClientSize.Width - xKieuIn - 16;
                int wKieuIn = Math.Max(160, Math.Min(260, maxW));

                cb_kieuin.Location = new Point(xKieuIn, comboTop);
                cb_kieuin.Size = new Size(wKieuIn, cb_kieuin.Height);

                if (maxW < 170)
                {
                    _lblKieuIn.Location = new Point(16, groupBox3.Bottom + 10);
                    cb_kieuin.Location = new Point(16, _lblKieuIn.Bottom + 6);
                    cb_kieuin.Width = keypadCard.ClientSize.Width - 32;
                }
            }

            this.Resize += (s, e) =>
            {
                label1.Size = new Size(this.ClientSize.Width - 240, _header.Height);
                RelayoutCards();
            };

            ApplyModernStyles();
            RelayoutCards();

            BeginInvoke(new Action(() => this.ActiveControl = null));
        }


        private void ApplyModernStyles()
        {
            StyleCombo(cb_loaikhach);
            StyleCombo(cb_khachhang);
            StyleCombo(cb_doituong);
            StyleCombo(cb_hinhthuc);
            StyleCombo(cb_kieuin);

            StyleNumeric(txtdongia);
            StyleNumeric(txtsoluong);
            StyleNumeric(txtkhuyenmai);
            StyleNumeric(txtkhachdua);

            txttienKM.BorderStyle = BorderStyle.FixedSingle;
            txttienKM.Font = F(10, false);

            StyleKeypad(nut0); StyleKeypad(nut1); StyleKeypad(nut2); StyleKeypad(nut3); StyleKeypad(nut4);
            StyleKeypad(nut5); StyleKeypad(nut6); StyleKeypad(nut7); StyleKeypad(nut8); StyleKeypad(nut9);

            StylePrimary(nuthemvaodon);
            StyleSecondary(nutxemdon);
            StyleWarning(nutxoanhaplai);

            gvMenu.BackgroundColor = Color.White;
            gvMenu.GridColor = Color.FromArgb(226, 232, 240);
            gvMenu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            gvMenu.ColumnHeadersDefaultCellStyle.ForeColor = _text;
            gvMenu.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            gvMenu.DefaultCellStyle.SelectionForeColor = _text;
        }

        private void StyleCombo(ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;
            cb.Font = F(10, false);
            cb.BackColor = Color.White;
            cb.ForeColor = _text;
        }

        private void StyleNumeric(NumericUpDown n)
        {
            n.Font = F(10, false);
            n.BackColor = Color.White;
            n.ForeColor = _text;
        }

        private void StyleKeypad(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = _blue1;
            b.ForeColor = Color.White;
            b.Font = F(12, true);
            b.Cursor = Cursors.Hand;
        }

        private void StylePrimary(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Color.FromArgb(34, 197, 94);
            b.ForeColor = Color.White;
            b.Font = F(12, true);
        }

        private void StyleSecondary(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = _blue2;
            b.ForeColor = Color.White;
            b.Font = F(12, true);
        }

        private void StyleWarning(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Color.FromArgb(245, 158, 11);
            b.ForeColor = Color.White;
            b.Font = F(11, true);
        }

        private Panel CreateCard(string title, string subtitle)
        {
            var card = new CardPanel
            {
                BackColor = _card,
                Dock = DockStyle.None,
                Margin = new Padding(10),
                Padding = new Padding(16),
                Radius = 14,
                BorderColor = _border,
                Shadow = true
            };

            _ = new Label
            {
                Parent = card,
                AutoSize = true,
                Text = title,
                Font = F(11, true),
                ForeColor = _text,
                Location = new Point(16, 14)
            };

            _ = new Label
            {
                Parent = card,
                AutoSize = true,
                Text = subtitle,
                Font = F(9, false),
                ForeColor = _muted,
                Location = new Point(16, 38)
            };

            return card;
        }

        private void PlaceLabel(Control parent, string text, int x, int y)
        {
            _ = new Label
            {
                Parent = parent,
                Text = text,
                AutoSize = true,
                ForeColor = _muted,
                Font = F(9, true),
                Location = new Point(x, y)
            };
        }

        private void MoveControl(Control c, Control parent, int x, int y, int width)
        {
            c.Parent = parent;
            c.Location = new Point(x, y);
            c.Width = width;
        }

        private void MoveMoneyLabel(Label lb, Control parent, int x, int y, Color color, float fontSize)
        {
            lb.Parent = parent;
            lb.AutoSize = true;
            lb.Location = new Point(x, y);
            lb.ForeColor = color;
            lb.Font = F(fontSize, true);
        }

        private void AddKeypadBtn(TableLayoutPanel p, Button b, int col, int row)
        {
            b.Parent = p;
            p.Controls.Add(b, col, row);
            b.Dock = DockStyle.Fill;
            b.Margin = new Padding(10);
        }

        // ---------- Custom Panels ----------
        private class GradientPanel : Panel
        {
            private readonly Color _c1;
            private readonly Color _c2;
            public GradientPanel(Color c1, Color c2) { _c1 = c1; _c2 = c2; DoubleBuffered = true; }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                using var br = new LinearGradientBrush(this.ClientRectangle, _c1, _c2, LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(br, this.ClientRectangle);
            }
        }

        private class CardPanel : Panel
        {
            public int Radius { get; set; } = 14;
            public Color BorderColor { get; set; } = Color.LightGray;
            public bool Shadow { get; set; } = true;

            public CardPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                using var path = RoundedRect(rect, Radius);

                if (Shadow)
                {
                    using var sh = new SolidBrush(Color.FromArgb(18, 0, 0, 0));
                    var shadowRect = new Rectangle(2, 4, Width - 4, Height - 4);
                    using var shadowPath = RoundedRect(shadowRect, Radius);
                    e.Graphics.FillPath(sh, shadowPath);
                }

                using var bg = new SolidBrush(BackColor);
                e.Graphics.FillPath(bg, path);

                using var pen = new Pen(BorderColor, 1f);
                e.Graphics.DrawPath(pen, path);
            }

            private GraphicsPath RoundedRect(Rectangle r, int radius)
            {
                int d = radius * 2;
                var path = new GraphicsPath();
                path.AddArc(r.X, r.Y, d, d, 180, 90);
                path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }
        }

        #endregion

        private void nuthemvaodon_Click(object sender, EventArgs e)
        {

        }

        private void nutxemdon_Click(object sender, EventArgs e)
        {

        }
    }
}
