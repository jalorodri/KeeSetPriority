using KeePass.Plugins;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeeSetPriority
{
    // Since it is declared as a struct, it is passed by value and not by reference to the settings window
    // Important later on in case the user doesn't want to save the modified settings
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
            allowDangerousPrioritites = AllowDangerousPrioritites.No;
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
        public AllowDangerousPrioritites allowDangerousPrioritites;

        public readonly bool isDefaultPriorityBoostSettable;
        public readonly PriorityBoostTypesKSP defaultPriorityBoost; // Priority boost when the program starts up
        public PriorityBoostTypesKSP priorityBoostState;            // Priority boost currently programmed
        

        public ProcessPriorityClassKSP changePriorityOnOpen;
        public ProcessPriorityClassKSP changePriorityOnSave;
        public ProcessPriorityClassKSP changePriorityOnInactive;

        public void ReadAndValidateSettings()
        {
            this.changePriorityOnOpen = (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnOpenString, ProcessPriorityClassKSP.Default.ToString()));
            this.changePriorityOnSave = (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnSaveString, ProcessPriorityClassKSP.Default.ToString()));
            this.changePriorityOnInactive = (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnInactiveString, ProcessPriorityClassKSP.Default.ToString()));
            this.priorityBoostState = (PriorityBoostTypesKSP)Enum.Parse(typeof(PriorityBoostTypesKSP), this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityBoostString, PriorityBoostTypesKSP.Default.ToString()));
            this.isAdvancedOptionsAvailable = bool.Parse(this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.isAdvancedOptionsAvailableString, bool.FalseString));
            this.allowDangerousPrioritites = (AllowDangerousPrioritites)Enum.Parse(typeof(AllowDangerousPrioritites), this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.allowDangerousPrioritiesString, AllowDangerousPrioritites.No.ToString()));

            // Check for invalid advanced options
            if(!this.isAdvancedOptionsAvailable && (allowDangerousPrioritites != AllowDangerousPrioritites.No || priorityBoostState != PriorityBoostTypesKSP.Default))
            {
                throw new Exception("isAdvancedOptionsAvailable does not allow for high priorities or setting priority boost modes");
            }
            // Check for invalid Realtime priorities
            if (this.allowDangerousPrioritites != AllowDangerousPrioritites.Yes && 
                (this.changePriorityOnOpen == ProcessPriorityClassKSP.RealTime || this.changePriorityOnSave == ProcessPriorityClassKSP.RealTime || this.changePriorityOnInactive == ProcessPriorityClassKSP.RealTime)
               )
            {
                throw new Exception("allowDangerousPriority does not allow for Real Time priorities");
            }
            // Check for invalid high priorities
            if (this.allowDangerousPrioritites == AllowDangerousPrioritites.No &&
                (this.changePriorityOnOpen == ProcessPriorityClassKSP.High || this.changePriorityOnSave == ProcessPriorityClassKSP.High || this.changePriorityOnInactive == ProcessPriorityClassKSP.High)
               )
            {
                throw new Exception("allowDangerousPriority does not allow for High priorities");
            }
        }

        // Call when incongruent settings are detected
        public bool OnIncongruentSettings(Exception ex)
        {
            DialogResult warningBox = MessageBox.Show("Settings are not readable and may be corrupted.\n\n" +
                    "Do you want to set them to default values? Previous values will be overwritten.\n\nFull error:\n" + ex.Message,
                    KeeSetPriorityTextStrings.ErrorBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            if (warningBox == DialogResult.Yes)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnOpenString, ProcessPriorityClassKSP.Default.ToString());
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnSaveString, ProcessPriorityClassKSP.Default.ToString());
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnInactiveString, ProcessPriorityClassKSP.Default.ToString());
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityBoostString, PriorityBoostTypesKSP.Default.ToString());
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.isAdvancedOptionsAvailableString, bool.FalseString);
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.allowDangerousPrioritiesString, AllowDangerousPrioritites.No.ToString());
                this.changePriorityOnOpen = ProcessPriorityClassKSP.Default;
                this.changePriorityOnSave = ProcessPriorityClassKSP.Default;
                this.changePriorityOnInactive = ProcessPriorityClassKSP.Default;
                this.priorityBoostState = PriorityBoostTypesKSP.Default;
                this.isAdvancedOptionsAvailable = false;
                this.allowDangerousPrioritites = AllowDangerousPrioritites.No;
                return true;
            }
            else
            {
                // Cannot continue with corrupted data, do not load the plugin
                return false;
            }
        }

        public void WriteSettings()
        {
            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnOpenString, this.changePriorityOnOpen.ToString());
            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnSaveString, this.changePriorityOnSave.ToString());
            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnInactiveString, this.changePriorityOnInactive.ToString());
            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityBoostString, this.priorityBoostState.ToString());
            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.isAdvancedOptionsAvailableString, this.isAdvancedOptionsAvailable.ToString());
            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.allowDangerousPrioritiesString, this.allowDangerousPrioritites.ToString());
        }
#if DEBUG
        public void PrintSettings()
        {
            MessageBox.Show("changePriorityOnOpen: " + changePriorityOnOpen.ToString() +
                            "\nchangePriorityOnSave: " + changePriorityOnSave.ToString() +
                            "\nchangePriorityOnInactive: " + changePriorityOnInactive.ToString() +
                            "\ndefaultProcessPriority: " + defaultProcessPriority.ToString() +
                            "\npriorityBoostState: " + priorityBoostState.ToString() +
                            "\ndefaultPriorityBoost: " + defaultPriorityBoost.ToString() +
                            "\nisDefaultPriorityBoostSettable: " + isDefaultPriorityBoostSettable.ToString() +
                            "\nisRealtimeAvailable: " + isRealtimeAvailable.ToString() +
                            "\nisAdvancedOptionsAvailable: " + isAdvancedOptionsAvailable.ToString()
                            , "dataStruct data");
        }
#endif
    }

    public enum ProcessPriorityClassKSP
    {
        Default = 0,
        RealTime = ProcessPriorityClass.RealTime,
        High = ProcessPriorityClass.High,
        AboveNormal = ProcessPriorityClass.AboveNormal,
        Normal = ProcessPriorityClass.Normal,
        BelowNormal = ProcessPriorityClass.BelowNormal,
        Idle = ProcessPriorityClass.Idle,
        NotSet = -1
    }

    public enum PriorityBoostTypesKSP
    {
        Default = 4,
        Enabled = 1,
        Disabled = 2
    }

    public enum ActionTypesKSP
    {
        Inactive = 4,
        Open = 1,
        Save = 2
    }

    public enum AllowDangerousPrioritites
    {
        No = 0,
        Yes = 1,
        OnlyHigh = 2
    }
}
