using MELCORUncertaintyHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MELCORUncertaintyHelper.Service
{
    public class PTFFileReadService
    {
        private PTFFile[] files;
        // PTF File에 존재하는 Package 총 개수
        private int totalPackageCnt;
        // 전체 Package에 존재하는 변수 및 Control Volume의 총 개수
        private int totalVariableCnt;
        // PTF FIle에 존재하는 Package
        private string[] packageNames;
        // 각 Package에 존재하는 변수 및 Control Volume의 개수
        private int[] packageVariableCnt;
        // 각 Package의 Control Voluem 및 변수 단위
        private string[] packageUnits;

        public PTFFileReadService(PTFFile[] files)
        {
            this.files = files;
        }

        public void Read()
        {
            for (var i = 0; i < this.files.Length; i++)
            {
                var thread = new Thread(() => this.ReadFile(i));
                thread.Start();
                thread.Join();
            }
        }

        private void ReadFile(int nth)
        {
            using (var fileStream = new FileStream(this.files[nth].path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                this.ReadTitleSection(fileStream, this.files[nth].name);
                this.ReadPackageSection(fileStream, this.files[nth].name);
            }
        }

        private void ReadTitleSection(FileStream fileStream, string fileName)
        {
            try
            {
                using (var binaryReader = new BinaryReader(fileStream, Encoding.UTF8, true))
                {
                    // Read Header
                    while (true)
                    {
                        var startByte = binaryReader.ReadInt32();
                        var tmp = new char[startByte];
                        binaryReader.Read(tmp, 0, startByte);
                        var str = new string(tmp).Trim();
                        var endByte = binaryReader.ReadInt32();
                        if (startByte != endByte)
                        {
                            // Reader can read file abnormally
                        }
                        if (str.Equals("KEY"))
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex, fileName);
                logWrite.MakeLogFile();
            }
        }

        private void ReadPackageSection(FileStream fileStream, string fileName)
        {
            try
            {
                using (var binaryReader = new BinaryReader(fileStream, Encoding.UTF8, true))
                {
                    // 전체 Package 수와 전체 Package에 존재하는 변수 및 Control Volume의 총 개수
                    while (true)
                    {
                        var startByte = binaryReader.ReadInt32();

                        this.totalPackageCnt = binaryReader.ReadInt32();
                        this.totalVariableCnt = binaryReader.ReadInt32();

                        var endByte = binaryReader.ReadInt32();

                        if (startByte != endByte)
                        {
                            // Reader can read file abnormally
                        }
                        break;
                    }

                    // Package Name 읽기
                    var packageNames = new List<string>();
                    while (true)
                    {
                        var startByte = binaryReader.ReadInt32();

                        var packageNameLength = startByte / this.totalPackageCnt;
                        var str = new char[packageNameLength];
                        for (var i = 0; i < this.totalPackageCnt; i++)
                        {
                            binaryReader.Read(str, 0, packageNameLength);
                            var name = new string(str).Trim();
                            packageNames.Add(name);
                        }

                        var endByte = binaryReader.ReadInt32();

                        if (startByte != endByte)
                        {
                            // Reader can read file abnormally
                        }
                        break;
                    }
                    this.packageNames = packageNames.ToArray();

                    // 각 Package에 존재하는 변수 및 Control Volume의 개수 읽기
                    var packageVariableCnt = new List<int>();
                    while (true)
                    {
                        var startByte = binaryReader.ReadInt32();

                        for (var i = 0; i < this.totalPackageCnt; i++)
                        {
                            var num = binaryReader.ReadInt32();
                            packageVariableCnt.Add(num);
                        }
                        packageVariableCnt.Add(packageVariableCnt[this.totalPackageCnt - 1] + 1);

                        var endByte = binaryReader.ReadInt32();

                        if (startByte != endByte)
                        {
                            // Reader can read file abnormally
                        }
                        break;
                    }
                    this.packageVariableCnt = packageVariableCnt.ToArray();

                    // 각 Package의 Control Voluem 및 변수 단위 읽기
                    var packageUnits = new List<string>();
                    while (true)
                    {
                        var startByte = binaryReader.ReadInt32();

                        // 단위는 16 byte 씩 읽어야 하므로
                        var strLength = 16;
                        var str = new char[strLength];
                        for (var i = 0; i < startByte / strLength; i++)
                        {
                            binaryReader.Read(str, 0, strLength);
                            var unit = new string(str).Trim();
                            if (string.IsNullOrEmpty(unit))
                            {
                                unit = "-";
                            }
                            packageUnits.Add(unit);
                        }

                        var endByte = binaryReader.ReadInt32();

                        if (startByte != endByte)
                        {
                            // Reader can read file abnormally
                        }
                        break;
                    }
                    this.packageUnits = packageUnits.ToArray();
                }
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex, fileName);
                logWrite.MakeLogFile();
            }
        }
    }
}
