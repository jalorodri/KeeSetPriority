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

        KeeSetPriorityData dataStruct;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null)
                return false;

            dataStruct = new KeeSetPriorityData(host);

            #region Load and test saved data for correctness
            // If a variable is not numeric, it may be badly corrupted; rewrite all the variables just in case
            try
            {
                // ReadAndValidateSettings() will throw an exception if variables are unable to be parsed
                // or in case the data is not valid (corrupted and/or user-modified)
                dataStruct.ReadAndValidateSettings();
            }
            catch (Exception ex)
            {
                if (!dataStruct.OnIncongruentSettings(ex))
                {
                    // OnIncongruentSettings(ex) returns false when the user selects not to continue
                    return false;
                }
            }
            #endregion

            dataStruct.m_host.MainWindow.FileSavingPre += OnFileSavePre;
            dataStruct.m_host.MainWindow.FileSaved += OnFileSavePost;
            // Apparently there isn't an event for just before opening a database, so detecting when KeePass accesses the database file suffices
            IOConnection.IOAccessPre += OnFileOpenPre;
            dataStruct.m_host.MainWindow.FileOpened += OnFileOpenPost;

            // Sets the current priority class on the value that was saved on changePriorityOnInactive but only if that value isn't default
            if (dataStruct.changePriorityOnInactive != 0)
            {
                dataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)dataStruct.changePriorityOnInactive;
                dataStruct.currentProcess.Refresh();
                if (dataStruct.currentProcess.PriorityClass != (ProcessPriorityClass)dataStruct.changePriorityOnInactive)
                {
                    // Windows doesn't let unelevated processes set their priority to Realtime, and will defer them to High
                    MessageBox.Show("Error setting the process priority to " + dataStruct.changePriorityOnInactive.ToString() + 
                        "\n\nSetting priority to " + dataStruct.currentProcess.PriorityClass.ToString(), KeeSetPriorityTextStrings.ErrorBoxTitleStr, 
                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            
            return true;
        }

        public override void Terminate()
        {
            dataStruct.WriteSettings();
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
                using (SettingsWindow settingsWindow = new SettingsWindow(dataStruct))
                {
                    if (settingsWindow.ShowDialog() == DialogResult.OK)
                    {
                        dataStruct = settingsWindow.dataStruct;
                    }
                }
            };
            return MainButtonToolsPanel;
        }

        #region Open and save functions
        private void OnFileSavePre(object sender, FileSavingEventArgs e)
        {
            if (dataStruct.changePriorityOnSave == 0)
            {
                // Safely sets it back to inactive priority
                OnFileSavePost();
            }
            else
            {
                dataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)dataStruct.changePriorityOnSave;
            }
        }

        private void OnFileSavePost(object sender = null, FileSavedEventArgs e = null)
        {
            if (dataStruct.changePriorityOnInactive == 0)
            {
                // defaultProcessPriority is guaranteed to be part of ProcessPriorityClass
                dataStruct.currentProcess.PriorityClass = dataStruct.defaultProcessPriority;
            }
            else
            {
                dataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)dataStruct.changePriorityOnInactive;
            }
        }

        private void OnFileOpenPre(object sender, IOAccessEventArgs e)
        {
            if (e.Type == IOAccessType.Read && e.IOConnectionInfo.Path.EndsWith("." + AppDefs.FileExtension.FileExt))
            {
                if (dataStruct.changePriorityOnOpen == 0)
                {
                    // Safely sets to inactive priority
                    OnFileOpenPost();
                }
                else
                {
                    dataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)dataStruct.changePriorityOnOpen;
                }
            }
        }

        private void OnFileOpenPost(object sender = null, FileOpenedEventArgs e = null)
        {
            if (dataStruct.changePriorityOnInactive == 0)
            {
                dataStruct.currentProcess.PriorityClass = dataStruct.defaultProcessPriority;
            }
            else
            {
                dataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)dataStruct.changePriorityOnInactive;
            }
        }
        #endregion
    }
}
