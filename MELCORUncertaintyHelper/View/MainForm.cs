using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
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
    public partial class MainForm : Form
    {
        private FileExplorerForm frmFileExplorer;
        private VariableInputForm frmVariableInput;
        private ExtractedVariableForm frmExtractedVariable;

        public MainForm()
        {
            InitializeComponent();

            this.frmFileExplorer = new FileExplorerForm();
            this.frmVariableInput = VariableInputForm.GetFrmVariableInput;
            this.frmExtractedVariable = new ExtractedVariableForm(this);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.frmFileExplorer.Show(this.dockPnlMain, DockState.DockLeft);
            this.frmVariableInput.Show(this.dockPnlMain, DockState.DockLeftAutoHide);
            this.frmExtractedVariable.Show(this.dockPnlMain, DockState.DockRight);

            this.dockPnlMain.UpdateDockWindowZOrder(DockStyle.Left, true);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void MsiOpen_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "PTF File (*.ptf, *.PTF)|*.ptf;*.PTF",
                Multiselect = true,
            };
            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            var openService = PTFFileOpenService.GetOpenService;
            openService.OpenFiles(ofd.FileNames);

            this.frmFileExplorer.OpenFiles(openService.GetFiles());
        }

        private void MsiDeleteAllFiles_Click(object sender, EventArgs e)
        {
            var openService = PTFFileOpenService.GetOpenService;
            openService.DeleteFiles();

            this.frmFileExplorer.DeleteAllFiles();
        }

        private void MsiShowInputFileList_Click(object sender, EventArgs e)
        {
            this.frmFileExplorer.Show(this.dockPnlMain, DockState.DockLeft);
        }

        private void MsiShowVariableInput_Click(object sender, EventArgs e)
        {
            this.frmVariableInput.Show(this.dockPnlMain, DockState.DockLeft);
        }

        private void MsiShowExtracted_Click(object sender, EventArgs e)
        {
            this.frmExtractedVariable.Show(this.dockPnlMain, DockState.DockRight);
        }

        private async void MsiRun_Click(object sender, EventArgs e)
        {
            var manager = new ExtractManager();
            await manager.Run();

            this.PrintExtractedVariables();
        }

        private void PrintExtractedVariables()
        {
            var variables = new List<string>();
            var extractData = (ExtractData[])ExtractDataManager.GetDataManager.GetExtractDatas();
            for (var i = 0; i < extractData.Length; i++)
            {
                for (var j = 0; j < extractData[i].timeRecordDatas.Length; j++)
                {
                    var name = extractData[i].timeRecordDatas[j].variableName;
                    if (!variables.Contains(name))
                    {
                        variables.Add(name);
                    }
                }
            }

            if (variables.Count <= 0)
            {
                MessageBox.Show("There is no data what you want to find", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.frmExtractedVariable.AddVariables(variables.ToArray());
        }

        public void ShowResult(string target)
        {
            var frmDgvResult = new VariableResultDgvForm
            {
                TabText = target
            };
            frmDgvResult.Show(this.dockPnlMain, DockState.Document);
            frmDgvResult.PrintResult(target);

            var frmGphResult = new VariableResultGphForm
            {
                TabText = target
            };
            frmGphResult.Show(this.dockPnlMain, DockState.Document);
            frmGphResult.PrintResult(target);
        }
    }
}
