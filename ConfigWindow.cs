using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace KeeSetPriority
{
    public partial class ConfigWindow : Form
    {
        // Set variables
        internal KeeSetPriorityData configWindowDataClass;

        private string[] comboboxNames;

        private bool initializationDone = false;
        public ConfigWindow(KeeSetPriorityData initdata)
        {
            configWindowDataClass = initdata;
            InitializeComponent();

            KeeSetPriorityData.UpdateThreadDataClass = initdata;

            // Set the configWindowDataClass to allow it to be modified
            ProcessPicker.SetDataStruct(ref configWindowDataClass);

            // saveProcessPicker
            this.saveProcessPicker = new KeeSetPriority.ProcessPicker(ActionTypesKSP.Save)
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(6, 87),
                Name = "saveProcessPicker",
                Size = new System.Drawing.Size(560, 300),
                TabIndex = 0
            };
            this.saveTabPage.Controls.Add(this.saveProcessPicker);
            // openProcessPicker
            this.openProcessPicker = new KeeSetPriority.ProcessPicker(ActionTypesKSP.Open)
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(6, 87),
                Name = "openProcessPicker",
                Size = new System.Drawing.Size(560, 300),
                TabIndex = 0
            };
            this.openTabPage.Controls.Add(this.openProcessPicker);
            // inactiveProcessPicker
            this.inactiveProcessPicker = new KeeSetPriority.ProcessPicker(ActionTypesKSP.Inactive)
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new System.Drawing.Point(6, 87),
                Name = "inactiveProcessPicker",
                Size = new System.Drawing.Size(560, 300),
                TabIndex = 0

            };
            this.inactiveTabPage.Controls.Add(this.inactiveProcessPicker);
        }

        public bool cancelUpdateThread = false;

        // For when loading the dialog
        // TO DO: use configWindowDataClass to set the correct settings on load
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
            if (configWindowDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Save) == PriorityChangeTypes.NeverSet)
            {
                saveDefaultRadioButton.Checked = true;
                saveSetPriorityRadioButton.Checked = false;
                saveSetDependantPriorityRadioButton.Checked = false;
                saveSettingsComboBox.Enabled = false;
                saveProcessPicker.Enabled = false;
            }
            else if (configWindowDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Save) == PriorityChangeTypes.AlwaysSet)
            {
                saveDefaultRadioButton.Checked = false;
                saveSetPriorityRadioButton.Checked = true;
                saveSetDependantPriorityRadioButton.Checked = false;
                saveSettingsComboBox.Enabled = true;
                saveProcessPicker.Enabled = false;
            }
            else if (configWindowDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Save) == PriorityChangeTypes.SetWhenDependent)
            {
                saveDefaultRadioButton.Checked = false;
                saveSetPriorityRadioButton.Checked = false;
                saveSetDependantPriorityRadioButton.Checked = true;
                saveSettingsComboBox.Enabled = true;
                saveProcessPicker.Enabled = true;
            }
            SetPriorityDropdownsOnStartup(ActionTypesKSP.Save);
            // Set open settings
            if (configWindowDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Open) == PriorityChangeTypes.NeverSet)
            {
                openDefaultRadioButton.Checked = true;
                openSetPriorityRadioButton.Checked = false;
                openSetDependantPriorityRadioButton.Checked = false;
                openSettingsComboBox.Enabled = false;
                openProcessPicker.Enabled = false;
            }
            else if(configWindowDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Open) == PriorityChangeTypes.AlwaysSet)
            {
                openDefaultRadioButton.Checked = false;
                openSetPriorityRadioButton.Checked = true;
                openSetDependantPriorityRadioButton.Checked = false;
                openSettingsComboBox.Enabled = true;
                openProcessPicker.Enabled = false;
            }
            else if (configWindowDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Open) == PriorityChangeTypes.SetWhenDependent)
            {
                openDefaultRadioButton.Checked = false;
                openSetPriorityRadioButton.Checked = false;
                openSetDependantPriorityRadioButton.Checked = true;
                openSettingsComboBox.Enabled = true;
                openProcessPicker.Enabled = true;
            }
            SetPriorityDropdownsOnStartup(ActionTypesKSP.Open);
            // Set inactive settings
            if (configWindowDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.NeverSet)
            {
                inactiveDefaultPriorityRadioButton.Checked = true;
                inactiveSetPriorityRadioButton.Checked = false;
                inactiveSetDependantPriorityRadioButton.Checked = false;
                inactiveSettingsComboBox.Enabled = false;
                inactiveProcessPicker.Enabled = false;
            }
            else if (configWindowDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.AlwaysSet)
            {
                inactiveDefaultPriorityRadioButton.Checked = false;
                inactiveSetPriorityRadioButton.Checked = true;
                inactiveSetDependantPriorityRadioButton.Checked = false;
                inactiveSettingsComboBox.Enabled = true;
                inactiveProcessPicker.Enabled = false;
            }
            else if (configWindowDataClass.configDataStruct.GetPriorityMode(ActionTypesKSP.Inactive) == PriorityChangeTypes.SetWhenDependent)
            {
                inactiveDefaultPriorityRadioButton.Checked = false;
                inactiveSetPriorityRadioButton.Checked = false;
                inactiveSetDependantPriorityRadioButton.Checked = true;
                inactiveSettingsComboBox.Enabled = true;
                inactiveProcessPicker.Enabled = true;
            }
            SetPriorityDropdownsOnStartup(ActionTypesKSP.Inactive);
            currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)KeeSetPriorityData.currentProcess.PriorityClass);
            currentPriorityGroupBox.Visible = currentPriorityGroupBox.Enabled = false;
            updateCurrentPriorityTag.RunWorkerAsync();

            // Set advanced settings panel
            advancedGroupBox.Enabled = configWindowDataClass.configDataStruct.isAdvancedOptionsAvailable;
            dialogEnableAdvancedSettings.Checked = configWindowDataClass.configDataStruct.isAdvancedOptionsAvailable;

            // Set high and realtime overrides
            allowHighPriorityButton.Checked = allowRealtimePriorityButton.Enabled = (configWindowDataClass.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes || configWindowDataClass.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh);
            allowRealtimePriorityButton.Checked = configWindowDataClass.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes;

            // Set priority boost
            if (KeeSetPriorityData.isDefaultPriorityBoostSettable)
            {
                setPriorityBoostButton.Enabled = true;
                switch (configWindowDataClass.configDataStruct.priorityBoostState)
                {
                    case PriorityBoostTypesKSP.Default:
                        setPriorityBoostButton.Checked = setPriorityBoostDropdown.Enabled = false;
                        setPriorityBoostDropdown.SelectedIndex = -1;
                        break;
                    case PriorityBoostTypesKSP.Enabled:
                        setPriorityBoostButton.Checked = setPriorityBoostDropdown.Enabled = true;
                        setPriorityBoostDropdown.SelectedIndex = 0;
                        break;
                    case PriorityBoostTypesKSP.Disabled:
                        setPriorityBoostButton.Checked = setPriorityBoostDropdown.Enabled = true;
                        setPriorityBoostDropdown.SelectedIndex = 1;
                        break;
                }
                priorityBoostStatusLabel.Text = KeeSetPriorityData.currentProcess.PriorityBoostEnabled ? KeeSetPriorityTextStrings.EnabledStr : KeeSetPriorityTextStrings.DisabledStr;
            }
            else
            {
                priorityBoostStatusLabel.Text = KeeSetPriorityTextStrings.NotAvailableStr;
                setPriorityBoostButton.Enabled = setPriorityBoostDropdown.Enabled = false;
                setPriorityBoostButton.Checked = configWindowDataClass.configDataStruct.priorityBoostState != PriorityBoostTypesKSP.Default;
            }

            // Set program list settings
            allowSystemProcessesCheckBox.Enabled = allowNonWindowedProcessesCheckbox.Checked = configWindowDataClass.configDataStruct.allowNonWindowedProcesses != AllowBackgroundSystemProcesses.No;
            allowSystemProcessesCheckBox.Checked = configWindowDataClass.configDataStruct.allowNonWindowedProcesses == AllowBackgroundSystemProcesses.BackgroundAndSystem;

            // Set update thread settings
            updateFrecuencyNumericUpDown.Value = configWindowDataClass.configDataStruct.updateThreadTime / 1000;

            // Signal that initialization is done
            initializationDone = true;
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancelUpdateThread = true;
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
            if (configWindowDataClass.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes)
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
            else if (configWindowDataClass.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh)
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
            configWindowDataClass.configDataStruct.SetPriorityLevel(action, prio);
            if(action == ActionTypesKSP.Inactive && !inactiveSetDependantPriorityRadioButton.Checked)
            {
                KeeSetPriorityData.currentProcess.PriorityClass = (ProcessPriorityClass)prio;
                KeeSetPriorityData.currentProcess.Refresh();
                currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)KeeSetPriorityData.currentProcess.PriorityClass);
            }
        }

        // Called every time an index is changed
        private void OnPriorityDropdownChangeIndex(int index, ActionTypesKSP action)
        {
            if (configWindowDataClass.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes)
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
            else if (configWindowDataClass.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh)
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
            ProcessPriorityClassKSP prio = configWindowDataClass.configDataStruct.GetPriorityLevel(action);
            switch (action)
            {
                case ActionTypesKSP.Open:
                    comboBox = openSettingsComboBox;
                    break;
                case ActionTypesKSP.Save:
                    comboBox = saveSettingsComboBox;
                    break;
                case ActionTypesKSP.Inactive:
                    comboBox = inactiveSettingsComboBox;
                    break;
            }

            if(prio == ProcessPriorityClassKSP.NotSet)
            {
                comboBox.SelectedIndex = -1;
            }
            else if (configWindowDataClass.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes)
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
            else if (configWindowDataClass.configDataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh)
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
                configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Save, saveSetDependantPriorityRadioButton.Checked ? PriorityChangeTypes.SetWhenDependent : PriorityChangeTypes.AlwaysSet);
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
                configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Save, PriorityChangeTypes.NeverSet);
                configWindowDataClass.configDataStruct.SetPriorityLevel(ActionTypesKSP.Save, ProcessPriorityClassKSP.NotSet);
            }
        }

        private void SaveSetPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && saveSetPriorityRadioButton.Checked)
            {
                saveSettingsComboBox.Enabled = true;
                saveProcessPicker.Enabled = false;
                if(saveSettingsComboBox.SelectedIndex != -1)
                {
                    configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Save, PriorityChangeTypes.AlwaysSet);
                }
            }
        }
        private void SaveSetDependantPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && saveSetDependantPriorityRadioButton.Checked)
            {
                saveSettingsComboBox.Enabled = true;
                saveProcessPicker.Enabled = true;
                if (saveSettingsComboBox.SelectedIndex != -1)
                {
                    configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Save, PriorityChangeTypes.SetWhenDependent);
                }
            }
        }

        #endregion

        #region Open tab functions

        private void OpenSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Open, openSetDependantPriorityRadioButton.Checked ? PriorityChangeTypes.SetWhenDependent : PriorityChangeTypes.AlwaysSet);
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
                configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Open, PriorityChangeTypes.NeverSet);
                configWindowDataClass.configDataStruct.SetPriorityLevel(ActionTypesKSP.Open, ProcessPriorityClassKSP.NotSet);
            }
        }

        private void OpenSetPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && openSetPriorityRadioButton.Checked)
            {
                openSettingsComboBox.Enabled = true;
                openProcessPicker.Enabled = false;
                if (openSettingsComboBox.SelectedIndex != -1)
                {
                    configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Open, PriorityChangeTypes.AlwaysSet);
                }
            }
        }

        private void OpenSetDependantPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && openSetDependantPriorityRadioButton.Checked)
            {
                openSettingsComboBox.Enabled = true;
                openProcessPicker.Enabled = true;
                if (openSettingsComboBox.SelectedIndex != -1)
                {
                    configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Open, PriorityChangeTypes.SetWhenDependent);
                }
            }
        }

        #endregion

        #region Inactive tab functions
        private void InactiveSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, inactiveSetDependantPriorityRadioButton.Checked ? PriorityChangeTypes.SetWhenDependent : PriorityChangeTypes.AlwaysSet);
                OnPriorityDropdownChangeIndex(inactiveSettingsComboBox.SelectedIndex, ActionTypesKSP.Inactive);
                KeeSetPriorityData.SetInactivePriorityOnChange();
                KeeSetPriorityData.currentProcess.Refresh();
                currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)KeeSetPriorityData.currentProcess.PriorityClass);
            }
        }

        private void InactiveDefaultPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && inactiveDefaultPriorityRadioButton.Checked)
            {
                inactiveSettingsComboBox.SelectedIndex = -1;
                inactiveSettingsComboBox.Enabled = false;
                configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, PriorityChangeTypes.NeverSet);
                configWindowDataClass.configDataStruct.SetPriorityLevel(ActionTypesKSP.Inactive, ProcessPriorityClassKSP.NotSet);
                inactiveProcessPicker.Enabled = false;
                KeeSetPriorityData.currentProcess.PriorityClass = KeeSetPriorityData.defaultProcessPriority;
                KeeSetPriorityData.SetInactivePriorityOnChange();
            }
            UpdateCurrentPriorityDisplay();
        }

        private void InactiveSetPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && inactiveSetPriorityRadioButton.Checked)
            {
                inactiveSettingsComboBox.Enabled = true;
                inactiveProcessPicker.Enabled = false;
                if(inactiveSettingsComboBox.SelectedIndex != -1)
                {
                    configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, PriorityChangeTypes.AlwaysSet);
                    KeeSetPriorityData.SetInactivePriorityOnChange();
                }
                
            }
        }

        private void InactiveSetDependantPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && inactiveSetDependantPriorityRadioButton.Checked)
            {
                inactiveSettingsComboBox.Enabled = true;
                inactiveProcessPicker.Enabled = true;
                if (inactiveSettingsComboBox.SelectedIndex != -1)
                {
                    configWindowDataClass.configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, PriorityChangeTypes.SetWhenDependent);
                    KeeSetPriorityData.SetInactivePriorityOnChange();
                }
            }
        }

        internal void UpdateCurrentPriorityDisplay()
        {
            KeeSetPriorityData.currentProcess.Refresh();
            currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)KeeSetPriorityData.currentProcess.PriorityClass);
        }

        #endregion

        #region Advanced tab functions
        // Main button
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
                        configWindowDataClass.configDataStruct.isAdvancedOptionsAvailable = true;
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
                    allowNonWindowedProcessesCheckbox.Checked = false;
                    allowRealtimePriorityButton.Checked = false;
                    allowHighPriorityButton.Checked = false;
                    setPriorityBoostButton.Checked = false;
                    setPriorityBoostDropdown.SelectedIndex = -1;
                    configWindowDataClass.configDataStruct.isAdvancedOptionsAvailable = false;
                }
            }
        }

        // Priority boost
        private void SetPriorityBoostButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                setPriorityBoostDropdown.SelectedIndex = -1; // Unset or make sure it is in a default state
                configWindowDataClass.configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
                setPriorityBoostDropdown.Enabled = setPriorityBoostButton.Checked;
                KeeSetPriorityData.currentProcess.PriorityBoostEnabled = KeeSetPriorityData.defaultPriorityBoost == PriorityBoostTypesKSP.Enabled;
                KeeSetPriorityData.currentProcess.Refresh();
                priorityBoostStatusLabel.Text = KeeSetPriorityData.currentProcess.PriorityBoostEnabled ? KeeSetPriorityTextStrings.EnabledStr : KeeSetPriorityTextStrings.DisabledStr;
            }
        }
        private void SetPriorityBoostDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (setPriorityBoostDropdown.SelectedIndex == 0) // Enabled
            {
                configWindowDataClass.configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Enabled;
            }
            else if (setPriorityBoostDropdown.SelectedIndex == 1) //disabled
            {
                configWindowDataClass.configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Disabled;
            }
            KeeSetPriorityData.currentProcess.PriorityBoostEnabled = configWindowDataClass.configDataStruct.priorityBoostState == PriorityBoostTypesKSP.Enabled;
            KeeSetPriorityData.currentProcess.Refresh();
            priorityBoostStatusLabel.Text = KeeSetPriorityData.currentProcess.PriorityBoostEnabled ? KeeSetPriorityTextStrings.EnabledStr : KeeSetPriorityTextStrings.DisabledStr;
        }

        // High priorities
        private void AllowRealtimePriorityButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                if (allowRealtimePriorityButton.Checked)
                {
                    DialogResult warningDialogRealtime = MessageBox.Show(KeeSetPriorityTextStrings.RealtimePriorityAdvancedSettingsString, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if(warningDialogRealtime == DialogResult.Yes)
                    {
                        configWindowDataClass.configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.Yes;
                    }
                    else
                    {
                        allowRealtimePriorityButton.Checked = false;
                    }
                }
                else if(allowHighPriorityButton.Checked)
                {
                    configWindowDataClass.configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.OnlyHigh;
                }
                else
                {
                    configWindowDataClass.configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
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
                    configWindowDataClass.configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.OnlyHigh;
                    allowRealtimePriorityButton.Enabled = true;
                }
                else
                {
                    configWindowDataClass.configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
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

        // Other processes
        private void AllowNonWindowedProcessesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                allowSystemProcessesCheckBox.Enabled = allowNonWindowedProcessesCheckbox.Checked;
                allowSystemProcessesCheckBox.Checked = false;
                configWindowDataClass.configDataStruct.allowNonWindowedProcesses = allowNonWindowedProcessesCheckbox.Checked ? AllowBackgroundSystemProcesses.OnlyBackground : AllowBackgroundSystemProcesses.No;
                ProcessPicker.UpdateProcessList(); // One call updates all the tabs' boxes
            }
        }
        private void AllowSystemProcessesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                configWindowDataClass.configDataStruct.allowNonWindowedProcesses = allowSystemProcessesCheckBox.Checked ? AllowBackgroundSystemProcesses.BackgroundAndSystem : AllowBackgroundSystemProcesses.OnlyBackground;
                ProcessPicker.UpdateProcessList(); // One call updates all the tabs' boxes
            }
        }

        // Update interval
        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                configWindowDataClass.configDataStruct.updateThreadTime = (int)updateFrecuencyNumericUpDown.Value * 1000;
            }
        }

        #endregion

        private void MainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Changes whether you get the current priority box at the bottom, since it should only show on the inactiveTabPage
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

        private void UpdateCurrentPriorityTag_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Thread.Sleep(200);
            KeeSetPriorityData.currentProcess.Refresh();
        }

        private void UpdateCurrentPriorityTag_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            UpdateCurrentPriorityDisplay();
            if (!cancelUpdateThread)
            {
                updateCurrentPriorityTag.RunWorkerAsync();
            }
        }
    }
}
