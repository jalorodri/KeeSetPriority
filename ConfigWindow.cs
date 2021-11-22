using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeeSetPriority
{
    public partial class SettingsWindow : Form
    {
        // Set public variables
        public KeeSetPriorityData dataStruct;

        private string[] comboboxNames;

        private bool initializationDone = false;
        public SettingsWindow(KeeSetPriorityData initdata)
        {
            dataStruct = initdata;
            InitializeComponent();
        }

        // For when loading the dialog
        // TO DO: use dataStruct to set the correct settings on load
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
            if (dataStruct.changePriorityOnSave == ProcessPriorityClassKSP.Default)
            {
                saveDefaultRadioButton.Checked = true;
                saveSetPriorityRadioButton.Checked = false;
                saveSettingsComboBox.Enabled = false;
            }
            else
            {
                saveDefaultRadioButton.Checked = false;
                saveSetPriorityRadioButton.Checked = true;
                saveSettingsComboBox.Enabled = true;
            }
            SetPriorityDropdownsOnStartup(ActionTypesKSP.Save);
            // Set open settings
            if (dataStruct.changePriorityOnOpen == ProcessPriorityClassKSP.Default)
            {
                openDefaultRadioButton.Checked = true;
                openSetPriorityRadioButton.Checked = false;
                openSettingsComboBox.Enabled = false;
            }
            else
            {
                openDefaultRadioButton.Checked = false;
                openSetPriorityRadioButton.Checked = true;
                openSettingsComboBox.Enabled = true;
            }
            SetPriorityDropdownsOnStartup(ActionTypesKSP.Open);
            // Set inactive settings
            if (dataStruct.changePriorityOnInactive == ProcessPriorityClassKSP.Default)
            {
                inactiveDefaultPriorityRadioButton.Checked = true;
                inactiveSetPriorityRadioButton.Checked = false;
                inactiveSettingsComboBox.Enabled = false;
            }
            else
            {
                inactiveDefaultPriorityRadioButton.Checked = false;
                inactiveSetPriorityRadioButton.Checked = true;
                inactiveSettingsComboBox.Enabled = true;
            }
            SetPriorityDropdownsOnStartup(ActionTypesKSP.Inactive);
            currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)dataStruct.currentProcess.PriorityClass);

            // Set advanced settings panel
            advancedGroupBox.Enabled = dataStruct.isAdvancedOptionsAvailable;
            dialogEnableAdvancedSettings.Checked = dataStruct.isAdvancedOptionsAvailable;

            // Set high and realtime overrides
            allowHighPriorityButton.Checked = allowRealtimePriorityButton.Enabled = (dataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes || dataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh);
            allowRealtimePriorityButton.Checked = dataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes;

            // Set priority boost
            if (dataStruct.isDefaultPriorityBoostSettable)
            {
                setPriorityBoostButton.Enabled = setPriorityBoostDropdown.Enabled = true;
                setPriorityBoostButton.Checked = dataStruct.priorityBoostState != PriorityBoostTypesKSP.Default;
                setPriorityBoostDropdown.Enabled = (dataStruct.isDefaultPriorityBoostSettable && setPriorityBoostButton.Checked);
                priorityBoostStatusLabel.Text = dataStruct.currentProcess.PriorityBoostEnabled ? KeeSetPriorityTextStrings.EnabledStr : KeeSetPriorityTextStrings.DisabledStr;
            }
            else
            {
                priorityBoostStatusLabel.Text = KeeSetPriorityTextStrings.NotAvailableStr;
                setPriorityBoostButton.Enabled = setPriorityBoostDropdown.Enabled = false;
                setPriorityBoostButton.Checked = dataStruct.priorityBoostState != PriorityBoostTypesKSP.Default;
            }
            initializationDone = true;
        }

        

        // When check the advanced settings for the first time
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
                        dataStruct.isAdvancedOptionsAvailable = true;
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
                    dataStruct.isAdvancedOptionsAvailable = false;
                }
            }
        }

        // When the priority boost check is changed
        private void SetPriorityBoostButton_CheckedChanged(object sender, EventArgs e)
        {
            setPriorityBoostDropdown.SelectedIndex = -1; // Unset
            dataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
            setPriorityBoostDropdown.Enabled = setPriorityBoostButton.Checked;
        }

        // When the priority boost dropdown index is changed
        private void SetPriorityBoostDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(setPriorityBoostDropdown.SelectedIndex == 0) // Enabled
            {
                dataStruct.priorityBoostState = PriorityBoostTypesKSP.Enabled;
            }
            else if (setPriorityBoostDropdown.SelectedIndex == 1) //disabled
            {
                dataStruct.priorityBoostState = PriorityBoostTypesKSP.Disabled;
            }
            dataStruct.currentProcess.PriorityBoostEnabled = dataStruct.priorityBoostState == PriorityBoostTypesKSP.Enabled;
            dataStruct.currentProcess.Refresh();
            priorityBoostStatusLabel.Text = dataStruct.currentProcess.PriorityBoostEnabled ? KeeSetPriorityTextStrings.EnabledStr : KeeSetPriorityTextStrings.DisabledStr;
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


        // Value is written to comboboxNames
        private void GetPriorityDropdowns()
        {
            if (dataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes)
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
            else if (dataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh)
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
                    dataStruct.changePriorityOnOpen = prio;
                    break;
                case ActionTypesKSP.Save:
                    dataStruct.changePriorityOnSave = prio;
                    break;
                case ActionTypesKSP.Inactive:
                    dataStruct.changePriorityOnInactive = prio;
                    dataStruct.currentProcess.PriorityClass = (ProcessPriorityClass)prio;
                    dataStruct.currentProcess.Refresh();
                    currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)dataStruct.currentProcess.PriorityClass);
                    break;
            }
        }

        // Called every time an index is changed
        private void OnPriorityDropdownChangeIndex(int index, ActionTypesKSP action)
        {
            if (dataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes)
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
            else if (dataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh)
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
                    prio = dataStruct.changePriorityOnOpen;
                    break;
                case ActionTypesKSP.Save:
                    comboBox = saveSettingsComboBox;
                    prio = dataStruct.changePriorityOnSave;
                    break;
                case ActionTypesKSP.Inactive:
                    comboBox = inactiveSettingsComboBox;
                    prio = dataStruct.changePriorityOnInactive;
                    break;
            }
            if(prio == ProcessPriorityClassKSP.Default)
            {
                comboBox.SelectedIndex = -1;
            }
            else if (dataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.Yes)
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
            else if (dataStruct.allowDangerousPrioritites == AllowDangerousPrioritites.OnlyHigh)
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

        private void SaveSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                OnPriorityDropdownChangeIndex(saveSettingsComboBox.SelectedIndex, ActionTypesKSP.Save);
            }
        }

        private void OpenSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                OnPriorityDropdownChangeIndex(openSettingsComboBox.SelectedIndex, ActionTypesKSP.Open);
            }
        }
        private void InactiveSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initializationDone)
            {
                OnPriorityDropdownChangeIndex(inactiveSettingsComboBox.SelectedIndex, ActionTypesKSP.Inactive);
                dataStruct.currentProcess.Refresh();
                currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)dataStruct.currentProcess.PriorityClass);
            }
        }

        private void SaveDefaultRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && saveDefaultRadioButton.Checked)
            {
                saveSettingsComboBox.SelectedIndex = -1;
                saveSettingsComboBox.Enabled = false;
                dataStruct.changePriorityOnSave = ProcessPriorityClassKSP.Default;
            }
        }

        private void OpenDefaultRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && openDefaultRadioButton.Checked)
            {
                openSettingsComboBox.SelectedIndex = -1;
                openSettingsComboBox.Enabled = false;
                dataStruct.changePriorityOnOpen = ProcessPriorityClassKSP.Default;
            }
        }

        private void InactiveDefaultPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && inactiveDefaultPriorityRadioButton.Checked)
            {
                inactiveSettingsComboBox.SelectedIndex = -1;
                inactiveSettingsComboBox.Enabled = false;
                dataStruct.changePriorityOnInactive = ProcessPriorityClassKSP.Default;
                dataStruct.currentProcess.PriorityClass = dataStruct.defaultProcessPriority;
            }
            dataStruct.currentProcess.Refresh();
            currentPriorityLabel.Text = KeeSetPriorityTextStrings.GetPriorityLevelStr((ProcessPriorityClassKSP)dataStruct.currentProcess.PriorityClass);
        }

        private void SaveSetPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && saveSetPriorityRadioButton.Checked)
            {
                saveSettingsComboBox.Enabled = true;
            }
        }

        private void OpenSetPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && openSetPriorityRadioButton.Checked)
            {
                openSettingsComboBox.Enabled = true;
            }
        }

        private void InactiveSetPriorityRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (initializationDone && inactiveSetPriorityRadioButton.Checked)
            {
                inactiveSettingsComboBox.Enabled = true;
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
                        dataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.Yes;
                    }
                    else
                    {
                        allowRealtimePriorityButton.Checked = false;
                    }
                }
                else if(allowHighPriorityButton.Checked)
                {
                    dataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.OnlyHigh;
                }
                else
                {
                    dataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
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
                    dataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.OnlyHigh;
                    allowRealtimePriorityButton.Enabled = true;
                }
                else
                {
                    dataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
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
    }
}
