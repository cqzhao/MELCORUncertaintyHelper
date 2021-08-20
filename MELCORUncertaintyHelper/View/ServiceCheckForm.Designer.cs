namespace MELCORUncertaintyHelper.View
{
    partial class ServiceCheckForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceCheckForm));
            this.pnlMain = new MetroFramework.Controls.MetroPanel();
            this.chkStatistics = new System.Windows.Forms.CheckBox();
            this.chkInterpolation = new System.Windows.Forms.CheckBox();
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.btnOk = new MetroFramework.Controls.MetroButton();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.chkStatistics);
            this.pnlMain.Controls.Add(this.chkInterpolation);
            this.pnlMain.Controls.Add(this.btnCancel);
            this.pnlMain.Controls.Add(this.btnOk);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.HorizontalScrollbarBarColor = true;
            this.pnlMain.HorizontalScrollbarHighlightOnWheel = false;
            this.pnlMain.HorizontalScrollbarSize = 12;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(439, 188);
            this.pnlMain.TabIndex = 0;
            this.pnlMain.VerticalScrollbarBarColor = true;
            this.pnlMain.VerticalScrollbarHighlightOnWheel = false;
            this.pnlMain.VerticalScrollbarSize = 11;
            // 
            // chkStatistics
            // 
            this.chkStatistics.AutoSize = true;
            this.chkStatistics.BackColor = System.Drawing.Color.White;
            this.chkStatistics.Font = new System.Drawing.Font("Verdana", 10F);
            this.chkStatistics.ForeColor = System.Drawing.Color.Black;
            this.chkStatistics.Location = new System.Drawing.Point(43, 70);
            this.chkStatistics.Name = "chkStatistics";
            this.chkStatistics.Size = new System.Drawing.Size(92, 21);
            this.chkStatistics.TabIndex = 4;
            this.chkStatistics.Text = "Statistics";
            this.chkStatistics.UseVisualStyleBackColor = false;
            this.chkStatistics.CheckedChanged += new System.EventHandler(this.ChkStatistics_CheckedChanged);
            // 
            // chkInterpolation
            // 
            this.chkInterpolation.AutoSize = true;
            this.chkInterpolation.BackColor = System.Drawing.Color.White;
            this.chkInterpolation.Font = new System.Drawing.Font("Verdana", 10F);
            this.chkInterpolation.ForeColor = System.Drawing.Color.Black;
            this.chkInterpolation.Location = new System.Drawing.Point(43, 20);
            this.chkInterpolation.Name = "chkInterpolation";
            this.chkInterpolation.Size = new System.Drawing.Size(117, 21);
            this.chkInterpolation.TabIndex = 1;
            this.chkInterpolation.Text = "Interpolation";
            this.chkInterpolation.UseVisualStyleBackColor = false;
            this.chkInterpolation.CheckedChanged += new System.EventHandler(this.ChkInterpolation_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.btnCancel.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnCancel.Location = new System.Drawing.Point(271, 115);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 58);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.btnOk.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.btnOk.Location = new System.Drawing.Point(70, 115);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(109, 58);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseSelectable = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // ServiceCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(439, 188);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Verdana", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServiceCheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Mode";
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel pnlMain;
        private MetroFramework.Controls.MetroButton btnCancel;
        private MetroFramework.Controls.MetroButton btnOk;
        private System.Windows.Forms.CheckBox chkStatistics;
        private System.Windows.Forms.CheckBox chkInterpolation;
    }
}