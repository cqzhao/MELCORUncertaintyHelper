using MELCORUncertaintyHelper.Manager;
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
        private PTFFile file;
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

        private string[] inputs;
        private int[] totalIdxes;

        public PTFFileReadService(PTFFile file)
        {
            this.file = file;
        }

        public void Read()
        {
            this.ReadFile();
        }

        private void ReadFile()
        {
            using (var fileStream = new FileStream(this.file.fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                this.ReadTitleSection(fileStream, this.file.name);
                this.ReadPackageSection(fileStream, this.file.name);
                var lastLeftDelimiter = this.ReadSPecialSection(fileStream, this.file.name);
                var inputService = InputVariableReadService.GetInputReadService;
                inputService.MakeIndexes(this.packageNames, this.packageVariableCnt, this.controlVolumes);
                this.inputs = (string[])inputService.GetInputs();
                this.totalIdxes = (int[])inputService.GetTotalIdxes();
                var timeRecordDatas = (TimeRecordData[])this.ReadTimeRecordsSection(fileStream, this.file.name, lastLeftDelimiter);
                var dataManager = ExtractDataManager.GetDataManager;
                dataManager.AddData(this.file.name, this.inputs, timeRecordDatas);
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
                    this.packageVariableCnt[this.packageVariableCnt.Length - 1] = this.controlVolumes.Length + 1;
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

        private Object ReadTimeRecordsSection(FileStream fileStream, string fileName, int lastLeftDelimiter)
        {
            var isVisited = false;
            var leftDelimiter = lastLeftDelimiter;
            int rightDelimiter;
            int lineDataLen;

            var dataIdx = new int[this.inputs.Length + 1];
            var pointing = new int[this.inputs.Length + 1];

            dataIdx[0] = 0;
            for (var i = 0; i < this.totalIdxes.Length; i++)
            {
                dataIdx[i + 1] = this.totalIdxes[i] + 4;
            }
            Array.Sort(dataIdx);

            pointing[0] = 0;
            for (var i = 0; i < this.totalIdxes.Length; i++)
            {
                pointing[i + 1] = Array.IndexOf(dataIdx, this.totalIdxes[i] + 4);
            }

            var lineData = new List<double>();
            var timeData = new List<double>();
            var valueData = new List<List<double>>();

            for (var i = 0; i < this.inputs.Length; i++)
            {
                valueData.Add(new List<double>());
            }

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
                                lineData.Clear();
                                lineDataLen = this.totalVariableCnt + 4;
                                if (lineDataLen != (leftDelimiter / 4))
                                {
                                    // Number of data does not match
                                }

                                var time = binaryReader.ReadSingle();
                                lineData.Add(time);
                                timeData.Add(time);

                                for (var i = 0; i < this.inputs.Length; i++)
                                {
                                    if (dataIdx[i + 1] != dataIdx[i])
                                    {
                                        var intervalSkip = new byte[(dataIdx[i + 1] - dataIdx[i] - 1) * 4];
                                        binaryReader.Read(intervalSkip, 0, (dataIdx[i + 1] - dataIdx[i] - 1) * 4);
                                        var data = binaryReader.ReadSingle();
                                        lineData.Add(data);
                                    }
                                    else
                                    {
                                        lineData.Add(0.0);
                                    }
                                }

                                var finalSkip = new byte[(lineDataLen - dataIdx[this.inputs.Length] - 1) * 4];
                                binaryReader.Read(finalSkip, 0, (lineDataLen - dataIdx[this.inputs.Length] - 1) * 4);

                                for (var i = 0; i < pointing.Length; i++)
                                {
                                    if (i != 0)
                                    {
                                        var value = lineData[i];
                                        valueData[i - 1].Add(value);
                                    }
                                }
                            }
                            else
                            {
                                var str = new char[4];
                                binaryReader.Read(str, 0, 4);
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

            var timeRecordDatas = new List<TimeRecordData>();
            for (var i = 0; i < this.inputs.Length; i++)
            {
                var recordData = new TimeRecordData
                {
                    variableName = this.inputs[i],
                    time = timeData.ToArray(),
                    value = valueData[i].ToArray(),
                };
                timeRecordDatas.Add(recordData);
            }

            return timeRecordDatas.ToArray().Clone();
        }
    }
}
