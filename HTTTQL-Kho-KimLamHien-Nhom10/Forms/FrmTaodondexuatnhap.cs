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
    public partial class FrmTaodondexuatnhap : Form
    {
        public FrmTaodondexuatnhap()
        {
            InitializeComponent();
        }

        private void FrmTaodondexuatnhap_Load(object sender, EventArgs e)
        {
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd1.png");
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
        }

        private void btnThemNCC_Click(object sender, EventArgs e)
        {

        }

        private void cboNCC_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dtpThoigian_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnGui_Click(object sender, EventArgs e)
        {

        }
    }
}
