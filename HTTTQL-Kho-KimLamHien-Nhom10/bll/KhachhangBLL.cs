using System;
using System.Data;
using System.Data.SqlClient;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.BLL
{
    public class KhachhangBLL
    {
        public string MaVuaTao { get; set; } = "";

        /// <summary>
        /// Tự động sinh mã khách hàng mới theo chuẩn KH0001, KH0002...
        /// </summary>
        public string SinhMaMoi()
        {
            string maMoi = "KH0001";
            try
            {
                string sql = "SELECT TOP 1 makh FROM khach_hang WHERE makh LIKE 'KH%' ORDER BY makh DESC";
                DataTable dt = Functions.GetDataToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string maCu = dt.Rows[0]["makh"].ToString().Trim();
                    string phanSo = maCu.Substring(2); // Cắt tiền tố "KH"
                    if (int.TryParse(phanSo, out int so))
                    {
                        maMoi = "KH" + (so + 1).ToString("D4");
                    }
                }
            }
            catch
            {
                maMoi = "KH0001";
            }
            return maMoi;
        }

        /// <summary>
        /// Kiểm tra số điện thoại đã tồn tại trong CSDL hay chưa
        /// </summary>
        public bool KiemTraSDT(string sdt, string maKHBoQua = "")
        {
            string sdtChuan = (sdt ?? "").Trim();
            string sql = $"SELECT COUNT(*) FROM khach_hang WHERE sdt = '{sdtChuan}'";
            if (!string.IsNullOrEmpty(maKHBoQua))
            {
                sql += $" AND makh <> '{maKHBoQua}'";
            }

            object res = Functions.ExecuteScalarParam(sql);
            int count = (res != null && int.TryParse(res.ToString(), out int c)) ? c : 0;
            return count > 0;
        }

        /// <summary>
        /// Lấy thông tin khách hàng theo số điện thoại
        /// </summary>
        public DataTable Kiemtrakhachhang(string sdt)
        {
            string sdtChuan = (sdt ?? "").Trim();
            string sql = $"SELECT makh, tenkh, sdt, diachi FROM khach_hang WHERE sdt = '{sdtChuan}'";
            return Functions.GetDataToTable(sql);
        }

        /// <summary>
        /// Tìm kiếm theo SĐT (dùng cho các form tìm kiếm / bán hàng)
        /// </summary>
        public DataTable TimKiemTheoSDT(string sdt)
        {
            return Kiemtrakhachhang(sdt);
        }

        /// <summary>
        /// Thêm mới khách hàng (có kiểm tra tính hợp lệ dữ liệu)
        /// Trả về null nếu thành công, trả về chuỗi thông báo nếu có lỗi
        /// </summary>
        public string ThemKhachHang(string tenKH, string sdt, string diaChi)
        {
            tenKH = (tenKH ?? "").Trim();
            diaChi = (diaChi ?? "").Trim();
            sdt = (sdt ?? "").Trim();

            if (tenKH.Length == 0)
                return "Bạn phải nhập tên khách hàng!";
            if (tenKH.Length < 2)
                return "Tên khách hàng phải có ít nhất 2 ký tự!";

            // Lọc các ký tự định dạng của MaskedTextBox
            string rawSDT = sdt.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "");
            if (rawSDT.Length == 0)
                return "Bạn phải nhập số điện thoại!";
            if (rawSDT.Length < 10)
                return "Số điện thoại phải có đủ 10 số!";

            if (KiemTraSDT(rawSDT))
                return "Số điện thoại đã tồn tại trong hệ thống!";

            if (diaChi.Length == 0)
                return "Bạn phải nhập địa chỉ!";

            string maKH = SinhMaMoi();
            string sqlInsert = @"INSERT INTO khach_hang (makh, tenkh, sdt, diachi) 
                                 VALUES (@makh, @tenkh, @sdt, @diachi)";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@makh", maKH),
                new SqlParameter("@tenkh", tenKH),
                new SqlParameter("@sdt", rawSDT),
                new SqlParameter("@diachi", diaChi)
            };

            try
            {
                Functions.ExecuteNonQueryParam(sqlInsert, prms);
                MaVuaTao = maKH;
                return null; // Thành công
            }
            catch (Exception ex)
            {
                return "Lỗi thêm khách hàng: " + ex.Message;
            }
        }

        /// <summary>
        /// Đảm bảo thông tin khách hàng tồn tại trước khi xuất/bán hóa đơn
        /// </summary>
        public string DamBaoKhachHang(string sdt, string tenKH, string diaChi)
        {
            string rawSDT = (sdt ?? "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "");
            DataTable dt = Kiemtrakhachhang(rawSDT);

            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0]["makh"].ToString();

            string maKH = SinhMaMoi();
            string sqlInsert = @"INSERT INTO khach_hang (makh, tenkh, sdt, diachi) 
                                 VALUES (@makh, @tenkh, @sdt, @diachi)";

            SqlParameter[] prms = new SqlParameter[]
            {
                new SqlParameter("@makh", maKH),
                new SqlParameter("@tenkh", (tenKH ?? "").Trim()),
                new SqlParameter("@sdt", rawSDT),
                new SqlParameter("@diachi", (diaChi ?? "").Trim())
            };

            Functions.ExecuteNonQueryParam(sqlInsert, prms);

            dt = Kiemtrakhachhang(rawSDT);
            if (dt == null || dt.Rows.Count == 0)
                throw new Exception("Không tạo được khách hàng.");

            return dt.Rows[0]["makh"].ToString();
        }
    }
}