using MELCORUncertaintyHelper.Model;
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
    public partial class FileExplorerForm : DockContent
    {
        public FileExplorerForm()
        {
            InitializeComponent();
        }

        private void FileExplorerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        public void OpenFiles(object inputFiles)
        {
            var files = (PTFFile[])inputFiles;
            this.tvwFiles.Nodes.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("PTF Files");
            stringBuilder.Append(" (");
            stringBuilder.Append(files.Length.ToString());
            stringBuilder.Append(")");
            var fileNode = new TreeNode(stringBuilder.ToString());
            for (var i = 0; i < files.Length; i++)
            {
                fileNode.Nodes.Add(files[i].path, files[i].name);
            }
            this.tvwFiles.Nodes.Add(fileNode);
        }

        public void DeleteAllFiles()
        {
            this.tvwFiles.Nodes.Clear();
        }
    }
}
