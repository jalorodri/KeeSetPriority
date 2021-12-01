using System;
using System.Diagnostics;
using System.Windows.Forms;

using KeePass.App;
using KeePass.Forms;
using KeePass.Plugins;
using KeePass.UI;

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

        KeeSetPriorityData mainDataStruct;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null)
                return false;

            mainDataStruct = new KeeSetPriorityData(host);

            #region Load and test saved data for correctness
            // If a variable is not numeric, it may be badly corrupted; rewrite all the variables just in case
            try
            {
                // ReadAndValidateSettings() will throw an exception if variables are unable to be parsed
                // or in case the data is not valid (corrupted and/or user-modified)
                mainDataStruct.ReadAndValidateSettings();
            }
            catch (Exception ex)
            {
                if (!mainDataStruct.OnIncongruentSettings(ex))
                {
                    // OnIncongruentSettings(ex) returns false when the user selects not to continue
                    return false;
                }
            }
            #endregion

            mainDataStruct.m_host.MainWindow.FileSavingPre += OnFileSavePre;
            mainDataStruct.m_host.MainWindow.FileSaved += OnFileSavePost;
            // Apparently there isn't an event for just before opening a database, so detecting when KeePass accesses the database file suffices
            IOConnection.IOAccessPre += OnFileOpenPre;
            mainDataStruct.m_host.MainWindow.FileOpened += OnFileOpenPost;

            // Sets the current priority class on the value that was saved on changePriorityOnInactive but only if that value isn't default
            if (mainDataStruct.priorityModeOnInactive == PriorityChangeTypes.AlwaysSet)
            {
                mainDataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataStruct.priorityLevelOnInactive;
                mainDataStruct.currentProcess.Refresh();
                if (mainDataStruct.currentProcess.PriorityClass != (ProcessPriorityClass)mainDataStruct.priorityLevelOnInactive)
                {
                    // Windows doesn't let unelevated processes set their priority to Realtime, and will defer them to High
                    MessageBox.Show("Error setting the process priority to " + mainDataStruct.priorityLevelOnInactive.ToString() + 
                        "\n\nSetting priority to " + mainDataStruct.currentProcess.PriorityClass.ToString() + "\n\nSaved settings are not changed", 
                        KeeSetPriorityTextStrings.ErrorBoxTitleStr, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            return true;
        }

        public override void Terminate()
        {
            mainDataStruct.WriteSettings();
            mainDataStruct.currentProcess.Dispose();
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
                using (SettingsWindow settingsWindow = new SettingsWindow(mainDataStruct))
                {
                    if (settingsWindow.ShowDialog() == DialogResult.OK)
                    {
                        mainDataStruct = settingsWindow.configDataStruct;
                    }
                }
            };
            return MainButtonToolsPanel;
        }

        #region Open and save functions
        private void OnFileSavePre(object sender, FileSavingEventArgs e)
        {
            if (mainDataStruct.priorityModeOnSave == PriorityChangeTypes.NeverSet)
            {
                // Safely sets it back to inactive priority
                OnFileSavePost();
            }
            else if (mainDataStruct.priorityModeOnSave == PriorityChangeTypes.AlwaysSet)
            {
                mainDataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataStruct.priorityLevelOnSave;
            }
            else if (mainDataStruct.priorityModeOnSave == PriorityChangeTypes.SetWhenDependent)
            {

            }
        }

        private void OnFileSavePost(object sender = null, FileSavedEventArgs e = null)
        {
            if (mainDataStruct.priorityModeOnInactive == PriorityChangeTypes.NeverSet)
            {
                // defaultProcessPriority is guaranteed to be part of ProcessPriorityClass
                mainDataStruct.currentProcess.PriorityClass = mainDataStruct.defaultProcessPriority;
            }
            else if (mainDataStruct.priorityModeOnInactive == PriorityChangeTypes.AlwaysSet)
            {
                mainDataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataStruct.priorityLevelOnInactive;
            }
        }

        private void OnFileOpenPre(object sender, IOAccessEventArgs e)
        {
            if (e.Type == IOAccessType.Read && e.IOConnectionInfo.Path.EndsWith("." + AppDefs.FileExtension.FileExt))
            {
                if (mainDataStruct.priorityModeOnOpen == PriorityChangeTypes.NeverSet)
                {
                    // Safely sets to inactive priority
                    OnFileOpenPost();
                }
                else if (mainDataStruct.priorityModeOnOpen == PriorityChangeTypes.AlwaysSet)
                {
                    mainDataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataStruct.priorityLevelOnOpen;
                }
            }
        }

        private void OnFileOpenPost(object sender = null, FileOpenedEventArgs e = null)
        {
            if (mainDataStruct.priorityModeOnInactive == PriorityChangeTypes.NeverSet)
            {
                mainDataStruct.currentProcess.PriorityClass = mainDataStruct.defaultProcessPriority;
            }
            else if (mainDataStruct.priorityModeOnInactive == PriorityChangeTypes.AlwaysSet)
            {
                mainDataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)mainDataStruct.priorityLevelOnInactive;
            }
        }
        #endregion
    }
}
