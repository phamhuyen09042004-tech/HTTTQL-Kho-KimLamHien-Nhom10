using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmGiaodienbanhang : Form
    {
        private bool isLoggingOut = false;
        private bool isExiting = false;

        private Timer timerRealtime = new Timer();

        private List<string> lastTop5Ten = new List<string>();
        private List<int> lastTop5SL = new List<int>();
        private List<string> lastNgay = new List<string>();
        private List<decimal> lastDoanhThu = new List<decimal>();
        public FrmGiaodienbanhang()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
        }

        private void FrmGiaodienbanhang_Load(object sender, EventArgs e)
        {

            lblManguoidung.Text = "Nhân viên: " + session.TenNhanVien;
            lblChucvu.Text = "Chức vụ: " + session.Chucvu;
            lblTime.Text = DateTime.Now.ToString("'Hôm nay, 'HH:mm: 'Ngày' dd/MM/yy");

            try
            {
                picLogo.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\logo1.ico");
                picAnh.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\user2.png");
                btnDangxuat.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\nutnguon.png");
            }
            catch { }

            // Khởi động Timer cập nhật thời gian thực sau 50 giây
            timerRealtime.Interval = 50000;
            timerRealtime.Start();
        }
    }
}
