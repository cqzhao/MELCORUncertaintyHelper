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
    }
}
