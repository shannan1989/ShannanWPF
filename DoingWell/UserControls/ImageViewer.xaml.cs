using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Shannan.DoingWell.UserControls
{
    public partial class ImageViewer : UserControl
    {
        public ImageViewer()
        {
            InitializeComponent();

            Loaded += ImageViewer_Loaded;
        }

        private void ImageViewer_Loaded(object sender, RoutedEventArgs e)
        {
            Photo.MouseLeftButtonDown += Photo_MouseLeftButtonDown;
            Photo.MouseMove += Photo_MouseMove;
            Photo.MouseWheel += Photo_MouseWheel;
        }

        private Point dragPoint;

        private void Photo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragPoint = e.GetPosition(this);
        }

        private void Photo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var current = e.GetPosition(this);
            translateTransform.X += (current.X - dragPoint.X) / scaleTransform.ScaleX;
            translateTransform.Y += (current.Y - dragPoint.Y) / scaleTransform.ScaleY;
            dragPoint = current;
        }

        private void Photo_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mousePos = e.GetPosition(sender as Image);

            var scale = scaleTransform.ScaleX * (e.Delta > 0 ? 1.2 : 1 / 1.2);
            scale = Math.Max(scale, 1);

            scaleTransform.ScaleX = scaleTransform.ScaleY = scale;

            if (scale == 1)
            {
                translateTransform.X = translateTransform.Y = 0;
            }
            else
            {
                var newPos = e.GetPosition(sender as Image);

                translateTransform.X += (newPos.X - mousePos.X);
                translateTransform.Y += (newPos.Y - mousePos.Y);
            }
        }

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(ImageSource),
            typeof(ImageViewer),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnSourceChanged),
                null
                )
            , null);

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageViewer).Photo.Source = e.NewValue as ImageSource;
        }
    }
}
