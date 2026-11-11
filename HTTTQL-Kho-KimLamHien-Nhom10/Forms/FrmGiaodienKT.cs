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
    public partial class FrmGiaodienKT : Form
    {
        private bool isLoggingOut = false;
        private bool isExiting = false;
        private Timer timerRealtime = new Timer();
        private int demGiay = 0;
        public FrmGiaodienKT()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
            GanSuKien();
        }

        private void FrmGiaodienKT_Load(object sender, EventArgs e)
        {
            picLogo.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\logo1.ico");
            picAnh.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\user2.png");
            btnDangxuat.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\nutnguon.png");
            btnBcDoanhthu.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\pnk.png");
            btnSpbanchay.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\category.png");
            btnBctonkho.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\warehouse.png");
            btnTheodoitonkho.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\baocaoton.png");
            btnLichsugiavon.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\history.png");
            lblManguoidung.Text = "Nhân viên: " + session.TenNhanVien;
            lblChucvu.Text = "Chức vụ: " + session.Chucvu;
            lblTime.Text = DateTime.Now.ToString("'Hôm nay, 'HH:mm: 'Ngày' dd/MM/yy");

            // Khởi động Timer cập nhật thời gian thực sau 50 giây
            timerRealtime.Interval = 50000;
            timerRealtime.Start();
        }
        private void GanSuKien()
        {
            // --- Menu trái ---
            GanClick(btnBcDoanhthu, () => new FrmBaocaodoanhthu());
            GanClick(btnBctonkho, () => new FrmBaocaotonkho());
            GanClick(btnLichsugiavon, () => new FrmQuanlygiavon());
            GanClick(btnSpbanchay, () => new FrmBaocaobanchay());
            GanClick(btnTheodoitonkho, () => new FrmTheodoitonkho());
            btnDangxuat.Click += btnDangxuat_Click;
        }
        private void GanClick(Control c, Func<Form> taoForm)
        {
            c.Cursor = Cursors.Hand;
            c.Click += (s, e) => MoForm(taoForm());
        }
        private void MoForm(Form f)
        {
            if (f == null)
            {
                MessageBox.Show("Chức năng này chưa được gắn form.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            timerRealtime.Stop();
            this.Hide();
            f.ShowDialog();
            f.Dispose();
            this.Show();

            demGiay = 0;
            timerRealtime.Start();
        }
        private void btnDangxuat_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq != DialogResult.Yes) return;

            session.Dangxuat();

            Form login = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is FrmLogin)
                {
                    login = f;
                    break;
                }
            }
            if (login == null) login = new FrmLogin();
            timerRealtime.Stop();
            login.Show();
            this.Close();
        }
    }
}
