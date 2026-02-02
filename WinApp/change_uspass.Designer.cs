namespace WinApp
{
    partial class change_uspass
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
            this.panelCard = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSaveProductDetail = new System.Windows.Forms.Button();
            this.cb_hienthi = new System.Windows.Forms.CheckBox();
            this.cb_robot = new System.Windows.Forms.CheckBox();
            this.txtmatkhaumoi = new System.Windows.Forms.TextBox();
            this.txtmatkhaucu = new System.Windows.Forms.TextBox();
            this.txttentaikhoan = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelCard.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.White;
            this.panelCard.Controls.Add(this.button1);
            this.panelCard.Controls.Add(this.btnSaveProductDetail);
            this.panelCard.Controls.Add(this.cb_hienthi);
            this.panelCard.Controls.Add(this.cb_robot);
            this.panelCard.Controls.Add(this.txtmatkhaumoi);
            this.panelCard.Controls.Add(this.txtmatkhaucu);
            this.panelCard.Controls.Add(this.txttentaikhoan);
            this.panelCard.Controls.Add(this.label3);
            this.panelCard.Controls.Add(this.label2);
            this.panelCard.Controls.Add(this.label1);
            this.panelCard.Controls.Add(this.panelHeader);
            this.panelCard.Location = new System.Drawing.Point(20, 20);
            this.panelCard.Name = "panelCard";
            this.panelCard.Size = new System.Drawing.Size(560, 330);
            this.panelCard.TabIndex = 0;
            // 
            // button1 (THOÁT)
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(55, 71, 79);
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(302, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 34);
            this.button1.TabIndex = 8;
            this.button1.Text = "THOÁT";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSaveProductDetail (ĐỔI MẬT KHẨU)
            // 
            this.btnSaveProductDetail.BackColor = System.Drawing.Color.FromArgb(33, 150, 243);
            this.btnSaveProductDetail.FlatAppearance.BorderSize = 0;
            this.btnSaveProductDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveProductDetail.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSaveProductDetail.ForeColor = System.Drawing.Color.White;
            this.btnSaveProductDetail.Location = new System.Drawing.Point(147, 280);
            this.btnSaveProductDetail.Name = "btnSaveProductDetail";
            this.btnSaveProductDetail.Size = new System.Drawing.Size(140, 34);
            this.btnSaveProductDetail.TabIndex = 7;
            this.btnSaveProductDetail.Text = "ĐỔI MẬT KHẨU";
            this.btnSaveProductDetail.UseVisualStyleBackColor = false;
            this.btnSaveProductDetail.Click += new System.EventHandler(this.btnSaveProductDetail_Click);
            // 
            // cb_hienthi
            // 
            this.cb_hienthi.AutoSize = true;
            this.cb_hienthi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cb_hienthi.ForeColor = System.Drawing.Color.FromArgb(55, 71, 79);
            this.cb_hienthi.Location = new System.Drawing.Point(410, 240);
            this.cb_hienthi.Name = "cb_hienthi";
            this.cb_hienthi.Size = new System.Drawing.Size(86, 24);
            this.cb_hienthi.TabIndex = 6;
            this.cb_hienthi.Text = "HIỂN THỊ";
            this.cb_hienthi.UseVisualStyleBackColor = true;
            this.cb_hienthi.CheckedChanged += new System.EventHandler(this.cb_hienthi_CheckedChanged);
            // 
            // cb_robot
            // 
            this.cb_robot.AutoSize = true;
            this.cb_robot.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cb_robot.ForeColor = System.Drawing.Color.FromArgb(55, 71, 79);
            this.cb_robot.Location = new System.Drawing.Point(40, 240);
            this.cb_robot.Name = "cb_robot";
            this.cb_robot.Size = new System.Drawing.Size(214, 24);
            this.cb_robot.TabIndex = 5;
            this.cb_robot.Text = "TÔI KHÔNG PHẢI LÀ ROBOT";
            this.cb_robot.UseVisualStyleBackColor = true;
            // 
            // txtmatkhaumoi
            // 
            this.txtmatkhaumoi.BackColor = System.Drawing.Color.FromArgb(246, 248, 252);
            this.txtmatkhaumoi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtmatkhaumoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtmatkhaumoi.Location = new System.Drawing.Point(40, 205);
            this.txtmatkhaumoi.MaxLength = 50;
            this.txtmatkhaumoi.Name = "txtmatkhaumoi";
            this.txtmatkhaumoi.Size = new System.Drawing.Size(456, 30);
            this.txtmatkhaumoi.TabIndex = 4;
            this.txtmatkhaumoi.UseSystemPasswordChar = true;
            // 
            // txtmatkhaucu
            // 
            this.txtmatkhaucu.BackColor = System.Drawing.Color.FromArgb(246, 248, 252);
            this.txtmatkhaucu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtmatkhaucu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtmatkhaucu.Location = new System.Drawing.Point(40, 145);
            this.txtmatkhaucu.MaxLength = 50;
            this.txtmatkhaucu.Name = "txtmatkhaucu";
            this.txtmatkhaucu.Size = new System.Drawing.Size(456, 30);
            this.txtmatkhaucu.TabIndex = 3;
            this.txtmatkhaucu.UseSystemPasswordChar = true;
            // 
            // txttentaikhoan
            // 
            this.txttentaikhoan.AutoSize = true;
            this.txttentaikhoan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txttentaikhoan.ForeColor = System.Drawing.Color.FromArgb(38, 50, 56);
            this.txttentaikhoan.Location = new System.Drawing.Point(190, 75);
            this.txttentaikhoan.Name = "txttentaikhoan";
            this.txttentaikhoan.Size = new System.Drawing.Size(58, 23);
            this.txttentaikhoan.TabIndex = 2;
            this.txttentaikhoan.Text = "admin";
            // 
            // label3 (TÊN TÀI KHOẢN)
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(144, 164, 174);
            this.label3.Location = new System.Drawing.Point(40, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "TÊN TÀI KHOẢN";
            // 
            // label2 (MẬT KHẨU MỚI)
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(189, 189, 189);
            this.label2.Location = new System.Drawing.Point(40, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "MẬT KHẨU MỚI";
            // 
            // label1 (MẬT KHẨU CŨ)
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(189, 189, 189);
            this.label1.Location = new System.Drawing.Point(40, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "MẬT KHẨU CŨ";
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(39, 48, 85);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(560, 50);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(150, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(258, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "ĐỔI MẬT KHẨU TÀI KHOẢN";
            // 
            // change_uspass (Form)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(242, 243, 247);
            this.ClientSize = new System.Drawing.Size(600, 370);
            this.Controls.Add(this.panelCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "change_uspass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ĐỔI MẬT KHẨU TÀI KHOẢN";
            this.panelCard.ResumeLayout(false);
            this.panelCard.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCard;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSaveProductDetail;
        private System.Windows.Forms.CheckBox cb_hienthi;
        private System.Windows.Forms.CheckBox cb_robot;
        private System.Windows.Forms.TextBox txtmatkhaumoi;
        private System.Windows.Forms.TextBox txtmatkhaucu;
        private System.Windows.Forms.Label txttentaikhoan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
    }
}
