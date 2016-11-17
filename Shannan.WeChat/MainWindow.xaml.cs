using System.ComponentModel;
using System.Windows;

namespace Shannan.WeChat
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += delegate
            {
            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Current.Shutdown();
            base.OnClosing(e);
        }
    }
}
