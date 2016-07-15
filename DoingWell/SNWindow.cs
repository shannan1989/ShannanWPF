using System.Windows;
using System.Windows.Input;

namespace Shannan.DoingWell
{
    public class SNWindow : Window
    {
        public SNWindow()
        {
            Loaded += SNWindow_Loaded;
            PreviewMouseDoubleClick += SNWindow_PreviewMouseDoubleClick;

            ShowInTaskbar = true;
            WindowState = WindowState.Maximized;
            //ResizeMode = ResizeMode.NoResize;
        }

        private void SNWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Width = SystemParameters.WorkArea.Width;
            Height = SystemParameters.WorkArea.Height;
        }

        private void SNWindow_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
