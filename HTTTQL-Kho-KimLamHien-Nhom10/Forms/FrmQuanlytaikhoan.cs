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
    public partial class FrmQuanlytaikhoan : Form
    {
        public FrmQuanlytaikhoan()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
        }

        private void FrmQuanlytaikhoan_Load(object sender, EventArgs e)
        {
            btnTimkiem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql4.png");
            btnXem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql5.png");
            txtTennhanvien.IconLeft = Image.FromFile(Application.StartupPath + @"\Images\User\ql8.png");
            btnSua.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql6.png");
            btnThem.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql3.png");
            btnLock.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql7.png");
            btnDlaiMK.Image = Image.FromFile(Application.StartupPath + @"\Images\User\ql2.png");
            DataTable dtChucVu = new DataTable();
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            dtChucVu.Columns.Add("Text");
            dtChucVu.Columns.Add("Value");

            dtChucVu.Rows.Add("-- Chọn chức vụ --", "");
            dtChucVu.Rows.Add("Giám đốc", "Giám đốc");
            dtChucVu.Rows.Add("Quản lý kho", "Quản lý kho");
            dtChucVu.Rows.Add("Nhân viên bán hàng", "Nhân viên bán hàng");
            dtChucVu.Rows.Add("Nhân viên thu ngân", "Nhân viên thu ngân");
            dtChucVu.Rows.Add("Kế toán", "Kế toán");

            cboChucvu.DataSource = dtChucVu;
            cboChucvu.DisplayMember = "Text";
            cboChucvu.ValueMember = "Value";

            cboChucvu.SelectedIndex = 0;
            cboChucvu.Height = 45;
            cboChucvu.ItemHeight = 35;

            txtManhanvien.Enabled = false;
            txtManhanvien.Text = "";
        }
    }
}
