namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    partial class FrmThemloaihang
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblMaLoai = new System.Windows.Forms.Label();
            this.lblTenLoai = new System.Windows.Forms.Label();
            this.txtMaloai = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTenloai = new Guna.UI2.WinForms.Guna2TextBox();
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
            this.guna2Panel3.Size = new System.Drawing.Size(800, 75);
            this.guna2Panel3.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Goldenrod;
            this.label3.Location = new System.Drawing.Point(95, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(277, 41);
            this.label3.TabIndex = 8;
            this.label3.Text = "THÊM LOẠI HÀNG";
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
            this.guna2Panel2.Controls.Add(this.label1);
            this.guna2Panel2.Controls.Add(this.lblMaLoai);
            this.guna2Panel2.Controls.Add(this.lblTenLoai);
            this.guna2Panel2.Controls.Add(this.txtMaloai);
            this.guna2Panel2.Controls.Add(this.txtTenloai);
            this.guna2Panel2.Location = new System.Drawing.Point(20, 95);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(754, 250);
            this.guna2Panel2.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label1.Location = new System.Drawing.Point(20, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "THÔNG TIN LOẠI HÀNG";
            // 
            // lblMaLoai
            // 
            this.lblMaLoai.AutoSize = true;
            this.lblMaLoai.BackColor = System.Drawing.Color.Transparent;
            this.lblMaLoai.Font = new System.Drawing.Font("Segoe UI", 11.2F, System.Drawing.FontStyle.Bold);
            this.lblMaLoai.ForeColor = System.Drawing.Color.Black;
            this.lblMaLoai.Location = new System.Drawing.Point(50, 68);
            this.lblMaLoai.Name = "lblMaLoai";
            this.lblMaLoai.Size = new System.Drawing.Size(132, 25);
            this.lblMaLoai.TabIndex = 0;
            this.lblMaLoai.Text = "Mã loại hàng:";
            // 
            // lblTenLoai
            // 
            this.lblTenLoai.AutoSize = true;
            this.lblTenLoai.Font = new System.Drawing.Font("Segoe UI", 11.2F, System.Drawing.FontStyle.Bold);
            this.lblTenLoai.ForeColor = System.Drawing.Color.Black;
            this.lblTenLoai.Location = new System.Drawing.Point(50, 155);
            this.lblTenLoai.Name = "lblTenLoai";
            this.lblTenLoai.Size = new System.Drawing.Size(144, 25);
            this.lblTenLoai.TabIndex = 2;
            this.lblTenLoai.Text = "Tên loại hàng*:";
            // 
            // txtMaloai
            // 
            this.txtMaloai.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaloai.BorderColor = System.Drawing.Color.LightGray;
            this.txtMaloai.BorderRadius = 6;
            this.txtMaloai.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaloai.DefaultText = "";
            this.txtMaloai.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(233)))), ((int)(((byte)(237)))));
            this.txtMaloai.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtMaloai.ForeColor = System.Drawing.Color.Black;
            this.txtMaloai.Location = new System.Drawing.Point(200, 58);
            this.txtMaloai.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMaloai.Name = "txtMaloai";
            this.txtMaloai.PlaceholderText = "";
            this.txtMaloai.ReadOnly = true;
            this.txtMaloai.SelectedText = "";
            this.txtMaloai.Size = new System.Drawing.Size(520, 45);
            this.txtMaloai.TabIndex = 0;
            // 
            // txtTenloai
            // 
            this.txtTenloai.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTenloai.BorderColor = System.Drawing.Color.DarkGoldenrod;
            this.txtTenloai.BorderRadius = 6;
            this.txtTenloai.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenloai.DefaultText = "";
            this.txtTenloai.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtTenloai.ForeColor = System.Drawing.Color.Black;
            this.txtTenloai.Location = new System.Drawing.Point(200, 145);
            this.txtTenloai.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTenloai.Name = "txtTenloai";
            this.txtTenloai.PlaceholderText = "VD: Vàng 24K, Vàng 9999, Bạc S925...";
            this.txtTenloai.SelectedText = "";
            this.txtTenloai.Size = new System.Drawing.Size(520, 45);
            this.txtTenloai.TabIndex = 1;
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
            this.btnLuu.Location = new System.Drawing.Point(604, 375);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(170, 45);
            this.btnLuu.TabIndex = 15;
            this.btnLuu.Text = "Lưu";
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
            this.btnHuy.Location = new System.Drawing.Point(400, 375);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(170, 45);
            this.btnHuy.TabIndex = 14;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // FrmThemloaihang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.ClientSize = new System.Drawing.Size(800, 445);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel3);
            this.Name = "FrmThemloaihang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "THÊM LOẠI HÀNG";
            this.Load += new System.EventHandler(this.FrmThemloaihang_Load);
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
        private System.Windows.Forms.Label lblMaLoai;
        private System.Windows.Forms.Label lblTenLoai;
        private Guna.UI2.WinForms.Guna2TextBox txtMaloai;
        private Guna.UI2.WinForms.Guna2TextBox txtTenloai;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
    }
}