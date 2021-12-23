using KeePass.Plugins;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace KeeSetPriority
{
    // Since it is declared as a struct, it is passed by value and not by reference to the settings window
    // Important later on in case the user doesn't want to save the modified settings
    public sealed partial class KeeSetPriorityData : IDisposable, ICloneable
    {
        /// <summary>
        /// This constructor should be called when the plugin is stated and the data is about to be read.
        /// </summary>
        /// <param name="host"></param>
        public KeeSetPriorityData(IPluginHost host)
        {
            m_host = host;
            this.configDataStruct.SetPriorityMode(ActionTypesKSP.AllActions,PriorityChangeTypes.NeverSet);
            this.configDataStruct.SetPriorityLevel(ActionTypesKSP.AllActions, ProcessPriorityClassKSP.NotSet);
            this.configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
            this.configDataStruct.isAdvancedOptionsAvailable = false;
            this.configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
            this.configDataStruct.updateThreadTime = 5000; // update every five seconds seems like a good default
        }
        /// <summary>
        /// This constructor should be called when the configuration window is created, and should only be used by the Clone method of this class
        /// </summary>
        /// <exception cref="ArgumentNullException">When an initial class is not instantiated with a valid IPluginHost</exception>
        private KeeSetPriorityData()
        {
            if (m_host == null)
                throw new ArgumentNullException("m_host is null");
            this.configDataStruct.SetPriorityMode(ActionTypesKSP.AllActions, PriorityChangeTypes.NeverSet);
            this.configDataStruct.SetPriorityLevel(ActionTypesKSP.AllActions, ProcessPriorityClassKSP.NotSet);
            this.configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
            this.configDataStruct.isAdvancedOptionsAvailable = false;
            this.configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
            this.configDataStruct.updateThreadTime = 5000; // update every five seconds seems like a good default
        }
        static KeeSetPriorityData()
        {
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
                defaultPriorityBoost = currentProcess.PriorityBoostEnabled ? PriorityBoostTypesKSP.Enabled : PriorityBoostTypesKSP.Disabled;
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

        // For configuration purposes
        internal static IPluginHost m_host = null;
        internal static Process currentProcess = Process.GetCurrentProcess();

        internal static readonly ProcessPriorityClass defaultProcessPriority = ProcessPriorityClass.Normal;
        internal static readonly bool isRealtimeAvailable = false;
        internal static readonly bool isDefaultPriorityBoostSettable = false;
        internal static readonly PriorityBoostTypesKSP defaultPriorityBoost = PriorityBoostTypesKSP.Enabled; // Priority boost when the program starts up

        #region Inactive processes monitoring thread stuff
        /*
          The way this works is that there is a thread that is running every x milliseconds to check whether the programs
          for the inactiveDependantPrograms list is running. The list it runs and the data it uses depends on updateThreadDataClass
          
          There is a updateThreadDataClass that determines what values does the thread use, which can be changed by settings UpdateThreadDataClass
        */

        private static readonly Thread updateThread = new Thread(new ThreadStart(UpdateThreadCode));
        /// <summary>
        /// Main mutex, used to control whether the thread will run
        /// </summary>
        private static readonly Mutex updateThreadLock = new Mutex();
        /// <summary>
        /// KeeSetPriorityData instance that is used for the data for the update function
        /// </summary>
        private static KeeSetPriorityData updateThreadDataClass = null;
        /// <summary>
        /// This locking object is used to synchronize reads and writes to updateThreadDataClass with the Monitor class
        /// </summary>
        private static readonly object updateThreadDataClassLock = new object();
        /// <summary>
        /// Used to set what KeeSetPriorityData instance is used as data for the update function
        /// </summary>
        internal static KeeSetPriorityData UpdateThreadDataClass
        {
            set
            {
                Monitor.Enter(updateThreadDataClassLock);
                updateThreadDataClass = value;
                Monitor.Exit(updateThreadDataClassLock);
            }
        }

        internal static void SetInactivePriorityOnStartup()
        {
            // Sets the current priority class on the value that was saved on changePriorityOnInactive but only if that value isn't default
            if (updateThreadDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.AlwaysSet)
            {
                try
                {
                    KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)updateThreadDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive);
                    KeeSetPriorityData.currentProcess.Refresh();
                    if (KeeSetPriorityData.currentProcess.PriorityClass != (ProcessPriorityClass)updateThreadDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive))
                    {
                        throw new KSPException(null);
                    }
                }
                catch (Exception ex)
                {
                    // Windows doesn't let unelevated processes set their priority to Realtime, and will defer them to High
                    MessageBox.Show("Error setting the process priority to " + updateThreadDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive).ToString() +
                        "\n\nSetting priority to " + KeeSetPriorityData.currentProcess.PriorityClass.ToString() + "\n\nSaved settings are not changed\n\nFull error:" + ex.Message,
                        KeeSetPriorityTextStrings.ErrorBoxTitleStr, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else if(updateThreadDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.SetWhenDependent)
            {
                updateThread.Start();
            }
            // There is no need to set for PriorityChangeTypes.NeverSet, as the program by definition is that way by default before we mess with it
        }

        /// <summary>
        /// This function should be called when the value of priorityModeOnInactive is changed in the configWindow
        /// </summary>
        internal static void SetInactivePriorityOnChange()
        {
            // Sets the current priority class on the value that was saved on changePriorityOnInactive but only if that value isn't default
            if (updateThreadDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.AlwaysSet)
            {
                updateThreadLock.WaitOne(); // Stops the update thread
                try
                {
                    KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)updateThreadDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive);
                }
                catch (Exception ex)
                {
                    // Windows doesn't let unelevated processes set their priority to Realtime, and will defer them to High
                    MessageBox.Show("Error setting the process priority to " + updateThreadDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive).ToString() +
                        "\n\nSetting priority to " + KeeSetPriorityData.currentProcess.PriorityClass.ToString() + "\n\nSaved settings are not changed\n\nFull error:" + ex.Message,
                        KeeSetPriorityTextStrings.ErrorBoxTitleStr, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else if (updateThreadDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.SetWhenDependent)
            {
                try
                {
                    updateThread.Start(); // Will throw a ThreadStateException if it's already started
                }
                catch (ThreadStateException)
                {
                    updateThreadLock.ReleaseMutex(); // Restarts the update thread after being stopped
                }
            }
            else if (updateThreadDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.NeverSet)
            {
                updateThreadLock.WaitOne(); // Stops the update thread
                KeeSetPriorityData.currentProcess.PriorityClass = KeeSetPriorityData.defaultProcessPriority;
            }
        }

        internal static void UpdateThreadCode()
        {
            while (true)
            {
                // MessageBox.Show("Thread trying to run");
                updateThreadLock.WaitOne();
                Monitor.Enter(updateThreadDataClassLock);
                // MessageBox.Show("Thread is running");
                if(!(updateThreadDataClass.configDataStruct.GetDependantPrograms(ActionTypesKSP.Inactive, false) == null || updateThreadDataClass.configDataStruct.GetDependantPrograms(ActionTypesKSP.Inactive, false).Length == 0))
                {
                    foreach (string processName in KeeSetPriorityData.updateThreadDataClass.configDataStruct.GetDependantPrograms(ActionTypesKSP.Inactive, false))
                    {
                        Process[] processes = Process.GetProcessesByName(processName);
                        if (processes.Length != 0)
                        {
                            try
                            {
                                KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)updateThreadDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive);
                            }
                            catch (Exception ex)
                            {
                                // Windows doesn't let unelevated processes set their priority to Realtime, and will defer them to High
                                MessageBox.Show("Error setting the process priority to " + updateThreadDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive).ToString() +
                                    "\n\nSetting priority to " + KeeSetPriorityData.currentProcess.PriorityClass.ToString() + "\n\nSaved settings are not changed\n\nFull error:" + ex.Message,
                                    KeeSetPriorityTextStrings.ErrorBoxTitleStr, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            }
                            KeeSetPriorityData.DisposeArray(processes);
                            break;
                        }
                        else
                        {
                            KeeSetPriorityData.currentProcess.PriorityClass = KeeSetPriorityData.defaultProcessPriority;
                        }
                        KeeSetPriorityData.DisposeArray(processes);
                    }
                }
                else
                {
                    KeeSetPriorityData.currentProcess.PriorityClass = KeeSetPriorityData.defaultProcessPriority;
                }
                updateThreadLock.ReleaseMutex();
                Monitor.Exit(updateThreadDataClassLock);
                Thread.Sleep(updateThreadDataClass.configDataStruct.updateThreadTime);
            }
        }
        #endregion

        // This struct is the only variable that's not static, as it needs to be able to 
        internal ConfigDataHoldingStruct configDataStruct;
        internal struct ConfigDataHoldingStruct
        {
            internal bool isAdvancedOptionsAvailable;
            internal AllowDangerousPrioritites allowDangerousPrioritites;
            internal PriorityBoostTypesKSP priorityBoostState;            // Priority boost currently programmed
            internal AllowBackgroundSystemProcesses allowNonWindowedProcesses;

            private PriorityChangeTypes priorityModeOnOpen;
            private PriorityChangeTypes priorityModeOnSave;
            private PriorityChangeTypes priorityModeOnInactive;

            /// <summary>
            /// Returns the value of the corresponding priorityModeOn*
            /// </summary>
            /// <param name="action"></param>
            /// <returns>PriorityChangeTypes</returns>
            /// <exception cref="IndexOutOfRangeException"></exception>
            internal PriorityChangeTypes GetPriorityMode(ActionTypesKSP action)
            {
                switch (action)
                {
                    case ActionTypesKSP.Open:
                        return priorityModeOnOpen;
                    case ActionTypesKSP.Save:
                        return priorityModeOnSave;
                    case ActionTypesKSP.Inactive:
                        return priorityModeOnInactive;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            /// <summary>
            /// Sets the value of the corresponding priorityModeOn*
            /// </summary>
            /// <param name="action"></param>
            /// <param name="changeType"></param>
            /// <exception cref="IndexOutOfRangeException"></exception>
            internal void SetPriorityMode(ActionTypesKSP action, PriorityChangeTypes changeType)
            {
                switch (action)
                {
                    case ActionTypesKSP.Open:
                        priorityModeOnOpen = changeType;
                        break;
                    case ActionTypesKSP.Save:
                        priorityModeOnSave = changeType;
                        break;
                    case ActionTypesKSP.Inactive:
                        priorityModeOnInactive = changeType;
                        break;
                    case ActionTypesKSP.AllActions:
                        SetPriorityMode(ActionTypesKSP.Open, changeType);
                        SetPriorityMode(ActionTypesKSP.Save, changeType);
                        SetPriorityMode(ActionTypesKSP.Inactive, changeType);
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            private ProcessPriorityClassKSP priorityLevelOnOpen;
            private ProcessPriorityClassKSP priorityLevelOnSave;
            private ProcessPriorityClassKSP priorityLevelOnInactive;

            /// <summary>
            /// Returns the value of the corresponding priorityLevelOn*
            /// </summary>
            /// <param name="action"></param>
            /// <returns>ProcessPriorityClassKSP</returns>
            /// <exception cref="IndexOutOfRangeException"></exception>
            internal ProcessPriorityClassKSP GetPriorityLevel(ActionTypesKSP action)
            {
                switch (action)
                {
                    case ActionTypesKSP.Open:
                        return priorityLevelOnOpen;
                    case ActionTypesKSP.Save:
                        return priorityLevelOnSave;
                    case ActionTypesKSP.Inactive:
                        return priorityLevelOnInactive;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            /// <summary>
            /// Sets the value of the corresponding priorityLevelOn*
            /// </summary>
            /// <param name="action"></param>
            /// <param name="priority"></param>
            /// <exception cref="IndexOutOfRangeException"></exception>
            internal void SetPriorityLevel(ActionTypesKSP action, ProcessPriorityClassKSP priority)
            {
                if (priority == ProcessPriorityClassKSP.Default)
                {
                    SetPriorityLevel(action, ProcessPriorityClassKSP.NotSet);
                }
                else
                {
                    switch (action)
                    {
                        case ActionTypesKSP.Open:
                            priorityLevelOnOpen = priority;
                            break;
                        case ActionTypesKSP.Save:
                            priorityLevelOnSave = priority;
                            break;
                        case ActionTypesKSP.Inactive:
                            priorityLevelOnInactive = priority;
                            break;
                        case ActionTypesKSP.AllActions:
                            SetPriorityLevel(ActionTypesKSP.Open, priority);
                            SetPriorityLevel(ActionTypesKSP.Save, priority);
                            SetPriorityLevel(ActionTypesKSP.Inactive, priority);
                            break;
                        default:
                            throw new IndexOutOfRangeException();
                    }
                }
            }

            private string[] openStringVector;
            private string[] saveStringVector;
            private string[] inactiveStringVector;

            /// <summary>
            /// Returns an array with the same members as the corresponding *StringArray array. 
            /// </summary>
            /// <param name="action"></param>
            /// <param name="returnOriginalArray">If returnOriginalArray is false, the array returned is a clone, so any changes made to the array do not 
            /// copy over to the original array (to prevent any accidental-writing errors.), otherwise it returns the original array (modifying this return will
            /// change the original values of the corresponding *StringArray array)</param>
            /// <returns>string[]</returns>
            /// <exception cref="IndexOutOfRangeException"></exception>
            internal string[] GetStringVector(ActionTypesKSP action, bool returnOriginalArray)
            {
                switch (action)
                {
                    case ActionTypesKSP.Open:
                        if (returnOriginalArray)
                        {
                            return openStringVector;
                        }
                        else
                        {
                            return (string[])openStringVector.Clone();
                        }
                    case ActionTypesKSP.Save:
                        if (returnOriginalArray)
                        {
                            return saveStringVector;
                        }
                        else
                        {
                            return (string[])saveStringVector.Clone();
                        }
                    case ActionTypesKSP.Inactive:
                        if (returnOriginalArray)
                        {
                            return inactiveStringVector;
                        }
                        else
                        {
                            return (string[])inactiveStringVector.Clone();
                        }
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            /// <summary>
            /// Sets the corresponding *StringVector variable based on the action parameter.
            /// </summary>
            /// <param name="action">Hola</param>
            /// <param name="stringArray">Adios</param>
            /// <param name="isDeepCopy">Sets whether it performs a deep copy (copies the individual string references from stringArray to
            /// another array) or a shallow copy (the string[] reference to the original array is copied)</param>
            /// <exception cref="IndexOutOfRangeException"></exception>
            internal void SetStringVector(ActionTypesKSP action, string[] stringArray, bool isDeepCopy)
            {
                switch (action)
                {
                    case ActionTypesKSP.Open:
                        if (stringArray == null)
                        {
                            openStringVector = null;
                        }
                        else if (isDeepCopy)
                        {
                            openStringVector = new string[stringArray.Length];
                            stringArray.CopyTo(openDependentPrograms, 0);
                        }
                        else
                        {
                            openStringVector = stringArray;
                        }
                        break;
                    case ActionTypesKSP.Save:
                        if (stringArray == null)
                        {
                            saveStringVector = null;
                        }
                        else
                        {
                            saveStringVector = new string[stringArray.Length];
                            stringArray.CopyTo(saveStringVector, 0);
                        }
                        break;
                    case ActionTypesKSP.Inactive:
                        if (stringArray == null)
                        {
                            inactiveStringVector = null;
                        }
                        else
                        {
                            inactiveStringVector = new string[stringArray.Length];
                            stringArray.CopyTo(inactiveStringVector, 0);
                        }
                        break;
                    case ActionTypesKSP.AllActions:
                        SetStringVector(ActionTypesKSP.Open, stringArray, isDeepCopy);
                        SetStringVector(ActionTypesKSP.Save, stringArray, isDeepCopy);
                        SetStringVector(ActionTypesKSP.Inactive, stringArray, isDeepCopy);
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            private string[] openDependentPrograms;
            private string[] saveDependentPrograms;
            private string[] inactiveDependentPrograms;
            // Since inactiveDependentPrograms is accesses by different threads at the same time, reads and writes must be coordinated 

            /// <summary>
            /// Returns an array with the same members as the corresponding *DependantPrograms array. 
            /// </summary>
            /// <param name="action"></param>
            /// <param name="returnOriginalArray">If returnOriginalArray is false, the array returned is a clone, so any changes made to the array do not 
            /// copy over to the original array (to prevent any accidental-writing errors.), otherwise it returns the original array (modifying this return will
            /// change the original values of the corresponding *DependantPrograms array)</param>
            /// <returns>string[]</returns>
            /// <exception cref="IndexOutOfRangeException"></exception>
            /// <remarks>Note: it is not thread-safe to get an array with returnOriginalArray = true, so secondary threads should use
            /// returnOriginalArray = false to get a clone of the array instead</remarks>
            internal string[] GetDependantPrograms(ActionTypesKSP action, bool returnOriginalArray)
            {
                switch (action)
                {
                    case ActionTypesKSP.Open:
                        if (returnOriginalArray)
                        {
                            return openDependentPrograms;
                        }
                        else
                        {
                            return (string[])(openDependentPrograms == null ? null : openDependentPrograms.Clone());
                        }
                    case ActionTypesKSP.Save:
                        if (returnOriginalArray)
                        {
                            return saveDependentPrograms;
                        }
                        else
                        {
                            return (string[])(saveDependentPrograms == null ? null : saveDependentPrograms.Clone());
                        }
                    case ActionTypesKSP.Inactive:
                        if (returnOriginalArray)
                        {
                            return inactiveDependentPrograms;
                        }
                        else
                        {
                            return (string[])(inactiveDependentPrograms == null ? null : inactiveDependentPrograms.Clone());
                        }
                        
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            /// <summary>
            /// Sets the corresponding *DependantPrograms variable based on the action parameter.
            /// </summary>
            /// <param name="action">Hola</param>
            /// <param name="dependantPrograms">Adios</param>
            /// <param name="isDeepCopy">Sets whether it performs a deep copy (copies the individual string references from dependantPrograms to
            /// another array) or a shallow copy (the string[] reference to the original array is copied)</param>
            /// <exception cref="IndexOutOfRangeException"></exception>
            internal void SetDependantPrograms(ActionTypesKSP action, string[] dependantPrograms, bool isDeepCopy)
            {
                switch (action)
                {
                    case ActionTypesKSP.Open:
                        if (dependantPrograms == null)
                        {
                            openDependentPrograms = null;
                        }
                        else if (isDeepCopy)
                        {
                            openDependentPrograms = new string[dependantPrograms.Length];
                            dependantPrograms.CopyTo(openDependentPrograms, 0);
                        }
                        else
                        {
                            openDependentPrograms = dependantPrograms;
                        }
                        break;
                    case ActionTypesKSP.Save:
                        if (dependantPrograms == null)
                        {
                            saveDependentPrograms = null;
                        }
                        else
                        {
                            saveDependentPrograms = new string[dependantPrograms.Length];
                            dependantPrograms.CopyTo(saveDependentPrograms, 0);
                        }
                        break;
                    case ActionTypesKSP.Inactive:
                        if(dependantPrograms == null)
                        {
                            inactiveDependentPrograms = null;
                        }
                        else
                        {
                            inactiveDependentPrograms = new string[dependantPrograms.Length];
                            dependantPrograms.CopyTo(inactiveDependentPrograms, 0);
                        }
                        break;
                    case ActionTypesKSP.AllActions:
                        SetDependantPrograms(ActionTypesKSP.Open, dependantPrograms, isDeepCopy);
                        SetDependantPrograms(ActionTypesKSP.Save, dependantPrograms, isDeepCopy);
                        SetDependantPrograms(ActionTypesKSP.Inactive, dependantPrograms, isDeepCopy);
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            // Update thread related stuff
            internal int updateThreadTime;
        }


        #region KeePass custom settings RW methods
        /// <summary>
        /// This function will read the settings and validate them
        /// </summary>
        /// <returns>True if the read and validation was correct, false if there was a fatal error</returns>
        internal bool ReadAndValidateSettings()
        {
            try
            {
                switch (CheckLegacySettings())
                {
                    case LegacyVersions.current:
                        ReadSettings();
                        break;
                    case LegacyVersions.v0_5:
                        OnLegacySettings(LegacyVersions.v0_5);
                        break;
                    case LegacyVersions.v0_8:
                        OnLegacySettings(LegacyVersions.v0_8);
                        break;
                }
                ValidateSettings();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads the main data (for the current version of KeeSetPriority). If the data is unreadable, then it will show a message to the user to ask
        /// for permission to turn that setting to its default value. If the user declines, a KSPException is thrown
        /// </summary>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="KSPException"></exception>
        private void ReadSettings()
        {
            ReadMainSettings(ActionTypesKSP.AllActions);

            #region Read and parse advanced options data
            try { configDataStruct.isAdvancedOptionsAvailable = bool.Parse(m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.isAdvancedOptionsAvailableString, bool.FalseString)); }
            catch
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.AdvancedOptionsDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    configDataStruct.isAdvancedOptionsAvailable = false;
                    configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
                    configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
                }
                else
                {
                    throw new KSPException();
                }
            }
            try { this.configDataStruct.allowDangerousPrioritites = (AllowDangerousPrioritites)Enum.Parse(typeof(AllowDangerousPrioritites), m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.allowDangerousPrioritiesString, AllowDangerousPrioritites.No.ToString())); }
            catch
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.AllowDangerousPrioritiesDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
                }
                else
                {
                    throw new KSPException();
                }
            }
            try { this.configDataStruct.priorityBoostState = (PriorityBoostTypesKSP)Enum.Parse(typeof(PriorityBoostTypesKSP), m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityBoostString, PriorityBoostTypesKSP.Default.ToString())); }
            catch
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.PriorityBoostDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
                }
                else
                {
                    throw new KSPException();
                }
            }
            try { this.configDataStruct.allowNonWindowedProcesses = (AllowBackgroundSystemProcesses)Enum.Parse(typeof(AllowBackgroundSystemProcesses), m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.allowBackgroundSystemProcessesString, AllowBackgroundSystemProcesses.No.ToString())); }
            catch
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.PriorityBoostDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
                }
                else
                {
                    throw new KSPException();
                }
            }
            // Update thread settings
            // They're all going to be stored as one setting separated by commas, allowing for expansion in the future
            try
            {
                string[] updateThreadVector = m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.updateThreadSettingsString, 5000.ToString()).Split(',');
                this.configDataStruct.updateThreadTime = int.Parse(updateThreadVector[0]);
            }
            catch
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.UpdateThreadSettingsCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    configDataStruct.updateThreadTime = 5000;
                }
                else
                {
                    throw new KSPException();
                }
            }
            #endregion
        }

        private void ReadMainSettings(ActionTypesKSP action)
        {
            if(action == ActionTypesKSP.AllActions)
            {
                ReadMainSettings(ActionTypesKSP.Open);
                ReadMainSettings(ActionTypesKSP.Save);
                ReadMainSettings(ActionTypesKSP.Inactive);
            }
            else
            {
                this.configDataStruct.SetStringVector(action, m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.GetChangePriorityString(action), PriorityChangeTypes.NeverSet.ToString()).Split(','), false);
                try
                {
                    this.configDataStruct.SetPriorityMode(action, (PriorityChangeTypes)Enum.Parse(typeof(PriorityChangeTypes), this.configDataStruct.GetStringVector(action, true)[0], true));
                    if ((this.configDataStruct.GetPriorityMode(action) != PriorityChangeTypes.NeverSet && this.configDataStruct.GetStringVector(action, true).Length == 1) ||
                        (this.configDataStruct.GetPriorityMode(action) == PriorityChangeTypes.SetWhenDependent && this.configDataStruct.GetStringVector(action, true).Length < 3))
                    {
                        throw new KSPException();
                    }
                    if (this.configDataStruct.GetPriorityMode(action) == PriorityChangeTypes.AlwaysSet)
                    {
                        this.configDataStruct.SetPriorityLevel(action, (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), this.configDataStruct.GetStringVector(action, true)[1]));
                    }
                    else if (configDataStruct.GetPriorityMode(action) == PriorityChangeTypes.SetWhenDependent)
                    {
                        this.configDataStruct.SetPriorityLevel(action, (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), this.configDataStruct.GetStringVector(action, true)[1]));
                        string[] intermediate = new string[configDataStruct.GetStringVector(action, true).Length - 2];
                        Array.Copy(configDataStruct.GetStringVector(action, true), 2, intermediate, 0, this.configDataStruct.GetStringVector(action, true).Length - 2);
                        this.configDataStruct.SetDependantPrograms(action, intermediate, false);
                    }
                }
                catch
                {

                    if (KeeSetPriorityTextStrings.GetDataCorrectionWarning(action) == DialogResult.Yes)
                    {
                        configDataStruct.SetPriorityLevel(action, ProcessPriorityClassKSP.NotSet);
                        configDataStruct.SetPriorityMode(action, PriorityChangeTypes.NeverSet);
                    }
                    else
                    {
                        throw new KSPException();
                    }
                }
            }
        }

        /// <summary>
        /// This function will validate whether the data is congruent with itself. If the data is unreadable, then it will show a message to the user to ask
        /// for permission to turn that setting to its default value. If the user declines, a KSPException is thrown
        /// </summary>
        /// <exception cref="KSPException"></exception>
        private void ValidateSettings()
        {
            // Check for disagreements between PriorityMode and PriorityLevel
            if (this.configDataStruct.GetPriorityMode(ActionTypesKSP.Open) != PriorityChangeTypes.NeverSet && this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Open) == ProcessPriorityClassKSP.NotSet)
                throw new KSPException();
            if (this.configDataStruct.GetPriorityMode(ActionTypesKSP.Save) != PriorityChangeTypes.NeverSet && this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Save) == ProcessPriorityClassKSP.NotSet)
                throw new KSPException();
            if (this.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) != PriorityChangeTypes.NeverSet && this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive) == ProcessPriorityClassKSP.NotSet)
                throw new KSPException();

            if (this.configDataStruct.GetPriorityMode(ActionTypesKSP.Open) == PriorityChangeTypes.NeverSet && this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Open) != ProcessPriorityClassKSP.NotSet)
                throw new KSPException();
            if (this.configDataStruct.GetPriorityMode(ActionTypesKSP.Save) == PriorityChangeTypes.NeverSet && this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Save) != ProcessPriorityClassKSP.NotSet)
                throw new KSPException();
            if (this.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.NeverSet && this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive) != ProcessPriorityClassKSP.NotSet)
                throw new KSPException();

            // Check for invalid advanced options
            if (!this.configDataStruct.isAdvancedOptionsAvailable && (this.configDataStruct.allowDangerousPrioritites != AllowDangerousPrioritites.No || configDataStruct.priorityBoostState != PriorityBoostTypesKSP.Default))
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.AdvancedOptionsDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    configDataStruct.isAdvancedOptionsAvailable = false;
                    configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
                    configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
                }
                else
                {
                    throw new KSPException();
                }
            }

            // Check for invalid Realtime priorities
            if (this.configDataStruct.allowDangerousPrioritites != AllowDangerousPrioritites.Yes &&
                (this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Open) == ProcessPriorityClassKSP.RealTime || this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Save) == ProcessPriorityClassKSP.RealTime || this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive) == ProcessPriorityClassKSP.RealTime) )
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.AllowDangerousPrioritiesDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Open) == ProcessPriorityClassKSP.RealTime) { this.configDataStruct.SetPriorityLevel(ActionTypesKSP.Open, ProcessPriorityClassKSP.High); }
                    if (this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Save) == ProcessPriorityClassKSP.RealTime) { this.configDataStruct.SetPriorityLevel(ActionTypesKSP.Save, ProcessPriorityClassKSP.High); }
                    if (this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive) == ProcessPriorityClassKSP.RealTime) { this.configDataStruct.SetPriorityLevel(ActionTypesKSP.Inactive, ProcessPriorityClassKSP.High); }
                }
                else
                {
                    throw new KSPException();
                }
            }
            // Check for invalid high priorities
            if (this.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.No &&
                (this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Open) == ProcessPriorityClassKSP.High || this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Save) == ProcessPriorityClassKSP.High || this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive) == ProcessPriorityClassKSP.High) )
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.AllowDangerousPrioritiesDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Open) == ProcessPriorityClassKSP.High) { this.configDataStruct.SetPriorityLevel(ActionTypesKSP.Open, ProcessPriorityClassKSP.Normal); }
                    if (this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Save) == ProcessPriorityClassKSP.High) { this.configDataStruct.SetPriorityLevel(ActionTypesKSP.Save, ProcessPriorityClassKSP.Normal); }
                    if (this.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive) == ProcessPriorityClassKSP.High) { this.configDataStruct.SetPriorityLevel(ActionTypesKSP.Inactive, ProcessPriorityClassKSP.Normal); }
                }
                else
                {
                    throw new KSPException();
                }
            }
        }

        internal void WriteSettings()
        {
            WriteMainSettings(ActionTypesKSP.AllActions);

            m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityBoostString, this.configDataStruct.priorityBoostState.ToString());
            m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.isAdvancedOptionsAvailableString, this.configDataStruct.isAdvancedOptionsAvailable.ToString());
            m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.allowDangerousPrioritiesString, this.configDataStruct.allowDangerousPrioritites.ToString());
            m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.allowBackgroundSystemProcessesString, this.configDataStruct.allowNonWindowedProcesses.ToString());
            m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.updateThreadSettingsString, this.configDataStruct.updateThreadTime.ToString());
        }

        private void WriteMainSettings(ActionTypesKSP action)
        {
            if(action == ActionTypesKSP.AllActions)
            {
                WriteMainSettings(ActionTypesKSP.Open);
                WriteMainSettings(ActionTypesKSP.Save);
                WriteMainSettings(ActionTypesKSP.Inactive);
            }
            else
            {
                if (this.configDataStruct.GetPriorityMode(action) == PriorityChangeTypes.NeverSet)
                {
                    m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.GetChangePriorityString(action), this.configDataStruct.GetPriorityMode(action).ToString());
                }
                else if (this.configDataStruct.GetPriorityMode(action) == PriorityChangeTypes.AlwaysSet)
                {
                    m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.GetChangePriorityString(action), this.configDataStruct.GetPriorityMode(action).ToString() + ',' + this.configDataStruct.GetPriorityLevel(action).ToString());
                }
                else if (this.configDataStruct.GetPriorityMode(action) == PriorityChangeTypes.SetWhenDependent)
                {
                    m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.GetChangePriorityString(action), this.configDataStruct.GetPriorityMode(action).ToString() + ',' + this.configDataStruct.GetPriorityLevel(action).ToString() + ',' + GetStringFromVector(this.configDataStruct.GetDependantPrograms(action, true), ','));
                }
            }
        }
        #endregion

        #region Interface methods and miscelaneous functions
        /// <summary>
        /// This is needed to be able to dispose of currentProcess.
        /// Only call when the plugin is about to exit since currentProcess is static
        /// </summary>
        public void Dispose() { KeeSetPriorityData.StaticDispose(); }
        private static void StaticDispose()
        {
            updateThreadDataClass = null;
            currentProcess.Dispose();
        }
        /// <summary>
        /// Returns a separate copy of a KeeSetPriorityData instance
        /// </summary>
        /// <returns>A new instance of the same object of the type KeeSetPriorityData with the same data as the caller instance</returns>
        public object Clone()
        {
            KeeSetPriorityData clone = new KeeSetPriorityData
            {
                // Since this is a struct, the struct is copied instead of referenced
                configDataStruct = this.configDataStruct
            };
            return clone;
        }

        /// <summary>
        /// Disposes of an array of IDisposable items
        /// </summary>
        /// <param name="array">Array of IDisposable items to dispose</param>
        internal static void DisposeArray(IDisposable[] array)
        {
            foreach (IDisposable obj in array)
            {
                obj.Dispose();
            }
        }
        
        /// <summary>
        /// Returns string with each member of a string[] separated by the delimitor
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="delimitor"></param>
        /// <returns>string with the elements separated by the delimitor</returns>
        private static string GetStringFromVector(string[] vector, char delimitor)
        {
            string stringResult = null;
            foreach (String str in vector)
            {
                if(stringResult == null)
                {
                    stringResult = str;
                }
                else
                {
                    stringResult += delimitor + str;
                }
            }
            return stringResult;
        }
        #endregion
    }

    #region Configuration and state enums
    internal enum ProcessPriorityClassKSP
    {
        Default = 0, //For legacy purposes, do not use
        RealTime = ProcessPriorityClass.RealTime,
        High = ProcessPriorityClass.High,
        AboveNormal = ProcessPriorityClass.AboveNormal,
        Normal = ProcessPriorityClass.Normal,
        BelowNormal = ProcessPriorityClass.BelowNormal,
        Idle = ProcessPriorityClass.Idle,
        NotSet = -1
    }

    internal enum PriorityChangeTypes
    {
        NeverSet = 0,
        AlwaysSet = 1,
        SetWhenDependent = 2,
    }

    internal enum PriorityBoostTypesKSP
    {
        Enabled = 1,
        Disabled = 2,
        Default = 4
    }

    internal enum ActionTypesKSP
    {
        Open = 0,
        Save = 1,
        Inactive = 2,

        AllActions = 0xFF
    }

    internal enum AllowDangerousPrioritites
    {
        No = 0,
        Yes = 1,
        OnlyHigh = 2
    }

    internal enum AllowBackgroundSystemProcesses
    {
        No = 0,
        OnlyBackground = 1,
        BackgroundAndSystem = 2
    }
    #endregion
}
