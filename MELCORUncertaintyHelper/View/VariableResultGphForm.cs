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

namespace MELCORUncertaintyHelper.View
{
    public partial class VariableResultGphForm : DockContent
    {
        private ExtractData[] data;
        private PlotModel plotModel;

        public VariableResultGphForm()
        {
            InitializeComponent();

            this.data = (ExtractData[])ExtractDataManager.GetDataManager.GetExtractDatas();
            this.plotModel = new PlotModel();
        }

        private void VariableResultGphForm_Load(object sender, EventArgs e)
        {
            /*var axesX = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                AxislineStyle = LineStyle.Solid,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
                PositionAtZeroCrossing = true,
            };

            var axesY = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                AxislineStyle = LineStyle.Solid,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
                PositionAtZeroCrossing = true,
            };

            this.plotModel.Axes.Add(axesX);
            this.plotModel.Axes.Add(axesY);*/

            this.gphResults.Model = this.plotModel;
        }

        public void PrintResult(string target)
        {
            for (var i = 0; i < this.data.Length; i++)
            {
                var targetIdx = 0;
                for (var j = 0; j < this.data[i].timeRecordDatas.Length; j++)
                {
                    if (this.data[i].timeRecordDatas[j].variableName.Equals(target))
                    {
                        targetIdx = j;
                    }
                }
                var dataLength = this.data[i].timeRecordDatas[targetIdx].time.Length;
                var series = new LineSeries()
                {
                    Title = this.data[i].fileName,
                };
                for (var j = 0; j < dataLength; j++)
                {
                    var x = this.data[i].timeRecordDatas[targetIdx].time[j];
                    var y = this.data[i].timeRecordDatas[targetIdx].value[j];
                    series.Points.Add(new DataPoint(x, y));
                }
                this.plotModel.Series.Add(series);
            }
        }
    }
}
