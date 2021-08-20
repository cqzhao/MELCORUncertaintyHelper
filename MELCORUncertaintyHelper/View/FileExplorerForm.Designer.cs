namespace MELCORUncertaintyHelper.View
{
    partial class FileExplorerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileExplorerForm));
            this.tvwFiles = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tvwFiles
            // 
            this.tvwFiles.AllowDrop = true;
            this.tvwFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwFiles.Font = new System.Drawing.Font("Verdana", 12F);
            this.tvwFiles.Location = new System.Drawing.Point(0, 0);
            this.tvwFiles.Name = "tvwFiles";
            this.tvwFiles.Size = new System.Drawing.Size(914, 525);
            this.tvwFiles.TabIndex = 0;
            this.tvwFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.TvwFiles_DragDrop);
            this.tvwFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.TvwFiles_DragEnter);
            // 
            // FileExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 525);
            this.Controls.Add(this.tvwFiles);
            this.Font = new System.Drawing.Font("Verdana", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileExplorerForm";
            this.TabText = "File Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileExplorerForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvwFiles;
    }
}