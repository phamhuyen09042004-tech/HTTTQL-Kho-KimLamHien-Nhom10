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
        //kiểm tra mật khẩu mới đạt tiêu chí không
        private void Kiemtratieuchi()
        {
            string mk = txtMatkhaumoi.Text;

            if (mk.Length < 6)
            {
                lblTieuchi1.Text = "Mật khẩu phải có ít nhất 6 kí tự";
                lblTieuchi1.ForeColor = Color.Red;
            }
            else
            {
                lblTieuchi1.Text = "Mật khẩu phải có ít nhất 6 kí tự";
                lblTieuchi1.ForeColor = Color.Green;
            }
            if (mk.Any(c => char.IsUpper(c)))
            {
                lblTieuchi2.Text = "Mật khẩu phải có ít nhất một chữ in hoa";
                lblTieuchi2.ForeColor = Color.Green;
            }
            else
            {
                lblTieuchi2.Text = "Mật khẩu phải có ít nhất một chữ in hoa";
                lblTieuchi2.ForeColor = Color.Red;
            }
            if (mk.Any(c => char.IsDigit(c)))
            {
                lblTieuchi3.Text = "Mật khẩu phải có ít nhất một chữ số";
                lblTieuchi3.ForeColor = Color.Green;
            }
            else
            {
                lblTieuchi3.Text = "Mật khẩu phải có ít nhất một chữ số";
                lblTieuchi3.ForeColor = Color.Red;
            }
            if (mk.Any(c => !char.IsLetterOrDigit(c)))
            {
                lblTieuchi4.Text = "Mật khẩu phải có ít nhất một kí tự đặc biệt";
                lblTieuchi4.ForeColor = Color.Green;
            }
            else
            {
                lblTieuchi4.Text = "Mật khẩu phải có ít nhất một kí tự đặc biệt";
                lblTieuchi4.ForeColor = Color.Red;
            }
        }
        private void txtMatkhaumoi_TextChanged(object sender, EventArgs e)
        {
            Kiemtratieuchi();
            KiemtrakhopMK();
        }
        private void txtNhaplaiMK_TextChanged(object sender, EventArgs e)
        {
            KiemtrakhopMK();
        }
        private void KiemtrakhopMK()
        {
            if (txtNhaplaiMK.Text.Length > 0 && txtMatkhaumoi.Text == txtNhaplaiMK.Text)
            {
                lblThongbao.Text = "Mật khẩu trùng khớp";
                lblThongbao.ForeColor = Color.Green;
            }
            else if (txtNhaplaiMK.Text.Length == 0)
            {
                lblThongbao.Text = "Bạn phải nhập lại mật khẩu mới";
                lblThongbao.ForeColor = Color.Red;
            }
            else
            {
                lblThongbao.Text = "Mật khẩu không trùng khớp";
                lblThongbao.ForeColor = Color.Red;
            }
        }
        bool ShowPass = false;
        private void pic1_Click(object sender, EventArgs e)
        {
            if (ShowPass)
            {
                txtMatkhau.UseSystemPasswordChar = true;
                ShowPass = false;
            }
            else
            {
                txtMatkhau.UseSystemPasswordChar = false;
                ShowPass = true;
            }
            if (txtMatkhau.UseSystemPasswordChar)
            {
                pic1.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login5.jpg");
            }
            else
            {
                pic1.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login.jpg");
            }
        }
        private void pic2_Click(object sender, EventArgs e)
        {
            if (ShowPass)
            {
                txtMatkhaumoi.UseSystemPasswordChar = true;
                ShowPass = false;
            }
            else
            {
                txtMatkhaumoi.UseSystemPasswordChar = false;
                ShowPass = true;
            }
            if (txtMatkhaumoi.UseSystemPasswordChar)
            {
                pic2.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login5.jpg");
            }
            else
            {
                pic2.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login.jpg");
            }
        }
        private void pic3_Click(object sender, EventArgs e)
        {
            if (ShowPass)
            {
                txtNhaplaiMK.UseSystemPasswordChar = true;
                ShowPass = false;
            }
            else
            {
                txtNhaplaiMK.UseSystemPasswordChar = false;
                ShowPass = true;
            }
            if (txtNhaplaiMK.UseSystemPasswordChar)
            {
                pic3.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login5.jpg");
            }
            else
            {
                pic3.Image = Image.FromFile(Application.StartupPath + @"\Images\Login\login.jpg");
            }
        }
        private bool Kiemtradutieuchi(string mk) //bool trả về true hoặc false
        {
            if (mk.Length < 6)
            { return false; }
            if (!mk.Any(c => char.IsUpper(c)))
            { return false; }
            if (!mk.Any(c => char.IsDigit(c)))
            { return false; }
            if (!mk.Any(c => !char.IsLetterOrDigit(c)))
            { return false; }
            return true;
        }
        private void btnXacnhan_Click(object sender, EventArgs e)
        {
            if (txtMatkhau.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập vào mật khẩu hiện tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMatkhaumoi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập vào mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtNhaplaiMK.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập lại mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Kiemtradutieuchi(txtMatkhaumoi.Text.Trim()) == false)
            {
                MessageBox.Show("Mật khẩu mới chưa đạt đủ tiêu chí", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtMatkhaumoi.Text.Trim() != txtNhaplaiMK.Text.Trim())
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // lấy mật khẩu đã mã hóa để xác minh mật khẩu cũ
            
            MessageBox.Show("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn thoát?\nBạn sẽ cần đổi mật khẩu ở lần đăng nhập tiếp theo.", "Thông báo",
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnXacnhan_Click_1(object sender, EventArgs e)
        {
            if (txtMatkhau.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập vào mật khẩu hiện tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMatkhaumoi.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập vào mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtNhaplaiMK.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập lại mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Kiemtradutieuchi(txtMatkhaumoi.Text.Trim()) == false)
            {
                MessageBox.Show("Mật khẩu mới chưa đạt đủ tiêu chí", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtMatkhaumoi.Text.Trim() != txtNhaplaiMK.Text.Trim())
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // lấy mật khẩu đã mã hóa để xác minh mật khẩu cũ
            
            MessageBox.Show("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.", "Thông báo",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn thoát?\nBạn sẽ cần đổi mật khẩu ở lần đăng nhập tiếp theo.", "Thông báo",
               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
