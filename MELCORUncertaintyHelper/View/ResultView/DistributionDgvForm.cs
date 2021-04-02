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

namespace MELCORUncertaintyHelper.View.ResultView
{
    public partial class DistributionDgvForm : DockContent
    {
        private RefineData[] refineDatas;
        private DistributionData[] distributionDatas;

        public DistributionDgvForm()
        {
            InitializeComponent();

            this.refineDatas = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
            this.distributionDatas = (DistributionData[])DistributionDataManager.GetDistributionDataManager.GetDistributionDatas();
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
                /*"Normal 5%",
                "Normal 50%",
                "Normal 95%",
                "Normal Mean",*/
                "LogNormal 5%",
                "LogNormal 50%",
                "LogNormal 95%",
                "LogNormal Mean",
                "Error Factor",
                /*"Moment 5%",
                "Moment 50%",
                "Moment 95%",
                "Moment Mean",*/
            };

            for (var i = 0; i < str.Count; i++)
            {
                this.dgvResults.Columns.Add("Distribution", str[i]);
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
                for (var j = 0; j < this.refineDatas.Length; j++)
                {
                    int idx = 0;
                    for (var k = 0; k < this.refineDatas[j].timeRecordDatas.Length; k++)
                    {
                        var variableName = this.refineDatas[j].timeRecordDatas[k].variableName;
                        if (variableName.Equals(target))
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
                }

                for (var j = 0; j < this.distributionDatas.Length; j++)
                {
                    var variableName = this.distributionDatas[j].variableName;
                    if (variableName.Equals(target))
                    {
                        /*values.Add(this.distributionDatas[j].normalDistributions[i].fivePercentage.ToString());
                        values.Add(this.distributionDatas[j].normalDistributions[i].fiftyPercentage.ToString());
                        values.Add(this.distributionDatas[j].normalDistributions[i].ninetyFivePercentage.ToString());
                        values.Add(this.distributionDatas[j].normalDistributions[i].mean.ToString());*/

                        values.Add(this.distributionDatas[j].lognormalDistributions[i].fivePercentage.ToString());
                        values.Add(this.distributionDatas[j].lognormalDistributions[i].fiftyPercentage.ToString());
                        values.Add(this.distributionDatas[j].lognormalDistributions[i].ninetyFivePercentage.ToString());
                        values.Add(this.distributionDatas[j].lognormalDistributions[i].mean.ToString());
                        values.Add(this.distributionDatas[j].lognormalDistributions[i].errorFactor.ToString());

                        /*values.Add(this.distributionDatas[j].momentDistributions[i].fivePercentage.ToString());
                        values.Add(this.distributionDatas[j].momentDistributions[i].fiftyPercentage.ToString());
                        values.Add(this.distributionDatas[j].momentDistributions[i].ninetyFivePercentage.ToString());
                        values.Add(this.distributionDatas[j].momentDistributions[i].mean.ToString());*/
                    }
                }

                for (var j = 0; j < values.Count; j++)
                {
                    this.dgvResults[j, i].Value = values[j];
                }
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
    }
}
