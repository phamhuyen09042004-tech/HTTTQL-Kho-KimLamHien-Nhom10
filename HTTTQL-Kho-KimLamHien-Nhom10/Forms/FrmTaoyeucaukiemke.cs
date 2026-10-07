using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmTaoyeucaukiemke : Form
    {
        public string MaPhieuVuaTao { get; private set; } = "";

        public FrmTaoyeucaukiemke()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
        }

        private void FrmTaoyeucaukiemke_Load(object sender, EventArgs e)
        {
            dtpThoigian.Value = DateTime.Now;
            LoadDanhSachKho();
            LoadDanhSachNhanVien();
        }

        private void LoadDanhSachKho()
        {
            try
            {
                // Bảng kho (makho, tenkho, trangthai)
                string sql = "SELECT makho, tenkho FROM kho WHERE trangthai = N'Hoạt động'";
                DataTable dt = Functions.GetDataToTable(sql);
                cboKho.DataSource = dt;
                cboKho.DisplayMember = "tenkho";
                cboKho.ValueMember = "makho";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachNhanVien()
        {
            try
            {
                string sql = "SELECT manv, tennv FROM nhan_vien WHERE trangthai = N'Hoạt động'";
                DataTable dt = Functions.GetDataToTable(sql);
                cboTenNV.DataSource = dt;
                cboTenNV.DisplayMember = "tennv";
                cboTenNV.ValueMember = "manv";

                // Chọn mặc định nhân viên đầu tiên trong danh sách thay vì gọi FrmMain.MaNV
                if (cboTenNV.Items.Count > 0)
                {
                    cboTenNV.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string SinhMaPhieuKiemKe()
        {
            string maMoi = "PKK001";
            try
            {
                string sql = "SELECT TOP 1 maphieukiemke FROM phieu_kiem_ke_kho WHERE maphieukiemke LIKE 'PKK%' ORDER BY maphieukiemke DESC";
                DataTable dt = Functions.GetDataToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string maCu = dt.Rows[0]["maphieukiemke"].ToString().Trim();
                    string soStr = maCu.Substring(3);
                    if (int.TryParse(soStr, out int so))
                    {
                        maMoi = "PKK" + (so + 1).ToString("D3");
                    }
                }
            }
            catch { }
            return maMoi;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cboKho.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn kho cần kiểm kê!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboKho.Focus();
                return;
            }

            if (cboTenNV.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên thực hiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTenNV.Focus();
                return;
            }

            string maKho = cboKho.SelectedValue.ToString();
            string tenKho = cboKho.Text;
            string maNV = cboTenNV.SelectedValue.ToString();

            // QUY TẮC NGHIỆP VỤ BẮT BUỘC: Kiểm tra kho có đợt kiểm kê nào chưa hoàn tất không
            string sqlCheckDangKiem = $"SELECT COUNT(*) FROM phieu_kiem_ke_kho WHERE makho = '{maKho}' AND trangthai <> N'Hoàn tất'";
            int countDangKiem = Convert.ToInt32(Functions.ExecuteScalarParam(sqlCheckDangKiem) ?? 0);

            if (countDangKiem > 0)
            {
                MessageBox.Show($"Kho [{tenKho}] hiện đang có đợt kiểm kê chưa hoàn tất!\nVui lòng hoàn tất hoặc hủy đợt kiểm kê cũ trước khi khởi tạo yêu cầu mới.",
                                "Cảnh báo kiểm kê", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tiến hành tạo phiếu kiểm kê mới ở trạng thái 'Chờ xử lý' (hoặc 'Chưa hoàn tất')
            string maPKK = SinhMaPhieuKiemKe();
            string sqlInsert = @"INSERT INTO phieu_kiem_ke_kho (maphieukiemke, ngaykiemke, trangthai, makho, manv)
                                 VALUES (@maphieukiemke, @ngaykiemke, N'Chờ xử lý', @makho, @manv)";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@maphieukiemke", maPKK),
                new SqlParameter("@ngaykiemke", dtpThoigian.Value),
                new SqlParameter("@makho", maKho),
                new SqlParameter("@manv", maNV)
            };

            try
            {
                Functions.ExecuteNonQueryParam(sqlInsert, prms);
                MaPhieuVuaTao = maPKK;
                MessageBox.Show($"Tạo yêu cầu kiểm kê thành công!\nMã phiếu: {maPKK}\nKho: {tenKho}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu phiếu kiểm kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn hủy tạo yêu cầu kiểm kê?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}