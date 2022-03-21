using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace KeeSetPriority
{
    public partial class ProcessPicker : UserControl
    {

        private static List<String> availableProcessesStr;
        private List<String> selectedProcessesStr;

        // It is marked as true if the getAvailableProcesses process is done
        private static bool isGetAvailableProcessesDone = false;

#pragma warning disable IDE0044
        private static List<ProcessPicker> processPickerList;
#pragma warning restore IDE0044
        private static KeeSetPriorityData processPickerDataClass;
        private readonly ActionTypesKSP actionType;

        private static readonly string[] systemProcesses = {
            "KeePass",
            "services",
            "Idle",
            "wininit",
            "winlogon",
            "csrss",
            "lsass",
            "dwm",
            "Memory Compression",
            "Registry",
            "RuntimeBroker",
            "smss",
            "System",
            "svchost",
            "sihost",
            // These ones don't make sense, since they could be pretty much anything
            "dllhost",
            "rundll32",
        };

        private static readonly string[] windowedSystemProcesses = {
            "ApplicationFrameHost",
            "TextInputHost"
        };

        internal ProcessPicker(ActionTypesKSP action)
        {
            if (processPickerDataClass == null)
                throw new ArgumentNullException("processPickerDataClass is not defined, need to call ProcessPicker.SetDataStruct first");

            actionType = action;

            InitializeComponent();

            // Only the most current processPicker instances should be on the list; if there are already 3, the list is stale
            // and needs to be cleared and updated
            if (processPickerList.Count >= 3)
            {
                processPickerList.Clear();
            }
            processPickerList.Add(this);
            
            availableProcessesBox.Items.Add("Loading...");
            UpdateAvailableProcessesBoxes();

            selectedProcessesStr = new List<string>();
            if (!(processPickerDataClass.configDataStruct.GetDependantPrograms(actionType, true) == null || processPickerDataClass.configDataStruct.GetDependantPrograms(actionType, true).GetLength(0) == 0))
            {
                selectedProcessesStr.AddRange(processPickerDataClass.configDataStruct.GetDependantPrograms(actionType, true));
                selectedProcessesStr.Sort();
                selectedProcessesBox.Items.AddRange(selectedProcessesStr.ToArray());
            }
        }

        static ProcessPicker()
        {
            processPickerList = new List<ProcessPicker>();
        }

        internal static void SetDataStruct(ref KeeSetPriorityData dataClass)
        {
            // By assignment, it is the same class, so changes dont by ProcessPicker will be visible to ConfigWindow
            processPickerDataClass = dataClass;
        }

        private void UpdateAvailableProcessesBoxes()
        {
            this.availableProcessesBox.Items.Clear();
            this.availableProcessesBox.Items.Add("Loading...");
            GetAvailableProcesses();
            this.availableProcessesBox.Items.Clear();
            this.availableProcessesBox.Items.AddRange(availableProcessesStr.ToArray());
        }

        private static void GetAvailableProcesses()
        {
            if (!isGetAvailableProcessesDone)
            {
                Process[] runningProcessesList = Process.GetProcesses();
                availableProcessesStr = new List<string>(runningProcessesList.Length);
                int j = 1;
                foreach (Process process in runningProcessesList)
                {
                    if (process.MainWindowTitle == "")
                    {
                        if (processPickerDataClass.configDataStruct.allowNonWindowedProcesses == AllowBackgroundSystemProcesses.BackgroundAndSystem)
                        {
                            availableProcessesStr.Add(process.ProcessName);
                        }
                        else if (processPickerDataClass.configDataStruct.allowNonWindowedProcesses == AllowBackgroundSystemProcesses.OnlyBackground)
                        {
                            if (!systemProcesses.Contains(process.ProcessName))
                            {
                                availableProcessesStr.Add(process.ProcessName);
                            }
                        }
                    }
                    else
                    {
                        if (!windowedSystemProcesses.Contains(process.ProcessName))
                        {
                            availableProcessesStr.Add(process.ProcessName + " [" + process.MainWindowTitle + ']');
                        }
                    }
                    j++;
                }
                KeeSetPriorityData.DisposeArray(runningProcessesList);

                availableProcessesStr = availableProcessesStr.Distinct().ToList();
                // Doesn't make much sense to have this listed, it's always gonna be running when the plugin is active
                // Remove core system processes
                if (processPickerDataClass.configDataStruct.allowNonWindowedProcesses != AllowBackgroundSystemProcesses.BackgroundAndSystem)
                {
                    foreach(string systemProcess in systemProcesses)
                    {

                    }
                }
                availableProcessesStr.Sort();

                isGetAvailableProcessesDone = true;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            foreach(int i in availableProcessesBox.SelectedIndices)
            {
                selectedProcessesStr.Add(availableProcessesBox.Items[i].ToString().Substring(0, availableProcessesBox.Items[i].ToString().IndexOf(' ') == -1 ? availableProcessesBox.Items[i].ToString().Length : availableProcessesBox.Items[i].ToString().IndexOf(' ')));
            }
            selectedProcessesStr = selectedProcessesStr.Distinct().ToList();
            selectedProcessesStr.Sort();
            selectedProcessesBox.Items.Clear();
            selectedProcessesBox.Items.AddRange(selectedProcessesStr.ToArray());
            processPickerDataClass.configDataStruct.SetDependantPrograms(actionType, selectedProcessesStr.ToArray(), true);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach (int i in selectedProcessesBox.SelectedIndices)
            {
                selectedProcessesStr.RemoveAt(i);
                selectedProcessesBox.Items.RemoveAt(i);
            }
            processPickerDataClass.configDataStruct.SetDependantPrograms(actionType, selectedProcessesStr.ToArray(), true);
        }


        internal static void UpdateProcessList()
        {
            foreach (ProcessPicker instance in processPickerList)
            {
                instance.UpdateButton_Click(null, null);
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            isGetAvailableProcessesDone = false;
            UpdateAvailableProcessesBoxes();
        }

        private void ProcessPicker_SizeChanged(object sender, EventArgs e)
        {
            availableProcessesBox.Size = selectedProcessesBox.Size = new System.Drawing.Size
            {
                Width = (this.Width - buttonsPanel.Size.Width - buttonsPanel.Margin.Horizontal - availableProcessesBox.Margin.Horizontal - selectedProcessesBox.Margin.Horizontal) / 2,
                Height = this.Height - 21 - availableProcessesBox.Margin.Bottom
            };
            selectedProcessesBox.Location = new System.Drawing.Point
            {
                X = this.Width - selectedProcessesBox.Size.Width - selectedProcessesBox.Margin.Right,
                Y = 21
            };
            buttonsPanel.Location = new System.Drawing.Point
            {
                X = availableProcessesBox.Location.X + availableProcessesBox.Size.Width + availableProcessesBox.Margin.Right + buttonsPanel.Margin.Left,
                Y = 21
            };
        }
    }
}
