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
    public partial class FrmQuanlyNCC : Form
    {
        public FrmQuanlyNCC()
        {
            InitializeComponent();
        }

        private void guna2ShadowPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmQuanlyNCC_Load(object sender, EventArgs e)
        {
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            txtMaNCC.IconLeft = Image.FromFile(Application.StartupPath + @"\Images\User\ql8.png");
            btnSua.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql6.png");
            btnThem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql3.png");
            btnKhoa.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql7.png");
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
