using HTTTQL_Kho_KimLamHien_Nhom10.Class;
using HTTTQL_Kho_KimLamHien_Nhom10.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace HTTTQL_Kho_KimLamHien_Nhom10.BLL
{
    internal class GiaodienBLL
    {
        private const string TT_HOADON_HOANTHANH = "Hoàn thành";
        private const string TT_HOADON_CHOXACNHAN = "Chờ xác nhận";
        private const string TT_HOADON_DANGXULY = "Đang xử lý";
        private const string TT_DEXUAT_CHODUYET = "Chờ duyệt";
        private const string TT_NHAP_HOANTHANH = "Hoàn thành";
        private const string TT_XUAT_HOANTHANH = "Hoàn thành";
        private const string TT_DEXUAT_DADUYET = "Đã duyệt";
        private const string TT_DEXUAT_NHAP = "Chờ xác nhận";
        public static bool ChoPhepTruyCapGiamdoc()
        {
            return session.LaGiamDoc();

        }
        public static bool ChoPhepTruyCapBanhang()
        {
            return session.LaNhanVienBanHang() || session.LaNhanVienThuNgan();
        }
        public static bool ChoPhepTruyCapKho()
        {
            return session.LaQuanLyKho();
        }
        public static bool ChoPhepTruyCapKT()
        {
            return session.LaNhanVienKeToan();
        }
        public static int LayDonChoXacNhan()
        {
            return GiaodienDAL.DemHoaDon(TT_HOADON_CHOXACNHAN);
        }
        public static int LayDonDangXuLy()
        {
            return GiaodienDAL.DemHoaDon(TT_HOADON_DANGXULY);
        }
        public static decimal LayDoanhThuHomNay()
        {
            return GiaodienDAL.LayDoanhThuHomNay(TT_HOADON_HOANTHANH);
        }
        public static int LayDeXuatChoXacNhan()
        {
            return GiaodienDAL.DemDeXuatTheoTrangThai(TT_DEXUAT_NHAP);
        }
        public static int LayDeXuatChoDuyet()
        {
            return GiaodienDAL.DemDeXuatTheoTrangThai(TT_DEXUAT_CHODUYET);
        }
        public static int LayDeXuatDaDuyet()
        {
            return GiaodienDAL.DemDeXuatTheoTrangThai(TT_DEXUAT_DADUYET);
        }
        public static int LayHangSapHet()
        {
            return GiaodienDAL.DemHangSapHet();
        }
        public static int LayTonKhoDauNgay()
        {
            int nhap = GiaodienDAL.TongNhapTruocHomNay(TT_NHAP_HOANTHANH);
            int xuat = GiaodienDAL.TongXuatTruocHomNay(TT_XUAT_HOANTHANH);
            return nhap - xuat;
        }
        // Trả về đủ 7 ngày (ngày không có đơn = 0). Cột: Ngay (dd/MM), DoanhThu
        public static DataTable LayDoanhThu7Ngay()
        {
            DataTable raw = GiaodienDAL.LayDoanhThu7Ngay(TT_HOADON_HOANTHANH);

            DataTable kq = new DataTable();
            kq.Columns.Add("Ngay", typeof(string));
            kq.Columns.Add("DoanhThu", typeof(decimal));

            for (int i = 6; i >= 0; i--)
            {
                DateTime ngay = DateTime.Today.AddDays(-i);
                decimal tong = 0;
                for (int j = 0; j < raw.Rows.Count; j++)
                {
                    if (Convert.ToDateTime(raw.Rows[j]["ngay"]).Date == ngay)
                        tong = Convert.ToDecimal(raw.Rows[j]["doanhthu"]);
                }
                kq.Rows.Add(ngay.ToString("dd/MM"), tong);
            }
            return kq;
        }
        public static DataTable LayTop5BanChay()
        {
            return GiaodienDAL.LayTop5BanChay(TT_HOADON_HOANTHANH);
        }
    }
}
