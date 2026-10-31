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
    public partial class FrmChitietphieuxuat : Form
    {
        public FrmChitietphieuxuat()
        {
            InitializeComponent();
        }

        private void FrmChitietphieuxuat_Load(object sender, EventArgs e)
        {
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
        }
    }
}
