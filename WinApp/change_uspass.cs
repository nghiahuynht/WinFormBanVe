using GM_DAL;
using GM_DAL.IServices;
using GM_DAL.Services;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace WinApp
{
    public partial class change_uspass : Form
    {
        private IUserInfoService us_service;

        // Mấy service này bạn chưa dùng trong form, nhưng mình giữ lại đúng như bản gốc
        private IProductService _productService;
        public change_uspass()
        {
            InitializeComponent();

            // Cho vẽ gradient mượt
            this.DoubleBuffered = true;

            // Khởi tạo service như bản cũ
            var context = new SQLAdoContext();
            us_service = new UserInfoService(context);

            txtmatkhaucu.UseSystemPasswordChar = true;
            txtmatkhaumoi.UseSystemPasswordChar = true;
            cb_hienthi.Checked = false;
        }

        // Thuộc tính nhận username từ Form1
        public string giatriUS
        {
            get { return txttentaikhoan.Text; }
            set { txttentaikhoan.Text = value; }
        }

        // NỀN FORM: gradient tím giống Form1
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

        // ========== LOGIC ĐỔI MẬT KHẨU (giữ nguyên bản gốc) ==========
        private async void btnSaveProductDetail_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtmatkhaucu.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị mật khẩu cũ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtmatkhaumoi.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị mật khẩu mới!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!cb_robot.Checked)
            {
                MessageBox.Show("Xác nhận bạn không phải là robot", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string ten = txttentaikhoan.Text.Trim();
            string mkc = txtmatkhaucu.Text.Trim();
            string mkm = txtmatkhaumoi.Text.Trim();

            try
            {
                // Gọi service đổi mật khẩu
                var res = await us_service.change_pass(ten, mkc, mkm);

                if (res != null && res.data != null)
                {
                    string msg = res.data.message;
                    decimal val = res.data.value;   // value là decimal

                    if (val == 1)   // hoặc: if (val > 0)
                    {
                        // Đổi mật khẩu thành công
                        MessageBox.Show(msg, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var loginForm = getLoginForm();

                        loginForm.Show();
                        this.Close();   // nếu muốn đóng form

                    }
                    else
                    {
                        // Sai mật khẩu cũ hoặc lỗi nghiệp vụ khác
                        MessageBox.Show(msg, "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (res != null && !string.IsNullOrEmpty(res.message.exMessage))
                {
                    // Lỗi exception ở tầng DAL
                    MessageBox.Show("Lỗi hệ thống: " + res.message.exMessage, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Không nhận được phản hồi từ server.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ngoại lệ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hiển thị / ẩn mật khẩu (giữ nguyên bản gốc)
        private void cb_hienthi_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_hienthi.Checked)
            {
                txtmatkhaucu.UseSystemPasswordChar = false;
                txtmatkhaumoi.UseSystemPasswordChar = false;
            }
            else
            {
                txtmatkhaucu.UseSystemPasswordChar = true;
                txtmatkhaumoi.UseSystemPasswordChar = true;
            }
        }

        // Xóa trắng file login
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


        private void button1_Click(object sender, EventArgs e)
        {
            ClearLoginFile();
           
            
            var loginForm = getLoginForm();
            loginForm.Show();
            CloseAllFormsExcept(loginForm);

            //var f = this.FindForm();
            //if (f != null)
            //{
            //    f.BeginInvoke(new Action(() => Application.Restart()));
            //}
            //else
            //{
            //    // fallback
            //    var t = new System.Windows.Forms.Timer();
            //    t.Interval = 10;
            //    t.Tick += (s, e2) => { t.Stop(); t.Dispose(); Application.Restart(); };
            //    t.Start();
            //}
        }

      

        public static void CloseAllFormsExcept(Form formToKeepOpen)
        {
            // Tạo bản sao tĩnh của danh sách form
            List<Form>openForms = Application.OpenForms.Cast<Form>().ToList();
    
            foreach (Form f in openForms)
            {
                if (f.Name != formToKeepOpen.Name)
                {
                    f.Close();
                }
               
            }
        }


       private Form getLoginForm()
       {
            var context = new SQLAdoContext();
            var userInfoService = new UserInfoService(context);
            var productService = new ProductService(context);
            var customerVipService = new CustomerVIPService(context);
            var login = new LoginForm(
                userInfoService,
                productService,
                customerVipService
            );
            return login;
       }


       
    }
}
