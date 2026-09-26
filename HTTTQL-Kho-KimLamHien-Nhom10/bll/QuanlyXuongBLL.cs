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
    public class QuanlyXuongBLL
    {
        public string MaVuaTao { get; set; } = "";

        // Lấy toàn bộ danh sách
        public DataTable GetAll()
        {
            string sql = "SELECT maxuong, tenxuong, diachi, sdt, trangthai FROM xuong_che_tac";
            return Functions.GetDataToTable(sql);
        }

        // Tìm kiếm + lọc trạng thái
        public DataTable SearchAndFilter(string keyword, string trangThai)
        {
            string sql = "SELECT maxuong, tenxuong, diachi, sdt, trangthai FROM xuong_che_tac WHERE 1=1";
            if (!string.IsNullOrEmpty(keyword))
            {
                sql += $" AND (tenxuong LIKE N'%{keyword.Trim()}%' OR maxuong LIKE N'%{keyword.Trim()}%' OR sdt LIKE '%{keyword.Trim()}%')";
            }
            if (!string.IsNullOrEmpty(trangThai) && trangThai != "Tất cả")
            {
                sql += $" AND trangthai = N'{trangThai.Trim()}'";
            }
            return Functions.GetDataToTable(sql);
        }

        // Lấy theo mã xưởng
        public DataTable LayTheoMa(string maXuong)
        {
            string sql = $"SELECT maxuong, tenxuong, diachi, sdt, trangthai FROM xuong_che_tac WHERE maxuong = '{maXuong.Trim()}'";
            return Functions.GetDataToTable(sql);
        }

        public DataTable GetByMaXuong(string maXuong)
        {
            return LayTheoMa(maXuong);
        }

        // Sinh mã xưởng tự động XU001, XU002...
        public string TaoMaXuong()
        {
            string maMoi = "XU001";
            try
            {
                string sql = "SELECT TOP 1 maxuong FROM xuong_che_tac WHERE maxuong LIKE 'XU%' ORDER BY maxuong DESC";
                DataTable dt = Functions.GetDataToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string maCu = dt.Rows[0]["maxuong"].ToString().Trim();
                    string soStr = maCu.Substring(2).Replace("_", "").Replace("B", "").Replace("V", "");
                    if (int.TryParse(soStr, out int so))
                    {
                        maMoi = "XU" + (so + 1).ToString("D3");
                    }
                }
            }
            catch
            {
                maMoi = "XU001";
            }
            return maMoi;
        }

        // Kiểm tra trùng tên
        public bool KiemTraTenTrung(string tenXuong)
        {
            string sql = $"SELECT COUNT(*) FROM xuong_che_tac WHERE tenxuong = N'{tenXuong.Trim()}'";
            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        public bool KiemTraTenTrung(string tenXuong, string maXuong)
        {
            string sql = $"SELECT COUNT(*) FROM xuong_che_tac WHERE tenxuong = N'{tenXuong.Trim()}' AND maxuong <> '{maXuong.Trim()}'";
            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        // Kiểm tra trùng SDT
        public bool KiemTraSDTTrung(string sdt)
        {
            string rawSDT = LaySDTChuan(sdt);
            string sql = $"SELECT COUNT(*) FROM xuong_che_tac WHERE sdt = '{rawSDT}'";
            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        public bool KiemTraSDTTrung(string sdt, string maXuong)
        {
            string rawSDT = LaySDTChuan(sdt);
            string sql = $"SELECT COUNT(*) FROM xuong_che_tac WHERE sdt = '{rawSDT}' AND maxuong <> '{maXuong.Trim()}'";
            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        // Thêm nhà xưởng
        public string ThemXuong(string tenXuong, string sdt, string diachi)
        {
            if (string.IsNullOrEmpty(tenXuong) || tenXuong.Trim().Length == 0)
                return "Bạn phải nhập tên nhà xưởng!";

            if (KiemTraSDTRong(sdt))
                return "Bạn phải nhập số điện thoại!";

            if (string.IsNullOrEmpty(diachi) || diachi.Trim().Length == 0)
                return "Bạn phải nhập địa chỉ!";

            if (KiemTraTenTrung(tenXuong.Trim()))
                return "Tên xưởng đã tồn tại!";

            if (KiemTraSDTTrung(sdt.Trim()))
                return "Số điện thoại đã tồn tại!";

            string maXuong = TaoMaXuong();
            string rawSDT = LaySDTChuan(sdt);

            string sql = @"INSERT INTO xuong_che_tac (maxuong, tenxuong, diachi, sdt, trangthai)
                           VALUES (@maxuong, @tenxuong, @diachi, @sdt, N'Hoạt động')";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@maxuong", maXuong),
                new SqlParameter("@tenxuong", tenXuong.Trim()),
                new SqlParameter("@diachi", diachi.Trim()),
                new SqlParameter("@sdt", rawSDT)
            };

            try
            {
                Functions.ExecuteNonQueryParam(sql, prms);
                MaVuaTao = maXuong;
                return null;
            }
            catch (Exception ex)
            {
                return "Lỗi thêm xưởng: " + ex.Message;
            }
        }

        // Sửa nhà xưởng
        public string SuaXuong(string maXuong, string tenXuong, string sdt, string diachi)
        {
            if (string.IsNullOrEmpty(maXuong) || maXuong.Trim().Length == 0)
                return "Không tìm thấy mã xưởng cần sửa!";

            if (string.IsNullOrEmpty(tenXuong) || tenXuong.Trim().Length == 0)
                return "Bạn phải nhập tên xưởng!";

            if (KiemTraSDTRong(sdt))
                return "Bạn phải nhập số điện thoại!";

            if (string.IsNullOrEmpty(diachi) || diachi.Trim().Length == 0)
                return "Bạn phải nhập địa chỉ!";

            if (KiemTraTenTrung(tenXuong.Trim(), maXuong.Trim()))
                return "Tên xưởng đã tồn tại!";

            if (KiemTraSDTTrung(sdt.Trim(), maXuong.Trim()))
                return "Số điện thoại đã tồn tại!";

            string rawSDT = LaySDTChuan(sdt);
            string sql = @"UPDATE xuong_che_tac 
                           SET tenxuong = @tenxuong, sdt = @sdt, diachi = @diachi 
                           WHERE maxuong = @maxuong";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@maxuong", maXuong.Trim()),
                new SqlParameter("@tenxuong", tenXuong.Trim()),
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
                return "Lỗi sửa xưởng: " + ex.Message;
            }
        }

        // Khóa nhà xưởng
        public void KhoaXuong(string maXuong)
        {
            string sql = $"UPDATE xuong_che_tac SET trangthai = N'Ngưng hoạt động' WHERE maxuong = '{maXuong.Trim()}'";
            Functions.RunSql(sql);
        }

        // Mở khóa nhà xưởng
        public void MoKhoaXuong(string maXuong)
        {
            string sql = $"UPDATE xuong_che_tac SET trangthai = N'Hoạt động' WHERE maxuong = '{maXuong.Trim()}'";
            Functions.RunSql(sql);
        }

        public string KiemTraRangBuocSuaKhoa(string maXuong)
        {
            int soDeXuat = DemDeXuatNhapChuaHoanThanh(maXuong);
            if (soDeXuat > 0)
                return "Xưởng chế tác này còn " + soDeXuat + " phiếu đề xuất nhập chưa hoàn thành!\n" +
                       "Vui lòng hoàn thành hoặc hủy các phiếu đề xuất trước.";

            int soPhieuNhap = DemPhieuNhapKhoChuaHoanThanh(maXuong);
            if (soPhieuNhap > 0)
                return "Xưởng chế tác này còn " + soPhieuNhap + " phiếu nhập kho chưa hoàn thành!\n" +
                       "Vui lòng hoàn thành các phiếu nhập trước.";

            return "";
        }

        public int DemDeXuatNhapChuaHoanThanh(string maXuong)
        {
            try
            {
                string sql = $"SELECT COUNT(*) FROM phieu_de_xuat_nhap WHERE maxuong = '{maXuong.Trim()}' AND trangthai <> N'Đã hoàn thành'";
                object res = Functions.ExecuteScalarParam(sql);
                return (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            }
            catch { return 0; }
        }

        public int DemPhieuNhapKhoChuaHoanThanh(string maXuong)
        {
            try
            {
                string sql = $"SELECT COUNT(*) FROM phieu_nhap_kho WHERE maxuong = '{maXuong.Trim()}' AND trangthai <> N'Đã hoàn thành'";
                object res = Functions.ExecuteScalarParam(sql);
                return (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            }
            catch { return 0; }
        }

        // Thống kê footer
        public int GetTongSoXuong()
        {
            string sql = "SELECT COUNT(*) FROM xuong_che_tac";
            object res = Functions.ExecuteScalarParam(sql);
            return (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
        }

        public int GetSoXuongHopTac()
        {
            string sql = "SELECT COUNT(*) FROM xuong_che_tac WHERE trangthai = N'Hoạt động'";
            object res = Functions.ExecuteScalarParam(sql);
            return (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
        }

        public int GetSoXuongNgungHopTac()
        {
            string sql = "SELECT COUNT(*) FROM xuong_che_tac WHERE trangthai = N'Ngưng hoạt động'";
            object res = Functions.ExecuteScalarParam(sql);
            return (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
        }

        // Kiểm tra MaskedTextBox số điện thoại có rỗng không
        private bool KiemTraSDTRong(string sdt)
        {
            if (sdt == null) return true;
            string tam = LaySDTChuan(sdt);
            return tam.Length == 0;
        }

        private string LaySDTChuan(string sdt)
        {
            if (string.IsNullOrEmpty(sdt)) return "";
            return sdt.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "").Trim();
        }
    }
}