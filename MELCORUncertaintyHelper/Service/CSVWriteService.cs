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
        private ExtractData[] extractDatas;
        private RefineData[] refineData;
        private DistributionData[] distributionDatas;
        private string[] variables;
        private bool isCheckedInterpolation;
        private bool isCheckedStatistics;

        public CSVWriteService(string[] variables, bool isCheckedInterpolation, bool isCheckedStatistics)
        {
            this.variables = variables;
            this.isCheckedInterpolation = isCheckedInterpolation;
            this.isCheckedStatistics = isCheckedStatistics;

            if (this.isCheckedInterpolation == true)
            {
                this.refineData = (RefineData[])RefineDataManager.GetRefineDataManager.GetRefineDatas();
                if (this.isCheckedStatistics == true)
                {
                    this.distributionDatas = (DistributionData[])DistributionDataManager.GetDistributionDataManager.GetDistributionDatas();
                }
            }
            else
            {
                this.extractDatas = (ExtractData[])ExtractDataManager.GetDataManager.GetExtractDatas();
            }
        }

        public async Task WriteFile()
        {
            await Task.Run(() =>
            {
                try
                {
                    var frmStatus = StatusOutputForm.GetFrmStatus;

                    if (this.isCheckedInterpolation == true)
                    {
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
                                        str.Append(",");
                                        break;
                                    }
                                }
                            }

                            if (this.isCheckedStatistics == true)
                            {
                                str.Append("LogNormal 5%,");
                                str.Append("LogNormal 50%,");
                                str.Append("LogNormal 95%,");
                                str.Append("LogNormal mean,");
                                str.Append("Error Factor");
                            }

                            str.AppendLine();

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
                                            str.Append(",");
                                            break;
                                        }
                                    }
                                }

                                if (this.isCheckedStatistics == true)
                                {
                                    for (var k = 0; k < this.distributionDatas.Length; k++)
                                    {
                                        var variableName = this.distributionDatas[k].variableName;
                                        if (variableName.Equals(this.variables[i]))
                                        {
                                            str.Append(this.distributionDatas[k].lognormalDistributions[j].fivePercentage);
                                            str.Append(",");
                                            str.Append(this.distributionDatas[k].lognormalDistributions[j].fiftyPercentage);
                                            str.Append(",");
                                            str.Append(this.distributionDatas[k].lognormalDistributions[j].ninetyFivePercentage);
                                            str.Append(",");
                                            str.Append(this.distributionDatas[k].lognormalDistributions[j].mean);
                                            str.Append(",");
                                            str.AppendLine(this.distributionDatas[k].lognormalDistributions[j].errorFactor.ToString());
                                            break;
                                        }
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
                    else
                    {
                        for (var i = 0; i < this.variables.Length; i++)
                        {
                            var str = new StringBuilder();

                            for (var j = 0; j < this.extractDatas.Length; j++)
                            {
                                for (var k = 0; k < this.extractDatas[j].timeRecordDatas.Length; k++)
                                {
                                    if (this.extractDatas[j].timeRecordDatas[k].variableName.Equals(this.variables[i]))
                                    {
                                        str.Append(this.extractDatas[j].fileName);
                                        str.Append(",");
                                        str.Append(",");
                                        break;
                                    }
                                }
                            }
                            str.AppendLine();

                            for (var j = 0; j < this.extractDatas.Length; j++)
                            {
                                for (var k = 0; k < this.extractDatas[j].timeRecordDatas.Length; k++)
                                {
                                    if (this.extractDatas[j].timeRecordDatas[k].variableName.Equals(this.variables[i]))
                                    {
                                        str.Append("Time,");
                                        str.Append("Value,");
                                        break;
                                    }
                                }
                            }
                            str.AppendLine();

                            var rowSize = this.FindMaxTimeLength(this.variables[i]);
                            for (var j = 0; j < rowSize; j++)
                            {
                                for (var k = 0; k < this.extractDatas.Length; k++)
                                {
                                    int idx = 0;
                                    for (var l = 0; l < this.extractDatas[k].timeRecordDatas.Length; l++)
                                    {
                                        if (this.extractDatas[k].timeRecordDatas[l].variableName.Equals(this.variables[i]))
                                        {
                                            idx = l;
                                            break;
                                        }
                                    }
                                    if (this.extractDatas[k].timeRecordDatas[idx].time.Length > j)
                                    {
                                        str.Append(this.extractDatas[k].timeRecordDatas[idx].time[j]);
                                        str.Append(",");
                                        str.Append(this.extractDatas[k].timeRecordDatas[idx].value[j]);
                                        str.Append(",");
                                    }
                                    else
                                    {
                                        str.Append(",");
                                        str.Append(",");
                                    }
                                }
                                str.AppendLine();
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
            if (this.isCheckedInterpolation == true)
            {
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
