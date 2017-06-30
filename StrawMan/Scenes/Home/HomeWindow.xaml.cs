using System.Windows;
using System.Windows.Controls;

namespace Shannan.StrawMan
{
    public partial class HomeWindow : SNWindow
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            switch (tag)
            {
                case "SystemInfo":
                    SNDialog dSystemInfo = new DSystemInfo();
                    dSystemInfo.Owner = GetWindow(this);
                    dSystemInfo.ShowDialog();
                    break;

                case "InstalledSoftware":
                    SNDialog dInstalledSoftware = new DInstalledSoftware();
                    dInstalledSoftware.Owner = GetWindow(this);
                    dInstalledSoftware.ShowDialog();
                    break;

                case "MultiTouch":
                    SNDialog dMultiTouch = new DMultiTouch();
                    dMultiTouch.Owner = GetWindow(this);
                    dMultiTouch.ShowDialog();
                    break;

                case "Manipulation":
                    SNDialog dManipulation = new DManipulation();
                    dManipulation.Owner = GetWindow(this);
                    dManipulation.ShowDialog();
                    break;

                case "Styles":
                    SNDialog dStyles = new DStyles();
                    dStyles.Owner = GetWindow(this);
                    dStyles.ShowDialog();
                    break;

                case "UserControls":
                    SNDialog dUserControls = new DUserControls();
                    dUserControls.Owner = GetWindow(this);
                    dUserControls.ShowDialog();
                    break;

                default:
                    break;
            }
        }
    }
}
