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
    public partial class FrmDoimatkhau : Form
    {
        public FrmDoimatkhau()
        {
            InitializeComponent();
        }
        private void FrmDoimatkhau_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Images\Login\login4.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            picDesign.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login3.png");
            picButton.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\logo.jpg");
            pic1.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login5.jpg");
            pic2.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login5.jpg");
            pic3.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login5.jpg");
            txtDangnhap.Enabled = false;
            txtMatkhau.Focus();
        }
        
    }
}
