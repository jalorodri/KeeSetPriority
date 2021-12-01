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
            priorityModeOnOpen = PriorityChangeTypes.NeverSet;
            priorityModeOnSave = PriorityChangeTypes.NeverSet;
            priorityModeOnInactive = PriorityChangeTypes.NeverSet;
            priorityLevelOnOpen = ProcessPriorityClassKSP.NotSet;
            priorityLevelOnSave = ProcessPriorityClassKSP.NotSet;
            priorityLevelOnInactive = ProcessPriorityClassKSP.NotSet;
            priorityBoostState = PriorityBoostTypesKSP.Default;
            defaultPriorityBoost = PriorityBoostTypesKSP.Default;
            isAdvancedOptionsAvailable = false;
            allowDangerousPrioritites = AllowDangerousPrioritites.No;
            openStringVector = null;
            saveStringVector = null;
            inactiveStringVector = null;
            openDependentPrograms = null;
            saveDependentPrograms = null;
            inactiveDependentPrograms = null;
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

        public PriorityChangeTypes priorityModeOnOpen;
        public PriorityChangeTypes priorityModeOnSave;
        public PriorityChangeTypes priorityModeOnInactive;

        public ProcessPriorityClassKSP priorityLevelOnOpen;
        public ProcessPriorityClassKSP priorityLevelOnSave;
        public ProcessPriorityClassKSP priorityLevelOnInactive;

        private string[] openStringVector;
        private string[] saveStringVector;
        private string[] inactiveStringVector;

        public string[] openDependentPrograms;
        public string[] saveDependentPrograms;
        public string[] inactiveDependentPrograms;

        public void ReadAndValidateSettings()
        {
            // Read main settings
            try
            {
                openStringVector = this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnOpenString, PriorityChangeTypes.NeverSet.ToString()).Split(',');
                saveStringVector = this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnSaveString, PriorityChangeTypes.NeverSet.ToString()).Split(',');
                inactiveStringVector = this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnInactiveString, PriorityChangeTypes.NeverSet.ToString()).Split(',');
                this.priorityModeOnOpen = (PriorityChangeTypes)Enum.Parse(typeof(PriorityChangeTypes), openStringVector[0]);
                this.priorityModeOnSave = (PriorityChangeTypes)Enum.Parse(typeof(PriorityChangeTypes), saveStringVector[0]);
                this.priorityModeOnInactive = (PriorityChangeTypes)Enum.Parse(typeof(PriorityChangeTypes), inactiveStringVector[0]);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while reading main data", ex);
            }
            

            if ((priorityModeOnOpen != PriorityChangeTypes.NeverSet && openStringVector.Length == 1) ||
               (priorityModeOnSave != PriorityChangeTypes.NeverSet && saveStringVector.Length == 1) ||
               (priorityModeOnInactive != PriorityChangeTypes.NeverSet && inactiveStringVector.Length == 1) ||
               (priorityModeOnOpen == PriorityChangeTypes.SetWhenDependent && openStringVector.Length <= 3) ||
               (priorityModeOnSave == PriorityChangeTypes.SetWhenDependent && saveStringVector.Length <= 3) ||
               (priorityModeOnInactive == PriorityChangeTypes.SetWhenDependent && inactiveStringVector.Length <= 3))
            {
                throw new Exception("Disagreement between PriorityChangeTypes and settings vector length");
            }

            if(priorityModeOnOpen != PriorityChangeTypes.NeverSet)
            {
                this.priorityLevelOnOpen = (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), openStringVector[1]);
            }
            if (priorityModeOnSave != PriorityChangeTypes.NeverSet)
            {
                this.priorityLevelOnSave = (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), saveStringVector[1]);
            }
            if (priorityModeOnInactive != PriorityChangeTypes.NeverSet)
            {
                this.priorityLevelOnInactive = (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), inactiveStringVector[1]);
            }

            // Read advanced options
            try
            {
                this.priorityBoostState = (PriorityBoostTypesKSP)Enum.Parse(typeof(PriorityBoostTypesKSP), this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityBoostString, PriorityBoostTypesKSP.Default.ToString()));
                this.isAdvancedOptionsAvailable = bool.Parse(this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.isAdvancedOptionsAvailableString, bool.FalseString));
                this.allowDangerousPrioritites = (AllowDangerousPrioritites)Enum.Parse(typeof(AllowDangerousPrioritites), this.m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.allowDangerousPrioritiesString, AllowDangerousPrioritites.No.ToString()));
            }
            catch(Exception ex)
            {
                throw new Exception("Error while reading advanced data", ex);
            }
            

            // Check for invalid advanced options
            if(!this.isAdvancedOptionsAvailable && (allowDangerousPrioritites != AllowDangerousPrioritites.No || priorityBoostState != PriorityBoostTypesKSP.Default))
            {
                throw new Exception("isAdvancedOptionsAvailable does not allow for high priorities or setting priority boost modes");
            }
            // Check for invalid Realtime priorities
            if (this.allowDangerousPrioritites != AllowDangerousPrioritites.Yes && 
                (this.priorityLevelOnOpen == ProcessPriorityClassKSP.RealTime || 
                 this.priorityLevelOnSave == ProcessPriorityClassKSP.RealTime || 
                 this.priorityLevelOnInactive == ProcessPriorityClassKSP.RealTime)
               )
            {
                throw new Exception("allowDangerousPriority does not allow for Real Time priorities");
            }
            // Check for invalid high priorities
            if (this.allowDangerousPrioritites == AllowDangerousPrioritites.No &&
                (this.priorityLevelOnOpen == ProcessPriorityClassKSP.High || 
                 this.priorityLevelOnSave == ProcessPriorityClassKSP.High || 
                 this.priorityLevelOnInactive == ProcessPriorityClassKSP.High)
               )
            {
                throw new Exception("allowDangerousPriority does not allow for High priorities");
            }
        }

        // Call when incongruent settings are detected
        public bool OnIncongruentSettings(Exception ex)
        {
            DialogResult warningBox = MessageBox.Show("Settings are not readable and may be corrupted.\n\n" +
                    "Do you want to set them to default values? Previous values will be overwritten.\n\nFull error:\n" 
                    + ex.Message + (ex.InnerException == null ? null : ("\n\nInner error:\n" + ex.InnerException.Message)),
                    KeeSetPriorityTextStrings.ErrorBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            if (warningBox == DialogResult.Yes)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnOpenString, PriorityChangeTypes.NeverSet.ToString());
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnSaveString, PriorityChangeTypes.NeverSet.ToString());
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnInactiveString, PriorityChangeTypes.NeverSet.ToString());
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityBoostString, PriorityBoostTypesKSP.Default.ToString());
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.isAdvancedOptionsAvailableString, bool.FalseString);
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.allowDangerousPrioritiesString, AllowDangerousPrioritites.No.ToString());
                this.priorityModeOnOpen = PriorityChangeTypes.NeverSet;
                this.priorityModeOnSave = PriorityChangeTypes.NeverSet;
                this.priorityModeOnInactive = PriorityChangeTypes.NeverSet;
                this.priorityLevelOnOpen = ProcessPriorityClassKSP.NotSet;
                this.priorityLevelOnSave = ProcessPriorityClassKSP.NotSet;
                this.priorityLevelOnInactive = ProcessPriorityClassKSP.NotSet;
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
            // Write open settings
            if(this.priorityModeOnOpen == PriorityChangeTypes.NeverSet)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnOpenString, this.priorityModeOnOpen.ToString());
            }
            else if(this.priorityModeOnOpen == PriorityChangeTypes.AlwaysSet)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnOpenString, this.priorityModeOnOpen.ToString() + ',' + this.priorityLevelOnOpen.ToString());
            }
            else if(this.priorityModeOnOpen == PriorityChangeTypes.SetWhenDependent)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnOpenString, this.priorityModeOnOpen.ToString() + ',' + this.priorityLevelOnOpen.ToString() + ',' + GetStringFromVector(openDependentPrograms,','));
            }
            else
            {
                throw new Exception("priorityModeOnOpen is invalid");
            }

            // Write save settings
            if (this.priorityModeOnSave == PriorityChangeTypes.NeverSet)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnSaveString, this.priorityModeOnSave.ToString());
            }
            else if (this.priorityModeOnSave == PriorityChangeTypes.AlwaysSet)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnSaveString, this.priorityModeOnSave.ToString() + ',' + this.priorityLevelOnSave.ToString());
            }
            else if (this.priorityModeOnSave == PriorityChangeTypes.SetWhenDependent)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnSaveString, this.priorityModeOnSave.ToString() + ',' + this.priorityLevelOnSave.ToString() + ',' + GetStringFromVector(saveDependentPrograms, ','));
            }
            else
            {
                throw new Exception("priorityModeOnSave is invalid");
            }

            // Write inactive settings
            if (this.priorityModeOnInactive == PriorityChangeTypes.NeverSet)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnInactiveString, this.priorityModeOnInactive.ToString());
            }
            else if (this.priorityModeOnInactive == PriorityChangeTypes.AlwaysSet)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnInactiveString, this.priorityModeOnInactive.ToString() + ',' + this.priorityLevelOnInactive.ToString());
            }
            else if (this.priorityModeOnInactive == PriorityChangeTypes.SetWhenDependent)
            {
                this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnInactiveString, this.priorityModeOnInactive.ToString() + ',' + this.priorityLevelOnInactive.ToString() + ',' + GetStringFromVector(inactiveDependentPrograms, ','));
            }
            else
            {
                throw new Exception("priorityModeOnInactive is invalid");
            }

            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnInactiveString, this.priorityModeOnInactive.ToString());
            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityBoostString, this.priorityBoostState.ToString());
            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.isAdvancedOptionsAvailableString, this.isAdvancedOptionsAvailable.ToString());
            this.m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.allowDangerousPrioritiesString, this.allowDangerousPrioritites.ToString());
        }

        public static string GetStringFromVector(string[] vector, char delimitor)
        {
            string stringResult = vector[0];
            foreach (String str in vector)
            {
                stringResult += delimitor + str;
            }
            return stringResult;
        }
#if DEBUG
        public void PrintSettings()
        {
            MessageBox.Show("priorityLevelOnOpen: " + priorityLevelOnOpen.ToString() +
                            "\npriorityLevelOnSave: " + priorityLevelOnSave.ToString() +
                            "\npriorityLevelOnInactive: " + priorityLevelOnInactive.ToString() +
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
        RealTime = ProcessPriorityClass.RealTime,
        High = ProcessPriorityClass.High,
        AboveNormal = ProcessPriorityClass.AboveNormal,
        Normal = ProcessPriorityClass.Normal,
        BelowNormal = ProcessPriorityClass.BelowNormal,
        Idle = ProcessPriorityClass.Idle,
        NotSet = -1
    }

    public enum PriorityChangeTypes
    {
        NeverSet = 0,
        AlwaysSet = 1,
        SetWhenDependent = 2
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
