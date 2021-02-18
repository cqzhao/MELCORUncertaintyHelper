using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
using MELCORUncertaintyHelper.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Service
{
    public class CSVWriteService
    {
        private RefineData[] refineData;
        private string[] variables;

        public CSVWriteService(string[] variables)
        {
            this.refineData = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
            this.variables = variables;
        }

        public async Task WriteFile()
        {
            await Task.Run(() =>
            {
                try
                {
                    var frmStatus = StatusOutputForm.GetFrmStatus;

                    for (var i = 0; i < this.variables.Length; i++)
                    {
                        var str = new StringBuilder();
                        str.Append("Time,");
                        for (var j = 0; j < this.refineData.Length; j++)
                        {
                            for (var k = 0; k < this.refineData[j].timeRecordDatas.Length; k++)
                            {
                                if (this.refineData[j].timeRecordDatas[k].variableName.Equals(this.variables[i]))
                                {
                                    str.Append(this.refineData[j].fileName);
                                    break;
                                }
                            }
                            if (j < this.refineData.Length - 1)
                            {
                                str.Append(",");
                            }
                            else
                            {
                                str.AppendLine();
                            }
                        }

                        var rowSize = this.FindMaxTimeLength(this.variables[i]);
                        for (var j = 0; j < rowSize; j++)
                        {
                            for (var k = 0; k < this.refineData.Length; k++)
                            {
                                for (var l = 0; l < this.refineData[k].timeRecordDatas.Length; l++)
                                {
                                    if (this.refineData[k].timeRecordDatas[l].variableName.Equals(this.variables[i]))
                                    {
                                        if (k == 0)
                                        {
                                            str.Append(this.refineData[k].timeRecordDatas[l].time[j]);
                                            str.Append(",");
                                        }
                                        str.Append(this.refineData[k].timeRecordDatas[l].value[j]);
                                        break;
                                    }
                                }
                                if (k < this.refineData.Length - 1)
                                {
                                    str.Append(",");
                                }
                                else
                                {
                                    str.AppendLine();
                                }
                            }
                        }

                        File.WriteAllText(this.variables[i] + ".csv", str.ToString());

                        var statusMsg = new StringBuilder();
                        statusMsg.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
                        statusMsg.Append("File ");
                        statusMsg.Append(this.variables[i]);
                        statusMsg.AppendLine(".csv is created");
                        frmStatus.PrintStatus(statusMsg);
                    }
                }
                catch (Exception ex)
                {
                    var logWrite = new LogFileWriteService(ex);
                    logWrite.MakeLogFile();
                    return;
                }
            });
        }

        private int FindMaxTimeLength(string target)
        {
            var max = Int32.MinValue;
            for (var i = 0; i < this.refineData.Length; i++)
            {
                for (var j = 0; j < this.refineData[i].timeRecordDatas.Length; j++)
                {
                    if (this.refineData[i].timeRecordDatas[j].variableName.Equals(target))
                    {
                        var tmp = this.refineData[i].timeRecordDatas[j].time.Length;
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
