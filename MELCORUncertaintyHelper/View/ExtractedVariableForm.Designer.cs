namespace MELCORUncertaintyHelper.View
{
    partial class ExtractedVariableForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractedVariableForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsBtnCheckAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnUncheckAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnSave = new System.Windows.Forms.ToolStripButton();
            this.dgvVariables = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnCheckAll,
            this.toolStripSeparator1,
            this.tsBtnUncheckAll,
            this.toolStripSeparator2,
            this.tsBtnSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsBtnCheckAll
            // 
            this.tsBtnCheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnCheckAll.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsBtnCheckAll.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnCheckAll.Image")));
            this.tsBtnCheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnCheckAll.Name = "tsBtnCheckAll";
            this.tsBtnCheckAll.Size = new System.Drawing.Size(64, 22);
            this.tsBtnCheckAll.Text = "Check All";
            this.tsBtnCheckAll.Click += new System.EventHandler(this.TsBtnCheckAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnUncheckAll
            // 
            this.tsBtnUncheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnUncheckAll.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.tsBtnUncheckAll.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnUncheckAll.Image")));
            this.tsBtnUncheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnUncheckAll.Name = "tsBtnUncheckAll";
            this.tsBtnUncheckAll.Size = new System.Drawing.Size(80, 22);
            this.tsBtnUncheckAll.Text = "UnCheck All";
            this.tsBtnUncheckAll.Click += new System.EventHandler(this.TsBtnUncheckAll_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnSave
            // 
            this.tsBtnSave.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.tsBtnSave.ForeColor = System.Drawing.Color.Black;
            this.tsBtnSave.Image = global::MELCORUncertaintyHelper.Properties.Resources.file_csv;
            this.tsBtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnSave.Name = "tsBtnSave";
            this.tsBtnSave.Size = new System.Drawing.Size(55, 22);
            this.tsBtnSave.Text = "Save";
            this.tsBtnSave.Click += new System.EventHandler(this.TsBtnSave_Click);
            // 
            // dgvVariables
            // 
            this.dgvVariables.AllowUserToAddRows = false;
            this.dgvVariables.AllowUserToDeleteRows = false;
            this.dgvVariables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVariables.BackgroundColor = System.Drawing.Color.White;
            this.dgvVariables.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvVariables.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvVariables.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVariables.GridColor = System.Drawing.Color.Black;
            this.dgvVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvVariables.Name = "dgvVariables";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvVariables.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dgvVariables.RowHeadersVisible = false;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dgvVariables.RowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvVariables.RowTemplate.Height = 23;
            this.dgvVariables.Size = new System.Drawing.Size(800, 425);
            this.dgvVariables.TabIndex = 2;
            this.dgvVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DdgvVariables_CellDoubleClick);
            // 
            // ExtractedVariableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvVariables);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ExtractedVariableForm";
            this.TabText = "Extracted Variable";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExtractedVariableForm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataGridView dgvVariables;
        private System.Windows.Forms.ToolStripButton tsBtnCheckAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsBtnUncheckAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsBtnSave;
    }
}