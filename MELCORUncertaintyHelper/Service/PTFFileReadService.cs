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
        private int[] inputTRIndexes;
        private ExtractDataManager dataManager;
        private TimeRecordData[] timeRecordData;

        // SPecial Section Marker
        private static readonly string spMarker = ".SP/";
        // Time Records Section Marker
        private static readonly string trMarker = ".TR/";

        public PTFFileReadService(PTFFile file)
        {
            this.file = file;
            this.inputVariableReader = InputVariableReadService.GetInputReadService;
            this.dataManager = ExtractDataManager.GetDataManager;
        }

        public void Read()
        {
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
            this.inputTRIndexes = (int[])this.inputVariableReader.GetInputTRIndexes();
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
