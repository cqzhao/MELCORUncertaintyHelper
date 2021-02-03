using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Manager
{
    public class RefineDataManager
    {
        private RefineData[] refines;

        private RefineDataManager()
        {

        }

        private static readonly Lazy<RefineDataManager> refineDataManager = new Lazy<RefineDataManager>(() => new RefineDataManager());

        public static RefineDataManager GetRefineDataManager
        {
            get
            {
                return refineDataManager.Value;
            }
        }

        public void SetRefineDatas(object refineDatas)
        {
            this.refines = (RefineData[])refineDatas;
        }

        public Object GetRefineDatas()
        {
            if (this.refines == null || this.refines.Length < 0)
            {
                return null;
            }
            else
            {
                return this.refines.Clone();
            }
        }
    }
}
