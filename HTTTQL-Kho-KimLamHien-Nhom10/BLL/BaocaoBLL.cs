using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;
using HTTTQL_Kho_KimLamHien_Nhom10.DAL;

namespace HTTTQL_Kho_KimLamHien_Nhom10.BLL
{
    public enum KyBaoCao { Ngay, Thang, Nam }
    internal class BaocaoBLL
    {
        private const string TT_HOADON_HOANTHANH = "Hoàn thành";
        private const string TT_HD_CHOXACNHAN = "Chờ xác nhận";
        private const string TT_HD_DANGXULY = "Đang xử lý";
        private const string TT_DEXUAT_DADUYET = "Đã duyệt";
        private const string TT_NHAP_HOANTHANH = "Hoàn thành";
        private const string TT_XUAT_HOANTHANH = "Hoàn thành";
        // ===================== DÙNG CHUNG =====================
        // Đổi lựa chọn trên form thành khoảng ngày thật. Sai thì ném Exception.
        public static void TinhKhoang(KyBaoCao ky, DateTime tuNgay, DateTime denNgay,
                                      int thang, int nam, out DateTime tu, out DateTime den)
        {
            if (ky == KyBaoCao.Ngay)
            {
                if (tuNgay.Date > denNgay.Date)
                    throw new Exception("Từ ngày không được lớn hơn đến ngày.");
                tu = tuNgay.Date;
                den = denNgay.Date;
            }
            else if (ky == KyBaoCao.Thang)
            {
                if (thang < 1 || thang > 12) throw new Exception("Vui lòng chọn tháng.");
                if (nam < 1900) throw new Exception("Vui lòng chọn năm.");
                tu = new DateTime(nam, thang, 1);
                den = tu.AddMonths(1).AddDays(-1);
            }
            else
            {
                if (nam < 1900) throw new Exception("Vui lòng chọn năm.");
                tu = new DateTime(nam, 1, 1);
                den = new DateTime(nam, 12, 31);
            }
        }
        // ===================== BÁO CÁO DOANH THU =====================
        public static bool ChoPhepXemDoanhThu()
        {
            return session.LaGiamDoc() || session.LaNhanVienKeToan();
        }
        public static DataTable LayDanhSachHoaDon(DateTime tu, DateTime den)
        {
            return BaocaoDAL.LayDanhSachHoaDon(tu, den, TT_HOADON_HOANTHANH);
        }
        // Dữ liệu biểu đồ, đủ mọi mốc (ngày/tháng không có đơn = 0). Cột: Nhan, DoanhThu
        public static DataTable LayDuLieuBieuDo(KyBaoCao ky, DateTime tu, DateTime den)
        {
            DataTable kq = new DataTable();
            kq.Columns.Add("Nhan", typeof(string));
            kq.Columns.Add("DoanhThu", typeof(decimal));

            if (ky == KyBaoCao.Nam)
            {
                DataTable raw = BaocaoDAL.LayDoanhThuTheoThang(tu.Year, TT_HOADON_HOANTHANH);
                for (int m = 1; m <= 12; m++)
                {
                    decimal tong = 0;
                    for (int j = 0; j < raw.Rows.Count; j++)
                    {
                        if (Convert.ToInt32(raw.Rows[j]["thang"]) == m)
                            tong = Convert.ToDecimal(raw.Rows[j]["doanhthu"]);
                    }
                    kq.Rows.Add("T" + m, tong);
                }
            }
            else
            {
                DataTable raw = BaocaoDAL.LayDoanhThuTheoNgay(tu, den, TT_HOADON_HOANTHANH);
                for (DateTime d = tu.Date; d <= den.Date; d = d.AddDays(1))
                {
                    decimal tong = 0;
                    for (int j = 0; j < raw.Rows.Count; j++)
                    {
                        if (Convert.ToDateTime(raw.Rows[j]["ngay"]).Date == d)
                            tong = Convert.ToDecimal(raw.Rows[j]["doanhthu"]);
                    }
                    kq.Rows.Add(d.ToString("dd/MM"), tong);
                }
            }
            return kq;
        }
        // Tổng doanh thu và số đơn từ bảng danh sách hóa đơn
        public static decimal TinhTong(DataTable dt, out int soDon)
        {
            decimal tong = 0;
            soDon = dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
                tong += Convert.ToDecimal(dt.Rows[i]["tongtien"]);
            return tong;
        }
        // ===== Xuất Excel (OpenXML) =====
        public static void XuatExcel(DataTable dt, string duongDan, string tieuDe)
        {
            if (dt == null || dt.Rows.Count == 0)
                throw new Exception("Không có dữ liệu để xuất.");

            string[] tieuDeCot = { "Mã hóa đơn", "Loại đơn", "Tên khách hàng", "Ngày lập", "Ngày cập nhật",
                                   "PT thanh toán", "Nhân viên lập", "Tổng tiền", "Tổng số lượng" };
            string[] tenCot = { "mahd", "loaidon", "tenkh", "ngaylap", "ngaycapnhat",
                                "ptthanhtoan", "tennv", "tongtien", "tongsl" };

            using (SpreadsheetDocument doc = SpreadsheetDocument.Create(duongDan, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart wb = doc.AddWorkbookPart();
                wb.Workbook = new Workbook();
                WorksheetPart ws = wb.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                ws.Worksheet = new Worksheet(sheetData);

                Sheets sheets = wb.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet { Id = wb.GetIdOfPart(ws), SheetId = 1, Name = "Doanh thu" });

                // Dòng tiêu đề + dòng trống
                Row r1 = new Row();
                r1.Append(TaoChu(tieuDe));
                sheetData.Append(r1);
                sheetData.Append(new Row());

                // Dòng tiêu đề cột
                Row hdr = new Row();
                for (int c = 0; c < tieuDeCot.Length; c++)
                    hdr.Append(TaoChu(tieuDeCot[c]));
                sheetData.Append(hdr);

                // Dữ liệu (ô NULL ra chuỗi rỗng)
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Row r = new Row();
                    for (int c = 0; c < tenCot.Length; c++)
                    {
                        object v = dt.Rows[i][tenCot[c]];
                        if (v is DateTime)
                            r.Append(TaoChu(((DateTime)v).ToString("dd/MM/yyyy HH:mm")));
                        else if (v is decimal || v is int || v is long)
                            r.Append(TaoSo(Convert.ToDecimal(v)));
                        else
                            r.Append(TaoChu(v == null || v == DBNull.Value ? "" : v.ToString()));
                    }
                    sheetData.Append(r);
                }

                // Dòng tổng: cột Tổng tiền là cột thứ 8 nên có 6 ô trống phía trước
                int soDon;
                decimal tong = TinhTong(dt, out soDon);
                Row rt = new Row();
                rt.Append(TaoChu("TỔNG"));
                for (int i = 0; i < 6; i++) rt.Append(TaoChu(""));
                rt.Append(TaoSo(tong));
                sheetData.Append(rt);

                wb.Workbook.Save();
            }
        }
        // ===================== BÁO CÁO TỒN KHO =====================
        // Combo loại hàng, dòng đầu là "Tất cả" (giá trị rỗng)
        public static DataTable LayLoaiHangCombo()
        {
            DataTable dt = BaocaoDAL.LayLoaiHang();
            DataRow r = dt.NewRow();
            r["maloai"] = "";
            r["tenloai"] = "Tất cả";
            dt.Rows.InsertAt(r, 0);
            return dt;
        }
        // Mốc tồn cuối kỳ (không vượt quá hôm nay)
        public static DateTime TinhMocTon(KyBaoCao ky, DateTime tuNgay, DateTime denNgay, int thang, int nam)
        {
            DateTime tu, den;
            TinhKhoang(ky, tuNgay, denNgay, thang, nam, out tu, out den);
            return den > DateTime.Today ? DateTime.Today : den;
        }
        public static DataTable LayTonKho(string maloai, bool chiCanhBao, DateTime moc)
        {
            return BaocaoDAL.LayTonKho(maloai, chiCanhBao, moc,
                new string[] { TT_HD_CHOXACNHAN, TT_HD_DANGXULY }, TT_DEXUAT_DADUYET,
                TT_NHAP_HOANTHANH, TT_XUAT_HOANTHANH);
        }
        public static void XuatExcelTonKho(DataTable dt, string duongDan, string tieuDe)
        {
            if (dt == null || dt.Rows.Count == 0)
                throw new Exception("Không có dữ liệu để xuất.");

            string[] tieuDeCot = { "Mã hàng", "Tên hàng", "Loại hàng", "Tồn thực tế",
                                   "Tồn khả dụng", "Tồn dự kiến", "Ngưỡng cảnh báo", "MaTheLoai" };
            string[] tenCot = { "mahang", "tenhang", "tenloai", "tonthucte",
                                "tonkhadung", "tondukien", "nguongcanhbao", "maloai" };

            using (SpreadsheetDocument doc = SpreadsheetDocument.Create(duongDan, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart wb = doc.AddWorkbookPart();
                wb.Workbook = new Workbook();
                WorksheetPart ws = wb.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                ws.Worksheet = new Worksheet(sheetData);

                Sheets sheets = wb.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet { Id = wb.GetIdOfPart(ws), SheetId = 1, Name = "Ton kho" });

                Row r1 = new Row();
                r1.Append(TaoChu(tieuDe));
                sheetData.Append(r1);
                sheetData.Append(new Row());

                Row hdr = new Row();
                for (int c = 0; c < tieuDeCot.Length; c++) hdr.Append(TaoChu(tieuDeCot[c]));
                sheetData.Append(hdr);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Row r = new Row();
                    for (int c = 0; c < tenCot.Length; c++)
                    {
                        object v = dt.Rows[i][tenCot[c]];
                        if (v is decimal || v is int || v is long)
                            r.Append(TaoSo(Convert.ToDecimal(v)));
                        else
                            r.Append(TaoChu(v == null || v == DBNull.Value ? "" : v.ToString()));
                    }
                    sheetData.Append(r);
                }
                wb.Workbook.Save();
            }
        }
        // ===================== BÁO CÁO BÁN CHẠY =====================
        public static DataTable LayBanChay(int top, string maloai, DateTime tu, DateTime den)
        {
            if (top < 1) throw new Exception("Số sản phẩm phải lớn hơn hoặc bằng 1.");
            if (tu.Date > den.Date) throw new Exception("Từ ngày không được lớn hơn đến ngày.");
            return BaocaoDAL.LayBanChay(top, maloai, tu, den, TT_HOADON_HOANTHANH);
        }
        public static void XuatExcelBanChay(DataTable dt, string duongDan, string tieuDe)
        {
            if (dt == null || dt.Rows.Count == 0)
                throw new Exception("Không có dữ liệu để xuất.");

            string[] tieuDeCot = { "Mã hàng", "Tên hàng", "Loại hàng", "ĐVT", "Số lượng bán", "Doanh thu (VNĐ)" };
            string[] tenCot = { "mahang", "tenhang", "tenloai", "dvt", "soluongban", "doanhthu" };

            using (SpreadsheetDocument doc = SpreadsheetDocument.Create(duongDan, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart wb = doc.AddWorkbookPart();
                wb.Workbook = new Workbook();
                WorksheetPart ws = wb.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                ws.Worksheet = new Worksheet(sheetData);

                Sheets sheets = wb.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet { Id = wb.GetIdOfPart(ws), SheetId = 1, Name = "Ban chay" });

                Row r1 = new Row();
                r1.Append(TaoChu(tieuDe));
                sheetData.Append(r1);
                sheetData.Append(new Row());

                Row hdr = new Row();
                for (int c = 0; c < tieuDeCot.Length; c++) hdr.Append(TaoChu(tieuDeCot[c]));
                sheetData.Append(hdr);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Row r = new Row();
                    for (int c = 0; c < tenCot.Length; c++)
                    {
                        object v = dt.Rows[i][tenCot[c]];
                        if (v is decimal || v is int || v is long)
                            r.Append(TaoSo(Convert.ToDecimal(v)));
                        else
                            r.Append(TaoChu(v == null || v == DBNull.Value ? "" : v.ToString()));
                    }
                    sheetData.Append(r);
                }
                wb.Workbook.Save();
            }
        }
        // ===================== HÀM PHỤ EXCEL =====================
        private static Cell TaoChu(string s)
        {
            return new Cell
            {
                DataType = CellValues.InlineString,
                InlineString = new InlineString(new Text(s))
            };
        }
        private static Cell TaoSo(decimal v)
        {
            return new Cell
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(v.ToString(CultureInfo.InvariantCulture))
            };
        }
    }
}
