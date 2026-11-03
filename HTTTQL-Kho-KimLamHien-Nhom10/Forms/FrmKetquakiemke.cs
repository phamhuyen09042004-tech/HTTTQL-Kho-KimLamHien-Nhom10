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
    public partial class FrmKetquakiemke : Form
    {
        private string makho;
        private bool isSaved = false;
        public FrmKetquakiemke()
        {
            InitializeComponent();
            Class.Functions.ApplyGlobalStyles(this);
        }
        public static string CheDoxemMaPhieu { get; set; } = null;
        private void FrmKetquakiemke_Load(object sender, EventArgs e)
        {
            btnQuaylai.Image = Image.FromFile(Application.StartupPath + @"\Images\Giaodien\gd10.png");
            makho = session.YeuCauKiemKeKho;
        }
    }
}
