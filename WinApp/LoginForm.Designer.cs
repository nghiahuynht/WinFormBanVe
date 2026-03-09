namespace WinApp
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelCard = new Panel();
            lblLoginResult = new Label();
            btnLogin = new Button();
            txtPassword = new TextBox();
            txtLoginName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panelHeader = new Panel();
            lblTitle = new Label();
            panelCard.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.Controls.Add(lblLoginResult);
            panelCard.Controls.Add(btnLogin);
            panelCard.Controls.Add(txtPassword);
            panelCard.Controls.Add(txtLoginName);
            panelCard.Controls.Add(label2);
            panelCard.Controls.Add(label1);
            panelCard.Controls.Add(panelHeader);
            panelCard.Location = new Point(90, 60);
            panelCard.Name = "panelCard";
            panelCard.Size = new Size(560, 340);
            panelCard.TabIndex = 0;
            // 
            // lblLoginResult
            // 
            lblLoginResult.AutoSize = true;
            lblLoginResult.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
            lblLoginResult.ForeColor = Color.Firebrick;
            lblLoginResult.Location = new Point(46, 254);
            lblLoginResult.Name = "lblLoginResult";
            lblLoginResult.Size = new Size(0, 20);
            lblLoginResult.TabIndex = 6;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(37, 99, 235);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(46, 284);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(468, 42);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.FromArgb(248, 250, 252);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtPassword.Location = new Point(46, 206);
            txtPassword.MaxLength = 50;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(468, 32);
            txtPassword.TabIndex = 2;
            // 
            // txtLoginName
            // 
            txtLoginName.BackColor = Color.FromArgb(248, 250, 252);
            txtLoginName.BorderStyle = BorderStyle.FixedSingle;
            txtLoginName.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            txtLoginName.Location = new Point(46, 128);
            txtLoginName.MaxLength = 50;
            txtLoginName.Name = "txtLoginName";
            txtLoginName.Size = new Size(468, 32);
            txtLoginName.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(100, 116, 139);
            label2.Location = new Point(46, 182);
            label2.Name = "label2";
            label2.Size = new Size(90, 20);
            label2.TabIndex = 0;
            label2.Text = "MẬT KHẨU";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(100, 116, 139);
            label1.Location = new Point(46, 104);
            label1.Name = "label1";
            label1.Size = new Size(134, 20);
            label1.TabIndex = 0;
            label1.Text = "TÊN ĐĂNG NHẬP";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(29, 78, 216);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(560, 72);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(44, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(448, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "  HỆ THỐNG BÁN VÉ KDL LANGBIANG";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 248, 252);
            ClientSize = new Size(740, 460);
            Controls.Add(panelCard);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelCard;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLoginName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblLoginResult;
    }
}
