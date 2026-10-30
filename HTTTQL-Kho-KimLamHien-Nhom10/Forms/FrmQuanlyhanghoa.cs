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
using ClosedXML.Excel;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmQuanlyhanghoa : Form
    {
        public FrmQuanlyhanghoa()
        {
            InitializeComponent();
        }

        private void FrmQuanlyhanghoa_Load(object sender, EventArgs e)
        {
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            btnTimkiem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql4.png");
            btnXem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql5.png");
            txtMahang.IconLeft = Image.FromFile(Application.StartupPath + @"\Images\User\ql8.png");
            btnSua.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql6.png");
            btnThem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql3.png");
            btnKhoa.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql2.png");
            btnXuat.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql7.png");
            cboLoaihang.Height = 50;
            txtMahang.Enabled = false;
        }
    }
}
