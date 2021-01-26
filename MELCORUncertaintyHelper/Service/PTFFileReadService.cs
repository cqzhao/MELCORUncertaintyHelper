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
        // 각 Package의 Control Volume 및 변수 단위
        private string[] packageUnits;
        // 각 Package에 존재하는 Control Volume
        private int[] controlVolumes;
        // SPecial Section에 존재하는 변수 정보들
        private string[] variableInfos;
        // SPecial Section과 Time Records Section을 구분하기 위한 string
        private string sptrStr = null;
        // SPecial Section Marker
        private static string spMarker = ".SP/";
        // Time Records Section Marker
        private static string trMarker = ".TR/";

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
                this.ReadSPecialSection(fileStream, this.files[nth].name);
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
                        var leftDelimiter = binaryReader.ReadInt32();
                        var tmp = new char[leftDelimiter];
                        binaryReader.Read(tmp, 0, leftDelimiter);
                        var str = new string(tmp).Trim();
                        var rightDelimiter = binaryReader.ReadInt32();
                        if (leftDelimiter != rightDelimiter)
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
                        var leftDelimiter = binaryReader.ReadInt32();

                        this.totalPackageCnt = binaryReader.ReadInt32();
                        this.totalVariableCnt = binaryReader.ReadInt32();

                        var rightDelimiter = binaryReader.ReadInt32();

                        if (leftDelimiter != rightDelimiter)
                        {
                            // Reader can read file abnormally
                        }
                        break;
                    }

                    // Package Name 읽기
                    var packageNames = new List<string>();
                    while (true)
                    {
                        var leftDelimiter = binaryReader.ReadInt32();

                        var packageNameLength = leftDelimiter / this.totalPackageCnt;
                        var str = new char[packageNameLength];
                        for (var i = 0; i < this.totalPackageCnt; i++)
                        {
                            binaryReader.Read(str, 0, packageNameLength);
                            var name = new string(str).Trim();
                            packageNames.Add(name);
                        }

                        var rightDelimiter = binaryReader.ReadInt32();

                        if (leftDelimiter != rightDelimiter)
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
                        var leftDelimiter = binaryReader.ReadInt32();

                        for (var i = 0; i < this.totalPackageCnt; i++)
                        {
                            var num = binaryReader.ReadInt32();
                            packageVariableCnt.Add(num);
                        }
                        packageVariableCnt.Add(packageVariableCnt[this.totalPackageCnt - 1] + 1);

                        var rightDelimiter = binaryReader.ReadInt32();

                        if (leftDelimiter != rightDelimiter)
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
                        var leftDelimiter = binaryReader.ReadInt32();

                        // 단위는 16 byte 씩 읽어야 하므로
                        var strLength = 16;
                        var str = new char[strLength];
                        for (var i = 0; i < leftDelimiter / strLength; i++)
                        {
                            binaryReader.Read(str, 0, strLength);
                            var unit = new string(str).Trim();
                            if (string.IsNullOrEmpty(unit))
                            {
                                unit = "-";
                            }
                            packageUnits.Add(unit);
                        }

                        var rightDelimiter = binaryReader.ReadInt32();

                        if (leftDelimiter != rightDelimiter)
                        {
                            // Reader can read file abnormally
                        }
                        break;
                    }
                    this.packageUnits = packageUnits.ToArray();

                    // 각 Package에 존재하는 Control Volume들을 순차적으로 읽고 저장
                    var controlVolumes = new List<int>();
                    while (true)
                    {
                        var leftDelimiter = binaryReader.ReadInt32();

                        for (var i = 0; i < this.totalVariableCnt; i++)
                        {
                            var volume = binaryReader.ReadInt32();
                            controlVolumes.Add(volume);
                        }

                        var rightDelimiter = binaryReader.ReadInt32();

                        if (leftDelimiter != rightDelimiter)
                        {
                            // Reader can read file abnormally
                        }
                        break;
                    }
                    this.controlVolumes = controlVolumes.ToArray();
                }
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex, fileName);
                logWrite.MakeLogFile();
            }
        }

        /// <summary>
        /// SPecial Section은 시간에 독립적인, 비시각화 변수 정보들이 존재
        /// The Specials Section contains time-independent, non-Plot Variable information.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName"></param>
        private int ReadSPecialSection(FileStream fileStream, string fileName)
        {
            var lastLeftDelimiter = 0;

            try
            {
                using (var binaryReader = new BinaryReader(fileStream, Encoding.UTF8, true))
                {
                    var variableInfos = new List<string>();
                    while (true)
                    {
                        var leftDelimiter = binaryReader.ReadInt32();

                        if (leftDelimiter == 4)
                        {
                            var str = new char[leftDelimiter];
                            binaryReader.Read(str, 0, leftDelimiter);
                            this.sptrStr = new string(str);
                        }
                        else
                        {
                            if (this.sptrStr.Equals(spMarker))
                            {
                                var str = new char[leftDelimiter];
                                binaryReader.Read(str, 0, leftDelimiter);
                                var info = new string(str).Trim();
                                if (info.Contains("NAME"))
                                {
                                    variableInfos.Add(info);
                                }
                            }
                            else if (this.sptrStr.Equals(trMarker))
                            {
                                lastLeftDelimiter = leftDelimiter;
                                break;
                            }
                        }

                        var rightDelimiter = binaryReader.ReadInt32();

                        if (leftDelimiter != rightDelimiter)
                        {
                            // Reader can read file abnormally
                        }
                    }
                    this.variableInfos = variableInfos.ToArray();
                }
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex, fileName);
                logWrite.MakeLogFile();
            }

            return lastLeftDelimiter;
        }

        private void ReadTimeRecordsSection(FileStream fileStream, string fileName, int lastLeftDelimiter)
        {
            var isVisited = false;
            var leftDelimiter = lastLeftDelimiter;
            var rightDelimiter = 0;

            try
            {
                using (var binaryReader = new BinaryReader(fileStream, Encoding.UTF8, true))
                {
                    while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                    {
                        if (isVisited == true)
                        {
                            leftDelimiter = binaryReader.ReadInt32();
                        }

                        if (leftDelimiter == 4)
                        {
                            var str = new char[leftDelimiter];
                            binaryReader.Read(str, 0, leftDelimiter);
                            this.sptrStr = new string(str);
                        }
                        else
                        {
                            if (this.sptrStr.Equals(trMarker))
                            {

                            }
                        }

                        rightDelimiter = binaryReader.ReadInt32();
                        if (leftDelimiter != rightDelimiter)
                        {
                            // Reader can read file abnormally
                        }
                        isVisited = true;
                    }
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
