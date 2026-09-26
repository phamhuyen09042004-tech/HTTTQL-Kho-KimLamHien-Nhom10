using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.BLL
{
    public class QuanlyNCCBLL
    {
        public string MaVuaTao { get; set; } = "";

        // Lấy danh sách toàn bộ nhà cung cấp
        public DataTable GetAll()
        {
            string sql = "SELECT mancc, tenncc, diachi, sdt, trangthai FROM nha_cung_cap";
            return Functions.GetDataToTable(sql);
        }

        // Lấy theo mã nhà cung cấp
        public DataTable LayTheoMa(string maNCC)
        {
            string sql = $"SELECT mancc, tenncc, diachi, sdt, trangthai FROM nha_cung_cap WHERE mancc = '{maNCC.Trim()}'";
            return Functions.GetDataToTable(sql);
        }

        // Tự động sinh mã nhà cung cấp mới NCC001, NCC002...
        public string TaoMaNCC()
        {
            string maMoi = "NCC001";
            try
            {
                string sql = "SELECT TOP 1 mancc FROM nha_cung_cap WHERE mancc LIKE 'NCC%' ORDER BY mancc DESC";
                DataTable dt = Functions.GetDataToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string maCu = dt.Rows[0]["mancc"].ToString().Trim();
                    string soStr = maCu.Substring(3);
                    if (int.TryParse(soStr, out int so))
                    {
                        maMoi = "NCC" + (so + 1).ToString("D3");
                    }
                }
            }
            catch
            {
                maMoi = "NCC001";
            }
            return maMoi;
        }

        // 1. Kiểm tra trùng tên khi Thêm mới
        public bool KiemTraTenTrung(string tenNCC)
        {
            if (string.IsNullOrEmpty(tenNCC)) return false;
            string sql = $"SELECT COUNT(*) FROM nha_cung_cap WHERE tenncc = N'{tenNCC.Trim()}'";
            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        // 2. Kiểm tra trùng tên khi Sửa (trừ mã hiện tại)
        public bool KiemTraTenTrung(string tenNCC, string maNCC)
        {
            if (string.IsNullOrEmpty(tenNCC)) return false;
            string sql = $"SELECT COUNT(*) FROM nha_cung_cap WHERE tenncc = N'{tenNCC.Trim()}' AND mancc <> '{maNCC.Trim()}'";
            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        // 3. Kiểm tra trùng số điện thoại khi Thêm mới
        public bool KiemTraSDTTrung(string sdt)
        {
            string rawSDT = LaySDTChuan(sdt);
            if (string.IsNullOrEmpty(rawSDT)) return false;
            string sql = $"SELECT COUNT(*) FROM nha_cung_cap WHERE sdt = '{rawSDT}'";
            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        // 4. Kiểm tra trùng số điện thoại khi Sửa
        public bool KiemTraSDTTrung(string sdt, string maNCC)
        {
            string rawSDT = LaySDTChuan(sdt);
            if (string.IsNullOrEmpty(rawSDT)) return false;
            string sql = $"SELECT COUNT(*) FROM nha_cung_cap WHERE sdt = '{rawSDT}' AND mancc <> '{maNCC.Trim()}'";
            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        // Lọc chuỗi số điện thoại chỉ lấy số
        private string LaySDTChuan(string sdt)
        {
            if (string.IsNullOrEmpty(sdt)) return "";
            return sdt.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "").Trim();
        }

        // Thêm mới nhà cung cấp
        public string ThemNhaCungCap(string tenNCC, string sdt, string diachi)
        {
            if (string.IsNullOrEmpty(tenNCC) || tenNCC.Trim().Length == 0)
                return "Bạn phải nhập tên nhà cung cấp!";

            string rawSDT = LaySDTChuan(sdt);
            if (string.IsNullOrEmpty(rawSDT))
                return "Bạn phải nhập số điện thoại!";

            if (rawSDT.Length < 10)
                return "Số điện thoại chưa đủ 10 chữ số, vui lòng kiểm tra lại!";

            if (string.IsNullOrEmpty(diachi) || diachi.Trim().Length == 0)
                return "Bạn phải nhập địa chỉ!";

            if (KiemTraTenTrung(tenNCC.Trim()))
                return "Tên nhà cung cấp đã tồn tại!";

            if (KiemTraSDTTrung(rawSDT))
                return "Số điện thoại đã tồn tại!";

            string maNCC = TaoMaNCC();

            string sql = @"INSERT INTO nha_cung_cap (mancc, tenncc, diachi, sdt, trangthai)
                           VALUES (@mancc, @tenncc, @diachi, @sdt, N'Hoạt động')";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@mancc", maNCC),
                new SqlParameter("@tenncc", tenNCC.Trim()),
                new SqlParameter("@diachi", diachi.Trim()),
                new SqlParameter("@sdt", rawSDT)
            };

            try
            {
                Functions.ExecuteNonQueryParam(sql, prms);
                MaVuaTao = maNCC;
                return null;
            }
            catch (Exception ex)
            {
                return "Lỗi thêm nhà cung cấp: " + ex.Message;
            }
        }

        // Sửa nhà cung cấp
        public string SuaNhaCungCap(string maNCC, string tenNCC, string sdt, string diachi)
        {
            if (string.IsNullOrEmpty(maNCC) || maNCC.Trim().Length == 0)
                return "Không tìm thấy mã nhà cung cấp cần sửa!";

            if (string.IsNullOrEmpty(tenNCC) || tenNCC.Trim().Length == 0)
                return "Bạn phải nhập tên nhà cung cấp!";

            string rawSDT = LaySDTChuan(sdt);
            if (string.IsNullOrEmpty(rawSDT))
                return "Bạn phải nhập số điện thoại!";

            if (rawSDT.Length < 10)
                return "Số điện thoại chưa đủ 10 chữ số, vui lòng kiểm tra lại!";

            if (string.IsNullOrEmpty(diachi) || diachi.Trim().Length == 0)
                return "Bạn phải nhập địa chỉ!";

            if (KiemTraTenTrung(tenNCC.Trim(), maNCC.Trim()))
                return "Tên nhà cung cấp đã tồn tại!";

            if (KiemTraSDTTrung(rawSDT, maNCC.Trim()))
                return "Số điện thoại đã tồn tại!";

            string sql = @"UPDATE nha_cung_cap 
                           SET tenncc = @tenncc, sdt = @sdt, diachi = @diachi 
                           WHERE mancc = @mancc";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@mancc", maNCC.Trim()),
                new SqlParameter("@tenncc", tenNCC.Trim()),
                new SqlParameter("@sdt", rawSDT),
                new SqlParameter("@diachi", diachi.Trim())
            };

            try
            {
                Functions.ExecuteNonQueryParam(sql, prms);
                return null;
            }
            catch (Exception ex)
            {
                return "Lỗi sửa nhà cung cấp: " + ex.Message;
            }
        }

        // Khóa nhà cung cấp
        public void KhoaNCC(string maNCC)
        {
            string sql = $"UPDATE nha_cung_cap SET trangthai = N'Ngưng hoạt động' WHERE mancc = '{maNCC.Trim()}'";
            Functions.RunSql(sql);
        }

        // Mở khóa nhà cung cấp
        public void MoKhoaNCC(string maNCC)
        {
            string sql = $"UPDATE nha_cung_cap SET trangthai = N'Hoạt động' WHERE mancc = '{maNCC.Trim()}'";
            Functions.RunSql(sql);
        }
    }
}