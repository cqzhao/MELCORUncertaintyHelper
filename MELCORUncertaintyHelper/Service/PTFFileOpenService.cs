using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MELCORUncertaintyHelper.Service
{
    public class PTFFileOpenService
    {
        private PTFFile[] files;

        private PTFFileOpenService()
        {

        }

        private static readonly Lazy<PTFFileOpenService> openService = new Lazy<PTFFileOpenService>(() => new PTFFileOpenService());

        public static PTFFileOpenService GetOpenService
        {
            get
            {
                return openService.Value;
            }
        }

        public object GetFiles()
        {
            if (this.files == null || this.files.Length < 1)
            {
                return null;
            }
            else
            {
                return this.files.Clone();
            }
        }

        public void OpenFiles(string[] inputFiles)
        {
            var files = new List<PTFFile>();
            try
            {
                for (var i = 0; i < inputFiles.Length; i++)
                {
                    var file = this.DivideFilePath(inputFiles[i]);
                    files.Add(file);
                }
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex);
                logWrite.MakeLogFile();
            }
            finally
            {
                this.files = files.ToArray();
            }
        }

        private PTFFile DivideFilePath(string filePath)
        {
            var file = new PTFFile
            {
                name = Path.GetFileName(filePath),
                path = Path.GetDirectoryName(filePath),
                fullPath = filePath
            };
            return file;
        }

        public void DeleteFiles()
        {
            if (MessageBox.Show("Are you sure you want to delete?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            var files = new List<PTFFile>();
            this.files = files.ToArray();
        }
    }
}
