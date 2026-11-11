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
using LiveCharts;
using LiveCharts.Wpf;
using HTTTQL_Kho_KimLamHien_Nhom10.BLL;
using System.Management;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmGiaodienadmin : Form
    {
        private Timer timerRealtime = new Timer();
        private int demGiay = 0;
        private Form formCon;

        private List<string> lastTop5Ten = new List<string>();
        private List<int> lastTop5SL = new List<int>();
        private List<string> lastNgay = new List<string>();
        private List<decimal> lastDoanhThu = new List<decimal>();
        public FrmGiaodienadmin()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
            GanSuKien();
        }

        private void FrmGiaodienadmin_Load(object sender, EventArgs e)
        {
            if (!GiaodienBLL.ChoPhepTruyCapGiamdoc())
            {
                MessageBox.Show("Bạn không có quyền truy cập giao diện này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
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
            // Đồng hồ chạy mỗi giây, cứ 30 giây thì tải lại số liệu
            timerRealtime.Interval = 1000;
            timerRealtime.Tick += TimerRealtime_Tick;
            timerRealtime.Start();
            TaiDuLieu();
        }
        private void TimerRealtime_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("'Hôm nay, 'HH:mm: 'Ngày' dd/MM/yy");
            demGiay++;
            if (demGiay >= 30 && formCon == null)
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
                lblDanggiaohang.Text = "Đang xử lý: " + GiaodienBLL.LayDonDangXuLy() + " đơn";

                lblDoanhThu.Text = string.Format("{0:N0} VNĐ", GiaodienBLL.LayDoanhThuHomNay());
                lblTonkho.Text = GiaodienBLL.LayTonKhoDauNgay() + " sản phẩm";

                btnChoduyet.Text = "Có " + GiaodienBLL.LayDeXuatChoDuyet() + " đơn đang chờ duyệt";
                btnSachsaphet.Text = "Có " + GiaodienBLL.LayHangSapHet() + " mặt hàng sắp hết";

                lblClicktopbanchay.Text = "TOP SẢN PHẨM BÁN CHẠY THÁNG " + DateTime.Now.Month + "/" + DateTime.Now.Year;

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
            GanClick(btnQlyPNK, () => new FrmQuanlyphieunhap());        // Quản lý phiếu nhập kho
            GanClick(btnQlyloaihang, () => new FrmThemloaihang());   // Thêm loại hàng
            GanClick(btnQlyHD, () => new FrmQuanlyhoadonban());         // Quản lý đơn hàng
            GanClick(btnQlyTK, () => new FrmQuanlytaikhoan());         // Quản lý người dùng
            GanClick(btnQlyxuong, () => new FrmQuanlyxuong());      // Quản lý xưởng
            GanClick(btnQlyNCC, () => new FrmQuanlyNCC());        // Quản lý nhà cung cấp
            GanClick(btnQlyPXK, () => new FrmQuanlyphieuxuat());        // Quản lý xuất kho
            GanClick(btnTaoyeucaukk, () => new FrmTaoyeucaukiemke());   // Tạo yêu cầu kiểm kê
            GanClick(btnQlyPKK, () => new FrmQuanlyphieukiemke());        // Quản lý phiếu kiểm kê
            GanClick(btnQlygiavon, () => new FrmQuanlygiavon());     // Lịch sử giá vốn
            GanClick(btnTheodoitonkho, () => new FrmTheodoitonkho()); // Theo dõi tồn kho
            GanClick(btnCapnhatgia, () => new FrmCapnhatgia());    // Cập nhật giá bán
            GanClick(lblClickdonhang, () => new FrmQuanlyhoadonban());
            // --- 3 nút trên cùng ---
            GanClick(btnSachmoi, () => new FrmQuanlyhanghoa());       // Quản lý sản phẩm
            GanClick(btnChoduyet, () => new FrmQuanlydondexuat());      // Đơn đang chờ duyệt (phiếu đề xuất nhập)
            GanClick(btnSachsaphet, () => new FrmBaocaotonkho());    // Mặt hàng sắp hết

            // --- Khối doanh thu hôm nay ---
            GanClick(lblClickDoanhThu, () => new FrmBaocaodoanhthu());
            GanClick(lblDoanhThu, () => new FrmBaocaodoanhthu());

            // --- Khối tồn kho đầu ngày ---
            GanClick(lblClickTonkho, () => new FrmTheodoitonkho());
            GanClick(lblTonkho, () => new FrmTheodoitonkho());

            // --- Biểu đồ ---
            GanClick(label2, () => new FrmBaocaodoanhthu());           // tiêu đề "Doanh thu theo ngày"
            GanClick(ChartDoanhthu, () => new FrmBaocaodoanhthu());
            GanClick(lblClicktopbanchay, () => new FrmBaocaobanchay());
            GanClick(chartTop5, () => new FrmBaocaobanchay());

            btnDangxuat1.Click += btnDangxuat1_Click;
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
        private void btnDangxuat1_Click(object sender, EventArgs e)
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
