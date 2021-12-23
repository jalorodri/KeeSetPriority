using System;
using System.Windows.Forms;

namespace KeeSetPriority
{
    public sealed partial class KeeSetPriorityData
    {
        /// <summary>
        /// Called when legacy settings are detected. Will throw an exception if the program is not allowed to continue (if the user didn't want to set the
        /// settings to default when errors or inconsistencies were detected)
        /// </summary>
        /// <param name="ver"></param>
        /// <exception cref="KSPException"></exception>
        private void OnLegacySettings(LegacyVersions ver)
        {
            DialogResult warningBox = MessageBox.Show(KeeSetPriorityTextStrings.LegacySettingsBoxStr,
                KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (warningBox == DialogResult.Yes)
            {
                try
                {
                    switch (ver)
                    {
                        case LegacyVersions.v0_5:
                            ReadAndValidateSettingsLegacy_v0_5();
                            break;
                    case LegacyVersions.v0_8:
                            ReadAndValidateSettingsLegacy_v0_8(); // Throws an exception if the data is corrupted and the user chooses not to turn to default settings
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnOpenStringLegacy_v05, null);
                    m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnOpenStringLegacy_v08, null);
                    m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnSaveStringLegacy_v08, null);
                    m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnSaveStringLegacy_v05, null);
                    m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnInactiveStringLegacy_v08, null);
                    m_host.CustomConfig.SetString(KeeSetPriorityTextStrings.changePriorityOnInactiveStringLegacy_v05, null);
                }
            }
            else
            {
                throw new KSPException();
            }
        }

        /// <summary>
        /// This function is to be called before any attempt to read the data into the <c>configDataHoldingStruct</c>,
        /// and will attempt to check what version of the settings is in use
        /// </summary>
        /// <returns>LegacyVersions value as defined in the LegacyVersions enum</returns>
        private static LegacyVersions CheckLegacySettings()
        {
            //Legacy v0.8 settings
            ProcessPriorityClassKSP testPPCKSP;
            if (m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnOpenStringLegacy_v08) != null ||
                m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnSaveStringLegacy_v08) != null ||
                m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnInactiveStringLegacy_v08) != null)
            {
                // Since they share access strings with current versions, it needs to check whether at least one of the strings is in the legacy format
                if (ProcessPriorityClassKSP.TryParse(m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnOpenStringLegacy_v08), true, out testPPCKSP) ||
                    ProcessPriorityClassKSP.TryParse(m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnSaveStringLegacy_v08), true, out testPPCKSP) ||
                    ProcessPriorityClassKSP.TryParse(m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnInactiveStringLegacy_v08), true, out testPPCKSP))
                {
                    return LegacyVersions.v0_8;
                }
            }

            // Legacy v0.5 settings
            // The access strings for v0.5 are not shared by any other version, so if they're defined at all they're from this version
            if (m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnOpenStringLegacy_v05) != null ||
                m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnSaveStringLegacy_v05) != null ||
                m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnInactiveStringLegacy_v05) != null)
            {
                return LegacyVersions.v0_5;
            }

            return LegacyVersions.current;
        }

        /// <summary>
        /// This function will attempt to read the data from the v0.8 settings. If it's corrupted, it will show a warning to the user and ask for permission
        /// to go back to default values. If the user declines, it will throw an exception.
        /// </summary>
        /// <exception cref="KSPException">When the user decides not to reset the value to default</exception>
        private void ReadAndValidateSettingsLegacy_v0_8()
        {
            try
            {
                configDataStruct.SetPriorityLevel(ActionTypesKSP.Open, (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnOpenStringLegacy_v08), true));
                configDataStruct.SetPriorityMode(ActionTypesKSP.Open, configDataStruct.GetPriorityLevel(ActionTypesKSP.Open) == ProcessPriorityClassKSP.NotSet ? PriorityChangeTypes.NeverSet : PriorityChangeTypes.AlwaysSet);
            }
            catch
            {
                if (KeeSetPriorityTextStrings.GetDataCorrectionWarning(ActionTypesKSP.Open) == DialogResult.Yes)
                {
                    configDataStruct.SetPriorityLevel(ActionTypesKSP.Open, ProcessPriorityClassKSP.NotSet);
                    configDataStruct.SetPriorityMode(ActionTypesKSP.Open, PriorityChangeTypes.NeverSet);
                }
                else
                {
                    throw new KSPException();
                }
            }
            try
            { 
                configDataStruct.SetPriorityLevel(ActionTypesKSP.Save, (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnSaveStringLegacy_v08), true));
                configDataStruct.SetPriorityMode(ActionTypesKSP.Save, configDataStruct.GetPriorityLevel(ActionTypesKSP.Save) == ProcessPriorityClassKSP.NotSet ? PriorityChangeTypes.NeverSet : PriorityChangeTypes.AlwaysSet);
            }
            catch
            {
                if (KeeSetPriorityTextStrings.GetDataCorrectionWarning(ActionTypesKSP.Save) == DialogResult.Yes)
                {
                    configDataStruct.SetPriorityLevel(ActionTypesKSP.Save, ProcessPriorityClassKSP.NotSet);
                    configDataStruct.SetPriorityMode(ActionTypesKSP.Save, PriorityChangeTypes.NeverSet);
                }
                else
                {
                    throw new KSPException();
                }
            }
            try
            {
                configDataStruct.SetPriorityLevel(ActionTypesKSP.Inactive, (ProcessPriorityClassKSP)Enum.Parse(typeof(ProcessPriorityClassKSP), m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnInactiveStringLegacy_v08), true));
                configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive) == ProcessPriorityClassKSP.NotSet ? PriorityChangeTypes.NeverSet : PriorityChangeTypes.AlwaysSet);
            }
            catch
            {
                if (KeeSetPriorityTextStrings.GetDataCorrectionWarning(ActionTypesKSP.Inactive) == DialogResult.Yes)
                {
                    configDataStruct.SetPriorityLevel(ActionTypesKSP.Inactive, ProcessPriorityClassKSP.NotSet);
                    configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, PriorityChangeTypes.NeverSet);
                }
                else
                {
                    throw new KSPException();
                }
            }
            try { configDataStruct.isAdvancedOptionsAvailable = bool.Parse(m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.isAdvancedOptionsAvailableString, bool.FalseString)); }
            catch
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.AdvancedOptionsDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    configDataStruct.isAdvancedOptionsAvailable = false;
                    configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
                    configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
                }
                else
                {
                    throw new KSPException();
                }
            }
            try { configDataStruct.allowDangerousPrioritites = (AllowDangerousPrioritites)Enum.Parse(typeof(AllowDangerousPrioritites), m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.allowDangerousPrioritiesString, AllowDangerousPrioritites.No.ToString())); }
            catch
            {
                if (MessageBox.Show(KeeSetPriorityTextStrings.AllowDangerousPrioritiesDataCorrectionWarning, KeeSetPriorityTextStrings.WarningBoxTitleStr, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    configDataStruct.isAdvancedOptionsAvailable = false;
                    configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.No;
                    configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;
                }
                else
                {
                    throw new KSPException();
                }
            }
            // I forgot to save the priority boost state in v0.8, so I'm just gonna say it's default
            configDataStruct.priorityBoostState = PriorityBoostTypesKSP.Default;

        }

        /// <summary>
        /// This function will attempt to read the data from the v0.5 settings. If it's corrupted, it will show a warning to the user and ask for permission
        /// to go back to default values. If the user declines, it will throw an exception.
        /// </summary>
        /// <exception cref="KSPException">When the user decides not to reset the value to default</exception>
        private void ReadAndValidateSettingsLegacy_v0_5()
        {
            // Open settings
            try
            {
                this.configDataStruct.SetPriorityLevel(ActionTypesKSP.Open, (ProcessPriorityClassKSP)int.Parse(m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnOpenStringLegacy_v05)));
                switch (configDataStruct.GetPriorityLevel(ActionTypesKSP.Open))
                {
                    case ProcessPriorityClassKSP.Default:
                        configDataStruct.SetPriorityLevel(ActionTypesKSP.Open, ProcessPriorityClassKSP.NotSet);
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Open, PriorityChangeTypes.NeverSet);
                        break;
                    case ProcessPriorityClassKSP.RealTime:
                        configDataStruct.isAdvancedOptionsAvailable = true;
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Open, PriorityChangeTypes.AlwaysSet);
                        configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.Yes;
                        break;
                    case ProcessPriorityClassKSP.High:
                        configDataStruct.isAdvancedOptionsAvailable = true;
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Open, PriorityChangeTypes.AlwaysSet);
                        configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.OnlyHigh;
                        break;
                    default:
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Open, PriorityChangeTypes.AlwaysSet);
                        break;
                }
            }
            catch
            {
                if (KeeSetPriorityTextStrings.GetDataCorrectionWarning(ActionTypesKSP.Open) == DialogResult.Yes)
                {
                    configDataStruct.SetPriorityMode(ActionTypesKSP.Open, PriorityChangeTypes.NeverSet);
                    configDataStruct.SetPriorityLevel(ActionTypesKSP.Open, ProcessPriorityClassKSP.NotSet);
                }
                else
                {
                    throw new KSPException();
                }
            }

            // Save settings
            try
            {
                configDataStruct.SetPriorityLevel(ActionTypesKSP.Save, (ProcessPriorityClassKSP)int.Parse(m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnSaveStringLegacy_v05)));
                switch (configDataStruct.GetPriorityLevel(ActionTypesKSP.Save))
                {
                    case ProcessPriorityClassKSP.Default:
                        configDataStruct.SetPriorityLevel(ActionTypesKSP.Save, ProcessPriorityClassKSP.NotSet);
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Save, PriorityChangeTypes.NeverSet);
                        break;
                    case ProcessPriorityClassKSP.RealTime:
                        configDataStruct.isAdvancedOptionsAvailable = true;
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Save, PriorityChangeTypes.AlwaysSet);
                        configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.Yes;
                        break;
                    case ProcessPriorityClassKSP.High:
                        configDataStruct.isAdvancedOptionsAvailable = true;
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Save, PriorityChangeTypes.AlwaysSet);
                        configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.OnlyHigh;
                        break;
                    default:
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Save, PriorityChangeTypes.AlwaysSet);
                        break;
                }
            }
            catch
            {
                if (KeeSetPriorityTextStrings.GetDataCorrectionWarning(ActionTypesKSP.Save) == DialogResult.Yes)
                {
                    configDataStruct.SetPriorityMode(ActionTypesKSP.Save, PriorityChangeTypes.NeverSet);
                    configDataStruct.SetPriorityLevel(ActionTypesKSP.Save, ProcessPriorityClassKSP.NotSet);
                }
                else
                {
                    throw new KSPException();
                }
            }

            // Inactive settings
            try
            {
                configDataStruct.SetPriorityLevel(ActionTypesKSP.Inactive, (ProcessPriorityClassKSP)int.Parse(m_host.CustomConfig.GetString(KeeSetPriorityTextStrings.changePriorityOnInactiveStringLegacy_v05)));
                switch (configDataStruct.GetPriorityLevel(ActionTypesKSP.Inactive))
                {
                    case ProcessPriorityClassKSP.Default:
                        configDataStruct.SetPriorityLevel(ActionTypesKSP.Inactive, ProcessPriorityClassKSP.NotSet);
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, PriorityChangeTypes.AlwaysSet);
                        break;
                    case ProcessPriorityClassKSP.RealTime:
                        configDataStruct.isAdvancedOptionsAvailable = true;
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, PriorityChangeTypes.AlwaysSet);
                        configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.Yes;
                        break;
                    case ProcessPriorityClassKSP.High:
                        configDataStruct.isAdvancedOptionsAvailable = true;
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, PriorityChangeTypes.AlwaysSet);
                        configDataStruct.allowDangerousPrioritites = AllowDangerousPrioritites.OnlyHigh;
                        break;
                    default:
                        configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, PriorityChangeTypes.AlwaysSet);
                        break;
                }
            }
            catch
            {
                if (KeeSetPriorityTextStrings.GetDataCorrectionWarning(ActionTypesKSP.Inactive) == DialogResult.Yes)
                {
                    configDataStruct.SetPriorityMode(ActionTypesKSP.Inactive, PriorityChangeTypes.NeverSet);
                    configDataStruct.SetPriorityLevel(ActionTypesKSP.Inactive, ProcessPriorityClassKSP.NotSet);
                }
                else
                {
                    throw new KSPException();
                }
            }
        }

        public enum LegacyVersions
        {
            v0_5,
            v0_8,
            current
        }
    }
}