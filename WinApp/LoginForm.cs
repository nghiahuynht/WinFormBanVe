using GM_DAL.IServices;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace WinApp
{
    public partial class LoginForm : Form
    {
        private IUserInfoService _userInfoService;
        private IProductService _productService;

        // ✅ THÊM: service khách VIP để truyền qua Form1
        private ICustomerVIPService _customerVIPService;

        string loginFile = ConfigurationManager.AppSettings["LoginFile"].ToString();

        public LoginForm(
            IUserInfoService userInfoService,
            IProductService productService,
            ICustomerVIPService customerVIPService
        )
        {
            InitializeComponent();
            txtPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;   // khỏi kêu "ding"
                    btnLogin_Click(btnLogin, EventArgs.Empty);
                }
            };
            this.DoubleBuffered = true; // vẽ mượt

            _userInfoService = userInfoService;
            _productService = productService;
            _customerVIPService = customerVIPService;

            if (!File.Exists(loginFile))
            {
                File.Create(loginFile).Dispose();
            }

            txtPassword.UseSystemPasswordChar = true;

            // ✅ CHỈ GIAO DIỆN
            ApplyBlueModernUI();
        }

        // ✅ NỀN FORM: đổi sang xanh hài hòa (UI thôi, không đụng logic)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Rectangle rect = this.ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                base.OnPaintBackground(e);
                return;
            }

            using (var brush = new LinearGradientBrush(
                       rect,
                       Color.FromArgb(246, 248, 252), // nền sáng xanh nhạt
                       Color.FromArgb(226, 232, 240), // nền đậm hơn nhẹ
                       LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
        }

        // ====== LOGIC CŨ: NÚT ĐĂNG NHẬP (GIỮ NGUYÊN) ======
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtLoginName.Text;
            string password = txtPassword.Text;
            string loginAlert = "ok";

            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
            {
                loginAlert = "Tên đăng nhập và mật khẩu không thể trống";
            }
            else
            {
                var loginRes = await _userInfoService.Login(userName, password);
                if (!string.IsNullOrEmpty(loginRes.message.exMessage))
                {
                    loginAlert = "Đăng nhập lỗi: " + loginRes.message.exMessage;
                }
                else
                {
                    if (loginRes.data == null)
                    {
                        loginAlert = "Sai tên đăng nhập hoặc mật khẩu";
                    }
                    else
                    {
                        using (StreamWriter writetext = new StreamWriter(loginFile))
                        {
                            string jsonString = JsonConvert.SerializeObject(loginRes.data);
                            writetext.WriteLine(jsonString);
                            writetext.Close();
                            writetext.Dispose();

                            this.Hide(); // tắt form login, mở form bán vé
                            FormBanVe formBan = new FormBanVe();
                            formBan.ShowDialog();
                        }
                    }
                }
            }

            lblLoginResult.Text = loginAlert;
        }

        // ================= UI ONLY =================
        private void ApplyBlueModernUI()
        {
            // Palette xanh giống FormBanVe
            Color bg = Color.FromArgb(246, 248, 252);
            Color cardBorder = Color.FromArgb(226, 232, 240);
            Color muted = Color.FromArgb(100, 116, 139);
            Color text = Color.FromArgb(15, 23, 42);
            Color blue1 = Color.FromArgb(37, 99, 235);
            Color blue2 = Color.FromArgb(29, 78, 216);
            Color inputBg = Color.FromArgb(248, 250, 252);

            this.BackColor = bg;

            // label
            label1.ForeColor = muted;
            label2.ForeColor = muted;
            lblLoginResult.ForeColor = Color.Firebrick;

            // textbox
            txtLoginName.BackColor = inputBg;
            txtPassword.BackColor = inputBg;
            txtLoginName.ForeColor = text;
            txtPassword.ForeColor = text;

            txtLoginName.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;

            // button
            btnLogin.BackColor = blue1;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.ForeColor = Color.White;

            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = blue2;
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = blue1;
            btnLogin.MouseDown += (s, e) => btnLogin.BackColor = Color.FromArgb(30, 64, 175);
            btnLogin.MouseUp += (s, e) => btnLogin.BackColor = blue2;

            // header gradient (UI)
            panelHeader.BackColor = Color.Transparent;
            panelHeader.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var br = new LinearGradientBrush(panelHeader.ClientRectangle, blue2, blue1, LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(br, panelHeader.ClientRectangle);
            };
            lblTitle.ForeColor = Color.White;

            // card bo góc + bóng nhẹ (UI)
            panelCard.BackColor = Color.Transparent;
            panelCard.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new Rectangle(0, 0, panelCard.Width - 1, panelCard.Height - 1);
                int radius = 18;

                using var path = RoundedRect(rect, radius);

                // shadow
                using (var sh = new SolidBrush(Color.FromArgb(22, 0, 0, 0)))
                {
                    var shadowRect = new Rectangle(4, 6, panelCard.Width - 8, panelCard.Height - 8);
                    using var shadowPath = RoundedRect(shadowRect, radius);
                    e.Graphics.FillPath(sh, shadowPath);
                }

                using (var bgBrush = new SolidBrush(Color.White))
                    e.Graphics.FillPath(bgBrush, path);

                using (var pen = new Pen(cardBorder, 1f))
                    e.Graphics.DrawPath(pen, path);
            };

            // bo góc cho panelCard bằng Region để click/hiển thị đúng
            panelCard.SizeChanged += (s, e) =>
            {
                using var gp = new GraphicsPath();
                int r = 18;
                var rr = new Rectangle(0, 0, panelCard.Width, panelCard.Height);
                int d = r * 2;
                gp.AddArc(rr.X, rr.Y, d, d, 180, 90);
                gp.AddArc(rr.Right - d, rr.Y, d, d, 270, 90);
                gp.AddArc(rr.Right - d, rr.Bottom - d, d, d, 0, 90);
                gp.AddArc(rr.X, rr.Bottom - d, d, d, 90, 90);
                gp.CloseFigure();
                panelCard.Region = new Region(gp);
            };

            // center card đẹp hơn theo form size (UI)
            void CenterCard()
            {
                int x = (this.ClientSize.Width - panelCard.Width) / 2;
                int y = (this.ClientSize.Height - panelCard.Height) / 2;
                if (x < 12) x = 12;
                if (y < 12) y = 12;
                panelCard.Location = new Point(x, y);
            }
            this.Resize += (s, e) => CenterCard();
            this.Shown += (s, e) => CenterCard();
            CenterCard();
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
}
