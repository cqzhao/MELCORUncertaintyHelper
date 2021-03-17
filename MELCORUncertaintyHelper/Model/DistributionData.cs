using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Model
{
    public class DistributionData
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

        public Distribution[] normalDistributions
        {
            set;
            get;
        }

        public Distribution[] lognormalDistributions
        {
            set;
            get;
        }

        public Distribution[] momentDistributions
        {
            set;
            get;
        }
    }
}
