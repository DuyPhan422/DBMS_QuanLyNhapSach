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
        public TraCuuSachForm()
        {
            InitializeComponent();
            dbConnect = new DBConnect();
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
                            { "SoLuongHienTai", ("Số lượng hiện tại", null, true) },
                            { "GiaSach", ("Giá sách", "N2", true) },
                            { "TrangThaiSach", ("Trạng thái", null, true) },
                            { "AnhBia", ("Ảnh Bìa", null, false) }
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
                            { "MaTacGia", ("Mã Tác Giả", null, true) },
                            { "TenTacGia", ("Tên Tác Giả", null, true) }
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
                            { "MaTheLoai", ("Mã Thể Loại", null, true) },
                            { "TenTheLoai", ("Tên Thể Loại", null, true) }
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
                            { "MaNXB", ("Mã Nhà Xuất Bản", null, true) },
                            { "TenNXB", ("Tên Nhà Xuất Bản", null, true) }
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
                            { "MaTheNhap", ("Mã Thẻ Nhập", null, true) },
                            { "MaNV", ("Mã Nhân Viên", null, true) },
                            { "MaSach", ("Mã Sách", null, true) },
                            { "TenNhanVien", ("Tên Nhân Viên", null, true) },
                            { "TenSach", ("Tên Sách", null, true) },
                            { "NgayNhap", ("Ngày Nhập", "dd/MM/yyyy", true) },
                            { "TongSoLuongNhap", ("Tổng Số Lượng Nhập", null, true) },
                            { "TongTienNhap", ("Tổng Tiền Nhập", "N2", true) },
                            { "TrangThai", ("Trạng Thái", null, true) }
                        }
                    )
                }
            };
        }

        private void TraCuuSachForm_Load(object sender, EventArgs e)
        {
            cbbTraCuu.Items.AddRange(new string[] { "Sách", "Tác giả", "Thể loại", "Nhà xuất bản", "Lịch sử nhập kho" });
         
            cbbTraCuu.SelectedIndex = -1; 
            dgvTraCuu.AutoGenerateColumns = true;
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

                DataTable dt = dbConnect.ExecuteQuery(config.Query);
                dgvTraCuu.DataSource = dt;
                ConfigureColumns(dgvTraCuu, config.Columns);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    
    }
}
    

