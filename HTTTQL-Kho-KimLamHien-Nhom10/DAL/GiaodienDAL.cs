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
    internal class GiaodienDAL
    {
        public static int DemHoaDon(string trangthai)
        {
            string sql = "SELECT COUNT(*) FROM hoa_don WHERE trangthai = @tt";
            return Convert.ToInt32(Functions.ExecuteScalarParam(sql, new SqlParameter("@tt", trangthai)));
        }
        public static decimal LayDoanhThuHomNay(string trangthai)
        {
            string sql = "SELECT ISNULL(SUM(tongtien), 0) FROM hoa_don " +
                         "WHERE trangthai = @tt AND CAST(ngaylap AS date) = CAST(GETDATE() AS date)";
            return Convert.ToDecimal(Functions.ExecuteScalarParam(sql, new SqlParameter("@tt", trangthai)));
        }
        public static int DemDeXuatTheoTrangThai(string trangthai)
        {
            string sql = "SELECT COUNT(*) FROM phieu_de_xuat_nhap WHERE trangthai = @tt";
            return Convert.ToInt32(Functions.ExecuteScalarParam(sql, new SqlParameter("@tt", trangthai)));
        }
        public static int DemHangSapHet()
        {
            string sql = "SELECT COUNT(*) FROM hang_hoa WHERE soluongton < muctontoithieu";
            return Convert.ToInt32(Functions.ExecuteScalarParam(sql));
        }
        public static int TongNhapTruocHomNay(string trangthai)
        {
            string sql = "SELECT ISNULL(SUM(ct.soluong), 0) " +
                         "FROM ct_phieu_nhap ct JOIN phieu_nhap_kho p ON ct.maphieunhap = p.maphieunhap " +
                         "WHERE p.trangthai = @tt AND CAST(p.ngaynhap AS date) < CAST(GETDATE() AS date)";
            return Convert.ToInt32(Functions.ExecuteScalarParam(sql, new SqlParameter("@tt", trangthai)));
        }
        public static int TongXuatTruocHomNay(string trangthai)
        {
            string sql = "SELECT ISNULL(SUM(ct.soluong), 0) " +
                         "FROM ct_phieu_xuat ct JOIN phieu_xuat_kho p ON ct.maphieuxuat = p.maphieuxuat " +
                         "WHERE p.trangthai = @tt AND CAST(p.ngayxuat AS date) < CAST(GETDATE() AS date)";
            return Convert.ToInt32(Functions.ExecuteScalarParam(sql, new SqlParameter("@tt", trangthai)));
        }
        // Doanh thu theo từng ngày trong 7 ngày gần nhất (chỉ trả về những ngày có đơn)
        public static DataTable LayDoanhThu7Ngay(string trangthai)
        {
            string sql = "SELECT CAST(ngaylap AS date) AS ngay, SUM(tongtien) AS doanhthu " +
                         "FROM hoa_don " +
                         "WHERE trangthai = @tt " +
                         "AND CAST(ngaylap AS date) >= CAST(DATEADD(day, -6, GETDATE()) AS date) " +
                         "AND CAST(ngaylap AS date) <= CAST(GETDATE() AS date) " +
                         "GROUP BY CAST(ngaylap AS date)";
            return Functions.GetDataToTableParam(sql, new SqlParameter("@tt", trangthai));
        }
        // Top 5 sản phẩm bán chạy của tháng hiện tại
        public static DataTable LayTop5BanChay(string trangthai)
        {
            string sql = "SELECT TOP 5 h.tenhang, SUM(c.soluong) AS soluong " +
                         "FROM ct_hoa_don c " +
                         "JOIN hoa_don d ON c.mahd = d.mahd " +
                         "JOIN hang_hoa h ON c.mahang = h.mahang " +
                         "WHERE d.trangthai = @tt " +
                         "AND MONTH(d.ngaylap) = MONTH(GETDATE()) AND YEAR(d.ngaylap) = YEAR(GETDATE()) " +
                         "GROUP BY h.mahang, h.tenhang " +
                         "ORDER BY SUM(c.soluong) DESC";
            return Functions.GetDataToTableParam(sql, new SqlParameter("@tt", trangthai));
        }
    }
}
