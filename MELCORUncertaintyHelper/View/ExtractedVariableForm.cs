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
using WeifenLuo.WinFormsUI.Docking;

namespace MELCORUncertaintyHelper.View
{
    public partial class ExtractedVariableForm : DockContent
    {
        private MainForm frmMain;
        private string[] colNames;

        public ExtractedVariableForm(MainForm frmMain)
        {
            InitializeComponent();

            this.frmMain = frmMain;
            this.SetColNames();
            this.ShowColNames();
        }

        private void ExtractedVariableForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void SetColNames()
        {
            var names = new List<string>
            {
                " ",
                "Variable Name"
            };

            this.colNames = names.ToArray();
        }

        private void ShowColNames()
        {
            for (var i = 0; i < this.colNames.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(this.colNames[i]))
                {
                    var chkCol = new DataGridViewCheckBoxColumn();
                    this.dgvVariables.Columns.Add(chkCol);
                }
                else
                {
                    this.dgvVariables.Columns.Add(this.colNames[i], this.colNames[i]);
                    this.dgvVariables.Columns[i].ReadOnly = true;
                }
                this.dgvVariables.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void AddVariables(string[] variables)
        {
            this.dgvVariables.Rows.Clear();
            for (var i = 0; i < variables.Length; i++)
            {
                this.dgvVariables.Rows.Add(false, variables[i]);
            }
        }

        private void DdgvVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                return;
            }
            if (e.RowIndex < 0)
            {
                return;
            }
            var target = this.dgvVariables[e.ColumnIndex, e.RowIndex].Value.ToString();
            this.frmMain.ShowResult(target);
        }

        private void TsBtnCheckAll_Click(object sender, EventArgs e)
        {
            if (this.dgvVariables.Rows.Count <= 0)
            {
                return;
            }

            this.MoveSelectedCell();

            for (var i = 0; i < this.dgvVariables.Rows.Count; i++)
            {
                var isChecked = Convert.ToBoolean(this.dgvVariables[0, i].Value);
                if (!isChecked)
                {
                    this.dgvVariables[0, i].Value = true;
                }
            }
        }

        private void TsBtnUncheckAll_Click(object sender, EventArgs e)
        {
            if (this.dgvVariables.Rows.Count <= 0)
            {
                return;
            }

            this.MoveSelectedCell();

            for (var i = 0; i < this.dgvVariables.Rows.Count; i++)
            {
                var isChecked = Convert.ToBoolean(this.dgvVariables[0, i].Value);
                if (isChecked)
                {
                    this.dgvVariables[0, i].Value = false;
                }
            }
        }

        private void MoveSelectedCell()
        {
            this.dgvVariables.CurrentCell = this.dgvVariables.Rows[0].Cells[1];
        }

        private async void TsBtnSave_Click(object sender, EventArgs e)
        {
            if (this.dgvVariables.Rows.Count <= 0)
            {
                return;
            }

            var variables = this.GetVariables().ToArray();
            var csvWriteService = new CSVWriteService(variables);
            await csvWriteService.WriteFile();
        }

        private List<string> GetVariables()
        {
            var variables = new List<string>();

            for (var i = 0; i < this.dgvVariables.Rows.Count; i++)
            {
                if (this.dgvVariables[1, i].Value != null)
                {
                    var variable = this.dgvVariables[1, i].Value.ToString();
                    variables.Add(variable);
                }
            }

            return variables;
        }
    }
}
