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
    public partial class NormalDistributionGphForm : DockContent
    {
        private RefineData[] refineDatas;
        private DistributionData[] distributionDatas;
        private PlotModel plotModel;

        public NormalDistributionGphForm()
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
                    var normalFiveSeries = new LineSeries()
                    {
                        Title = "Normal 5%",
                        Color = OxyColors.Green,
                    };
                    var normalFiftySeries = new LineSeries()
                    {
                        Title = "Normal 50%",
                        Color = OxyColors.Blue,
                    };
                    var normalNinetyFiveSeries = new LineSeries()
                    {
                        Title = "Normal 95%",
                        Color = OxyColors.Red,
                    };
                    var normalMeanSeries = new LineSeries()
                    {
                        Title = "Normal Mean",
                        Color = OxyColors.Black,
                    };

                    var dataLength = this.distributionDatas[i].time.Length;
                    for (var j = 0; j < dataLength; j++)
                    {
                        var x = this.distributionDatas[i].time[j];
                        var normalFive = this.distributionDatas[i].normalDistributions[j].fivePercentage;
                        var normalFifty = this.distributionDatas[i].normalDistributions[j].fiftyPercentage;
                        var normalNinetyFive = this.distributionDatas[i].normalDistributions[j].ninetyFivePercentage;
                        var normalMean = this.distributionDatas[i].normalDistributions[j].mean;

                        normalFiveSeries.Points.Add(new DataPoint(x, normalFive));
                        normalFiftySeries.Points.Add(new DataPoint(x, normalFifty));
                        normalNinetyFiveSeries.Points.Add(new DataPoint(x, normalNinetyFive));
                        normalMeanSeries.Points.Add(new DataPoint(x, normalMean));
                    }

                    this.plotModel.Series.Add(normalFiveSeries);
                    this.plotModel.Series.Add(normalFiftySeries);
                    this.plotModel.Series.Add(normalNinetyFiveSeries);
                    this.plotModel.Series.Add(normalMeanSeries);
                }
            }
        }
    }
}
