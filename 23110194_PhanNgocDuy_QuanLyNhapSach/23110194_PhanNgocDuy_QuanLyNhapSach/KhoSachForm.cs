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
            try
            {
                string query = "SELECT * FROM ViewChiTietNhapKho ORDER BY MaNhanVien";
                DataTable dt = dbConnect.ExecuteQuery(query);
                dgvKhoSach.DataSource = dt;
                ConfigureColumns(dgvKhoSach, columnConfig);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMaSachComboBox()
        {
            try
            {
                string query = "SELECT MaSach FROM ViewChiTietNhapKho";
                DataTable dt = dbConnect.ExecuteQuery(query);
                cbxMaSach.DataSource = dt;
                cbxMaSach.DisplayMember = "MaSach";
                cbxMaSach.ValueMember = "MaSach";
                cbxMaSach.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi tải mã sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetIdSFromMaSach(string maSach)
        {
            if (string.IsNullOrEmpty(maSach) || !maSach.StartsWith("S") || maSach.Length < 2)
            {
                return -1;
            }
            return int.TryParse(maSach.Substring(1), out int idS) ? idS : -1;
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

            // Xử lý lỗi từ SQL
            try
            {
                dbConnect.ExecuteNonQuery("sp_CapNhatKhoSach", new SqlParameter[]
                {
            new SqlParameter("@IdS", idS),
            new SqlParameter("@SoLuongThem", soLuongThem)
                }, CommandType.StoredProcedure);

                // Chỉ hiển thị thông báo thành công nếu không có lỗi
                MessageBox.Show("Cập nhật kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadKhoSach();
                txtSoLuong.Clear();
            }
            catch (SqlException ex)
            {
                // Hiển thị thông báo lỗi từ stored procedure
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKiemTraKho_Click(object sender, EventArgs e)
        {
            try
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

                DataTable dt = dbConnect.ExecuteQuery("SELECT dbo.fn_KiemTraTrangThaiKhoSach(@p_IdS) AS KiemTraKho",
                    new SqlParameter[] { new SqlParameter("@p_IdS", idS) });

                if (dt.Rows.Count > 0)
                {
                    string kiemTraKho = dt.Rows[0]["KiemTraKho"].ToString();
                    MessageBox.Show(kiemTraKho, "Thông tin trạng thái kho", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Mã sách {maSach} không có dữ liệu trạng thái kho!", "Thông tin trạng thái kho", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            txtSoLuong.Clear();
            cbxMaSach.SelectedIndex = -1;

            LoadKhoSach();
        }

        
    }
}
