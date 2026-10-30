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

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmGiaodienquanlykho : Form
    {
        private Timer timerRealtime = new Timer();

        // Lưu cache để tránh vẽ lại biểu đồ không cần thiết
        private List<string> lastTop5Ten = new List<string>();
        private List<int> lastTop5SL = new List<int>();
        public FrmGiaodienquanlykho()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void FrmGiaodienquanlykho_Load(object sender, EventArgs e)
        {
            try
            {
                picAnh.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\user2.png");
                picLogo.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\logo1.ico");
                btnDangxuat1.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\nutnguon.png");
                btnTaodonNH.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\import.png");
                btnTaoPNK.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\pnk.png");
                btnTaoPKK.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\create3.png");
                btnQlyxuong.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\baocaoton.png");
                btnQlyNCC.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\ncc.png");
                btnQlyPNK.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\pnk.png");
                btnQlydondexuat.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\shipment.png");
                btnQlyPKK.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\create3.png");
                btnQlySP.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\category.png");
                btnTheodoitonkho.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\history.png");
            }
            catch { }

            lblManguoidung.Text = "Nhân viên: " + session.TenNhanVien;
            lblChucvu.Text = "Chức vụ: " + session.Chucvu;
            lblTime.Text = DateTime.Now.ToString("'Hôm nay, 'HH:mm: 'Ngày' dd/MM/yy");

            // Kiểm tra thông báo yêu cầu kiểm kê từ admin
            if (!string.IsNullOrEmpty(session.YeuCauKiemKeNhanVien)
                && session.YeuCauKiemKeNhanVien == session.MaNhanVien
                && !session.DaDocYeuCauKiemKe)
            {
                string noidung = $"📋 YÊU CẦU KIỂM KÊ\n\n" +
                                 $"Chi tiết: {session.YeuCauKiemKeChiTiet}";
                MessageBox.Show(noidung, "Thông báo từ quản lý",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                session.DaDocYeuCauKiemKe = true;
            }

            // Khởi động Timer cập nhật thời gian thực sau 50 giây
            timerRealtime.Interval = 50000;
            timerRealtime.Start();
        }
    }
}
