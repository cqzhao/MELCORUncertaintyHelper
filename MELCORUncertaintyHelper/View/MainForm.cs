using MELCORUncertaintyHelper.Manager;
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
        private StatusOutputForm frmStatusOutput;

        public MainForm()
        {
            InitializeComponent();

            this.frmFileExplorer = new FileExplorerForm();
            this.frmVariableInput = VariableInputForm.GetFrmVariableInput;
            this.frmStatusOutput = StatusOutputForm.GetFrmStatus;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.frmFileExplorer.Show(this.dockPnlMain, DockState.DockLeft);
            this.frmVariableInput.Show(this.dockPnlMain, DockState.DockLeftAutoHide);
            this.frmStatusOutput.Show(this.dockPnlMain, DockState.DockBottom);

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

        private void MsiShowStatus_Click(object sender, EventArgs e)
        {
            this.frmStatusOutput.Show(this.dockPnlMain, DockState.DockBottom);
        }

        private async void MsiRun_Click(object sender, EventArgs e)
        {
            var manager = new ExtractManager();
            await manager.Run();
        }
    }
}
