using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Model
{
    public class TimeRecordData
    {
        public string variableName
        {
            set;
            get;
        }

        public double[] time
        {
            set;
            get;
        }

        public double[] value
        {
            set;
            get;
        }
    }
}
