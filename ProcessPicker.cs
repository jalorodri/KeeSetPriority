using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeeSetPriority
{
    public partial class ProcessPicker : UserControl
    {
        private static List<String> availableProcessesStr;
        private List<String> selectedProcessesStr; 
        private String[] dataStructSelectedProcesses;

        private static Mutex isGetAvailableProcessesMutex = new Mutex();
        private static bool isGetAvailableProcessesDone = false;

        private static List<ProcessPicker> processPickerList;

        private static KeeSetPriorityData dataStruct;

        public ProcessPicker(ActionTypesKSP action)
        {
            InitializeComponent();
            processPickerList.Add(this);
            availableProcessesBox.Items.Add("Loading...");
            getAvailableProcessesBackgroundWorker.RunWorkerAsync();
            selectedProcessesStr = new List<string>();
            switch (action)
            {
                case ActionTypesKSP.Open:
                    dataStructSelectedProcesses = dataStruct.openDependentPrograms;
                    break;
                case ActionTypesKSP.Save:
                    dataStructSelectedProcesses = dataStruct.saveDependentPrograms;
                    break;
                case ActionTypesKSP.Inactive:
                    dataStructSelectedProcesses = dataStruct.inactiveDependentPrograms;
                    break;
            }
            if(dataStructSelectedProcesses != null)
            {
                selectedProcessesStr.AddRange(dataStructSelectedProcesses);
                selectedProcessesStr.Sort();
                selectedProcessesBox.Items.Add(selectedProcessesStr.ToArray());
            }
        }

        ~ProcessPicker()
        {
            processPickerList.Remove(this);
        }

        static ProcessPicker()
        {
            processPickerList = new List<ProcessPicker>();
        }

        public static void SetDataStruct(ref KeeSetPriorityData dataStruct)
        {
            ProcessPicker.dataStruct = dataStruct;
        }

        public static void GetAvailableProcesses(bool windowedProcesses = false)
        {
            isGetAvailableProcessesMutex.WaitOne();
            if (!isGetAvailableProcessesDone)
            {
                Process[] processList = Process.GetProcesses(); 
                availableProcessesStr = new List<string>(processList.Length);
                int j = 1;
                foreach (Process process in processList)
                {
                    // VERY SLOW, must optimize
                    // For now, it just runs asynchronously
                    try
                    {
                        availableProcessesStr.Add(process.MainModule.FileName.Substring(process.MainModule.FileName.LastIndexOf('\\') + 1));
                        // j won't be incremented if it throws an exception
                        j++;
                    }
                    catch (Win32Exception)
                    {
                        // Win32Exceptions mostly come due to not having SeDebugPrivilege and trying to access an OS-level process like ntoskrnl.exe or Services
                        // Just ignore them
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error while trying to retrieve available processes", ex);
                    }
                    finally
                    {
                        process.Dispose();
                    }
                }
                availableProcessesStr = availableProcessesStr.Distinct().ToList();
                availableProcessesStr.Remove("KeePass.exe"); // Doesn't make much sense to have this listed, it's always gonna be running when the plugin is active
                availableProcessesStr.Sort();
                isGetAvailableProcessesDone = true;
            }
            isGetAvailableProcessesMutex.ReleaseMutex();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            foreach(int i in availableProcessesBox.SelectedIndices)
            {
                selectedProcessesStr.Add(selectedProcessesBox.Items[i].ToString());
                selectedProcessesStr.Sort();
                selectedProcessesBox.Items.Clear();
                selectedProcessesBox.Items.AddRange(selectedProcessesStr.ToArray());
                dataStructSelectedProcesses = selectedProcessesStr.ToArray();
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach (int i in selectedProcessesBox.SelectedIndices)
            {
                selectedProcessesStr.RemoveAt(i);
                selectedProcessesBox.Items.RemoveAt(i);
                dataStructSelectedProcesses = selectedProcessesStr.ToArray();
            }
        }

        private void GetAvailableProcessesBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
                GetAvailableProcesses();
        }

        private void GetAvailableProcessesBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach(ProcessPicker processPicker in processPickerList)
            {
                processPicker.availableProcessesBox.Items.Clear();
                processPicker.availableProcessesBox.Items.AddRange(availableProcessesStr.ToArray());
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            foreach (ProcessPicker processPicker in processPickerList)
            {
                processPicker.availableProcessesBox.Items.Clear();
                processPicker.availableProcessesBox.Items.Add("Loading...");
            }
            isGetAvailableProcessesDone = false;
            getAvailableProcessesBackgroundWorker.RunWorkerAsync();
        }
    }
}
