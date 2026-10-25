using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmThemloaihang : Form
    {
        public string MaLoaiMoi = "LH01";
        public string TenLoaiMoi = "";

        public FrmThemloaihang()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
        }

        private void FrmThemloaihang_Load(object sender, EventArgs e)
        {
            string iconPath = Path.Combine(Application.StartupPath, "Images", "Giaodien", "logo1.ico");
            if (File.Exists(iconPath))
            {
                this.ShowIcon = true;
                this.Icon = new Icon(iconPath);

                using (var bmp = new Bitmap(iconPath))
                {
                    pic1.Image = new Bitmap(bmp);
                }
                pic1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            pic1.SizeMode = PictureBoxSizeMode.Zoom;

            txtMaloai.Text = "LH01";
            txtTenloai.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenLoai = txtTenloai.Text.Trim();
            if (string.IsNullOrEmpty(tenLoai))
            {
                MessageBox.Show("Vui lòng nhập tên loại hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenloai.Focus();
                return;
            }

            TenLoaiMoi = tenLoai;
            MessageBox.Show("Thêm loại hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}