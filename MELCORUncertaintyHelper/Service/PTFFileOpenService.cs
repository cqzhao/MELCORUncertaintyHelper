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
                /*
                 * 기존에 불러온 파일이 존재한다면
                 * 기존에 존재하던 파일에
                 * 추가적으로 불러올 파일도 같이 저장하기 위하여
                 */
                if (this.files != null && this.files.Length > 0)
                {
                    files = this.files.ToList();
                }

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
