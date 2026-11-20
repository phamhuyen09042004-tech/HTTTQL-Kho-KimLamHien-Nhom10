using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.DAL
{
    internal class NguoidungDAL
    {
        // ten rỗng = tất cả, chucvu rỗng = tất cả
        public static DataTable LayDanhSach(string ten, string chucvu)
        {
            string sql = "SELECT manv, tennv, chucvu, tendangnhap, " +
                         "CAST(lanDNdau AS int) AS landndau, ngaytao, ngaycapnhat, trangthai " +
                         "FROM nhan_vien " +
                         "WHERE (@ten = '' OR tennv LIKE @like) " +
                         "AND (@cv = '' OR chucvu = @cv) " +
                         "ORDER BY manv DESC";
            return Functions.GetDataToTableParam(sql,
                new SqlParameter("@ten", ten ?? ""),
                new SqlParameter("@like", "%" + (ten ?? "") + "%"),
                new SqlParameter("@cv", chucvu ?? ""));
        }
        public static DataTable LayChucVu()
        {
            return Functions.GetDataToTableParam(
                "SELECT DISTINCT chucvu FROM nhan_vien WHERE chucvu IS NOT NULL ORDER BY chucvu");
        }
        public static DataTable LayMotNhanVien(string manv)
        {
            return Functions.GetDataToTableParam(
                "SELECT manv, tendangnhap, trangthai FROM nhan_vien WHERE manv = @manv",
                new SqlParameter("@manv", manv));
        }
        public static void DoiTrangThai(string manv, string trangthai)
        {
            Functions.ExecuteNonQueryParam(
                "UPDATE nhan_vien SET trangthai = @tt, ngaycapnhat = GETDATE() WHERE manv = @manv",
                new SqlParameter("@tt", trangthai),
                new SqlParameter("@manv", manv));
        }
    }
}
