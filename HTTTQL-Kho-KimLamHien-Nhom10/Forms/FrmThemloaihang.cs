using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.BLL;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmThemloaihang : Form
    {
        private LoaihangBLL bll = new LoaihangBLL();

        // Biến trả về cho Form Thêm Hàng Hóa nhận diện
        public string MaLoaiMoi = "";
        public string TenLoaiMoi = "";

        public FrmThemloaihang()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
        }

        private void FrmThemloaihang_Load(object sender, EventArgs e)
        {
            try
            {
                string logoPath = Path.Combine(Application.StartupPath, "Images", "Giaodien", "logo1.ico");
                if (File.Exists(logoPath))
                {
                    pic1.Image = Image.FromFile(logoPath);
                }
            }
            catch { }
            pic1.SizeMode = PictureBoxSizeMode.Zoom;

            // Tự động sinh mã loại mới (VD: LH01, LH02...)
            txtMaloai.Text = bll.SinhMaMoi();
            txtTenloai.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenLoai = txtTenloai.Text.Trim();

            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(tenLoai))
            {
                MessageBox.Show("Vui lòng nhập tên loại hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenloai.Focus();
                return;
            }

            // Gọi qua BLL thực hiện kiểm tra trùng và thêm vào bảng loai_hang
            string error = bll.ThemLoaiHang(tenLoai);
            if (error != null)
            {
                MessageBox.Show(error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenloai.Focus();
                return;
            }

            // Gán dữ liệu trả về cho form cha (nếu có)
            MaLoaiMoi = txtMaloai.Text.Trim();
            TenLoaiMoi = tenLoai;

            MessageBox.Show("Thêm loại hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTenloai.Text.Trim()))
            {
                DialogResult dr = MessageBox.Show("Dữ liệu chưa được lưu. Bạn có muốn hủy bỏ không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}