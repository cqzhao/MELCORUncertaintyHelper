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
        private StatusOutputForm frmStatus;
        private TimeInputForm frmTimeInput;

        public MainForm()
        {
            InitializeComponent();

            this.frmFileExplorer = new FileExplorerForm();
            this.frmVariableInput = VariableInputForm.GetFrmVariableInput;
            this.frmExtractedVariable = new ExtractedVariableForm(this);
            this.frmStatus = StatusOutputForm.GetFrmStatus;
            this.frmTimeInput = TimeInputForm.GetFrmTimeInupt;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.frmFileExplorer.Show(this.dockPnlMain, DockState.DockLeft);
            this.frmVariableInput.Show(this.frmFileExplorer.Pane, DockAlignment.Bottom, 0.6);
            this.frmExtractedVariable.Show(this.dockPnlMain, DockState.DockRight);
            this.frmStatus.Show(this.dockPnlMain, DockState.DockBottom);
            this.frmTimeInput.Show(this.frmVariableInput.Pane, DockAlignment.Bottom, 0.5);

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
            this.frmVariableInput.Show(this.frmFileExplorer.Pane, DockAlignment.Bottom, 0.5);
        }

        private void MsiShowExtracted_Click(object sender, EventArgs e)
        {
            this.frmExtractedVariable.Show(this.dockPnlMain, DockState.DockRight);
        }

        private void MsiShowStatus_Click(object sender, EventArgs e)
        {
            this.frmStatus.Show(this.dockPnlMain, DockState.DockBottom);
        }

        private void MsiShowTimeInput_Click(object sender, EventArgs e)
        {
            this.frmTimeInput.Show(this.frmVariableInput.Pane, DockAlignment.Bottom, 0.5);
        }

        private async void MsiRun_Click(object sender, EventArgs e)
        {
            var str = new StringBuilder();
            str.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
            str.AppendLine("Running is started");
            this.frmStatus.PrintStatus(str);
            var manager = new ExtractManager();
            await manager.Run();

            this.PrintExtractedVariables();

            str.Clear();
            str.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
            str.AppendLine("Running is completed");
            this.frmStatus.PrintStatus(str);
        }

        private void PrintExtractedVariables()
        {
            var variables = new List<string>();
            try
            {
                var refineData = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
                for (var i = 0; i < refineData.Length; i++)
                {
                    for (var j = 0; j < refineData[i].timeRecordDatas.Length; j++)
                    {
                        var name = refineData[i].timeRecordDatas[j].variableName;
                        if (!variables.Contains(name))
                        {
                            variables.Add(name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex);
                logWrite.MakeLogFile();
                return;
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
                TabText = target + " Table"
            };
            frmDgvResult.Show(this.dockPnlMain, DockState.Document);
            frmDgvResult.PrintResult(target);

            var frmGphResult = new VariableResultGphForm
            {
                TabText = target + " Graph"
            };
            frmGphResult.Show(this.dockPnlMain, DockState.Document);
            frmGphResult.PrintResult(target);

            var frmDgvResult2 = new ResultWithDistributionDgvForm
            {
                TabText = target + " Table"
            };
            frmDgvResult2.Show(this.dockPnlMain, DockState.Document);
            frmDgvResult2.PrintResult(target);

            var frmGphResult2 = new ResultWithDistributionGphForm
            {
                TabText = target + " Graph with Normal"
            };
            frmGphResult2.Show(this.dockPnlMain, DockState.Document);
            frmGphResult2.PrintResult(target);

            var frmGphResult3 = new ResultWithDistributionGphForm2
            {
                TabText = target + " Graph with LogNormal"
            };
            frmGphResult3.Show(this.dockPnlMain, DockState.Document);
            frmGphResult3.PrintResult(target);
        }
    }
}
