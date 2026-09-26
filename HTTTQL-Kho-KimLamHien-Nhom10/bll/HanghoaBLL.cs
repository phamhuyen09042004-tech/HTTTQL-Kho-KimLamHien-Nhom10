using System;
using System.Data;
using System.Data.SqlClient;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.BLL
{
    public class HanghoaBLL
    {
        public string MaVuaTao { get; set; } = "";

        public DataTable LayDanhSachLoai()
        {
            string sql = "SELECT maloai, tenloai FROM loai_hang WHERE trangthai = N'Hoạt động'";
            return Functions.GetDataToTable(sql);
        }

        public DataTable LayDanhSachDVT()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("dvt");
            dt.Rows.Add("chỉ");
            dt.Rows.Add("phân");
            dt.Rows.Add("gam");
            dt.Rows.Add("chiếc");
            dt.Rows.Add("bộ");
            return dt;
        }

        public bool KiemTraTrungMa(string maHang)
        {
            string sql = $"SELECT COUNT(*) FROM hang_hoa WHERE mahang = '{maHang.Trim()}'";
            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        public string ThemHang(string maHang, string tenHang, string maLoai, string dvt, string mota)
        {
            if (string.IsNullOrEmpty(maHang)) return "Vui lòng nhập mã hàng!";
            if (KiemTraTrungMa(maHang)) return "Mã hàng này đã tồn tại!";
            if (string.IsNullOrEmpty(tenHang)) return "Vui lòng nhập tên hàng hóa!";
            if (string.IsNullOrEmpty(maLoai)) return "Vui lòng chọn loại hàng!";

            // Phân loại kho tự động: HLS925 -> Kho bạc (KHO_B01), còn lại -> Kho vàng (KHO_B02)
            string maKho = (maLoai == "HLS925") ? "KHO_B01" : "KHO_B02";

            string sql = @"INSERT INTO hang_hoa (mahang, tenhang, dvt, maloai, anhbia, mota, dongiavonbq, muctontoithieu, soluongton, trangthai, makho)
                           VALUES (@mahang, @tenhang, @dvt, @maloai, @anhbia, @mota, 0, 5, 0, N'Đang kinh doanh', @makho)";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@mahang", maHang),
                new SqlParameter("@tenhang", tenHang),
                new SqlParameter("@dvt", (dvt ?? "").Trim()),
                new SqlParameter("@maloai", maLoai),
                new SqlParameter("@anhbia", "default.jpg"),
                new SqlParameter("@mota", (mota ?? "").Trim()),
                new SqlParameter("@makho", maKho)
            };

            try
            {
                Functions.ExecuteNonQueryParam(sql, prms);
                MaVuaTao = maHang;
                return null;
            }
            catch (Exception ex)
            {
                return "Lỗi thêm hàng hóa: " + ex.Message;
            }
        }
    }
}