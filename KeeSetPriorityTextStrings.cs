using System.Diagnostics;

namespace KeeSetPriority
{
    public static class KeeSetPriorityTextStrings
    {
        public static string GetPriorityLevelStr(ProcessPriorityClassKSP prio)
        {
            switch (prio)
            {
                case ProcessPriorityClassKSP.Default:
                    return DefaultLevelStr;
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

        private const string DefaultLevelStr = "Default";
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

        public const string RealtimeTooltipStr = "Realtime priority not available; KeePass needs administrator privileges to enable realtime priority";

        public const string ErrorPriorityLevelRWStr = "Error reading and writing priority level from process. This may indicate problems with the application or the OS.\n\nThe plugin KeeSetPriority will not load.\n\nFull exception:\n";
        public const string ErrorPriorityBoostRWStr = "Error reading and writing priority boost state from process. This may indicate problems with the application or the OS.\n\nFull exception:\n";

        public const string AdvancedSettingsBoxStr = "You are about to turn advanced settings on.\n\nKeep in mind these settings can cause undesired side effects and may cause data loss or corruption.\n\nAre you sure you want to continue?";
    }
}
