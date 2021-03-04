using Accord.Statistics.Distributions.Univariate;
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
        private DistributionData[] distributionDatas;

        public DistributionService()
        {
            this.refineDatas = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
        }

        public void Run()
        {
            this.InitializeDistribution();
            this.MakeDistribution();
            DistributionDataManager.GetDistributionDataManager.SetDistributionsDatas(this.distributionDatas.Clone());
        }

        private void InitializeDistribution()
        {
            this.distributionDatas = new DistributionData[this.refineDatas[0].inputVariables.Length];
            for (var i = 0; i < this.refineDatas[0].timeRecordDatas.Length; i++)
            {
                var distributionLength = this.refineDatas[0].timeRecordDatas[i].time.Length;
                var distributionData = new DistributionData
                {
                    variableName = this.refineDatas[0].timeRecordDatas[i].variableName,
                    time = this.refineDatas[0].timeRecordDatas[i].time,
                    normalDistributions = new Distribution[distributionLength],
                    lognormalDistributions = new Distribution[distributionLength],
                };
                this.distributionDatas[i] = distributionData;
            }
        }

        private void MakeDistribution()
        {
            var fileLength = this.refineDatas.Length;
            var recordDataLength = this.refineDatas[0].timeRecordDatas.Length;
            for (var i = 0; i < recordDataLength; i++)
            {
                var timeLength = this.refineDatas[0].timeRecordDatas[i].time.Length;
                for (var j = 0; j < timeLength; j++)
                {
                    var observations = new List<double>();
                    for (var k = 0; k < fileLength; k++)
                    {
                        var value = this.refineDatas[k].timeRecordDatas[i].value[j];
                        observations.Add(value);
                    }

                    var normalDistribution = this.CalcNormalDistribution(observations.ToArray());
                    var lognormalDistribution = this.CalcLognormalDistribution(observations.ToArray());

                    this.distributionDatas[i].normalDistributions[j] = normalDistribution;
                    this.distributionDatas[i].lognormalDistributions[j] = lognormalDistribution;
                }
            }
        }

        private Distribution CalcNormalDistribution(double[] observations)
        {
            var normal = new NormalDistribution();
            double mean;
            double stdDeviation;

            var sameCnt = 0;
            for (var i = 0; i < observations.Length; i++)
            {
                if (observations[i] == observations[i])
                {
                    sameCnt += 1;
                }
            }

            if (sameCnt == observations.Length)
            {
                mean = Statistics.Mean(observations);
                stdDeviation = Statistics.StandardDeviation(observations);
            }
            else
            {
                normal.Fit(observations);
                mean = normal.Mean;
                stdDeviation = normal.StandardDeviation;
            }

            /*var mean = Statistics.Mean(inputs);
            var stdDeviation = Statistics.StandardDeviation(inputs);

            var values = new double[1000000];
            var normal = new Normal(mean, stdDeviation);
            normal.Samples(values);

            var histogram = new Histogram(values, 100);*/

            var fivePer = -1.645 * stdDeviation + mean;
            //var tenPer = -1.28 * stdDeviation + mean;
            var fiftyPer = 0 * stdDeviation + mean;
            //var ninetyPer = 1.28 * stdDeviation + mean;
            var ninetyFivePer = 1.645 * stdDeviation + mean;

            var distribution = new Distribution
            {
                fivePercentage = fivePer,
                fiftyPercentage = fiftyPer,
                ninetyFivePercentage = ninetyFivePer,
                mean = mean,
            };

            return distribution;
        }

        private Distribution CalcLognormalDistribution(double[] observations)
        {
            var lognormal = new LognormalDistribution();
            lognormal.Fit(observations);

            var mu = lognormal.Location;
            var sigma = lognormal.Shape;

            double fivePer;
            double fiftyPer;
            double ninetyFivePer;
            double mean;

            if (Double.IsNaN(Math.Exp(mu) / Math.Exp(1.645 * sigma)))
            {
                fivePer = 0;
            }
            else
            {
                fivePer = Math.Exp(mu) / Math.Exp(1.645 * sigma);
            }

            fiftyPer = Math.Exp(mu);

            if (Double.IsNaN(Math.Exp(mu) * Math.Exp(1.645 * sigma)))
            {
                ninetyFivePer = 0;
            }
            else
            {
                ninetyFivePer = Math.Exp(mu) * Math.Exp(1.645 * sigma);
            }

            if (Double.IsNaN(lognormal.Mean))
            {
                mean = 0;
            }
            else
            {
                mean = lognormal.Mean;
            }

            var distribution = new Distribution()
            {
                fivePercentage = fivePer,
                fiftyPercentage = fiftyPer,
                ninetyFivePercentage = ninetyFivePer,
                mean = mean,
            };

            return distribution;
        }
    }
}
