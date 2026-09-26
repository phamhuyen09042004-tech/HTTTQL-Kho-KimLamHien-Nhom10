using System;
using System.Data;
using System.Data.SqlClient;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.BLL
{
    public class LoaihangBLL
    {
        public string MaVuaTao { get; set; } = "";

        public string SinhMaMoi()
        {
            string maMoi = "LH01";
            try
            {
                string sql = "SELECT TOP 1 maloai FROM loai_hang WHERE maloai LIKE 'LH%' ORDER BY maloai DESC";
                DataTable dt = Functions.GetDataToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string maCu = dt.Rows[0]["maloai"].ToString().Trim();
                    string soStr = maCu.Substring(2);
                    if (int.TryParse(soStr, out int so))
                    {
                        maMoi = "LH" + (so + 1).ToString("D2");
                    }
                }
            }
            catch { }
            return maMoi;
        }

        public string ThemLoaiHang(string tenLoai)
        {
            tenLoai = (tenLoai ?? "").Trim();
            if (string.IsNullOrEmpty(tenLoai)) return "Vui lòng nhập tên loại hàng!";

            string sqlCheck = $"SELECT COUNT(*) FROM loai_hang WHERE tenloai = N'{tenLoai}'";
            object res = Functions.ExecuteScalarParam(sqlCheck);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            if (count > 0) return "Tên loại hàng đã tồn tại!";

            string maMoi = SinhMaMoi();
            string sql = "INSERT INTO loai_hang (maloai, tenloai, trangthai) VALUES (@maloai, @tenloai, N'Hoạt động')";
            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@maloai", maMoi),
                new SqlParameter("@tenloai", tenLoai)
            };

            try
            {
                Functions.ExecuteNonQueryParam(sql, prms);
                MaVuaTao = maMoi;
                return null;
            }
            catch (Exception ex)
            {
                return "Lỗi thêm loại hàng: " + ex.Message;
            }
        }
    }
}