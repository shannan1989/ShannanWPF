using Microsoft.Win32;
using System;
using System.Text;
using System.Threading;
using System.Windows;

namespace Shannan.StrawMan
{
    public partial class DInstalledSoftware : SNDialog
    {
        public DInstalledSoftware()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Loaded += delegate
            {
                //GetInstalledSoftwares();
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                    GetInstalledSoftwares();
                }));
            };
        }

        private void GetInstalledSoftwares()
        {
            StringBuilder info = new StringBuilder();
            string keyStr = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyStr))
            {
                foreach (string subKeyName in key.GetSubKeyNames())
                {
                    info.AppendLine(subKeyName);
                    using (RegistryKey subKey = key.OpenSubKey(subKeyName))
                    {
                        foreach (string name in subKey.GetValueNames())
                        {
                            info.AppendLine("---- " + name + " : " + subKey.GetValue(name));
                        }
                    }
                    info.AppendLine();
                }
            }

            info.AppendLine("----------------------------------------------------------");

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyStr))
            {
                foreach (string subKeyName in key.GetSubKeyNames())
                {
                    info.AppendLine(subKeyName);
                    using (RegistryKey subKey = key.OpenSubKey(subKeyName))
                    {
                        foreach (string name in subKey.GetValueNames())
                        {
                            info.AppendLine("---- " + name + " : " + subKey.GetValue(name));
                        }
                    }
                    info.AppendLine();
                }
            }

            Thread.Sleep(1000);
            ApplicationsInfo.Text = info.ToString();
        }
    }
}
