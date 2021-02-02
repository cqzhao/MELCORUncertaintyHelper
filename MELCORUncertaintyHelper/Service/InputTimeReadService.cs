using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MELCORUncertaintyHelper.Service
{
    public class InputTimeReadService
    {
        private TimeInputData[] timeInputData;

        private InputTimeReadService()
        {

        }

        private static readonly Lazy<InputTimeReadService> inputTimeReadService = new Lazy<InputTimeReadService>(() => new InputTimeReadService());

        public static InputTimeReadService GetInputTimeReadService
        {
            get
            {
                return inputTimeReadService.Value;
            }
        }

        public Object GetInputTimes()
        {
            if (this.timeInputData == null || this.timeInputData.Length < 0)
            {
                return null;
            }
            else
            {
                return this.timeInputData.Clone();
            }
        }

        public void ExtractInput(DataGridView dgvTime)
        {
            var times = new List<TimeInputData>();

            try
            {
                for (var i = 0; i < dgvTime.Rows.Count - 1; i++)
                {
                    var time = new TimeInputData
                    {
                        timeSection = Convert.ToDouble(dgvTime[0, i].Value),
                        timeStep = Convert.ToDouble(dgvTime[1, i].Value)
                    };
                    times.Add(time);
                }
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex);
                logWrite.MakeLogFile();
            }

            if (times.Count < 0)
            {
                this.timeInputData = times.ToArray();
                return;
            }

            times.Sort((x1, x2) => x1.timeSection.CompareTo(x2.timeSection));
            this.timeInputData = times.ToArray();
        }
    }
}
