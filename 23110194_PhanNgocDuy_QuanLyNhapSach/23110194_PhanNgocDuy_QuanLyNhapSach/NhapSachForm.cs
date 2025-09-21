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
        private byte[] tempImageData;
        private Dictionary<string, byte[]> tempImages;

        public NhapSachForm(bool isAdmin, string maNV)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            this.currentMaNV = maNV;
            db = new DBConnect();
            tempImageData = null;
            tempImages = new Dictionary<string, byte[]>();
        }


        private void NhapSachForm_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            LoadDataGridView();
            LoadTheLoaiComboBox();

            tempImages = new Dictionary<string, byte[]>();
        }
        private void ConfigureDataGridView()
        {
            dgvNhapSach.Columns.Add("MaNhanVien", "Mã nhân viên");
            dgvNhapSach.Columns.Add("MaTheNhap", "Mã thẻ nhập");
            dgvNhapSach.Columns.Add("MaSach", "Mã sách");
            dgvNhapSach.Columns.Add("TenSach", "Tên sách");
            dgvNhapSach.Columns.Add("TenTacGia", "Tên tác giả");
            dgvNhapSach.Columns.Add("TheLoai", "Thể Loại");
            dgvNhapSach.Columns.Add("NhaXuatBan", "Nhà xuất bản");
            dgvNhapSach.Columns.Add("NamXuatBan", "Năm xuất bản");
            dgvNhapSach.Columns.Add("NgayNhap", "Ngày nhập");
            dgvNhapSach.Columns.Add("SoLuong", "Số lượng nhập");
            dgvNhapSach.Columns.Add("GiaNhap", "Giá nhập");
            dgvNhapSach.Columns.Add("ThanhTien", "Thành tiền");
            dgvNhapSach.Columns.Add("TrangThai", "Trạng thái nhập");
            dgvNhapSach.Height = 320;

            foreach (DataGridViewColumn column in dgvNhapSach.Columns)
            {
                column.DataPropertyName = column.Name;
            }

            dgvNhapSach.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            dgvNhapSach.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

        }
        // Tải danh sách thể loại vào ComboBox
        private void LoadTheLoaiComboBox()
        {
            string query = "SELECT TenTheLoai FROM THE_LOAI";
            DataTable dt = db.ExecuteQuery(query);
            cbbTheLoai.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                cbbTheLoai.Items.Add(row["TenTheLoai"].ToString());
            }
        }
        // Tải dữ liệu từ ViewChiTietTheNhap vào DataGridView
        private void LoadDataGridView()
        {
            string query = "SELECT * FROM ViewChiTietTheNhap";
            DataTable dt = db.ExecuteQuery(query);
            dgvNhapSach.DataSource = dt;
        }

        private bool ValidateInput()
        {
            txtMaNhanVien.Text = txtMaNhanVien.Text.Trim();
            txtTenSach.Text = txtTenSach.Text.Trim();
            txtTacGia.Text = txtTacGia.Text.Trim();
            txtNhaXuatBan.Text = txtNhaXuatBan.Text.Trim();
            cbbTheLoai.Text = cbbTheLoai.Text.Trim();
            txtNamXuatBan.Text = txtNamXuatBan.Text.Trim();
            txtSoLuong.Text = txtSoLuong.Text.Trim();
            txtGiaNhap.Text = txtGiaNhap.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtMaNhanVien.Text) ||
                string.IsNullOrWhiteSpace(txtTenSach.Text) ||
                string.IsNullOrWhiteSpace(txtTacGia.Text) ||
                string.IsNullOrWhiteSpace(txtNhaXuatBan.Text) ||
                string.IsNullOrWhiteSpace(cbbTheLoai.Text) ||
                string.IsNullOrWhiteSpace(txtNamXuatBan.Text) ||
                string.IsNullOrWhiteSpace(txtSoLuong.Text) ||
                string.IsNullOrWhiteSpace(txtGiaNhap.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập phải là số không âm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!int.TryParse(txtNamXuatBan.Text, out int namXuatBan) || namXuatBan <= 0 || namXuatBan > DateTime.Now.Year)
            {
                MessageBox.Show("Năm xuất bản phải là số nguyên dương và không được lớn hơn năm hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            tempImageData = null;
            dgvNhapSach.ClearSelection();
        }



        // Phương thức thêm sách mới
        public bool InsertSach(string maNV, string tenSach, string tenTacGia, string tenTheLoai, string tenNXB, int namXuatBan, decimal giaNhap, int soLuong, DateTime ngayNhap, byte[] anhBia)
        {
            SqlParameter[] paramsArr = new SqlParameter[]
            {
                new SqlParameter("@MaNV", SqlDbType.VarChar) { Value = maNV },
                new SqlParameter("@TenSach", SqlDbType.NVarChar) { Value = tenSach },
                new SqlParameter("@TenTacGia", SqlDbType.NVarChar) { Value = tenTacGia },
                new SqlParameter("@TenTheLoai", SqlDbType.NVarChar) { Value = tenTheLoai },
                new SqlParameter("@TenNXB", SqlDbType.NVarChar) { Value = tenNXB },
                new SqlParameter("@NamXuatBan", SqlDbType.Int) { Value = namXuatBan },
                new SqlParameter("@GiaNhap", SqlDbType.Decimal) { Value = giaNhap },
                new SqlParameter("@SoLuong", SqlDbType.Int) { Value = soLuong },
                new SqlParameter("@NgayNhap", SqlDbType.Date) { Value = ngayNhap }
            };

            try
            {
                // Match the original call in btnNhapSach_Click
                DataTable result = db.ExecuteQuery(
                    "EXEC sp_NhapSach @MaNV, @TenSach, @TenTacGia, @TenTheLoai, @TenNXB, @NamXuatBan, @GiaNhap, @SoLuong, @NgayNhap",
                    paramsArr);

                string maSach = result.Rows[0]["MaSach"].ToString();
                string maTheNhap = result.Rows[0]["MaTheNhap"].ToString();

                if (anhBia != null)
                {
                    SqlParameter[] imageParams = new SqlParameter[]
                    {
                        new SqlParameter("@MaSach", SqlDbType.VarChar) { Value = maSach },
                        new SqlParameter("@AnhBia", SqlDbType.VarBinary) { Value = anhBia }
                    };
                    db.ExecuteNonQuery("EXEC sp_CapNhatAnhBia @MaSach, @AnhBia", imageParams);
                    tempImages[maTheNhap] = anhBia;
                }

                MessageBox.Show("Nhập sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Phương thức cập nhật sách
        public bool UpdateSach(string maTheNhap, string tenSach, string tenTacGia, string tenTheLoai, string tenNXB, int namXuatBan, decimal giaNhap, int soLuong, DateTime ngayNhap)
        {
            SqlParameter[] paramsArr = new SqlParameter[]
            {
                new SqlParameter("@MaTheNhap", SqlDbType.VarChar) { Value = maTheNhap },
                new SqlParameter("@TenSach", SqlDbType.NVarChar) { Value = tenSach },
                new SqlParameter("@TenTacGia", SqlDbType.NVarChar) { Value = tenTacGia },
                new SqlParameter("@TenTheLoai", SqlDbType.NVarChar) { Value = tenTheLoai },
                new SqlParameter("@TenNXB", SqlDbType.NVarChar) { Value = tenNXB },
                new SqlParameter("@NamXuatBan", SqlDbType.Int) { Value = namXuatBan },
                new SqlParameter("@GiaNhap", SqlDbType.Decimal) { Value = giaNhap },
                new SqlParameter("@SoLuong", SqlDbType.Int) { Value = soLuong },
                new SqlParameter("@NgayNhap", SqlDbType.Date) { Value = ngayNhap }
            };

            try
            {
                db.ExecuteNonQuery("sp_CapNhatSach", paramsArr, CommandType.StoredProcedure);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Phương thức xóa sách
        public bool DeleteSach(string maTheNhap, string maSach)
        {
            SqlParameter[] paramsArr = new SqlParameter[]
            {
                new SqlParameter("@MaTheNhap", SqlDbType.VarChar) { Value = maTheNhap },
                new SqlParameter("@MaSach", SqlDbType.VarChar) { Value = maSach }
            };

            try
            {
                db.ExecuteNonQuery("sp_XoaSach", paramsArr, CommandType.StoredProcedure);
                tempImages.Remove(maTheNhap);
                MessageBox.Show("Xóa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Phương thức xác nhận nhập sách
        public bool ConfirmNhapSach(string maTheNhap)
        {
            if (!isAdmin)
            {
                MessageBox.Show("Bạn không có quyền xác nhận nhập! Vui lòng liên hệ Admin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            SqlParameter[] paramsArr = new SqlParameter[]
            {
                new SqlParameter("@MaTheNhap", SqlDbType.VarChar) { Value = maTheNhap }
            };

        
                db.ExecuteNonQuery("sp_XacNhanNhap", paramsArr, CommandType.StoredProcedure);
                tempImages.Remove(maTheNhap);
                MessageBox.Show("Xác nhận nhập thành công! Sách đã được tiếp nhận vào kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            
        }

        // Phương thức cập nhật ảnh bìa
        public bool UpdateAnhBia(string maSach, byte[] anhBia)
        {
            SqlParameter[] imageParams = new SqlParameter[]
            {
                new SqlParameter("@MaSach", SqlDbType.VarChar) { Value = maSach },
                new SqlParameter("@AnhBia", SqlDbType.VarBinary) { Value = anhBia }
            };

            try
            {
                db.ExecuteNonQuery("sp_CapNhatAnhBia", imageParams, CommandType.StoredProcedure);
                MessageBox.Show("Tải ảnh lên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool ClearForm()
        {
            try
            {
                ClearInput();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
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

            bool success = InsertSach(
                txtMaNhanVien.Text.Trim(),
                txtTenSach.Text.Trim(),
                txtTacGia.Text.Trim(),
                cbbTheLoai.Text.Trim(),
                txtNhaXuatBan.Text.Trim(),
                int.Parse(txtNamXuatBan.Text),
                decimal.Parse(txtGiaNhap.Text),
                int.Parse(txtSoLuong.Text),
                dtpNgayNhap.Value,
                tempImageData
            );

            if (success)
            {
                LoadDataGridView();
                ClearInput();
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
                MessageBox.Show("Thẻ nhập đã được xác nhận, không thể chỉnh sửa thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success = UpdateSach(
                maTheNhap,
                txtTenSach.Text.Trim(),
                txtTacGia.Text.Trim(),
                cbbTheLoai.Text.Trim(),
                txtNhaXuatBan.Text.Trim(),
                int.Parse(txtNamXuatBan.Text),
                decimal.Parse(txtGiaNhap.Text),
                int.Parse(txtSoLuong.Text),
                dtpNgayNhap.Value
            );

            if (success)
            {
                LoadDataGridView();
                ClearInput();
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

            bool success = DeleteSach(maTheNhap, maSach);
            if (success)
            {
                LoadDataGridView();
                ClearInput();
            }
        }
        // Xử lý tải và cập nhật ảnh bìa
        

        private void btnUploadAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string anhBia = openFileDialog.FileName;
                tempImageData = File.ReadAllBytes(anhBia);
                picUploadAnh.Image = Image.FromFile(anhBia);
                picUploadAnh.SizeMode = PictureBoxSizeMode.StretchImage;

                if (tempImageData.Length > 1048576)
                {
                    MessageBox.Show("Kích thước ảnh không được vượt quá 1MB!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tempImageData = null;
                    picUploadAnh.Image = null;
                    return;
                }

                if (dgvNhapSach.SelectedRows.Count > 0)
                {
                    string trangThai = dgvNhapSach.SelectedRows[0].Cells["TrangThai"].Value?.ToString();
                    string maTheNhap = dgvNhapSach.SelectedRows[0].Cells["MaTheNhap"].Value?.ToString();
                    string maSach = dgvNhapSach.SelectedRows[0].Cells["MaSach"].Value?.ToString();

                    if (trangThai == "Đã nhập")
                    {
                        MessageBox.Show("Thẻ nhập đã được xác nhận, không thể cập nhật ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tempImageData = null;
                        picUploadAnh.Image = null;
                        return;
                    }

                    bool success = UpdateAnhBia(maSach, tempImageData);
                    if (success)
                    {
                        tempImages[maTheNhap] = tempImageData;
                    }
                    else
                    {
                        tempImageData = null;
                        picUploadAnh.Image = null;
                    }
                }
                else
                {
                    MessageBox.Show("Tải ảnh lên thành công! Ảnh sẽ được lưu khi nhập sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Kiểm tra đầu vào hợp lệ
        

        private void btnXacNhanNhap_Click(object sender, EventArgs e)
        {
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

            bool success = ConfirmNhapSach(maTheNhap);
            if (success)
            {
                LoadDataGridView();
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

                string maSach = row.Cells["MaSach"].Value?.ToString() ?? "";
                string maTheNhap = row.Cells["MaTheNhap"].Value?.ToString() ?? "";
                string trangThai = row.Cells["TrangThai"].Value?.ToString() ?? "";

                if (trangThai == "Chưa nhập" && tempImages.ContainsKey(maTheNhap))
                {
                    try
                    {
                        byte[] imageData = tempImages[maTheNhap];
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            picUploadAnh.Image = Image.FromStream(ms);
                            picUploadAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi hiển thị ảnh tạm thời: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        picUploadAnh.Image = null;
                    }
                }
                else
                {
                    string query = "SELECT AnhBia FROM SACH WHERE MaSach = @MaSach";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MaSach", SqlDbType.VarChar) { Value = maSach }
                    };
                    DataTable dt = db.ExecuteQuery(query, parameters);
                    if (dt.Rows.Count > 0 && dt.Rows[0]["AnhBia"] != DBNull.Value)
                    {
                        try
                        {
                            byte[] imageData = (byte[])dt.Rows[0]["AnhBia"];
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                picUploadAnh.Image = Image.FromStream(ms);
                                picUploadAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi hiển thị ảnh từ cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            picUploadAnh.Image = null;
                        }
                    }
                    else
                    {
                        picUploadAnh.Image = null;
                    }
                }
            }
        }

        
        private void picClean_Click(object sender, EventArgs e)
        {
            ClearInput();

        }

        
    }
}
