using Shannan.StrawMan.Properties;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Shannan.StrawMan
{
    public class WindowBase : Window
    {
        private static readonly FieldInfo _menuDropAlignmentField;

        public WindowBase()
        {
            if (Settings.Default.Debug == true)
            {
                AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ButtonClickHandler));
            }
        }

        private void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            Console.WriteLine((e.OriginalSource as Button));
        }

        static WindowBase()
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
