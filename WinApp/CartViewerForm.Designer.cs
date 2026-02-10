namespace WinApp
{
    partial class CartViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridCartView = new DataGridView();
            label1 = new Label();
            lblTotalCart = new Label();
            label2 = new Label();
            label3 = new Label();
            lblTienThoi = new Label();
            txtTienKhachDua = new TextBox();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridCartView).BeginInit();
            SuspendLayout();
            // 
            // dataGridCartView
            // 
            dataGridCartView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridCartView.Location = new Point(22, 27);
            dataGridCartView.Name = "dataGridCartView";
            dataGridCartView.RowHeadersWidth = 51;
            dataGridCartView.RowTemplate.Height = 29;
            dataGridCartView.Size = new Size(1212, 258);
            dataGridCartView.TabIndex = 0;
            dataGridCartView.CellContentClick += dataGridCartView_CellContentClick;
            dataGridCartView.CellFormatting += dataGridCartView_CellFormatting;
            dataGridCartView.CellMouseEnter += dataGridCartView_CellMouseEnter;
            dataGridCartView.CellMouseLeave += dataGridCartView_CellMouseLeave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(22, 308);
            label1.Name = "label1";
            label1.Size = new Size(94, 20);
            label1.TabIndex = 1;
            label1.Text = "TỔNG TIỀN:";
            // 
            // lblTotalCart
            // 
            lblTotalCart.AutoSize = true;
            lblTotalCart.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblTotalCart.ForeColor = Color.Red;
            lblTotalCart.Location = new Point(122, 300);
            lblTotalCart.Name = "lblTotalCart";
            lblTotalCart.Size = new Size(27, 32);
            lblTotalCart.TabIndex = 2;
            lblTotalCart.Text = "0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(260, 309);
            label2.Name = "label2";
            label2.Size = new Size(99, 20);
            label2.TabIndex = 3;
            label2.Text = "KHÁCH ĐƯA";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(567, 311);
            label3.Name = "label3";
            label3.Size = new Size(87, 20);
            label3.TabIndex = 5;
            label3.Text = "TIỀN THỐI:";
            // 
            // lblTienThoi
            // 
            lblTienThoi.AutoSize = true;
            lblTienThoi.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblTienThoi.ForeColor = Color.FromArgb(64, 64, 64);
            lblTienThoi.Location = new Point(655, 305);
            lblTienThoi.Name = "lblTienThoi";
            lblTienThoi.Size = new Size(27, 32);
            lblTienThoi.TabIndex = 6;
            lblTienThoi.Text = "0";
            // 
            // txtTienKhachDua
            // 
            txtTienKhachDua.Location = new Point(365, 305);
            txtTienKhachDua.Name = "txtTienKhachDua";
            txtTienKhachDua.Size = new Size(175, 27);
            txtTienKhachDua.TabIndex = 7;
            txtTienKhachDua.Text = "0";
            txtTienKhachDua.MouseLeave += txtTienKhachDua_MouseLeave;
            // 
            // button1
            // 
            button1.Location = new Point(1140, 308);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 9;
            button1.Text = "IN tesst";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // CartViewerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(192, 255, 192);
            ClientSize = new Size(1258, 361);
            Controls.Add(button1);
            Controls.Add(txtTienKhachDua);
            Controls.Add(lblTienThoi);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblTotalCart);
            Controls.Add(label1);
            Controls.Add(dataGridCartView);
            Name = "CartViewerForm";
            Text = "DANH SÁCH ĐƠN ĐANG BÁN";
            Load += CartViewerForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridCartView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridCartView;
        private Label label1;
        private Label lblTotalCart;
        private Label label2;
        private Label label3;
        private Label lblTienThoi;
        private TextBox txtTienKhachDua;
        private Button button1;
    }
}