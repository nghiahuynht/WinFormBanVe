using GM_DAL;
using GM_DAL.IServices;
using GM_DAL.Models.Customer;
using GM_DAL.Models.CustomerType;
using GM_DAL.Models.Ticket;
using GM_DAL.Models.TicketGroup;
using GM_DAL.Models.TicketOrder;
using GM_DAL.Models.User;
using GM_DAL.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WinApp
{
    public partial class FormBanVe : Form
    {
        private readonly Color _selectedRed = Color.FromArgb(255, 190, 190);
        private readonly Color _selectedRedBorder = Color.FromArgb(239, 154, 154);
        private string _selectedTicketCode = null;

        private const string COL_RADIO = "col_radio";
        private const string COL_CODE = "col_code";

        private const int COMBO_BORDER_H = 42;
        private const int COMBO_INNER_PAD_Y = 6;

        private Panel _cartBadgeHost;

        internal Label CounterLabel => lblCounterCartNum;
        internal Panel LeftCardRef => _leftCardRef;

        private readonly ITicketGroupService _ticketGroupService;
        private readonly ITicketService _ticketService;
        private readonly ICustomerTypeService _customerTypeService;
        private readonly ICustomerService _customerService;
        private readonly ITicketOrderService _ticketOrderService;

        private bool _isBindingLoaiKhach;
        private DataGridView gvMenu;
        private TicketModel _ticketDangChon;
        private List<TicketPricePolicyModel> LstPricePolicy = new List<TicketPricePolicyModel>();

        public List<PostOrderSaveModel> lstItemCarts = new List<PostOrderSaveModel>();
        private int _hoverRowIndex = -1;

        private readonly Color _hoverBlue = Color.FromArgb(175, 215, 255);
        string imgsFolder = ConfigurationManager.AppSettings["ImageLibaryPath"];

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

            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F12)
                {
                    var tuner = new FormUiTuner(this);
                    tuner.Show();
                    tuner.BringToFront();
                }
            };

            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            var context = new SQLAdoContext();
            _ticketGroupService = new TicketGroupService(context);
            _ticketService = new TicketService(context);
            _customerTypeService = new CustomerTypeService(context);
            _customerService = new CustomerService(context);
            _ticketOrderService = new TicketOrderService(context);

            this.Load += FormBanVe_Load;
            this.Shown += FormBanVe_Shown;

            LoadDoiTuongComboBox();
            SetupGiaNumeric();
            loadhinhthucthanhtoan();
            loadkieuin();
            CounterCart();
            GetListPricePolicy();

            lbltendangnhap.Text = AuthenInfo().userName;
        }

        private void FormBanVe_Shown(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                this.ActiveControl = null;
                this.Focus();
            }));
        }

        private void CounterCart()
        {
            int cartCount = lstItemCarts.Count;
            lblCounterCartNum.Text = cartCount.ToString();

            if (_cartBadgeHost == null) return;

            int padX = 18;
            int padY = 8;

            Size textSize = TextRenderer.MeasureText(lblCounterCartNum.Text, lblCounterCartNum.Font);

            int h = Math.Max(34, textSize.Height + padY * 2 + 2);
            int w = textSize.Width + padX * 2 + 2;

            w = Math.Max(h, w);
            w = Math.Min(200, w);

            _cartBadgeHost.Size = new Size(w, h);

            lblCounterCartNum.Location = new Point(
                (w - textSize.Width) / 2,
                (h - textSize.Height) / 2 - 1
            );
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

            txtkhuyenmai.DecimalPlaces = 0;
            txtkhuyenmai.ThousandsSeparator = true;
            txtkhuyenmai.Minimum = 0;
            txtkhuyenmai.Maximum = 100;
            txtkhuyenmai.Increment = 1;
        }

        private void FormBanVe_Load(object sender, EventArgs e)
        {
            InitMenuGrid();
            LoadMenuLikeImage();
            LoadLoaiKhachComboBox();

            cb_loaikhach.SelectionChangeCommitted -= cb_loaikhach_SelectionChangeCommitted;
            cb_loaikhach.SelectionChangeCommitted += cb_loaikhach_SelectionChangeCommitted;

            load_cus_theo_style();

            BuildModernCardLayout();

            rad_xanh.CheckedChanged -= ThemeRadio_CheckedChanged;
            rad_nau.CheckedChanged -= ThemeRadio_CheckedChanged;
            rad_xanh.CheckedChanged += ThemeRadio_CheckedChanged;
            rad_nau.CheckedChanged += ThemeRadio_CheckedChanged;

            if (!rad_xanh.Checked && !rad_nau.Checked) rad_xanh.Checked = true;
            ApplyThemePalette();

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

            if (cb_khachhang.Items.Count > 0) cb_khachhang.SelectedIndex = 0;
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

            if (cb_loaikhach.Items.Count > 0) cb_loaikhach.SelectedIndex = 0;

            _isBindingLoaiKhach = false;
        }

        // ================== GRID ==================
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

            gvMenu.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            gvMenu.DefaultCellStyle.SelectionBackColor = Color.WhiteSmoke;
            gvMenu.DefaultCellStyle.SelectionForeColor = Color.Black;
            gvMenu.ColumnHeadersDefaultCellStyle.SelectionBackColor = gvMenu.ColumnHeadersDefaultCellStyle.BackColor;
            gvMenu.ColumnHeadersDefaultCellStyle.SelectionForeColor = gvMenu.ColumnHeadersDefaultCellStyle.ForeColor;

            gvMenu.DefaultCellStyle.Padding = new Padding(0, 4, 0, 4);
            gvMenu.RowTemplate.Height = 40;

            gvMenu.Columns.Clear();
            gvMenu.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = COL_CODE,
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
            gvMenu.CellMouseEnter += GvMenu_CellMouseEnter;
            gvMenu.CellMouseLeave += GvMenu_CellMouseLeave;

            gvMenu.CellPainting += gvMenu_CellPaintingRadioInCode;
            gvMenu.CellClick += gvMenu_CellClickSelectTicketRadio;
        }

        private void gvMenu_CellPaintingRadioInCode(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (gvMenu.Columns[e.ColumnIndex].Name != COL_CODE) return;

            var row = gvMenu.Rows[e.RowIndex];
            if (!(row.Tag is TicketModel ticket)) return;

            string code = ticket.Code ?? "";
            bool isChecked = (!string.IsNullOrWhiteSpace(code) && _selectedTicketCode == code);

            e.PaintBackground(e.CellBounds, true);
            e.Paint(e.CellBounds, DataGridViewPaintParts.Border);

            Rectangle r = e.CellBounds;
            int radioSize = Math.Min(16, r.Height - 10);
            if (radioSize < 10) radioSize = 10;

            int radioX = r.Left + 8;
            int radioY = r.Top + (r.Height - radioSize) / 2;

            var outer = new Rectangle(radioX, radioY, radioSize, radioSize);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var pen = new Pen(Color.DodgerBlue, 2))
                e.Graphics.DrawEllipse(pen, outer);

            if (isChecked)
            {
                int innerSize = (int)(radioSize * 0.55);
                var inner = new Rectangle(
                    radioX + (radioSize - innerSize) / 2,
                    radioY + (radioSize - innerSize) / 2,
                    innerSize,
                    innerSize
                );
                using (var b = new SolidBrush(Color.DodgerBlue))
                    e.Graphics.FillEllipse(b, inner);
            }

            string text = code;
            int leftPad = 8 + radioSize + 8;
            var textRect = new Rectangle(r.Left + leftPad, r.Top, r.Width - leftPad - 6, r.Height);

            TextRenderer.DrawText(
                e.Graphics,
                text,
                e.CellStyle.Font,
                textRect,
                e.CellStyle.ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
            );

            e.Handled = true;
        }

        private void gvMenu_CellClickSelectTicketRadio(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = gvMenu.Rows[e.RowIndex];
            if (!(row.Tag is TicketModel ticket)) return;

            string code = ticket.Code ?? "";
            if (string.IsNullOrWhiteSpace(code)) return;

            _selectedTicketCode = code;
            gvMenu.ClearSelection();

            // reset nền các dòng vé
            foreach (DataGridViewRow r in gvMenu.Rows)
            {
                if (r.Tag is TicketModel)
                {
                    r.DefaultCellStyle.BackColor = Color.White;
                    r.DefaultCellStyle.SelectionBackColor = _selectedRed;
                    r.DefaultCellStyle.SelectionForeColor = _text;
                }
            }

            // select + tô đỏ nhạt dòng hiện tại
            row.Selected = true;
            row.DefaultCellStyle.BackColor = _selectedRed;
            row.DefaultCellStyle.SelectionBackColor = _selectedRed;
            row.DefaultCellStyle.SelectionForeColor = _text;

            gvMenu.Invalidate();

            _ticketDangChon = laythongtinve(code);

            if (_ticketDangChon != null)
            {
                decimal giaVeFromBangGia = PriceSaleFromPolicy();
                txtdongia.Text = giaVeFromBangGia.ToString("N0");
            }
            else
            {
                txtdongia.Text = "";
            }

            txtkhuyenmai.Enabled = true;
            txtsoluong.Enabled = true;
            cb_hinhthuc.Enabled = true;
            txtkhachdua.Enabled = true;

            gvMenu.Invalidate();
        }

        private void GvMenu_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = gvMenu.Rows[e.RowIndex];
            if (!(row.Tag is TicketModel)) return;

            _hoverRowIndex = e.RowIndex;

            if (row.Selected) return;
            row.DefaultCellStyle.BackColor = _hoverBlue;
        }

        private void GvMenu_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (_hoverRowIndex < 0) return;
            if (_hoverRowIndex >= gvMenu.Rows.Count) { _hoverRowIndex = -1; return; }

            var row = gvMenu.Rows[_hoverRowIndex];

            if (row.Tag is TicketModel)
            {
                if (!row.Selected)
                    row.DefaultCellStyle.BackColor = Color.White;
                else
                    row.DefaultCellStyle.BackColor = _selectedRed; // ✅ giữ đỏ nếu đang chọn
            }

            _hoverRowIndex = -1;
        }

        private void LoadMenuLikeImage()
        {
            var gRes = _ticketGroupService.toanbo();
            if (gRes?.data == null)
            {
                MessageBox.Show(gRes?.message?.exMessage ?? "Không lấy được TicketGroup!");
                return;
            }

            var tRes = _ticketService.toanbo();
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

                headerRow.Height = 44;
                headerRow.DefaultCellStyle.Padding = new Padding(0, 6, 0, 6);

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
                        gvMenu.Rows[idx].Height = 40;
                    }
                }

                int spacerIdx = gvMenu.Rows.Add("", "");
                var spacerRow = gvMenu.Rows[spacerIdx];
                spacerRow.Height = 15;
                spacerRow.DefaultCellStyle.BackColor = Color.White;
                spacerRow.DefaultCellStyle.SelectionBackColor = Color.White;
                spacerRow.DefaultCellStyle.ForeColor = Color.White;
                spacerRow.DefaultCellStyle.SelectionForeColor = Color.White;
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

            Rectangle rowRect2 = gvMenu.GetRowDisplayRectangle(e.RowIndex, true);
            using (var bg2 = new SolidBrush(Color.White))
                e.Graphics.FillRectangle(bg2, rowRect2);

            string text = $"••••  {header.GroupName.ToUpper()}  ••••";
            TextRenderer.DrawText(
                e.Graphics,
                text,
                new Font("Segoe UI", 10F, FontStyle.Bold),
                new Rectangle(rowRect2.X + 8, rowRect2.Y, rowRect2.Width - 16, rowRect2.Height),
                Color.DodgerBlue,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
            );

            using (var pen = new Pen(Color.Gainsboro))
                e.Graphics.DrawLine(pen, rowRect2.Left, rowRect2.Bottom - 1, rowRect2.Right, rowRect2.Bottom - 1);
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
                    decimal giaVeFromBangGia = PriceSaleFromPolicy();
                    txtdongia.Text = giaVeFromBangGia.ToString("N0");
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
            TinhTongBill();
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

        private void TinhTongBill()
        {
            if (txtdongia.Value > 0 && txtsoluong.Value > 0)
            {
                decimal tienKM = txtdongia.Value * (txtkhuyenmai.Value / 100) * txtsoluong.Value;
                decimal roundedDown = Math.Floor(tienKM / 1000) * 1000;

                txttienKM.Text = roundedDown.ToString("N0");
                lblthanhtien.Text = (txtdongia.Value * txtsoluong.Value).ToString("N0");
                lbltongthanhtoan.Text = (txtdongia.Value * txtsoluong.Value - roundedDown).ToString("N0");
                lbltienthoi.Text = (txtkhachdua.Value - (txtdongia.Value * txtsoluong.Value - roundedDown)).ToString("N0");
            }
        }

        private void txtdongia_ValueChanged(object sender, EventArgs e) => TinhTongBill();
        private void txtsoluong_ValueChanged(object sender, EventArgs e) => TinhTongBill();
        private void txtkhachdua_ValueChanged(object sender, EventArgs e) => TinhTongBill();

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

        #region UI CARD LAYOUT

        private Panel _header;
        private TableLayoutPanel _mainGrid;

        private Label _lblKieuIn;
        private Label _lblKhachDua;

        private Label _lblDanhSachVeTitle;
        private Panel _leftCardRef;

        private bool _coffeeMode = false;

        private readonly Color _bgBlue = Color.FromArgb(246, 248, 252);
        private readonly Color _cardBlue = Color.White;
        private readonly Color _borderBlue = Color.FromArgb(226, 232, 240);
        private readonly Color _textBlue = Color.FromArgb(15, 23, 42);
        private readonly Color _mutedBlue = Color.FromArgb(100, 116, 139);
        private readonly Color _blue1 = Color.FromArgb(37, 99, 235);
        private readonly Color _blue2 = Color.FromArgb(29, 78, 216);

        private readonly Color _bgCoffee = Color.FromArgb(248, 245, 242);
        private readonly Color _cardCoffee = Color.White;
        private readonly Color _borderCoffee = Color.FromArgb(229, 222, 215);
        private readonly Color _textCoffee = Color.FromArgb(35, 24, 18);
        private readonly Color _mutedCoffee = Color.FromArgb(128, 110, 96);
        private readonly Color _coffee1 = Color.FromArgb(111, 78, 55);
        private readonly Color _coffee2 = Color.FromArgb(74, 49, 35);

        private Color _bg, _card, _border, _text, _muted, _accent1, _accent2;

        private Font F(float size, bool bold = false)
            => new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Point);

        private void ThemeRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (rad_nau.Checked) _coffeeMode = true;
            else if (rad_xanh.Checked) _coffeeMode = false;

            ApplyThemePalette();
        }

        private void ApplyThemePalette()
        {
            if (_coffeeMode)
            {
                _bg = _bgCoffee;
                _card = _cardCoffee;
                _border = _borderCoffee;
                _text = _textCoffee;
                _muted = _mutedCoffee;
                _accent1 = _coffee1;
                _accent2 = _coffee2;
            }
            else
            {
                _bg = _bgBlue;
                _card = _cardBlue;
                _border = _borderBlue;
                _text = _textBlue;
                _muted = _mutedBlue;
                _accent1 = _blue1;
                _accent2 = _blue2;
            }

            this.BackColor = _bg;

            if (_header is GradientPanel gp)
            {
                gp.SetColors(_accent2, _accent1);
                gp.Invalidate();
            }

            if (label9 != null) { label9.BackColor = Color.Transparent; label9.ForeColor = Color.White; label9.Visible = true; }
            if (label1 != null) { label1.BackColor = Color.Transparent; label1.ForeColor = Color.White; label1.Visible = true; }

            if (_lblKieuIn != null) _lblKieuIn.ForeColor = _muted;
            if (_lblKhachDua != null) _lblKhachDua.ForeColor = _muted;
            if (_lblDanhSachVeTitle != null) _lblDanhSachVeTitle.ForeColor = _text;

            if (lblCounterCartNum != null)
            {
                lblCounterCartNum.BackColor = Color.Transparent;
                lblCounterCartNum.Visible = true;
            }

            ApplyModernStyles();
        }

        private void EnableRecalcOnClickAnywhere()
        {
            this.MouseDown += (s, e) => TinhTongBill();
            AttachClickRecursive(this);
        }

        private void AttachClickRecursive(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (!(c is NumericUpDown) && !(c is TextBox) && !(c is ComboBox) && !(c is DataGridView))
                {
                    c.MouseDown += (s, e) => TinhTongBill();
                }

                if (c.HasChildren) AttachClickRecursive(c);
            }
        }

        private void ApplyModernStyles()
        {
            this.BackColor = _bg;

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
            txttienKM.BackColor = Color.White;
            txttienKM.ForeColor = _text;

            StyleKeypad(nut0); StyleKeypad(nut1); StyleKeypad(nut2); StyleKeypad(nut3); StyleKeypad(nut4);
            StyleKeypad(nut5); StyleKeypad(nut6); StyleKeypad(nut7); StyleKeypad(nut8); StyleKeypad(nut9);

            StylePrimary(nuthemvaodon);
            StyleSecondary(nutxemdon);
            StyleWarning(nutxoanhaplai);

            gvMenu.BackgroundColor = Color.White;
            gvMenu.GridColor = _border;
            gvMenu.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249);
            gvMenu.ColumnHeadersDefaultCellStyle.ForeColor = _text;
            gvMenu.DefaultCellStyle.SelectionBackColor = _selectedRed;
            gvMenu.DefaultCellStyle.SelectionForeColor = _text;

            gvMenu.ColumnHeadersDefaultCellStyle.SelectionBackColor = gvMenu.ColumnHeadersDefaultCellStyle.BackColor;
            gvMenu.ColumnHeadersDefaultCellStyle.SelectionForeColor = gvMenu.ColumnHeadersDefaultCellStyle.ForeColor;

            lblthanhtien.ForeColor = _accent1;

            StyleLoaiDonGroup();
        }

        private void StyleCombo(ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;
            cb.Font = F(11, false);
            cb.BackColor = Color.White;
            cb.ForeColor = _text;
        }

        private void ResizeComboBorder(ComboBox cb, int width)
        {
            if (cb?.Parent == null) return;
            var inner = cb.Parent;
            var border = inner.Parent as Panel;
            if (border == null || border.Tag?.ToString() != "cb_border") return;

            border.Width = width;
            border.Height = COMBO_BORDER_H;
        }

        // ✅ NEW: move cả cái khung border (outer) của combo
        private void MoveComboBorder(ComboBox cb, Control parent, int x, int y, int width)
        {
            if (cb == null || parent == null) return;

            // nếu chưa wrap thì wrap luôn
            WrapComboWithBorder(cb);

            var inner = cb.Parent as Panel;
            var border = inner?.Parent as Panel;
            if (border == null || border.Tag?.ToString() != "cb_border") return;

            border.Parent = parent;
            border.Location = new Point(x, y);
            border.Size = new Size(Math.Max(160, width), COMBO_BORDER_H);
            border.Anchor = AnchorStyles.Top | AnchorStyles.Left; // ✅ tránh bung full width theo parent
            border.BringToFront();
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
            b.BackColor = _accent1;
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
            b.BackColor = _accent2;
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
                Font = F(12, true),
                ForeColor = _text,
                Location = new Point(16, 14)
            };

            _ = new Label
            {
                Parent = card,
                AutoSize = true,
                Text = subtitle,
                Font = F(10, false),
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
                Font = F(10, true),
                Location = new Point(x, y)
            };
        }

        private void WrapComboWithBorder(ComboBox cb)
        {
            if (cb == null) return;

            if (cb.Parent != null && cb.Parent.Tag?.ToString() == "cb_inner") return;

            var parent = cb.Parent;
            var loc = cb.Location;
            var size = cb.Size;
            var anchor = cb.Anchor;

            var border = new Panel
            {
                Parent = parent,
                Location = loc,
                Size = new Size(size.Width, COMBO_BORDER_H),
                Anchor = anchor,
                BackColor = _border,
                Padding = new Padding(1),
                Tag = "cb_border"
            };

            var inner = new Panel
            {
                Parent = border,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(8, COMBO_INNER_PAD_Y, 8, COMBO_INNER_PAD_Y),
                Tag = "cb_inner"
            };

            cb.Parent = inner;
            cb.Dock = DockStyle.Fill;

            cb.FlatStyle = FlatStyle.Flat;
            cb.BackColor = Color.White;
            cb.IntegralHeight = false;
        }

        private void ApplyComboBorders()
        {
            WrapComboWithBorder(cb_loaikhach);
            WrapComboWithBorder(cb_khachhang);
            WrapComboWithBorder(cb_doituong);
            WrapComboWithBorder(cb_hinhthuc);

            // ❌ QUAN TRỌNG: không wrap cb_kieuin ở đây (lúc này nó chưa nằm trong keypadCard)
            // WrapComboWithBorder(cb_kieuin);
        }

        private void MoveControl(Control c, Control parent, int x, int y, int width)
        {
            if (c == null || parent == null) return;

            c.Parent = parent;
            c.Location = new Point(x, y);

            if (width <= 20)
                c.Width = Math.Max(120, parent.ClientSize.Width - x - 16);
            else
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

        private void StyleLoaiDonGroup()
        {
            if (groupBox3 == null) return;

            groupBox3.Font = F(11, true);
            groupBox3.ForeColor = _text;

            if (rad_binhthuong != null)
            {
                rad_binhthuong.Font = F(11, false);
                rad_binhthuong.ForeColor = _text;
            }
            if (rad_mienphi != null)
            {
                rad_mienphi.Font = F(11, false);
                rad_mienphi.ForeColor = _text;
            }
        }

        private class GradientPanel : Panel
        {
            private Color _c1;
            private Color _c2;

            public GradientPanel(Color c1, Color c2)
            {
                _c1 = c1;
                _c2 = c2;
                DoubleBuffered = true;
            }

            public void SetColors(Color c1, Color c2)
            {
                _c1 = c1;
                _c2 = c2;
                Invalidate();
            }

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
            int nexLineId = lstItemCarts.Count + 1;
            string customerTypeSelected = cb_loaikhach.SelectedValue != null ? cb_loaikhach.SelectedValue.ToString() : string.Empty;
            string customerCodeSelected = cb_khachhang.SelectedValue != null ? cb_khachhang.SelectedValue.ToString() : string.Empty;
            string customerNameSelected = cb_khachhang.SelectedValue != null ? cb_khachhang.Text.ToString() : string.Empty;
            string doiTuongSelected = cb_doituong.SelectedValue != null ? cb_doituong.SelectedValue.ToString() : string.Empty;
            int soluong = Convert.ToInt16(txtsoluong.Value);
            decimal giaBan = Convert.ToDecimal(txtdongia.Value);
            decimal totalFirst = giaBan * soluong;
            int giamPhanTram = Convert.ToInt16(txtkhuyenmai.Value);
            decimal tienKhuyenMai = Math.Round((giamPhanTram * totalFirst) / 100);
            decimal tongSauKhuyenMai = totalFirst - tienKhuyenMai;
            string paymentType = cb_hinhthuc.SelectedValue.ToString();

            var newCartOrder = new PostOrderSaveModel
            {
                CartLineId = nexLineId,
                TicketCode = _ticketDangChon?.Code,
                CustomerType = customerTypeSelected,
                CustomerCode = customerCodeSelected,
                CustomerName = customerNameSelected,
                ObjType = doiTuongSelected,
                Quanti = soluong,
                Price = giaBan,
                DiscountPercent = giamPhanTram,
                DiscountValue = tienKhuyenMai,
                TienKhachDua = txtkhachdua.Value.ToString(),
                PaymentType = paymentType,
                UserLogin = AuthenInfo()?.userName,
                TotalAfterDiscount = tongSauKhuyenMai
            };

            lstItemCarts.Add(newCartOrder);
            CounterCart();
            ResetFormInput();
        }

        private void BuildModernCardLayout()
        {
            if (_mainGrid != null) return;

            _coffeeMode = rad_nau != null && rad_nau.Checked;
            ApplyThemePalette();

            groupBox1.Visible = false;
            groupBox2.Visible = false;

            // ================= HEADER =================
            _header = new GradientPanel(_accent2, _accent1)
            {
                Dock = DockStyle.Top,
                Height = 66,
                Padding = new Padding(18, 10, 18, 10)
            };
            this.Controls.Add(_header);
            _header.BringToFront();

            var headerGrid = new TableLayoutPanel
            {
                Parent = _header,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 3,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320));
            headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            headerGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260));

            var leftHeader = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            var midHeader = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            var rightHeader = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            headerGrid.Controls.Add(leftHeader, 0, 0);
            headerGrid.Controls.Add(midHeader, 1, 0);
            headerGrid.Controls.Add(rightHeader, 2, 0);

            if (label9 != null)
            {
                label9.Parent = leftHeader;
                label9.AutoSize = true;
                label9.BackColor = Color.Transparent;
                label9.ForeColor = Color.White;
                label9.Font = F(11, true);
                label9.Location = new Point(0, 10);
                label9.BringToFront();
            }

            if (rad_xanh != null)
            {
                rad_xanh.Parent = leftHeader;
                rad_xanh.AutoSize = true;
                rad_xanh.BackColor = Color.Transparent;
                rad_xanh.ForeColor = Color.White;
                rad_xanh.Location = new Point(92, 12);
                rad_xanh.BringToFront();
            }
            if (rad_nau != null)
            {
                rad_nau.Parent = leftHeader;
                rad_nau.AutoSize = true;
                rad_nau.BackColor = Color.Transparent;
                rad_nau.ForeColor = Color.White;
                rad_nau.Location = new Point(182, 12);
                rad_nau.BringToFront();
            }

            if (label1 != null)
            {
                label1.Parent = midHeader;
                label1.Dock = DockStyle.Fill;
                label1.AutoSize = false;
                label1.BackColor = Color.Transparent;
                label1.ForeColor = Color.White;
                label1.Font = F(11, true);
                label1.TextAlign = ContentAlignment.TopCenter;
                label1.Padding = new Padding(0, 16, 0, 0);
                label1.BringToFront();
            }

            var userFlow = new FlowLayoutPanel
            {
                Parent = rightHeader,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 16, 0, 0),
                Margin = Padding.Empty
            };
            userFlow.BringToFront();

            Panel avatarHost = null;
            if (picus != null)
            {
                avatarHost = new Panel
                {
                    Width = 30,
                    Height = 30,
                    Margin = new Padding(0, 0, 8, 0)
                };

                avatarHost.SizeChanged += (s, e) =>
                {
                    using (var gp = new GraphicsPath())
                    {
                        gp.AddEllipse(0, 0, avatarHost.Width - 1, avatarHost.Height - 1);
                        avatarHost.Region = new Region(gp);
                    }
                };

                picus.Parent = avatarHost;
                picus.Visible = true;
                picus.BackColor = Color.Transparent;
                picus.SizeMode = PictureBoxSizeMode.Zoom;
                picus.Dock = DockStyle.Fill;
                picus.Margin = Padding.Empty;

                if (picus.Image == null)
                {
                    try
                    {
                        string exeDir = AppDomain.CurrentDomain.BaseDirectory;

                        string FindFileUpwards(string startDir, string relativePath)
                        {
                            var dir = new DirectoryInfo(startDir);
                            for (int i = 0; i < 12 && dir != null; i++)
                            {
                                string candidate = Path.Combine(dir.FullName, relativePath);
                                if (File.Exists(candidate)) return candidate;
                                dir = dir.Parent;
                            }
                            return null;
                        }

                        string imgPath = FindFileUpwards(exeDir, Path.Combine("WinApp", "imgs", "user-244.png"));
                        if (!string.IsNullOrWhiteSpace(imgPath) && File.Exists(imgPath))
                        {
                            using (var tmp = Image.FromFile(imgPath))
                                picus.Image = new Bitmap(tmp);
                        }
                    }
                    catch { }
                }

                if (picus.Image == null)
                {
                    string imgIconPath = Path.Combine(imgsFolder, "user-244.png");
                    try { picus.ImageLocation = imgIconPath; } catch { }
                }
            }

            if (lbltendangnhap != null)
            {
                lbltendangnhap.Visible = true;
                lbltendangnhap.AutoSize = true;
                lbltendangnhap.BackColor = Color.Transparent;
                lbltendangnhap.ForeColor = Color.White;
                lbltendangnhap.Font = F(10, true);
                lbltendangnhap.Margin = new Padding(0);
                lbltendangnhap.TextAlign = ContentAlignment.MiddleLeft;
            }

            if (avatarHost != null) userFlow.Controls.Add(avatarHost);
            if (lbltendangnhap != null) userFlow.Controls.Add(lbltendangnhap);

            void RelayoutUserFlow()
            {
                int rightInset = 30;
                int x = Math.Max(0, rightHeader.ClientSize.Width - userFlow.PreferredSize.Width - rightInset);
                userFlow.Location = new Point(x, 0);
            }
            rightHeader.Resize += (s, e) => RelayoutUserFlow();
            RelayoutUserFlow();

            // ================= MAIN GRID =================
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

            // ================= LEFT CARD =================
            var leftCard = CreateCard("GIỎ HÀNG / ĐƠN ĐANG BÁN", "");
            _leftCardRef = leftCard;
            leftCard.Dock = DockStyle.Fill;
            _mainGrid.Controls.Add(leftCard, 0, 0);

            if (pictureBox1 != null)
            {
                pictureBox1.Parent = leftCard;
                pictureBox1.BackColor = Color.Transparent;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.SetBounds(16, 16, 28, 28);
                pictureBox1.BringToFront();
            }

            var lblTitle = leftCard.Controls.OfType<Label>().FirstOrDefault(l => l.Text == "GIỎ HÀNG / ĐƠN ĐANG BÁN");
            if (lblTitle != null) lblTitle.Location = new Point(52, 14);

            if (label2 != null) label2.Visible = false;

            _lblDanhSachVeTitle = new Label
            {
                Parent = leftCard,
                AutoSize = true,
                Text = "DANH SÁCH VÉ",
                Font = F(10.5f, true),
                ForeColor = _text,
                Location = new Point(16, 60)
            };

            if (_cartBadgeHost == null)
            {
                _cartBadgeHost = new Panel
                {
                    Parent = leftCard,
                    BackColor = _coffeeMode ? Color.FromArgb(255, 240, 230) : Color.FromArgb(239, 246, 255),
                    Size = new Size(52, 34),
                    Padding = Padding.Empty
                };

                _cartBadgeHost.SizeChanged += (s, e) =>
                {
                    int r = _cartBadgeHost.Height;
                    var rect = new Rectangle(0, 0, _cartBadgeHost.Width - 1, _cartBadgeHost.Height - 1);
                    using (var path = new GraphicsPath())
                    {
                        path.AddArc(rect.X, rect.Y, r, r, 180, 90);
                        path.AddArc(rect.Right - r, rect.Y, r, r, 270, 90);
                        path.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90);
                        path.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90);
                        path.CloseFigure();
                        _cartBadgeHost.Region = new Region(path);
                    }
                };
            }

            lblCounterCartNum.Parent = _cartBadgeHost;
            lblCounterCartNum.AutoSize = true;
            lblCounterCartNum.BackColor = Color.Transparent;
            lblCounterCartNum.ForeColor = Color.FromArgb(220, 38, 38);
            lblCounterCartNum.Font = F(13, true);
            lblCounterCartNum.TextAlign = ContentAlignment.MiddleCenter;
            lblCounterCartNum.Cursor = Cursors.Hand;
            lblCounterCartNum.BringToFront();

            void RelayoutCartBadge()
            {
                int x = leftCard.ClientSize.Width - _cartBadgeHost.Width - 16;
                int y = 14;
                _cartBadgeHost.Location = new Point(x, y);
                _cartBadgeHost.BringToFront();
            }

            leftCard.Resize += (s, e) => RelayoutCartBadge();

            CounterCart();
            RelayoutCartBadge();

            int gridTop = 92;
            gvMenu.Parent = leftCard;
            gvMenu.Location = new Point(16, gridTop);
            gvMenu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // ================= RIGHT STACK =================
            var rightStack = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = _bg,
                ColumnCount = 1,
                RowCount = 2
            };
            rightStack.RowStyles.Add(new RowStyle(SizeType.Absolute, 430));
            rightStack.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _mainGrid.Controls.Add(rightStack, 1, 0);

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

            PlaceLabel(cardInfo, "Loại khách", 16, 62);
            MoveControl(cb_loaikhach, cardInfo, 16, 88, 10);
            PlaceLabel(cardInfo, "Khách hàng", 16, 128);
            MoveControl(cb_khachhang, cardInfo, 16, 154, 10);
            PlaceLabel(cardInfo, "Đối tượng", 16, 194);
            MoveControl(cb_doituong, cardInfo, 16, 220, 10);

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

            _lblKhachDua = new Label
            {
                Parent = cardPrice,
                Text = "Khách đưa",
                AutoSize = true,
                ForeColor = _muted,
                Font = F(10, true),
                Location = new Point(16, 330)
            };
            txtkhachdua.Parent = cardPrice;
            txtkhachdua.Location = new Point(16, 356);
            txtkhachdua.Width = 10;

            PlaceLabel(cardPay, "Thành tiền", 16, 62);
            MoveMoneyLabel(lblthanhtien, cardPay, 16, 90, _accent1, 18);

            PlaceLabel(cardPay, "Tổng thanh toán", 16, 132);
            MoveMoneyLabel(lbltongthanhtoan, cardPay, 16, 160, Color.FromArgb(220, 38, 38), 18);

            PlaceLabel(cardPay, "Tiền thối", 16, 202);
            MoveMoneyLabel(lbltienthoi, cardPay, 16, 230, _text, 16);

            PlaceLabel(cardPay, "Hình thức", 200, 62);
            MoveControl(cb_hinhthuc, cardPay, 200, 88, 10);

            ApplyComboBorders();

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

            // ✅ FIX CỨNG: reset Dock/Anchor trước khi wrap lại trong keypadCard
            cb_kieuin.Parent = keypadCard;
            cb_kieuin.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_kieuin.Dock = DockStyle.None;                 // ✅ quan trọng (tránh bung full width)
            cb_kieuin.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cb_kieuin.Location = new Point(10, 10);
            cb_kieuin.Width = 200;

            // ✅ wrap border đúng parent mới (keypadCard)
            WrapComboWithBorder(cb_kieuin);

            var keypadPanel = new TableLayoutPanel
            {
                Parent = keypadCard,
                BackColor = _card,
                ColumnCount = 6,
                RowCount = 3,
                Size = new Size(keypadCard.ClientSize.Width - 32, 240),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            for (int i = 0; i < 6; i++) keypadPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66f));
            keypadPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            keypadPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            keypadPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));

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

            nuthemvaodon.Parent = keypadPanel;
            keypadPanel.Controls.Add(nuthemvaodon, 0, 2);
            keypadPanel.SetColumnSpan(nuthemvaodon, 3);
            nuthemvaodon.Dock = DockStyle.Fill;
            nuthemvaodon.Margin = new Padding(10);

            nutxemdon.Parent = keypadPanel;
            keypadPanel.Controls.Add(nutxemdon, 3, 2);
            keypadPanel.SetColumnSpan(nutxemdon, 3);
            nutxemdon.Dock = DockStyle.Fill;
            nutxemdon.Margin = new Padding(10);

            void RelayoutCards()
            {
                int wInfo = Math.Max(120, cardInfo.ClientSize.Width - 32);
                ResizeComboBorder(cb_loaikhach, wInfo);
                ResizeComboBorder(cb_khachhang, wInfo);
                ResizeComboBorder(cb_doituong, wInfo);

                int wPrice = Math.Max(120, cardPrice.ClientSize.Width - 32);
                txtdongia.Width = wPrice;
                txtsoluong.Width = wPrice;
                txtkhuyenmai.Width = wPrice;
                txttienKM.Width = wPrice;
                txtkhachdua.Width = wPrice;

                cb_hinhthuc.Width = Math.Max(120, cardPay.ClientSize.Width - (cardPay.ClientSize.Width / 2) - 16);
                cb_hinhthuc.Left = Math.Max(160, cardPay.ClientSize.Width / 2);

                keypadPanel.Width = keypadCard.ClientSize.Width - 32;

                int totalBlockH = groupBox3.Height + 18 + keypadPanel.Height;
                int free = keypadCard.ClientSize.Height - 68 - totalBlockH;
                int offsetY = Math.Max(0, free / 2);

                groupBox3.Left = 16;
                groupBox3.Top = 64 + offsetY;

                keypadPanel.Left = 16;
                keypadPanel.Top = groupBox3.Bottom + 18;

                // ✅ KIEU IN: đặt đúng vị trí vàng (bên phải groupBox3, combo nằm dưới label)
                int xKieuIn = groupBox3.Right + 24;
                int yKieuIn = groupBox3.Top + 6;

                _lblKieuIn.Location = new Point(xKieuIn, yKieuIn);

                int maxW = keypadCard.ClientSize.Width - xKieuIn - 16;
                int wKieuIn = Math.Max(160, Math.Min(320, maxW));

                int comboTop = _lblKieuIn.Bottom + 6;


                MoveComboBorder(cb_kieuin, keypadCard, xKieuIn, comboTop, wKieuIn);


                if (maxW < 170)
                {
                    _lblKieuIn.Location = new Point(16, groupBox3.Bottom + 10);
                    MoveComboBorder(cb_kieuin, keypadCard, 16, _lblKieuIn.Bottom + 6, keypadCard.ClientSize.Width - 32);
                }

                RelayoutUserFlow();
            }

            this.Resize += (s, e) => { RelayoutCards(); };

            ApplyModernStyles();
            RelayoutCards();
            EnableRecalcOnClickAnywhere();
            BeginInvoke(new Action(() => this.ActiveControl = null));
        }

        private void viewCartForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CounterCart();
        }

        private void ResetFormInput()
        {
            _ticketDangChon = null;
            txtdongia.Value = 0;
            txtkhachdua.Value = 0;
            txtkhuyenmai.Value = 0;
            txtsoluong.Value = 1;
            lblthanhtien.Text = "0";
            lbltongthanhtoan.Text = "0";
            lbltienthoi.Text = "0";
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

        private void lblCounterCartNum_Click(object sender, EventArgs e)
        {
            CartViewerForm viewCart = new CartViewerForm(lstItemCarts, _ticketOrderService);
            viewCart.ShowDialog();
        }

        private void lbltendangnhap_Click(object sender, EventArgs e)
        {
            MessageBox.Show("đăng xuất");
        }

        private void picus_Click(object sender, EventArgs e)
        {
            var rs = MessageBox.Show(
                "Bạn có chắc muốn đăng xuất hay không ?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (rs != DialogResult.Yes) return;

            try
            {
                ClearLoginFile();
                var context = new SQLAdoContext();
                IUserInfoService userInfoService = new UserInfoService(context);
                IProductService productService = new ProductService(context);
                ICustomerVIPService customerVIPService = new CustomerVIPService(context);
                var login = new LoginForm(userInfoService, productService, customerVIPService);
                this.Hide();
                login.FormClosed += (s, args) => this.Close();
                login.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể đăng xuất: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nutxemdon_Click(object sender, EventArgs e)
        {
            CartViewerForm viewCart = new CartViewerForm(lstItemCarts, _ticketOrderService);
            viewCart.FormClosed += viewCartForm_FormClosed;
            viewCart.ShowDialog();
        }

        public static void ClearLoginFile()
        {
            string loginFile = ConfigurationManager.AppSettings["LoginFile"];

            if (File.Exists(loginFile))
            {
                try
                {
                    File.WriteAllText(loginFile, string.Empty);
                }
                catch (IOException)
                {
                    throw;
                }
            }
        }

        private void GetListPricePolicy()
        {
            LstPricePolicy = _ticketService.GetListPricePolicy().data;
        }

        private decimal PriceSaleFromPolicy()
        {
            string ticketCode = _ticketDangChon.Code;
            string customerType = cb_loaikhach.SelectedValue != null ? cb_loaikhach.SelectedValue.ToString() : string.Empty;
            string doituong = cb_doituong.SelectedValue != null ? cb_doituong.SelectedValue.ToString() : string.Empty;

            var priceObject = LstPricePolicy.Where(x => x.TicketCode == ticketCode
            && x.CustomerType == customerType
            && x.CustomerForm == doituong).FirstOrDefault();

            return (priceObject != null ? priceObject.Price : 0);
        }
    }
}
