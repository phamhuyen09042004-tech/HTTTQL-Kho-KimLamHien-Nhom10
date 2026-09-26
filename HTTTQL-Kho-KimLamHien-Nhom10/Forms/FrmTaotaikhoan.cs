using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmTaotaikhoan : Form
    {
        private string cheDo = "THEM";
        private string maNVSua = "";
        private string trangThaiHienTai = "Hoạt động";

        public FrmTaotaikhoan()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = "THEM";
        }

        public FrmTaotaikhoan(string cheDo, string maNV)
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = cheDo;
            this.maNVSua = maNV;
        }

        private void FrmTaotaikhoan_Load(object sender, EventArgs e)
        {
            LoadDanhSachChucVu();

            if (cheDo == "SUA")
            {
                this.Text = "Sửa thông tin nhân viên";
                lblTitle.Text = "THÔNG TIN NGƯỜI DÙNG";
                lblMatKhau.Text = "Mật khẩu:";
                txtMatKhauTam.Text = "****** (Đã mã hóa)";
                LoadThongTinNhanVien(maNVSua);
            }
            else
            {
                this.Text = "Tạo tài khoản nhân viên";
                lblTitle.Text = "TẠO TÀI KHOẢN MỚI";
                txtMaNV.Text = SinhMaNhanVien();
                trangThaiHienTai = "Hoạt động";
                lblTrangThai.Text = "✔ Hoạt động";
                lblTrangThai.ForeColor = Color.ForestGreen;

                // Ngày cập nhật gần nhất khi tạo mới là ngày hôm nay
                lblNgayTao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtMatKhauTam.Text = "123456"; // Mật khẩu mặc định
            }
        }

        private void LoadDanhSachChucVu()
        {
            try
            {
                // Bảng chuc_vu (macv, tencv) theo đúng thiết kế CSDL
                string sql = "SELECT macv, tencv FROM chuc_vu";
                DataTable dt = Functions.GetDataToTable(sql);
                cboChucVu.DataSource = dt;
                cboChucVu.DisplayMember = "tencv";
                cboChucVu.ValueMember = "macv";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục chức vụ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string SinhMaNhanVien()
        {
            string maMoi = "NV001";
            try
            {
                string sql = "SELECT TOP 1 manv FROM nhan_vien WHERE manv LIKE 'NV%' ORDER BY manv DESC";
                DataTable dt = Functions.GetDataToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string maCu = dt.Rows[0]["manv"].ToString().Trim();
                    string soStr = maCu.Substring(2);
                    if (int.TryParse(soStr, out int so))
                    {
                        maMoi = "NV" + (so + 1).ToString("D3");
                    }
                }
            }
            catch { }
            return maMoi;
        }

        // Tự động sinh tên đăng nhập khi gõ Họ tên ở chế độ TẠO MỚI
        private void txtHoTen_TextChanged(object sender, EventArgs e)
        {
            if (cheDo == "THEM")
            {
                string hoTen = txtHoTen.Text.Trim();
                if (!string.IsNullOrEmpty(hoTen))
                {
                    txtTenDangNhap.Text = ChuyenHoTenThanhUsername(hoTen);
                }
                else
                {
                    txtTenDangNhap.Text = "";
                }
            }
        }

        private string ChuyenHoTenThanhUsername(string hoTen)
        {
            string str = hoTen.Normalize(NormalizationForm.FormD);
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            str = regex.Replace(str, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            string[] words = str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) return "";

            // Lấy tên chính + chữ cái đầu các từ lót (ví dụ: Trần Quốc Hoàn -> hoantq)
            string tenChinh = words[words.Length - 1].ToLower();
            string hoLot = "";
            for (int i = 0; i < words.Length - 1; i++)
            {
                hoLot += words[i].Substring(0, 1).ToLower();
            }

            return (tenChinh + hoLot).Trim();
        }

        private void LoadThongTinNhanVien(string maNV)
        {
            try
            {
                string sql = $"SELECT manv, tennv, tendangnhap, machucvu, sdt, diachi, email, trangthai, ngaycapnhat, ngaytao FROM nhan_vien WHERE manv = '{maNV}'";
                DataTable dt = Functions.GetDataToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    txtMaNV.Text = r["manv"].ToString();
                    txtHoTen.Text = r["tennv"].ToString();
                    txtTenDangNhap.Text = r["tendangnhap"].ToString();
                    txtSDT.Text = r["sdt"].ToString();
                    txtDiaChi.Text = r["diachi"].ToString();
                    txtEmail.Text = r["email"].ToString();

                    if (r["machucvu"] != DBNull.Value)
                        cboChucVu.SelectedValue = r["machucvu"].ToString();

                    trangThaiHienTai = r["trangthai"].ToString();
                    if (trangThaiHienTai == "Hoạt động")
                    {
                        lblTrangThai.Text = "✔ Hoạt động";
                        lblTrangThai.ForeColor = Color.ForestGreen;
                    }
                    else
                    {
                        lblTrangThai.Text = "✖ Đã khóa";
                        lblTrangThai.ForeColor = Color.Red;
                    }

                    // Ngày cập nhật gần nhất
                    if (r["ngaycapnhat"] != DBNull.Value)
                    {
                        lblNgayTao.Text = Convert.ToDateTime(r["ngaycapnhat"]).ToString("dd/MM/yyyy");
                    }
                    else if (r["ngaytao"] != DBNull.Value)
                    {
                        lblNgayTao.Text = Convert.ToDateTime(r["ngaytao"]).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        lblNgayTao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy thông tin: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string email = txtEmail.Text.Trim();

            // 1. Kiểm tra tính hợp lệ dữ liệu (Validation)
            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ và tên nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (cboChucVu.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn chức vụ cho nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboChucVu.Focus();
                return;
            }

            string rawSDT = sdt.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "");
            if (string.IsNullOrEmpty(rawSDT))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
            if (rawSDT.Length < 10)
            {
                MessageBox.Show("Số điện thoại phải có ít nhất 10 số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            if (string.IsNullOrEmpty(diaChi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ Email nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // Kiểm tra định dạng Email hợp lệ
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Địa chỉ Email không đúng định dạng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            string maChucVu = cboChucVu.SelectedValue.ToString();

            // 2. Kiểm tra trùng Email trong CSDL (Email là UNIQUE trong thiết kế)
            string sqlCheckEmail = $"SELECT COUNT(*) FROM nhan_vien WHERE email = '{email}'";
            if (cheDo == "SUA")
            {
                sqlCheckEmail += $" AND manv <> '{txtMaNV.Text.Trim()}'";
            }
            int countEmail = Convert.ToInt32(Functions.ExecuteScalarParam(sqlCheckEmail) ?? 0);
            if (countEmail > 0)
            {
                MessageBox.Show("Địa chỉ Email này đã được sử dụng bởi nhân viên khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                if (cheDo == "THEM")
                {
                    string maNV = txtMaNV.Text.Trim();
                    string username = txtTenDangNhap.Text.Trim();
                    string matKhau = txtMatKhauTam.Text.Trim();

                    // Đảm bảo tính duy nhất của tên đăng nhập
                    string sqlCheckUser = $"SELECT COUNT(*) FROM nhan_vien WHERE tendangnhap = '{username}'";
                    int countUser = Convert.ToInt32(Functions.ExecuteScalarParam(sqlCheckUser) ?? 0);
                    if (countUser > 0)
                    {
                        username = username + maNV.ToLower();
                    }

                    // CHUẨN NGHIỆP VỤ: lanDNdau phải là 1 để kích hoạt đổi mật khẩu khi đăng nhập lần đầu
                    string sqlInsert = @"INSERT INTO nhan_vien (manv, tennv, tendangnhap, matkhau, lanDNdau, machucvu, sdt, diachi, email, trangthai, ngaycapnhat, ngaytao)
                                         VALUES (@manv, @tennv, @tendangnhap, @matkhau, 1, @machucvu, @sdt, @diachi, @email, N'Hoạt động', GETDATE(), GETDATE())";

                    SqlParameter[] prms = new SqlParameter[]
                    {
                        new SqlParameter("@manv", maNV),
                        new SqlParameter("@tennv", hoTen),
                        new SqlParameter("@tendangnhap", username),
                        new SqlParameter("@matkhau", matKhau),
                        new SqlParameter("@machucvu", maChucVu),
                        new SqlParameter("@sdt", rawSDT),
                        new SqlParameter("@diachi", diaChi),
                        new SqlParameter("@email", email)
                    };

                    Functions.ExecuteNonQueryParam(sqlInsert, prms);
                    MessageBox.Show("Tạo tài khoản nhân viên thành công!\nTên đăng nhập: " + username + "\nMật khẩu tạm: " + matKhau, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string sqlUpdate = @"UPDATE nhan_vien 
                                         SET tennv = @tennv, machucvu = @machucvu, sdt = @sdt, diachi = @diachi, email = @email, ngaycapnhat = GETDATE()
                                         WHERE manv = @manv";

                    SqlParameter[] prms = new SqlParameter[]
                    {
                        new SqlParameter("@manv", txtMaNV.Text.Trim()),
                        new SqlParameter("@tennv", hoTen),
                        new SqlParameter("@machucvu", maChucVu),
                        new SqlParameter("@sdt", rawSDT),
                        new SqlParameter("@diachi", diaChi),
                        new SqlParameter("@email", email)
                    };

                    Functions.ExecuteNonQueryParam(sqlUpdate, prms);
                    MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Dữ liệu chưa được lưu. Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}