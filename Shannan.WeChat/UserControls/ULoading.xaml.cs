using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Shannan.WeChat.UserControls
{
    public enum LoadingType
    {
        Small = 0,
        Large = 1
    }

    public partial class ULoading : UserControl
    {
        public ULoading()
        {
            InitializeComponent();
        }

        public LoadingType Type
        {
            get { return (LoadingType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(LoadingType), typeof(ULoading), new PropertyMetadata(LoadingType.Large, TypeChanged));

        private static void TypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((LoadingType)e.NewValue == LoadingType.Large)
            {
                (d as ULoading).BackgroundImage.Source = new BitmapImage(new Uri("/Images/UserControls/loading_background_large.png", UriKind.RelativeOrAbsolute));
                (d as ULoading).Ring.Source = new BitmapImage(new Uri("/Images/UserControls/loading_ring_large.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                (d as ULoading).BackgroundImage.Source = new BitmapImage(new Uri("/Images/UserControls/loading_background.png", UriKind.RelativeOrAbsolute));
                (d as ULoading).Ring.Source = new BitmapImage(new Uri("/Images/UserControls/loading_ring.png", UriKind.RelativeOrAbsolute));
            }
        }
    }
}
