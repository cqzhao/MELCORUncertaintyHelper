using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Model
{
    public class Distribution
    {
        public double fivePercentage
        {
            set;
            get;
        }

        public double fiftyPercentage
        {
            set;
            get;
        }

        public double ninetyFivePercentage
        {
            set;
            get;
        }

        public double mean
        {
            set;
            get;
        }

        public Histogram histogram
        {
            set;
            get;
        }
    }
}
