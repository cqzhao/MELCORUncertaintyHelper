using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
using OxyPlot;
using OxyPlot.Axes;
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
    public partial class VariableResultGphForm : DockContent
    {
        private ExtractData[] extractDatas;
        private RefineData[] refineDatas;
        private PlotModel plotModel;
        private bool isCheckedInterpolation;

        public VariableResultGphForm(bool isCheckedInterpolation)
        {
            InitializeComponent();

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
                        Title = this.refineDatas[i].fileName,
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
