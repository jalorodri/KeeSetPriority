using KeePass.Plugins;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeeSetPriority
{
    public struct KeeSetPriorityData
    {
        public KeeSetPriorityData(IPluginHost host)
        {
            m_host = host;
            changePriorityOnOpen = ProcessPriorityClassKSP.Default;
            changePriorityOnSave = ProcessPriorityClassKSP.Default;
            changePriorityOnInactive = ProcessPriorityClassKSP.Default;
            priorityBoostState = PriorityBoostTypesKSP.Default;
            defaultPriorityBoost = PriorityBoostTypesKSP.Default;
            isAdvancedOptionsAvailable = false;
            // Save current process priority (the one it launched with) as default
            currentProcess = Process.GetCurrentProcess();
            try
            {
                // Checks that the process priority is both readable and writeable
                defaultProcessPriority = currentProcess.PriorityClass;
                currentProcess.Refresh();
                currentProcess.PriorityClass = defaultProcessPriority;
            }
            catch (Exception e)
            {
                MessageBox.Show(KeeSetPriorityTextStrings.ErrorPriorityLevelRWStr + e.Message, KeeSetPriorityTextStrings.ErrorBoxTitleStr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            try
            {
                // Checks status of PriorityBoost
                defaultPriorityBoost = currentProcess.PriorityBoostEnabled?PriorityBoostTypesKSP.Enabled:PriorityBoostTypesKSP.Disabled;
                currentProcess.Refresh();
                currentProcess.PriorityBoostEnabled = defaultPriorityBoost == PriorityBoostTypesKSP.Enabled;
                // If an exception is thrown, this value wouldn't be set
                isDefaultPriorityBoostSettable = true;
            }
            catch (Exception e)
            {
                // Don't rethrow, maybe it's just an old system or this doesn't exist in UNIX or Mono
                // isDefaultPriorityBoostSettable will disable the appropiate settings
                isDefaultPriorityBoostSettable = false;
                MessageBox.Show(KeeSetPriorityTextStrings.ErrorPriorityBoostRWStr + e.Message, KeeSetPriorityTextStrings.ErrorBoxTitleStr, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Check for realtime availability
            currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
            currentProcess.Refresh();
            isRealtimeAvailable = (currentProcess.PriorityClass == ProcessPriorityClass.RealTime);
            currentProcess.PriorityClass = defaultProcessPriority;
        }

        public IPluginHost m_host;

        public bool isRealtimeAvailable;

        public Process currentProcess;
        public readonly ProcessPriorityClass defaultProcessPriority;

        public bool isAdvancedOptionsAvailable;
        public readonly bool isDefaultPriorityBoostSettable;
        public readonly PriorityBoostTypesKSP defaultPriorityBoost; // Priority boost when the program starts up
        public PriorityBoostTypesKSP priorityBoostState;            // Priority boost currently programmed
        

        public ProcessPriorityClassKSP changePriorityOnOpen;
        public ProcessPriorityClassKSP changePriorityOnSave;
        public ProcessPriorityClassKSP changePriorityOnInactive;

    }

    public enum ProcessPriorityClassKSP
    {
        Default = 0,
        RealTime = ProcessPriorityClass.RealTime,
        High = ProcessPriorityClass.High,
        AboveNormal = ProcessPriorityClass.AboveNormal,
        Normal = ProcessPriorityClass.Normal,
        BelowNormal = ProcessPriorityClass.BelowNormal,
        Idle = ProcessPriorityClass.Idle
    }

    public enum PriorityBoostTypesKSP
    {
        Default = 0,
        Enabled = 1,
        Disabled = 2
    }

    public enum ActionTypesKSP
    {
        Inactive = 0,
        Open = 1,
        Save = 2
    }
}
