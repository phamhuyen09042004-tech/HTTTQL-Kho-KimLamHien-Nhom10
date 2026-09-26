using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
            this.Resize += (s, ev) => AlignControls();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            Class.Functions.Ketnoi();
            this.BackgroundImage = Image.FromFile(Application.StartupPath + @"\Image\Login\login2.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            AlignControls();
        }
        private void AlignControls()
        {
            btnDangnhap.Left = (this.ClientSize.Width - btnDangnhap.Width) / 2;
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;
            btnDangnhap.Top = this.ClientSize.Height - btnDangnhap.Height - 60;
            label1.Top = btnDangnhap.Top - 30;
        }
        private void btnDangnhap_Click(object sender, EventArgs e)
        {
            FrmLogin f = new FrmLogin();
            this.Hide();
            f.FormClosed += (s, args) =>
            {
                this.Show();
                this.PerformLayout();
                this.Invalidate(true);
                this.Update();
                AlignControls();
            };
            f.Show();
        }
    }
}
