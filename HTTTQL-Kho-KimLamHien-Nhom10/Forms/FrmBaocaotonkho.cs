using HTTTQL_Kho_KimLamHien_Nhom10.BLL;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmBaocaotonkho : Form
    {
        private Timer timerRealtime = new Timer();
        private DataTable dtHienTai;
        private DateTime mocHienTai;
        private string lastChartKey = "";
        private bool dangDatGiaTri = false;
        private bool tickCanhBaoMacDinh;
        private KyBaoCao kyApDung;
        private DateTime tuNgayApDung, denNgayApDung;
        private int thangApDung, namApDung;
        private string maloaiApDung = "";
        private bool canhBaoApDung;
        public FrmBaocaotonkho(bool tickCanhBao = true)
        {
            InitializeComponent();
            tickCanhBaoMacDinh = tickCanhBao;
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
            this.FormClosing += (s, e) => timerRealtime.Stop();
        }

        private void FrmBaocaotonkho_Load(object sender, EventArgs e)
        {
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            dangDatGiaTri = true;
            cboThang.Items.Clear();
            for (int m = 1; m <= 12; m++) cboThang.Items.Add(m);
            cboNam.Items.Clear();
            for (int y = DateTime.Now.Year; y >= DateTime.Now.Year - 5; y--) cboNam.Items.Add(y);
            cboTheloai.DisplayMember = "tenloai";
            cboTheloai.ValueMember = "maloai";
            cboTheloai.DataSource = BaocaoBLL.LayLoaiHangCombo();
            DatMacDinh();
            dangDatGiaTri = false;
            CapNhatTrangThaiControl();
            // Gắn sự kiện sau khi đặt mặc định
            rdoNgay.CheckedChanged += DoiRadio;
            rdoThang.CheckedChanged += DoiRadio;
            rdoNam.CheckedChanged += DoiRadio;
            dtimeTungay.ValueChanged += DoiNgay;
            dtimeDenngay.ValueChanged += DoiNgay;
            btnQuaylai.Click += (s, ev) => this.Close();
            // -= trước để không gắn đôi nếu Designer đã gắn sẵn
            btnXuat.Click -= btnXuat_Click;
            btnXuat.Click += btnXuat_Click;
            ApDungVaTai(false);   // lần đầu: kỳ Ngày hôm nay, tick cảnh báo
            timerRealtime.Interval = 30000;
            timerRealtime.Tick += (s, ev) => TaiDuLieu();
            timerRealtime.Start();
        }
        // Mặc định: kỳ Ngày, hôm nay, tất cả loại hàng, cảnh báo theo tham số khi mở
        private void DatMacDinh()
        {
            rdoNgay.Checked = true;
            dtimeTungay.Value = DateTime.Today;
            dtimeDenngay.Value = DateTime.Today;
            cboThang.SelectedIndex = DateTime.Now.Month - 1;
            cboNam.SelectedIndex = 0;
            cboTheloai.SelectedIndex = 0;
            chkCanhbao.Checked = tickCanhBaoMacDinh;
        }
        // ===== Chọn kỳ và khóa ô =====
        private void DoiRadio(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2RadioButton rb = sender as Guna.UI2.WinForms.Guna2RadioButton;
            if (dangDatGiaTri || (rb != null && !rb.Checked)) return;
            CapNhatTrangThaiControl();
        }
        // Ngày: 2 ô ngày | Tháng: tháng + năm | Năm: chỉ năm
        private void CapNhatTrangThaiControl()
        {
            dtimeTungay.Enabled = rdoNgay.Checked;
            dtimeDenngay.Enabled = rdoNgay.Checked;
            cboThang.Enabled = rdoThang.Checked;
            cboNam.Enabled = rdoThang.Checked || rdoNam.Checked;
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
        private void btnLammoi_Click(object sender, EventArgs e)
        {
            ApDungVaTai(true);
        }
        // Lấy lựa chọn hiện tại làm kỳ áp dụng rồi tải
        private void ApDungVaTai(bool baoKhongCoDuLieu)
        {
            kyApDung = LayKy();
            tuNgayApDung = dtimeTungay.Value;
            denNgayApDung = dtimeDenngay.Value;
            thangApDung = cboThang.SelectedItem != null ? Convert.ToInt32(cboThang.SelectedItem) : 0;
            namApDung = cboNam.SelectedItem != null ? Convert.ToInt32(cboNam.SelectedItem) : 0;
            maloaiApDung = Convert.ToString(cboTheloai.SelectedValue) ?? "";
            canhBaoApDung = chkCanhbao.Checked;

            bool ok = TaiDuLieu();
            if (ok && baoKhongCoDuLieu && dtHienTai != null && dtHienTai.Rows.Count == 0)
            {
                string noiDung = canhBaoApDung
                    ? "Không có mặt hàng nào dưới ngưỡng cảnh báo."
                    : "Không có dữ liệu tồn kho trong lựa chọn đã chọn.";
                MessageBox.Show(noiDung, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private KyBaoCao LayKy()
        {
            if (rdoNgay.Checked) return KyBaoCao.Ngay;
            if (rdoNam.Checked) return KyBaoCao.Nam;
            return KyBaoCao.Thang;
        }
        // ===== Tải dữ liệu =====
        private bool TaiDuLieu()
        {
            try
            {
                DateTime moc = BaocaoBLL.TinhMocTon(kyApDung, tuNgayApDung, denNgayApDung, thangApDung, namApDung);
                DataTable dt = BaocaoBLL.LayTonKho(maloaiApDung, canhBaoApDung, moc);
                dtHienTai = dt;
                mocHienTai = moc;

                HienThiLuoi(dt);

                int duoiNguong = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dt.Rows[i]["tonthucte"]) < Convert.ToInt32(dt.Rows[i]["nguongcanhbao"]))
                        duoiNguong++;
                }

                lblThongbao.ForeColor = Color.DimGray;
                lblThongbao.Text = "Tồn tại ngày " + moc.ToString("dd/MM/yyyy") +
                                   "   |   Tổng số mặt hàng: " + dt.Rows.Count +
                                   "   |   Số mặt hàng dưới ngưỡng: " + duoiNguong;
                if (canhBaoApDung && dt.Rows.Count == 0)
                    lblThongbao.Text = "Không có mặt hàng nào dưới ngưỡng cảnh báo.";

                VeBieuDo(dt);
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
            string maDangChon = null;
            if (dgvDS.CurrentRow != null && dgvDS.Columns.Contains("mahang"))
                maDangChon = Convert.ToString(dgvDS.CurrentRow.Cells["mahang"].Value);

            dgvDS.DataSource = dt;

            DatTieuDe("mahang", "Mã hàng");
            DatTieuDe("tenhang", "Tên hàng");
            DatTieuDe("tenloai", "Loại hàng");
            DatTieuDe("tonthucte", "Tồn thực tế");
            DatTieuDe("tonkhadung", "Tồn khả dụng");
            DatTieuDe("tondukien", "Tồn dự kiến");
            DatTieuDe("nguongcanhbao", "Ngưỡng cảnh báo");
            DatTieuDe("maloai", "Mã thể loại");

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
        // Top 10 mặt hàng tồn thực tế cao nhất (dt đã sắp giảm dần)
        private void VeBieuDo(DataTable dt)
        {
            ChartValues<double> giaTri = new ChartValues<double>();
            List<string> nhan = new List<string>();
            int n = Math.Min(10, dt.Rows.Count);
            for (int i = 0; i < n; i++)
            {
                giaTri.Add(Convert.ToDouble(dt.Rows[i]["tonthucte"]));
                nhan.Add(dt.Rows[i]["tenhang"].ToString());
            }

            string key = string.Join("|", nhan) + "#" + string.Join("|", giaTri);
            if (key == lastChartKey) return;
            lastChartKey = key;

            columnchartTonkho.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Tồn kho",
                    Values = giaTri,
                    DataLabels = true,
                    Fill = System.Windows.Media.Brushes.DarkRed
                }
            };
            columnchartTonkho.AxisX.Clear();
            columnchartTonkho.AxisX.Add(new Axis
            {
                Labels = nhan,
                LabelsRotation = 15,
                Separator = new Separator { Step = 1 }
            });
            columnchartTonkho.AxisY.Clear();
            columnchartTonkho.AxisY.Add(new Axis { MinValue = 0 });
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
                sfd.FileName = "BaoCaoTonKho_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    BaocaoBLL.XuatExcelTonKho(dtHienTai, sfd.FileName,
                        "BÁO CÁO TỒN KHO tại ngày " + mocHienTai.ToString("dd/MM/yyyy"));
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
    }
}
