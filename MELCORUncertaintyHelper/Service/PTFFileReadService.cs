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
        private int leftDelimiter;
        private int rightDelimiter;
        private int plotKeyCnt;
        private int plotVarCnt;
        private string[] plotKeys;
        private int[] offsets;
        private string[] units;
        private int[] indexes;
        private InputVariableReadService inputVariableReader;
        private string[] inputVariables;
        private string[] inputPlotKeys;
        private int[] inputIndexes;
        private int[] inputTRIndexes;
        private ExtractDataManager dataManager;
        private TimeRecordData[] timeRecordData;

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
        private static readonly string spMarker = ".SP/";
        // Time Records Section Marker
        private static readonly string trMarker = ".TR/";

        private string[] inputs;
        private int[] totalIdxes;

        public PTFFileReadService(PTFFile file)
        {
            this.file = file;
            this.inputVariableReader = InputVariableReadService.GetInputReadService;
            this.dataManager = ExtractDataManager.GetDataManager;
        }

        public void Read()
        {
            //this.ReadFile();
            try
            {
                using (var fileStream = new FileStream(this.file.fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    this.ReadHeader(fileStream);
                    this.ReadSpecials(fileStream);
                    this.InputProcess();
                    this.ReadTimeRecords(fileStream);
                    this.dataManager.AddData(this.file.name, this.inputVariables, this.timeRecordData);
                }
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex);
                logWrite.MakeLogFile();
            }
        }

        private void InputProcess()
        {
            this.inputVariableReader.FindInputTRIndexes(this.plotKeys, this.offsets, this.indexes);

            this.inputVariables = (string[])this.inputVariableReader.GetInputVariables();
            this.inputPlotKeys = (string[])this.inputVariableReader.GetInputPlotKeys();
            this.inputIndexes = (int[])this.inputVariableReader.GetInputIndexes();
            this.inputTRIndexes = (int[])this.inputVariableReader.GetInputTRIndexes();
        }

        private void ReadFile()
        {
            using (var fileStream = new FileStream(this.file.fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                this.ReadTitleSection(fileStream, this.file.name);
                this.ReadPackageSection(fileStream, this.file.name);
                var lastLeftDelimiter = this.ReadSPecialSection(fileStream, this.file.name);
                try
                {
                    inputVariableReader.MakeIndexes(this.packageNames, this.packageVariableCnt, this.controlVolumes);
                    this.inputs = (string[])inputVariableReader.GetInputVariables();
                    this.totalIdxes = (int[])inputVariableReader.GetTotalIdxes();
                }
                catch (Exception ex)
                {
                    var logWrite = new LogFileWriteService(ex);
                    logWrite.MakeLogFile();
                }
                var timeRecordDatas = (TimeRecordData[])this.ReadTimeRecordsSection(fileStream, this.file.name, lastLeftDelimiter);
                this.dataManager.AddData(this.file.name, this.inputs, timeRecordDatas);
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

        private void ReadHeader(FileStream fileStream)
        {
            using (var binaryReader = new BinaryReader(fileStream, Encoding.UTF8, true))
            {
                // Title of Header
                while (true)
                {
                    this.leftDelimiter = binaryReader.ReadInt32();
                    var buf = new char[this.leftDelimiter];
                    binaryReader.Read(buf, 0, this.leftDelimiter);
                    var str = new string(buf).Trim();
                    this.rightDelimiter = binaryReader.ReadInt32();

                    if (str.Equals("KEY"))
                    {
                        break;
                    }
                }

                /*
                 * Key of Header
                 */

                // Read <PlotKeyCount><PlotVariableCount>
                while (true)
                {
                    this.leftDelimiter = binaryReader.ReadInt32();
                    this.plotKeyCnt = binaryReader.ReadInt32();
                    this.plotVarCnt = binaryReader.ReadInt32();
                    this.rightDelimiter = binaryReader.ReadInt32();
                    break;
                }

                // Read <PlotKeys>
                var plotKeys = new List<string>();
                while (true)
                {
                    this.leftDelimiter = binaryReader.ReadInt32();
                    var plotKeysLength = this.leftDelimiter / this.plotKeyCnt;
                    var buf = new char[plotKeysLength];
                    for (var i = 0; i < this.plotKeyCnt; i++)
                    {
                        binaryReader.Read(buf, 0, plotKeysLength);
                        var plotKey = new string(buf).Trim();
                        plotKeys.Add(plotKey);
                    }
                    this.rightDelimiter = binaryReader.ReadInt32();
                    break;
                }
                this.plotKeys = plotKeys.ToArray();

                // Read <Offsets>
                var offsets = new List<int>();
                while (true)
                {
                    this.leftDelimiter = binaryReader.ReadInt32();
                    for (var i = 0; i < this.plotKeyCnt; i++)
                    {
                        var offset = binaryReader.ReadInt32();
                        offsets.Add(offset);
                    }
                    this.rightDelimiter = binaryReader.ReadInt32();
                    break;
                }
                this.offsets = offsets.ToArray();

                // Read <Units>
                var units = new List<string>();
                while (true)
                {
                    this.leftDelimiter = binaryReader.ReadInt32();
                    var unitLength = this.leftDelimiter / this.plotKeyCnt;
                    var buf = new char[unitLength];
                    for (var i = 0; i < this.plotKeyCnt; i++)
                    {
                        binaryReader.Read(buf, 0, unitLength);
                        var unit = new string(buf).Trim();
                        if (String.IsNullOrEmpty(unit))
                        {
                            unit = "-";
                        }
                        units.Add(unit);
                    }
                    this.rightDelimiter = binaryReader.ReadInt32();
                    break;
                }
                this.units = units.ToArray();

                // Read <Indexes>
                var indexes = new List<int>();
                while (true)
                {
                    this.leftDelimiter = binaryReader.ReadInt32();
                    for (var i = 0; i < this.plotVarCnt; i++)
                    {
                        var index = binaryReader.ReadInt32();
                        indexes.Add(index);
                    }
                    this.rightDelimiter = binaryReader.ReadInt32();
                    break;
                }
                this.indexes = indexes.ToArray();
            }
        }

        private void ReadSpecials(FileStream fileStream)
        {
            using (var binaryReader = new BinaryReader(fileStream, Encoding.UTF8, true))
            {
                //var spData = new List<string>();
                while (true)
                {
                    this.leftDelimiter = binaryReader.ReadInt32();
                    var buf = new char[this.leftDelimiter];
                    binaryReader.Read(buf, 0, this.leftDelimiter);
                    var str = new string(buf).Trim();
                    if (str.Equals(spMarker))
                    {

                    }
                    else if (str.Equals(trMarker))
                    {
                        break;
                    }
                    /*else
                    {
                        spData.Add(str);
                    }*/
                    this.rightDelimiter = binaryReader.ReadInt32();
                }
                //this.spData = spData.ToArray();
            }
        }

        private void ReadTimeRecords(FileStream fileStream)
        {
            var isVisited = false;
            var times = new List<double>();
            var values = new List<List<double>>();

            using (var binaryReader = new BinaryReader(fileStream, Encoding.UTF8, true))
            {
                var stringBuilder = new StringBuilder();
                var time = 0.0;
                var inputTRIndexes = this.inputTRIndexes.ToList();
                inputTRIndexes.Sort();
                var trData = new List<double>();
                for (var i = 0; i < inputTRIndexes.Count; i++)
                {
                    values.Add(new List<double>());
                }

                while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                {
                    if (isVisited == false)
                    {
                        this.rightDelimiter = binaryReader.ReadInt32();
                        isVisited = true;
                        continue;
                    }
                    else
                    {
                        this.leftDelimiter = binaryReader.ReadInt32();
                        if (this.leftDelimiter == 4)
                        {
                            var buf = new char[this.leftDelimiter];
                            binaryReader.Read(buf, 0, this.leftDelimiter);
                        }
                        else
                        {
                            trData.Clear();
                            time = binaryReader.ReadSingle();
                            times.Add(time);
                            var prev = 0;
                            var curr = 0;
                            var skipLength = 0;
                            var dataSkip = new byte[skipLength];
                            for (var i = 0; i < inputTRIndexes.Count; i++)
                            {
                                if (i == 0)
                                {
                                    prev = 0;
                                }
                                curr = inputTRIndexes[i];
                                skipLength = (curr - prev - 1) * 4;
                                dataSkip = new byte[skipLength];
                                binaryReader.Read(dataSkip, 0, skipLength);

                                var data = binaryReader.ReadSingle();
                                trData.Add(data);

                                prev = curr;
                            }
                            var trDataLength = this.plotVarCnt + 4;
                            skipLength = (trDataLength - curr - 1) * 4;
                            dataSkip = new byte[skipLength];
                            binaryReader.Read(dataSkip, 0, skipLength);

                            for (var i = 0; i < inputTRIndexes.Count; i++)
                            {
                                var originIdx = Array.FindIndex(this.inputTRIndexes, x => x.Equals(inputTRIndexes[i]));
                                values[originIdx].Add(trData[i]);
                            }
                        }
                        this.rightDelimiter = binaryReader.ReadInt32();
                    }
                }
            }

            var timeRecordData = new List<TimeRecordData>();
            for (var i = 0; i < this.inputVariables.Length; i++)
            {
                var recordData = new TimeRecordData
                {
                    variableName = this.inputVariables[i],
                    time = times.ToArray(),
                    value = values[i].ToArray(),
                };
                timeRecordData.Add(recordData);
            }

            this.timeRecordData = timeRecordData.ToArray();
        }
    }
}
