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
        public string DVTMoi = "chỉ";
        public string MaLoaiMoi = "";
        public string TenLoaiMoi = "";
        public string MotaMoi = "";

        public FrmThemhanghoa()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
        }

        private void FrmThemhanghoa_Load(object sender, EventArgs e)
        {
            HienThongBaoLoi("");

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

            // Gắn sự kiện tính tự động tổng trọng lượng & xóa thông báo khi gõ
            txtTLvangbac.TextChanged += TinhTongTrongLuong_TextChanged;
            txtTLda.TextChanged += TinhTongTrongLuong_TextChanged;
            txtTenhang.TextChanged += (s, ev) => HienThongBaoLoi("");
            txtMahang.TextChanged += (s, ev) => HienThongBaoLoi("");

            txtTongluongSP.ReadOnly = true;
            txtTLda.Text = "0";
            txtTongluongSP.Text = "0";

            LoadCombo();
            txtMahang.Focus();
        }

        private void LoadCombo()
        {
            try
            {
                // 1. Nạp danh sách loại hàng
                string sqlLoai = "SELECT maloai, tenloai FROM loai_hang WHERE trangthai = N'Hoạt động'";
                DataTable dtLoai = Functions.GetDataToTable(sqlLoai);
                cboLoai.DataSource = dtLoai;
                cboLoai.DisplayMember = "tenloai";
                cboLoai.ValueMember = "maloai";
                if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;

                // 2. Nạp đơn vị tính cố định
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
                cboDVT.SelectedValue = "chỉ"; // Mặc định là chỉ
            }
            catch (Exception ex)
            {
                HienThongBaoLoi("Lỗi tải danh mục: " + ex.Message);
            }
        }

        private void TinhTongTrongLuong_TextChanged(object sender, EventArgs e)
        {
            HienThongBaoLoi(""); // Ẩn cảnh báo ngay khi bắt đầu gõ

            decimal tlVang = 0;
            decimal tlDa = 0;

            decimal.TryParse(txtTLvangbac.Text.Trim(), out tlVang);
            decimal.TryParse(txtTLda.Text.Trim(), out tlDa);

            txtTongluongSP.Text = (tlVang + tlDa).ToString("G29");
        }

        private void txtMahang_Leave(object sender, EventArgs e)
        {
            string maHang = txtMahang.Text.Trim();
            if (string.IsNullOrEmpty(maHang)) return;

            if (KiemTraTrungMa(maHang))
            {
                HienThongBaoLoi($"⚠ Mã hàng '{maHang}' đã tồn tại! Vui lòng nhập mã khác.");
            }
            else
            {
                HienThongBaoLoi("");
            }
        }

        private bool KiemTraTrungMa(string maHang)
        {
            try
            {
                string sqlCheck = $"SELECT COUNT(*) FROM hang_hoa WHERE mahang = '{maHang}'";
                int count = Convert.ToInt32(Functions.ExecuteScalarParam(sqlCheck) ?? 0);
                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        private void HienThongBaoLoi(string thongBao)
        {
            Control lblLoi = LayControl("lblThongbao") ?? LayControl("lblTrungma");
            if (lblLoi != null)
            {
                if (string.IsNullOrEmpty(thongBao))
                {
                    lblLoi.Text = "";
                    lblLoi.Visible = false;
                }
                else
                {
                    lblLoi.Text = thongBao;
                    lblLoi.ForeColor = Color.Red;
                    lblLoi.Visible = true;
                }
            }
            else if (!string.IsNullOrEmpty(thongBao))
            {
                MessageBox.Show(thongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Control LayControl(string name)
        {
            Control[] arr = this.Controls.Find(name, true);
            return (arr.Length > 0) ? arr[0] : null;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            HienThongBaoLoi("");

            string maHang = txtMahang.Text.Trim();
            string tenHang = txtTenhang.Text.Trim();

            // 1. Kiểm tra mã hàng
            if (string.IsNullOrEmpty(maHang))
            {
                HienThongBaoLoi("⚠ Vui lòng nhập mã hàng hóa!");
                txtMahang.Focus();
                return;
            }

            if (KiemTraTrungMa(maHang))
            {
                HienThongBaoLoi($"⚠ Mã hàng '{maHang}' đã tồn tại! Vui lòng chọn mã khác.");
                txtMahang.Focus();
                txtMahang.SelectAll();
                return;
            }

            // 2. Kiểm tra tên hàng
            if (string.IsNullOrEmpty(tenHang))
            {
                HienThongBaoLoi("⚠ Vui lòng nhập tên hàng hóa!");
                txtTenhang.Focus();
                return;
            }

            // 3. Kiểm tra loại hàng
            if (cboLoai.SelectedIndex == -1 || cboLoai.SelectedValue == null)
            {
                HienThongBaoLoi("⚠ Vui lòng chọn loại hàng hóa!");
                cboLoai.Focus();
                return;
            }

            // 4. KIỂM TRA TRỌNG LƯỢNG (Thông báo rõ ràng trường lỗi)
            decimal tlVang = 0;
            decimal tlDa = 0;

            if (string.IsNullOrEmpty(txtTLvangbac.Text.Trim()))
            {
                HienThongBaoLoi("⚠ [Trọng lượng] Vui lòng nhập trọng lượng vàng/bạc!");
                txtTLvangbac.Focus();
                return;
            }

            if (!decimal.TryParse(txtTLvangbac.Text.Trim(), out tlVang) || tlVang <= 0)
            {
                HienThongBaoLoi("⚠ [Trọng lượng] Trọng lượng vàng/bạc phải là số lớn hơn 0!");
                txtTLvangbac.Focus();
                txtTLvangbac.SelectAll();
                return;
            }

            if (!string.IsNullOrEmpty(txtTLda.Text.Trim()))
            {
                if (!decimal.TryParse(txtTLda.Text.Trim(), out tlDa) || tlDa < 0)
                {
                    HienThongBaoLoi("⚠ [Trọng lượng] Trọng lượng đá không hợp lệ (không được âm)!");
                    txtTLda.Focus();
                    txtTLda.SelectAll();
                    return;
                }
            }

            decimal tlTong = tlVang + tlDa;
            txtTongluongSP.Text = tlTong.ToString("G29");

            string maL = cboLoai.SelectedValue.ToString();
            string tenLoai = cboLoai.Text;
            string dvt = (cboDVT.SelectedValue != null) ? cboDVT.SelectedValue.ToString() : "chỉ";
            string mota = $"Tổng: {tlTong} | Vàng/Bạc: {tlVang} | Đá: {tlDa}";

            // 5. Xác định kho hàng: loại bạc -> KHO_B01, loại vàng -> KHO_B02
            string maKho = (maL == "HLS925" || tenLoai.ToLower().Contains("bạc")) ? "KHO_B01" : "KHO_B02";

            // 6. CÂU LỆNH INSERT VÀ DANH SÁCH THAM SỐ
            string sqlInsert = @"INSERT INTO hang_hoa (mahang, tenhang, dvt, maloai, anhbia, mota, dongiavonbq, muctontoithieu, soluongton, trangthai, makho)
                                 VALUES (@mahang, @tenhang, @dvt, @maloai, @anhbia, @mota, @dongiavonbq, @muctontoithieu, @soluongton, @trangthai, @makho)";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@mahang", SqlDbType.VarChar) { Value = maHang },
                new SqlParameter("@tenhang", SqlDbType.NVarChar) { Value = tenHang },
                new SqlParameter("@dvt", SqlDbType.NVarChar) { Value = dvt },
                new SqlParameter("@maloai", SqlDbType.VarChar) { Value = maL },
                new SqlParameter("@anhbia", SqlDbType.NVarChar) { Value = "default.jpg" },
                new SqlParameter("@mota", SqlDbType.NVarChar) { Value = mota },
                new SqlParameter("@dongiavonbq", SqlDbType.Decimal) { Value = 0 },
                new SqlParameter("@muctontoithieu", SqlDbType.Int) { Value = 5 },
                new SqlParameter("@soluongton", SqlDbType.Decimal) { Value = 0 },
                new SqlParameter("@trangthai", SqlDbType.NVarChar) { Value = "Đang kinh doanh" },
                new SqlParameter("@makho", SqlDbType.VarChar) { Value = maKho }
            };

            try
            {
                Functions.ExecuteNonQueryParam(sqlInsert, prms);

                MaHangMoi = maHang;
                TenHangMoi = tenHang;
                DVTMoi = dvt;
                MaLoaiMoi = maL;
                TenLoaiMoi = tenLoai;
                MotaMoi = mota;

                MessageBox.Show("Thêm hàng hóa mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                HienThongBaoLoi("Lỗi khi thêm hàng hóa: " + ex.Message);
            }
        }

        private void resetvalue()
        {
            txtMahang.Text = "";
            txtTenhang.Text = "";
            txtTLvangbac.Text = "";
            txtTLda.Text = "0";
            txtTongluongSP.Text = "0";
            if (cboLoai.Items.Count > 0) cboLoai.SelectedIndex = 0;
            cboDVT.SelectedValue = "chỉ";
            HienThongBaoLoi("");
        }

        private bool IsDataChanged()
        {
            if (!string.IsNullOrEmpty(txtMahang.Text.Trim())) return true;
            if (!string.IsNullOrEmpty(txtTenhang.Text.Trim())) return true;
            if (!string.IsNullOrEmpty(txtTLvangbac.Text.Trim())) return true;
            return false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (!IsDataChanged())
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            if (MessageBox.Show("Dữ liệu đã nhập chưa được lưu. Bạn có muốn hủy không?",
                "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                resetvalue();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}