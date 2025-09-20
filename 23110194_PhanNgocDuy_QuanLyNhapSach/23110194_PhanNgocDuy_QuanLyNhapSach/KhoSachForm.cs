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

            // Gọi stored procedure với tham số OUTPUT
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@IdS", SqlDbType.Int) { Value = idS },
        new SqlParameter("@SoLuongThem", SqlDbType.Int) { Value = soLuongThem },
        new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output }
            };

            try
            {
                dbConnect.ExecuteNonQuery("sp_CapNhatKhoSach", parameters, CommandType.StoredProcedure);

                // Lấy thông báo từ tham số OUTPUT
                string resultMessage = parameters[2].Value?.ToString() ?? "Không nhận được thông báo từ SP";

                // Hiển thị thông báo theo yêu cầu
                if (resultMessage == "Không có thông tin trong kho")
                {
                    MessageBox.Show(resultMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (resultMessage.Contains("vượt quá số lượng đã nhập"))
                {
                    int startNewQty = resultMessage.IndexOf("vượt quá số lượng đã nhập là ") + "vượt quá số lượng đã nhập là ".Length;
                    int endNewQty = resultMessage.IndexOf(" so với tổng ");
                    string newQty = resultMessage.Substring(startNewQty, endNewQty - startNewQty);
                    MessageBox.Show($"Số lượng thêm vượt quá số lượng đã nhập là {newQty}\nCập nhật thất bại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (resultMessage.Contains("không đủ sách"))
                {
                    int startCurrentQty = resultMessage.IndexOf("Số lượng trong kho hiện tại là ") + "Số lượng trong kho hiện tại là ".Length;
                    int endCurrentQty = resultMessage.IndexOf(" không đủ sách");
                    int startReduceQty = resultMessage.IndexOf("giảm ") + "giảm ".Length;
                    string currentQty = resultMessage.Substring(startCurrentQty, endCurrentQty - startCurrentQty);
                    string reduceQty = resultMessage.Substring(startReduceQty);
                    MessageBox.Show($"Số lượng trong kho hiện tại là {currentQty} không đủ sách\nCập nhật thất bại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (resultMessage == "Cập nhật thành công")
                {
                    MessageBox.Show(resultMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadKhoSach();
                    txtSoLuong.Clear();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Thông báo chung nếu không khớp
                }
            }
            catch (Exception)
            {
                // Bắt mọi lỗi nhưng chỉ hiển thị thông báo chung, không để lộ chi tiết
                MessageBox.Show("Cập nhật thất bại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    lastCheckedMaSach = maSach; // Lưu mã sách vừa kiểm tra
                    hasData = !kiemTraKho.Contains("chưa có dữ liệu nhập kho"); // Cập nhật trạng thái
                }
                else
                {
                    MessageBox.Show($"Mã sách {maSach} không có dữ liệu trạng thái kho!", "Thông tin trạng thái kho", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lastCheckedMaSach = maSach;
                    hasData = false;
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
            lastCheckedMaSach = null; // Làm mới trạng thái kiểm tra
            hasData = true; // Đặt lại trạng thái mặc định
            LoadKhoSach();
        }

        
    }
}
