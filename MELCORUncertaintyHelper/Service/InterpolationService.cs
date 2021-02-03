using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Service
{
    public class InterpolationService
    {
        private ExtractData[] extractDatas;
        private RefineData[] refineDatas;
        private RefineDataManager refineDataManager;

        public InterpolationService()
        {

        }

        public void Interpolation()
        {
            this.LoadData();
            var dataLength = this.refineDatas.Length;
            var variableLength = this.refineDatas[0].timeRecordDatas.Length;

            for (var i = 0; i < dataLength; i++)
            {
                for (var j = 0; j < variableLength; j++)
                {
                    this.refineDatas[i].timeRecordDatas[j].value = new double[this.refineDatas[i].timeRecordDatas[j].time.Length];
                    for (var k = 0; k < this.refineDatas[i].timeRecordDatas[j].time.Length; k++)
                    {
                        if (k == 0)
                        {
                            this.refineDatas[i].timeRecordDatas[j].value[k] = this.extractDatas[i].timeRecordDatas[j].value[k];
                        }
                        else
                        {
                            var x = this.extractDatas[i].timeRecordDatas[j].time[k];
                            var y = this.extractDatas[i].timeRecordDatas[j].value[k];
                            var xPrime = this.extractDatas[i].timeRecordDatas[j].time[k + 1];
                            var yPrime = this.extractDatas[i].timeRecordDatas[j].value[k + 1];
                            var p = this.refineDatas[i].timeRecordDatas[j].time[k];
                            var q = this.Calculation(x, y, xPrime, yPrime, p);
                            this.refineDatas[i].timeRecordDatas[j].value[k] = q;
                        }
                    }
                }
            }

            this.refineDataManager = RefineDataManager.GetRefineDataManager;
            this.refineDataManager.SetRefineDatas(this.refineDatas.Clone());
        }

        private void LoadData()
        {
            this.extractDatas = (ExtractData[])ExtractDataManager.GetDataManager.GetExtractDatas();
            if (this.extractDatas == null || this.extractDatas.Length < 0)
            {
                return;
            }
            this.refineDatas = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
            if (this.refineDatas == null || this.refineDatas.Length < 0)
            {
                return;
            }
        }

        private double Calculation(double x, double y, double xPrime, double yPrime, double p)
        {
            return (yPrime - y) * (p - x) / (xPrime - x) + y;
        }
    }
}
