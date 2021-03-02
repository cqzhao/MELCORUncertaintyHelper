using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Manager
{
    public class DistributionDataManager
    {
        private DistributionData[] distributionDatas;

        private DistributionDataManager()
        {

        }

        private static readonly Lazy<DistributionDataManager> distributionDataManager = new Lazy<DistributionDataManager>(() => new DistributionDataManager());

        public static DistributionDataManager GetDistributionDataManager
        {
            get
            {
                return distributionDataManager.Value;
            }
        }

        public void SetDistributionsDatas(object distributions)
        {
            this.distributionDatas = (DistributionData[])distributions;
        }

        public object GetDistributionDatas()
        {
            if (this.distributionDatas == null || this.distributionDatas.Length < 0)
            {
                return null;
            }
            else
            {
                return this.distributionDatas.Clone();
            }
        }
    }
}
