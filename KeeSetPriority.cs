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

        #region Class variables and strings
        private IPluginHost m_host;

        private int changePriorityOnOpen = 0;
        private const string changePriorityOnOpenString = "KeeSetPriority.changePriorityOnOpen";
        private int changePriorityOnSave = 0;
        private const string changePriorityOnSaveString = "KeeSetPriority.changePriorityOnSave";
        private int changePriorityOnInactive = 0;
        private const string changePriorityOnInactiveString = "KeeSetPriority.changePriorityOnInactive";

        private bool isRealtimeAvailable = true;

        private Process currentProcess;
        private ProcessPriorityClass defaultProcessPriority;

        private const string DefaultLevelStr = "Default";
        private const string RealtimeStr = "Realtime";
        private const string HighStr = "High";
        private const string AboveNormalStr = "Above normal";
        private const string NormalStr = "Normal";
        private const string BelowNormalStr = "Below Normal";
        private const string IdleStr = "Idle";

        private const string FormalName = "KeeSetPriority";
        private const string OpenPhrase = "Set priority level on database open";
        private const string SavePhrase = "Set priority level on database save";
        private const string InactivePhrase = "Set priority level while inactive";
        private const string ErrorBoxTitle = "KeeSetPriority Error";
        private const string WarningBoxTitle = "KeeSetPriority Warning";

        private const string HighPriorityMessage1 = "You are about to set the priority to ";
        private const string HighPriorityMessage2 = ".\n\nThis could cause the computer to stop responding or lock up ";
        private const string InactivePriorityMessage = "while the program is running.";
        private const string OpeningPriorityMessage = "while the program is opening a database.";
        private const string SavingPriorityMessage = "while the program is saving a database.";
        private const string ConfirmationMessage = "\n\nAre you sure you want to continue?";
        private const string RealtimeTooltip = "Realtime priority not available; " +
            "KeePass needs administrator privileges to enable realtime priority";
        #endregion

        public override bool Initialize(IPluginHost host)
        {
            if (host == null)
                return false;
            m_host = host;

            #region Load and test saved data for correctness
            // If a variable is not numeric, it may be badly corrupted; rewrite all the variables just in case
            try
            {
                // int.Parse will throw an exception if variables are unable to be parsed
                // and we'll throw our own exception in case the values are not valid
                changePriorityOnOpen = int.Parse(m_host.CustomConfig.GetString(changePriorityOnOpenString, "0"));
                changePriorityOnSave = int.Parse(m_host.CustomConfig.GetString(changePriorityOnSaveString, "0"));
                changePriorityOnInactive = int.Parse(m_host.CustomConfig.GetString(changePriorityOnInactiveString, "0"));

                if (changePriorityOnOpen != 0 && !Enum.IsDefined(typeof(ProcessPriorityClass), changePriorityOnOpen)) 
                    throw new Exception("changePriorityOnOpen is not 0 or a valid ProcessPriorityClass value");
                if (changePriorityOnSave != 0 && !Enum.IsDefined(typeof(ProcessPriorityClass), changePriorityOnSave)) 
                    throw new Exception("changePriorityOnSave is not 0 or a valid ProcessPriorityClass value");
                if (changePriorityOnInactive != 0 && !Enum.IsDefined(typeof(ProcessPriorityClass), changePriorityOnInactive)) 
                    throw new Exception("changePriorityOnInactive is not 0 or a valid ProcessPriorityClass value");
            }
            catch (Exception ex)
            {
                DialogResult warningBox = MessageBox.Show("Settings are not readable and may be corrupted.\n\n" +
                    "Do you want to set them to default values? Previous values will be overwritten.\n\nFull error:\n" + ex.Message, 
                    ErrorBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                if (warningBox == DialogResult.Yes)
                {
                    m_host.CustomConfig.SetString(changePriorityOnOpenString, "0");
                    m_host.CustomConfig.SetString(changePriorityOnSaveString, "0");
                    m_host.CustomConfig.SetString(changePriorityOnInactiveString, "0");
                    changePriorityOnOpen = 0;
                    changePriorityOnSave = 0;
                    changePriorityOnInactive = 0;
                }
                else
                {
                    // Cannot continue with corrupted data, do not load the plugin
                    return false;
                }
            }
            #endregion

            m_host.MainWindow.FileSavingPre += OnFileSavePre;
            m_host.MainWindow.FileSaved += OnFileSavePost;
            // Apparently there isn't an event for just before opening a database, so detecting when KeePass accesses the database file suffices
            IOConnection.IOAccessPre += OnFileOpenPre;
            m_host.MainWindow.FileOpened += OnFileOpenPost;

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
                MessageBox.Show("Error reading and writing priority level from process. This may indicate problems with the application " +
                    "or the OS." + "\n\nThe plugin KeeSetPriority will not load.\n\nProceed at your own risk." + "\n\nFull exception:\n" 
                    + e.Message, ErrorBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check for realtime availability
            currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
            currentProcess.Refresh();
            isRealtimeAvailable = (currentProcess.PriorityClass == ProcessPriorityClass.RealTime);
            currentProcess.PriorityClass = defaultProcessPriority;

            // Sets the current priority class on the value that was saved on changePriorityOnInactive but only if that value isn't default
            if (changePriorityOnInactive != 0)
            {
                currentProcess.PriorityClass = (ProcessPriorityClass)changePriorityOnInactive;
                currentProcess.Refresh();
                if (currentProcess.PriorityClass != (ProcessPriorityClass)changePriorityOnInactive)
                {
                    // Windows doesn't let unelevated processes set their priority to Realtime, and will defer them to High
                    MessageBox.Show("Error setting the process priority to " + ((ProcessPriorityClass)changePriorityOnInactive).ToString() + 
                        "\n\nSetting priority to " + currentProcess.PriorityClass.ToString(), ErrorBoxTitle, 
                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            
            return true;
        }

        public override void Terminate()
        {
            m_host.CustomConfig.SetString(changePriorityOnOpenString, changePriorityOnOpen.ToString());
            m_host.CustomConfig.SetString(changePriorityOnSaveString, changePriorityOnSave.ToString());
            m_host.CustomConfig.SetString(changePriorityOnInactiveString, changePriorityOnInactive.ToString());
        }

        public override ToolStripMenuItem GetMenuItem(PluginMenuType t)
        {
            if (t != PluginMenuType.Main)
            {
                return null;
            }

            // Main button
            ToolStripMenuItem MainButtonToolsPanel = new ToolStripMenuItem(FormalName);

            #region Inactive buttons
            // On inactive
            ToolStripMenuItem onInactiveMainButton = new ToolStripMenuItem(InactivePhrase);
            ToolStripMenuItem onInactiveDefaultButton = new ToolStripMenuItem(DefaultLevelStr);
            ToolStripSeparator onInactiveSeparator = new ToolStripSeparator();
            ToolStripMenuItem onInactiveRealtime = new ToolStripMenuItem(RealtimeStr);
            ToolStripMenuItem onInactiveHigh = new ToolStripMenuItem(HighStr);
            ToolStripMenuItem onInactiveAboveNormal = new ToolStripMenuItem(AboveNormalStr);
            ToolStripMenuItem onInactiveNormal = new ToolStripMenuItem(NormalStr);
            ToolStripMenuItem onInactiveBelowNormal = new ToolStripMenuItem(BelowNormalStr);
            ToolStripMenuItem onInactiveIdle = new ToolStripMenuItem(IdleStr);
            ToolStripMenuItem[] onInactiveButtons = new ToolStripMenuItem[] { onInactiveDefaultButton, onInactiveRealtime,
                onInactiveHigh, onInactiveAboveNormal, onInactiveNormal, onInactiveBelowNormal, onInactiveIdle };

            for (int i = 0; i < onInactiveButtons.Length; ++i)
            {
                onInactiveButtons[i].Click += delegate (object sender, EventArgs e)
                {
                    if (sender == onInactiveButtons[0])
                    {
                        changePriorityOnInactive = 0;
                        currentProcess.PriorityClass = defaultProcessPriority;
                    }
                    else
                    {
                        if (sender == onInactiveButtons[1])
                        {
                            DialogResult warningBox = MessageBox.Show(HighPriorityMessage1 + RealtimeStr + HighPriorityMessage2 + 
                                InactivePriorityMessage + ConfirmationMessage, WarningBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (warningBox == DialogResult.Yes)
                            {
                                changePriorityOnInactive = (int)ProcessPriorityClass.RealTime;
                            }
                        }
                        else if (sender == onInactiveButtons[2])
                        {
                            DialogResult warningBox = MessageBox.Show(HighPriorityMessage1 + HighStr + HighPriorityMessage2 + 
                                InactivePriorityMessage + ConfirmationMessage, WarningBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (warningBox == DialogResult.Yes)
                            {
                                changePriorityOnInactive = (int)ProcessPriorityClass.High;
                            }
                        }
                        else if (sender == onInactiveButtons[3])
                            changePriorityOnInactive = (int)ProcessPriorityClass.AboveNormal;
                        else if (sender == onInactiveButtons[4])
                            changePriorityOnInactive = (int)ProcessPriorityClass.Normal;
                        else if (sender == onInactiveButtons[5])
                            changePriorityOnInactive = (int)ProcessPriorityClass.BelowNormal;
                        else if (sender == onInactiveButtons[6])
                            changePriorityOnInactive = (int)ProcessPriorityClass.Idle;
                        else
                        {
                            MessageBox.Show("Wrong menu object referenced", ErrorBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        currentProcess.PriorityClass = (ProcessPriorityClass)changePriorityOnInactive;
                        currentProcess.Refresh();
                        if (currentProcess.PriorityClass != (ProcessPriorityClass)changePriorityOnInactive)
                        {
                            // Windows doesn't let unelevated processes set their priority to Realtime, and will defer them to High
                            MessageBox.Show("Error setting priority value to " + ((ProcessPriorityClass)changePriorityOnInactive).ToString() + 
                                "\n\nSetting priority to " + currentProcess.PriorityClass.ToString(), ErrorBoxTitle, 
                                MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                };
            }
            onInactiveMainButton.DropDownItems.AddRange(new ToolStripItem[] { onInactiveDefaultButton, onInactiveSeparator, onInactiveRealtime,
                onInactiveHigh, onInactiveAboveNormal, onInactiveNormal, onInactiveBelowNormal, onInactiveIdle });

            MainButtonToolsPanel.DropDownItems.Add(onInactiveMainButton);
            #endregion

            #region Open buttons
            // On open
            ToolStripMenuItem onOpenMainButton = new ToolStripMenuItem(OpenPhrase);
            ToolStripMenuItem onOpenDefaultButton = new ToolStripMenuItem(DefaultLevelStr);
            ToolStripSeparator onOpenSeparator = new ToolStripSeparator();
            ToolStripMenuItem onOpenRealtime = new ToolStripMenuItem(RealtimeStr);
            ToolStripMenuItem onOpenHigh = new ToolStripMenuItem(HighStr);
            ToolStripMenuItem onOpenAboveNormal = new ToolStripMenuItem(AboveNormalStr);
            ToolStripMenuItem onOpenNormal = new ToolStripMenuItem(NormalStr);
            ToolStripMenuItem onOpenBelowNormal = new ToolStripMenuItem(BelowNormalStr);
            ToolStripMenuItem onOpenIdle = new ToolStripMenuItem(IdleStr);
            ToolStripMenuItem[] onOpenButtons = new ToolStripMenuItem[] { onOpenDefaultButton, onOpenRealtime, onOpenHigh,
                onOpenAboveNormal, onOpenNormal, onOpenBelowNormal, onOpenIdle };

            for (int i = 0; i < onOpenButtons.Length; ++i)
                onOpenButtons[i].Click += delegate (object sender, EventArgs e)
                {
                    if (sender == onOpenButtons[0])
                        changePriorityOnOpen = 0;
                    else if (sender == onOpenButtons[1])
                    {
                        DialogResult warningBox = MessageBox.Show(HighPriorityMessage1 + RealtimeStr + HighPriorityMessage2 + 
                            OpeningPriorityMessage + ConfirmationMessage, WarningBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (warningBox == DialogResult.Yes)
                        {
                            changePriorityOnOpen = (int)ProcessPriorityClass.RealTime;
                        }

                    }
                    else if (sender == onOpenButtons[2])
                    {
                        DialogResult warningBox = MessageBox.Show(HighPriorityMessage1 + HighStr + HighPriorityMessage2 + 
                            OpeningPriorityMessage + ConfirmationMessage, WarningBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (warningBox == DialogResult.Yes)
                        {
                            changePriorityOnOpen = (int)ProcessPriorityClass.High;
                        }
                    }
                    else if (sender == onOpenButtons[3])
                        changePriorityOnOpen = (int)ProcessPriorityClass.AboveNormal;
                    else if (sender == onOpenButtons[4])
                        changePriorityOnOpen = (int)ProcessPriorityClass.Normal;
                    else if (sender == onOpenButtons[5])
                        changePriorityOnOpen = (int)ProcessPriorityClass.BelowNormal;
                    else if (sender == onOpenButtons[6])
                        changePriorityOnOpen = (int)ProcessPriorityClass.Idle;
                    else
                    {
                        MessageBox.Show("Wrong menu object referenced", ErrorBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                };
            onOpenMainButton.DropDownItems.AddRange(new ToolStripItem[] { onOpenDefaultButton, onOpenSeparator, onOpenRealtime,
                onOpenHigh, onOpenAboveNormal, onOpenNormal, onOpenBelowNormal, onOpenIdle });

            MainButtonToolsPanel.DropDownItems.Add(onOpenMainButton);
            #endregion

            #region Save buttons
            // On save
            ToolStripMenuItem onSaveMainButton = new ToolStripMenuItem(SavePhrase);
            ToolStripMenuItem onSaveDefaultButton = new ToolStripMenuItem(DefaultLevelStr);
            ToolStripSeparator onSaveSeparator = new ToolStripSeparator();
            ToolStripMenuItem onSaveRealtime = new ToolStripMenuItem(RealtimeStr);
            ToolStripMenuItem onSaveHigh = new ToolStripMenuItem(HighStr);
            ToolStripMenuItem onSaveAboveNormal = new ToolStripMenuItem(AboveNormalStr);
            ToolStripMenuItem onSaveNormal = new ToolStripMenuItem(NormalStr);
            ToolStripMenuItem onSaveBelowNormal = new ToolStripMenuItem(BelowNormalStr);
            ToolStripMenuItem onSaveIdle = new ToolStripMenuItem(IdleStr);
            ToolStripMenuItem[] onSaveButtons = new ToolStripMenuItem[] { onSaveDefaultButton, onSaveRealtime, onSaveHigh,
                onSaveAboveNormal, onSaveNormal, onSaveBelowNormal, onSaveIdle };

            for (int i = 0; i < onSaveButtons.Length; ++i)
                onSaveButtons[i].Click += delegate (object sender, EventArgs e)
                {
                    if (sender == onSaveButtons[0])
                        changePriorityOnSave = 0;
                    else if (sender == onSaveButtons[1])
                    {
                        DialogResult warningBox = MessageBox.Show(HighPriorityMessage1 + RealtimeStr + HighPriorityMessage2 + 
                            SavingPriorityMessage + ConfirmationMessage, WarningBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (warningBox == DialogResult.Yes)
                        {
                            changePriorityOnSave = (int)ProcessPriorityClass.RealTime;
                        }
                    }
                    else if (sender == onSaveButtons[2])
                    {
                        DialogResult warningBox = MessageBox.Show(HighPriorityMessage1 + HighStr + HighPriorityMessage2 + 
                            SavingPriorityMessage + ConfirmationMessage, WarningBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (warningBox == DialogResult.Yes)
                        {
                            changePriorityOnSave = (int)ProcessPriorityClass.High;
                        }
                    }
                    else if (sender == onSaveButtons[3])
                        changePriorityOnSave = (int)ProcessPriorityClass.AboveNormal;
                    else if (sender == onSaveButtons[4])
                        changePriorityOnSave = (int)ProcessPriorityClass.Normal;
                    else if (sender == onSaveButtons[5])
                        changePriorityOnSave = (int)ProcessPriorityClass.BelowNormal;
                    else if (sender == onSaveButtons[6])
                        changePriorityOnSave = (int)ProcessPriorityClass.Idle;
                    else
                    {
                        MessageBox.Show("Wrong menu object referenced", ErrorBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                };
            onSaveMainButton.DropDownItems.AddRange(new ToolStripItem[] { onSaveDefaultButton, onSaveSeparator, onSaveRealtime,
                onSaveHigh, onSaveAboveNormal, onSaveNormal, onSaveBelowNormal, onSaveIdle });

            MainButtonToolsPanel.DropDownItems.Add(onSaveMainButton);
            #endregion

            // Add separator and inactive button that displays current priority status
            MainButtonToolsPanel.DropDownItems.Add(new ToolStripSeparator());
            // No need to add the menu now, since it's added on main button click anyway
            ToolStripMenuItem currentPriorityStatus = new ToolStripMenuItem(); 
            MainButtonToolsPanel.DropDownItems.Add(currentPriorityStatus);
            currentPriorityStatus.Enabled = false;

            // Set the function to execute when clicking on the button
            MainButtonToolsPanel.DropDownOpening += delegate (object sender, EventArgs e)
            {
                currentProcess.Refresh();
                currentPriorityStatus.Text = "Current process priority: " + currentProcess.PriorityClass.ToString();
                if (isRealtimeAvailable)
                {
                    onInactiveButtons[1].ToolTipText = null;
                }
                else if (currentProcess.PriorityClass == ProcessPriorityClass.RealTime)
                {
                    // Through some Task Manager magic you can set the program to Realtime, or maybe it runs in administrator mode
                    onInactiveButtons[1].ToolTipText = "Current priority was set to Realtime externally (via Task Manager or third party programs)\n"
                         + RealtimeTooltip;
                }
                else if (changePriorityOnInactive == (int)ProcessPriorityClass.RealTime)
                {
                    // If we have Realtime as changePriorityOnInactive
                    onInactiveButtons[1].ToolTipText = "Saved inactive value is Realtime, but current priority value is " + 
                        currentProcess.PriorityClass.ToString() + "\n" + RealtimeTooltip;
                }
                else
                {
                    onInactiveButtons[1].ToolTipText = RealtimeTooltip;
                }
                onInactiveButtons[1].Enabled = isRealtimeAvailable;
                UpdatePriorityCheckmarks(onInactiveButtons, changePriorityOnInactive, (!isRealtimeAvailable &&
                    changePriorityOnInactive == (int)ProcessPriorityClass.RealTime) ? (int)currentProcess.PriorityClass : 1);
                UpdatePriorityCheckmarks(onOpenButtons, changePriorityOnOpen);
                UpdatePriorityCheckmarks(onSaveButtons, changePriorityOnSave);
            };
            return MainButtonToolsPanel;
        }

        private void UpdatePriorityCheckmarks(ToolStripMenuItem[] buttons, int prio, int prio2 = 1)
        {
            for (int i = 0; i < buttons.Length; ++i)
                UIUtil.SetChecked(buttons[i], false);
            switch (prio)
            {
                case 0:
                    UIUtil.SetChecked(buttons[0], true);
                    break;
                case (int)ProcessPriorityClass.RealTime:
                    UIUtil.SetChecked(buttons[1], true);
                    break;
                case (int)ProcessPriorityClass.High:
                    UIUtil.SetChecked(buttons[2], true);
                    break;
                case (int)ProcessPriorityClass.AboveNormal:
                    UIUtil.SetChecked(buttons[3], true);
                    break;
                case (int)ProcessPriorityClass.Normal:
                    UIUtil.SetChecked(buttons[4], true);
                    break;
                case (int)ProcessPriorityClass.BelowNormal:
                    UIUtil.SetChecked(buttons[5], true);
                    break;
                case (int)ProcessPriorityClass.Idle:
                    UIUtil.SetChecked(buttons[6], true);
                    break;
                default:
                    break;
            }
            switch (prio2)
            {
                case 0:
                    UIUtil.SetChecked(buttons[0], true);
                    break;
                case (int)ProcessPriorityClass.RealTime:
                    UIUtil.SetChecked(buttons[1], true);
                    break;
                case (int)ProcessPriorityClass.High:
                    UIUtil.SetChecked(buttons[2], true);
                    break;
                case (int)ProcessPriorityClass.AboveNormal:
                    UIUtil.SetChecked(buttons[3], true);
                    break;
                case (int)ProcessPriorityClass.Normal:
                    UIUtil.SetChecked(buttons[4], true);
                    break;
                case (int)ProcessPriorityClass.BelowNormal:
                    UIUtil.SetChecked(buttons[5], true);
                    break;
                case (int)ProcessPriorityClass.Idle:
                    UIUtil.SetChecked(buttons[6], true);
                    break;
                default:
                    break;
            }
        }

        #region Open and save functions
        private void OnFileSavePre(object sender, FileSavingEventArgs e)
        {
            if (changePriorityOnSave == 0)
            {
                // Safely sets it back to inactive priority
                OnFileSavePost();
            }
            else
            {
                currentProcess.PriorityClass = (ProcessPriorityClass)changePriorityOnSave;
            }
        }

        private void OnFileSavePost(object sender = null, FileSavedEventArgs e = null)
        {
            if (changePriorityOnInactive == 0)
            {
                // defaultProcessPriority is guaranteed to be part of ProcessPriorityClass
                currentProcess.PriorityClass = defaultProcessPriority;
            }
            else
            {
                currentProcess.PriorityClass = (ProcessPriorityClass)changePriorityOnInactive;
            }
        }

        private void OnFileOpenPre(object sender, IOAccessEventArgs e)
        {
            if (e.Type == IOAccessType.Read && e.IOConnectionInfo.Path.EndsWith("." + AppDefs.FileExtension.FileExt))
            {
                if (changePriorityOnOpen == 0)
                {
                    // Safely sets to inactive priority
                    OnFileOpenPost();
                }
                else
                {
                    currentProcess.PriorityClass = (ProcessPriorityClass)changePriorityOnOpen;
                }
            }
        }

        private void OnFileOpenPost(object sender = null, FileOpenedEventArgs e = null)
        {
            if (changePriorityOnInactive == 0)
            {
                currentProcess.PriorityClass = defaultProcessPriority;
            }
            else
            {
                currentProcess.PriorityClass = (ProcessPriorityClass)changePriorityOnInactive;
            }
        }
        #endregion
    }

}
