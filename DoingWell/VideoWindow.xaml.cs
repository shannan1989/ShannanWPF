using System;
using System.Windows;

namespace Shannan.DoingWell
{
    public partial class VideoWindow : SNWindow
    {
        public VideoWindow()
        {
            InitializeComponent();

            Loaded += VideoWindow_Loaded;
        }

        private void VideoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mediaElement.Source = new Uri("http://v.leleketang.com/dat/ps/la/k/video/25055.mp4");
        }
    }
}
