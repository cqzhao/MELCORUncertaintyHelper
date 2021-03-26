using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
using OxyPlot;
using OxyPlot.Series;
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
    public partial class LogNormalDistributionGphForm : DockContent
    {
        private RefineData[] refineDatas;
        private DistributionData[] distributionDatas;
        private PlotModel plotModel;

        public LogNormalDistributionGphForm()
        {
            InitializeComponent();

            this.refineDatas = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
            this.distributionDatas = (DistributionData[])DistributionDataManager.GetDistributionDataManager.GetDistributionDatas();
            this.plotModel = new PlotModel()
            {
                LegendBorder = OxyColors.Black,
                LegendBackground = OxyColor.FromAColor(32, OxyColors.Black),
                LegendPosition = LegendPosition.TopCenter,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Horizontal,
            };
            this.gphResults.Model = this.plotModel;
        }

        public void PrintResult(string target)
        {
            for (var i = 0; i < this.refineDatas.Length; i++)
            {
                var targetIdx = 0;
                for (var j = 0; j < this.refineDatas[i].timeRecordDatas.Length; j++)
                {
                    if (this.refineDatas[i].timeRecordDatas[j].variableName.Equals(target))
                    {
                        targetIdx = j;
                    }
                }
                var dataLength = this.refineDatas[i].timeRecordDatas[targetIdx].time.Length;
                var series = new LineSeries()
                {
                    Title = this.refineDatas[i].fileName,
                    Color = OxyColors.DimGray,
                };
                for (var j = 0; j < dataLength; j++)
                {
                    var x = this.refineDatas[i].timeRecordDatas[targetIdx].time[j];
                    var y = this.refineDatas[i].timeRecordDatas[targetIdx].value[j];
                    series.Points.Add(new DataPoint(x, y));
                }
                this.plotModel.Series.Add(series);
            }

            for (var i = 0; i < this.distributionDatas.Length; i++)
            {
                var variableName = this.distributionDatas[i].variableName;
                if (variableName.Equals(target))
                {
                    var lognormalFiveSeries = new LineSeries()
                    {
                        Title = "LogNormal 5%",
                        Color = OxyColors.Green,
                    };
                    var lognormalFiftySeries = new LineSeries()
                    {
                        Title = "LogNormal 50%",
                        Color = OxyColors.Blue,
                    };
                    var lognormalNinetyFiveSeries = new LineSeries()
                    {
                        Title = "LogNormal 95%",
                        Color = OxyColors.Red,
                    };
                    var lognormalMeanSeries = new LineSeries()
                    {
                        Title = "LogNormal Mean",
                        Color = OxyColors.Black,
                    };

                    var dataLength = this.distributionDatas[i].time.Length;
                    for (var j = 0; j < dataLength; j++)
                    {
                        var x = this.distributionDatas[i].time[j];

                        var lognormalFive = this.distributionDatas[i].lognormalDistributions[j].fivePercentage;
                        var lognormalFifty = this.distributionDatas[i].lognormalDistributions[j].fiftyPercentage;
                        var lognormalNinetyFive = this.distributionDatas[i].lognormalDistributions[j].ninetyFivePercentage;
                        var lognormalMean = this.distributionDatas[i].lognormalDistributions[j].mean;

                        lognormalFiveSeries.Points.Add(new DataPoint(x, lognormalFive));
                        lognormalFiftySeries.Points.Add(new DataPoint(x, lognormalFifty));
                        lognormalNinetyFiveSeries.Points.Add(new DataPoint(x, lognormalNinetyFive));
                        lognormalMeanSeries.Points.Add(new DataPoint(x, lognormalMean));
                    }

                    this.plotModel.Series.Add(lognormalFiveSeries);
                    this.plotModel.Series.Add(lognormalFiftySeries);
                    this.plotModel.Series.Add(lognormalNinetyFiveSeries);
                    this.plotModel.Series.Add(lognormalMeanSeries);
                }
            }
        }
    }
}
