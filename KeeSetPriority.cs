using System;
using System.Diagnostics;
using System.Windows.Forms;

using KeePass.App;
using KeePass.Forms;
using KeePass.Plugins;

using KeePassLib;
using KeePassLib.Serialization;

namespace KeeSetPriority
{
    public sealed class KeeSetPriorityExt : Plugin
    {
        // DO NOT DEFINE AS A VARIABLE
        public override string UpdateUrl
        {
            get { return "https://raw.githubusercontent.com/jalorodri/KeeSetPriority/master/version.info"; }
        }

        KeeSetPriorityData mainDataClass;

        public override bool Initialize(IPluginHost host)
        {
            try
            {
                if (host == null)
                {
                    return false;
                }

                mainDataClass = new KeeSetPriorityData(host);
                KeeSetPriorityData.UpdateThreadDataClass = mainDataClass;

                if (!mainDataClass.ReadAndValidateSettings())
                {
                    throw new KSPException();
                }

                KeeSetPriorityData.SetInactivePriorityOnStartup();

                KeeSetPriorityData.m_host.MainWindow.FileSavingPre += OnFileSavePre;
                KeeSetPriorityData.m_host.MainWindow.FileSaved += OnFileSavePost;
                // Apparently there isn't an event for just before opening a database, so detecting when KeePass accesses the database file suffices
                IOConnection.IOAccessPre += OnFileOpenPre;
                KeeSetPriorityData.m_host.MainWindow.FileOpened += OnFileOpenPost;

                return true;
            }
            catch
            {
                // An unhandled exception is not survivable by the plugin
                mainDataClass.Dispose();
                return false;
            }
        }

        // While KeePass automatically calls Terminate, it does so before the update tag thread is deleted, which gives an exception for the last cycle of the thread loop
        // As such, if we place the Dispose in Terminate we get an unhandled exception
        ~KeeSetPriorityExt()
        {
            // Use the destructor to do stuff that needs to be done dead last, like releasing IDisposable resources
            mainDataClass.Dispose();
        }

        /// <summary>
        /// To be executed (automatically by KeePass) when the program exits successfully
        /// </summary>
        public override void Terminate()
        {
            // Use this method to do stuff that depends on KeePass, like writing settings to disk
            mainDataClass.WriteSettings();
        }

        public override ToolStripMenuItem GetMenuItem(PluginMenuType t)
        {
            if (t != PluginMenuType.Main)
            {
                return null;
            }

            // Main button
            ToolStripMenuItem MainButtonToolsPanel = new ToolStripMenuItem(KeeSetPriorityTextStrings.FormalNameStr);
            MainButtonToolsPanel.Click += delegate (object sender, EventArgs e)
            {
                // We want a second version of the class with a different but initially-identical configDataStruct
                using (ConfigWindow settingsWindow = new ConfigWindow((KeeSetPriorityData)mainDataClass.Clone()))
                {
                    if (settingsWindow.ShowDialog() == DialogResult.OK)
                    {
                        // If the user wants to save the settings, the new main class is the one from settingsWindow and the old mainDataClass is discarted
                        mainDataClass = settingsWindow.configWindowDataClass;
                    }
                    else
                    {
                        KeeSetPriorityData.UpdateThreadDataClass = mainDataClass;
                    }
                }
            };
            return MainButtonToolsPanel;
        }

        #region Open and save functions
        private void OnFileSavePre(object sender, FileSavingEventArgs e)
        {
            if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Save) == PriorityChangeTypes.NeverSet)
            {
                // Safely sets it back to inactive priority
                OnFileSavePost();
            }
            else if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Save) == PriorityChangeTypes.AlwaysSet)
            {
                KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Save);
            }
            else if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Save) == PriorityChangeTypes.SetWhenDependent)
            {
                SetWhenDependantPre(ActionTypesKSP.Save);
            }
        }

        private void OnFileSavePost(object sender = null, FileSavedEventArgs e = null)
        {
            if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.NeverSet)
            {
                // defaultProcessPriority is guaranteed to be part of ProcessPriorityClass
                KeeSetPriorityData.currentProcess.PriorityClass = KeeSetPriorityData.defaultProcessPriority;
            }
            else if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.AlwaysSet)
            {
                KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive);
            }
            else if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.SetWhenDependent)
            {
                SetWhenDependantPost();
            }
        }

        private void OnFileOpenPre(object sender, IOAccessEventArgs e)
        {
            if (e.Type == IOAccessType.Read && e.IOConnectionInfo.Path.EndsWith("." + AppDefs.FileExtension.FileExt))
            {
                if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Open) == PriorityChangeTypes.NeverSet)
                {
                    // Safely sets to inactive priority
                    OnFileOpenPost();
                }
                else if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Open) == PriorityChangeTypes.AlwaysSet)
                {
                    KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Open);
                }
                else if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Open) == PriorityChangeTypes.SetWhenDependent)
                {
                    SetWhenDependantPre(ActionTypesKSP.Open);
                }
            }
        }

        private void OnFileOpenPost(object sender = null, FileOpenedEventArgs e = null)
        {
            if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.NeverSet)
            {
                KeeSetPriorityData.currentProcess.PriorityClass = KeeSetPriorityData.defaultProcessPriority;
            }
            else if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.AlwaysSet)
            {
                KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive);
            }
            else if (mainDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.SetWhenDependent)
            {
                SetWhenDependantPost();
            }
        }

        private void SetWhenDependantPre(ActionTypesKSP action)
        {
            if (!(mainDataClass.configDataStruct.GetDependantPrograms(action, false) == null || mainDataClass.configDataStruct.GetDependantPrograms(action, false).Length == 0))
            {
                foreach (string processName in mainDataClass.configDataStruct.GetDependantPrograms(action, false))
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    if (processes.Length != 0)
                    {
                        try
                        {
                            KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataClass.configDataStruct.GetPriorityLevel(action);
                        }
                        catch (Exception ex)
                        {
                            // Windows doesn't let unelevated processes set their priority to Realtime, and will defer them to High
                            MessageBox.Show("Error setting the process priority to " + mainDataClass.configDataStruct.GetPriorityLevel(action).ToString() +
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
        }

        private void SetWhenDependantPost()
        {
            if (!(mainDataClass.configDataStruct.GetDependantPrograms(ActionTypesKSP.Inactive, false) == null || mainDataClass.configDataStruct.GetDependantPrograms(ActionTypesKSP.Inactive, false).Length == 0))
            {
                foreach (string processName in mainDataClass.configDataStruct.GetDependantPrograms(ActionTypesKSP.Inactive, false))
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    if (processes.Length != 0)
                    {
                        try
                        {
                            KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive);
                        }
                        catch (Exception ex)
                        {
                            // Windows doesn't let unelevated processes set their priority to Realtime, and will defer them to High
                            MessageBox.Show("Error setting the process priority to " + mainDataClass.configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive).ToString() +
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
        }
        #endregion
    }
}
