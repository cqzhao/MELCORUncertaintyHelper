namespace MELCORUncertaintyHelper.View
{
    partial class GphAxisControlForm
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
            this.txtAxisXTitle = new MaterialSkin.Controls.MaterialTextBox();
            this.txtAxisYTitle = new MaterialSkin.Controls.MaterialTextBox();
            this.btnCancel = new MaterialSkin.Controls.MaterialButton();
            this.btnOK = new MaterialSkin.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // txtAxisXTitle
            // 
            this.txtAxisXTitle.AnimateReadOnly = false;
            this.txtAxisXTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAxisXTitle.Depth = 0;
            this.txtAxisXTitle.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAxisXTitle.Hint = "X Axis Title";
            this.txtAxisXTitle.LeadingIcon = null;
            this.txtAxisXTitle.Location = new System.Drawing.Point(6, 83);
            this.txtAxisXTitle.MaxLength = 50;
            this.txtAxisXTitle.MouseState = MaterialSkin.MouseState.OUT;
            this.txtAxisXTitle.Multiline = false;
            this.txtAxisXTitle.Name = "txtAxisXTitle";
            this.txtAxisXTitle.Size = new System.Drawing.Size(488, 50);
            this.txtAxisXTitle.TabIndex = 4;
            this.txtAxisXTitle.Text = "";
            this.txtAxisXTitle.TrailingIcon = null;
            // 
            // txtAxisYTitle
            // 
            this.txtAxisYTitle.AnimateReadOnly = false;
            this.txtAxisYTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAxisYTitle.Depth = 0;
            this.txtAxisYTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAxisYTitle.Hint = "Y Axis Title";
            this.txtAxisYTitle.LeadingIcon = null;
            this.txtAxisYTitle.Location = new System.Drawing.Point(6, 158);
            this.txtAxisYTitle.MaxLength = 50;
            this.txtAxisYTitle.MouseState = MaterialSkin.MouseState.OUT;
            this.txtAxisYTitle.Multiline = false;
            this.txtAxisYTitle.Name = "txtAxisYTitle";
            this.txtAxisYTitle.Size = new System.Drawing.Size(493, 50);
            this.txtAxisYTitle.TabIndex = 6;
            this.txtAxisYTitle.Text = "";
            this.txtAxisYTitle.TrailingIcon = null;
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnCancel.Depth = 0;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.HighEmphasis = true;
            this.btnCancel.Icon = null;
            this.btnCancel.Location = new System.Drawing.Point(274, 235);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnCancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnCancel.Size = new System.Drawing.Size(77, 36);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnCancel.UseAccentColor = false;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Clicked);
            // 
            // btnOK
            // 
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnOK.Depth = 0;
            this.btnOK.HighEmphasis = true;
            this.btnOK.Icon = null;
            this.btnOK.Location = new System.Drawing.Point(136, 235);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnOK.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnOK.Name = "btnOK";
            this.btnOK.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnOK.Size = new System.Drawing.Size(64, 36);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnOK.UseAccentColor = false;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOk_Clicked);
            // 
            // GphAxisControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(500, 280);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtAxisYTitle);
            this.Controls.Add(this.txtAxisXTitle);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GphAxisControlForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Axis Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialTextBox txtAxisXTitle;
        private MaterialSkin.Controls.MaterialTextBox txtAxisYTitle;
        private MaterialSkin.Controls.MaterialButton btnCancel;
        private MaterialSkin.Controls.MaterialButton btnOK;
    }
}