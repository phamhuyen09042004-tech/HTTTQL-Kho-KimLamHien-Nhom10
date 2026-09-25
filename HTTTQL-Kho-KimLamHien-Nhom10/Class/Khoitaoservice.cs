using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Class
{
    internal class Khoitaoservice
    {
        public static void KhoiTao()
        {
            if (Functions.KiemTraNhanVienTonTai() == true)
                return;

            TaoAdminMacDinh();
        }
        private static void TaoAdminMacDinh()
        {
            string matkhauTam = Functions.TaoMatKhauNgauNhien();
            string hash = BCrypt.Net.BCrypt.HashPassword(matkhauTam, workFactor: 12);

            string sql;
            sql = "INSERT INTO nhan_vien " +
                  "(manv, tennv, tendangnhap, matkhau, chucvu, sdt, diachi, email, " +
                  "lanDNdau, trangthai, ngaycapnhat, ngaytao) " +
                  "VALUES " +
                  "(@manv, @tenv, @tendangnhap, @matkhau, @chucvu, @sdt, @diachi, @email, " +
                  "1, N'Hoạt động', GETDATE(), GETDATE())";

            Functions.Ketnoi();

            SqlCommand cmd = new SqlCommand(sql, Functions.Conn);

            cmd.Parameters.AddWithValue("@manv", "NV001");
            cmd.Parameters.AddWithValue("@tenv", "Quản trị viên");
            cmd.Parameters.AddWithValue("@tendangnhap", "admin");
            cmd.Parameters.AddWithValue("@matkhau", hash);
            cmd.Parameters.AddWithValue("@chucvu", "Giám đốc");
            cmd.Parameters.AddWithValue("@sdt", "0123456789");
            cmd.Parameters.AddWithValue("@diachi", "Hà Tĩnh");
            cmd.Parameters.AddWithValue("@email", "ceo@kimlamhien.com");
            cmd.ExecuteNonQuery();
            Functions.Ngatketnoi();

            MessageBox.Show(
                "Hệ thống chưa có tài khoản quản trị.\n\n" +
                "Đã tạo tài khoản admin mặc định:\n\n" +
                "Tên đăng nhập: admin\n" +
                "Mật khẩu tạm  : " + matkhauTam + "\n\n" +
                "Vui lòng đổi mật khẩu sau khi đăng nhập.",
                "Khởi tạo hệ thống",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
