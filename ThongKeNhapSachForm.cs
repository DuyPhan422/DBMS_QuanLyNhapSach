using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TheArtOfDevHtmlRenderer.Adapters;

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
            // DataSeries là tập hợp dữ liệu được vẽ trên biểu đồ, với trục X (TenSach hoặc NgayNhap) và trục Y (TongSoLuongNhap)
            Series dataSeries = new Series("DataSeries")
            {
                ChartType = SeriesChartType.Column // Mặc định là biểu đồ cột
            };
            chartThongKe.Series.Add(dataSeries);
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadChartData();
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

                string query;
                SqlParameter[] parameters;

                if (cbbThongKe.SelectedIndex == 0) // Thống kê theo sách (sử dụng sp_ThongKeSachDaNhap)
                {
                    query = "sp_ThongKeSachDaNhap";
                    chartThongKe.Series["DataSeries"].ChartType = SeriesChartType.Column; // Biểu đồ cột
                    chartThongKe.Series["DataSeries"].XValueMember = "TenSach"; // Trục X: Tên sách
                    chartThongKe.Series["DataSeries"].YValueMembers = "TongSoLuongNhap"; // Trục Y: Số lượng nhập
                }
                else // Thống kê theo ngày (sử dụng sp_ThongKeNhapSachTheoNgay)
                {
                    query = "sp_ThongKeNhapSachTheoNgay";
                    chartThongKe.Series["DataSeries"].ChartType = SeriesChartType.Line; // Biểu đồ đường
                    chartThongKe.Series["DataSeries"].XValueMember = "NgayNhap"; // Trục X: Ngày nhập
                    chartThongKe.Series["DataSeries"].YValueMembers = "TongSoLuongNhap"; // Trục Y: Số lượng nhập
                }

                // Thiết lập tham số cho stored procedure
                parameters = new[]
                {
                    new SqlParameter("@TuNgay", dtpTuNgay.Value.Date) { SqlDbType = SqlDbType.Date },
                    new SqlParameter("@DenNgay", dtpDenNgay.Value.Date) { SqlDbType = SqlDbType.Date }
                };

                // Gọi stored procedure và lấy dữ liệu
                DataTable dt = new DataTable();
                dbConnect.OpenConnection();
                using (SqlCommand cmd = new SqlCommand(query, dbConnect.GetConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                dbConnect.CloseConnection();

                // Debug: Kiểm tra dữ liệu trả về (bỏ bình luận để sử dụng)
                //MessageBox.Show($"Số dòng: {dt.Rows.Count}, Cột: {string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");
                //foreach (DataRow row in dt.Rows)
                //{
                //    MessageBox.Show(cbbThongKe.SelectedIndex == 0
                //        ? $"TenSach: {row["TenSach"]}, TongSoLuongNhap: {row["TongSoLuongNhap"]}"
                //        : $"NgayNhap: {row["NgayNhap"]}, TongSoLuongNhap: {row["TongSoLuongNhap"]}");
                //}

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

                // Gán dữ liệu cho biểu đồ
                chartThongKe.DataSource = dt;
                chartThongKe.DataBind();

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
                chartThongKe.Series["DataSeries"].ToolTip = "#VALX: #VALY sách"; // Tooltip khi di chuột
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
