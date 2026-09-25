using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Class
{
    internal class session
    {
        public static string MaNhanVien { get; set; }
        public static string TenNhanVien { get; set; }
        public static string TenDangNhap { get; set; }
        public static string Chucvu { get; set; }

        public static string YeuCauKiemKeNhanVien { get; set; }
        public static string YeuCauKiemKeKho { get; set; }
        public static string YeuCauKiemKeChiTiet { get; set; }

        public const string GIAM_DOC = "Giám đốc";
        public const string QUAN_LY_KHO = "Quản lý kho";
        public const string NHAN_VIEN_BAN_HANG = "Nhân viên bán hàng";
        public const string NHAN_VIEN_KE_TOAN = "Nhân viên kế toán";
        public const string KE_TOAN = "Kế toán";
        public const string NHAN_VIEN_THU_NGAN = "Nhân viên thu ngân";
        public static bool LaGiamDoc()
        {
            return Chucvu == GIAM_DOC;
        }
        public static bool LaQuanLyKho()
        {
            return Chucvu == QUAN_LY_KHO;
        }
        public static bool LaNhanVienBanHang()
        {
            return Chucvu == NHAN_VIEN_BAN_HANG;
        }
        public static bool LaNhanVienKeToan()
        {
            return Chucvu == NHAN_VIEN_KE_TOAN || Chucvu == KE_TOAN;
        }
        public static bool LaNhanVienThuNgan()
        {
            return Chucvu == NHAN_VIEN_THU_NGAN;
        }
        public static void Dangnhap(string manv, string tenNhanVien, string tenDangNhap, string chucvu)
        {
            MaNhanVien = manv;
            TenNhanVien = tenNhanVien;
            TenDangNhap = tenDangNhap;
            Chucvu = chucvu;
        }
        public static void Dangxuat()
        {
            MaNhanVien = null;
            TenNhanVien = null;
            TenDangNhap = null;
            Chucvu = null;
        }
        //nếu đã đăng nhập 
        public static bool DaDangnhap()
        {
            return MaNhanVien != null;
        }
        public static bool DaDocYeuCauKiemKe { get; set; } = false;
    }
}
