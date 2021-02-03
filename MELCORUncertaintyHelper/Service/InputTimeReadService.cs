using MELCORUncertaintyHelper.Model;
using MELCORUncertaintyHelper.View;
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
        private double[] times;

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

        public Object GetTimes()
        {
            if (this.times == null || this.times.Length < 0)
            {
                return null;
            }
            else
            {
                return this.times.Clone();
            }
        }

        public void ExtractTime()
        {
            var frmTimeInput = TimeInputForm.GetFrmTimeInupt;
            this.ExtractInput(frmTimeInput.GetDgvTime());
            this.SetInterpolationTime();
        }

        private void ExtractInput(DataGridView dgvTime)
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

        private void SetInterpolationTime()
        {
            var times = new List<double>();
            try
            {
                for (var i = 0; i < this.timeInputData.Length - 1; i++)
                {
                    for (var j = this.timeInputData[i].timeSection; j < this.timeInputData[i + 1].timeSection; j += this.timeInputData[i].timeStep)
                    {
                        times.Add(j);
                    }
                }
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex);
                logWrite.MakeLogFile();
            }

            if (times.Count < 0)
            {
                this.times = times.ToArray();
                return;
            }

            this.times = times.ToArray();
        }
    }
}
