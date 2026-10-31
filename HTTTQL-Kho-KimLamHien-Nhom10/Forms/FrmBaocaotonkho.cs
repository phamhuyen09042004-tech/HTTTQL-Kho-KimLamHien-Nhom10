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
        DataTable tblDT;

        public FrmBaocaotonkho()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
        }

        private void FrmBaocaotonkho_Load(object sender, EventArgs e)
        {
            rdoNgay.Checked = true;
            dtimeTungay.Value = DateTime.Today.AddDays(-30);
            dtimeDenngay.Value = DateTime.Today;
            cboThang.SelectedIndex = -1;
            cboNam.SelectedIndex = -1;
            CapNhatHienThi();
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            btnLammoi_Click(null, null);
        }

        private void CapNhatHienThi()
        {
            if (rdoNgay.Checked)
            {
                dtimeTungay.Enabled = true;
                dtimeDenngay.Enabled = true;
                cboThang.Enabled = false;
                cboNam.Enabled = false;
            }
            else if (rdoThang.Checked)
            {
                dtimeTungay.Enabled = false;
                dtimeDenngay.Enabled = false;
                cboThang.Enabled = true;
                cboNam.Enabled = true;
            }
            else
            {
                dtimeTungay.Enabled = false;
                dtimeDenngay.Enabled = false;
                cboThang.Enabled = false;
                cboNam.Enabled = true;
            }
        }


        private void rdoNgay_CheckedChanged(object sender, EventArgs e)
        {
            CapNhatHienThi();
        }

        private void rdoThang_CheckedChanged(object sender, EventArgs e)
        {
            CapNhatHienThi();
        }

        private void rdoNam_CheckedChanged(object sender, EventArgs e)
        {
            CapNhatHienThi();
        }

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            try
            {
                string loaiKy = rdoNgay.Checked ? "Ngay"
                              : rdoThang.Checked ? "Thang"
                              : "Nam";

                DateTime tuNgay = dtimeTungay.Value.Date;
                DateTime denNgay = dtimeDenngay.Value.Date;

                int thang = 0;
                int nam = 0;

                if (cboThang.SelectedItem != null && cboThang.SelectedItem.ToString() != "")
                    thang = Convert.ToInt32(cboThang.SelectedItem);

                if (cboNam.SelectedItem != null && cboNam.SelectedItem.ToString() != "")
                    nam = Convert.ToInt32(cboNam.SelectedItem);

                string maTheLoai = cboTheloai.SelectedValue?.ToString() ?? "ALL";
                bool chiCanhBao = chkCanhbao.Checked;

                string tuNgaySql = tuNgay.ToString("yyyy-MM-dd");
                string denNgaySql = denNgay.ToString("yyyy-MM-dd");

                    return;



                // Ghi nhật ký hoạt động


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (tblDT == null || tblDT.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Files|*.xlsx";
                sfd.FileName = "BaoCaoTonKho_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var wb = new ClosedXML.Excel.XLWorkbook();
                    var ws = wb.Worksheets.Add("Báo cáo tồn kho");

                    int currentCol = 1;
                    for (int i = 0; i < dgvDS.Columns.Count; i++)
                    {
                        if (!dgvDS.Columns[i].Visible) continue;
                        ws.Cell(1, currentCol).Value = dgvDS.Columns[i].HeaderText;
                        ws.Cell(1, currentCol).Style.Font.Bold = true;
                        ws.Cell(1, currentCol).Style.Fill.BackgroundColor =
                            ClosedXML.Excel.XLColor.FromArgb(139, 0, 0);
                        ws.Cell(1, currentCol).Style.Font.FontColor =
                            ClosedXML.Excel.XLColor.White;
                        ws.Cell(1, currentCol).Style.Alignment.Horizontal =
                            ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                        currentCol++;
                    }

                    for (int r = 0; r < tblDT.Rows.Count; r++)
                    {
                        currentCol = 1;
                        for (int c = 0; c < dgvDS.Columns.Count; c++)
                        {
                            if (dgvDS.Columns[c].Visible)
                            {
                                string propName = dgvDS.Columns[c].DataPropertyName;
                                ws.Cell(r + 2, currentCol).Value = tblDT.Rows[r][propName].ToString();
                                currentCol++;
                            }
                        }
                    }

                    int dongTong = tblDT.Rows.Count + 2;
                    ws.Cell(dongTong, 1).Value = lblThongbao.Text;
                    ws.Cell(dongTong, 1).Style.Font.Bold = true;

                    ws.Columns().AdjustToContents();
                    wb.SaveAs(sfd.FileName);

                    // Ghi nhật ký

                    MessageBox.Show("Xuất Excel thành công!\n" + sfd.FileName,
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tblDT == null || tblDT.Rows.Count == 0) return;
            if (e.RowIndex < 0) return;

            string tenSach = dgvDS.CurrentRow.Cells["TenSach"].Value.ToString();
            int ton = Convert.ToInt32(dgvDS.CurrentRow.Cells["TonThucTe"].Value);
            int nguong = Convert.ToInt32(dgvDS.CurrentRow.Cells["NguongCanhBao"].Value);

            lblThongbao.Text = "Hàng hóa: " + tenSach +
                               "   |   Tồn thực tế: " + ton +
                               "   |   Ngưỡng: " + nguong +
                               (ton <= nguong ? "   DƯỚI NGƯỠNG" : "   Đủ hàng");
        }

        private void btnQuaylai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
