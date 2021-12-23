using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace KeeSetPriority
{
    internal static class KeeSetPriorityTextStrings
    {
        public static string GetPriorityLevelStr(ProcessPriorityClassKSP prio)
        {
            switch (prio)
            {
                case ProcessPriorityClassKSP.RealTime:
                    return RealtimeStr;
                case ProcessPriorityClassKSP.High:
                    return HighStr;
                case ProcessPriorityClassKSP.AboveNormal:
                    return AboveNormalStr;
                case ProcessPriorityClassKSP.Normal:
                    return NormalStr;
                case ProcessPriorityClassKSP.BelowNormal:
                    return BelowNormalStr;
                case ProcessPriorityClassKSP.Idle:
                    return IdleStr;
                default:
                    return null;
            }
        }

        private const string RealtimeStr = "Realtime";
        private const string HighStr = "High";
        private const string AboveNormalStr = "Above normal";
        private const string NormalStr = "Normal";
        private const string BelowNormalStr = "Below Normal";
        private const string IdleStr = "Idle";

        public const string FormalNameStr = "KeeSetPriority";
        public const string OpenPhraseStr = "Set priority level on database open";
        public const string SavePhraseStr = "Set priority level on database save";
        public const string InactivePhraseStr = "Set priority level while inactive";
        public const string ErrorBoxTitleStr = "KeeSetPriority Error";
        public const string WarningBoxTitleStr = "KeeSetPriority Warning";

        public const string EnabledStr = "Enabled";
        public const string DisabledStr = "Disabled";
        public const string NotAvailableStr = "Not available";

        private const string OnOpenDataCorrectionWarning = "The data to set priority while the database is opening is corrupted or invalid.\n\nDo you want to reset the setting to its default value? KeeSetPriority will not be able to continue if you press 'no'.";
        private const string OnSaveDataCorrectionWarning = "The data to set priority while the database is saving is corrupted or invalid.\n\nDo you want to reset the setting to its default value? KeeSetPriority will not be able to continue if you press 'no'.";
        private const string OnInactiveDataCorrectionWarning = "The data to set priority while KeePass is running is corrupted or invalid.\n\nDo you want to reset the setting to its default value? KeeSetPriority will not be able to continue if you press 'no'.";
        
        internal static DialogResult GetDataCorrectionWarning(ActionTypesKSP action)
        {
            switch (action)
            {
                case ActionTypesKSP.Open:
                    return MessageBox.Show(KeeSetPriorityTextStrings.OnOpenDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                case ActionTypesKSP.Save:
                    return MessageBox.Show(KeeSetPriorityTextStrings.OnSaveDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                case ActionTypesKSP.Inactive:
                    return MessageBox.Show(KeeSetPriorityTextStrings.OnInactiveDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                default:
                    throw new IndexOutOfRangeException("action not part of supported ActioTypeKSP value.\n\naction = " + action.ToString() + "Function: GetChangePriorityString(ActionTypesKSP action)");
            }
        }

        public const string AdvancedOptionsDataCorrectionWarning = "The data to allow advanced options is corrupted or invalid.\n\nDo you want to reset the setting to its default value? KeeSetPriority will not be able to continue if you press 'no'.";
        public const string AllowDangerousPrioritiesDataCorrectionWarning = "The data to allow dangerous priorities (High, Real time) is corrupted or invalid.\n\nDo you want to reset the setting to its default value? KeeSetPriority will not be able to continue if you press 'no'.";
        public const string PriorityBoostDataCorrectionWarning = "The data to set priority boost is corrupted or invalid.\n\nDo you want to reset the setting to its default value? KeeSetPriority will not be able to continue if you press 'no'.";
        public const string UpdateThreadSettingsCorrectionWarning = "The configuration data for the update thread is corrupted or invalid.\n\nDo you want to reset the setting to its default value? KeeSetPriority will not be able to continue if you press 'no'.";

        public static string HighPriorityWarningMessageStr(ProcessPriorityClassKSP prio, ActionTypesKSP action)
        {
            string str = "You are about to set the priority to " + GetPriorityLevelStr(prio) + ".\n\nThis could cause the computer to stop responding or lock up ";
            switch (action)
            {
                case ActionTypesKSP.Inactive:
                    str += "while the program is running.";
                    break;
                case ActionTypesKSP.Open:
                    str += "while the program is opening a database.";
                    break;
                case ActionTypesKSP.Save:
                    str += "while the program is saving a database.";
                    break;
            }
            str += "\n\nAre you sure you want to continue?";
            return str;
        }
        public static string RealtimePriorityAdvancedSettingsString = "You are about to turn on Realtime priority. This could cause the entire computer to stop responsing and lose your data.\nAre you sure you want to continue?";

        public const string RealtimeTooltipStr = "Realtime priority not available; KeePass needs administrator privileges to enable realtime priority";

        public const string ErrorPriorityLevelRWStr = "Error reading and writing priority level from process. This may indicate problems with the application or the OS.\n\nThe plugin KeeSetPriority will not load.\n\nFull exception:\n";
        public const string ErrorPriorityBoostRWStr = "Error reading and writing priority boost state from process. This may indicate problems with the application or the OS.\n\nFull exception:\n";

        public const string AdvancedSettingsBoxStr = "You are about to turn advanced settings on.\n\nKeep in mind these settings can cause undesired side effects and may cause data loss or corruption.\n\nAre you sure you want to continue?";
        public const string LegacySettingsBoxStr = "Settings saved are from a previous version of KeeSetPriority.\n\nDo you want to upgrade them to the new format? Previous values will not be readable by previous versions of KeeSetPriority.";
        
        #region Configuration strings, do not change
        private const string changePriorityOnOpenString = "KeeSetPriority.priorityLevelOnOpen";
        private const string changePriorityOnSaveString = "KeeSetPriority.priorityLevelOnSave";
        private const string changePriorityOnInactiveString = "KeeSetPriority.priorityLevelOnInactive";
        internal static string GetChangePriorityString(ActionTypesKSP action)
        {
            switch (action)
            {
                case ActionTypesKSP.Open:
                    return changePriorityOnOpenString;
                case ActionTypesKSP.Save:
                    return changePriorityOnSaveString;
                case ActionTypesKSP.Inactive:
                    return changePriorityOnInactiveString;
                default:
                    throw new IndexOutOfRangeException("action not part of supported ActioTypeKSP value.\n\naction = " + action.ToString() + "Function: GetChangePriorityString(ActionTypesKSP action)");
            }
        }

        public const string allowBackgroundSystemProcessesString = "KeeSetPriority.allowBackgroundProcesses";
        public const string changePriorityBoostString = "KeeSetPriority.priorityBoost";
        public const string isAdvancedOptionsAvailableString = "KeeSetPriority.isAdvancedOptionsAvailable";
        public const string allowDangerousPrioritiesString = "KeeSetPriority.allowDangerousPriorities";
        public const string updateThreadSettingsString = "KeeSetPriority.updateThreadSettings";
        // These are legacy configuration strings
        public const string changePriorityOnOpenStringLegacy_v08 = "KeeSetPriority.changePriorityOnOpen";
        public const string changePriorityOnOpenStringLegacy_v05 = "changePriorityOnOpen";
        public const string changePriorityOnSaveStringLegacy_v08 = "KeeSetPriority.changePriorityOnSave";
        public const string changePriorityOnSaveStringLegacy_v05 = "changePriorityOnSave";
        public const string changePriorityOnInactiveStringLegacy_v08 = "KeeSetPriority.changePriorityOnInactive";
        public const string changePriorityOnInactiveStringLegacy_v05 = "changePriorityOnInactive";
        #endregion
    }
}
