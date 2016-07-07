using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shannan.DoingWell.UserControls
{
    public partial class DrawLine : UserControl
    {
        public DrawLine()
        {
            InitializeComponent();

            Loaded += DrawLine_Loaded;
        }

        private void DrawLine_Loaded(object sender, RoutedEventArgs e)
        {
            saveButton.Click += SaveButton_Click;

            colorSelector.SelectionChanged += ColorSelector_SelectionChanged;
            styleSelector.SelectionChanged += StyleSelector_SelectionChanged;

            lineCanvas.PreviewMouseLeftButtonDown += LineCanvas_PreviewMouseLeftButtonDown;
            lineCanvas.PreviewMouseLeftButtonUp += LineCanvas_PreviewMouseLeftButtonUp;
            lineCanvas.PreviewMouseMove += LineCanvas_PreviewMouseMove;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var rtb = new RenderTargetBitmap((int)lineCanvas.ActualWidth, (int)lineCanvas.ActualHeight, 96, 96, PixelFormats.Default);
            rtb.Render(lineCanvas);

            displayImage.Source = rtb;
        }

        Point startPoint;
        List<Point> points = new List<Point>();

        Brush lineStroke = Brushes.Black;
        DoubleCollection lineStrokeDashArray = new DoubleCollection(new List<double>() { });


        private void LineCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(sender as Canvas);
        }

        private void LineCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            points.Clear();
        }

        private void LineCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var current = e.GetPosition(sender as Canvas);
            if (points.Count == 0)
            {
                points.Add(new Point(startPoint.X, startPoint.Y));
            }
            else
            {
                points.Add(current);
            }

            //points.Distinct().ToList();
            int count = points.Count();
            if (count >= 2)
            {
                Line line = new Line();
                line.Stroke = lineStroke;
                line.StrokeDashArray = lineStrokeDashArray;
                line.StrokeThickness = 2;

                line.X1 = points[count - 2].X;
                line.Y1 = points[count - 2].Y;
                line.X2 = current.X;
                line.Y2 = current.Y;

                lineCanvas.Children.Add(line);
            }
        }

        private void ColorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string color = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
            if (color == "默认")
            {
                lineStroke = Brushes.Black;
            }
            if (color == "红色")
            {
                lineStroke = Brushes.Red;
            }
            if (color == "绿色")
            {
                lineStroke = Brushes.Green;
            }
        }

        private void StyleSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string style = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
            if (style == "默认")
            {
                lineStrokeDashArray = new DoubleCollection(new List<double>() { });
            }
            if (style == "虚线")
            {
                lineStrokeDashArray = new DoubleCollection(new List<double>() { 1, 1, 1, 1 });
            }
        }

    }
}
