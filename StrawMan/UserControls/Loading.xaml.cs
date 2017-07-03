using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Shannan.StrawMan.UserControls
{
    public partial class Loading : UserControl
    {
        public enum LoadingType
        {
            Small,
            Large
        }

        public Loading()
        {
            InitializeComponent();
        }

        public LoadingType Type
        {
            get { return (LoadingType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(LoadingType), typeof(Loading), new PropertyMetadata(LoadingType.Small, TypeChanged));

        private static void TypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((LoadingType)e.NewValue == LoadingType.Large)
            {
                (d as Loading).BackgroundImage.Source = new BitmapImage(new Uri("/UserControls/Images/loading_background_large.png", UriKind.RelativeOrAbsolute));
                (d as Loading).Ring.Source = new BitmapImage(new Uri("/UserControls/Images/loading_ring_large.png", UriKind.RelativeOrAbsolute));
            }
        }
    }
}
