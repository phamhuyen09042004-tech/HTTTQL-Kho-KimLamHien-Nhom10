using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTTTQL_Kho_KimLamHien_Nhom10
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Class.Khoitaoservice.KhoiTao();
                Application.Run(new FrmMain());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi khởi động",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
