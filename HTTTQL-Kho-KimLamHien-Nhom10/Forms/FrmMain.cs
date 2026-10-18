using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HTTTQL_Kho_KimLamHien_Nhom10.Class;

namespace HTTTQL_Kho_KimLamHien_Nhom10.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            Functions.ApplyGlobalStyles(this);
            this.DoubleBuffered = true;
            this.Resize += (s, ev) => AlignControls();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Gán icon góc trái Form từ thư mục Images
            string iconPath = Path.Combine(Application.StartupPath, "Images", "Giaodien", "logo1.ico");
            if (File.Exists(iconPath))
            {
                this.ShowIcon = true;
                this.Icon = new Icon(iconPath);
            }

            // Tải ảnh nền đăng nhập
            string bgPath = Path.Combine(Application.StartupPath, "Images", "Login", "login2.png");
            if (File.Exists(bgPath))
            {
                using (var bmp = new Bitmap(bgPath))
                {
                    this.BackgroundImage = new Bitmap(bmp);
                }
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            // 1. Kết nối CSDL an toàn
            try
            {
                Functions.Ketnoi();
            }
            catch { }

            // 2. Tải an toàn ảnh nền (hỗ trợ cả thư mục 'Image' lẫn 'Images')
            LoadBackgroundImageAnToan();

            this.BackgroundImageLayout = ImageLayout.Stretch;
            AlignControls();
        }

        private void LoadBackgroundImageAnToan()
        {
            string path1 = Path.Combine(Application.StartupPath, "Image", "Login", "login2.png");
            string path2 = Path.Combine(Application.StartupPath, "Images", "Login", "login2.png");

            string finalPath = File.Exists(path1) ? path1 : (File.Exists(path2) ? path2 : "");

            if (!string.IsNullOrEmpty(finalPath))
            {
                try
                {
                    using (var bmp = new Bitmap(finalPath))
                    {
                        this.BackgroundImage = new Bitmap(bmp); // Tránh lock file
                    }
                }
                catch { }
            }
        }

        private void AlignControls()
        {
            if (btnDangnhap == null || label1 == null) return;

            // Căn giữa màn hình theo chiều ngang
            btnDangnhap.Left = (this.ClientSize.Width - btnDangnhap.Width) / 2;
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;

            // Căn đáy màn hình
            btnDangnhap.Top = this.ClientSize.Height - btnDangnhap.Height - 60;
            label1.Top = btnDangnhap.Top - 35;
        }

        private void btnDangnhap_Click(object sender, EventArgs e)
        {
            this.Hide();

            using (FrmLogin f = new FrmLogin())
            {
                DialogResult result = f.ShowDialog();

                // Nếu đăng nhập THÀNH CÔNG: Mở giao diện làm việc chính (Dashboard)
                if (result == DialogResult.OK)
                {
                    // Chuyển tiếp sang Form giao diện làm việc Dashboard của bạn
                    // (Ví dụ: FrmGiaodienchinh hoặc FrmDashboard)
                    // FrmGiaodienchinh frmGD = new FrmGiaodienchinh();
                    // frmGD.ShowDialog();

                    // Khi người dùng đăng xuất khỏi hệ thống thì mới hiện lại màn hình chào này:
                    this.Show();
                    AlignControls();
                }
                else
                {
                    // Người dùng bấm Hủy / Tắt FrmLogin thì hiện lại màn hình chào
                    this.Show();
                    AlignControls();
                }
            }
        }
    }
}