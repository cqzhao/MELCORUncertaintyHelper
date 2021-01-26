using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Model
{
    public class ExtractData
    {
        public string fileName
        {
            set;
            get;
        }

        public string[] inputVariables
        {
            set;
            get;
        }

        public TimeRecordData[] timeRecordDatas
        {
            set;
            get;
        }
    }
}
