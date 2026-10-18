using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmThemkhachhang : Form
    {
        public string MaKHMoi { get; set; } = "";
        public string TenKHMoi { get; set; } = "";
        public string PreFilledSDT { get; set; } = "";

        public FrmThemkhachhang()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
        }

        public FrmThemkhachhang(string sdtSanCo)
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.PreFilledSDT = sdtSanCo;
        }

        private void FrmThemkhachhang_Load(object sender, EventArgs e)
        {
            HienThongBaoLoi("");

            string iconPath = Path.Combine(Application.StartupPath, "Images", "Giaodien", "logo1.ico");
            if (File.Exists(iconPath))
            {
                this.ShowIcon = true;
                this.Icon = new Icon(iconPath);

                using (var bmp = new Bitmap(iconPath))
                {
                    pic1.Image = new Bitmap(bmp);
                }
                pic1.SizeMode = PictureBoxSizeMode.Zoom;
            }

            // Điền sẵn số điện thoại từ form Hóa đơn nếu có
            if (!string.IsNullOrEmpty(PreFilledSDT))
            {
                GanGiaTriSDT(PreFilledSDT);
            }

            // Focus vào ô họ tên
            Control txtName = LayControl("txtHoten") ?? LayControl("txtTenKH");
            if (txtName != null) txtName.Focus();
        }

        // Tự động sinh mã khách hàng KH0001, KH0002...
        private string SinhMaKhachHang()
        {
            string maMoi = "KH0001";
            try
            {
                string sql = "SELECT TOP 1 makh FROM khach_hang WHERE makh LIKE 'KH%' ORDER BY makh DESC";
                DataTable dt = Functions.GetDataToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string maCu = dt.Rows[0]["makh"].ToString().Trim();
                    string soStr = maCu.Substring(2);
                    if (int.TryParse(soStr, out int so))
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

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string hoTen = LayTextControl("txtHoten") ?? LayTextControl("txtTenKH") ?? "";
            string rawSDT = LaySDTChuan();
            string diaChi = LayTextControl("txtDiachi") ?? "";

            // 1. Kiểm tra Họ tên (Bắt buộc không rỗng)
            if (string.IsNullOrEmpty(hoTen))
            {
                HienThongBaoLoi("⚠ Vui lòng nhập họ và tên khách hàng!");
                FocusControl("txtHoten", "txtTenKH");
                return;
            }

            // 2. Kiểm tra Số điện thoại (Bắt buộc, đủ 10 số)
            if (string.IsNullOrEmpty(rawSDT))
            {
                HienThongBaoLoi("⚠ Vui lòng nhập số điện thoại khách hàng!");
                FocusControl("txtSDT", "mskSDT");
                return;
            }

            if (rawSDT.Length < 10)
            {
                HienThongBaoLoi("⚠ Số điện thoại chưa nhập đủ 10 số!");
                FocusControl("txtSDT", "mskSDT");
                return;
            }

            // 3. Kiểm tra Địa chỉ (Bắt buộc)
            if (string.IsNullOrEmpty(diaChi))
            {
                HienThongBaoLoi("⚠ Vui lòng nhập địa chỉ của khách hàng!");
                FocusControl("txtDiachi");
                return;
            }

            // 4. Kiểm tra trùng số điện thoại trong DB
            try
            {
                string sqlCheck = $"SELECT COUNT(*) FROM khach_hang WHERE sdt = '{rawSDT}'";
                int count = Convert.ToInt32(Functions.ExecuteScalarParam(sqlCheck) ?? 0);
                if (count > 0)
                {
                    HienThongBaoLoi($"⚠ Số điện thoại '{rawSDT}' đã được đăng ký cho khách hàng khác!");
                    FocusControl("txtSDT", "mskSDT");
                    return;
                }

                // 5. Insert vào Database
                string maKH = SinhMaKhachHang();
                string sqlInsert = @"INSERT INTO khach_hang (makh, tenkh, diachi, sdt)
                                     VALUES (@makh, @tenkh, @diachi, @sdt)";

                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@makh", maKH),
                    new SqlParameter("@tenkh", hoTen),
                    new SqlParameter("@diachi", diaChi),
                    new SqlParameter("@sdt", rawSDT)
                };

                Functions.ExecuteNonQueryParam(sqlInsert, prms);

                // Gán dữ liệu trả về cho Form Hóa đơn
                MaKHMoi = maKH;
                TenKHMoi = hoTen;

                MessageBox.Show("Thêm mới khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                HienThongBaoLoi("⚠ Lỗi khi lưu dữ liệu: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ================= CÁC HÀM TIỆN ÍCH HỖ TRỢ CẢ 2 CÁCH ĐẶT TÊN CONTROL =================

        private Control LayControl(string name)
        {
            Control[] arr = this.Controls.Find(name, true);
            return (arr.Length > 0) ? arr[0] : null;
        }

        private string LayTextControl(string name)
        {
            Control c = LayControl(name);
            return c != null ? c.Text.Trim() : null;
        }

        private void FocusControl(params string[] names)
        {
            foreach (var n in names)
            {
                Control c = LayControl(n);
                if (c != null)
                {
                    c.Focus();
                    return;
                }
            }
        }

        private string LaySDTChuan()
        {
            string sdt = LayTextControl("txtSDT") ?? LayTextControl("mskSDT") ?? "";
            return sdt.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "").Trim();
        }

        private void GanGiaTriSDT(string val)
        {
            Control c = LayControl("txtSDT") ?? LayControl("mskSDT");
            if (c != null) c.Text = val;
        }

        private void HienThongBaoLoi(string thongBao)
        {
            Control lbl = LayControl("lblThongbao");
            if (lbl != null)
            {
                if (string.IsNullOrEmpty(thongBao))
                {
                    lbl.Text = "";
                    lbl.Visible = false;
                }
                else
                {
                    lbl.Text = thongBao;
                    lbl.ForeColor = Color.Red;
                    lbl.Visible = true;
                }
            }
            else if (!string.IsNullOrEmpty(thongBao))
            {
                MessageBox.Show(thongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}