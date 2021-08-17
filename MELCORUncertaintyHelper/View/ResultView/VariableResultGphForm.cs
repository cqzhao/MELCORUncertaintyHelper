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
    public partial class VariableResultGphForm : DockContent
    {
        private StatusOutputForm frmStatus;
        private ExtractData[] extractDatas;
        private RefineData[] refineDatas;
        private PlotModel plotModel;
        private bool isCheckedInterpolation;

        public VariableResultGphForm(bool isCheckedInterpolation)
        {
            InitializeComponent();

            this.frmStatus = StatusOutputForm.GetFrmStatus;
            this.extractDatas = (ExtractData[])ExtractDataManager.GetDataManager.GetExtractDatas();
            this.refineDatas = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
            this.plotModel = new PlotModel()
            {
                LegendBorder = OxyColors.Black,
                LegendBackground = OxyColor.FromAColor(32, OxyColors.Black),
                LegendPosition = LegendPosition.TopCenter,
                LegendPlacement = LegendPlacement.Outside,
                LegendOrientation = LegendOrientation.Horizontal,
            };
            this.gphResults.Model = this.plotModel;
            this.isCheckedInterpolation = isCheckedInterpolation;
        }

        private async void TsbtnSave_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                DefaultExt = "png",
                Filter = "PNG Files(*.png)|*.png",
            };
            string fileName = string.Format("{0}.png", this.TabText.ToString());
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

        public void PrintResult(string target)
        {
            if (this.isCheckedInterpolation == true)
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
            }
            else
            {
                for (var i = 0; i < this.extractDatas.Length; i++)
                {
                    var targetIdx = 0;
                    for (var j = 0; j < this.extractDatas[i].timeRecordDatas.Length; j++)
                    {
                        if (this.extractDatas[i].timeRecordDatas[j].variableName.Equals(target))
                        {
                            targetIdx = j;
                        }
                    }
                    var dataLength = this.extractDatas[i].timeRecordDatas[targetIdx].time.Length;
                    var series = new LineSeries()
                    {
                        //Title = this.extractDatas[i].fileName,
                        Color = OxyColors.DimGray,
                    };
                    for (var j = 0; j < dataLength; j++)
                    {
                        var x = this.extractDatas[i].timeRecordDatas[targetIdx].time[j];
                        var y = this.extractDatas[i].timeRecordDatas[targetIdx].value[j];
                        series.Points.Add(new DataPoint(x, y));
                    }
                    this.plotModel.Series.Add(series);
                }
            }
        }
    }
}
