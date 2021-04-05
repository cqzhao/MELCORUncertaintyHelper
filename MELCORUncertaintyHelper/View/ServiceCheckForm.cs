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
    public partial class ServiceCheckForm : Form
    {
        public bool isCheckedInterpolation;
        public bool isCheckedStatistics;
        public bool isClicked;

        public ServiceCheckForm()
        {
            InitializeComponent();

            this.isCheckedInterpolation = false;
            this.isCheckedStatistics = false;
            this.isClicked = false;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.ExtractValues();
            this.isClicked = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.isClicked = false;
            this.Close();
        }

        private void ChkInterpolation_CheckedChanged(object sender, EventArgs e)
        {
            this.ExtractValues();
            if (this.isCheckedInterpolation == false && this.isCheckedStatistics == true)
            {
                this.isCheckedStatistics = false;
                this.chkStatistics.Checked = false;
            }
        }

        private void ChkStatistics_CheckedChanged(object sender, EventArgs e)
        {
            this.ExtractValues();
            if (this.isCheckedInterpolation == false && this.isCheckedStatistics == true)
            {
                this.isCheckedInterpolation = true;
                this.chkInterpolation.Checked = true;
            }
        }

        private void ExtractValues()
        {
            this.isCheckedInterpolation = this.chkInterpolation.Checked;
            this.isCheckedStatistics = this.chkStatistics.Checked;
        }
    }
}
