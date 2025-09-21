
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
using System.Windows.Forms.DataVisualization.Charting;


namespace _23110194_PhanNgocDuy_QuanLyNhapSach
{
    public partial class ThongKeNhapSachForm : Form
    {
        private readonly DBConnect dbConnect;

        public ThongKeNhapSachForm()
        {
            InitializeComponent();
            dbConnect = new DBConnect();

        }

        private void ThongKeNhapSachForm_Load(object sender, EventArgs e)
        {
            cbbThongKe.Items.AddRange(new object[] { "Thống kê sách đã nhập", "Thống kê ngày đã nhập" });
            cbbThongKe.SelectedIndex = -1; // Không chọn mặc định

            // Khởi tạo biểu đồ
            InitializeChart();


        }
        private void InitializeChart()
        {
            // Xóa các Series và ChartAreas hiện có để tránh xung đột
            chartThongKe.Series.Clear();
            chartThongKe.ChartAreas.Clear();

            // Thêm ChartArea (vùng hiển thị biểu đồ)
            ChartArea mainArea = new ChartArea("MainArea")
            {
                AxisX = {
                    LabelStyle = {
                        Font = new Font("Arial", 7), // Phông chữ nhỏ để hiển thị nhiều nhãn
                        Enabled = true // Bật nhãn
                    },
                    MajorGrid = { Enabled = false } // Tắt lưới trục X để rõ ràng hơn
                },
                AxisY = { TitleFont = new Font("Arial", 10) }
            };
            chartThongKe.ChartAreas.Add(mainArea);

            // Thêm Series (chuỗi dữ liệu) để hiển thị dữ liệu thống kê
            Series dataSeries = new Series("DataSeries")
            {
                ChartType = SeriesChartType.Column // Mặc định là biểu đồ cột
            };
            chartThongKe.Series.Add(dataSeries);
        }
        // Phương thức lấy thống kê sách đã nhập
        public bool GetThongKeSachDaNhap(DateTime tuNgay, DateTime denNgay, out DataTable dt)
        {
            dt = new DataTable();
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@TuNgay", SqlDbType.Date) { Value = tuNgay },
                new SqlParameter("@DenNgay", SqlDbType.Date) { Value = denNgay }
            };

            try
            {
                dt = dbConnect.ExecuteQuery("EXEC sp_ThongKeSachDaNhap @TuNgay, @DenNgay", parameters);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu thống kê: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        // Phương thức lấy thống kê nhập sách theo ngày
        public bool GetThongKeNhapSachTheoNgay(DateTime tuNgay, DateTime denNgay, out DataTable dt)
        {
            dt = new DataTable();
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@TuNgay", SqlDbType.Date) { Value = tuNgay },
                new SqlParameter("@DenNgay", SqlDbType.Date) { Value = denNgay }
            };

            try
            {
                dt = dbConnect.ExecuteQuery("EXEC sp_ThongKeNhapSachTheoNgay @TuNgay, @DenNgay", parameters);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu thống kê: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        // Phương thức làm sạch dữ liệu
        public bool ClearForm()
        {
            try
            {
                LoadChartData();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void cbbThongKe_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChartData();
        }
        private void LoadChartData()
        {
            try
            {
                // Kiểm tra nếu chưa chọn loại thống kê
                if (cbbThongKe.SelectedIndex == -1)
                {
                    chartThongKe.Series["DataSeries"].Points.Clear();
                    chartThongKe.Titles.Clear();
                    chartThongKe.Titles.Add(new Title
                    {
                        Text = "Vui lòng chọn loại thống kê",
                        Font = new Font("Arial", 12, FontStyle.Bold)
                    });
                    return;
                }

                // Kiểm tra ngày hợp lệ
                if (dtpTuNgay.Value.Date > dtpDenNgay.Value.Date)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xóa dữ liệu cũ trong chuỗi DataSeries
                chartThongKe.Series["DataSeries"].Points.Clear();

                DataTable dt;
                bool success;

                if (cbbThongKe.SelectedIndex == 0) // Thống kê theo sách
                {
                    chartThongKe.Series["DataSeries"].ChartType = SeriesChartType.Column; // Biểu đồ cột
                    chartThongKe.Series["DataSeries"].XValueMember = "TenSach"; // Trục X: Tên sách
                    chartThongKe.Series["DataSeries"].YValueMembers = "TongSoLuongNhap"; // Trục Y: Số lượng nhập
                    success = GetThongKeSachDaNhap(dtpTuNgay.Value.Date, dtpDenNgay.Value.Date, out dt);
                }
                else // Thống kê theo ngày
                {
                    chartThongKe.Series["DataSeries"].ChartType = SeriesChartType.Line; // Biểu đồ đường
                    success = GetThongKeNhapSachTheoNgay(dtpTuNgay.Value.Date, dtpDenNgay.Value.Date, out dt);
                }

                if (!success)
                {
                    return;
                }

                // Kiểm tra nếu dữ liệu rỗng
                if (dt.Rows.Count == 0)
                {
                    chartThongKe.Titles.Clear();
                    chartThongKe.Titles.Add(new Title
                    {
                        Text = "Không có dữ liệu thống kê trong khoảng thời gian đã chọn",
                        Font = new Font("Arial", 12, FontStyle.Bold)
                    });
                    return;
                }

                // Định dạng trục X
                if (cbbThongKe.SelectedIndex == 0) // Thống kê theo sách
                {
                    chartThongKe.ChartAreas["MainArea"].AxisX.LabelStyle.Angle = -45; // Xoay nhãn 45 độ
                    chartThongKe.ChartAreas["MainArea"].AxisX.Interval = 1; // Hiển thị tất cả nhãn
                    chartThongKe.ChartAreas["MainArea"].AxisX.LabelStyle.Enabled = true; // Bật nhãn
                    chartThongKe.ChartAreas["MainArea"].AxisX.LabelStyle.TruncatedLabels = false; // Tắt cắt ngắn nhãn
                    chartThongKe.ChartAreas["MainArea"].AxisX.MajorGrid.Enabled = false; // Tắt lưới trục X
                }
                else // Thống kê theo ngày
                {
                    chartThongKe.ChartAreas["MainArea"].AxisX.LabelStyle.Format = "dd/MM/yyyy"; // Định dạng ngày
                    chartThongKe.ChartAreas["MainArea"].AxisX.LabelStyle.Angle = -45; // Xoay nhãn 45 độ
                    chartThongKe.ChartAreas["MainArea"].AxisX.IntervalType = DateTimeIntervalType.Days; // Khoảng cách theo ngày
                    chartThongKe.ChartAreas["MainArea"].AxisX.Interval = 1; // Hiển thị nhãn mỗi ngày
                }

                // Gán dữ liệu thủ công cho biểu đồ
                chartThongKe.Series["DataSeries"].Points.Clear();
                if (cbbThongKe.SelectedIndex == 0) // Thống kê theo sách
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string tenSach = row["TenSach"].ToString();
                        double tongSoLuong = Convert.ToDouble(row["TongSoLuongNhap"]);
                        string maSach = row["MaSach"].ToString();
                        DataPoint point = new DataPoint();
                        point.SetValueXY(tenSach, tongSoLuong);
                        point.SetCustomProperty("MaSach", maSach);
                        point.ToolTip = "Mã sách: #CUSTOMPROPERTY(MaSach)\nTên sách: #AXISLABEL\nSố lượng: #VALY sách";
                        chartThongKe.Series["DataSeries"].Points.Add(point);
                    }
                }
                else // Thống kê theo ngày
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime ngayNhap = Convert.ToDateTime(row["NgayNhap"]);
                        double tongSoLuong = Convert.ToDouble(row["TongSoLuongNhap"]);
                        DataPoint point = new DataPoint();
                        point.SetValueXY(ngayNhap, tongSoLuong);
                        point.AxisLabel = ngayNhap.ToString("dd/MM/yyyy");
                        point.ToolTip = "Ngày nhập: #AXISLABEL\nSố lượng: #VALY sách";
                        chartThongKe.Series["DataSeries"].Points.Add(point);
                    }
                }

                // Thiết lập tiêu đề và nhãn
                chartThongKe.Titles.Clear();
                chartThongKe.Titles.Add(new Title
                {
                    Text = cbbThongKe.SelectedIndex == 0 ? "Thống kê số lượng sách nhập" : "Thống kê nhập sách theo ngày",
                    Font = new Font("Arial", 12, FontStyle.Bold)
                });
                chartThongKe.ChartAreas["MainArea"].AxisX.Title = cbbThongKe.SelectedIndex == 0 ? "Tên sách" : "Ngày nhập";
                chartThongKe.ChartAreas["MainArea"].AxisY.Title = "Số lượng nhập";

                // Tùy chỉnh giao diện biểu đồ
                chartThongKe.Series["DataSeries"].Color = Color.DodgerBlue; // Màu xanh dương
                chartThongKe.Series["DataSeries"].BorderWidth = 2; // Tăng độ dày đường viền
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Lỗi SQL khi tải dữ liệu thống kê: {ex.Message} (Mã lỗi: {ex.Number})", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu thống kê: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

