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
    public partial class FrmBaocaodoanhthu : Form
    {
        private Timer timerRealtime = new Timer();
        private DataTable dtHienTai;
        private DateTime tuHienTai, denHienTai;
        private string lastChartKey = "";
        private bool dangDatGiaTri = false;
        private KyBaoCao kyApDung;
        private DateTime tuNgayApDung, denNgayApDung;
        private int thangApDung, namApDung;
        public FrmBaocaodoanhthu()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
            this.FormClosing += (s, e) => timerRealtime.Stop();
        }

        private void FrmBaocaodoanhthu_Load(object sender, EventArgs e)
        {
            if (!BaocaoBLL.ChoPhepXemDoanhThu())
            {
                MessageBox.Show("Bạn không có quyền xem báo cáo doanh thu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            dgvChitiet.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            dgvChitiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dgvChitiet.ColumnHeadersHeight = 60;
            dgvChitiet.RowTemplate.Height = 40;
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            // Combo tháng 1-12, năm từ hiện tại lùi 5 năm
            cboThang.Items.Clear();
            for (int m = 1; m <= 12; m++) cboThang.Items.Add(m);
            cboNam.Items.Clear();
            for (int y = DateTime.Now.Year; y >= DateTime.Now.Year - 5; y--) cboNam.Items.Add(y);
            // Dạng lưới
            dgvChitiet.ReadOnly = true;
            dgvChitiet.AllowUserToAddRows = false;
            dgvChitiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChitiet.MultiSelect = false;
            // Mặc định: tháng hiện tại
            dangDatGiaTri = true;
            rdoThang.Checked = true;
            cboThang.SelectedIndex = DateTime.Now.Month - 1;
            cboNam.SelectedIndex = 0;
            dtimeTungay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtimeDenngay.Value = DateTime.Today;
            dangDatGiaTri = false;
            CapNhatTrangThaiControl();
            // Gắn sự kiện sau khi đặt mặc định
            rdoNgay.CheckedChanged += DoiRadio;
            rdoThang.CheckedChanged += DoiRadio;
            rdoNam.CheckedChanged += DoiRadio;
            dtimeTungay.ValueChanged += DoiNgay;
            dtimeDenngay.ValueChanged += DoiNgay; 
            btnXem.Click += (s, ev) => ApDungVaTai(true);
            btnQuaylai.Click += (s, ev) => this.Close();
            dgvChitiet.CellDoubleClick += dgvChitiet_CellDoubleClick;
            // Lần đầu: tải tháng hiện tại
            ApDungVaTai(false);
            // Realtime: tải lại mỗi 30 giây
            timerRealtime.Interval = 30000;
            timerRealtime.Tick += (s, ev) => TaiDuLieu();
            timerRealtime.Start();
            TaiDuLieu();
        }
        private void DoiRadio(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2RadioButton rb = sender as Guna.UI2.WinForms.Guna2RadioButton;
            if (rb != null && !rb.Checked) return;
            CapNhatTrangThaiControl();
        }
        private void ApDungVaTai(bool baoKhongCoDuLieu)
        {
            kyApDung = LayKy();
            tuNgayApDung = dtimeTungay.Value;
            denNgayApDung = dtimeDenngay.Value;
            thangApDung = cboThang.SelectedItem != null ? Convert.ToInt32(cboThang.SelectedItem) : 0;
            namApDung = cboNam.SelectedItem != null ? Convert.ToInt32(cboNam.SelectedItem) : 0;

            bool ok = TaiDuLieu();
            if (ok && baoKhongCoDuLieu && dtHienTai != null && dtHienTai.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu doanh thu trong kỳ đã chọn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void CapNhatTrangThaiControl()
        {
            bool ngay = rdoNgay.Checked;
            bool thang = rdoThang.Checked;
            bool nam = rdoNam.Checked;

            dtimeTungay.Enabled = ngay;
            dtimeDenngay.Enabled = ngay;
            cboThang.Enabled = ngay || thang;
            cboNam.Enabled = ngay || thang || nam;
        }
        // Từ ngày > đến ngày thì ô kia tự chỉnh theo
        private void DoiNgay(object sender, EventArgs e)
        {
            if (dangDatGiaTri) return;
            if (dtimeTungay.Value.Date > dtimeDenngay.Value.Date)
            {
                dangDatGiaTri = true;
                if (sender == dtimeTungay) dtimeDenngay.Value = dtimeTungay.Value;
                else dtimeTungay.Value = dtimeDenngay.Value;
                dangDatGiaTri = false;
            }
        }
        private KyBaoCao LayKy()
        {
            if (rdoNgay.Checked) return KyBaoCao.Ngay;
            if (rdoNam.Checked) return KyBaoCao.Nam;
            return KyBaoCao.Thang;
        }
        private bool TaiDuLieu()
        {
            try
            {
                DateTime tu, den;
                BaocaoBLL.TinhKhoang(kyApDung, tuNgayApDung, denNgayApDung, thangApDung, namApDung, out tu, out den);

                DataTable dt = BaocaoBLL.LayDanhSachHoaDon(tu, den);
                tuHienTai = tu;
                denHienTai = den;
                dtHienTai = dt;

                HienThiLuoi(dt);

                int soDon;
                decimal tong = BaocaoBLL.TinhTong(dt, out soDon);
                lblThongbao.ForeColor = Color.DimGray;
                lblThongbao.Text = "Tổng doanh thu: " + string.Format("{0:N0}", tong) + " VNĐ   |   Số đơn: " + soDon;
                if (dt.Rows.Count == 0)
                    lblThongbao.Text = "Không có dữ liệu trong kỳ đã chọn.";
                VeBieuDo(BaocaoBLL.LayDuLieuBieuDo(kyApDung, tu, den));
                return true;
            }
            catch (Exception ex)
            {
                lblThongbao.ForeColor = Color.Red;
                lblThongbao.Text = "Lỗi: " + ex.Message;
                return false;
            }
        }
        private void HienThiLuoi(DataTable dt)
        {
            // Nhớ dòng đang chọn để tải lại realtime không mất vị trí
            string maDangChon = null;
            if (dgvChitiet.CurrentRow != null && dgvChitiet.Columns.Contains("mahd"))
                maDangChon = Convert.ToString(dgvChitiet.CurrentRow.Cells["mahd"].Value);

            dgvChitiet.DataSource = dt;

            DatTieuDe("mahd", "Mã hóa đơn");
            DatTieuDe("loaidon", "Loại đơn");
            DatTieuDe("tenkh", "Tên khách hàng");
            DatTieuDe("ngaylap", "Ngày lập");
            DatTieuDe("ngaycapnhat", "Ngày cập nhật");
            DatTieuDe("ptthanhtoan", "PT thanh toán");
            DatTieuDe("tennv", "Nhân viên lập");
            DatTieuDe("tongtien", "Tổng tiền");
            DatTieuDe("tongsl", "Tổng số lượng");

            if (dgvChitiet.Columns.Contains("ngaylap"))
                dgvChitiet.Columns["ngaylap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            if (dgvChitiet.Columns.Contains("ngaycapnhat"))
                dgvChitiet.Columns["ngaycapnhat"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            if (dgvChitiet.Columns.Contains("tongtien"))
                dgvChitiet.Columns["tongtien"].DefaultCellStyle.Format = "N0";

            if (maDangChon != null)
            {
                for (int i = 0; i < dgvChitiet.Rows.Count; i++)
                {
                    if (Convert.ToString(dgvChitiet.Rows[i].Cells["mahd"].Value) == maDangChon)
                    {
                        dgvChitiet.CurrentCell = dgvChitiet.Rows[i].Cells[0];
                        break;
                    }
                }
            }
        }
        private void DatTieuDe(string cot, string tieuDe)
        {
            if (dgvChitiet.Columns.Contains(cot))
                dgvChitiet.Columns[cot].HeaderText = tieuDe;
        }

        private void dgvChitiet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            MoChiTiet(Convert.ToString(dgvChitiet.Rows[e.RowIndex].Cells["mahd"].Value));
        }

        private void VeBieuDo(DataTable dt)
        {
            ChartValues<double> giaTri = new ChartValues<double>();
            List<string> nhan = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                giaTri.Add(Convert.ToDouble(dt.Rows[i]["DoanhThu"]));
                nhan.Add(dt.Rows[i]["Nhan"].ToString());
            }

            // Dữ liệu không đổi thì không vẽ lại, tránh nhấp nháy
            string key = string.Join("|", nhan) + "#" + string.Join("|", giaTri);
            if (key == lastChartKey) return;
            lastChartKey = key;

            linechartDoanhthu.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Doanh thu",
                    Values = giaTri,
                    DataLabels = giaTri.Count <= 15,
                    LabelPoint = p => p.Y.ToString("N0"),
                    Stroke = System.Windows.Media.Brushes.DarkRed,
                    Fill = System.Windows.Media.Brushes.Transparent
                }
            };
            linechartDoanhthu.AxisX.Clear();
            linechartDoanhthu.AxisX.Add(new Axis { Labels = nhan });
            linechartDoanhthu.AxisY.Clear();
            linechartDoanhthu.AxisY.Add(new Axis { MinValue = 0, LabelFormatter = v => v.ToString("N0") });
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (dtHienTai == null || dtHienTai.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel (*.xlsx)|*.xlsx";
                sfd.FileName = "BaoCaoDoanhThu_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    string tieuDe = "BÁO CÁO DOANH THU từ " + tuHienTai.ToString("dd/MM/yyyy") +
                                    " đến " + denHienTai.ToString("dd/MM/yyyy");
                    BaocaoBLL.XuatExcel(dtHienTai, sfd.FileName, tieuDe);
                    MessageBox.Show("Xuất Excel thành công.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không xuất được file: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MoChiTiet(string mahd)
        {
            timerRealtime.Stop();
            using (FrmChitietdonhang f = new FrmChitietdonhang()) //mahd
            {
                f.ShowDialog();
            }
            TaiDuLieu();
            timerRealtime.Start();
        }

    }
}
