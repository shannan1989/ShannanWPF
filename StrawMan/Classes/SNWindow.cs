using System.Reflection;
using System.Windows;

namespace Shannan.StrawMan
{
    public class SNWindow : Window
    {
        private static readonly FieldInfo _menuDropAlignmentField;

        public SNWindow()
        {
            ShowInTaskbar = true;
            WindowState = WindowState.Maximized;

            PreviewMouseDoubleClick += (sender, e) =>
            {
                e.Handled = true;
            };
        }

        static SNWindow()
        {
            _menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            EnsureStandardPopupAlignment();
            //SystemParameters.StaticPropertyChanged += delegate
            //{
            //    EnsureStandardPopupAlignment();
            //};
        }

        private static void EnsureStandardPopupAlignment()
        {
            if (SystemParameters.MenuDropAlignment && _menuDropAlignmentField != null)
            {
                _menuDropAlignmentField.SetValue(null, false);
            }
        }
    }
}
