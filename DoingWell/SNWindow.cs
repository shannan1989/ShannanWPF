using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Shannan.DoingWell
{
    public class SNWindow : Window
    {
        public SNWindow()
        {
            Loaded += SNWindow_Loaded;
            PreviewMouseDoubleClick += SNWindow_PreviewMouseDoubleClick;
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ButtonClicked));

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

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine((e.OriginalSource as Button));
        }
    }
}
