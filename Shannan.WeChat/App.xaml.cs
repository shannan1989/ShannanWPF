using System.Windows;

namespace Shannan.WeChat
{
    public partial class App : Application
    {
        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            LoginWindow lw = new LoginWindow();
            bool? lwResult = lw.ShowDialog();
            if (lwResult.HasValue && lwResult.Value == true)
            {
                MainWindow window = new MainWindow();
                window.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}
