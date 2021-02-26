using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace MELCORUncertaintyHelper.View
{
    public partial class ResultWithDistributionForm : DockContent
    {
        private RefineData[] refineDatas;

        public ResultWithDistributionForm()
        {
            InitializeComponent();

            this.refineDatas = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
        }

        private void ResultWithDistributionForm_Load(object sender, EventArgs e)
        {
            this.dgvResults.Columns.Add("Time", "Time");
            for (var i = 0; i < this.refineDatas.Length; i++)
            {
                this.dgvResults.Columns.Add(this.refineDatas[i].fileName, this.refineDatas[i].fileName);
            }

            var str = new List<string>
            {
                "Normal 5%",
                "Normal 50%",
                "Normal 95%",
                "Normal Mean"
            };

            for (var i = 0; i < str.Count; i++)
            {
                this.dgvResults.Columns.Add("Normal Distribution", str[i]);
            }

            var columnLength = this.dgvResults.Columns.Count;
            for (var i = 0; i < columnLength; i++)
            {
                this.dgvResults.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void PrintResult(string target)
        {
            var rowSize = this.FindMaxTimeLength(target);
            for (var i = 0; i < rowSize; i++)
            {
                this.dgvResults.Rows.Add();
                var values = new List<string>();
                var inputs = new double[this.refineDatas.Length];
                for (var j = 0; j < this.refineDatas.Length; j++)
                {
                    int idx = 0;
                    for (var k = 0; k < this.refineDatas[j].timeRecordDatas.Length; k++)
                    {
                        var variable = this.refineDatas[j].timeRecordDatas[k].variableName;
                        if (variable.Equals(target))
                        {
                            idx = k;
                            break;
                        }
                    }
                    if (j == 0)
                    {
                        values.Add(this.refineDatas[j].timeRecordDatas[idx].time[i].ToString());
                    }
                    values.Add(this.refineDatas[j].timeRecordDatas[idx].value[i].ToString());
                    inputs[j] = this.refineDatas[j].timeRecordDatas[idx].value[i];
                    /*if (this.refineDatas[j].timeRecordDatas[idx].time.Length > i)
                    {
                        values.Add(this.refineDatas[j].timeRecordDatas[idx].value[i].ToString());
                    }
                    else
                    {
                        values.Add(null);
                    }*/
                }
                var distribution = this.CalcNormalDistribution(inputs);
                for (var j = 0; j < values.Count; j++)
                {
                    this.dgvResults[j, i].Value = values[j];
                }
                this.dgvResults[values.Count, i].Value = distribution.fivePercentage;
                this.dgvResults[values.Count + 1, i].Value = distribution.fiftyPercentage;
                this.dgvResults[values.Count + 2, i].Value = distribution.ninetyFivePercentage;
                this.dgvResults[values.Count + 3, i].Value = distribution.mean;
            }
        }

        private int FindMaxTimeLength(string target)
        {
            var max = Int32.MinValue;
            for (var i = 0; i < this.refineDatas.Length; i++)
            {
                for (var j = 0; j < this.refineDatas[i].timeRecordDatas.Length; j++)
                {
                    if (this.refineDatas[i].timeRecordDatas[j].variableName.Equals(target))
                    {
                        var tmp = this.refineDatas[i].timeRecordDatas[j].time.Length;
                        if (tmp > max)
                        {
                            max = tmp;
                        }
                    }
                }
            }
            return max;
        }

        private Distribution CalcNormalDistribution(double[] inputs)
        {
            var mean = Statistics.Mean(inputs);
            var stdDeviation = Statistics.StandardDeviation(inputs);

            /*var values = new double[100000];
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
