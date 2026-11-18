using HTTTQL_Kho_KimLamHien_Nhom10.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTTQL_Kho_KimLamHien_Nhom10.DAL
{
    internal class BaocaoDAL
    {
        // ===== Báo cáo doanh thu =====
        // Danh sách hóa đơn trong khoảng ngày
        public static DataTable LayDanhSachHoaDon(DateTime tu, DateTime den, string trangthai)
        {
            string sql = "SELECT h.mahd, " +
                         "CAST(NULL AS NVARCHAR(50)) AS loaidon, " +      // chưa có nguồn, để trống
                         "ISNULL(k.tenkh, N'Khách lẻ') AS tenkh, h.ngaylap, " +
                         "CAST(NULL AS DATETIME) AS ngaycapnhat, " +       // chưa có nguồn, để trống
                         "h.ptthanhtoan, n.tennv, ISNULL(h.tongtien, 0) AS tongtien, " +
                         "(SELECT ISNULL(SUM(c.soluong), 0) FROM ct_hoa_don c WHERE c.mahd = h.mahd) AS tongsl " +
                         "FROM hoa_don h " +
                         "LEFT JOIN khach_hang k ON h.makh = k.makh " +
                         "LEFT JOIN nhan_vien n ON h.manv = n.manv " +
                         "WHERE h.trangthai = @tt " +
                         "AND CAST(h.ngaylap AS date) BETWEEN @tu AND @den " +
                         "ORDER BY h.ngaylap DESC";
            return Functions.GetDataToTableParam(sql,
                new SqlParameter("@tt", trangthai),
                new SqlParameter("@tu", SqlDbType.Date) { Value = tu.Date },
                new SqlParameter("@den", SqlDbType.Date) { Value = den.Date });
        }
        // Doanh thu từng ngày (chỉ trả về những ngày có đơn)
        public static DataTable LayDoanhThuTheoNgay(DateTime tu, DateTime den, string trangthai)
        {
            string sql = "SELECT CAST(ngaylap AS date) AS ngay, SUM(ISNULL(tongtien, 0)) AS doanhthu " +
                         "FROM hoa_don " +
                         "WHERE trangthai = @tt " +
                         "AND CAST(ngaylap AS date) BETWEEN @tu AND @den " +
                         "GROUP BY CAST(ngaylap AS date)";
            return Functions.GetDataToTableParam(sql,
                new SqlParameter("@tt", trangthai),
                new SqlParameter("@tu", SqlDbType.Date) { Value = tu.Date },
                new SqlParameter("@den", SqlDbType.Date) { Value = den.Date });
        }
        // Doanh thu từng tháng trong một năm (chỉ trả về những tháng có đơn)
        public static DataTable LayDoanhThuTheoThang(int nam, string trangthai)
        {
            string sql = "SELECT MONTH(ngaylap) AS thang, SUM(ISNULL(tongtien, 0)) AS doanhthu " +
                         "FROM hoa_don " +
                         "WHERE trangthai = @tt AND YEAR(ngaylap) = @nam " +
                         "GROUP BY MONTH(ngaylap)";
            return Functions.GetDataToTableParam(sql,
                new SqlParameter("@tt", trangthai),
                new SqlParameter("@nam", nam));
        }
        // ===================== BÁO CÁO TỒN KHO =====================
        public static DataTable LayLoaiHang()
        {
            return Functions.GetDataToTableParam("SELECT maloai, tenloai FROM loai_hang");
        }
        // maloai rỗng = tất cả. den = mốc tồn cuối kỳ.
        public static DataTable LayTonKho(string maloai, bool chiCanhBao, DateTime den,
                                          string[] ttHoaDonCho, string ttDeXuatDaDuyet,
                                          string ttNhap, string ttXuat)
        {
            string sql =
                "SELECT mahang, tenhang, tenloai, tonthucte, " +
                "       tonthucte - chodk AS tonkhadung, tonthucte - chodk + sapdk AS tondukien, " +
                "       nguongcanhbao, maloai " +
                "FROM (" +
                "  SELECT h.mahang, h.tenhang, l.tenloai, h.maloai, " +
                "         h.soluongton - ISNULL(nh.sl, 0) + ISNULL(xk.sl, 0) AS tonthucte, " +
                "         ISNULL(cho.sl, 0) * @hientai AS chodk, " +
                "         ISNULL(sap.sl, 0) * @hientai AS sapdk, " +
                "         ISNULL(h.muctontoithieu, 0) AS nguongcanhbao " +
                "  FROM hang_hoa h " +
                "  LEFT JOIN loai_hang l ON h.maloai = l.maloai " +
                "  LEFT JOIN (SELECT c.mahang, SUM(c.soluong) AS sl FROM ct_phieu_nhap c " +
                "             JOIN phieu_nhap_kho p ON c.maphieunhap = p.maphieunhap " +
                "             WHERE p.trangthai = @ttnhap AND CAST(p.ngaynhap AS date) > @den " +
                "             GROUP BY c.mahang) nh ON nh.mahang = h.mahang " +
                "  LEFT JOIN (SELECT c.mahang, SUM(c.soluong) AS sl FROM ct_phieu_xuat c " +
                "             JOIN phieu_xuat_kho p ON c.maphieuxuat = p.maphieuxuat " +
                "             WHERE p.trangthai = @ttxuat AND CAST(p.ngayxuat AS date) > @den " +
                "             GROUP BY c.mahang) xk ON xk.mahang = h.mahang " +
                "  LEFT JOIN (SELECT c.mahang, SUM(c.soluong) AS sl FROM ct_hoa_don c " +
                "             JOIN hoa_don d ON c.mahd = d.mahd " +
                "             WHERE d.trangthai IN (@cho1, @cho2) GROUP BY c.mahang) cho ON cho.mahang = h.mahang " +
                "  LEFT JOIN (SELECT x.mahang, SUM(x.soluongdk) AS sl FROM ct_phieu_de_xuat x " +
                "             JOIN phieu_de_xuat_nhap p ON x.madexuat = p.madexuat " +
                "             WHERE p.trangthai = @dx " +
                "             AND NOT EXISTS (SELECT 1 FROM phieu_nhap_kho n WHERE n.madexuat = p.madexuat) " +
                "             GROUP BY x.mahang) sap ON sap.mahang = h.mahang " +
                "  WHERE (@maloai = '' OR h.maloai = @maloai)" +
                ") t " +
                "WHERE (@canhbao = 0 OR tonthucte < nguongcanhbao) " +
                "ORDER BY tonthucte DESC";

            return Functions.GetDataToTableParam(sql,
                new SqlParameter("@den", SqlDbType.Date) { Value = den.Date },
                new SqlParameter("@ttnhap", ttNhap),
                new SqlParameter("@ttxuat", ttXuat),
                new SqlParameter("@cho1", ttHoaDonCho[0]),
                new SqlParameter("@cho2", ttHoaDonCho[1]),
                new SqlParameter("@dx", ttDeXuatDaDuyet),
                new SqlParameter("@maloai", maloai ?? ""),
                new SqlParameter("@canhbao", chiCanhBao ? 1 : 0),
                new SqlParameter("@hientai", den.Date >= DateTime.Today ? 1 : 0));
        }
        // ===================== BÁO CÁO BÁN CHẠY =====================
        // maloai rỗng = tất cả. Lấy top N mặt hàng bán nhiều nhất trong khoảng ngày.
        public static DataTable LayBanChay(int top, string maloai, DateTime tu, DateTime den, string trangthai)
        {
            string sql = "SELECT TOP (@top) h.mahang, h.tenhang, l.tenloai, h.dvt, " +
                         "SUM(c.soluong) AS soluongban, ISNULL(SUM(c.thanhtien), 0) AS doanhthu " +
                         "FROM ct_hoa_don c " +
                         "JOIN hoa_don d ON c.mahd = d.mahd " +
                         "JOIN hang_hoa h ON c.mahang = h.mahang " +
                         "LEFT JOIN loai_hang l ON h.maloai = l.maloai " +
                         "WHERE d.trangthai = @tt " +
                         "AND CAST(d.ngaylap AS date) BETWEEN @tu AND @den " +
                         "AND (@maloai = '' OR h.maloai = @maloai) " +
                         "GROUP BY h.mahang, h.tenhang, l.tenloai, h.dvt " +
                         "ORDER BY SUM(c.soluong) DESC, SUM(c.thanhtien) DESC";
            return Functions.GetDataToTableParam(sql,
                new SqlParameter("@top", top),
                new SqlParameter("@tt", trangthai),
                new SqlParameter("@tu", SqlDbType.Date) { Value = tu.Date },
                new SqlParameter("@den", SqlDbType.Date) { Value = den.Date },
                new SqlParameter("@maloai", maloai ?? ""));
        }
    }
}
