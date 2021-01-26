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
        private string[] inputs;
        private string[] inputPackageNames;
        private int[] inputControlVolumes;
        private int[] idxes;
        private int[] totalIdxes;

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

        public Object GetInputs() => this.inputs.Clone();

        public Object GetIdxes() => this.idxes.Clone();

        public Object GetTotalIdxes() => this.totalIdxes.Clone();

        public void InputManage()
        {
            this.ReadInput();
            if (this.inputs.Length <= 0)
            {
                MessageBox.Show("There is no search word", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.InputPostProcess();
        }

        private void ReadInput()
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

            var inputs = new List<string>();
            for (var i = 0; i < dgvInputs.RowCount - 1; i++)
            {
                var input = dgvInputs[colIdx, i].Value.ToString();
                if (!string.IsNullOrEmpty(input))
                {
                    inputs.Add(input);
                }
            }

            this.inputs = inputs.ToArray();
        }

        private void InputPostProcess()
        {
            var packageNames = new List<string>();
            var controlVolumes = new List<int>();

            for (var i = 0; i < this.inputs.Length; i++)
            {
                string name;
                int node;

                if (this.inputs[i].Contains("."))
                {
                    name = this.inputs[i].Substring(0, this.inputs[i].LastIndexOf("."));
                    node = Convert.ToInt32(this.inputs[i].Substring(this.inputs[i].LastIndexOf(".") + 1));
                }
                else
                {
                    name = this.inputs[i];
                    node = 0;
                }

                packageNames.Add(name);
                controlVolumes.Add(node);
            }

            this.inputPackageNames = packageNames.ToArray();
            this.inputControlVolumes = controlVolumes.ToArray();
        }

        public void MakeIndexes(string[] packageNames, int[] packageVariableCnt, int[] controlVolumes)
        {
            var idxes = new List<int>();
            var totalIdxes = new List<int>();

            int frontIdx;
            int rearIdx;
            int totalIdx;

            for (var i = 0; i < this.inputs.Length; i++)
            {
                frontIdx = Array.IndexOf(packageNames, this.inputPackageNames[i]);
                rearIdx = Array.LastIndexOf(packageNames, this.inputPackageNames[i]);
                if (frontIdx == -1)
                {
                    frontIdx = Array.IndexOf(packageNames, this.inputs[i]);
                    if (frontIdx == -1)
                    {
                        totalIdx = packageVariableCnt[frontIdx] - 1;
                    }
                    else
                    {
                        totalIdx = -1;
                    }
                }
                else if (frontIdx == rearIdx)
                {
                    totalIdx = Array.IndexOf(controlVolumes, this.inputControlVolumes[i],
                        packageVariableCnt[frontIdx] - 1, packageVariableCnt[frontIdx + 1] - packageVariableCnt[frontIdx]);
                }
                else
                {
                    totalIdx = Array.IndexOf(controlVolumes, this.inputControlVolumes[i], frontIdx, rearIdx);
                }

                idxes.Add(frontIdx);
                totalIdxes.Add(totalIdx);
            }

            this.idxes = idxes.ToArray();
            this.totalIdxes = totalIdxes.ToArray();
        }
    }
}
