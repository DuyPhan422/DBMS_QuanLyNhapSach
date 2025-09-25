namespace _23110194_PhanNgocDuy_QuanLyNhapSach
{
    partial class KhoSachForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KhoSachForm));
            this.dgvKhoSach = new System.Windows.Forms.DataGridView();
            this.label11 = new System.Windows.Forms.Label();
            this.grThongTinKho = new Guna.UI2.WinForms.Guna2GroupBox();
            this.lbTenSach = new System.Windows.Forms.Label();
            this.picClean = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lbMaSach = new System.Windows.Forms.Label();
            this.cbxMaSach = new System.Windows.Forms.ComboBox();
            this.lbSoLuongMoi = new System.Windows.Forms.Label();
            this.btnCapNhatKho = new System.Windows.Forms.Button();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.btnKiemTraKho = new System.Windows.Forms.Button();
            this.lbBangNhapSach = new System.Windows.Forms.Label();
            this.txtTenSach = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSoLuongHienTai = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhoSach)).BeginInit();
            this.grThongTinKho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClean)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvKhoSach
            // 
            this.dgvKhoSach.AllowUserToAddRows = false;
            this.dgvKhoSach.AllowUserToDeleteRows = false;
            this.dgvKhoSach.AllowUserToResizeColumns = false;
            this.dgvKhoSach.AllowUserToResizeRows = false;
            this.dgvKhoSach.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvKhoSach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKhoSach.Location = new System.Drawing.Point(46, 477);
            this.dgvKhoSach.Name = "dgvKhoSach";
            this.dgvKhoSach.RowHeadersVisible = false;
            this.dgvKhoSach.RowHeadersWidth = 51;
            this.dgvKhoSach.RowTemplate.Height = 24;
            this.dgvKhoSach.Size = new System.Drawing.Size(1575, 277);
            this.dgvKhoSach.TabIndex = 0;
            this.dgvKhoSach.SelectionChanged += new System.EventHandler(this.dgvKhoSach_SelectionChanged);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Ivory;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(0, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(1681, 60);
            this.label11.TabIndex = 78;
            this.label11.Text = "QUẢN LÝ KHO SÁCH UTE";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grThongTinKho
            // 
            this.grThongTinKho.Controls.Add(this.txtSoLuongHienTai);
            this.grThongTinKho.Controls.Add(this.label1);
            this.grThongTinKho.Controls.Add(this.txtTenSach);
            this.grThongTinKho.Controls.Add(this.lbTenSach);
            this.grThongTinKho.Controls.Add(this.picClean);
            this.grThongTinKho.Controls.Add(this.lbMaSach);
            this.grThongTinKho.Controls.Add(this.cbxMaSach);
            this.grThongTinKho.Controls.Add(this.lbSoLuongMoi);
            this.grThongTinKho.Controls.Add(this.btnCapNhatKho);
            this.grThongTinKho.Controls.Add(this.txtSoLuong);
            this.grThongTinKho.Controls.Add(this.btnKiemTraKho);
            this.grThongTinKho.CustomBorderColor = System.Drawing.Color.Bisque;
            this.grThongTinKho.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grThongTinKho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.grThongTinKho.Location = new System.Drawing.Point(46, 104);
            this.grThongTinKho.Name = "grThongTinKho";
            this.grThongTinKho.Size = new System.Drawing.Size(622, 341);
            this.grThongTinKho.TabIndex = 79;
            this.grThongTinKho.Text = "Thông tin kho";
            // 
            // lbTenSach
            // 
            this.lbTenSach.AutoSize = true;
            this.lbTenSach.BackColor = System.Drawing.Color.Transparent;
            this.lbTenSach.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTenSach.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbTenSach.Location = new System.Drawing.Point(17, 96);
            this.lbTenSach.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTenSach.Name = "lbTenSach";
            this.lbTenSach.Size = new System.Drawing.Size(101, 26);
            this.lbTenSach.TabIndex = 87;
            this.lbTenSach.Text = "Tên sách:";
            // 
            // picClean
            // 
            this.picClean.Image = ((System.Drawing.Image)(resources.GetObject("picClean.Image")));
            this.picClean.ImageRotate = 0F;
            this.picClean.Location = new System.Drawing.Point(577, 0);
            this.picClean.Name = "picClean";
            this.picClean.Size = new System.Drawing.Size(45, 39);
            this.picClean.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picClean.TabIndex = 86;
            this.picClean.TabStop = false;
            this.picClean.Click += new System.EventHandler(this.picClean_Click);
            // 
            // lbMaSach
            // 
            this.lbMaSach.AutoSize = true;
            this.lbMaSach.BackColor = System.Drawing.Color.Transparent;
            this.lbMaSach.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMaSach.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbMaSach.Location = new System.Drawing.Point(17, 54);
            this.lbMaSach.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbMaSach.Name = "lbMaSach";
            this.lbMaSach.Size = new System.Drawing.Size(94, 26);
            this.lbMaSach.TabIndex = 68;
            this.lbMaSach.Text = "Mã sách:";
            // 
            // cbxMaSach
            // 
            this.cbxMaSach.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMaSach.FormattingEnabled = true;
            this.cbxMaSach.Location = new System.Drawing.Point(246, 46);
            this.cbxMaSach.Name = "cbxMaSach";
            this.cbxMaSach.Size = new System.Drawing.Size(238, 34);
            this.cbxMaSach.TabIndex = 69;
            this.cbxMaSach.SelectedIndexChanged += new System.EventHandler(this.cbxMaSach_SelectedIndexChanged);
            // 
            // lbSoLuongMoi
            // 
            this.lbSoLuongMoi.AutoSize = true;
            this.lbSoLuongMoi.BackColor = System.Drawing.Color.Transparent;
            this.lbSoLuongMoi.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSoLuongMoi.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbSoLuongMoi.Location = new System.Drawing.Point(17, 189);
            this.lbSoLuongMoi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSoLuongMoi.Name = "lbSoLuongMoi";
            this.lbSoLuongMoi.Size = new System.Drawing.Size(196, 26);
            this.lbSoLuongMoi.TabIndex = 76;
            this.lbSoLuongMoi.Text = "Số lượng thêm/ bớt:";
            // 
            // btnCapNhatKho
            // 
            this.btnCapNhatKho.Image = ((System.Drawing.Image)(resources.GetObject("btnCapNhatKho.Image")));
            this.btnCapNhatKho.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCapNhatKho.Location = new System.Drawing.Point(318, 255);
            this.btnCapNhatKho.Name = "btnCapNhatKho";
            this.btnCapNhatKho.Size = new System.Drawing.Size(219, 62);
            this.btnCapNhatKho.TabIndex = 5;
            this.btnCapNhatKho.Text = "Cập nhật kho";
            this.btnCapNhatKho.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCapNhatKho.UseVisualStyleBackColor = true;
            this.btnCapNhatKho.Click += new System.EventHandler(this.btnCapNhatKho_Click);
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(246, 189);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(238, 34);
            this.txtSoLuong.TabIndex = 4;
            this.txtSoLuong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSoLuong_KeyPress);
            // 
            // btnKiemTraKho
            // 
            this.btnKiemTraKho.Image = ((System.Drawing.Image)(resources.GetObject("btnKiemTraKho.Image")));
            this.btnKiemTraKho.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnKiemTraKho.Location = new System.Drawing.Point(33, 255);
            this.btnKiemTraKho.Name = "btnKiemTraKho";
            this.btnKiemTraKho.Size = new System.Drawing.Size(217, 62);
            this.btnKiemTraKho.TabIndex = 6;
            this.btnKiemTraKho.Text = "Kiểm tra kho";
            this.btnKiemTraKho.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnKiemTraKho.UseVisualStyleBackColor = true;
            this.btnKiemTraKho.Click += new System.EventHandler(this.btnKiemTraKho_Click);
            // 
            // lbBangNhapSach
            // 
            this.lbBangNhapSach.AutoSize = true;
            this.lbBangNhapSach.BackColor = System.Drawing.Color.Transparent;
            this.lbBangNhapSach.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBangNhapSach.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbBangNhapSach.Location = new System.Drawing.Point(41, 448);
            this.lbBangNhapSach.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBangNhapSach.Name = "lbBangNhapSach";
            this.lbBangNhapSach.Size = new System.Drawing.Size(154, 26);
            this.lbBangNhapSach.TabIndex = 80;
            this.lbBangNhapSach.Text = "Bảng kho sách:";
            // 
            // txtTenSach
            // 
            this.txtTenSach.Location = new System.Drawing.Point(246, 96);
            this.txtTenSach.Name = "txtTenSach";
            this.txtTenSach.ReadOnly = true;
            this.txtTenSach.Size = new System.Drawing.Size(238, 34);
            this.txtTenSach.TabIndex = 88;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(17, 142);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 26);
            this.label1.TabIndex = 89;
            this.label1.Text = "Số lượng hiện tại:";
            // 
            // txtSoLuongHienTai
            // 
            this.txtSoLuongHienTai.Location = new System.Drawing.Point(246, 142);
            this.txtSoLuongHienTai.Name = "txtSoLuongHienTai";
            this.txtSoLuongHienTai.ReadOnly = true;
            this.txtSoLuongHienTai.Size = new System.Drawing.Size(238, 34);
            this.txtSoLuongHienTai.TabIndex = 90;
            // 
            // KhoSachForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1681, 729);
            this.Controls.Add(this.lbBangNhapSach);
            this.Controls.Add(this.grThongTinKho);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dgvKhoSach);
            this.Name = "KhoSachForm";
            this.Text = "KhoSachForm";
            this.Load += new System.EventHandler(this.KhoSachForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhoSach)).EndInit();
            this.grThongTinKho.ResumeLayout(false);
            this.grThongTinKho.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClean)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvKhoSach;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2GroupBox grThongTinKho;
        private System.Windows.Forms.Label lbMaSach;
        private System.Windows.Forms.ComboBox cbxMaSach;
        private System.Windows.Forms.Label lbSoLuongMoi;
        private System.Windows.Forms.Button btnCapNhatKho;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Button btnKiemTraKho;
        private Guna.UI2.WinForms.Guna2PictureBox picClean;
        private System.Windows.Forms.Label lbBangNhapSach;
        private System.Windows.Forms.Label lbTenSach;
        private System.Windows.Forms.TextBox txtTenSach;
        private System.Windows.Forms.TextBox txtSoLuongHienTai;
        private System.Windows.Forms.Label label1;
    }
}