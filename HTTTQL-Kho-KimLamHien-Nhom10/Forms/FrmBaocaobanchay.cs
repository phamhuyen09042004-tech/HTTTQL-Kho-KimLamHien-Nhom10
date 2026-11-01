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
    public partial class FrmBaocaobanchay : Form
    {
        public FrmBaocaobanchay()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
        }

        private void FrmBaocaobanchay_Load(object sender, EventArgs e)
        {
            dtimeTungay.Value = DateTime.Today.AddDays(-30);
            dtimeDenngay.Value = DateTime.Today;
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
        }
    }
}
