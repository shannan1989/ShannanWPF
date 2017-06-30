using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Shannan.StrawMan
{
    public partial class App : Application
    {
        public App()
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            File.WriteAllText("log.txt", "我们很抱歉，当前应用程序（" + Assembly.GetEntryAssembly().GetName().Version.ToString(3) + "）遇到一些问题：" + e.ToString() + e.Exception.StackTrace);
            //LToast.Show("我们很抱歉，当前应用程序（" + Assembly.GetEntryAssembly().GetName().Version.ToString(3) + "）遇到一些问题：" + e.ToString());
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            File.WriteAllText("log.txt", "我们很抱歉，当前应用程序（" + Assembly.GetEntryAssembly().GetName().Version.ToString(3) + "）遇到一些问题：" + e.ToString() + e.ExceptionObject.ToString());
            //LToast.Show("我们很抱歉，当前应用程序（" + Assembly.GetEntryAssembly().GetName().Version.ToString(3) + "）遇到一些问题：" + e.ToString());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("zh-CN");
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            //Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            HomeWindow window = new HomeWindow();
            window.Show();

            base.OnStartup(e);
        }
    }
}
