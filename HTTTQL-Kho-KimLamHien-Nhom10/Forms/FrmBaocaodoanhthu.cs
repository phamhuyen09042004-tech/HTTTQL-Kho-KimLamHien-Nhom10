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
    public partial class FrmBaocaodoanhthu : Form
    {
        public FrmBaocaodoanhthu()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
        }

        private void FrmBaocaodoanhthu_Load(object sender, EventArgs e)
        {
            dgvChitiet.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            dgvChitiet.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dgvChitiet.ColumnHeadersHeight = 60;
            dgvChitiet.RowTemplate.Height = 40;
            dgvChitiet.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.MistyRose;
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
        }
    }
}
