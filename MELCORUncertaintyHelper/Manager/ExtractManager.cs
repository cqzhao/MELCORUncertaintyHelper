using MELCORUncertaintyHelper.Model;
using MELCORUncertaintyHelper.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MELCORUncertaintyHelper.Manager
{
    public class ExtractManager
    {
        private PTFFileOpenService ptfOpenSerivce;
        private InputVariableReadService inputReadService;
        private PTFFileReadService ptfReadService;

        public ExtractManager()
        {

        }

        public async Task Run()
        {
            await Task.Run(() =>
            {
                this.ptfOpenSerivce = PTFFileOpenService.GetOpenService;
                var ptfFiles = (PTFFile[])this.ptfOpenSerivce.GetFiles();
                if (ptfFiles == null || ptfFiles.Length <= 0)
                {
                    MessageBox.Show("There is no PTF file", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                this.inputReadService = InputVariableReadService.GetInputReadService;
                this.inputReadService.InputManage();

                for (var i = 0; i < ptfFiles.Length; i++)
                {
                    this.ptfReadService = new PTFFileReadService(ptfFiles[i]);
                    this.ptfReadService.Read();
                }
            });
        }
    }
}
