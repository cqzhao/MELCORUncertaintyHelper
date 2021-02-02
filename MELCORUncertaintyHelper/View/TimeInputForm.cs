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
    public partial class TimeInputForm : DockContent
    {
        private string[] colNames;

        private TimeInputForm()
        {
            InitializeComponent();

            this.SetColNames();
            this.ShowColNames();
        }

        private static readonly Lazy<TimeInputForm> frmTimeInput = new Lazy<TimeInputForm>(() => new TimeInputForm());

        public static TimeInputForm GetFrmTimeInupt
        {
            get
            {
                return frmTimeInput.Value;
            }
        }

        private void TimeInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public DataGridView GetDgvTime() => this.dgvTime;

        private void SetColNames()
        {
            var names = new List<string>
            {
                "Time",
                "DTPLT"
            };

            this.colNames = names.ToArray();
        }

        private void ShowColNames()
        {
            this.dgvTime.ColumnCount = this.colNames.Length;
            for (var i = 0; i < this.colNames.Length; i++)
            {
                this.dgvTime.Columns[i].Name = this.colNames[i];
                this.dgvTime.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
    }
}
