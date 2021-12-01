
namespace KeeSetPriority
{
    partial class SettingsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dialogButtonSave = new System.Windows.Forms.Button();
            this.dialogButtonCancel = new System.Windows.Forms.Button();
            this.saveSettingsComboBox = new System.Windows.Forms.ComboBox();
            this.saveSetPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.saveDefaultRadioButton = new System.Windows.Forms.RadioButton();
            this.currentPriorityLabel = new System.Windows.Forms.Label();
            this.inactiveSettingsComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.inactiveDefaultPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.inactiveSetPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.openSettingsComboBox = new System.Windows.Forms.ComboBox();
            this.openDefaultRadioButton = new System.Windows.Forms.RadioButton();
            this.openSetPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dialogEnableAdvancedSettings = new System.Windows.Forms.CheckBox();
            this.advancedGroupBox = new System.Windows.Forms.GroupBox();
            this.setPriorityBoostDropdown = new System.Windows.Forms.ComboBox();
            this.priorityBoostStatusLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.setPriorityBoostButton = new System.Windows.Forms.CheckBox();
            this.allowHighPriorityButton = new System.Windows.Forms.CheckBox();
            this.allowRealtimePriorityButton = new System.Windows.Forms.CheckBox();
            this.settingsTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.aboutButton = new System.Windows.Forms.Button();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.saveTabPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.saveSetDependantPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.openTabPage = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.openSetDependantPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.inactiveTabPage = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.inactiveSetDependantPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.advancedTabPage = new System.Windows.Forms.TabPage();
            this.currentPriorityGroupBox = new System.Windows.Forms.GroupBox();
            this.advancedGroupBox.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.saveTabPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.openTabPage.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.inactiveTabPage.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.advancedTabPage.SuspendLayout();
            this.currentPriorityGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dialogButtonSave
            // 
            this.dialogButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButtonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.dialogButtonSave.Location = new System.Drawing.Point(404, 448);
            this.dialogButtonSave.Name = "dialogButtonSave";
            this.dialogButtonSave.Size = new System.Drawing.Size(91, 31);
            this.dialogButtonSave.TabIndex = 0;
            this.dialogButtonSave.Text = "Save";
            this.dialogButtonSave.UseVisualStyleBackColor = true;
            // 
            // dialogButtonCancel
            // 
            this.dialogButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dialogButtonCancel.Location = new System.Drawing.Point(501, 448);
            this.dialogButtonCancel.Name = "dialogButtonCancel";
            this.dialogButtonCancel.Size = new System.Drawing.Size(91, 31);
            this.dialogButtonCancel.TabIndex = 1;
            this.dialogButtonCancel.Text = "Cancel";
            this.dialogButtonCancel.UseVisualStyleBackColor = true;
            // 
            // saveSettingsComboBox
            // 
            this.saveSettingsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.saveSettingsComboBox.FormattingEnabled = true;
            this.saveSettingsComboBox.Items.AddRange(new object[] {
            "Realtime",
            "High",
            "Above normal",
            "Normal",
            "Below normal",
            "Idle"});
            this.saveSettingsComboBox.Location = new System.Drawing.Point(6, 24);
            this.saveSettingsComboBox.Name = "saveSettingsComboBox";
            this.saveSettingsComboBox.Size = new System.Drawing.Size(129, 24);
            this.saveSettingsComboBox.TabIndex = 6;
            this.saveSettingsComboBox.SelectedIndexChanged += new System.EventHandler(this.SaveSettingsComboBox_SelectedIndexChanged);
            // 
            // saveSetPriorityRadioButton
            // 
            this.saveSetPriorityRadioButton.AutoSize = true;
            this.saveSetPriorityRadioButton.Location = new System.Drawing.Point(6, 33);
            this.saveSetPriorityRadioButton.Name = "saveSetPriorityRadioButton";
            this.saveSetPriorityRadioButton.Size = new System.Drawing.Size(241, 21);
            this.saveSetPriorityRadioButton.TabIndex = 1;
            this.saveSetPriorityRadioButton.Text = "Always set priority values on save";
            this.saveSetPriorityRadioButton.UseVisualStyleBackColor = true;
            this.saveSetPriorityRadioButton.CheckedChanged += new System.EventHandler(this.SaveSetPriorityRadioButton_CheckedChanged);
            // 
            // saveDefaultRadioButton
            // 
            this.saveDefaultRadioButton.AutoSize = true;
            this.saveDefaultRadioButton.Checked = true;
            this.saveDefaultRadioButton.Location = new System.Drawing.Point(6, 6);
            this.saveDefaultRadioButton.Name = "saveDefaultRadioButton";
            this.saveDefaultRadioButton.Size = new System.Drawing.Size(268, 21);
            this.saveDefaultRadioButton.TabIndex = 0;
            this.saveDefaultRadioButton.TabStop = true;
            this.saveDefaultRadioButton.Text = "Do not change priority values on save";
            this.saveDefaultRadioButton.UseVisualStyleBackColor = true;
            this.saveDefaultRadioButton.CheckedChanged += new System.EventHandler(this.SaveDefaultRadioButton_CheckedChanged);
            // 
            // currentPriorityLabel
            // 
            this.currentPriorityLabel.AutoSize = true;
            this.currentPriorityLabel.Location = new System.Drawing.Point(6, 18);
            this.currentPriorityLabel.Name = "currentPriorityLabel";
            this.currentPriorityLabel.Size = new System.Drawing.Size(56, 17);
            this.currentPriorityLabel.TabIndex = 7;
            this.currentPriorityLabel.Text = "(status)";
            // 
            // inactiveSettingsComboBox
            // 
            this.inactiveSettingsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inactiveSettingsComboBox.FormattingEnabled = true;
            this.inactiveSettingsComboBox.Items.AddRange(new object[] {
            "Realtime",
            "High",
            "Above normal",
            "Normal",
            "Below normal",
            "Idle"});
            this.inactiveSettingsComboBox.Location = new System.Drawing.Point(6, 24);
            this.inactiveSettingsComboBox.Name = "inactiveSettingsComboBox";
            this.inactiveSettingsComboBox.Size = new System.Drawing.Size(129, 24);
            this.inactiveSettingsComboBox.TabIndex = 9;
            this.inactiveSettingsComboBox.SelectedIndexChanged += new System.EventHandler(this.InactiveSettingsComboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(402, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 17);
            this.label6.TabIndex = 6;
            // 
            // inactiveDefaultPriorityRadioButton
            // 
            this.inactiveDefaultPriorityRadioButton.AutoSize = true;
            this.inactiveDefaultPriorityRadioButton.Checked = true;
            this.inactiveDefaultPriorityRadioButton.Location = new System.Drawing.Point(6, 6);
            this.inactiveDefaultPriorityRadioButton.Name = "inactiveDefaultPriorityRadioButton";
            this.inactiveDefaultPriorityRadioButton.Size = new System.Drawing.Size(301, 21);
            this.inactiveDefaultPriorityRadioButton.TabIndex = 7;
            this.inactiveDefaultPriorityRadioButton.TabStop = true;
            this.inactiveDefaultPriorityRadioButton.Text = "Do not change priority values while inactive";
            this.inactiveDefaultPriorityRadioButton.UseVisualStyleBackColor = true;
            this.inactiveDefaultPriorityRadioButton.CheckedChanged += new System.EventHandler(this.InactiveDefaultPriorityRadioButton_CheckedChanged);
            // 
            // inactiveSetPriorityRadioButton
            // 
            this.inactiveSetPriorityRadioButton.AutoSize = true;
            this.inactiveSetPriorityRadioButton.Location = new System.Drawing.Point(6, 33);
            this.inactiveSetPriorityRadioButton.Name = "inactiveSetPriorityRadioButton";
            this.inactiveSetPriorityRadioButton.Size = new System.Drawing.Size(271, 21);
            this.inactiveSetPriorityRadioButton.TabIndex = 8;
            this.inactiveSetPriorityRadioButton.Text = "Always set priority value while inactive:";
            this.inactiveSetPriorityRadioButton.UseVisualStyleBackColor = true;
            this.inactiveSetPriorityRadioButton.CheckedChanged += new System.EventHandler(this.InactiveSetPriorityRadioButton_CheckedChanged);
            // 
            // openSettingsComboBox
            // 
            this.openSettingsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.openSettingsComboBox.FormattingEnabled = true;
            this.openSettingsComboBox.Items.AddRange(new object[] {
            "Realtime",
            "High",
            "Above normal",
            "Normal",
            "Below normal",
            "Idle"});
            this.openSettingsComboBox.Location = new System.Drawing.Point(6, 24);
            this.openSettingsComboBox.Name = "openSettingsComboBox";
            this.openSettingsComboBox.Size = new System.Drawing.Size(129, 24);
            this.openSettingsComboBox.TabIndex = 9;
            this.openSettingsComboBox.SelectedIndexChanged += new System.EventHandler(this.OpenSettingsComboBox_SelectedIndexChanged);
            // 
            // openDefaultRadioButton
            // 
            this.openDefaultRadioButton.AutoSize = true;
            this.openDefaultRadioButton.Checked = true;
            this.openDefaultRadioButton.Location = new System.Drawing.Point(6, 6);
            this.openDefaultRadioButton.Name = "openDefaultRadioButton";
            this.openDefaultRadioButton.Size = new System.Drawing.Size(270, 21);
            this.openDefaultRadioButton.TabIndex = 7;
            this.openDefaultRadioButton.TabStop = true;
            this.openDefaultRadioButton.Text = "Do not change priority values on open";
            this.openDefaultRadioButton.UseVisualStyleBackColor = true;
            this.openDefaultRadioButton.CheckedChanged += new System.EventHandler(this.OpenDefaultRadioButton_CheckedChanged);
            // 
            // openSetPriorityRadioButton
            // 
            this.openSetPriorityRadioButton.AutoSize = true;
            this.openSetPriorityRadioButton.Location = new System.Drawing.Point(6, 33);
            this.openSetPriorityRadioButton.Name = "openSetPriorityRadioButton";
            this.openSetPriorityRadioButton.Size = new System.Drawing.Size(243, 21);
            this.openSetPriorityRadioButton.TabIndex = 8;
            this.openSetPriorityRadioButton.Text = "Always set priority values on open";
            this.openSetPriorityRadioButton.UseVisualStyleBackColor = true;
            this.openSetPriorityRadioButton.CheckedChanged += new System.EventHandler(this.OpenSetPriorityRadioButton_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(400, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 5;
            // 
            // dialogEnableAdvancedSettings
            // 
            this.dialogEnableAdvancedSettings.AutoSize = true;
            this.dialogEnableAdvancedSettings.Location = new System.Drawing.Point(6, 6);
            this.dialogEnableAdvancedSettings.Name = "dialogEnableAdvancedSettings";
            this.dialogEnableAdvancedSettings.Size = new System.Drawing.Size(193, 21);
            this.dialogEnableAdvancedSettings.TabIndex = 7;
            this.dialogEnableAdvancedSettings.Text = "Enable advanced settings";
            this.dialogEnableAdvancedSettings.UseVisualStyleBackColor = true;
            this.dialogEnableAdvancedSettings.CheckedChanged += new System.EventHandler(this.DialogEnableAdvancedSettings_CheckedChanged);
            // 
            // advancedGroupBox
            // 
            this.advancedGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedGroupBox.Controls.Add(this.setPriorityBoostDropdown);
            this.advancedGroupBox.Controls.Add(this.priorityBoostStatusLabel);
            this.advancedGroupBox.Controls.Add(this.label3);
            this.advancedGroupBox.Controls.Add(this.setPriorityBoostButton);
            this.advancedGroupBox.Controls.Add(this.allowHighPriorityButton);
            this.advancedGroupBox.Controls.Add(this.allowRealtimePriorityButton);
            this.advancedGroupBox.Enabled = false;
            this.advancedGroupBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.advancedGroupBox.Location = new System.Drawing.Point(6, 24);
            this.advancedGroupBox.Name = "advancedGroupBox";
            this.advancedGroupBox.Size = new System.Drawing.Size(560, 362);
            this.advancedGroupBox.TabIndex = 8;
            this.advancedGroupBox.TabStop = false;
            // 
            // setPriorityBoostDropdown
            // 
            this.setPriorityBoostDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.setPriorityBoostDropdown.FormattingEnabled = true;
            this.setPriorityBoostDropdown.Items.AddRange(new object[] {
            "Enable",
            "Disable"});
            this.setPriorityBoostDropdown.Location = new System.Drawing.Point(255, 66);
            this.setPriorityBoostDropdown.Name = "setPriorityBoostDropdown";
            this.setPriorityBoostDropdown.Size = new System.Drawing.Size(121, 24);
            this.setPriorityBoostDropdown.TabIndex = 12;
            this.setPriorityBoostDropdown.SelectedIndexChanged += new System.EventHandler(this.SetPriorityBoostDropdown_SelectedIndexChanged);
            // 
            // priorityBoostStatusLabel
            // 
            this.priorityBoostStatusLabel.AutoSize = true;
            this.priorityBoostStatusLabel.Location = new System.Drawing.Point(218, 93);
            this.priorityBoostStatusLabel.Name = "priorityBoostStatusLabel";
            this.priorityBoostStatusLabel.Size = new System.Drawing.Size(56, 17);
            this.priorityBoostStatusLabel.TabIndex = 4;
            this.priorityBoostStatusLabel.Text = "(status)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(187, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Current priority boost status:";
            // 
            // setPriorityBoostButton
            // 
            this.setPriorityBoostButton.AutoSize = true;
            this.setPriorityBoostButton.Location = new System.Drawing.Point(6, 68);
            this.setPriorityBoostButton.Name = "setPriorityBoostButton";
            this.setPriorityBoostButton.Size = new System.Drawing.Size(252, 21);
            this.setPriorityBoostButton.TabIndex = 11;
            this.setPriorityBoostButton.Text = "Set priority boost on window focus: ";
            this.setPriorityBoostButton.UseVisualStyleBackColor = true;
            this.setPriorityBoostButton.CheckedChanged += new System.EventHandler(this.SetPriorityBoostButton_CheckedChanged);
            // 
            // allowHighPriorityButton
            // 
            this.allowHighPriorityButton.AutoSize = true;
            this.allowHighPriorityButton.Location = new System.Drawing.Point(6, 11);
            this.allowHighPriorityButton.Name = "allowHighPriorityButton";
            this.allowHighPriorityButton.Size = new System.Drawing.Size(142, 21);
            this.allowHighPriorityButton.TabIndex = 9;
            this.allowHighPriorityButton.Text = "Allow High priority";
            this.allowHighPriorityButton.UseVisualStyleBackColor = true;
            this.allowHighPriorityButton.CheckedChanged += new System.EventHandler(this.AllowHighPriorityButton_CheckedChanged);
            // 
            // allowRealtimePriorityButton
            // 
            this.allowRealtimePriorityButton.AutoSize = true;
            this.allowRealtimePriorityButton.Location = new System.Drawing.Point(30, 37);
            this.allowRealtimePriorityButton.Name = "allowRealtimePriorityButton";
            this.allowRealtimePriorityButton.Size = new System.Drawing.Size(271, 21);
            this.allowRealtimePriorityButton.TabIndex = 10;
            this.allowRealtimePriorityButton.Text = "Allow Realtime priority (DANGEROUS)";
            this.allowRealtimePriorityButton.UseVisualStyleBackColor = true;
            this.allowRealtimePriorityButton.CheckedChanged += new System.EventHandler(this.AllowRealtimePriorityButton_CheckedChanged);
            // 
            // settingsTooltip
            // 
            this.settingsTooltip.Popup += new System.Windows.Forms.PopupEventHandler(this.SettingsTooltip_Popup);
            // 
            // aboutButton
            // 
            this.aboutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.aboutButton.Location = new System.Drawing.Point(12, 448);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(166, 31);
            this.aboutButton.TabIndex = 10;
            this.aboutButton.Text = "About KeeSetPriority";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTabControl.Controls.Add(this.saveTabPage);
            this.mainTabControl.Controls.Add(this.openTabPage);
            this.mainTabControl.Controls.Add(this.inactiveTabPage);
            this.mainTabControl.Controls.Add(this.advancedTabPage);
            this.mainTabControl.Location = new System.Drawing.Point(12, 12);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(580, 421);
            this.mainTabControl.TabIndex = 11;
            this.mainTabControl.SelectedIndexChanged += new System.EventHandler(this.MainTabControl_SelectedIndexChanged);
            // 
            // saveTabPage
            // 
            this.saveTabPage.Controls.Add(this.groupBox2);
            this.saveTabPage.Controls.Add(this.saveSetDependantPriorityRadioButton);
            this.saveTabPage.Controls.Add(this.saveSetPriorityRadioButton);
            this.saveTabPage.Controls.Add(this.saveDefaultRadioButton);
            this.saveTabPage.Location = new System.Drawing.Point(4, 25);
            this.saveTabPage.Name = "saveTabPage";
            this.saveTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.saveTabPage.Size = new System.Drawing.Size(572, 392);
            this.saveTabPage.TabIndex = 0;
            this.saveTabPage.Text = "On Save";
            this.saveTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.saveSettingsComboBox);
            this.groupBox2.Location = new System.Drawing.Point(425, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(141, 55);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Priority to set";
            // 
            // saveSetDependantPriorityRadioButton
            // 
            this.saveSetDependantPriorityRadioButton.AutoSize = true;
            this.saveSetDependantPriorityRadioButton.Location = new System.Drawing.Point(6, 60);
            this.saveSetDependantPriorityRadioButton.Name = "saveSetDependantPriorityRadioButton";
            this.saveSetDependantPriorityRadioButton.Size = new System.Drawing.Size(380, 21);
            this.saveSetDependantPriorityRadioButton.TabIndex = 13;
            this.saveSetDependantPriorityRadioButton.TabStop = true;
            this.saveSetDependantPriorityRadioButton.Text = "Set priority value on save if these programs are running";
            this.saveSetDependantPriorityRadioButton.UseVisualStyleBackColor = true;
            this.saveSetDependantPriorityRadioButton.CheckedChanged += new System.EventHandler(this.SaveSetDependantPriorityRadioButton_CheckedChanged);
            // 
            // openTabPage
            // 
            this.openTabPage.Controls.Add(this.groupBox3);
            this.openTabPage.Controls.Add(this.openSetDependantPriorityRadioButton);
            this.openTabPage.Controls.Add(this.openSetPriorityRadioButton);
            this.openTabPage.Controls.Add(this.openDefaultRadioButton);
            this.openTabPage.Location = new System.Drawing.Point(4, 25);
            this.openTabPage.Name = "openTabPage";
            this.openTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.openTabPage.Size = new System.Drawing.Size(572, 392);
            this.openTabPage.TabIndex = 1;
            this.openTabPage.Text = "On Open";
            this.openTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.openSettingsComboBox);
            this.groupBox3.Location = new System.Drawing.Point(425, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(141, 55);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Priority to set";
            // 
            // openSetDependantPriorityRadioButton
            // 
            this.openSetDependantPriorityRadioButton.AutoSize = true;
            this.openSetDependantPriorityRadioButton.Location = new System.Drawing.Point(6, 60);
            this.openSetDependantPriorityRadioButton.Name = "openSetDependantPriorityRadioButton";
            this.openSetDependantPriorityRadioButton.Size = new System.Drawing.Size(382, 21);
            this.openSetDependantPriorityRadioButton.TabIndex = 13;
            this.openSetDependantPriorityRadioButton.TabStop = true;
            this.openSetDependantPriorityRadioButton.Text = "Set priority value on open if these programs are running";
            this.openSetDependantPriorityRadioButton.UseVisualStyleBackColor = true;
            this.openSetDependantPriorityRadioButton.CheckedChanged += new System.EventHandler(this.OpenSetDependantPriorityRadioButton_CheckedChanged);
            // 
            // inactiveTabPage
            // 
            this.inactiveTabPage.Controls.Add(this.groupBox4);
            this.inactiveTabPage.Controls.Add(this.inactiveSetDependantPriorityRadioButton);
            this.inactiveTabPage.Controls.Add(this.label6);
            this.inactiveTabPage.Controls.Add(this.inactiveSetPriorityRadioButton);
            this.inactiveTabPage.Controls.Add(this.inactiveDefaultPriorityRadioButton);
            this.inactiveTabPage.Location = new System.Drawing.Point(4, 25);
            this.inactiveTabPage.Name = "inactiveTabPage";
            this.inactiveTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.inactiveTabPage.Size = new System.Drawing.Size(572, 392);
            this.inactiveTabPage.TabIndex = 2;
            this.inactiveTabPage.Text = "On Inactive";
            this.inactiveTabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.inactiveSettingsComboBox);
            this.groupBox4.Location = new System.Drawing.Point(425, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(141, 55);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Priority to set";
            // 
            // inactiveSetDependantPriorityRadioButton
            // 
            this.inactiveSetDependantPriorityRadioButton.AutoSize = true;
            this.inactiveSetDependantPriorityRadioButton.Location = new System.Drawing.Point(6, 60);
            this.inactiveSetDependantPriorityRadioButton.Name = "inactiveSetDependantPriorityRadioButton";
            this.inactiveSetDependantPriorityRadioButton.Size = new System.Drawing.Size(413, 21);
            this.inactiveSetDependantPriorityRadioButton.TabIndex = 11;
            this.inactiveSetDependantPriorityRadioButton.TabStop = true;
            this.inactiveSetDependantPriorityRadioButton.Text = "Set priority value while inactive if these programs are running";
            this.inactiveSetDependantPriorityRadioButton.UseVisualStyleBackColor = true;
            this.inactiveSetDependantPriorityRadioButton.CheckedChanged += new System.EventHandler(this.InactiveSetDependantPriorityRadioButton_CheckedChanged);
            // 
            // advancedTabPage
            // 
            this.advancedTabPage.Controls.Add(this.advancedGroupBox);
            this.advancedTabPage.Controls.Add(this.dialogEnableAdvancedSettings);
            this.advancedTabPage.Location = new System.Drawing.Point(4, 25);
            this.advancedTabPage.Name = "advancedTabPage";
            this.advancedTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.advancedTabPage.Size = new System.Drawing.Size(572, 392);
            this.advancedTabPage.TabIndex = 3;
            this.advancedTabPage.Text = "Advanced";
            this.advancedTabPage.UseVisualStyleBackColor = true;
            // 
            // currentPriorityGroupBox
            // 
            this.currentPriorityGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.currentPriorityGroupBox.Controls.Add(this.currentPriorityLabel);
            this.currentPriorityGroupBox.Location = new System.Drawing.Point(200, 439);
            this.currentPriorityGroupBox.Name = "currentPriorityGroupBox";
            this.currentPriorityGroupBox.Size = new System.Drawing.Size(123, 45);
            this.currentPriorityGroupBox.TabIndex = 10;
            this.currentPriorityGroupBox.TabStop = false;
            this.currentPriorityGroupBox.Text = "Current priority:";
            // 
            // SettingsWindow
            // 
            this.AcceptButton = this.dialogButtonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.dialogButtonCancel;
            this.ClientSize = new System.Drawing.Size(604, 491);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.currentPriorityGroupBox);
            this.Controls.Add(this.dialogButtonCancel);
            this.Controls.Add(this.dialogButtonSave);
            this.Controls.Add(this.mainTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(590, 300);
            this.Name = "SettingsWindow";
            this.Text = "KeeSetPriority Settings";
            this.Load += new System.EventHandler(this.SettingsWindow_Load);
            this.advancedGroupBox.ResumeLayout(false);
            this.advancedGroupBox.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.saveTabPage.ResumeLayout(false);
            this.saveTabPage.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.openTabPage.ResumeLayout(false);
            this.openTabPage.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.inactiveTabPage.ResumeLayout(false);
            this.inactiveTabPage.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.advancedTabPage.ResumeLayout(false);
            this.advancedTabPage.PerformLayout();
            this.currentPriorityGroupBox.ResumeLayout(false);
            this.currentPriorityGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button dialogButtonSave;
        private System.Windows.Forms.Button dialogButtonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox dialogEnableAdvancedSettings;
        private System.Windows.Forms.GroupBox advancedGroupBox;
        private System.Windows.Forms.CheckBox setPriorityBoostButton;
        private System.Windows.Forms.CheckBox allowHighPriorityButton;
        private System.Windows.Forms.CheckBox allowRealtimePriorityButton;
        private System.Windows.Forms.Label priorityBoostStatusLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox setPriorityBoostDropdown;
        private System.Windows.Forms.ToolTip settingsTooltip;
        private System.Windows.Forms.ComboBox saveSettingsComboBox;
        private System.Windows.Forms.RadioButton saveSetPriorityRadioButton;
        private System.Windows.Forms.RadioButton saveDefaultRadioButton;
        private System.Windows.Forms.ComboBox openSettingsComboBox;
        private System.Windows.Forms.RadioButton openDefaultRadioButton;
        private System.Windows.Forms.RadioButton openSetPriorityRadioButton;
        private System.Windows.Forms.ComboBox inactiveSettingsComboBox;
        private System.Windows.Forms.RadioButton inactiveDefaultPriorityRadioButton;
        private System.Windows.Forms.RadioButton inactiveSetPriorityRadioButton;
        private System.Windows.Forms.Label currentPriorityLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button aboutButton;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage saveTabPage;
        private System.Windows.Forms.TabPage openTabPage;
        private System.Windows.Forms.TabPage inactiveTabPage;
        private System.Windows.Forms.TabPage advancedTabPage;
        private System.Windows.Forms.GroupBox currentPriorityGroupBox;
        private System.Windows.Forms.RadioButton inactiveSetDependantPriorityRadioButton;
        private System.Windows.Forms.RadioButton saveSetDependantPriorityRadioButton;
        private System.Windows.Forms.RadioButton openSetDependantPriorityRadioButton;
        private ProcessPicker saveProcessPicker;
        private ProcessPicker openProcessPicker;
        private ProcessPicker inactiveProcessPicker;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}