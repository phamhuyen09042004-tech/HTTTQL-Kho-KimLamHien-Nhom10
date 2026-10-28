using HTTTQL_Kho_KimLamHien_Nhom10.Class;
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

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmGiaodienadmin : Form
    {
        private Timer timerRealtime = new Timer();

        private List<string> lastTop5Ten = new List<string>();
        private List<int> lastTop5SL = new List<int>();
        private List<string> lastNgay = new List<string>();
        private List<decimal> lastDoanhThu = new List<decimal>();
        public FrmGiaodienadmin()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
        }

        private void FrmGiaodienadmin_Load(object sender, EventArgs e)
        {
            try
            {
                picLogo.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\logo1.ico");
                picAnh.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\user2.png");
                btnDangxuat1.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\nutnguon.png");
                btnTaoyeucaukk.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\warehouse.png");
                btnQlyPNK.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\pnk.png");
                btnQlyloaihang.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\category.png");
                btnQlyHD.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\create2.png");
                btnQlyTK.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\human.png");
                btnQlyxuong.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\warehouse.png");
                btnQlyNCC.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\ncc.png");
                btnQlyPXK.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\pnk.png");
                btnQlyPKK.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\pnk.png");
                btnQlygiavon.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\history.png");
                btnTheodoitonkho.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\baocaoton.png");
                btnCapnhatgia.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\create3.png");
            }
            catch { }

            lblManguoidung.Text = "Nhân viên: " + session.TenNhanVien;
            lblChucvu.Text = "Chức vụ: " + session.Chucvu;
            lblTime.Text = DateTime.Now.ToString("'Hôm nay, 'HH:mm: 'Ngày' dd/MM/yy");
            btnSachmoi.Visible = true;
        }
    }
}
