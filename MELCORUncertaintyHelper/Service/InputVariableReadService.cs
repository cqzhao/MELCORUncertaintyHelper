using MELCORUncertaintyHelper.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MELCORUncertaintyHelper.Service
{
    public class InputVariableReadService
    {
        private string[] inputVariables;
        private string[] inputPlotKeys;
        private int[] inputIndexes;
        private int[] inputTRIndexes;

        private InputVariableReadService()
        {

        }

        private static readonly Lazy<InputVariableReadService> inputReadService = new Lazy<InputVariableReadService>(() => new InputVariableReadService());

        public static InputVariableReadService GetInputReadService
        {
            get
            {
                return inputReadService.Value;
            }
        }

        public Object GetInputVariables() => this.inputVariables.Clone();

        public Object GetInputTRIndexes() => this.inputTRIndexes.Clone();


        public bool InputManage()
        {
            try
            {
                this.ReadInput();
                if (this.inputVariables.Length < 0 || this.inputVariables == null)
                {
                    MessageBox.Show("There is no search word", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                this.InputPostProcess();
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex);
                logWrite.MakeLogFile();
                return false;
            }

            return true;
        }

        private void ReadInput()
        {
            try
            {
                var dgvInputs = VariableInputForm.GetFrmVariableInput.GetDgvVariable();

                var colIdx = 0;
                for (var i = 0; i < dgvInputs.ColumnCount; i++)
                {
                    if (dgvInputs.Columns[i].Name.Equals("Variable Name"))
                    {
                        colIdx = i;
                    }
                }

                var inputVariables = new List<string>();
                for (var i = 0; i < dgvInputs.RowCount - 1; i++)
                {
                    var input = dgvInputs[colIdx, i].Value.ToString();
                    if (!string.IsNullOrEmpty(input))
                    {
                        inputVariables.Add(input);
                    }
                }

                this.inputVariables = inputVariables.ToArray();
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex);
                logWrite.MakeLogFile();
                return;
            }
        }

        private void InputPostProcess()
        {
            try
            {
                var inputPlotKeys = new List<string>();
                var inputIndexes = new List<int>();

                for (var i = 0; i < this.inputVariables.Length; i++)
                {
                    var input = this.inputVariables[i];
                    string plotKey;
                    int index;

                    if (input.Contains("."))
                    {
                        var targetIdx = input.LastIndexOf(".");
                        plotKey = input.Substring(0, targetIdx);
                        index = Convert.ToInt32(input.Substring(targetIdx + 1));
                    }
                    else
                    {
                        plotKey = input;
                        index = 0;
                    }

                    inputPlotKeys.Add(plotKey);
                    inputIndexes.Add(index);
                }

                this.inputPlotKeys = inputPlotKeys.ToArray();
                this.inputIndexes = inputIndexes.ToArray();
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex);
                logWrite.MakeLogFile();
            }
        }

        public void FindInputTRIndexes(string[] plotKeys, int[] offsets, int[] indexes)
        {
            try
            {
                var inputTRIndexes = new List<int>();
                for (var i = 0; i < this.inputPlotKeys.Length; i++)
                {
                    var plotKeyIdx = Array.FindIndex(plotKeys, x => x.Equals(this.inputPlotKeys[i]));
                    var plotKeyOffset = offsets[plotKeyIdx];
                    int plotKeyNextOffset;
                    if (plotKeyIdx == offsets.Length - 1)
                    {
                        plotKeyNextOffset = indexes.Length;
                    }
                    else
                    {
                        plotKeyNextOffset = offsets[plotKeyIdx + 1];
                    }

                    var start = plotKeyOffset - 1;
                    var end = plotKeyNextOffset;
                    var idx = Array.IndexOf(indexes, this.inputIndexes[i], start, end - start) + 4;
                    inputTRIndexes.Add(idx);
                }
                this.inputTRIndexes = inputTRIndexes.ToArray();
            }
            catch (Exception ex)
            {
                var logWrite = new LogFileWriteService(ex);
                logWrite.MakeLogFile();
            }
        }
    }
}
