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
                        var p = this.refineDatas[i].timeRecordDatas[j].time[k];
                        var subList = new List<double>();
                        subList = this.extractDatas[i].timeRecordDatas[j].time.ToList();
                        /*var tmp = Math.Abs(this.refineDatas[i].timeRecordDatas[j].time.Length - this.extractDatas[i].timeRecordDatas[j].time.Length);
                        if (Math.Abs(k - this.extractDatas[i].timeRecordDatas[j].time.Length) <= tmp || k < 100)
                        {
                            if (k < 100)
                            {
                                for (var l = 0; l < k + 100; l++)
                                {
                                    subList.Add(this.extractDatas[i].timeRecordDatas[j].time[l]);
                                }
                            }
                            else
                            {
                                for (var l = k - 99; l < this.extractDatas[i].timeRecordDatas[j].time.Length; l++)
                                {
                                    subList.Add(this.extractDatas[i].timeRecordDatas[j].time[l]);
                                }
                            }
                        }
                        else
                        {
                            for (var l = k - 49; l < k + 50; l++)
                            {
                                subList.Add(this.extractDatas[i].timeRecordDatas[j].time[l]);
                            }
                        }*/

                        var nearTimes = this.FindNearTime(p, subList);
                        var xIdx = Array.FindIndex(this.extractDatas[i].timeRecordDatas[j].time, target => target == nearTimes[0]);
                        var xPrimeIdx = Array.FindIndex(this.extractDatas[i].timeRecordDatas[j].time, target => target == nearTimes[1]);
                        var x = this.extractDatas[i].timeRecordDatas[j].time[xIdx];
                        var y = this.extractDatas[i].timeRecordDatas[j].value[xIdx];
                        var xPrime = this.extractDatas[i].timeRecordDatas[j].time[xPrimeIdx];
                        var yPrime = this.extractDatas[i].timeRecordDatas[j].value[xPrimeIdx];
                        var q = this.Calculation(x, y, xPrime, yPrime, p);
                        this.refineDatas[i].timeRecordDatas[j].value[k] = q;
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

        private double[] FindNearTime(double target, List<double> time)
        {
            var min = Double.MaxValue;
            double nearTime = 0.0;
            int idx = 0;
            var nearTimes = new List<double>();

            for (var i = 0; i < time.Count; i++)
            {
                var abs = Math.Abs(time[i] - target);
                if (abs < min)
                {
                    min = abs;
                    idx = i;
                    nearTime = time[i];
                }
            }
            nearTimes.Add(nearTime);
            time.RemoveAt(idx);
            min = Double.MaxValue;
            for (var i = 0; i < time.Count; i++)
            {
                var abs = Math.Abs(time[i] - target);
                if (abs < min)
                {
                    min = abs;
                    nearTime = time[i];
                }
            }
            nearTimes.Add(nearTime);

            return nearTimes.ToArray();
        }
    }
}
