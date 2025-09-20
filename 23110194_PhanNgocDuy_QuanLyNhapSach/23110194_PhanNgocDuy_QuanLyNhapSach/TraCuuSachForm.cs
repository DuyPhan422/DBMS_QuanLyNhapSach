using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _23110194_PhanNgocDuy_QuanLyNhapSach
{
    public partial class TraCuuSachForm : Form
    {
        private readonly DBConnect dbConnect;
        private readonly Dictionary<string, (string Query, string SortColumn, Dictionary<string, (string Header, string Format, bool Visible)> Columns)> viewConfig;
        private DataTable originalDataTable; // Lưu trữ dữ liệu gốc để lọc
        private DataView dataView; // Lưu trữ DataView để quản lý bộ lọc
        public TraCuuSachForm()
        {
            InitializeComponent();
            dbConnect = new DBConnect();
            originalDataTable = null;
            dataView = null;
            // Cấu hình view và cột
            viewConfig = new Dictionary<string, (string Query, string SortColumn, Dictionary<string, (string Header, string Format, bool Visible)> Columns)>
            {
                {
                    "Sách",
                    (
                        "SELECT * FROM ViewDanhSachSach ORDER BY MaSach ASC",
                        "MaSach",
                        new Dictionary<string, (string Header, string Format, bool Visible)>
                        {
                            { "MaSach", ("Mã sách", null, true) },
                            { "TenSach", ("Tên sách", null, true) },
                            { "TenTacGia", ("Tác giả", null, true) },
                            { "TenTheLoai", ("Thể loại", null, true) },
                            { "TenNXB", ("Nhà xuất bản", null, true) },
                            { "NamXuatBan", ("Năm xuất bản", null, true) },
                            { "SoLuongHienTai", ("Số lượng hiện tại", "N0", true) },
                            { "GiaSach", ("Giá sách", "N0", true) }, 
                            { "TrangThaiSach", ("Trạng thái", null, true) },
                            { "AnhBia", ("Ảnh bìa", null, false) }
                        }
                    )
                },
                {
                    "Tác giả",
                    (
                        "SELECT * FROM ViewDanhSachTacGia ORDER BY MaTacGia ASC",
                        "MaTacGia",
                        new Dictionary<string, (string Header, string Format, bool Visible)>
                        {
                            { "MaTacGia", ("Mã tác giả", null, true) },
                            { "TenTacGia", ("Tên tác giả", null, true) },
                            { "SoLuongSach", ("Số lượng sách", "N0", true) } // Sử dụng SoLuongSach từ view
                        }
                    )
                },
                {
                    "Thể loại",
                    (
                        "SELECT * FROM ViewDanhSachTheLoai ORDER BY MaTheLoai ASC",
                        "MaTheLoai",
                        new Dictionary<string, (string Header, string Format, bool Visible)>
                        {
                            { "MaTheLoai", ("Mã thể loại", null, true) },
                            { "TenTheLoai", ("Tên thể loại", null, true) },
                            { "SoLuongSach", ("Số lượng sách", "N0", true) } 
                        }
                    )
                },
                {
                    "Nhà xuất bản",
                    (
                        "SELECT * FROM ViewDanhSachNhaXuatBan ORDER BY MaNXB ASC",
                        "MaNXB",
                        new Dictionary<string, (string Header, string Format, bool Visible)>
                        {
                            { "MaNXB", ("Mã nhà xuất bản", null, true) },
                            { "TenNXB", ("Tên nhà xuất bản", null, true) },
                            { "SoLuongSach", ("Số lượng sách", "N0", true) } 
                        }
                    )
                },
                {
                    "Lịch sử nhập kho",
                    (
                        "SELECT * FROM ViewLichSuNhapKho ORDER BY MaTheNhap ASC",
                        "MaTheNhap",
                        new Dictionary<string, (string Header, string Format, bool Visible)>
                        {
                            { "MaTheNhap", ("Mã thẻ nhập", null, true) },
                            { "MaNV", ("Mã nhân viên", null, true) },
                            { "MaSach", ("Mã sách", null, true) },
                            { "TenNhanVien", ("Tên nhân viên", null, true) },
                            { "TenSach", ("Tên sách", null, true) },
                            { "NgayNhap", ("Ngày nhập", "dd/MM/yyyy", true) },
                            { "TongSoLuongNhap", ("Số lượng nhập", "N0", true) },
                            { "GiaNhap", ("Giá nhập", "N0", true) },
                            { "TongTienNhap", ("Thành tiền", "N0", true) }, 
                            { "TrangThai", ("Trạng thái nhập", null, true) }
                        }
                    )
                }
            };
        }


        private void TraCuuSachForm_Load(object sender, EventArgs e)
        {
            
            cbbTraCuu.Items.AddRange(new string[] { "Sách", "Tác giả", "Thể loại", "Nhà xuất bản", "Lịch sử nhập kho" });
            cbbTraCuu.SelectedIndex = -1;
            dgvTraCuu.AutoGenerateColumns = true; // Giữ nguyên AutoGenerateColumns = true
            dgvTraCuu.AllowUserToAddRows = false;
            dgvTraCuu.ReadOnly = true;
            dgvTraCuu.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvTraCuu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvTraCuu.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvTraCuu.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTraCuu.BackgroundColor = Color.White;
            dgvTraCuu.DefaultCellStyle.ForeColor = Color.Black;
            dgvTraCuu.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgvTraCuu.GridColor = Color.Gray;
            dgvTraCuu.BorderStyle = BorderStyle.Fixed3D;
            dgvTraCuu.EnableHeadersVisualStyles = true;
            dgvTraCuu.RowHeadersVisible = false;

        }
        private void ConfigureColumns(DataGridView dgv, Dictionary<string, (string Header, string Format, bool Visible)> columnConfig)
        {
            foreach (var column in columnConfig)
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
        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbbTraCuu.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn một mục để tra cứu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedItem = cbbTraCuu.SelectedItem.ToString();
                if (!viewConfig.TryGetValue(selectedItem, out var config))
                {
                    MessageBox.Show("Mục tra cứu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy và lưu trữ dữ liệu gốc
                originalDataTable = dbConnect.ExecuteQuery(config.Query);
                dataView = new DataView(originalDataTable);
                dgvTraCuu.DataSource = dataView;
                ConfigureColumns(dgvTraCuu, config.Columns);
                txtTimKiem.Text = string.Empty; // Xóa nội dung tìm kiếm khi tra cứu mới
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbTraCuu.SelectedItem == null || originalDataTable == null || dataView == null)
                {
                    return; // Không thực hiện lọc nếu chưa chọn mục tra cứu hoặc chưa có dữ liệu
                }

                string selectedItem = cbbTraCuu.SelectedItem.ToString();
                if (!viewConfig.TryGetValue(selectedItem, out var config))
                {
                    return;
                }

                string filterText = txtTimKiem.Text.Trim().Replace("'", "''"); // Thoát ký tự đặc biệt để tránh lỗi SQL injection
                if (string.IsNullOrEmpty(filterText))
                {
                    dataView.RowFilter = string.Empty; // Xóa bộ lọc để hiển thị toàn bộ dữ liệu
                    dgvTraCuu.DataSource = dataView;
                    ConfigureColumns(dgvTraCuu, config.Columns);
                    return;
                }

                // Tạo bộ lọc cho các cột hiển thị
                var visibleColumns = config.Columns.Where(c => c.Value.Visible).Select(c => c.Key).ToList();
                if (!visibleColumns.Any())
                {
                    dataView.RowFilter = string.Empty; // Không có cột hiển thị để lọc
                    dgvTraCuu.DataSource = dataView;
                    ConfigureColumns(dgvTraCuu, config.Columns);
                    return;
                }

                // Tạo biểu thức lọc
                StringBuilder filterExpression = new StringBuilder();
                bool hasValidFilter = false;
                foreach (var column in visibleColumns)
                {
                    if (!originalDataTable.Columns.Contains(column)) continue; // Bỏ qua nếu cột không tồn tại
                    var columnType = originalDataTable.Columns[column].DataType;
                    string filterCondition = null;

                    if (columnType == typeof(string))
                    {
                        filterCondition = $"[{column}] LIKE '%{filterText}%'";
                    }
                    else if (columnType == typeof(int) || columnType == typeof(decimal))
                    {
                        if (decimal.TryParse(filterText, out decimal number))
                        {
                            filterCondition = $"[{column}] = {number}";
                        }
                    }
                    else if (columnType == typeof(DateTime))
                    {
                        if (DateTime.TryParseExact(filterText, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                        {
                            filterCondition = $"CONVERT([{column}], 'System.String') LIKE '%{filterText}%'";
                        }
                    }

                    if (!string.IsNullOrEmpty(filterCondition))
                    {
                        if (hasValidFilter)
                        {
                            filterExpression.Append(" OR ");
                        }
                        filterExpression.Append(filterCondition);
                        hasValidFilter = true;
                    }
                }

                // Áp dụng bộ lọc
                dataView.RowFilter = hasValidFilter ? filterExpression.ToString() : string.Empty;
                dgvTraCuu.DataSource = dataView;
                ConfigureColumns(dgvTraCuu, config.Columns);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}


    

    

