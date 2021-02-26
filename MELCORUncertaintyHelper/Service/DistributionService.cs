using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Service
{
    public class DistributionService
    {
        private RefineData[] refineDatas;
        private RefineDataManager refineDataManager;

        public DistributionService()
        {

        }

        public void MakeDistribution()
        {
            this.refineDatas = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
            var fileLength = this.refineDatas.Length;
            var recordDataLength = this.refineDatas[0].timeRecordDatas.Length;

            this.refineDatas[0].distributionDatas = new DistributionData[this.refineDatas[0].timeRecordDatas.Length];
            for (var j = 0; j < recordDataLength; j++)
            {
                var distributionData = new DistributionData
                {
                    variableName = this.refineDatas[0].timeRecordDatas[j].variableName,
                    time = this.refineDatas[0].timeRecordDatas[j].time,
                    normalDistributions = new Distribution[this.refineDatas[0].timeRecordDatas[j].time.Length],
                };
                this.refineDatas[0].distributionDatas[j] = distributionData;
            }

            for (var i = 0; i < recordDataLength; i++)
            {
                var timeLength = this.refineDatas[0].timeRecordDatas[i].time.Length;
                for (var j = 0; j < timeLength; j++)
                {
                    var values = new List<double>();
                    for (var k = 0; k < fileLength; k++)
                    {
                        var value = this.refineDatas[k].timeRecordDatas[i].value[j];
                        values.Add(value);
                    }
                    var normalDistribution = this.CalcNormalDistribution(values.ToArray());
                    this.refineDatas[0].distributionDatas[i].normalDistributions[j] = normalDistribution;
                }
            }

            this.refineDataManager = RefineDataManager.GetRefineDataManager;
            this.refineDataManager.SetRefineDatas(this.refineDatas.Clone());
        }

        private Distribution CalcNormalDistribution(double[] inputs)
        {
            var mean = Statistics.Mean(inputs);
            var stdDeviation = Statistics.StandardDeviation(inputs);

            /*var values = new double[1000000];
            var normal = new Normal(mean, stdDeviation);
            normal.Samples(values);

            var histogram = new Histogram(values, 100);*/

            var fivePer = 1.64 * stdDeviation + mean;
            //var tenPer = 1.28 * stdDeviation + mean;
            var fiftyPer = 0 * stdDeviation + mean;
            //var ninetyPer = -1.28 * stdDeviation + mean;
            var ninetyFivePer = -1.64 * stdDeviation + mean;

            var distribution = new Distribution
            {
                fivePercentage = fivePer,
                fiftyPercentage = fiftyPer,
                ninetyFivePercentage = ninetyFivePer,
                mean = mean,
                //histogram = histogram,
            };

            return distribution;
        }
    }
}
