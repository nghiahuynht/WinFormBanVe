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
            panelCard = new System.Windows.Forms.Panel();
            lblLoginResult = new System.Windows.Forms.Label();
            btnLogin = new System.Windows.Forms.Button();
            txtPassword = new System.Windows.Forms.TextBox();
            txtLoginName = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            panelHeader = new System.Windows.Forms.Panel();
            lblTitle = new System.Windows.Forms.Label();
            panelCard.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelCard
            // 
            panelCard.BackColor = System.Drawing.Color.White;
            panelCard.Controls.Add(lblLoginResult);
            panelCard.Controls.Add(btnLogin);
            panelCard.Controls.Add(txtPassword);
            panelCard.Controls.Add(txtLoginName);
            panelCard.Controls.Add(label2);
            panelCard.Controls.Add(label1);
            panelCard.Controls.Add(panelHeader);
            panelCard.Location = new System.Drawing.Point(90, 60);
            panelCard.Name = "panelCard";
            panelCard.Size = new System.Drawing.Size(560, 340);
            panelCard.TabIndex = 0;
            // 
            // lblLoginResult
            // 
            lblLoginResult.AutoSize = true;
            lblLoginResult.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            lblLoginResult.ForeColor = System.Drawing.Color.Firebrick;
            lblLoginResult.Location = new System.Drawing.Point(46, 254);
            lblLoginResult.Name = "lblLoginResult";
            lblLoginResult.Size = new System.Drawing.Size(0, 20);
            lblLoginResult.TabIndex = 6;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnLogin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.Location = new System.Drawing.Point(46, 284);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(468, 42);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtPassword.Location = new System.Drawing.Point(46, 206);
            txtPassword.MaxLength = 50;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new System.Drawing.Size(468, 32);
            txtPassword.TabIndex = 2;
            txtPassword.Text = "123";
            // 
            // txtLoginName
            // 
            txtLoginName.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            txtLoginName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            txtLoginName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtLoginName.Location = new System.Drawing.Point(46, 128);
            txtLoginName.MaxLength = 50;
            txtLoginName.Name = "txtLoginName";
            txtLoginName.Size = new System.Drawing.Size(468, 32);
            txtLoginName.TabIndex = 1;
            txtLoginName.Text = "nghiahotro";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label2.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            label2.Location = new System.Drawing.Point(46, 182);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(90, 20);
            label2.TabIndex = 0;
            label2.Text = "MẬT KHẨU";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label1.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            label1.Location = new System.Drawing.Point(46, 104);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(134, 20);
            label1.TabIndex = 0;
            label1.Text = "TÊN ĐĂNG NHẬP";
            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.FromArgb(29, 78, 216);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(560, 72);
            panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Location = new System.Drawing.Point(44, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(246, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "  HỆ THỐNG BÁN VÉ KDL LANGBIAN";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(246, 248, 252);
            ClientSize = new System.Drawing.Size(740, 460);
            Controls.Add(panelCard);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
