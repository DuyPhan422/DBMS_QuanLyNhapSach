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

namespace _23110194_PhanNgocDuy_QuanLyNhapSach
{
    public partial class LoginForm : Form
    {
        private DBConnect dbConnect;
        public LoginForm()
        {
            InitializeComponent();
            dbConnect = new DBConnect();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            cbbVaiTro.Items.AddRange(new string[] { "Admin", "Nhân viên" });
            cbbVaiTro.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy vai trò được chọn từ ComboBox (0 = Admin, 1 = Thủ thư)
            int selectedVaiTro = cbbVaiTro.SelectedIndex; // 0 cho Admin, 1 cho Thủ thư

            // Câu lệnh SQL để kiểm tra đăng nhập
            string query = "SELECT MaTK, VaiTro, TrangThai FROM TaiKhoan " +
                           "WHERE TenDangNhap = @TenDangNhap AND MatKhauMaHoa = @MatKhau";
            SqlParameter[] paramsArr = new SqlParameter[] {
                new SqlParameter("@TenDangNhap", SqlDbType.VarChar) { Value = tenDangNhap },
                new SqlParameter("@MatKhau", SqlDbType.VarChar) { Value = matKhau } // Thay bằng hashing thực tế
            };

            try
            {
                DataTable dt = dbConnect.ExecuteQuery(query, paramsArr);
                if (dt.Rows.Count > 0)
                {
                    int maTK = Convert.ToInt32(dt.Rows[0]["MaTK"]);
                    int vaiTroFromDB = Convert.ToInt32(dt.Rows[0]["VaiTro"]); // 0 = Admin, 1 = NhanVien
                    int trangThai = Convert.ToInt32(dt.Rows[0]["TrangThai"]); // 0 = KhoaVinhVien, 1 = HoatDong, 2 = TamKhoa

                    if (trangThai == 0)
                    {
                        MessageBox.Show("Tài khoản đã bị khóa vĩnh viễn!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (trangThai == 2)
                    {
                        MessageBox.Show("Tài khoản tạm khóa, vui lòng liên hệ admin!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Kiểm tra vai trò được chọn có khớp với vai trò trong DB không
                    if (selectedVaiTro != vaiTroFromDB)
                    {
                        MessageBox.Show("Vai trò được chọn không khớp với tài khoản!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Đăng nhập thành công, mở ThuThuForm
                    this.Hide();
                    ThuThuForm thuThuForm = new ThuThuForm(maTK, selectedVaiTro == 0); // Truyền MaTK và vai trò
                    thuThuForm.ShowDialog();
                    this.Close(); // Đóng LoginForm sau khi ThuThuForm đóng
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (btnShowPass.Checked)
            {
                txtMatKhau.PasswordChar = '\0';
            }
            else
            {
                txtMatKhau.PasswordChar = '*';
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
