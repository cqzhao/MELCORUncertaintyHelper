using MELCORUncertaintyHelper.Manager;
using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Service
{
    public class RefineDataProcessService
    {
        private ExtractData[] extractDatas;
        private RefineData[] refineDatas;
        private double[] interpolationTimes;
        private RefineDataManager refineDataManager;
        private InterpolationService interpolationService;

        public RefineDataProcessService()
        {

        }

        public void Refine()
        {
            this.extractDatas = (ExtractData[])ExtractDataManager.GetDataManager.GetExtractDatas();
            if (this.extractDatas == null || this.extractDatas.Length < 0)
            {
                return;
            }
            this.SetRefineData();
            this.PreProcessInterpolation();

            this.refineDataManager = RefineDataManager.GetRefineDataManager;
            this.refineDataManager.SetRefineDatas(this.refineDatas.Clone());

            this.interpolationService = new InterpolationService();
            this.interpolationService.Interpolation();
        }

        private void SetRefineData()
        {
            this.interpolationTimes = (double[])InputTimeReadService.GetInputTimeReadService.GetTimes();
            if (this.extractDatas == null || this.extractDatas.Length < 0)
            {
                return;
            }

            var dataLength = this.extractDatas.Length;
            this.refineDatas = new RefineData[dataLength];

            for (var i = 0; i < dataLength; i++)
            {
                var timeRecords = new TimeRecordData[this.extractDatas[i].inputVariables.Length];

                for (var j = 0; j < this.extractDatas[i].inputVariables.Length; j++)
                {
                    var recordData = new TimeRecordData
                    {
                        variableName = this.extractDatas[i].inputVariables[j],
                        time = this.interpolationTimes,
                    };
                    timeRecords[j] = recordData;
                }

                var data = new RefineData
                {
                    fileName = this.extractDatas[i].fileName,
                    inputVariables = this.extractDatas[i].inputVariables,
                    timeRecordDatas = timeRecords.ToArray()
                };

                this.refineDatas[i] = data;
            }
        }

        private void PreProcessInterpolation()
        {
            var dataLength = this.extractDatas.Length;
            for (var i = 0; i < dataLength; i++)
            {
                var timeRecordLength = this.extractDatas[i].timeRecordDatas.Length;
                for (var j = 0; j < timeRecordLength; j++)
                {
                    var lastTime = this.extractDatas[i].timeRecordDatas[j].time.Last();
                    var lastInterpolationTime = this.interpolationTimes.Last();
                    if (lastTime < lastInterpolationTime)
                    {
                        var nearIdx = this.FindNearTimeIdx(lastTime);
                        var timeLength = this.extractDatas[i].timeRecordDatas[j].time.Length;
                        var additionalTimeLength = this.interpolationTimes.Length - nearIdx;
                        var newTimeLength = timeLength + additionalTimeLength;
                        var newTimes = new double[newTimeLength];
                        var newValues = new double[newTimeLength];

                        Array.Copy(this.extractDatas[i].timeRecordDatas[j].time, newTimes, timeLength);
                        Array.Copy(this.interpolationTimes, nearIdx, newTimes, timeLength, additionalTimeLength);

                        var lastValue = this.extractDatas[i].timeRecordDatas[j].value.Last();
                        var additionalValues = Enumerable.Repeat<double>(lastValue, additionalTimeLength).ToArray<double>();
                        Array.Copy(this.extractDatas[i].timeRecordDatas[j].value, newValues, timeLength);
                        Array.Copy(additionalValues, 0, newValues, timeLength, additionalTimeLength);

                        this.extractDatas[i].timeRecordDatas[j].time = new double[newTimeLength];
                        this.extractDatas[i].timeRecordDatas[j].time = newTimes;
                        this.extractDatas[i].timeRecordDatas[j].value = new double[newTimeLength];
                        this.extractDatas[i].timeRecordDatas[j].value = newValues;
                    }
                }
            }
            ExtractDataManager.GetDataManager.UpdateData(this.extractDatas.Clone());
        }

        private int FindNearTimeIdx(double lastTime)
        {
            var interpolationTimeLength = this.interpolationTimes.Length;
            var min = Double.MaxValue;
            var idx = 0;
            for (var i = 0; i < interpolationTimeLength; i++)
            {
                var abs = Math.Abs(this.interpolationTimes[i] - lastTime);
                if (abs < min)
                {
                    min = abs;
                    idx = i;
                }
            }
            return idx;
        }
    }
}
