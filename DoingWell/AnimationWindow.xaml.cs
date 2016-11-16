using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Shannan.DoingWell
{
    public partial class AnimationWindow : Window
    {
        private Rectangle rect;

        public AnimationWindow()
        {
            InitializeComponent();

            Loaded += AnimationWindow_Loaded;
        }

        private void AnimationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Colors.Green);
            rect.Width = 50;
            rect.Height = 50;
            rect.RadiusX = 5;
            rect.RadiusY = 5;
            Carrier.Children.Add(rect);
            Canvas.SetTop(rect, 0);
            Canvas.SetLeft(rect, 0);
        }

        private void Carrier_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(Carrier);

            Storyboard storyboard = new Storyboard();

            DoubleAnimation leftAnimation = new DoubleAnimation(Canvas.GetLeft(rect), p.X, new Duration(TimeSpan.FromMilliseconds(500)));
            storyboard.Children.Add(leftAnimation);
            Storyboard.SetTarget(leftAnimation, rect);
            Storyboard.SetTargetProperty(leftAnimation, new PropertyPath("(Canvas.Left)"));

            DoubleAnimation topAnimation = new DoubleAnimation(Canvas.GetTop(rect), p.Y, new Duration(TimeSpan.FromMilliseconds(500)));
            storyboard.Children.Add(topAnimation);
            Storyboard.SetTarget(topAnimation, rect);
            Storyboard.SetTargetProperty(topAnimation, new PropertyPath("(Canvas.Top)"));

            if (Resources.Contains("rectAnimation") == false)
            {
                Resources.Add("rectAnimation", storyboard);
            }

            storyboard.Begin();
        }
    }
}
