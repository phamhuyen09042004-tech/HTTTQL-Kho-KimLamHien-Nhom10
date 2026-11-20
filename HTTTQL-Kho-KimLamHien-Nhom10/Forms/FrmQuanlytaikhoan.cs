using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;
using HTTTQL_Kho_KimLamHien_Nhom10.BLL;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmQuanlytaikhoan : Form
    {
        public FrmQuanlytaikhoan()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
        }

        private void FrmQuanlytaikhoan_Load(object sender, EventArgs e)
        {
            try
            {
                btnTimkiem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql4.png");
                btnXem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql5.png");
                txtTennhanvien.IconLeft = Image.FromFile(Application.StartupPath + @"\Images\User\ql8.png");
                btnSua.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql6.png");
                btnThem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql3.png");
                btnLock.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql7.png");
                btnDlaiMK.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql2.png");
            }
            catch { }

            DataTable dtChucVu = new DataTable();
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            dtChucVu.Columns.Add("Text");
            dtChucVu.Columns.Add("Value");

            dtChucVu.Rows.Add("-- Chọn chức vụ --", "");
            dtChucVu.Rows.Add("Giám đốc", "Giám đốc");
            dtChucVu.Rows.Add("Quản lý kho", "Quản lý kho");
            dtChucVu.Rows.Add("Nhân viên bán hàng", "Nhân viên bán hàng");
            dtChucVu.Rows.Add("Nhân viên thu ngân", "Nhân viên thu ngân");
            dtChucVu.Rows.Add("Kế toán", "Kế toán");

            cboChucvu.DataSource = dtChucVu;
            cboChucvu.DisplayMember = "Text";
            cboChucvu.ValueMember = "Value";

            cboChucvu.SelectedIndex = 0;
            cboChucvu.Height = 45;
            cboChucvu.ItemHeight = 35;

            txtManhanvien.Enabled = false;
            txtManhanvien.Text = "";
            // -= trước để không gắn đôi nếu Designer đã gắn sẵn
            dtgridNhanvien.SelectionChanged -= dtgridNhanvien_SelectionChanged;
            dtgridNhanvien.SelectionChanged += dtgridNhanvien_SelectionChanged;

            btnTimkiem.Click -= btnTimkiem_Click;
            btnTimkiem.Click += btnTimkiem_Click;
            txtTennhanvien.KeyDown -= txtTennhanvien_KeyDown;
            txtTennhanvien.KeyDown += txtTennhanvien_KeyDown;

            btnXem.Click -= btnXem_Click;
            btnXem.Click += btnXem_Click;
            btnSua.Click -= btnSua_Click;
            btnSua.Click += btnSua_Click;
            btnThem.Click -= btnThem_Click;
            btnThem.Click += btnThem_Click;
            btnLock.Click -= btnLock_Click;
            btnLock.Click += btnLock_Click;
            btnDlaiMK.Click -= btnDlaiMK_Click;
            btnDlaiMK.Click += btnDlaiMK_Click;
            btnQuaylai.Click -= btnQuaylai_Click;
            btnQuaylai.Click += btnQuaylai_Click;
            TaiDuLieu(false);
        }
        private void TaiDuLieu(bool baoKhongCoDuLieu)
        {
            try
            {
                string maDangChon = txtManhanvien.Text;
                string chucvu = Convert.ToString(cboChucvu.SelectedValue) ?? "";

                DataTable dt = NguoidungBLL.LayDanhSach(txtTennhanvien.Text, chucvu);
                dtgridNhanvien.DataSource = dt;

                DatTieuDe("manv", "Mã nhân viên");
                DatTieuDe("tennv", "Họ tên");
                DatTieuDe("chucvu", "Chức vụ");
                DatTieuDe("tendangnhap", "Tên đăng nhập");
                DatTieuDe("landndau", "Lần đăng nhập");
                DatTieuDe("ngaytao", "Ngày tạo");
                DatTieuDe("ngaycapnhat", "Ngày cập nhật");
                DatTieuDe("trangthai", "Trạng thái");

                if (dtgridNhanvien.Columns.Contains("ngaytao"))
                    dtgridNhanvien.Columns["ngaytao"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                if (dtgridNhanvien.Columns.Contains("ngaycapnhat"))
                    dtgridNhanvien.Columns["ngaycapnhat"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                ToMauTheoTrangThai();

                // Chọn lại dòng đang chọn trước đó (nếu còn)
                for (int i = 0; i < dtgridNhanvien.Rows.Count; i++)
                {
                    if (Convert.ToString(dtgridNhanvien.Rows[i].Cells["manv"].Value) == maDangChon)
                    {
                        dtgridNhanvien.CurrentCell = dtgridNhanvien.Rows[i].Cells[0];
                        break;
                    }
                }
                CapNhatChon();

                if (baoKhongCoDuLieu && dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào phù hợp.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DatTieuDe(string cot, string tieuDe)
        {
            if (dtgridNhanvien.Columns.Contains(cot))
                dtgridNhanvien.Columns[cot].HeaderText = tieuDe;
        }
        // Hoạt động: xanh, Đã khóa: xám. Dòng được chọn: màu kem như mẫu.
        private void ToMauTheoTrangThai()
        {
            for (int i = 0; i < dtgridNhanvien.Rows.Count; i++)
            {
                DataGridViewRow row = dtgridNhanvien.Rows[i];
                string tt = Convert.ToString(row.Cells["trangthai"].Value);

                row.DefaultCellStyle.BackColor = (tt == NguoidungBLL.TT_DAKHOA) ? Color.LightGray : Color.LightGreen;
                row.DefaultCellStyle.SelectionBackColor = Color.PapayaWhip;
                row.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
        }
        // ===== Dòng đang chọn =====
        private void dtgridNhanvien_SelectionChanged(object sender, EventArgs e)
        {
            CapNhatChon();
        }
        private void CapNhatChon()
        {
            if (dtgridNhanvien.CurrentRow == null || !dtgridNhanvien.Columns.Contains("manv"))
            {
                txtManhanvien.Text = "";
                btnLock.Text = "Khóa";
                return;
            }

            DataGridViewRow row = dtgridNhanvien.CurrentRow;
            txtManhanvien.Text = Convert.ToString(row.Cells["manv"].Value);
            string tt = Convert.ToString(row.Cells["trangthai"].Value);
            btnLock.Text = (tt == NguoidungBLL.TT_DAKHOA) ? "Mở khóa" : "Khóa";
        }
        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            TaiDuLieu(true);
        }
        private void txtTennhanvien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TaiDuLieu(true);
            }
        }
        private void btnXem_Click(object sender, EventArgs e)
        {
            if (!CoChon()) return;
            MoForm(new FrmTaotaikhoan("XEM", txtManhanvien.Text));
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!CoChon()) return;
            MoForm(new FrmTaotaikhoan("SUA", txtManhanvien.Text));
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            MoForm(new FrmTaotaikhoan());
        }
        private bool CoChon()
        {
            if (string.IsNullOrWhiteSpace(txtManhanvien.Text))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }
        // Mở form khác dạng dialog, đóng xong thì tải lại lưới
        private void MoForm(Form f)
        {
            if (f == null)
            {
                MessageBox.Show("Chức năng này chưa được gắn form.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (f)
            {
                f.ShowDialog();
            }
            TaiDuLieu(false);
        }
        private void btnLock_Click(object sender, EventArgs e)
        {
            if (!CoChon()) return;

            string hanhDong = btnLock.Text.ToLower();
            DialogResult kq = MessageBox.Show(
                "Bạn có chắc muốn " + hanhDong + " tài khoản " + txtManhanvien.Text + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq != DialogResult.Yes) return;

            try
            {
                string thongBao = NguoidungBLL.KhoaMo(txtManhanvien.Text);
                TaiDuLieu(false);
                MessageBox.Show(thongBao, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDlaiMK_Click(object sender, EventArgs e)
        {
            if (!CoChon()) return;

            DialogResult kq = MessageBox.Show(
                "Đặt lại mật khẩu cho tài khoản " + txtManhanvien.Text + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq != DialogResult.Yes) return;

            try
            {
                string matkhauTam = NguoidungBLL.DatLaiMatKhau(txtManhanvien.Text);
                TaiDuLieu(false);
                MessageBox.Show(
                    "Đã đặt lại mật khẩu cho " + txtManhanvien.Text + ".\n\n" +
                    "Mật khẩu tạm: " + matkhauTam + "\n\n" +
                    "Hãy gửi cho nhân viên, họ sẽ phải đổi mật khẩu ở lần đăng nhập sau. " +
                    "Mật khẩu này chỉ hiện một lần.",
                    "Đặt lại mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
