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
            this.btnTraCuu = new System.Windows.Forms.Button();
            this.dgvTraCuu = new System.Windows.Forms.DataGridView();
            this.cbbTraCuu = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraCuu)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTraCuu
            // 
            this.btnTraCuu.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTraCuu.Location = new System.Drawing.Point(30, 95);
            this.btnTraCuu.Name = "btnTraCuu";
            this.btnTraCuu.Size = new System.Drawing.Size(164, 50);
            this.btnTraCuu.TabIndex = 39;
            this.btnTraCuu.Text = "Tra Cứu:";
            this.btnTraCuu.UseVisualStyleBackColor = true;
            this.btnTraCuu.Click += new System.EventHandler(this.btnTraCuu_Click);
            // 
            // dgvTraCuu
            // 
            this.dgvTraCuu.AllowUserToResizeColumns = false;
            this.dgvTraCuu.AllowUserToResizeRows = false;
            this.dgvTraCuu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTraCuu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTraCuu.Location = new System.Drawing.Point(30, 180);
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
            this.cbbTraCuu.Location = new System.Drawing.Point(257, 106);
            this.cbbTraCuu.Name = "cbbTraCuu";
            this.cbbTraCuu.Size = new System.Drawing.Size(287, 31);
            this.cbbTraCuu.TabIndex = 50;
            // 
            // TraCuuSachForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1485, 724);
            this.Controls.Add(this.cbbTraCuu);
            this.Controls.Add(this.dgvTraCuu);
            this.Controls.Add(this.btnTraCuu);
            this.Name = "TraCuuSachForm";
            this.Text = "TraCuuSachForm";
            this.Load += new System.EventHandler(this.TraCuuSachForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTraCuu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnTraCuu;
        private System.Windows.Forms.DataGridView dgvTraCuu;
        private System.Windows.Forms.ComboBox cbbTraCuu;
    }
}