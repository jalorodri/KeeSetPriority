using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeeSetPriority
{
    public partial class SettingsWindow : Form
    {
        // Set variables
        internal KeeSetPriorityData configDataStruct;

        private string[] comboboxNames;

        private bool initializationDone = false;
        public SettingsWindow(KeeSetPriorityData initdata)
        {
            configDataStruct = initdata;
            InitializeComponent();

            // Set the configDataStruct to allow it to be modified
            ProcessPicker.SetDataStruct(ref configDataStruct);
            // saveProcessPicker
            this.saveProcessPicker = new KeeSetPriority.ProcessPicker(ActionTypesKSP.Save)
            {
                Location = new System.Drawing.Point(6, 87),
                Name = "saveProcessPicker",
                Size = new System.Drawing.Size(560, 300),
                TabIndex = 0
            };
            this.saveTabPage.Controls.Add(this.saveProcessPicker);
            // openProcessPicker
            this.openProcessPicker = new KeeSetPriority.ProcessPicker(ActionTypesKSP.Open)
            {
                Location = new System.Drawing.Point(6, 87),
                Name = "openProcessPicker",
                Size = new System.Drawing.Size(560, 300),
                TabIndex = 0
            };
            this.openTabPage.Controls.Add(this.openProcessPicker);
            // inactiveProcessPicker
            this.inactiveProcessPicker = new KeeSetPriority.ProcessPicker(ActionTypesKSP.Inactive)
            {
                Location = new System.Drawing.Point(6, 87),
                Name = "inactiveProcessPicker",
                Size = new System.Drawing.Size(560, 300),
                TabIndex = 0

            };
            this.inactiveTabPage.Controls.Add(this.inactiveProcessPicker);
        }

        // For when loading the dialog
        // TO DO: use configDataStruct to set the correct settings on load
        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            // Set appropiate labels for dropdowns
            GetPriorityDropdowns();
            saveSettingsComboBox.Items.Clear();
            saveSettingsComboBox.Items.AddRange(comboboxNames);
            openSettingsComboBox.Items.Clear();
            openSettingsComboBox.Items.AddRange(comboboxNames);
            inactiveSettingsComboBox.Items.Clear();
            inactiveSettingsComboBox.Items.AddRange(comboboxNames);
            
            // Set save settings
            if (configDataStruct.priorityModeOnSave == PriorityChangeTypes.NeverSet)
            {
                saveDefaultRadioButton.Checked = true;
                saveSetPriorityRadioButton.Checked = false;
                saveSetDependantPriorityRadioButton.Checked = false;
                saveSettingsComboBox.Enabled = false;
                saveProcessPicker.Enabled = false;
            }
            else if (configDataStruct.priorityModeOnSave == PriorityChangeTypes.AlwaysSet)
            {
                saveDefaultRadioButton.Checked = false;
                saveSetPriorityRadioButton.Checked = true;
                saveSetDependantPriorityRadioButton.Checked = false;
                saveSettingsComboBox.Enabled = true;
                saveProcessPicker.Enabled = false;
            }
            else if (configDataStruct.priorityModeOnSave == PriorityChangeTypes.SetWhenDependent)
            {
                saveDefaultRadioButton.Checked = false;
                saveSetPriorityRadioButton.Checked = false;
                saveSetDependantPriorityRadioButton.Checked = true;
                saveSettingsComboBox.Enabled = true;
                saveProcessPicker.Enabled = true;
            }
            SetPriorityDropdownsOnStartup(ActionTypesKSP.Save);
            // Set open settings
            if (configDataStruct.priorityModeOnOpen == PriorityChangeTypes.NeverSet)
            {
                openDefaultRadioButton.Checked = true;
                openSetPriorityRadioButton.Checked = false;
                openSetDependantPriorityRadioButton.Checked = false;
                openSettingsComboBox.Enabled = false;
                openProcessPicker.Enabled = false;
            }
            else if(configDataStruct.priorityModeOnOpen == PriorityChangeTypes.AlwaysSet)
            {
                openDefaultRadioButton.Checked = false;
                openSetPriorityRadioButton.Checked = true;
                openSetDependantPriorityRadioButton.Checked = false;
                openSettingsComboBox.Enabled = true;
                openProcessPicker.Enabled = false;
            }
            else if (configDataStruct.priorityModeOnOpen == PriorityChangeTypes.SetWhenDependent)
            {
                openDefaultRadioButton.Checked = false;
                openSetPriorityRadioButton.Checked = false;
                openSetDependantPriorityRadioButton.Checked = true;
                openSettingsComboBox.Enabled = true;
                openProcessPicker.Enabled = true;
            }
            SetPriorityDropdownsOnStartup(ActionTypesKSP.Open);
            // Set inactive settings
            if (configDataStruct.priorityModeOnInactive == PriorityChangeTypes.NeverSet)
            {
                inactiveDefaultPriorityRadioButton.Checked = true;
                inactiveSetPriorityRadioButton.Checked = false;
                inactiveSetDependantPriorityRadioButton.Checked = false;
                inactiveSettingsComboBox.Enabled = false;
                inactiveProcessPicker.Enabled = false;
            }
            else if (configDataStruct.priorityModeOnInactive == PriorityChangeTypes.AlwaysSet)
            {
                inactiveDefaultPriorityRadioButton.Checked = false;
                inactiveSetPriorityRadioButton.Checked = true;
                inactiveSetDependantPriorityRadioButton.Checked = false;
                inactiveSettingsComboBox.Enabled = true;
                inactiveProcessPicker.Enabled = false;
            }
            else if (configDataStruct.priorityModeOnInactive == PriorityChangeTypes.SetWhenDependent)
            {
                inactiveDefaultPriorityRadioButton.Checked = false;
                inactiveSetPriorityRadioButton.Checked = false;
                inactiveSetDependantPriorityRadioButton.Checked = true;
                inactiveSettingsComboBox.Enabled = true;
                inactiveProcessPicker.Enabled = true;
            }
            SetPriorityDropdownsOnStartup(ActionTypesKSP.Inactive);
            currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)configDataStruct.currentProcess.PriorityClass);
            currentPriorityGroupBox.Visible = currentPriorityGroupBox.Enabled = false;

            // Set advanced settings panel
            advancedGroupBox.Enabled = configDataStruct.isAdvancedOptionsAvailable;
            dialogEnableAdvancedSettings.Checked = configDataStruct.isAdvancedOptionsAvailable;

            // Set high and realtime overrides
            allowHighPriorityButton.Checked = allowRealtimePriorityButton.Enabled = (configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes || configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh);
            allowRealtimePriorityButton.Checked = configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes;

            // Set priority boost
            if (configDataStruct.isDefaultPriorityBoostSettable)
            {
                setPriorityBoostButton.Enabled = setPriorityBoostDropdown.Enabled = true;
                setPriorityBoostButton.Checked = configDataStruct.priorityBoostState != PriorityBoostTypesKSP.Default;
                setPriorityBoostDropdown.Enabled = (configDataStruct.isDefaultPriorityBoostSettable && setPriorityBoostButton.Checked);
                priorityBoostStatusLabel.Text = configDataStruct.currentProcess.PriorityBoostEnabled ? KeeSetPriorityTextStrings.EnabledStr : KeeSetPriorityTextStrings.DisabledStr;
            }
            else
            {
                priorityBoostStatusLabel.Text = KeeSetPriorityTextStrings.NotAvailableStr;
                setPriorityBoostButton.Enabled = setPriorityBoostDropdown.Enabled = false;
                setPriorityBoostButton.Checked = configDataStruct.priorityBoostState != PriorityBoostTypesKSP.Default;
            }
            initializationDone = true;
        }

        // TODO: create proper tooltips
        private void SettingsTooltip_Popup(object sender, PopupEventArgs e)
        {
            if (setPriorityBoostButton.Enabled)
            {
                settingsTooltip.SetToolTip(setPriorityBoostButton, null);
            }
            else
            {
                settingsTooltip.SetToolTip(setPriorityBoostButton, "");
            }
        }

        #region Helper functions
        // Value is written to comboboxNames
        private void GetPriorityDropdowns()
        {
            if (configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes)
            {
                comboboxNames = new string[] {
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.RealTime),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.High),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.AboveNormal),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.Normal),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.BelowNormal),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.Idle),
                };
            }
            else if (configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh)
            {
                comboboxNames = new string[] {
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.High),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.AboveNormal),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.Normal),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.BelowNormal),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.Idle),
                };
            }
            else
            {
                comboboxNames = new string[] {
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.AboveNormal),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.Normal),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.BelowNormal),
                    KeeSetPriorityTextStrings.GetPriorityLevelStr(ProcessPriorityClassKSP.Idle),
                };
            }
        }

        // Called by OnPriorityDropdownChangeIndex
        private void OnPriorityDropdownSetValues(ProcessPriorityClassKSP prio, ActionTypesKSP action)
        {
            switch (action)
            {
                case ActionTypesKSP.Open:
                    configDataStruct.priorityLevelOnOpen = prio;
                    break;
                case ActionTypesKSP.Save:
                    configDataStruct.priorityLevelOnSave = prio;
                    break;
                case ActionTypesKSP.Inactive:
                    configDataStruct.priorityLevelOnInactive = prio;
                    configDataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)prio;
                    configDataStruct.currentProcess.Refresh();
                    currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)configDataStruct.currentProcess.PriorityClass);
                    break;
            }
        }

        // Called every time an index is changed
        private void OnPriorityDropdownChangeIndex(int index, ActionTypesKSP action)
        {
            if (configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes)
            {
                switch (index)
                {
                    case 0:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.RealTime, action);
                        break;
                    case 1:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.High, action);
                        break;
                    case 2:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.AboveNormal, action);
                        break;
                    case 3:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.Normal, action);
                        break;
                    case 4:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.BelowNormal, action);
                        break;
                    case 5:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.Idle, action);
                        break;
                }
            }
            else if (configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh)
            {
                switch (index)
                {
                    case 0:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.High, action);
                        break;
                    case 1:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.AboveNormal, action);
                        break;
                    case 2:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.Normal, action);
                        break;
                    case 3:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.BelowNormal, action);
                        break;
                    case 4:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.Idle, action);
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 0:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.AboveNormal, action);
                        break;
                    case 1:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.Normal, action);
                        break;
                    case 2:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.BelowNormal, action);
                        break;
                    case 3:
                        OnPriorityDropdownSetValues(ProcessPriorityClassKSP.Idle, action);
                        break;
                }
            }
        }

        private void SetPriorityDropdownsOnStartup(ActionTypesKSP action)
        {
            ComboBox comboBox = null;
            ProcessPriorityClassKSP prio = ProcessPriorityClassKSP.NotSet;
            switch (action)
            {
                case ActionTypesKSP.Open:
                    comboBox = openSettingsComboBox;
                    prio = configDataStruct.priorityLevelOnOpen;
                    break;
                case ActionTypesKSP.Save:
                    comboBox = saveSettingsComboBox;
                    prio = configDataStruct.priorityLevelOnSave;
                    break;
                case ActionTypesKSP.Inactive:
                    comboBox = inactiveSettingsComboBox;
                    prio = configDataStruct.priorityLevelOnInactive;
                    break;
            }
            if(prio == ProcessPriorityClassKSP.NotSet)
            {
                comboBox.SelectedIndex = -1;
            }
            else if (configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes)
            {
                switch (prio)
                {
                    case ProcessPriorityClassKSP.RealTime:
                        comboBox.SelectedIndex = 0;
                        break;
                    case ProcessPriorityClassKSP.High:
                        comboBox.SelectedIndex = 1;
                        break;
                    case ProcessPriorityClassKSP.AboveNormal:
                        comboBox.SelectedIndex = 2;
                        break;
                    case ProcessPriorityClassKSP.Normal:
                        comboBox.SelectedIndex = 3;
                        break;
                    case ProcessPriorityClassKSP.BelowNormal:
                        comboBox.SelectedIndex = 4;
                        break;
                    case ProcessPriorityClassKSP.Idle:
                        comboBox.SelectedIndex = 5;
                        break;
                }
            }
            else if (configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh)
            {
                switch (prio)
                {
                    case ProcessPriorityClassKSP.High:
                        comboBox.SelectedIndex = 0;
                        break;
                    case ProcessPriorityClassKSP.AboveNormal:
                        comboBox.SelectedIndex = 1;
                        break;
                    case ProcessPriorityClassKSP.Normal:
                        comboBox.SelectedIndex = 2;
                        break;
                    case ProcessPriorityClassKSP.BelowNormal:
                        comboBox.SelectedIndex = 3;
                        break;
                    case ProcessPriorityClassKSP.Idle:
                        comboBox.SelectedIndex = 4;
                        break;
                }
            }
            else
            {
                switch (prio)
                {
                    case ProcessPriorityClassKSP.AboveNormal:
                        comboBox.SelectedIndex = 0;
                        break;
                    case ProcessPriorityClassKSP.Normal:
                        comboBox.SelectedIndex = 1;
                        break;
                    case ProcessPriorityClassKSP.BelowNormal:
                        comboBox.SelectedIndex = 2;
                        break;
                    case ProcessPriorityClassKSP.Idle:
                        comboBox.SelectedIndex = 3;
                        break;
                }
            }
        }

        #endregion

        #region Save tab functions
        private void SaveSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                configDataStruct.priorityModeOnSave = PriorityChangeTypes.AlwaysSet;
                OnPriorityDropdownChangeIndex(saveSettingsComboBox.SelectedIndex, ActionTypesKSP.Save);
            }
        }

        private void SaveDefaultRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && saveDefaultRadioButton.Checked)
            {
                saveSettingsComboBox.SelectedIndex = -1;
                saveSettingsComboBox.Enabled = false;
                saveProcessPicker.Enabled = false;
                configDataStruct.priorityModeOnSave = PriorityChangeTypes.NeverSet;
                configDataStruct.priorityLevelOnSave = ProcessPriorityClassKSP.NotSet;
            }
        }

        private void SaveSetPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && saveSetPriorityRadioButton.Checked)
            {
                saveSettingsComboBox.Enabled = true;
                saveProcessPicker.Enabled = false;
            }
        }
        private void SaveSetDependantPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && saveSetDependantPriorityRadioButton.Checked)
            {
                saveSettingsComboBox.Enabled = true;
                saveProcessPicker.Enabled = true;
            }
        }

        #endregion

        #region Open tab functions

        private void OpenSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                configDataStruct.priorityModeOnOpen = PriorityChangeTypes.AlwaysSet;
                OnPriorityDropdownChangeIndex(openSettingsComboBox.SelectedIndex, ActionTypesKSP.Open);
            }
        }

        private void OpenDefaultRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && openDefaultRadioButton.Checked)
            {
                openSettingsComboBox.SelectedIndex = -1;
                openSettingsComboBox.Enabled = false;
                openProcessPicker.Enabled = false;
                configDataStruct.priorityModeOnOpen = PriorityChangeTypes.NeverSet;
                configDataStruct.priorityLevelOnOpen = ProcessPriorityClassKSP.NotSet;
            }
        }

        private void OpenSetPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && openSetPriorityRadioButton.Checked)
            {
                openSettingsComboBox.Enabled = true;
                openProcessPicker.Enabled = false;
            }
        }

        private void OpenSetDependantPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && openSetDependantPriorityRadioButton.Checked)
            {
                openSettingsComboBox.Enabled = true;
                openProcessPicker.Enabled = true;
            }
        }

        #endregion

        #region Inactive tab functions

        private void InactiveSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                configDataStruct.priorityModeOnInactive = PriorityChangeTypes.AlwaysSet;
                OnPriorityDropdownChangeIndex(inactiveSettingsComboBox.SelectedIndex, ActionTypesKSP.Inactive);
                configDataStruct.currentProcess.Refresh();
                currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)configDataStruct.currentProcess.PriorityClass);
            }
        }

        private void InactiveDefaultPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && inactiveDefaultPriorityRadioButton.Checked)
            {
                inactiveSettingsComboBox.SelectedIndex = -1;
                inactiveSettingsComboBox.Enabled = false;
                configDataStruct.priorityModeOnInactive = PriorityChangeTypes.NeverSet;
                configDataStruct.priorityLevelOnInactive = ProcessPriorityClassKSP.NotSet;
                inactiveProcessPicker.Enabled = false;
                configDataStruct.currentProcess.PriorityClass = configDataStruct.defaultProcessPriority;
            }
            configDataStruct.currentProcess.Refresh();
            currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)configDataStruct.currentProcess.PriorityClass);
        }

        private void InactiveSetPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && inactiveSetPriorityRadioButton.Checked)
            {
                inactiveSettingsComboBox.Enabled = true;
                inactiveProcessPicker.Enabled = false;
            }
        }

        private void InactiveSetDependantPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && inactiveSetDependantPriorityRadioButton.Checked)
            {
                inactiveSettingsComboBox.Enabled = true;
                inactiveProcessPicker.Enabled = true;
            }
        }

        #endregion

        #region Advanced tab functions

        // When the priority boost dropdown index is changed
        private void SetPriorityBoostDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (setPriorityBoostDropdown.SelectedIndex == 0) // Enabled
            {
                configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Enabled;
            }
            else if (setPriorityBoostDropdown.SelectedIndex == 1) //disabled
            {
                configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Disabled;
            }
            configDataStruct.currentProcess.PriorityBoostEnabled = configDataStruct.priorityBoostState == PriorityBoostTypesKSP.Enabled;
            configDataStruct.currentProcess.Refresh();
            priorityBoostStatusLabel.Text = configDataStruct.currentProcess.PriorityBoostEnabled ? KeeSetPriorityTextStrings.EnabledStr : KeeSetPriorityTextStrings.DisabledStr;
        }

        // When the priority boost check is changed
        private void SetPriorityBoostButton_CheckedChanged(object sender, EventArgs e)
        {
            setPriorityBoostDropdown.SelectedIndex = -1; // Unset
            configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
            setPriorityBoostDropdown.Enabled = setPriorityBoostButton.Checked;
        }

        private void DialogEnableAdvancedSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                if (dialogEnableAdvancedSettings.Checked == true)
                {
                    DialogResult warningDialogAdvanced = MessageBox.Show(KeeSetPriorityTextStrings.AdvancedSettingsBoxStr, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (warningDialogAdvanced == DialogResult.Yes)
                    {
                        advancedGroupBox.Enabled = true;
                        configDataStruct.isAdvancedOptionsAvailable = true;
                    }
                    else
                    {
                        advancedGroupBox.Enabled = false;
                        dialogEnableAdvancedSettings.Checked = false;
                    }
                }
                else
                {
                    advancedGroupBox.Enabled = false;
                    configDataStruct.isAdvancedOptionsAvailable = false;
                }
            }
        }

        private void AllowRealtimePriorityButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                if (allowRealtimePriorityButton.Checked)
                {
                    DialogResult warningDialogRealtime = MessageBox.Show(KeeSetPriorityTextStrings.RealtimePriorityAdvancedSettingsString, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if(warningDialogRealtime == DialogResult.Yes)
                    {
                        configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.Yes;
                    }
                    else
                    {
                        allowRealtimePriorityButton.Checked = false;
                    }
                }
                else if(allowHighPriorityButton.Checked)
                {
                    configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.OnlyHigh;
                }
                else
                {
                    configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
                }
                GetPriorityDropdowns();
                saveSettingsComboBox.Items.Clear();
                saveSettingsComboBox.Items.AddRange(comboboxNames);
                openSettingsComboBox.Items.Clear();
                openSettingsComboBox.Items.AddRange(comboboxNames);
                inactiveSettingsComboBox.Items.Clear();
                inactiveSettingsComboBox.Items.AddRange(comboboxNames);
                SetPriorityDropdownsOnStartup(ActionTypesKSP.Save);
                SetPriorityDropdownsOnStartup(ActionTypesKSP.Open);
                SetPriorityDropdownsOnStartup(ActionTypesKSP.Inactive);
            }
        }

        private void AllowHighPriorityButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                if (allowHighPriorityButton.Checked)
                {
                    configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.OnlyHigh;
                    allowRealtimePriorityButton.Enabled = true;
                }
                else
                {
                    configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
                    allowRealtimePriorityButton.Checked = allowRealtimePriorityButton.Enabled = false;
                }
                GetPriorityDropdowns();
                saveSettingsComboBox.Items.Clear();
                saveSettingsComboBox.Items.AddRange(comboboxNames);
                openSettingsComboBox.Items.Clear();
                openSettingsComboBox.Items.AddRange(comboboxNames);
                inactiveSettingsComboBox.Items.Clear();
                inactiveSettingsComboBox.Items.AddRange(comboboxNames);
                SetPriorityDropdownsOnStartup(ActionTypesKSP.Save);
                SetPriorityDropdownsOnStartup(ActionTypesKSP.Open);
                SetPriorityDropdownsOnStartup(ActionTypesKSP.Inactive);
            }
        }

        #endregion

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(initializationDone && mainTabControl.SelectedTab == inactiveTabPage)
            {
                currentPriorityGroupBox.Visible = currentPriorityGroupBox.Enabled = true;
            }
            else
            {
                currentPriorityGroupBox.Visible = currentPriorityGroupBox.Enabled = false;
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            using (AboutForm aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog();
            }
        }
    }
}
