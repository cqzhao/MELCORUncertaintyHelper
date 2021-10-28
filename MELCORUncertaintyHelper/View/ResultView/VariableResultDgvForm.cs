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
    public partial class VariableResultDgvForm : DockContent
    {
        private ExtractData[] extractDatas;
        private RefineData[] refineDatas;
        private bool isCheckedInterpolation;

        public VariableResultDgvForm(bool isCheckedInterpolation)
        {
            InitializeComponent();

            this.isCheckedInterpolation = isCheckedInterpolation;

            this.extractDatas = (ExtractData[])ExtractDataManager.GetDataManager.GetExtractDatas();
            if (this.isCheckedInterpolation == true)
            {
                this.refineDatas = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
            }
        }

        private void VariableResultForm_Load(object sender, EventArgs e)
        {
            if (this.isCheckedInterpolation == true)
            {
                for (var i = 0; i < this.refineDatas.Length; i++)
                {
                    this.dgvResults.Columns.Add(this.refineDatas[i].fileName, "Time");
                    this.dgvResults.Columns.Add(this.refineDatas[i].fileName, "Value");
                }
            }
            else
            {
                for (var i = 0; i < this.extractDatas.Length; i++)
                {
                    this.dgvResults.Columns.Add(this.extractDatas[i].fileName, "Time");
                    this.dgvResults.Columns.Add(this.extractDatas[i].fileName, "Value");
                }
            }
            for (var i = 0; i < this.dgvResults.Columns.Count; i++)
            {
                this.dgvResults.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dgvResults.ColumnHeadersHeight *= 2;
        }

        private void DgvResults_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            var header = this.dgvResults.DisplayRectangle;
            header.Height = this.dgvResults.ColumnHeadersHeight / 2;
            this.dgvResults.Invalidate(header);
        }

        private void DgvResults_Scroll(object sender, ScrollEventArgs e)
        {
            var header = this.dgvResults.DisplayRectangle;
            header.Height = this.dgvResults.ColumnHeadersHeight / 2;
            this.dgvResults.Invalidate(header);
        }

        private void DgvResults_Paint(object sender, PaintEventArgs e)
        {
            var files = new List<string>();
            if (this.isCheckedInterpolation == true)
            {
                for (var i = 0; i < this.refineDatas.Length; i++)
                {
                    files.Add(this.refineDatas[i].fileName);
                }
            }
            else
            {
                for (var i = 0; i < this.extractDatas.Length; i++)
                {
                    files.Add(this.extractDatas[i].fileName);
                }
            }
            for (var i = 0; i < files.Count * 2; i += 2)
            {
                var rectangle = this.dgvResults.GetCellDisplayRectangle(i, -1, true);
                var width = this.dgvResults.GetCellDisplayRectangle(i + 1, -1, true).Width;

                rectangle.X += 1;
                rectangle.Y += 1;
                rectangle.Width = rectangle.Width + width - 2;
                rectangle.Height = rectangle.Height / 2 - 2;
                e.Graphics.FillRectangle(new SolidBrush(this.dgvResults.ColumnHeadersDefaultCellStyle.BackColor), rectangle);
                var strFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString(files[i / 2], this.dgvResults.ColumnHeadersDefaultCellStyle.Font,
                    new SolidBrush(this.dgvResults.ColumnHeadersDefaultCellStyle.ForeColor), rectangle, strFormat);
            }
        }

        private void DgvResults_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                var rectangle = e.CellBounds;
                rectangle.Y += e.CellBounds.Height / 2;
                rectangle.Height = e.CellBounds.Height / 2;
                e.PaintBackground(rectangle, true);
                e.PaintContent(rectangle);
                e.Handled = true;
            }
        }

        private void DgvResults_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.FillWeight = 10;
        }

        public void PrintResult(string target)
        {
            var rowSize = this.FindMaxTimeLength(target);
            if (this.isCheckedInterpolation == true)
            {
                for (var i = 0; i < rowSize; i++)
                {
                    this.dgvResults.Rows.Add();
                    var values = new List<string>();
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
                        if (this.refineDatas[j].timeRecordDatas[idx].time.Length > i)
                        {
                            values.Add(this.refineDatas[j].timeRecordDatas[idx].time[i].ToString());
                            values.Add(this.refineDatas[j].timeRecordDatas[idx].value[i].ToString());
                        }
                        else
                        {
                            values.Add(null);
                            values.Add(null);
                        }
                    }
                    for (var j = 0; j < values.Count; j++)
                    {
                        this.dgvResults[j, i].Value = values[j];
                    }
                }
            }
            else
            {
                for (var i = 0; i < rowSize; i++)
                {
                    this.dgvResults.Rows.Add();
                    var values = new List<string>();
                    for (var j = 0; j < this.extractDatas.Length; j++)
                    {
                        int idx = 0;
                        for (var k = 0; k < this.extractDatas[j].timeRecordDatas.Length; k++)
                        {
                            var variable = this.extractDatas[j].timeRecordDatas[k].variableName;
                            if (variable.Equals(target))
                            {
                                idx = k;
                                break;
                            }
                        }
                        if (this.extractDatas[j].timeRecordDatas[idx].time.Length > i)
                        {
                            values.Add(this.extractDatas[j].timeRecordDatas[idx].time[i].ToString());
                            values.Add(this.extractDatas[j].timeRecordDatas[idx].value[i].ToString());
                        }
                        else
                        {
                            values.Add(null);
                            values.Add(null);
                        }
                    }
                    for (var j = 0; j < values.Count; j++)
                    {
                        this.dgvResults[j, i].Value = values[j];
                    }
                }
            }
        }

        private int FindMaxTimeLength(string target)
        {
            var max = Int32.MinValue;
            if (this.isCheckedInterpolation == true)
            {
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
            }
            else
            {
                for (var i = 0; i < this.extractDatas.Length; i++)
                {
                    for (var j = 0; j < this.extractDatas[i].timeRecordDatas.Length; j++)
                    {
                        if (this.extractDatas[i].timeRecordDatas[j].variableName.Equals(target))
                        {
                            var tmp = this.extractDatas[i].timeRecordDatas[j].time.Length;
                            if (tmp > max)
                            {
                                max = tmp;
                            }
                        }
                    }
                }
            }
            return max;
        }
    }
}
