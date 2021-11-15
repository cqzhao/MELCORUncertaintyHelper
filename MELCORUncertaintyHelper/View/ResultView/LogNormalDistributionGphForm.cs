using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
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
    public partial class LogNormalDistributionGphForm : DockContent
    {
        private GphAxisControlForm frmGphAxisControl;
        private StatusOutputForm frmStatus;
        private RefineData[] refineDatas;
        private DistributionData[] distributionDatas;
        private PlotModel plotModel;

        private bool isOKClicked;
        private string axisXTitle;
        private string axisYTitle;

        public LogNormalDistributionGphForm()
        {
            InitializeComponent();

            this.frmGphAxisControl = new GphAxisControlForm();
            this.isOKClicked = this.frmGphAxisControl.GetIsOkClicked();
            this.frmStatus = StatusOutputForm.GetFrmStatus;
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

        private async void TsbtnSave_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                DefaultExt = "png",
                Filter = "PNG Files(*.png)|*.png",
            };
            string fileName = string.Format("{0}_Log-Normal_Distribution.png", this.TabText.ToString());
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fileName = sfd.FileName;
            }
            await Task.Run(() =>
            {
                PngExporter.Export(this.plotModel, fileName, 800, 600, OxyColors.White);
                var statusContents = new StringBuilder();
                statusContents.AppendFormat("{0}   File {1} is created{2}",
                    DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]"), fileName, Environment.NewLine);
                this.frmStatus.PrintStatus(statusContents);
            });
        }

        private void TsbtnAxisSetting_Click(object sender, EventArgs e)
        {
            this.frmGphAxisControl.ShowDialog();
            this.isOKClicked = this.frmGphAxisControl.GetIsOkClicked();
            if (this.isOKClicked == false)
            {
                return;
            }
            this.axisXTitle = this.frmGphAxisControl.GetAxisXTitle();
            this.axisYTitle = this.frmGphAxisControl.GetAxisYTitle();
            this.gphResults.Invalidate(true);
            this.plotModel.Axes.Clear();
            var axisX = new LinearAxis
            {
                Title = this.axisXTitle,
                TitleFont = "Segoe UI",
                TitleFontSize = 15,
                AxisTitleDistance = 30,
                Position = AxisPosition.Bottom,
            };
            var axisY = new LinearAxis
            {
                Title = this.axisYTitle,
                TitleFont = "Segoe UI",
                TitleFontSize = 15,
                AxisTitleDistance = 30,
                Position = AxisPosition.Left,
            };
            this.plotModel.Axes.Add(axisX);
            this.plotModel.Axes.Add(axisY);
            axisX.Reset();
            axisY.Reset();
            this.plotModel.ResetAllAxes();
            this.plotModel.InvalidatePlot(true);
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
                    //Title = this.refineDatas[i].fileName,
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
