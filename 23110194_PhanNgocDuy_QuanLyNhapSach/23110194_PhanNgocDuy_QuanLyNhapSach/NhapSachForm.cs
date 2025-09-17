using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _23110194_PhanNgocDuy_QuanLyNhapSach
{
    public partial class NhapSachForm : Form
    {
        private DBConnect db;
        private bool isAdmin;
        private string currentMaNV;
        public NhapSachForm(bool isAdmin, string maNV)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            this.currentMaNV = maNV;

            db = new DBConnect(); // Khởi tạo DBConnect
        }

        private void NhapSachForm_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            LoadDataGridView();
            LoadTheLoaiComboBox();
            dtpNgayNhap.Value = DateTime.Now;
            txtNamXuatBan.Text = DateTime.Now.Year.ToString();
            
            dgvNhapSach.ClearSelection(); // Không chọn hàng nào khi load
        }
        private void ConfigureDataGridView()
        {
            // Thêm các cột
            dgvNhapSach.Columns.Add("MaNhanVien", "Mã Nhân Viên");
            dgvNhapSach.Columns.Add("MaTheNhap", "Mã Thẻ Nhập");
            dgvNhapSach.Columns.Add("MaSach", "Mã Sách");
            dgvNhapSach.Columns.Add("TenSach", "Tên Sách");
            dgvNhapSach.Columns.Add("TenTacGia", "Tên Tác Giả");
            dgvNhapSach.Columns.Add("NhaXuatBan", "Nhà Xuất Bản");
            dgvNhapSach.Columns.Add("TheLoai", "Thể Loại");
            dgvNhapSach.Columns.Add("NamXuatBan", "Năm XB");
            dgvNhapSach.Columns.Add("NgayNhap", "Ngày Nhập");
            dgvNhapSach.Columns.Add("SoLuong", "Số Lượng");
            dgvNhapSach.Columns.Add("GiaNhap", "Giá Nhập");
            dgvNhapSach.Columns.Add("ThanhTien", "Thành Tiền");
            dgvNhapSach.Columns.Add("TrangThai", "Trạng Thái");


            // Cấu hình cột
            foreach (DataGridViewColumn column in dgvNhapSach.Columns)
            {
                column.DataPropertyName = column.Name;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Bật wrap text
            }


            // Cấu hình DataGridView
            dgvNhapSach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhapSach.AllowUserToAddRows = false;
            dgvNhapSach.AllowUserToDeleteRows = false;
            dgvNhapSach.ReadOnly = true;
            dgvNhapSach.RowHeadersVisible = false;
            dgvNhapSach.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvNhapSach.ScrollBars = ScrollBars.Both; // Bật cả thanh cuộn dọc và ngang
            dgvNhapSach.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; // Tự động điều chỉnh chiều cao hàng
            dgvNhapSach.RowTemplate.Height = 50; // Chiều cao hàng tối thiểu
            dgvNhapSach.Height = 300;
        }
        private void LoadTheLoaiComboBox()
        {
            cbbTheLoai.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbbTheLoai.AutoCompleteSource = AutoCompleteSource.ListItems;

            string query = "SELECT TenTheLoai FROM THE_LOAI";
            DataTable dt = db.ExecuteQuery(query);
            cbbTheLoai.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                cbbTheLoai.Items.Add(row["TenTheLoai"].ToString());
            }
        }

        private void LoadDataGridView()
        {
            string query = "SELECT * FROM ViewChiTietTheNhap";
            DataTable dt = db.ExecuteQuery(query);
            dgvNhapSach.DataSource = dt;
        }
        private void btnNhapSach_Click(object sender, EventArgs e)
        {
            if (dgvNhapSach.SelectedRows.Count > 0)
            {
                MessageBox.Show("Vui lòng làm sạch dữ liệu trước khi nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput()) return;

            if (!isAdmin && txtMaNhanVien.Text.Trim() != currentMaNV)
            {
                MessageBox.Show("Bạn chỉ được phép nhập sách với mã nhân viên của mình!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlParameter[] paramsArr = new SqlParameter[]
            {
                new SqlParameter("@MaNV", txtMaNhanVien.Text.Trim()),
                new SqlParameter("@TenSach", txtTenSach.Text.Trim()),
                new SqlParameter("@TenTacGia", txtTacGia.Text.Trim()),
                new SqlParameter("@TenTheLoai", cbbTheLoai.Text.Trim()),
                new SqlParameter("@TenNXB", txtNhaXuatBan.Text.Trim()),
                new SqlParameter("@NamXuatBan", int.Parse(txtNamXuatBan.Text)),
                new SqlParameter("@GiaNhap", decimal.Parse(txtGiaNhap.Text)),
                new SqlParameter("@SoLuong", int.Parse(txtSoLuong.Text)),
                new SqlParameter("@NgayNhap", dtpNgayNhap.Value)
            };
            try
            {
                db.ExecuteNonQuery("EXEC sp_NhapSach @MaNV, @TenSach, @TenTacGia, @TenTheLoai, @TenNXB, @NamXuatBan, @GiaNhap, @SoLuong, @NgayNhap", paramsArr);
                MessageBox.Show("Nhập sách vào bảng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGridView();
                ClearInput();
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                MessageBox.Show("Lỗi: Mã nhân viên không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex) when (ex.Number == 50004)
            {
                MessageBox.Show("Lỗi: Không thể tạo hoặc lấy mã sách. Vui lòng kiểm tra dữ liệu đầu vào!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (dgvNhapSach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hàng sách để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput()) return;
            string maTheNhap = dgvNhapSach.SelectedRows[0].Cells["MaTheNhap"].Value.ToString();
            string trangThai = dgvNhapSach.SelectedRows[0].Cells["TrangThai"].Value?.ToString();
            if (trangThai == "Đã nhập")
            {
                MessageBox.Show("Thẻ nhập đã được xác nhận, không thể chỉnh sửa bất kỳ thông tin nào!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlParameter[] paramsArr = new SqlParameter[]
            {
                new SqlParameter("@MaTheNhap", maTheNhap),
                new SqlParameter("@TenSach", txtTenSach.Text.Trim()),
                new SqlParameter("@TenTacGia", txtTacGia.Text.Trim()),
                new SqlParameter("@TenTheLoai", cbbTheLoai.Text.Trim()),
                new SqlParameter("@TenNXB", txtNhaXuatBan.Text.Trim()),
                new SqlParameter("@NamXuatBan", int.Parse(txtNamXuatBan.Text)),
                new SqlParameter("@GiaNhap", decimal.Parse(txtGiaNhap.Text)),
                new SqlParameter("@SoLuong", int.Parse(txtSoLuong.Text)),
                new SqlParameter("@NgayNhap", dtpNgayNhap.Value)
            };
            try
            {
                db.ExecuteNonQuery("EXEC sp_CapNhatSach @MaTheNhap, @TenSach, @TenTacGia, @TenTheLoai, @TenNXB, @NamXuatBan, @GiaNhap, @SoLuong, @NgayNhap", paramsArr);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGridView();
                ClearInput();
            }
            catch (SqlException ex) when (ex.Number == 50007)
            {
                MessageBox.Show("Lỗi: Thẻ nhập đã được xác nhận, không thể chỉnh sửa thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaSach_Click(object sender, EventArgs e)
        {
            if (dgvNhapSach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hàng sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maTheNhap = dgvNhapSach.SelectedRows[0].Cells["MaTheNhap"].Value.ToString();
            string maSach = dgvNhapSach.SelectedRows[0].Cells["MaSach"].Value.ToString();
            string trangThai = dgvNhapSach.SelectedRows[0].Cells["TrangThai"].Value?.ToString();

            if (trangThai == "Đã nhập")
            {
                MessageBox.Show("Hàng sách này đã được nhập vào kho, không thể xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                db.ExecuteNonQuery("DELETE FROM The_Nhap WHERE MaTheNhap = @MaTheNhap; DELETE FROM SACH WHERE MaSach = @MaSach",
                    new SqlParameter[] {
                        new SqlParameter("@MaTheNhap", maTheNhap),
                        new SqlParameter("@MaSach", maSach)
                    });
                MessageBox.Show("Xóa sách thành công!");
                LoadDataGridView();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnUploadAnh_Click(object sender, EventArgs e)
        {
            if (dgvNhapSach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hàng sách để cập nhật ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string trangThai = dgvNhapSach.SelectedRows[0].Cells["TrangThai"].Value?.ToString();
            if (trangThai == "Đã nhập")
            {
                MessageBox.Show("Thẻ nhập đã được xác nhận, không thể cập nhật ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string anhBia = openFileDialog.FileName;
                byte[] imageData = File.ReadAllBytes(anhBia);
                string maSach = dgvNhapSach.SelectedRows[0].Cells["MaSach"].Value.ToString();

                string updateImageQuery = "UPDATE SACH SET AnhBia = @AnhBia WHERE MaSach = @MaSach";
                SqlParameter[] updateImageParams = new SqlParameter[] {
                    new SqlParameter("@AnhBia", SqlDbType.VarBinary) { Value = imageData },
                    new SqlParameter("@MaSach", SqlDbType.VarChar) { Value = maSach }
                };
                try
                {
                    db.ExecuteNonQuery(updateImageQuery, updateImageParams);
                    picUploadAnh.Image = Image.FromFile(anhBia);
                    picUploadAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                    MessageBox.Show("Tải ảnh lên thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
        private bool ValidateInput()
        {
            // Loại bỏ khoảng trắng thừa
            txtMaNhanVien.Text = txtMaNhanVien.Text.Trim();
            txtTenSach.Text = txtTenSach.Text.Trim();
            txtTacGia.Text = txtTacGia.Text.Trim();
            txtNhaXuatBan.Text = txtNhaXuatBan.Text.Trim();
            cbbTheLoai.Text = cbbTheLoai.Text.Trim();
            txtNamXuatBan.Text = txtNamXuatBan.Text.Trim();
            txtSoLuong.Text = txtSoLuong.Text.Trim();
            txtGiaNhap.Text = txtGiaNhap.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text) || string.IsNullOrWhiteSpace(txtTenSach.Text) ||
                string.IsNullOrWhiteSpace(txtTacGia.Text) || string.IsNullOrWhiteSpace(txtNhaXuatBan.Text) ||
                string.IsNullOrWhiteSpace(cbbTheLoai.Text) || string.IsNullOrWhiteSpace(txtNamXuatBan.Text) ||
                string.IsNullOrWhiteSpace(txtSoLuong.Text) || string.IsNullOrWhiteSpace(txtGiaNhap.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return false;
            }
            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!");
                return false;
            }
            if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập 1 cuốn sách phải là số không âm!");
                return false;
            }
            if (!int.TryParse(txtNamXuatBan.Text, out int namXuatBan) || namXuatBan <= 0)
            {
                MessageBox.Show("Năm xuất bản phải là số nguyên dương!");
                return false;
            }
            return true;
        }
        

        private void ClearInput()
        {
            txtMaNhanVien.Text = "";
            txtTenSach.Text = "";
            txtTacGia.Text = "";
            txtNhaXuatBan.Text = "";
            cbbTheLoai.Text = "";
            txtNamXuatBan.Text = DateTime.Now.Year.ToString();
            txtSoLuong.Text = "";
            txtGiaNhap.Text = "";
            picUploadAnh.Image = null;
            dgvNhapSach.ClearSelection();

        }
        private void btnXacNhanNhap_Click(object sender, EventArgs e)
        {
            if (!isAdmin)
            {
                MessageBox.Show("Bạn không có quyền xác nhận nhập! Vui lòng liên hệ Admin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvNhapSach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hàng sách để xác nhận!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maTheNhap = dgvNhapSach.SelectedRows[0].Cells["MaTheNhap"].Value.ToString();
            string trangThai = dgvNhapSach.SelectedRows[0].Cells["TrangThai"].Value?.ToString();
            if (trangThai == "Đã nhập")
            {
                MessageBox.Show("Hàng sách này đã được nhập trước đó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Bạn chắc chắn xác nhận nhập vào kho? Sau khi xác nhận, bạn sẽ không thể chỉnh sửa bất kỳ thông tin nào của thẻ nhập này!",
                "Xác nhận nhập sách", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }
            SqlParameter[] paramsArr = new SqlParameter[]
            {
                new SqlParameter("@MaTheNhap", maTheNhap)
            };
            try
            {
                db.ExecuteNonQuery("EXEC sp_XacNhanNhap @MaTheNhap", paramsArr);
                MessageBox.Show("Xác nhận nhập thành công! Sách đã được tiếp nhận vào kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGridView();
            }
            catch (SqlException ex) when (ex.Number == 50003)
            {
                MessageBox.Show("Lỗi: Mã thẻ nhập không tồn tại hoặc đã được xác nhận. Vui lòng thử lại sau vài giây.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvNhapSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhapSach.Rows[e.RowIndex];
                txtMaNhanVien.Text = row.Cells["MaNhanVien"].Value?.ToString() ?? "";
                txtTenSach.Text = row.Cells["TenSach"].Value?.ToString() ?? "";
                txtTacGia.Text = row.Cells["TenTacGia"].Value?.ToString() ?? "";
                txtNhaXuatBan.Text = row.Cells["NhaXuatBan"].Value?.ToString() ?? "";
                cbbTheLoai.Text = row.Cells["TheLoai"].Value?.ToString() ?? "";
                txtNamXuatBan.Text = row.Cells["NamXuatBan"].Value?.ToString() ?? "";
                dtpNgayNhap.Value = row.Cells["NgayNhap"].Value != DBNull.Value ? (DateTime)row.Cells["NgayNhap"].Value : DateTime.Now;
                txtSoLuong.Text = row.Cells["SoLuong"].Value?.ToString() ?? "";
                txtGiaNhap.Text = row.Cells["GiaNhap"].Value?.ToString() ?? "";
                // Hiển thị ảnh nếu có
                string maSach = row.Cells["MaSach"].Value?.ToString() ?? "";
                string query = "SELECT AnhBia FROM SACH WHERE MaSach = @MaSach";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSach", SqlDbType.VarChar) { Value = maSach }
                };
                DataTable dt = db.ExecuteQuery(query, parameters);
                if (dt.Rows.Count > 0 && dt.Rows[0]["AnhBia"] != DBNull.Value)
                {
                    byte[] imageData = (byte[])dt.Rows[0]["AnhBia"];
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        picUploadAnh.Image = Image.FromStream(ms);
                        picUploadAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                else
                {
                    picUploadAnh.Image = null;
                }
            }
        }

        private void picClean_Click(object sender, EventArgs e)
        {
            ClearInput();

        }
    }
}
