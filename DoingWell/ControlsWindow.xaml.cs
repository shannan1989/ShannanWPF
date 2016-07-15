using System.Windows;

namespace Shannan.DoingWell
{
    public partial class ControlsWindow : SNWindow
    {
        public ControlsWindow()
        {
            InitializeComponent();

            Loaded += ControlsWindow_Loaded;
        }

        private void ControlsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            pager.CurrentPageChanged += Pager_CurrentPageChanged;
        }

        private void Pager_CurrentPageChanged(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(pager.CurrentPage.ToString());
        }

    }
}
