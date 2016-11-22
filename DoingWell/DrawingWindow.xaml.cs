using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Shannan.DoingWell
{
    public partial class DrawingWindow : SNWindow
    {
        public DrawingWindow()
        {
            InitializeComponent();

            Loaded += DrawingWindow_Loaded;
        }

        private void DrawingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            initImage.Source = new BitmapImage(new Uri("http://m.leleketang.com/temp/homework/160708/6304825808408639956.jpeg"));
            drawBorder.MouseEnter += DrawBorder_MouseEnter;

            ObservableCollection<object> colors = new ObservableCollection<object>();
            colors.Add(new
            {
                ColorName = "红",
                FillColor = new SolidColorBrush(Colors.Red)
            });
            colors.Add(new
            {
                ColorName = "黑",
                FillColor = new SolidColorBrush(Colors.Black)
            });
            colorsList.ItemsSource = colors;
            colorsList.SelectionChanged += ColorsList_SelectionChanged;

            pigaiButton.Checked += PigaiButton_Checked;
            eraseButton.Checked += EraseButton_Checked;
            pigaiButton.IsChecked = true;
        }

        private void DrawBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            //drawBorder.Height = initImage.ActualHeight;
            //drawBorder.Width = initImage.ActualWidth;
        }

        private void PigaiButton_Checked(object sender, RoutedEventArgs e)
        {
            drawCanvas.EditingMode = InkCanvasEditingMode.Ink;
            drawCanvas.Cursor = Cursors.Pen;
        }

        private void EraseButton_Checked(object sender, RoutedEventArgs e)
        {
            drawCanvas.EraserShape = new EllipseStylusShape(6, 6);
            drawCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            drawCanvas.Cursor = Cursors.Hand;
        }

        private void ColorsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            drawCanvas.DefaultDrawingAttributes.Color = ((sender as ListBox).SelectedValue as SolidColorBrush).Color;
        }
    }
}
