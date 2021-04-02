using MELCORUncertaintyHelper.Model;
using MELCORUncertaintyHelper.Service;
using MELCORUncertaintyHelper.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MELCORUncertaintyHelper.Manager
{
    public class ExtractManager
    {
        private PTFFileOpenService ptfOpenSerivce;
        private InputVariableReadService inputReadService;
        private PTFFileReadService ptfReadService;
        private InputTimeReadService inputTimeReadService;
        private RefineDataProcessService refineProcessService;
        private DistributionService distributionService;

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
                var isInputManageFinished = this.inputReadService.InputManage();
                if (isInputManageFinished == false)
                {
                    return;
                }

                // 현재 Run()을 실행하면 데이터가 중복으로 생성되는 경우가 발생
                // 이를 임시적으로 해결하기 위한 방안
                ExtractDataManager.GetDataManager.InitializeData();

                var frmStatus = StatusOutputForm.GetFrmStatus;
                var msg = new StringBuilder();

                for (var i = 0; i < ptfFiles.Length; i++)
                {
                    this.ptfReadService = new PTFFileReadService(ptfFiles[i]);
                    this.ptfReadService.Read();
                    msg = new StringBuilder();
                    msg.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
                    msg.Append("Completed Read ");
                    msg.AppendLine(ptfFiles[i].fullPath);
                    frmStatus.PrintStatus(msg);
                }

                msg = new StringBuilder();
                msg.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
                msg.AppendLine("Interpolation Process is started");
                frmStatus.PrintStatus(msg);

                this.inputTimeReadService = InputTimeReadService.GetInputTimeReadService;
                this.inputTimeReadService.ExtractTime();

                this.refineProcessService = new RefineDataProcessService();
                this.refineProcessService.Refine();

                msg = new StringBuilder();
                msg.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
                msg.AppendLine("Interpolation Process is completed");
                frmStatus.PrintStatus(msg);

                msg = new StringBuilder();
                msg.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
                msg.AppendLine("Distribution Process is started");
                frmStatus.PrintStatus(msg);

                this.distributionService = new DistributionService();
                this.distributionService.Run();

                msg = new StringBuilder();
                msg.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
                msg.AppendLine("Distribution Process is completed");
                frmStatus.PrintStatus(msg);
            });
        }
    }
}
