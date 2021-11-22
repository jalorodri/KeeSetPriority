
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveSettingsComboBox = new System.Windows.Forms.ComboBox();
            this.saveSetPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.saveDefaultRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.currentPriorityLabel = new System.Windows.Forms.Label();
            this.inactiveSettingsComboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.inactiveDefaultPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.inactiveSetPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.openSettingsComboBox = new System.Windows.Forms.ComboBox();
            this.openDefaultRadioButton = new System.Windows.Forms.RadioButton();
            this.openSetPriorityRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dialogEnableAdvancedSettings = new System.Windows.Forms.CheckBox();
            this.advancedGroupBox = new System.Windows.Forms.GroupBox();
            this.setPriorityBoostDropdown = new System.Windows.Forms.ComboBox();
            this.priorityBoostStatusLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.setPriorityBoostButton = new System.Windows.Forms.CheckBox();
            this.allowHighPriorityButton = new System.Windows.Forms.CheckBox();
            this.allowRealtimePriorityButton = new System.Windows.Forms.CheckBox();
            this.settingsTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.advancedSettingsPanel = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.advancedGroupBox.SuspendLayout();
            this.advancedSettingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dialogButtonSave
            // 
            this.dialogButtonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.dialogButtonSave.Location = new System.Drawing.Point(222, 235);
            this.dialogButtonSave.Name = "dialogButtonSave";
            this.dialogButtonSave.Size = new System.Drawing.Size(75, 31);
            this.dialogButtonSave.TabIndex = 0;
            this.dialogButtonSave.Text = "Save";
            this.dialogButtonSave.UseVisualStyleBackColor = true;
            // 
            // dialogButtonCancel
            // 
            this.dialogButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dialogButtonCancel.Location = new System.Drawing.Point(303, 235);
            this.dialogButtonCancel.Name = "dialogButtonCancel";
            this.dialogButtonCancel.Size = new System.Drawing.Size(75, 31);
            this.dialogButtonCancel.TabIndex = 1;
            this.dialogButtonCancel.Text = "Cancel";
            this.dialogButtonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.saveSettingsComboBox);
            this.groupBox1.Controls.Add(this.saveSetPriorityRadioButton);
            this.groupBox1.Controls.Add(this.saveDefaultRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 89);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Save settings";
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
            this.saveSettingsComboBox.Location = new System.Drawing.Point(252, 49);
            this.saveSettingsComboBox.Name = "saveSettingsComboBox";
            this.saveSettingsComboBox.Size = new System.Drawing.Size(121, 24);
            this.saveSettingsComboBox.TabIndex = 6;
            this.saveSettingsComboBox.SelectedIndexChanged += new System.EventHandler(this.SaveSettingsComboBox_SelectedIndexChanged);
            // 
            // saveSetPriorityRadioButton
            // 
            this.saveSetPriorityRadioButton.AutoSize = true;
            this.saveSetPriorityRadioButton.Location = new System.Drawing.Point(7, 50);
            this.saveSetPriorityRadioButton.Name = "saveSetPriorityRadioButton";
            this.saveSetPriorityRadioButton.Size = new System.Drawing.Size(245, 21);
            this.saveSetPriorityRadioButton.TabIndex = 1;
            this.saveSetPriorityRadioButton.TabStop = true;
            this.saveSetPriorityRadioButton.Text = "Always set priority values on save:";
            this.saveSetPriorityRadioButton.UseVisualStyleBackColor = true;
            this.saveSetPriorityRadioButton.CheckedChanged += new System.EventHandler(this.SaveSetPriorityRadioButton_CheckedChanged);
            // 
            // saveDefaultRadioButton
            // 
            this.saveDefaultRadioButton.AutoSize = true;
            this.saveDefaultRadioButton.Location = new System.Drawing.Point(7, 22);
            this.saveDefaultRadioButton.Name = "saveDefaultRadioButton";
            this.saveDefaultRadioButton.Size = new System.Drawing.Size(268, 21);
            this.saveDefaultRadioButton.TabIndex = 0;
            this.saveDefaultRadioButton.TabStop = true;
            this.saveDefaultRadioButton.Text = "Do not change priority values on save";
            this.saveDefaultRadioButton.UseVisualStyleBackColor = true;
            this.saveDefaultRadioButton.CheckedChanged += new System.EventHandler(this.SaveDefaultRadioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.currentPriorityLabel);
            this.groupBox2.Controls.Add(this.inactiveSettingsComboBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.inactiveDefaultPriorityRadioButton);
            this.groupBox2.Controls.Add(this.inactiveSetPriorityRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 117);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Inactive settings";
            // 
            // currentPriorityLabel
            // 
            this.currentPriorityLabel.AutoSize = true;
            this.currentPriorityLabel.Location = new System.Drawing.Point(172, 83);
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
            this.inactiveSettingsComboBox.Location = new System.Drawing.Point(252, 49);
            this.inactiveSettingsComboBox.Name = "inactiveSettingsComboBox";
            this.inactiveSettingsComboBox.Size = new System.Drawing.Size(121, 24);
            this.inactiveSettingsComboBox.TabIndex = 9;
            this.inactiveSettingsComboBox.SelectedIndexChanged += new System.EventHandler(this.InactiveSettingsComboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Current priority level:";
            // 
            // inactiveDefaultPriorityRadioButton
            // 
            this.inactiveDefaultPriorityRadioButton.AutoSize = true;
            this.inactiveDefaultPriorityRadioButton.Location = new System.Drawing.Point(7, 21);
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
            this.inactiveSetPriorityRadioButton.Location = new System.Drawing.Point(7, 49);
            this.inactiveSetPriorityRadioButton.Name = "inactiveSetPriorityRadioButton";
            this.inactiveSetPriorityRadioButton.Size = new System.Drawing.Size(226, 21);
            this.inactiveSetPriorityRadioButton.TabIndex = 8;
            this.inactiveSetPriorityRadioButton.TabStop = true;
            this.inactiveSetPriorityRadioButton.Text = "Set priority value while inactive:";
            this.inactiveSetPriorityRadioButton.UseVisualStyleBackColor = true;
            this.inactiveSetPriorityRadioButton.CheckedChanged += new System.EventHandler(this.InactiveSetPriorityRadioButton_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.openSettingsComboBox);
            this.groupBox3.Controls.Add(this.openDefaultRadioButton);
            this.groupBox3.Controls.Add(this.openSetPriorityRadioButton);
            this.groupBox3.Location = new System.Drawing.Point(397, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(391, 89);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Open settings";
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
            this.openSettingsComboBox.Location = new System.Drawing.Point(264, 50);
            this.openSettingsComboBox.Name = "openSettingsComboBox";
            this.openSettingsComboBox.Size = new System.Drawing.Size(121, 24);
            this.openSettingsComboBox.TabIndex = 9;
            this.openSettingsComboBox.SelectedIndexChanged += new System.EventHandler(this.OpenSettingsComboBox_SelectedIndexChanged);
            // 
            // openDefaultRadioButton
            // 
            this.openDefaultRadioButton.AutoSize = true;
            this.openDefaultRadioButton.Location = new System.Drawing.Point(6, 22);
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
            this.openSetPriorityRadioButton.Location = new System.Drawing.Point(6, 50);
            this.openSetPriorityRadioButton.Name = "openSetPriorityRadioButton";
            this.openSetPriorityRadioButton.Size = new System.Drawing.Size(247, 21);
            this.openSetPriorityRadioButton.TabIndex = 8;
            this.openSetPriorityRadioButton.TabStop = true;
            this.openSetPriorityRadioButton.Text = "Always set priority values on open:";
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Advanced settings";
            // 
            // dialogEnableAdvancedSettings
            // 
            this.dialogEnableAdvancedSettings.AutoSize = true;
            this.dialogEnableAdvancedSettings.Location = new System.Drawing.Point(3, 29);
            this.dialogEnableAdvancedSettings.Name = "dialogEnableAdvancedSettings";
            this.dialogEnableAdvancedSettings.Size = new System.Drawing.Size(193, 21);
            this.dialogEnableAdvancedSettings.TabIndex = 7;
            this.dialogEnableAdvancedSettings.Text = "Enable advanced settings";
            this.dialogEnableAdvancedSettings.UseVisualStyleBackColor = true;
            this.dialogEnableAdvancedSettings.CheckedChanged += new System.EventHandler(this.DialogEnableAdvancedSettings_CheckedChanged);
            // 
            // advancedGroupBox
            // 
            this.advancedGroupBox.Controls.Add(this.setPriorityBoostDropdown);
            this.advancedGroupBox.Controls.Add(this.priorityBoostStatusLabel);
            this.advancedGroupBox.Controls.Add(this.label3);
            this.advancedGroupBox.Controls.Add(this.setPriorityBoostButton);
            this.advancedGroupBox.Controls.Add(this.allowHighPriorityButton);
            this.advancedGroupBox.Controls.Add(this.allowRealtimePriorityButton);
            this.advancedGroupBox.Enabled = false;
            this.advancedGroupBox.Location = new System.Drawing.Point(9, 49);
            this.advancedGroupBox.Name = "advancedGroupBox";
            this.advancedGroupBox.Size = new System.Drawing.Size(382, 119);
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
            this.setPriorityBoostDropdown.TabIndex = 5;
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
            this.setPriorityBoostButton.TabIndex = 2;
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
            this.allowHighPriorityButton.TabIndex = 1;
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
            this.allowRealtimePriorityButton.TabIndex = 0;
            this.allowRealtimePriorityButton.Text = "Allow Realtime priority (DANGEROUS)";
            this.allowRealtimePriorityButton.UseVisualStyleBackColor = true;
            this.allowRealtimePriorityButton.CheckedChanged += new System.EventHandler(this.AllowRealtimePriorityButton_CheckedChanged);
            // 
            // settingsTooltip
            // 
            this.settingsTooltip.Popup += new System.Windows.Forms.PopupEventHandler(this.SettingsTooltip_Popup);
            // 
            // advancedSettingsPanel
            // 
            this.advancedSettingsPanel.Controls.Add(this.advancedGroupBox);
            this.advancedSettingsPanel.Controls.Add(this.label2);
            this.advancedSettingsPanel.Controls.Add(this.dialogEnableAdvancedSettings);
            this.advancedSettingsPanel.Location = new System.Drawing.Point(397, 107);
            this.advancedSettingsPanel.Name = "advancedSettingsPanel";
            this.advancedSettingsPanel.Size = new System.Drawing.Size(403, 180);
            this.advancedSettingsPanel.TabIndex = 9;
            // 
            // SettingsWindow
            // 
            this.AcceptButton = this.dialogButtonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.dialogButtonCancel;
            this.ClientSize = new System.Drawing.Size(800, 287);
            this.Controls.Add(this.advancedSettingsPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dialogButtonCancel);
            this.Controls.Add(this.dialogButtonSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsWindow";
            this.Text = "KeeSetPriority Settings";
            this.Load += new System.EventHandler(this.SettingsWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.advancedGroupBox.ResumeLayout(false);
            this.advancedGroupBox.PerformLayout();
            this.advancedSettingsPanel.ResumeLayout(false);
            this.advancedSettingsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button dialogButtonSave;
        private System.Windows.Forms.Button dialogButtonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.Panel advancedSettingsPanel;
    }
}