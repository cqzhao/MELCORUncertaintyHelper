using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Manager
{
    public class ExtractDataManager
    {
        private ExtractData[] extractDatas;

        private ExtractDataManager()
        {

        }

        private static readonly Lazy<ExtractDataManager> dataManager = new Lazy<ExtractDataManager>(() => new ExtractDataManager());

        public static ExtractDataManager GetDataManager
        {
            get
            {
                return dataManager.Value;
            }
        }

        public Object GetExtractDatas() => this.extractDatas.Clone();

        public void AddData(string fileName, string[] inputVariables, TimeRecordData[] timeRecordDatas)
        {
            var datas = new List<ExtractData>();

            if (this.extractDatas != null && this.extractDatas.Length > 0)
            {
                datas = this.extractDatas.ToList();
            }

            var data = new ExtractData
            {
                fileName = fileName,
                inputVariables = inputVariables,
                timeRecordDatas = timeRecordDatas,
            };
            datas.Add(data);

            this.extractDatas = datas.ToArray();
        }

        public void InitializeData()
        {
            var datas = new List<ExtractData>();
            this.extractDatas = datas.ToArray();
        }
    }
}
