﻿using Microsoft.Win32;
using System;
namespace Eu4Importer
{
    public static class InstalledApplications
    {
        public static string GetApplictionInstallPath(string nameOfAppToFind)
        {
            string installedPath;
            string keyName;

            // search in: CurrentUser
            keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            installedPath = ExistsInSubKey(Registry.CurrentUser, keyName, "DisplayName", nameOfAppToFind);
            if (!string.IsNullOrEmpty(installedPath))
            {
                return installedPath;
            }

            // search in: LocalMachine_32
            keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            installedPath = ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", nameOfAppToFind);
            if (!string.IsNullOrEmpty(installedPath))
            {
                return installedPath;
            }

            // search in: LocalMachine_64
            keyName = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            installedPath = ExistsInSubKey(Registry.LocalMachine, keyName, "DisplayName", nameOfAppToFind);
            if (!string.IsNullOrEmpty(installedPath))
            {
                return installedPath;
            }

            return string.Empty;
        }

        private static string ExistsInSubKey(RegistryKey root, string subKeyName, string attributeName, string nameOfAppToFind)
        {
            RegistryKey subkey;
            string displayName;

            using (RegistryKey key = root.OpenSubKey(subKeyName))
            {
                if (key != null)
                {
                    foreach (string kn in key.GetSubKeyNames())
                    {
                        using (subkey = key.OpenSubKey(kn))
                        {
                            displayName = subkey.GetValue(attributeName) as string;
                            if (nameOfAppToFind.Equals(displayName, StringComparison.OrdinalIgnoreCase) == true)
                            {
                                return subkey.GetValue("InstallLocation") as string;
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
