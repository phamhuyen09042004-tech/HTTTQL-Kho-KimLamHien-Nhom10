using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmChonkhokiemke : Form
    {
        public FrmChonkhokiemke()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
        }

        private void FrmChonkhokiemke_Load(object sender, EventArgs e)
        {
            // Bọc kiểm tra file để nếu thiếu ảnh cũng không bao giờ bị dừng cả form
            string logoPath = Path.Combine(Application.StartupPath, "Image", "Giaodien", "logo1.ico");
            if (File.Exists(logoPath))
            {
                pic1.Image = Image.FromFile(logoPath);
            }

            // Đặt mặc định trạng thái ban đầu
            txtXacnhankho.Text = "";
            guna2TextBox1.Text = "";
            txtThongtin.Text = "";

            // Khóa chỉ đọc
            txtXacnhankho.ReadOnly = true;
            guna2TextBox1.ReadOnly = true;
            txtThongtin.ReadOnly = true;

            // Cho phép hiển thị nhiều dòng nếu thông báo dài
            txtThongtin.Multiline = true;
            txtThongtin.WordWrap = true;
            txtThongtin.ScrollBars = ScrollBars.Vertical;
        }

        private void rdKho_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null && rb.Checked)
            {
                // 1. Dùng đúng mã kho trong SQL: KHO_B02 hoặc KHO_B01
                string makho = rdKhovang.Checked ? "KHO_B02" : "KHO_B01";
                string tenkho = rdKhovang.Checked ? "Kho vàng" : "Kho bạc";
                txtXacnhankho.Text = tenkho;

                try
                {
                    // 2. Dùng đúng trạng thái: trangthai <> N'Đã hoàn thành'
                    string sql = $"SELECT maphieukiemke, ngaykiemke, manv, trangthai FROM phieu_kiem_ke_kho WHERE makho = '{makho}' AND trangthai <> N'Đã hoàn thành'";
                    DataTable dt = Functions.GetDataToTable(sql);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        guna2TextBox1.Text = dt.Rows.Count.ToString();

                        string chiTiet = $"Hiện có {dt.Rows.Count} phiếu chưa hoàn tất:" + Environment.NewLine;
                        foreach (DataRow r in dt.Rows)
                        {
                            chiTiet += $"• {r["maphieukiemke"]} ({r["trangthai"]}) - Ngày: {Convert.ToDateTime(r["ngaykiemke"]):dd/MM/yyyy} - NV: {r["manv"]}" + Environment.NewLine;
                        }
                        txtThongtin.Text = chiTiet.TrimEnd();
                        btnLuu.Enabled = false;
                    }
                    else
                    {
                        guna2TextBox1.Text = "0";
                        txtThongtin.Text = $"Tất cả phiếu kiểm kê tại {tenkho} đã hoàn tất. Đủ điều kiện tạo đợt mới.";
                        btnLuu.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi truy vấn dữ liệu kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!rdKhovang.Checked && !rdKhobac.Checked)
            {
                MessageBox.Show("Vui lòng chọn kho hàng cần kiểm kê!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (guna2TextBox1.Text == "1")
            {
                MessageBox.Show("Kho này đang được kiểm kê, vui lòng hoàn tất đợt cũ trước khi tạo mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string makhoSelected = rdKhovang.Checked ? "KV" : "KB";
            session.YeuCauKiemKeKho = makhoSelected;

            this.Hide();
            FrmKetquakiemke frm = new FrmKetquakiemke();
            frm.ShowDialog();
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}