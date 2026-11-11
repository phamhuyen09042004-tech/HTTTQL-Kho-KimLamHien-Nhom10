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
using LiveCharts;
using LiveCharts.Wpf;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmGiaodienquanlykho : Form
    {
        private Timer timerRealtime = new Timer();
        private int demGiay = 0;

        // Lưu cache để tránh vẽ lại biểu đồ không cần thiết
        private List<string> lastTop5Ten = new List<string>();
        private List<int> lastTop5SL = new List<int>();
        public FrmGiaodienquanlykho()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            GanSuKien();
        }

        private void FrmGiaodienquanlykho_Load(object sender, EventArgs e)
        {
            if (!GiaodienBLL.ChoPhepTruyCapKho())
            {
                MessageBox.Show("Bạn không có quyền truy cập giao diện này.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
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
            timerRealtime.Tick += TimerRealtime_Tick;
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
                lblTonkho.Text = GiaodienBLL.LayTonKhoDauNgay() + " sản phẩm";

                lblNhap.Text = GiaodienBLL.LayDeXuatChoXacNhan() + " đơn";
                lblChoduyet.Text = GiaodienBLL.LayDeXuatChoDuyet() + " đơn";
                lblDaduyet.Text = GiaodienBLL.LayDeXuatDaDuyet() + " đơn";

                btnSachsaphet.Text = "Có " + GiaodienBLL.LayHangSapHet() + " mặt hàng sắp hết";

                VeBieuDoTop5();
            }
            catch (Exception ex)
            {
                timerRealtime.Stop();
                MessageBox.Show("Không tải được dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Chỉ vẽ lại khi dữ liệu thay đổi (dùng 2 list cache có sẵn)
        private void VeBieuDoTop5()
        {
            DataTable dt = GiaodienBLL.LayTop5BanChay();
            List<string> ten = new List<string>();
            List<int> sl = new List<int>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ten.Add(dt.Rows[i]["tenhang"].ToString());
                sl.Add(Convert.ToInt32(dt.Rows[i]["soluong"]));
            }

            if (ten.SequenceEqual(lastTop5Ten) && sl.SequenceEqual(lastTop5SL))
                return;

            lastTop5Ten = ten;
            lastTop5SL = sl;

            ChartValues<double> giaTri = new ChartValues<double>();
            for (int i = 0; i < sl.Count; i++)
                giaTri.Add(sl[i]);

            chartTop5.Series = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "Số lượng",
                    Values = giaTri,
                    DataLabels = true,
                    LabelPoint = p => p.X + " sản phẩm",
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
            GanClick(btnTaodonNH, () => new FrmTaodondexuatnhap());        // Tạo đơn nhập hàng (phiếu đề xuất nhập)
            GanClick(btnTaoPNK, () => new FrmTaophieunhap());          // Tạo phiếu nhập kho
            GanClick(btnTaoPKK, () => new FrmKetquakiemke());          // Tạo phiếu kiểm kê
            GanClick(btnQlyxuong, () => new FrmQuanlyxuong());        // Quản lý xưởng
            GanClick(btnQlyNCC, () => new FrmQuanlyNCC());          // Quản lý nhà cung cấp
            GanClick(btnQlyPNK, () => new FrmQuanlyphieunhap());          // Quản lý phiếu nhập kho
            GanClick(btnQlydondexuat, () => new FrmQuanlydondexuat());    // Quản lý đơn đề xuất nhập
            GanClick(btnQlySP, () => new FrmQuanlyhanghoa());           // Quản lý sản phẩm
            GanClick(btnQlyPKK, () => new FrmQuanlyphieukiemke());          // Quản lý phiếu kiểm kê
            GanClick(btnTheodoitonkho, () => new FrmTheodoitonkho());

            // --- Nút Số vàng sắp hết -> Theo dõi tồn kho ---
            GanClick(btnSachsaphet, () => new FrmBaocaotonkho());

            // --- Khối tồn kho đầu ngày -> Theo dõi tồn kho ---
            GanClick(lblClickTonkho, () => new FrmBaocaotonkho());
            GanClick(lblTonkho, () => new FrmBaocaotonkho());

            // --- Khối theo dõi phiếu nhập -> Quản lý đơn đề xuất nhập ---
            GanClick(lblTime, () => new FrmQuanlydondexuat());

            // --- Biểu đồ -> Báo cáo bán chạy ---
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
