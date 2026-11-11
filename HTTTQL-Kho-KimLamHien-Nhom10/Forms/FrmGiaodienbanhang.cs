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
using HTTTQL_Kho_KimLamHien_Nhom10.BLL;
using LiveCharts;
using LiveCharts.Wpf;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmGiaodienbanhang : Form
    {
        private bool isLoggingOut = false;
        private bool isExiting = false;

        private Timer timerRealtime = new Timer();
        private int demGiay = 0;

        private List<string> lastTop5Ten = new List<string>();
        private List<int> lastTop5SL = new List<int>();
        private List<string> lastNgay = new List<string>();
        private List<decimal> lastDoanhThu = new List<decimal>();
        public FrmGiaodienbanhang()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
            GanSuKien();
        }

        private void FrmGiaodienbanhang_Load(object sender, EventArgs e)
        {
            if (!GiaodienBLL.ChoPhepTruyCapBanhang())
            {
                MessageBox.Show("Bạn không có quyền truy cập giao diện này.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
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
            TaiDuLieu();
        }
        private void TimerRealtime_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("'Hôm nay, 'HH:mm: 'Ngày' dd/MM/yy");
            demGiay++;
            if (demGiay >= 50)
            {
                demGiay = 0;
                TaiDuLieu();
            }
        }
        private void TaiDuLieu()
        {
            try
            {
                lblChoxacnhan.Text = "Chờ xác nhận: " + GiaodienBLL.LayDonChoXacNhan() + " đơn";
                lblChogiaohang.Text = "Đang xử lý: " + GiaodienBLL.LayDonDangXuLy() + " đơn";

                lblDoanhThu.Text = string.Format("{0:N0} VNĐ", GiaodienBLL.LayDoanhThuHomNay());

                lblClicktopbanchay.Text = "TOP SẢN PHẨM BÁN CHẠY T" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                btnSachsaphet.Text = "Có " + GiaodienBLL.LayHangSapHet() + " mặt hàng sắp hết";

                VeBieuDoDoanhThu();
                VeBieuDoTop5();
            }
            catch (Exception ex)
            {
                timerRealtime.Stop();
                MessageBox.Show("Không tải được dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void VeBieuDoDoanhThu()
        {
            DataTable dt = GiaodienBLL.LayDoanhThu7Ngay();
            ChartValues<double> giaTri = new ChartValues<double>();
            List<string> nhan = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                giaTri.Add(Convert.ToDouble(dt.Rows[i]["DoanhThu"]));
                nhan.Add(dt.Rows[i]["Ngay"].ToString());
            }

            ChartDoanhthu.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Doanh thu",
                    Values = giaTri,
                    Stroke = System.Windows.Media.Brushes.Goldenrod,
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };
            ChartDoanhthu.AxisX.Clear();
            ChartDoanhthu.AxisX.Add(new Axis { Labels = nhan });
            ChartDoanhthu.AxisY.Clear();
            ChartDoanhthu.AxisY.Add(new Axis { MinValue = 0, LabelFormatter = v => v.ToString("N0") });
        }
        private void VeBieuDoTop5()
        {
            DataTable dt = GiaodienBLL.LayTop5BanChay();
            ChartValues<double> soLuong = new ChartValues<double>();
            List<string> ten = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                soLuong.Add(Convert.ToDouble(dt.Rows[i]["soluong"]));
                ten.Add(dt.Rows[i]["tenhang"].ToString());
            }

            chartTop5.Series = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Số lượng",
                    Values = soLuong,
                    DataLabels = true,
                    Fill = System.Windows.Media.Brushes.Goldenrod
                }
            };
            chartTop5.AxisY.Clear();
            chartTop5.AxisY.Add(new Axis { Labels = ten, Separator = new Separator { Step = 1 } });
            chartTop5.AxisX.Clear();
            chartTop5.AxisX.Add(new Axis { MinValue = 0 });
        }
        private void GanSuKien()
        {
            // --- Menu trái ---
            GanClick(btnTaoDonBan, () => new FrmTaohoadonban());                  // Tạo hóa đơn bán hàng
            GanClick(btnQuanLyDanhMuc, () => new FrmQuanlyhanghoa());  // Quản lý sản phẩm
            GanClick(btnQuanLyHD, () => QuanLyDonHang());             // Quản lý đơn hàng

            // --- Nút Số vàng sắp hết -> Theo dõi tồn kho ---
            GanClick(btnSachsaphet, () => new FrmTheodoitonkho());

            // --- Thống kê đơn hàng -> Quản lý đơn hàng ---
            GanClick(lblDonhang, () => QuanLyDonHang());
            GanClick(lblChoxacnhan, () => QuanLyDonHang());
            GanClick(lblChogiaohang, () => QuanLyDonHang());

            // --- Top bán chạy -> Báo cáo bán chạy ---
            GanClick(lblClicktopbanchay, () => new FrmBaocaobanchay());
            GanClick(chartTop5, () => new FrmBaocaobanchay());

            btnDangxuat.Click += btnDangxuat_Click;
        }
        private Form QuanLyDonHang()
        {
            return new FrmQuanlyhoadonban();
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
            TaiDuLieu();
            timerRealtime.Start();
        }
        private void btnDangxuat_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq != DialogResult.Yes) return;

            timerRealtime.Stop();
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

            login.Show();
            this.Close();
        }
    }
}
