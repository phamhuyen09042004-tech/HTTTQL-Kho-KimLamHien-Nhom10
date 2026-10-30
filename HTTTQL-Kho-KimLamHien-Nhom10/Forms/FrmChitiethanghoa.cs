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
    public partial class FrmChitiethanghoa : Form
    {
        private string _mode;
        private string _maHang;
        private string _maKho;
        private string _anhBiaPath = null;
        private decimal _tlSp = 1m;
        private decimal _tienCong = 0m;
        public FrmChitiethanghoa()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
        }

        private void FrmChitiethanghoa_Load(object sender, EventArgs e)
        {
            try
            {
                btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            }
            catch { }

            // Giới hạn NumericUpDown
            numTonkho.Minimum = 0; numTonkho.Maximum = 9_999_999;
            numGiavon.Minimum = 0; numGiavon.Maximum = 999_999_999;
            numGiaban.Minimum = 0; numGiaban.Maximum = 999_999_999;
            numMuctontoithieu.Minimum = 0; numMuctontoithieu.Maximum = 9_999_999;

        }
    }
}
