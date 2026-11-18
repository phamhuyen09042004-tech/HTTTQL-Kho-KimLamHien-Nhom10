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
using LiveCharts;
using LiveCharts.Wpf;
using HTTTQL_Kho_KimLamHien_Nhom10.BLL;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmBaocaobanchay : Form
    {
        private Timer timerRealtime = new Timer();
        private DataTable dtHienTai;
        private string lastChartKey = "";
        private bool dangDatGiaTri = false;
        // Lựa chọn đã áp dụng (lần bấm Xem gần nhất), timer tải lại theo đây
        private int topApDung = 5;
        private string maloaiApDung = "";
        private DateTime tuApDung, denApDung;
        public FrmBaocaobanchay()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
            this.FormClosing += (s, e) => timerRealtime.Stop();
        }

        private void numSoluong_ValueChanged(object sender, EventArgs e)
        {
            if (dangDatGiaTri) return;
            if (numSoluong.Value < 1)
            {
                dangDatGiaTri = true;
                numSoluong.Value = 1;
                dangDatGiaTri = false;
            }
        }
        private void FrmBaocaobanchay_Load(object sender, EventArgs e)
        {
            dtimeTungay.Value = DateTime.Today.AddDays(-30);
            dtimeDenngay.Value = DateTime.Today;
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            // Số sản phẩm luôn >= 1
            numSoluong.Minimum = 1;
            // Mặc định
            dangDatGiaTri = true;
            cboTheloai.DisplayMember = "tenloai";
            cboTheloai.ValueMember = "maloai";
            cboTheloai.DataSource = BaocaoBLL.LayLoaiHangCombo();
            cboTheloai.SelectedIndex = 0;
            numSoluong.Value = 5;
            dtimeTungay.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtimeDenngay.Value = DateTime.Today;
            dangDatGiaTri = false;
            numSoluong.ValueChanged += numSoluong_ValueChanged;
            numSoluong.Leave += numSoluong_ValueChanged;
            dtimeTungay.ValueChanged += DoiNgay;
            dtimeDenngay.ValueChanged += DoiNgay;
            // -= trước để không gắn đôi nếu Designer đã gắn sẵn
            btnXem.Click -= btnXem_Click;
            btnXem.Click += btnXem_Click;
            btnXuat.Click -= btnXuat_Click;
            btnXuat.Click += btnXuat_Click;
            btnQuaylai.Click += (s, ev) => this.Close();
            ApDungVaTai(false);   // lần đầu, không bật hộp thoại
            timerRealtime.Interval = 30000;
            timerRealtime.Tick += (s, ev) => { string loi; TaiDuLieu(out loi); };
            timerRealtime.Start();
        }
        private void btnXem_Click(object sender, EventArgs e)
        {
            ApDungVaTai(true);
        }
        // Lấy lựa chọn hiện tại làm lựa chọn áp dụng rồi tải
        private void ApDungVaTai(bool baoThongBao)
        {
            if (numSoluong.Value < 1) numSoluong.Value = 1;

            topApDung = Convert.ToInt32(numSoluong.Value);
            maloaiApDung = Convert.ToString(cboTheloai.SelectedValue) ?? "";
            tuApDung = dtimeTungay.Value;
            denApDung = dtimeDenngay.Value;

            string loi;
            bool ok = TaiDuLieu(out loi);

            if (!baoThongBao) return;

            if (!ok)
            {
                MessageBox.Show(loi, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (dtHienTai != null && dtHienTai.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu bán hàng trong lựa chọn đã chọn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // Tải theo lựa chọn đã áp dụng. Trả về true nếu không lỗi.
        // Tải theo lựa chọn đã áp dụng. Trả về true nếu không lỗi, loi chứa nội dung lỗi.
        private bool TaiDuLieu(out string loi)
        {
            loi = "";
            try
            {
                DataTable dt = BaocaoBLL.LayBanChay(topApDung, maloaiApDung, tuApDung, denApDung);
                dtHienTai = dt;

                HienThiLuoi(dt);
                VeBieuDo(dt);
                return true;
            }
            catch (Exception ex)
            {
                loi = ex.Message;
                return false;
            }
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
                sfd.FileName = "BaoCaoBanChay_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    string tieuDe = "BÁO CÁO SẢN PHẨM BÁN CHẠY từ " + tuApDung.ToString("dd/MM/yyyy") +
                                    " đến " + denApDung.ToString("dd/MM/yyyy");
                    BaocaoBLL.XuatExcelBanChay(dtHienTai, sfd.FileName, tieuDe);
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
        private void HienThiLuoi(DataTable dt)
        {
            string maDangChon = null;
            if (dgvDS.CurrentRow != null && dgvDS.Columns.Contains("mahang"))
                maDangChon = Convert.ToString(dgvDS.CurrentRow.Cells["mahang"].Value);

            dgvDS.DataSource = dt;

            DatTieuDe("mahang", "Mã hàng");
            DatTieuDe("tenhang", "Tên hàng");
            DatTieuDe("tenloai", "Loại hàng");
            DatTieuDe("dvt", "ĐVT");
            DatTieuDe("soluongban", "Số lượng bán");
            DatTieuDe("doanhthu", "Doanh thu (VNĐ)");

            if (dgvDS.Columns.Contains("doanhthu"))
                dgvDS.Columns["doanhthu"].DefaultCellStyle.Format = "N0";

            if (maDangChon != null && dgvDS.Columns.Contains("mahang"))
            {
                for (int i = 0; i < dgvDS.Rows.Count; i++)
                {
                    if (Convert.ToString(dgvDS.Rows[i].Cells["mahang"].Value) == maDangChon)
                    {
                        dgvDS.CurrentCell = dgvDS.Rows[i].Cells[0];
                        break;
                    }
                }
            }
        }
        private void DatTieuDe(string cot, string tieuDe)
        {
            if (dgvDS.Columns.Contains(cot))
                dgvDS.Columns[cot].HeaderText = tieuDe;
        }
        private void VeBieuDo(DataTable dt)
        {
            ChartValues<double> giaTri = new ChartValues<double>();
            List<string> nhan = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                giaTri.Add(Convert.ToDouble(dt.Rows[i]["soluongban"]));
                nhan.Add(dt.Rows[i]["tenhang"].ToString());
            }

            // Dữ liệu không đổi thì không vẽ lại, tránh nhấp nháy
            string key = string.Join("|", nhan) + "#" + string.Join("|", giaTri);
            if (key == lastChartKey) return;
            lastChartKey = key;

            columnchartBanchay.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Số lượng bán",
                    Values = giaTri,
                    DataLabels = true,
                    Fill = System.Windows.Media.Brushes.DarkRed
                }
            };
            columnchartBanchay.AxisX.Clear();
            columnchartBanchay.AxisX.Add(new Axis
            {
                Labels = nhan,
                LabelsRotation = 15,
                Separator = new Separator { Step = 1 }
            });
            columnchartBanchay.AxisY.Clear();
            columnchartBanchay.AxisY.Add(new Axis { MinValue = 0 });
        }
    }
}
