using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Service
{
    public class LogFileWriteService
    {
        private Exception exception;

        public LogFileWriteService(Exception ex)
        {
            this.exception = ex;
        }

        public void MakeLogFile()
        {
            var msg = new StringBuilder();
            msg.Append(this.exception.ToString());

            var fileName = new StringBuilder();
            fileName.Append(DateTime.Now.ToString("yyyyMMddHHmms"));
            fileName.Append("_Log.txt");

            var filePath = Path.Combine(Environment.CurrentDirectory, "log", fileName.ToString());
            File.WriteAllText(filePath, msg.ToString());
        }
    }
}
