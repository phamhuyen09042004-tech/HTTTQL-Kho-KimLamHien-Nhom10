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
    public partial class FrmGiaodienKT : Form
    {
        private bool isLoggingOut = false;
        private bool isExiting = false;
        public FrmGiaodienKT()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
        }

        private void FrmGiaodienKT_Load(object sender, EventArgs e)
        {
            picLogo.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\logo1.ico");
            picAnh.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\user2.png");
            btnDangxuat.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\nutnguon.png");
            btnBcDoanhthu.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\pnk.png");
            btnSpbanchay.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\category.png");
            btnBctonkho.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\warehouse.png");
            btnTheodoitonkho.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\baocaoton.png");
            btnLichsugiavon.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\history.png");
            lblManguoidung.Text = "Nhân viên: " + session.TenNhanVien;
            lblChucvu.Text = "Chức vụ: " + session.Chucvu;
            lblTime.Text = DateTime.Now.ToString("'Hôm nay, 'HH:mm: 'Ngày' dd/MM/yy");
        }
    }
}
