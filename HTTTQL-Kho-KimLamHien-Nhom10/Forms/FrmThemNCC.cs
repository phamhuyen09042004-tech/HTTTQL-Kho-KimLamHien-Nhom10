using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmThemNCC : Form
    {
        private string cheDo = "THEM";
        private string maNCCSua = "";

        public string MaNCCMoi = "";
        public string TenNCCMoi = "";

        public FrmThemNCC()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = "THEM";
        }

        public FrmThemNCC(string cheDo, string maNCC)
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = cheDo;
            this.maNCCSua = maNCC;
        }

        private void FrmThemNCC_Load(object sender, EventArgs e)
        {
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

            lblCanhbao.Visible = false;
            lblCanhbao.Text = "⚠ Nhà cung cấp đã tồn tại!";

            if (cheDo == "SUA")
            {
                this.Text = "Sửa nhà cung cấp";
                label1.Text = "SỬA NHÀ CUNG CẤP";
                LoadThongTin();
            }
            else
            {
                this.Text = "Thêm mới nhà cung cấp";
                label1.Text = "THÊM NHÀ CUNG CẤP";
            }

            txtNCC.Focus();
        }

        // Tự động sinh mã nhà cung cấp mới: NCC001, NCC002...
        private string SinhMaNCC()
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

        // Kiểm tra trùng tên nhà cung cấp trong CSDL
        private bool KiemTraTenTrung(string tenNCC)
        {
            try
            {
                string sql = $"SELECT COUNT(*) FROM nha_cung_cap WHERE tenncc = N'{tenNCC}'";
                if (cheDo == "SUA")
                {
                    sql += $" AND mancc <> '{maNCCSua}'";
                }
                int count = Convert.ToInt32(Functions.ExecuteScalarParam(sql) ?? 0);
                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        private void txtNCC_Leave(object sender, EventArgs e)
        {
            string tenNCC = txtNCC.Text.Trim();
            if (string.IsNullOrEmpty(tenNCC))
            {
                lblCanhbao.Visible = false;
                return;
            }

            lblCanhbao.Visible = KiemTraTenTrung(tenNCC);
        }

        private void txtNCC_TextChanged(object sender, EventArgs e)
        {
            lblCanhbao.Visible = false;
        }

        private void LoadThongTin()
        {
            try
            {
                string sql = $"SELECT mancc, tenncc, sdt, diachi FROM nha_cung_cap WHERE mancc = '{maNCCSua}'";
                DataTable dt = Functions.GetDataToTable(sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                DataRow row = dt.Rows[0];
                txtNCC.Text = row["tenncc"].ToString();
                mskSDT.Text = row["sdt"].ToString();
                txtDiachi.Text = row["diachi"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy thông tin: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenNCC = txtNCC.Text.Trim();
            string diaChi = txtDiachi.Text.Trim();
            string rawSDT = mskSDT.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "").Trim();

            // 1. Kiểm tra tên nhà cung cấp
            if (string.IsNullOrEmpty(tenNCC))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNCC.Focus();
                return;
            }

            // 2. Kiểm tra tên trùng
            if (lblCanhbao.Visible || KiemTraTenTrung(tenNCC))
            {
                MessageBox.Show("Tên nhà cung cấp đã tồn tại, vui lòng nhập tên khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblCanhbao.Visible = true;
                txtNCC.Focus();
                return;
            }

            // 3. Kiểm tra số điện thoại (bắt buộc đủ 10 chữ số)
            if (string.IsNullOrEmpty(rawSDT))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSDT.Focus();
                return;
            }

            if (!mskSDT.MaskFull || rawSDT.Length < 10)
            {
                MessageBox.Show("Số điện thoại chưa nhập đủ 10 số. Vui lòng nhập đầy đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mskSDT.Focus();
                return;
            }

            // 4. Kiểm tra địa chỉ
            if (string.IsNullOrEmpty(diaChi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiachi.Focus();
                return;
            }

            // 5. Lưu vào Database
            try
            {
                if (cheDo == "THEM")
                {
                    string maMoi = SinhMaNCC();
                    string sqlInsert = @"INSERT INTO nha_cung_cap (mancc, tenncc, sdt, diachi, trangthai)
                                         VALUES (@mancc, @tenncc, @sdt, @diachi, N'Hoạt động')";

                    SqlParameter[] prms = new SqlParameter[]
                    {
                        new SqlParameter("@mancc", maMoi),
                        new SqlParameter("@tenncc", tenNCC),
                        new SqlParameter("@sdt", rawSDT),
                        new SqlParameter("@diachi", diaChi)
                    };

                    Functions.ExecuteNonQueryParam(sqlInsert, prms);

                    MaNCCMoi = maMoi;
                    TenNCCMoi = tenNCC;

                    MessageBox.Show("Thêm nhà cung cấp mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string sqlUpdate = @"UPDATE nha_cung_cap 
                                         SET tenncc = @tenncc, sdt = @sdt, diachi = @diachi 
                                         WHERE mancc = @mancc";

                    SqlParameter[] prms = new SqlParameter[]
                    {
                        new SqlParameter("@mancc", maNCCSua),
                        new SqlParameter("@tenncc", tenNCC),
                        new SqlParameter("@sdt", rawSDT),
                        new SqlParameter("@diachi", diaChi)
                    };

                    Functions.ExecuteNonQueryParam(sqlUpdate, prms);

                    MaNCCMoi = maNCCSua;
                    TenNCCMoi = tenNCC;

                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsDataChanged()
        {
            if (!string.IsNullOrEmpty(txtNCC.Text.Trim())) return true;
            string rawSDT = mskSDT.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "").Trim();
            if (!string.IsNullOrEmpty(rawSDT)) return true;
            if (!string.IsNullOrEmpty(txtDiachi.Text.Trim())) return true;
            return false;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (!IsDataChanged())
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            if (MessageBox.Show("Dữ liệu đã nhập chưa được lưu. Bạn có muốn hủy không?", "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}