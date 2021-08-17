namespace MELCORUncertaintyHelper.View.ResultView
{
    partial class LogNormalDistributionGphForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogNormalDistributionGphForm));
            this.tsrMenu = new System.Windows.Forms.ToolStrip();
            this.gphResults = new OxyPlot.WindowsForms.PlotView();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.tsrMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsrMenu
            // 
            this.tsrMenu.Font = new System.Drawing.Font("Verdana", 9F);
            this.tsrMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSave});
            this.tsrMenu.Location = new System.Drawing.Point(0, 0);
            this.tsrMenu.Name = "tsrMenu";
            this.tsrMenu.Size = new System.Drawing.Size(914, 25);
            this.tsrMenu.TabIndex = 0;
            this.tsrMenu.Text = "toolStrip1";
            // 
            // gphResults
            // 
            this.gphResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gphResults.Location = new System.Drawing.Point(0, 25);
            this.gphResults.Name = "gphResults";
            this.gphResults.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.gphResults.Size = new System.Drawing.Size(914, 500);
            this.gphResults.TabIndex = 1;
            this.gphResults.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.gphResults.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.gphResults.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.Image = global::MELCORUncertaintyHelper.Properties.Resources.save_image;
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(100, 22);
            this.tsbtnSave.Text = "Save Graph";
            this.tsbtnSave.Click += new System.EventHandler(this.TsbtnSave_Click);
            // 
            // LogNormalDistributionGphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 525);
            this.Controls.Add(this.gphResults);
            this.Controls.Add(this.tsrMenu);
            this.Font = new System.Drawing.Font("Verdana", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LogNormalDistributionGphForm";
            this.tsrMenu.ResumeLayout(false);
            this.tsrMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsrMenu;
        private OxyPlot.WindowsForms.PlotView gphResults;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
    }
}