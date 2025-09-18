namespace _23110194_PhanNgocDuy_QuanLyNhapSach
{
    partial class TraCuuSachForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TraCuuSachForm));
            this.btnTraCuu = new System.Windows.Forms.Button();
            this.dgvTraCuu = new System.Windows.Forms.DataGridView();
            this.cbbTraCuu = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lbBangNhapSach = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraCuu)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTraCuu
            // 
            this.btnTraCuu.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTraCuu.Image = ((System.Drawing.Image)(resources.GetObject("btnTraCuu.Image")));
            this.btnTraCuu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTraCuu.Location = new System.Drawing.Point(30, 95);
            this.btnTraCuu.Name = "btnTraCuu";
            this.btnTraCuu.Size = new System.Drawing.Size(149, 50);
            this.btnTraCuu.TabIndex = 39;
            this.btnTraCuu.Text = "Tra cứu:";
            this.btnTraCuu.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTraCuu.UseVisualStyleBackColor = true;
            this.btnTraCuu.Click += new System.EventHandler(this.btnTraCuu_Click);
            // 
            // dgvTraCuu
            // 
            this.dgvTraCuu.AllowUserToResizeColumns = false;
            this.dgvTraCuu.AllowUserToResizeRows = false;
            this.dgvTraCuu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTraCuu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTraCuu.Location = new System.Drawing.Point(30, 227);
            this.dgvTraCuu.Name = "dgvTraCuu";
            this.dgvTraCuu.RowHeadersVisible = false;
            this.dgvTraCuu.RowHeadersWidth = 51;
            this.dgvTraCuu.RowTemplate.Height = 24;
            this.dgvTraCuu.Size = new System.Drawing.Size(1461, 384);
            this.dgvTraCuu.TabIndex = 49;
            // 
            // cbbTraCuu
            // 
            this.cbbTraCuu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTraCuu.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbTraCuu.FormattingEnabled = true;
            this.cbbTraCuu.Location = new System.Drawing.Point(216, 106);
            this.cbbTraCuu.Name = "cbbTraCuu";
            this.cbbTraCuu.Size = new System.Drawing.Size(287, 31);
            this.cbbTraCuu.TabIndex = 50;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Ivory;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(0, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(1485, 61);
            this.label11.TabIndex = 87;
            this.label11.Text = "QUẢN LÝ TRA CỨU SÁCH UTE";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBangNhapSach
            // 
            this.lbBangNhapSach.AutoSize = true;
            this.lbBangNhapSach.BackColor = System.Drawing.Color.Transparent;
            this.lbBangNhapSach.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBangNhapSach.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbBangNhapSach.Location = new System.Drawing.Point(25, 179);
            this.lbBangNhapSach.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBangNhapSach.Name = "lbBangNhapSach";
            this.lbBangNhapSach.Size = new System.Drawing.Size(135, 26);
            this.lbBangNhapSach.TabIndex = 88;
            this.lbBangNhapSach.Text = "Bảng tra cứu:";
            // 
            // TraCuuSachForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1485, 724);
            this.Controls.Add(this.lbBangNhapSach);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cbbTraCuu);
            this.Controls.Add(this.dgvTraCuu);
            this.Controls.Add(this.btnTraCuu);
            this.Name = "TraCuuSachForm";
            this.Text = "TraCuuSachForm";
            this.Load += new System.EventHandler(this.TraCuuSachForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraCuu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnTraCuu;
        private System.Windows.Forms.DataGridView dgvTraCuu;
        private System.Windows.Forms.ComboBox cbbTraCuu;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbBangNhapSach;
    }
}