using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmThemkhachhang : Form
    {
        // Tạo đối tượng BLL
        BLL.KhachhangBLL bll = new BLL.KhachhangBLL();

        // Biến trả về cho Form Hóa đơn bán hàng
        public string MaKHMoi = "";
        public string TenKHMoi = "";
        public string PreFilledSDT = "";

        public string GetTenKH() => txtTenKH.Text.Trim();
        public string GetSDT() => mskSDT.Text.Trim();
        public string GetDiachi() => txtDiachi.Text.Trim();

        public FrmThemkhachhang()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
        }

        private void FrmThemkhachhang_Load(object sender, EventArgs e)
        {
            // Tải ảnh an toàn (tránh văng lỗi nếu thiếu file ảnh ngoài đĩa)
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

            if (!string.IsNullOrEmpty(PreFilledSDT))
            {
                mskSDT.Text = PreFilledSDT;
            }

            txtTenKH.Focus();
        }

        private void KhoaTatCa(bool khoa)
        {
            txtTenKH.ReadOnly = khoa;
            mskSDT.ReadOnly = khoa;
            txtDiachi.ReadOnly = khoa;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenKH = txtTenKH.Text.Trim();
            string sdt = mskSDT.Text.Trim();
            string diachi = txtDiachi.Text.Trim();

            // Gọi qua BLL để thực hiện validate và Insert vào bảng khach_hang
            string loi = bll.ThemKhachHang(tenKH, sdt, diachi);
            if (loi != null)
            {
                MessageBox.Show(loi, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // GÁN GIÁ TRỊ TRẢ VỀ CHO FORM HÓA ĐƠN
            MaKHMoi = bll.MaVuaTao;
            TenKHMoi = tenKH;

            MessageBox.Show("Thêm khách hàng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            KhoaTatCa(true);
            btnLuu.Enabled = false;

            // Đặt DialogResult = OK để form cha tự động reload lại danh sách / combobox
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool IsDataChanged()
        {
            if (!string.IsNullOrEmpty(txtTenKH.Text.Trim())) return true;

            string rawSDT = mskSDT.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
            if (!string.IsNullOrEmpty(rawSDT)) return true;

            if (!string.IsNullOrEmpty(txtDiachi.Text.Trim())) return true;

            return false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (!IsDataChanged())
            {
                this.Close();
                return;
            }

            if (MessageBox.Show("Dữ liệu đã nhập chưa được lưu. Bạn có chắc chắn muốn hủy không?",
                                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                resetvalue();
                this.Close();
            }
        }

        private void resetvalue()
        {
            txtTenKH.Text = "";
            mskSDT.Text = "";
            txtDiachi.Text = "";
        }
    }
}