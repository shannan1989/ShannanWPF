using System.Windows;

namespace Shannan.StrawMan
{
    public class SNWindow : WindowBase
    {
        public SNWindow()
        {
            ShowInTaskbar = true;
            WindowState = WindowState.Maximized;

            PreviewMouseDoubleClick += (sender, e) =>
            {
                e.Handled = true;
            };
        }
    }
}
