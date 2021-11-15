using MaterialSkin.Controls;
using MELCORUncertaintyHelper.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MELCORUncertaintyHelper.View
{
    public partial class GphAxisControlForm : MaterialForm
    {
        private bool isOKClicked;
        private string axisXTitle;
        private string axisYTitle;

        public GphAxisControlForm()
        {
            InitializeComponent();

            this.isOKClicked = false;
        }

        public bool GetIsOkClicked()
        {
            return this.isOKClicked;
        }

        public string GetAxisXTitle()
        {
            return this.axisXTitle;
        }

        public string GetAxisYTitle()
        {
            return this.axisYTitle;
        }

        private void BtnOk_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.axisXTitle = this.txtAxisXTitle.Text;
                this.axisYTitle = this.txtAxisYTitle.Text;

                this.isOKClicked = true;
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Check the input value.", "MERTAG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.isOKClicked = false;
                this.Close();
            }
            catch (Exception ex)
            {
                var logWriter = new LogFileWriteService(ex, "BtnCancel_Clicked()");
                logWriter.MakeLogFile();
            }
        }
    }
}
