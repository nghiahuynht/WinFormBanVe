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
        }

        // NỀN FORM: gradient tím giống Form1 / change_uspass
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
                       Color.FromArgb(39, 48, 85),   // trên: xanh đậm
                       Color.FromArgb(115, 74, 155), // dưới: tím
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
    }
}
