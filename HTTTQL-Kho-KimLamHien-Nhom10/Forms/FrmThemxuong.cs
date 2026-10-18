using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmThemxuong : Form
    {
        private string cheDo = "THEM";
        private string maXuongSua = "";

        public string MaXuongMoi = "";
        public string TenXuongMoi = "";

        public FrmThemxuong()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = "THEM";
        }

        public FrmThemxuong(string cheDo, string maXuong)
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.cheDo = cheDo;
            this.maXuongSua = maXuong;
        }

        private void FrmThemxuong_Load(object sender, EventArgs e)
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
            lblCanhbao.Text = "⚠ Xưởng chế tác đã tồn tại!";

            if (cheDo == "SUA")
            {
                this.Text = "Sửa xưởng chế tác";
                label3.Text = "SỬA XƯỞNG CHẾ TÁC";
                LoadThongTin();
            }
            else
            {
                this.Text = "Thêm xưởng chế tác";
                label3.Text = "THÊM XƯỞNG CHẾ TÁC";
            }

            txtTenXuong.Focus();
        }

        // Tự động sinh mã xưởng mới: XC001, XC002...
        private string SinhMaXuong()
        {
            string maMoi = "XC001";
            try
            {
                string sql = "SELECT TOP 1 maxuong FROM xuong_che_tac WHERE maxuong LIKE 'XC%' ORDER BY maxuong DESC";
                DataTable dt = Functions.GetDataToTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string maCu = dt.Rows[0]["maxuong"].ToString().Trim();
                    string soStr = maCu.Substring(2);
                    if (int.TryParse(soStr, out int so))
                    {
                        maMoi = "XC" + (so + 1).ToString("D3");
                    }
                }
            }
            catch
            {
                maMoi = "XC001";
            }
            return maMoi;
        }

        // Kiểm tra trùng tên xưởng trong CSDL
        private bool KiemTraTenTrung(string tenXuong)
        {
            try
            {
                string sql = $"SELECT COUNT(*) FROM xuong_che_tac WHERE tenxuong = N'{tenXuong}'";
                if (cheDo == "SUA")
                {
                    sql += $" AND maxuong <> '{maXuongSua}'";
                }
                int count = Convert.ToInt32(Functions.ExecuteScalarParam(sql) ?? 0);
                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        private void txtTenXuong_Leave(object sender, EventArgs e)
        {
            string tenXuong = txtTenXuong.Text.Trim();
            if (string.IsNullOrEmpty(tenXuong))
            {
                lblCanhbao.Visible = false;
                return;
            }

            lblCanhbao.Visible = KiemTraTenTrung(tenXuong);
        }

        private void txtTenXuong_TextChanged(object sender, EventArgs e)
        {
            lblCanhbao.Visible = false;
        }

        private void LoadThongTin()
        {
            try
            {
                string sql = $"SELECT maxuong, tenxuong, sdt, diachi FROM xuong_che_tac WHERE maxuong = '{maXuongSua}'";
                DataTable dt = Functions.GetDataToTable(sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin xưởng chế tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                DataRow row = dt.Rows[0];
                txtTenXuong.Text = row["tenxuong"].ToString();
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
            string tenXuong = txtTenXuong.Text.Trim();
            string diaChi = txtDiachi.Text.Trim();
            string rawSDT = mskSDT.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("_", "").Trim();

            // 1. Kiểm tra bắt buộc nhập tên xưởng
            if (string.IsNullOrEmpty(tenXuong))
            {
                MessageBox.Show("Vui lòng nhập tên xưởng chế tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenXuong.Focus();
                return;
            }

            // 2. Kiểm tra tên xưởng đã tồn tại
            if (lblCanhbao.Visible || KiemTraTenTrung(tenXuong))
            {
                MessageBox.Show("Xưởng chế tác đã tồn tại, vui lòng nhập tên khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblCanhbao.Visible = true;
                txtTenXuong.Focus();
                return;
            }

            // 3. Kiểm tra số điện thoại
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
                MessageBox.Show("Vui lòng nhập địa chỉ xưởng chế tác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiachi.Focus();
                return;
            }

            // 5. Lưu vào Database
            try
            {
                if (cheDo == "THEM")
                {
                    string maMoi = SinhMaXuong();
                    string sqlInsert = @"INSERT INTO xuong_che_tac (maxuong, tenxuong, sdt, diachi, trangthai)
                                         VALUES (@maxuong, @tenxuong, @sdt, @diachi, N'Hoạt động')";

                    SqlParameter[] prms = new SqlParameter[]
                    {
                        new SqlParameter("@maxuong", maMoi),
                        new SqlParameter("@tenxuong", tenXuong),
                        new SqlParameter("@sdt", rawSDT),
                        new SqlParameter("@diachi", diaChi)
                    };

                    Functions.ExecuteNonQueryParam(sqlInsert, prms);

                    MaXuongMoi = maMoi;
                    TenXuongMoi = tenXuong;

                    MessageBox.Show("Thêm xưởng chế tác mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string sqlUpdate = @"UPDATE xuong_che_tac 
                                         SET tenxuong = @tenxuong, sdt = @sdt, diachi = @diachi 
                                         WHERE maxuong = @maxuong";

                    SqlParameter[] prms = new SqlParameter[]
                    {
                        new SqlParameter("@maxuong", maXuongSua),
                        new SqlParameter("@tenxuong", tenXuong),
                        new SqlParameter("@sdt", rawSDT),
                        new SqlParameter("@diachi", diaChi)
                    };

                    Functions.ExecuteNonQueryParam(sqlUpdate, prms);

                    MaXuongMoi = maXuongSua;
                    TenXuongMoi = tenXuong;

                    MessageBox.Show("Cập nhật xưởng chế tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!string.IsNullOrEmpty(txtTenXuong.Text.Trim())) return true;
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