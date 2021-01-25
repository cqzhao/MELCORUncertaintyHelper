using MELCORUncertaintyHelper.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Manager
{
    public class ExtractManager
    {
        private InputVariableReadService inputReadService;

        public ExtractManager()
        {

        }

        public void Run()
        {
            this.inputReadService = InputVariableReadService.GetInputReadService;
            this.inputReadService.InputManage();
        }
    }
}
