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
    public partial class FrmQuanlyxuong : Form
    {
        public FrmQuanlyxuong()
        {
            InitializeComponent();
        }

        private void FrmQuanlyxuong_Load(object sender, EventArgs e)
        {
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
     
            txtMaNCC.IconLeft = Image.FromFile(Application.StartupPath + @"\Images\User\ql8.png");
            btnSua.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql6.png");
            btnThem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql3.png");
            btnKhoa.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql7.png");
        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtMaNCC_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
