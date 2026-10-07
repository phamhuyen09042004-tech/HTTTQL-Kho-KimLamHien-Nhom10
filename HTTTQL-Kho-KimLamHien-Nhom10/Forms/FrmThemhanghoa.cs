using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmThemhanghoa : Form
    {
        public string MaHangMoi = "";
        public string TenHangMoi = "";
        public string DVTMoi = "";
        public string MotaMoi = "";

        public FrmThemhanghoa()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
        }

        private void FrmThemhanghoa_Load(object sender, EventArgs e)
        {
            txtMahang.Focus();
            lblTrungma.Visible = false;

            string logoPath = Path.Combine(Application.StartupPath, "Image", "Giaodien", "logo1.ico");
            if (File.Exists(logoPath))
            {
                pic1.Image = Image.FromFile(logoPath);
            }
            pic1.SizeMode = PictureBoxSizeMode.Zoom;

            LoadCombo();
        }

        private void LoadCombo()
        {
            try
            {
                // Nạp danh sách loại hàng từ bảng loai_hang
                string sqlLoai = "SELECT maloai, tenloai FROM loai_hang WHERE trangthai = N'Hoạt động'";
                Functions.FillCombo(sqlLoai, cboLoai, "maloai", "tenloai");
                cboLoai.SelectedIndex = -1;

                // Nạp danh sách đơn vị tính cố định cho ngành vàng bạc trang sức
                DataTable dtDVT = new DataTable();
                dtDVT.Columns.Add("dvt");
                dtDVT.Rows.Add("chỉ");
                dtDVT.Rows.Add("phân");
                dtDVT.Rows.Add("gam");
                dtDVT.Rows.Add("chiếc");
                dtDVT.Rows.Add("bộ");

                cboDVT.DataSource = dtDVT;
                cboDVT.DisplayMember = "dvt";
                cboDVT.ValueMember = "dvt";
                cboDVT.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMahang_Leave(object sender, EventArgs e)
        {
            string maHang = txtMahang.Text.Trim();
            if (string.IsNullOrEmpty(maHang))
            {
                lblTrungma.Visible = false;
                return;
            }

            string sqlCheck = $"SELECT COUNT(*) FROM hang_hoa WHERE mahang = '{maHang}'";
            object res = Functions.ExecuteScalarParam(sqlCheck);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            lblTrungma.Visible = (count > 0);
        }

        // Tự động tính: Tổng trọng lượng = Trọng lượng vàng/bạc + Trọng lượng đá
        private void TinhTongTrongLuong_TextChanged(object sender, EventArgs e)
        {
            decimal tlVang = 0;
            decimal tlDa = 0;

            decimal.TryParse(txtTLvangbac.Text.Trim(), out tlVang);
            decimal.TryParse(txtTLda.Text.Trim(), out tlDa);

            if (tlVang > 0 || tlDa > 0)
            {
                txtTongluongSP.Text = (tlVang + tlDa).ToString("G29");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maHang = txtMahang.Text.Trim();
            string tenHang = txtTenhang.Text.Trim();

            // 1. Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(maHang))
            {
                MessageBox.Show("Vui lòng nhập mã hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMahang.Focus();
                return;
            }

            if (lblTrungma.Visible)
            {
                MessageBox.Show("Mã hàng này đã tồn tại, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMahang.Focus();
                return;
            }

            if (string.IsNullOrEmpty(tenHang))
            {
                MessageBox.Show("Vui lòng nhập tên hàng hóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenhang.Focus();
                return;
            }

            if (cboLoai.SelectedIndex == -1 || cboLoai.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn loại hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoai.Focus();
                return;
            }

            if (cboDVT.SelectedIndex == -1 && string.IsNullOrEmpty(cboDVT.Text.Trim()))
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDVT.Focus();
                return;
            }

            // 2. Validate trọng lượng kim hoàn
            decimal tlTong = 0;
            decimal tlVang = 0;
            decimal tlDa = 0;

            if (!decimal.TryParse(txtTLvangbac.Text.Trim(), out tlVang) || tlVang <= 0)
            {
                MessageBox.Show("Trọng lượng vàng/bạc phải là số lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTLvangbac.Focus();
                return;
            }

            decimal.TryParse(txtTLda.Text.Trim(), out tlDa);
            if (tlDa < 0)
            {
                MessageBox.Show("Trọng lượng đá không được là số âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTLda.Focus();
                return;
            }

            tlTong = tlVang + tlDa;
            txtTongluongSP.Text = tlTong.ToString("G29");

            string maL = cboLoai.SelectedValue.ToString();
            string tenLoai = cboLoai.Text.ToLower();
            string DVT = cboDVT.Text.Trim();
            string mota = $"Tổng: {tlTong} | Vàng/Bạc: {tlVang} | Đá: {tlDa}";

            // 3. Phân kho chuẩn CSDL (Kho bạc: KHO_B01 hoặc mã kho chứa chữ 'bạc', còn lại là Kho vàng)
            string maKho = "KHO_B02"; // Mặc định là Kho vàng
            try
            {
                if (maL == "HLS925" || tenLoai.Contains("bạc"))
                {
                    maKho = "KHO_B01";
                }

                // Kiểm tra xem maKho có tồn tại trong CSDL không, nếu không lấy mã kho hợp lệ đầu tiên
                string sqlCheckKho = $"SELECT TOP 1 makho FROM kho WHERE makho = '{maKho}'";
                object resKho = Functions.ExecuteScalarParam(sqlCheckKho);
                if (resKho == null)
                {
                    string sqlLayKhoDau = "SELECT TOP 1 makho FROM kho WHERE trangthai = N'Hoạt động'";
                    object khoAlt = Functions.ExecuteScalarParam(sqlLayKhoDau);
                    if (khoAlt != null) maKho = khoAlt.ToString();
                }
            }
            catch { }

            // 4. Thực hiện Insert vào bảng hang_hoa
            string sqlInsert = @"INSERT INTO hang_hoa (mahang, tenhang, dvt, maloai, anhbia, mota, dongiavonbq, muctontoithieu, soluongton, trangthai, makho)
                                 VALUES (@mahang, @tenhang, @dvt, @maloai, @anhbia, @mota, @dongiavonbq, @muctontoithieu, @soluongton, @trangthai, @makho)";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@mahang", maHang),
                new SqlParameter("@tenhang", tenHang),
                new SqlParameter("@dvt", DVT),
                new SqlParameter("@maloai", maL),
                new SqlParameter("@anhbia", "default.jpg"),
                new SqlParameter("@mota", mota),
                new SqlParameter("@dongiavonbq", 0),
                new SqlParameter("@muctontoithieu", 5),
                new SqlParameter("@soluongton", 0),
                new SqlParameter("@trangthai", "Đang kinh doanh"),
                new SqlParameter("@makho", maKho)
            };

            try
            {
                Functions.ExecuteNonQueryParam(sqlInsert, prms);

                MaHangMoi = maHang;
                TenHangMoi = tenHang;
                DVTMoi = DVT;
                MotaMoi = mota;

                MessageBox.Show("Thêm hàng hóa mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hàng hóa: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void resetvalue()
        {
            txtMahang.Text = "";
            txtTenhang.Text = "";
            txtTongluongSP.Text = "";
            txtTLvangbac.Text = "";
            txtTLda.Text = "0";
            cboLoai.SelectedIndex = -1;
            cboDVT.SelectedIndex = -1;
            lblTrungma.Visible = false;
        }

        private bool IsDataChanged()
        {
            if (!string.IsNullOrEmpty(txtMahang.Text.Trim())) return true;
            if (!string.IsNullOrEmpty(txtTenhang.Text.Trim())) return true;
            if (!string.IsNullOrEmpty(txtTLvangbac.Text.Trim())) return true;
            if (cboLoai.SelectedIndex != -1) return true;
            if (cboDVT.SelectedIndex != -1) return true;
            return false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (!IsDataChanged())
            {
                this.Close();
                return;
            }

            if (MessageBox.Show("Dữ liệu đã nhập chưa được lưu. Bạn có muốn hủy không?",
                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                resetvalue();
                this.Close();
            }
        }
    }
}