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
    public partial class VariableInputForm : DockContent
    {
        private string[] colNames;

        private VariableInputForm()
        {
            InitializeComponent();

            this.SetColNames();
            this.ShowColNames();
        }

        private static readonly Lazy<VariableInputForm> frmVariableInput = new Lazy<VariableInputForm>(() => new VariableInputForm());

        public static VariableInputForm GetFrmVariableInput
        {
            get
            {
                return frmVariableInput.Value;
            }
        }

        private void VariableInputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public DataGridView GetDgvVariable() => this.dgvVariable;

        private void SetColNames()
        {
            var names = new List<string>
            {
                "No",
                "Variable Name"
            };

            this.colNames = names.ToArray();
        }

        private void ShowColNames()
        {
            this.dgvVariable.ColumnCount = this.colNames.Length;
            for (var i = 0; i < this.colNames.Length; i++)
            {
                this.dgvVariable.Columns[i].Name = this.colNames[i];
                this.dgvVariable.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (this.colNames[i].Equals("No"))
                {
                    this.dgvVariable.Columns[i].ReadOnly = true;
                }
            }
        }

        /// <summary>
        /// DataGridView의 Column에 자동으로 번호를 매김
        /// 이 DataGridView에서는 "No" Column이 0번에 존재하기에
        /// "var noColIdx = 0"으로 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvVariable_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var noColIdx = 0;
            this.dgvVariable.Rows[e.RowIndex].Cells[noColIdx].Value = (e.RowIndex + 1).ToString();
        }
    }
}
