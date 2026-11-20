using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;
using HTTTQL_Kho_KimLamHien_Nhom10.DAL;

namespace HTTTQL_Kho_KimLamHien_Nhom10.BLL
{
    internal class NguoidungBLL
    {
        public const string TT_HOATDONG = "Hoạt động";
        public const string TT_DAKHOA = "Đã khóa";
        public static bool ChoPhepTruyCap()
        {
            return session.LaGiamDoc();
        }
        public static DataTable LayDanhSach(string ten, string chucvu)
        {
            return NguoidungDAL.LayDanhSach((ten ?? "").Trim(), chucvu);
        }
        // Combo chức vụ, dòng đầu là "-- Chọn chức vụ --" (giá trị rỗng)
        public static DataTable LayChucVuCombo()
        {
            DataTable dt = NguoidungDAL.LayChucVu();
            DataRow r = dt.NewRow();
            r["chucvu"] = "";
            dt.Rows.InsertAt(r, 0);
            return dt;
        }
        // Khóa nếu đang hoạt động, mở khóa nếu đang bị khóa. Trả về thông báo kết quả.
        public static string KhoaMo(string manv)
        {
            DataRow nv = LayNhanVienHopLe(manv);

            if (manv == session.MaNhanVien)
                throw new Exception("Không thể khóa chính tài khoản đang đăng nhập.");

            string hienTai = Convert.ToString(nv["trangthai"]);
            if (hienTai == TT_DAKHOA)
            {
                NguoidungDAL.DoiTrangThai(manv, TT_HOATDONG);
                return "Đã mở khóa tài khoản " + manv + ".";
            }
            NguoidungDAL.DoiTrangThai(manv, TT_DAKHOA);
            return "Đã khóa tài khoản " + manv + ".";
        }
        // Tạo mật khẩu tạm, lưu bản băm, bắt đổi mật khẩu ở lần đăng nhập sau. Trả về mật khẩu tạm.
        public static string DatLaiMatKhau(string manv)
        {
            DataRow nv = LayNhanVienHopLe(manv);

            string matkhauTam = Functions.TaoMatKhauNgauNhien();
            string hash = BCrypt.Net.BCrypt.HashPassword(matkhauTam, workFactor: 12);
            Functions.CapNhatMatKhau(Convert.ToString(nv["tendangnhap"]), hash, true);
            return matkhauTam;
        }
        private static DataRow LayNhanVienHopLe(string manv)
        {
            if (string.IsNullOrWhiteSpace(manv))
                throw new Exception("Vui lòng chọn một nhân viên.");

            DataTable dt = NguoidungDAL.LayMotNhanVien(manv);
            if (dt.Rows.Count == 0)
                throw new Exception("Không tìm thấy nhân viên " + manv + ".");
            return dt.Rows[0];
        }
    }
}
