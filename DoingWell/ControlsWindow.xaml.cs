using System;
using System.Windows;
using System.Windows.Media.Imaging;

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

            imageViewer.Source = new BitmapImage(new Uri("http://uploads.xuexila.com/allimg/1507/641-150G31I335.jpg"));
        }

        private void Pager_CurrentPageChanged(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(pager.CurrentPage.ToString());
        }

    }
}
