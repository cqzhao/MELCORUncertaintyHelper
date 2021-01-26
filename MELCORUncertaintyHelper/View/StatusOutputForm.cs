using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MELCORUncertaintyHelper.View
{
    public partial class StatusOutputForm : DockContent
    {
        private StatusOutputForm()
        {
            InitializeComponent();
        }

        private static readonly Lazy<StatusOutputForm> frmStatus = new Lazy<StatusOutputForm>(() => new StatusOutputForm());

        public static StatusOutputForm GetFrmStatus
        {
            get
            {
                return frmStatus.Value;
            }
        }

        private void StatusOutputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public void PrintStatus(StringBuilder msg)
        {
            this.txtStatus.Text += msg.ToString();
        }
    }
}
