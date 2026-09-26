using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmThemNCC : Form
    {
        private string cheDo = "THEM";
        private string maNCCSua = "";

        private BLL.QuanlyNCCBLL bll = new BLL.QuanlyNCCBLL();

        // Biến trả về cho form Đề xuất nhập hàng
        public string MaNCCMoi = "";
        public string TenNCCMoi = "";

        public FrmThemNCC()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = "THEM";
        }

        public FrmThemNCC(string cheDo, string maNCC)
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = cheDo;
            this.maNCCSua = maNCC;
        }

        private void FrmThemNCC_Load(object sender, EventArgs e)
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
            lblCanhbao.Visible = false;

            if (cheDo == "SUA")
            {
                this.Text = "Sửa thông tin nhà cung cấp";
                label1.Text = "SỬA NHÀ CUNG CẤP";
                LoadThongTin();
            }
            else
            {
                this.Text = "Thêm mới nhà cung cấp";
                label1.Text = "THÊM NHÀ CUNG CẤP";
            }

            txtNCC.Focus();
        }

        private void txtNCC_Leave(object sender, EventArgs e)
        {
            string tenNCC = txtNCC.Text.Trim();
            if (tenNCC.Length == 0)
            {
                lblCanhbao.Visible = false;
                return;
            }

            if (cheDo == "SUA" && bll.KiemTraTenTrung(tenNCC, maNCCSua))
                lblCanhbao.Visible = true;
            else if (cheDo != "SUA" && bll.KiemTraTenTrung(tenNCC))
                lblCanhbao.Visible = true;
            else
                lblCanhbao.Visible = false;
        }

        private void txtNCC_TextChanged(object sender, EventArgs e)
        {
            lblCanhbao.Visible = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenNCC = txtNCC.Text.Trim();
            string diachi = txtDiachi.Text.Trim();

            // 1. Kiểm tra tên NCC
            if (string.IsNullOrEmpty(tenNCC))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNCC.Focus();
                return;
            }

            if (lblCanhbao.Visible)
            {
                MessageBox.Show("Tên nhà cung cấp đã tồn tại, vui lòng nhập tên khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNCC.Focus();
                return;
            }

            // 2. KIỂM TRA SỐ ĐIỆN THOẠI (Chặn ngay nếu chưa gõ đủ 10 số)
            string rawSDT = mskSDT.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "").Trim();

            if (string.IsNullOrEmpty(rawSDT))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSDT.Focus();
                return;
            }

            // MaskedTextBox có thuộc tính MaskFull (kiểm tra đã điền kín các dấu _ hay chưa)
            if (!mskSDT.MaskFull || rawSDT.Length < 10)
            {
                MessageBox.Show("Số điện thoại chưa nhập đủ 10 số. Vui lòng nhập đầy đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSDT.Focus();
                return;
            }

            // 3. Kiểm tra địa chỉ
            if (string.IsNullOrEmpty(diachi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiachi.Focus();
                return;
            }

            // Gọi BLL lưu dữ liệu
            string loi;
            if (cheDo == "SUA")
            {
                loi = bll.SuaNhaCungCap(maNCCSua, tenNCC, mskSDT.Text.Trim(), diachi);
            }
            else
            {
                loi = bll.ThemNhaCungCap(tenNCC, mskSDT.Text.Trim(), diachi);
            }

            if (loi != null)
            {
                MessageBox.Show(loi, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gán dữ liệu trả về và đóng form
            if (cheDo == "THEM")
            {
                MaNCCMoi = bll.MaVuaTao;
                TenNCCMoi = tenNCC;
                MessageBox.Show("Thêm nhà cung cấp mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MaNCCMoi = maNCCSua;
                TenNCCMoi = tenNCC;
                MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            KhoaTatCa(true);
            btnLuu.Enabled = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void KhoaTatCa(bool khoa)
        {
            txtNCC.ReadOnly = khoa;
            mskSDT.ReadOnly = khoa;
            txtDiachi.ReadOnly = khoa;
        }

        private bool IsDataChanged()
        {
            if (txtNCC.Text.Trim() != "") return true;
            string rawSDT = mskSDT.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
            if (rawSDT.Length > 0) return true;
            if (txtDiachi.Text.Trim() != "") return true;
            return false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (!IsDataChanged())
            {
                this.Close();
                return;
            }

            if (MessageBox.Show("Dữ liệu đã nhập chưa được lưu. Bạn có chắc chắn muốn hủy không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                resetvalue();
                this.Close();
            }
        }

        private void resetvalue()
        {
            txtNCC.Text = "";
            mskSDT.Text = "";
            txtDiachi.Text = "";
            lblCanhbao.Visible = false;
        }

        private void LoadThongTin()
        {
            DataTable dt = bll.LayTheoMa(maNCCSua);

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy thông tin nhà cung cấp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            DataRow row = dt.Rows[0];
            txtNCC.Text = row["tenncc"].ToString();
            mskSDT.Text = row["sdt"].ToString();
            txtDiachi.Text = row["diachi"].ToString();
        }
    }
}