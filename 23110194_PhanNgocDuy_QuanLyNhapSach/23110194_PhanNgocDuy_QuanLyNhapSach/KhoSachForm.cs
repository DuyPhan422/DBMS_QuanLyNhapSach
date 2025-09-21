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
    public partial class KhoSachForm : Form
    {
        private readonly DBConnect dbConnect;
        private readonly Dictionary<string, (string Header, string Format, bool Visible)> columnConfig;
        private string lastCheckedMaSach; // Lưu mã sách được kiểm tra gần nhất
        private bool hasData;
        public KhoSachForm()
        {
            InitializeComponent();
            dbConnect = new DBConnect();
            columnConfig = new Dictionary<string, (string Header, string Format, bool Visible)>
            {
                { "MaNhanVien", ("Mã nhân viên", null, true) },
                { "MaTheNhap", ("Mã thẻ nhập", null, true) },
                { "MaSach", ("Mã sách", null, true) },
                { "TenSach", ("Tên sách", null, true) },
                { "MaTacGia", ("Mã tác giả", null, true) },
                { "TenTacGia", ("Tên tác giả", null, true) },
                { "MaTheLoai", ("Mã thể loại", null, true) },
                { "TheLoai", ("Thể loại", null, true) },
                { "MaNhaXuatBan", ("Mã nhà xuất bản", null, true) },
                { "TenNhaXuatBan", ("Tên nhà xuất bản", null, true) },
                { "NamXuatBan", ("Năm xuất bản", null, true) },
                { "SoLuong", ("Số lượng hiện tại", null, true) },
                { "NgayNhap", ("Ngày nhập", "dd/MM/yyyy", true) },
                { "GiaNhap", ("Giá nhập", "N0", true) },
                { "ThanhTien", ("Thành tiền", "N0", true) },
                { "TrangThaiSach", ("Trạng thái sách", null, true) }
            };
            LoadMaSachComboBox();
            LoadKhoSach();
            lastCheckedMaSach = null; // Khởi tạo ban đầu
            hasData = true; // Giả định có dữ liệu ban đầu
        }
        private void KhoSachForm_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView(dgvKhoSach);
        }

        private void ConfigureDataGridView(DataGridView dgv)
        {
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.ScrollBars = ScrollBars.Both;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.RowTemplate.Height = 50;
            dgv.Height = 300;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.BackgroundColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgv.GridColor = Color.Gray;
            dgv.BorderStyle = BorderStyle.Fixed3D;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                column.Width = 150;
            }
        }

        private void ConfigureColumns(DataGridView dgv, Dictionary<string, (string Header, string Format, bool Visible)> config)
        {
            foreach (var column in config)
            {
                if (dgv.Columns.Contains(column.Key))
                {
                    dgv.Columns[column.Key].HeaderText = column.Value.Header;
                    if (!string.IsNullOrEmpty(column.Value.Format))
                    {
                        dgv.Columns[column.Key].DefaultCellStyle.Format = column.Value.Format;
                    }
                    dgv.Columns[column.Key].Visible = column.Value.Visible;
                }
            }
        }

        private void LoadKhoSach()
        {
              string query = "SELECT * FROM ViewChiTietNhapKho ORDER BY MaNhanVien";
                DataTable dt = dbConnect.ExecuteQuery(query);
                dgvKhoSach.DataSource = dt;
                ConfigureColumns(dgvKhoSach, columnConfig);
            
        }

        private void LoadMaSachComboBox()
        {

            string query = "SELECT MaSach FROM ViewChiTietNhapKho";
            DataTable dt = dbConnect.ExecuteQuery(query);
            cbxMaSach.DataSource = dt;
            cbxMaSach.DisplayMember = "MaSach";
            cbxMaSach.ValueMember = "MaSach";
            cbxMaSach.SelectedIndex = -1;

        }

        private int GetIdSFromMaSach(string maSach)
        {
            if (string.IsNullOrEmpty(maSach) || !maSach.StartsWith("S") || maSach.Length < 2)
            {
                return -1;
            }
            return int.TryParse(maSach.Substring(1), out int idS) ? idS : -1;
        }
        // Phương thức cập nhật kho sách
        public bool UpdateKhoSach(int idS, int soLuongThem, out string resultMessage)
        {
            resultMessage = "";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IdS", SqlDbType.Int) { Value = idS },
                new SqlParameter("@SoLuongThem", SqlDbType.Int) { Value = soLuongThem },
                new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output }
            };

            try
            {
                dbConnect.ExecuteNonQuery("sp_CapNhatKhoSach", parameters, CommandType.StoredProcedure);
                resultMessage = parameters[2].Value?.ToString() ?? "Cập nhật thất bại";
                return true;
            }
            catch (Exception)
            {
                resultMessage = "Cập nhật thất bại";
                return false;
            }
        }

        // Phương thức kiểm tra trạng thái kho sách
        public bool CheckKhoSach(int idS, out string kiemTraKho)
        {
            kiemTraKho = "";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@p_IdS", SqlDbType.Int) { Value = idS }
            };

            try
            {
                DataTable dt = dbConnect.ExecuteQuery("SELECT dbo.fn_KiemTraTrangThaiKhoSach(@p_IdS) AS KiemTraKho", parameters);
                if (dt.Rows.Count > 0)
                {
                    kiemTraKho = dt.Rows[0]["KiemTraKho"].ToString();
                    return true;
                }
                else
                {
                    kiemTraKho = $"Mã sách S{idS:D3} không có dữ liệu trạng thái kho!";
                    return false;
                }
            }
            catch (Exception ex)
            {
                kiemTraKho = $"Đã xảy ra lỗi: {ex.Message}";
                return false;
            }
        }

        // Phương thức làm sạch dữ liệu
        public bool ClearForm()
        {

            txtSoLuong.Clear();
            txtTenSach.Clear();
            txtSoLuongHienTai.Clear();
            cbxMaSach.SelectedIndex = -1;
            lastCheckedMaSach = null; // Làm mới trạng thái kiểm tra
            hasData = true; // Đặt lại trạng thái mặc định
            
            dgvKhoSach.ClearSelection(); // Bỏ chọn tất cả hàng trong DataGridView
            return true;


        }

        private void btnCapNhatKho_Click(object sender, EventArgs e)
        {
            // Kiểm tra đầu vào
            if (cbxMaSach.SelectedIndex == -1 || string.IsNullOrEmpty(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng chọn mã sách và nhập số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSach = cbxMaSach.SelectedValue?.ToString();
            int idS = GetIdSFromMaSach(maSach);

            if (!int.TryParse(txtSoLuong.Text, out int soLuongThem))
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra số lượng không thay đổi
            if (soLuongThem == 0)
            {
                MessageBox.Show("Số lượng không thay đổi, không cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Gọi phương thức UpdateKhoSach
            string resultMessage;
            bool success = UpdateKhoSach(idS, soLuongThem, out resultMessage);

            // Thay \n bằng Environment.NewLine để hiển thị đúng định dạng xuống dòng
            resultMessage = resultMessage.Replace("\\n", Environment.NewLine);

            // Hiển thị thông báo
            MessageBox.Show(resultMessage, success ? "Thông báo" : "Cảnh báo", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            // Cập nhật giao diện nếu thành công
            if (success && resultMessage == "Cập nhật thành công")
            {
                txtSoLuongHienTai.Text = (int.Parse(txtSoLuongHienTai.Text) + soLuongThem).ToString();
                txtSoLuong.Clear();
            }
        }

        private void btnKiemTraKho_Click(object sender, EventArgs e)
        {
            if (cbxMaSach.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn mã sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSach = cbxMaSach.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(maSach))
            {
                MessageBox.Show("Mã sách không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idS = GetIdSFromMaSach(maSach);
            if (idS == -1)
            {
                MessageBox.Show($"Mã sách '{maSach}' không hợp lệ! Phải theo định dạng Sxxx (ví dụ: S001).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string kiemTraKho;
            bool success = CheckKhoSach(idS, out kiemTraKho);

            MessageBox.Show(kiemTraKho, "Thông tin trạng thái kho", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lastCheckedMaSach = maSach;
            hasData = !kiemTraKho.Contains("chưa có dữ liệu nhập kho");
        }
        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
            }
        }

        

        private void picClean_Click(object sender, EventArgs e)
        {
            bool success = ClearForm();
            if (success)
            {
                MessageBox.Show("Đã làm sạch dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbxMaSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Nếu chưa chọn mã sách, xóa các TextBox và thoát
            if (cbxMaSach.SelectedIndex == -1 || cbxMaSach.SelectedValue == null)
            {
                txtTenSach.Clear();
                txtSoLuongHienTai.Clear();
                return;
            }

            string maSach = cbxMaSach.SelectedValue.ToString();
            if (string.IsNullOrEmpty(maSach))
            {
                txtTenSach.Clear();
                txtSoLuongHienTai.Clear();
                return;
            }

            string query = "SELECT TenSach, SoLuong AS SoLuongHienTai FROM ViewChiTietNhapKho WHERE MaSach = @MaSach";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaSach", SqlDbType.VarChar) { Value = maSach }
            };

            try
            {
                DataTable dt = dbConnect.ExecuteQuery(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    txtTenSach.Text = dt.Rows[0]["TenSach"].ToString();
                    txtSoLuongHienTai.Text = dt.Rows[0]["SoLuongHienTai"].ToString();
                }
                else
                {
                    txtTenSach.Clear();
                    txtSoLuongHienTai.Clear();
                    // Không hiển thị thông báo để tránh làm phiền người dùng
                }
            }
            catch (Exception ex)
            {
                txtTenSach.Clear();
                txtSoLuongHienTai.Clear();
                // Chỉ hiển thị lỗi nghiêm trọng
                MessageBox.Show($"Đã xảy ra lỗi khi truy vấn dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKhoSach_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKhoSach.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvKhoSach.SelectedRows[0];
                string maSach = selectedRow.Cells["MaSach"].Value?.ToString();
                string tenSach = selectedRow.Cells["TenSach"].Value?.ToString();
                string soLuongHienTai = selectedRow.Cells["SoLuong"].Value?.ToString();

                // Cập nhật các control
                if (!string.IsNullOrEmpty(maSach))
                {
                    cbxMaSach.SelectedValue = maSach;
                }
                txtTenSach.Text = tenSach ?? "";
                txtSoLuongHienTai.Text = soLuongHienTai ?? "";
            }
        }
    }
}
