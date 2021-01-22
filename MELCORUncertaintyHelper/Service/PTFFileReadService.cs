using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Service
{
    public class PTFFileReadService
    {
        private PTFFile[] files;

        public PTFFileReadService(object files)
        {
            this.files = (PTFFile[])files;
        }
    }
}
