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

namespace MELCORUncertaintyHelper.View.ResultView
{
    public partial class MomentEstimationGphForm : DockContent
    {
        private RefineData[] refineDatas;
        private DistributionData[] distributionDatas;
        private PlotModel plotModel;

        public MomentEstimationGphForm()
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
                    var momentFiveSeries = new LineSeries()
                    {
                        Title = "Moment 5%",
                        Color = OxyColors.Green,
                    };
                    var momentFiftySeries = new LineSeries()
                    {
                        Title = "Moment 50%",
                        Color = OxyColors.Blue,
                    };
                    var momentNinetyFiveSeries = new LineSeries()
                    {
                        Title = "Moment 95%",
                        Color = OxyColors.Red,
                    };
                    var momentMeanSeries = new LineSeries()
                    {
                        Title = "Moment Mean",
                        Color = OxyColors.Black,
                    };

                    var dataLength = this.distributionDatas[i].time.Length;
                    for (var j = 0; j < dataLength; j++)
                    {
                        var x = this.distributionDatas[i].time[j];

                        var momentFive = this.distributionDatas[i].momentDistributions[j].fivePercentage;
                        var momentFifty = this.distributionDatas[i].momentDistributions[j].fiftyPercentage;
                        var momentNinetyFive = this.distributionDatas[i].momentDistributions[j].ninetyFivePercentage;
                        var momentMean = this.distributionDatas[i].momentDistributions[j].mean;

                        momentFiveSeries.Points.Add(new DataPoint(x, momentFive));
                        momentFiftySeries.Points.Add(new DataPoint(x, momentFifty));
                        momentNinetyFiveSeries.Points.Add(new DataPoint(x, momentNinetyFive));
                        momentMeanSeries.Points.Add(new DataPoint(x, momentMean));
                    }

                    this.plotModel.Series.Add(momentFiveSeries);
                    this.plotModel.Series.Add(momentFiftySeries);
                    this.plotModel.Series.Add(momentNinetyFiveSeries);
                    this.plotModel.Series.Add(momentMeanSeries);
                }
            }
        }
    }
}
