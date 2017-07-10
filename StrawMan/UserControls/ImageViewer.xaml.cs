using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Shannan.StrawMan.UserControls
{
    public partial class ImageViewer : UserControl
    {
        public ImageViewer()
        {
            InitializeComponent();

            DownloadProgress.Visibility = ShowProgress ? Visibility.Visible : Visibility.Collapsed;
        }

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(ImageViewer), new PropertyMetadata(null, SourceChanged));

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageViewer).LoadSource();
        }

        public int Rotation
        {
            get { return (int)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register("Rotation", typeof(int), typeof(ImageViewer), new PropertyMetadata(0, RotationChanged));

        private static void RotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageViewer).Rotate();
        }

        public bool ShowProgress
        {
            get { return (bool)GetValue(ShowProgressProperty); }
            set { SetValue(ShowProgressProperty, value); }
        }

        public static readonly DependencyProperty ShowProgressProperty = DependencyProperty.Register("ShowProgress", typeof(bool), typeof(ImageViewer), new PropertyMetadata(false));

        private void LoadSource()
        {
            BitmapImage bmp = new BitmapImage();
            try
            {
                bmp.BeginInit();
                bmp.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                bmp.UriSource = new Uri(Source);
                bmp.EndInit();
            }
            catch (Exception)
            {
                DisplayFailed();
                return;
            }

            ImageBehavior.SetAnimatedSource(DisplayImage, bmp);

            if (bmp.Height > 1)
            {
                if ((Height != double.NaN && Height < bmp.Height) || (Width != double.NaN && Width < bmp.Width))
                {
                    DisplayImage.Stretch = Stretch.Uniform;
                }
                else if ((ActualHeight != DesiredSize.Height && ActualHeight < bmp.Height) || (ActualWidth != DesiredSize.Width && ActualWidth < bmp.Width))
                {
                    DisplayImage.Stretch = Stretch.Uniform;
                }
                else
                {
                    DisplayImage.Stretch = Stretch.None;
                }

                DownloadProgress.Visibility = Loading.Visibility = Visibility.Collapsed;
            }

            bmp.DownloadProgress += (sender, e) =>
            {
                DownloadProgress.Text = e.Progress + "%";
            };
            bmp.DownloadCompleted += delegate
            {
                if ((Height != double.NaN && Height < bmp.Height) || (Width != double.NaN && Width < bmp.Width))
                {
                    DisplayImage.Stretch = Stretch.Uniform;
                }
                else if ((ActualHeight != DesiredSize.Height && ActualHeight < bmp.Height) || (ActualWidth != DesiredSize.Width && ActualWidth < bmp.Width))
                {
                    DisplayImage.Stretch = Stretch.Uniform;
                }
                else
                {
                    DisplayImage.Stretch = Stretch.None;
                }

                DownloadProgress.Visibility = Loading.Visibility = Visibility.Collapsed;
            };
            bmp.DownloadFailed += delegate
            {
                DisplayFailed();
            };
            bmp.DecodeFailed += delegate
            {
                DisplayFailed();
            };
        }

        private void DisplayFailed()
        {
            DisplayImage.Source = new BitmapImage(new Uri("pack://application:,,,/UserControls/Images/image_fail.png"));
            DisplayImage.Stretch = Stretch.None;
            DisplayImage.UpdateLayout();
            DownloadProgress.Visibility = Loading.Visibility = Visibility.Collapsed;
        }

        private void Rotate()
        {
            double scale = 0;
            if (Rotation % 180 == 0)
            {
                scale = 1;
            }
            else
            {
                if (DisplayImage.ActualWidth < DisplayImage.ActualHeight)
                {
                    scale = ActualWidth / DisplayImage.ActualHeight;
                }
                else
                {
                    if (ActualWidth / ActualHeight > DisplayImage.ActualWidth / DisplayImage.ActualHeight || ActualWidth / ActualHeight > DisplayImage.ActualHeight / DisplayImage.ActualWidth)
                    {
                        scale = ActualHeight / DisplayImage.ActualWidth;
                    }
                    else
                    {
                        scale = ActualWidth / DisplayImage.ActualHeight;
                    }
                }
            }
            ImageScale.ScaleX = ImageScale.ScaleY = Math.Min(scale, 1);
            ImageRotate.Angle = Rotation;
        }
    }
}
