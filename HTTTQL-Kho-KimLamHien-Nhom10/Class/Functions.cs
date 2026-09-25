using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Class
{
    internal class Functions
    {
        public static SqlConnection Conn;  //khai báo đối tượng kết nối
        public static string connString;   //khai báo biến chứa chuỗi kết nối

        public static void Ketnoi()
        {
            //Thiết lập giá trị cho chuỗi kết nối
            connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=QLDA_NHOM_10;Integrated Security=True;Encrypt=False";

            Conn = new SqlConnection();        //Cấp phát đối tượng
            Conn.ConnectionString = connString; //Kết nối
            Conn.Open();                       //Mở kết nối
            //MessageBox.Show("Bạn đã kết nối thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Ngatketnoi()
        {
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();   	//Đóng kết nối
                    Conn.Dispose();     //giải phóng tài nguyên
                }
                Conn = null;
            }
        }
        public static DataTable GetDataToTable(string sql)
        {
            Ketnoi();
            SqlDataAdapter Mydata = new SqlDataAdapter();	// Khai báo
            // Tạo đối tượng Command thực hiện câu lệnh SELECT        
            Mydata.SelectCommand = new SqlCommand();
            Mydata.SelectCommand.Connection = Functions.Conn; 	// Kết nối CSDL
            Mydata.SelectCommand.CommandText = sql;	// Gán câu lệnh SELECT
            DataTable table = new DataTable();    // Khai báo DataTable nhận dữ liệu trả về
            Mydata.Fill(table); 	//Thực hiện câu lệnh SELECT và đổ dữ liệu vào bảng table
            Ngatketnoi();
            return table;
        }
        public static bool KiemTraNhanVienTonTai()
        {
            string sql = "SELECT COUNT(*) FROM nhan_vien";
            try
            {
                Ketnoi();
                SqlCommand cmd = new SqlCommand(sql, Conn);
                int Soluong = Convert.ToInt32(cmd.ExecuteScalar());
                return Soluong > 0; //trả về true là có nhân viên
            }
            finally
            {
                Ngatketnoi();
            }
        }
        public static string TaoMatKhauNgauNhien()
        {
            string hoa = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string thuong = "abcdefghijklmnopqrstuvwxyz";
            string so = "23456789";
            string kitu = "@#$!";
            string tatca = hoa + thuong + so + kitu;

            Random a = new Random();

            string matkhau = "";
            matkhau = matkhau + hoa[a.Next(hoa.Length)];
            matkhau = matkhau + thuong[a.Next(thuong.Length)];
            matkhau = matkhau + so[a.Next(so.Length)];
            matkhau = matkhau + kitu[a.Next(kitu.Length)];
            matkhau = matkhau + tatca[a.Next(tatca.Length)];
            matkhau = matkhau + tatca[a.Next(tatca.Length)];

            // Xáo trộn thứ tự để không đoán được vị trí từng loại ký tự
            char[] arr = matkhau.ToCharArray();
            for (int i = arr.Length - 1; i > 0; i--)
            {
                int j = a.Next(i + 1);
                char tmp = arr[i];
                arr[i] = arr[j];
                arr[j] = tmp;
            }
            return new string(arr);
        }
        public static void RunSql(string sql)
        {
            SqlCommand cmd;		                // Khai báo đối tượng SqlCommand
            cmd = new SqlCommand();	         // Khởi tạo đối tượng
            cmd.Connection = Functions.Conn;	  // Gán kết nối
            cmd.CommandText = sql;			  // Gán câu lệnh SQL
            try
            {
                cmd.ExecuteNonQuery();		  // Thực hiện câu lệnh SQL
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;
        }
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter Mydata = new SqlDataAdapter(sql, Functions.Conn);
            DataTable table = new DataTable();
            Mydata.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public static void RunSqlDel(string sql)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Functions.Conn;
            cmd.CommandText = sql;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception)
            {
                MessageBox.Show("Dữ liệu đang được dùng, không thể xóa...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            cmd.Dispose();
            cmd = null;
        }
        public static void FillCombo(string sql, ComboBox cbo, string ma, string ten)
        {
            SqlDataAdapter Mydata = new SqlDataAdapter(sql, Functions.Conn);
            DataTable table = new DataTable();
            Mydata.Fill(table);
            cbo.DataSource = table;

            cbo.ValueMember = ma;    // Truong gia tri
            cbo.DisplayMember = ten;    // Truong hien thi
        }
        public static string SinhMa(string tenbang, string tencot, string tiento)
        {
            string sql = "SELECT " + tencot + " FROM " + tenbang;
            DataTable tbl = GetDataToTable(sql);

            int max = 0;

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                string ma = tbl.Rows[i][tencot].ToString();

                if (ma.StartsWith(tiento))
                {
                    int so;
                    if (int.TryParse(ma.Substring(tiento.Length), out so))
                    {
                        if (so > max)
                            max = so;
                    }
                }
            }
            max++;
            return tiento + max.ToString("000");
        }
        public static bool ThucThiVoiTransaction(string[] danhSachSql)
        {
            Ketnoi();
            SqlTransaction transaction = Conn.BeginTransaction();

            for (int i = 0; i < danhSachSql.Length; i++)
            {
                SqlCommand cmd = new SqlCommand(danhSachSql[i], Conn, transaction);
                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
            Ngatketnoi();
            return true;
        }
        public static void CapNhatMatKhau(string tendangnhap, string hash, bool laLanDau)
        {
            string sql;
            sql = "UPDATE nhan_vien " +
                  "SET matkhau     = @Hash, " +
                  "    lanDNdau    = @LaLanDau, " +
                  "    ngaycapnhat = GETDATE() " +
                  "WHERE tendangnhap = @TenDN";

            Class.Functions.Ketnoi();
            SqlCommand cmd = new SqlCommand(sql, Class.Functions.Conn);
            cmd.Parameters.AddWithValue("@Hash", hash);
            cmd.Parameters.AddWithValue("@TenDN", tendangnhap);

            if (laLanDau == true)
                cmd.Parameters.AddWithValue("@LaLanDau", 1);
            else
                cmd.Parameters.AddWithValue("@LaLanDau", 0);

            cmd.ExecuteNonQuery();
            Class.Functions.Ngatketnoi();
        }
        public static bool IsDate(string d) //kiem tra dlieu nhap vao co phai la kieu so ko 
        {
            string[] parts = d.Split('/'); //tach cac phan tu trong mang thanh cac mang rieng bang dau /
            if ((Convert.ToInt32(parts[0]) >= 1) && (Convert.ToInt32(parts[0]) <= 31) && (Convert.ToInt32(parts[1]) >= 1) && (Convert.ToInt32(parts[1]) <= 12) && (Convert.ToInt32(parts[2]) >= 1900))
                return true;
            else
                return false;
        }
        public static string ConvertDateTime(string d)
        {
            string[] parts = d.Split('/');
            string dt = String.Format("{0}/{1}/{2}", parts[1], parts[0], parts[2]);
            return dt;
        }

        public static decimal ParseValueFromMota(string mota, string key)
        {
            if (string.IsNullOrEmpty(mota)) return 0;
            int idx = mota.IndexOf(key);
            if (idx < 0) return 0;
            int start = idx + key.Length;
            int end = mota.IndexOf("|", start);
            string valStr = end < 0 ? mota.Substring(start) : mota.Substring(start, end - start);
            decimal val = 0;
            decimal.TryParse(valStr.Trim(), out val);
            return val;
        }

        public static void ApplyGlobalStyles(Control parent)
        {
            if (parent == null) return;
            string formName = parent.FindForm()?.Name ?? "";
            bool isDashboardForm = formName == "FrmGiaodienAdmin" ||
                                   formName == "FrmGiaodienbanhang" ||
                                   formName == "FrmGiaodienquanlykho" ||
                                   formName == "FrmGiaodienKT" ||
                                   formName == "FrmMain" ||
                                   formName == "FrmLogin";
            ApplyGlobalStylesInternal(parent, isDashboardForm);
        }

        private static void ApplyGlobalStylesInternal(Control parent, bool isDashboardForm)
        {
            foreach (Control ctrl in parent.Controls)
            {
                string typeName = ctrl.GetType().Name;
                if (ctrl is TextBoxBase || typeName.Contains("TextBox"))
                {
                    ctrl.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Regular);
                }
                else if (ctrl is ComboBox || typeName == "Guna2ComboBox")
                {
                    ctrl.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Regular);
                }
                else if (!isDashboardForm && (ctrl is Button || typeName == "Guna2Button"))
                {
                    ctrl.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold);
                }
                else if (ctrl is DataGridView)
                {
                    DataGridView dgv = (DataGridView)ctrl;
                    FormatDataGridView(dgv);

                    dgv.DataBindingComplete -= Dgv_DataBindingComplete;
                    dgv.DataBindingComplete += Dgv_DataBindingComplete;
                }

                if (ctrl.HasChildren)
                {
                    ApplyGlobalStylesInternal(ctrl, isDashboardForm);
                }
            }
        }

        private static void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (sender is DataGridView dgv)
            {
                FormatDataGridView(dgv);
            }
        }

        private static void FormatDataGridView(DataGridView dgv)
        {
            try
            {
                dgv.ColumnHeadersHeight = 60;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgv.RowTemplate.Height = 40;

                // Bắt buộc đặt EnableHeadersVisualStyles = false để tùy biến màu nền Header
                dgv.EnableHeadersVisualStyles = false;

                // Định dạng Header mặc định: nền đen, chữ goldenrod, select nền oldlace chữ goldenrod
                dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Goldenrod;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.OldLace;
                dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Goldenrod;
                dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);

                // Định dạng dòng mặc định: nền trắng, chữ đen, select nền PapayaWhip chữ đen
                dgv.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.PapayaWhip;
                dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Regular);

                // Nếu là Guna2DataGridView thì cấu hình thêm ThemeStyle để áp dụng chuẩn màu ngay khi load
                if (dgv.GetType().Name == "Guna2DataGridView")
                {
                    dynamic gunaDgv = dgv;
                    gunaDgv.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Default;

                    gunaDgv.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.Black;
                    gunaDgv.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Goldenrod;
                    gunaDgv.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
                    gunaDgv.ThemeStyle.HeaderStyle.Height = 60;

                    gunaDgv.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
                    gunaDgv.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
                    gunaDgv.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.PapayaWhip;
                    gunaDgv.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
                    gunaDgv.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Regular);
                }

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    row.Height = 40;
                }

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    col.HeaderCell.Style.BackColor = System.Drawing.Color.Black;
                    col.HeaderCell.Style.ForeColor = System.Drawing.Color.Goldenrod;
                    col.HeaderCell.Style.SelectionBackColor = System.Drawing.Color.OldLace;
                    col.HeaderCell.Style.SelectionForeColor = System.Drawing.Color.Goldenrod;
                    col.HeaderCell.Style.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);

                    col.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    col.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    col.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.PapayaWhip;
                    col.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                    col.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Regular);
                }
            }
            catch (Exception)
            {
                // Bỏ qua lỗi
            }
        }
        // Truy vấn trả DataTable với tham số
        public static DataTable GetDataToTableParam(string sql, params SqlParameter[] prms)
        {
            Ketnoi();
            DataTable dt = new DataTable();
            using (SqlCommand cmd = new SqlCommand(sql, Conn))
            {
                cmd.Parameters.AddRange(prms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
            }
            Ngatketnoi();
            return dt;
        }

        // ExecuteNonQuery với tham số
        public static void ExecuteNonQueryParam(string sql, params SqlParameter[] prms)
        {
            Ketnoi();
            using (SqlCommand cmd = new SqlCommand(sql, Conn))
            {
                cmd.Parameters.AddRange(prms);
                cmd.ExecuteNonQuery();
            }
            Ngatketnoi();
        }

        // ExecuteScalar với tham số
        public static object ExecuteScalarParam(string sql, params SqlParameter[] prms)
        {
            Ketnoi();
            using (SqlCommand cmd = new SqlCommand(sql, Conn))
            {
                cmd.Parameters.AddRange(prms);
                object val = cmd.ExecuteScalar();
                Ngatketnoi();
                return val;
            }
        }
    }
}
