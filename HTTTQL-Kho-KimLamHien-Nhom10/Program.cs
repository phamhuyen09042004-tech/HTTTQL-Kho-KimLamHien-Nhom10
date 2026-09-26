using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Forms;

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
                // Bắt buộc mở kết nối CSDL trước khi nạp form đầu tiên
                Class.Functions.Ketnoi();
                //  Class.Khoitaoservice.KhoiTao();
                Application.Run(new FrmThemxuong());

                // Đóng kết nối khi tắt app
                Class.Functions.Ngatketnoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi khởi động",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
