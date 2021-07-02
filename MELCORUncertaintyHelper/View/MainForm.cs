using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
using MELCORUncertaintyHelper.Service;
using MELCORUncertaintyHelper.View.ResultView;
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
        private bool isCheckedInterpolation;
        private bool isCheckedStatistics;

        public MainForm()
        {
            InitializeComponent();

            this.frmFileExplorer = new FileExplorerForm();
            this.frmVariableInput = VariableInputForm.GetFrmVariableInput;
            this.frmExtractedVariable = new ExtractedVariableForm(this, this.isCheckedInterpolation, this.isCheckedStatistics);
            this.frmStatus = StatusOutputForm.GetFrmStatus;
            this.frmTimeInput = TimeInputForm.GetFrmTimeInupt;

            this.isCheckedInterpolation = false;
            this.isCheckedStatistics = false;
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
            var frmServiceCheck = new ServiceCheckForm();
            frmServiceCheck.ShowDialog();
            if (frmServiceCheck.isClicked == false)
            {
                return;
            }

            this.isCheckedInterpolation = frmServiceCheck.isCheckedInterpolation;
            this.isCheckedStatistics = frmServiceCheck.isCheckedStatistics;

            var str = new StringBuilder();
            str.Append(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]   "));
            str.AppendLine("Running is started");
            this.frmStatus.PrintStatus(str);
            var manager = new ExtractManager(this.isCheckedInterpolation, this.isCheckedStatistics);
            await manager.Run();

            this.PrintExtractedVariables();

            str.Clear();
            str.Append(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]   "));
            str.AppendLine("Running is completed");
            this.frmStatus.PrintStatus(str);
        }

        private void PrintExtractedVariables()
        {
            var variables = new List<string>();
            try
            {
                if (this.isCheckedInterpolation == true)
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
                else
                {
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
            this.frmExtractedVariable.SetFlag(this.isCheckedInterpolation, this.isCheckedStatistics);
        }

        public void ShowResult(string target)
        {
            var frmPlainGph = new VariableResultGphForm(this.isCheckedInterpolation)
            {
                TabText = target + " Graph"
            };
            frmPlainGph.Show(this.dockPnlMain, DockState.Document);
            frmPlainGph.PrintResult(target);

            if (this.isCheckedStatistics == true)
            {
                /*var frmNormalGph = new NormalDistributionGphForm
                {
                    TabText = target + " Normal Distribution Graph"
                };
                frmNormalGph.Show(this.dockPnlMain, DockState.Document);
                frmNormalGph.PrintResult(target);*/

                var frmLogNormalGph = new LogNormalDistributionGphForm
                {
                    TabText = target + " Log-Normal Distribution Graph"
                };
                frmLogNormalGph.Show(this.dockPnlMain, DockState.Document);
                frmLogNormalGph.PrintResult(target);

                /*var frmMomentGph = new MomentEstimationGphForm
                {
                    TabText = target + " Method of Moments Graph"
                };
                frmMomentGph.Show(this.dockPnlMain, DockState.Document);
                frmMomentGph.PrintResult(target);*/

                var frmDistributionDgv = new DistributionDgvForm(this.isCheckedStatistics)
                {
                    TabText = target + " Table"
                };
                frmDistributionDgv.Show(this.dockPnlMain, DockState.Document);
                frmDistributionDgv.PrintResult(target);
            }
            else
            {
                if (this.isCheckedInterpolation == true)
                {
                    var frmDistributionDgv = new DistributionDgvForm(this.isCheckedStatistics)
                    {
                        TabText = target + " Table"
                    };
                    frmDistributionDgv.Show(this.dockPnlMain, DockState.Document);
                    frmDistributionDgv.PrintResult(target);
                }
                else
                {
                    var frmDgvResult = new VariableResultDgvForm(this.isCheckedInterpolation)
                    {
                        TabText = target + " Table"
                    };
                    frmDgvResult.Show(this.dockPnlMain, DockState.Document);
                    frmDgvResult.PrintResult(target);
                }
            }
        }
    }
}
