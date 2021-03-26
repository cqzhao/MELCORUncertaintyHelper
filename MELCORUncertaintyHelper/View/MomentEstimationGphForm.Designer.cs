namespace MELCORUncertaintyHelper.View
{
    partial class MomentEstimationGphForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MomentEstimationGphForm));
            this.gphResults = new OxyPlot.WindowsForms.PlotView();
            this.SuspendLayout();
            // 
            // gphResults
            // 
            this.gphResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gphResults.Location = new System.Drawing.Point(0, 0);
            this.gphResults.Name = "gphResults";
            this.gphResults.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.gphResults.Size = new System.Drawing.Size(800, 450);
            this.gphResults.TabIndex = 3;
            this.gphResults.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.gphResults.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.gphResults.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // ResultWithDistributionGphForm3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gphResults);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ResultWithDistributionGphForm3";
            this.ResumeLayout(false);

        }

        #endregion

        private OxyPlot.WindowsForms.PlotView gphResults;
    }
}