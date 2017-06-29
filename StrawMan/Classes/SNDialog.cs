using System.Windows;

namespace Shannan.StrawMan
{
    public class SNDialog : Window
    {
        public SNDialog()
        {
            ShowInTaskbar = false;

            PreviewMouseDoubleClick += (sender, e) =>
            {
                e.Handled = true;
            };
        }
    }
}
