namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    partial class FrmTaoyeucaukiemke
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
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.pic1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.cboKho = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboTenNV = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpThoigian = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblChiTiet = new System.Windows.Forms.Label();
            this.txtChiTiet = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).BeginInit();
            this.guna2Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BorderRadius = 15;
            this.guna2Panel3.Controls.Add(this.label3);
            this.guna2Panel3.Controls.Add(this.pic1);
            this.guna2Panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel3.FillColor = System.Drawing.Color.Black;
            this.guna2Panel3.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(860, 75);
            this.guna2Panel3.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Goldenrod;
            this.label3.Location = new System.Drawing.Point(95, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(339, 41);
            this.label3.TabIndex = 8;
            this.label3.Text = "TẠO YÊU CẦU KIỂM KÊ";
            // 
            // pic1
            // 
            this.pic1.BackColor = System.Drawing.Color.Transparent;
            this.pic1.Image = global::HTTTQL_Kho_KimLamHien_Nhom10.Properties.Resources.login3;
            this.pic1.ImageRotate = 0F;
            this.pic1.Location = new System.Drawing.Point(15, 8);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(65, 60);
            this.pic1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pic1.TabIndex = 7;
            this.pic1.TabStop = false;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Panel2.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.guna2Panel2.BorderRadius = 8;
            this.guna2Panel2.BorderThickness = 1;
            this.guna2Panel2.Controls.Add(this.label4);
            this.guna2Panel2.Controls.Add(this.cboKho);
            this.guna2Panel2.Controls.Add(this.label2);
            this.guna2Panel2.Controls.Add(this.cboTenNV);
            this.guna2Panel2.Controls.Add(this.label8);
            this.guna2Panel2.Controls.Add(this.dtpThoigian);
            this.guna2Panel2.Controls.Add(this.label1);
            this.guna2Panel2.Controls.Add(this.lblChiTiet);
            this.guna2Panel2.Controls.Add(this.txtChiTiet);
            this.guna2Panel2.Location = new System.Drawing.Point(12, 95);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(836, 350);
            this.guna2Panel2.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(30, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 25);
            this.label4.TabIndex = 28;
            this.label4.Text = "Chọn kho kiểm*:";
            // 
            // cboKho
            // 
            this.cboKho.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboKho.BackColor = System.Drawing.Color.Transparent;
            this.cboKho.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.cboKho.BorderRadius = 6;
            this.cboKho.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboKho.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKho.FocusedColor = System.Drawing.Color.Empty;
            this.cboKho.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.cboKho.ForeColor = System.Drawing.Color.Black;
            this.cboKho.ItemHeight = 30;
            this.cboKho.Location = new System.Drawing.Point(220, 188);
            this.cboKho.Name = "cboKho";
            this.cboKho.Size = new System.Drawing.Size(580, 36);
            this.cboKho.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(30, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 25);
            this.label2.TabIndex = 26;
            this.label2.Text = "Người lập yêu cầu*:";
            // 
            // cboTenNV
            // 
            this.cboTenNV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTenNV.BackColor = System.Drawing.Color.Transparent;
            this.cboTenNV.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.cboTenNV.BorderRadius = 6;
            this.cboTenNV.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboTenNV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTenNV.FocusedColor = System.Drawing.Color.Empty;
            this.cboTenNV.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.cboTenNV.ForeColor = System.Drawing.Color.Black;
            this.cboTenNV.ItemHeight = 30;
            this.cboTenNV.Location = new System.Drawing.Point(220, 120);
            this.cboTenNV.Name = "cboTenNV";
            this.cboTenNV.Size = new System.Drawing.Size(580, 36);
            this.cboTenNV.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(30, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(146, 25);
            this.label8.TabIndex = 17;
            this.label8.Text = "Ngày yêu cầu*:";
            // 
            // dtpThoigian
            // 
            this.dtpThoigian.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.dtpThoigian.BorderRadius = 6;
            this.dtpThoigian.BorderThickness = 1;
            this.dtpThoigian.Checked = true;
            this.dtpThoigian.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpThoigian.FillColor = System.Drawing.Color.White;
            this.dtpThoigian.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.dtpThoigian.ForeColor = System.Drawing.Color.Black;
            this.dtpThoigian.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpThoigian.Location = new System.Drawing.Point(220, 52);
            this.dtpThoigian.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpThoigian.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpThoigian.Name = "dtpThoigian";
            this.dtpThoigian.Size = new System.Drawing.Size(260, 42);
            this.dtpThoigian.TabIndex = 0;
            this.dtpThoigian.Value = new System.DateTime(2026, 9, 27, 2, 6, 15, 693);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label1.Location = new System.Drawing.Point(20, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "THÔNG TIN YÊU CẦU";
            // 
            // lblChiTiet
            // 
            this.lblChiTiet.AutoSize = true;
            this.lblChiTiet.BackColor = System.Drawing.Color.Transparent;
            this.lblChiTiet.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblChiTiet.ForeColor = System.Drawing.Color.Black;
            this.lblChiTiet.Location = new System.Drawing.Point(30, 260);
            this.lblChiTiet.Name = "lblChiTiet";
            this.lblChiTiet.Size = new System.Drawing.Size(157, 25);
            this.lblChiTiet.TabIndex = 0;
            this.lblChiTiet.Text = "Ghi chú yêu cầu:";
            // 
            // txtChiTiet
            // 
            this.txtChiTiet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChiTiet.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.txtChiTiet.BorderRadius = 6;
            this.txtChiTiet.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtChiTiet.DefaultText = "";
            this.txtChiTiet.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtChiTiet.ForeColor = System.Drawing.Color.Black;
            this.txtChiTiet.Location = new System.Drawing.Point(220, 252);
            this.txtChiTiet.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtChiTiet.Multiline = true;
            this.txtChiTiet.Name = "txtChiTiet";
            this.txtChiTiet.PlaceholderText = "Nhập mục đích hoặc lưu ý kiểm kê (đột xuất / định kỳ)...";
            this.txtChiTiet.SelectedText = "";
            this.txtChiTiet.Size = new System.Drawing.Size(580, 75);
            this.txtChiTiet.TabIndex = 3;
            // 
            // btnLuu
            // 
            this.btnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuu.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnLuu.BorderRadius = 8;
            this.btnLuu.BorderThickness = 2;
            this.btnLuu.FillColor = System.Drawing.Color.DarkGoldenrod;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.HoverState.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnLuu.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.btnLuu.HoverState.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.btnLuu.Location = new System.Drawing.Point(470, 465);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(175, 45);
            this.btnLuu.TabIndex = 0;
            this.btnLuu.Text = "Gửi yêu cầu";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuy.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnHuy.BorderRadius = 8;
            this.btnHuy.BorderThickness = 2;
            this.btnHuy.FillColor = System.Drawing.Color.WhiteSmoke;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.btnHuy.HoverState.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.btnHuy.HoverState.FillColor = System.Drawing.Color.DarkGoldenrod;
            this.btnHuy.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(670, 465);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(175, 45);
            this.btnHuy.TabIndex = 1;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // FrmTaoyeucaukiemke
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.ClientSize = new System.Drawing.Size(860, 525);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel3);
            this.Name = "FrmTaoyeucaukiemke";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TẠO YÊU CẦU KIỂM KÊ";
            this.Load += new System.EventHandler(this.FrmTaoyeucaukiemke_Load);
            this.guna2Panel3.ResumeLayout(false);
            this.guna2Panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).EndInit();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2PictureBox pic1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblChiTiet;
        private Guna.UI2.WinForms.Guna2TextBox txtChiTiet;
        private System.Windows.Forms.Label label8;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpThoigian;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2ComboBox cboTenNV;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox cboKho;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
    }
}