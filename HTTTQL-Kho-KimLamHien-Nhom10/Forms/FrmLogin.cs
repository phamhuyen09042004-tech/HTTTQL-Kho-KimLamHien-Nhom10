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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void picDesign_Click(object sender, EventArgs e)
        {

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            picShow.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login.jpg");
            piclogo.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login1.png");
            picButton.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\logo.jpg");
            picDesign.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login3.png");
            this.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Images\Login\login4.png");
        }

        private void picShow_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
