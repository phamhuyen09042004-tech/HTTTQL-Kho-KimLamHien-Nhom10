using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmThemxuong : Form
    {
        private string cheDo = "THEM";
        private string maXuongSua = "";
        private BLL.QuanlyXuongBLL bll = new BLL.QuanlyXuongBLL();

        // Biến trả về cho form Đề xuất nhập kho
        public string MaXuongMoi = "";
        public string TenXuongMoi = "";

        public FrmThemxuong()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = "THEM";
        }

        public FrmThemxuong(string cheDo, string maXuong)
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = cheDo;
            this.maXuongSua = maXuong;
        }

        private void FrmThemxuong_Load(object sender, EventArgs e)
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
                this.Text = "Sửa thông tin xưởng chế tác";
                label3.Text = "SỬA XƯỞNG CHẾ TÁC";
                LoadThongTin();
            }
            else
            {
                this.Text = "Thêm mới xưởng chế tác";
                label3.Text = "THÊM XƯỞNG CHẾ TÁC";
            }

            txtTenXuong.Focus();
        }

        private void txtTenXuong_Leave(object sender, EventArgs e)
        {
            string tenXuong = txtTenXuong.Text.Trim();
            if (tenXuong.Length == 0)
            {
                lblCanhbao.Visible = false;
                return;
            }

            if (cheDo == "SUA" && bll.KiemTraTenTrung(tenXuong, maXuongSua))
                lblCanhbao.Visible = true;
            else if (cheDo != "SUA" && bll.KiemTraTenTrung(tenXuong))
                lblCanhbao.Visible = true;
            else
                lblCanhbao.Visible = false;
        }

        private void txtTenXuong_TextChanged(object sender, EventArgs e)
        {
            lblCanhbao.Visible = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenXuong = txtTenXuong.Text.Trim();
            string diachi = txtDiachi.Text.Trim();

            // 1. Kiểm tra tên xưởng
            if (string.IsNullOrEmpty(tenXuong))
            {
                MessageBox.Show("Vui lòng nhập tên xưởng chế tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenXuong.Focus();
                return;
            }

            if (lblCanhbao.Visible)
            {
                MessageBox.Show("Tên xưởng chế tác đã tồn tại, bạn phải nhập tên khác!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenXuong.Focus();
                return;
            }

            // 2. KIỂM TRA SỐ ĐIỆN THOẠI (Bắt buộc đủ 10 chữ số)
            string rawSDT = mskSDT.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "").Trim();
            if (string.IsNullOrEmpty(rawSDT))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSDT.Focus();
                return;
            }

            if (!mskSDT.MaskFull || rawSDT.Length < 10)
            {
                MessageBox.Show("Số điện thoại chưa nhập đủ 10 số. Vui lòng nhập đầy đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSDT.Focus();
                return;
            }

            // 3. Kiểm tra địa chỉ
            if (string.IsNullOrEmpty(diachi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ xưởng chế tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiachi.Focus();
                return;
            }

            string loi;
            if (cheDo == "SUA")
            {
                loi = bll.SuaXuong(maXuongSua, tenXuong, mskSDT.Text.Trim(), diachi);
            }
            else
            {
                loi = bll.ThemXuong(tenXuong, mskSDT.Text.Trim(), diachi);
            }

            if (loi != null)
            {
                MessageBox.Show(loi, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // GÁN GIÁ TRỊ TRẢ VỀ CHO FORM ĐỀ XUẤT NHẬP
            if (cheDo == "THEM")
            {
                MaXuongMoi = bll.MaVuaTao;
                TenXuongMoi = tenXuong;
                MessageBox.Show("Thêm xưởng chế tác mới thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MaXuongMoi = maXuongSua;
                TenXuongMoi = tenXuong;
                MessageBox.Show("Cập nhật xưởng chế tác thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            KhoaTatCa(true);
            btnLuu.Enabled = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void KhoaTatCa(bool khoa)
        {
            txtTenXuong.ReadOnly = khoa;
            mskSDT.ReadOnly = khoa;
            txtDiachi.ReadOnly = khoa;
        }

        private bool IsDataChanged()
        {
            if (txtTenXuong.Text.Trim() != "") return true;
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

            if (MessageBox.Show("Dữ liệu đã nhập chưa được lưu. Bạn có muốn hủy không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                ResetValue();
                this.Close();
            }
        }

        private void ResetValue()
        {
            txtTenXuong.Text = "";
            mskSDT.Text = "";
            txtDiachi.Text = "";
            lblCanhbao.Visible = false;
        }

        private void LoadThongTin()
        {
            DataTable dt = bll.LayTheoMa(maXuongSua);

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy thông tin xưởng chế tác!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            DataRow row = dt.Rows[0];
            txtTenXuong.Text = row["tenxuong"].ToString();
            mskSDT.Text = row["sdt"].ToString();
            txtDiachi.Text = row["diachi"].ToString();
        }
    }
}