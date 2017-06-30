using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Shannan.StrawMan
{
    public partial class DMultiTouch : SNDialog
    {
        public DMultiTouch()
        {
            InitializeComponent();

            Width = SystemParameters.WorkArea.Width * 0.9;
            Height = SystemParameters.WorkArea.Height * 0.9;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Loaded += delegate
            {
                touchPad.TouchDown += TouchPad_TouchDown;
                touchPad.TouchUp += TouchPad_TouchUp;
                touchPad.TouchMove += TouchPad_TouchMove;
            };
        }

        private Dictionary<int, Ellipse> movingEllipses = new Dictionary<int, Ellipse>();
        private Random rd = new Random();

        private void TouchPad_TouchDown(object sender, TouchEventArgs e)
        {
            //TouchDown 事件主要是完成当触碰产生时在<Canvas>控件中生成彩色圆圈的任务
            //使用Ellipse创建随机颜色的圆圈，通过GetTouchPoint方法获取触碰位置点，并调整圆圈在<Canvas>中的位置。
            //为了跟踪手指移动轨迹，需要将触屏设备ID及UI控件存储在集合movingEllipses 中。
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 60;
            ellipse.Height = 60;
            ellipse.Stroke = Brushes.White;
            ellipse.Fill = new SolidColorBrush(Color.FromRgb((byte)rd.Next(0, 255), (byte)rd.Next(0, 255), (byte)rd.Next(0, 255)));

            TouchPoint touchPoint = e.GetTouchPoint(touchPad);
            Canvas.SetTop(ellipse, touchPoint.Bounds.Top);
            Canvas.SetLeft(ellipse, touchPoint.Bounds.Left);

            touchPad.Children.Add(ellipse);

            movingEllipses[e.TouchDevice.Id] = ellipse;
        }

        private void TouchPad_TouchUp(object sender, TouchEventArgs e)
        {
            //当手指离开触屏时TouchUp事件将被触发，首先从<Canvas>中将彩色圆圈移除,再将触碰设备从movingEllipses集合中删除不再跟踪手指相关操作。
            Ellipse ellipse = movingEllipses[e.TouchDevice.Id];
            touchPad.Children.Remove(ellipse);
            movingEllipses.Remove(e.TouchDevice.Id);
        }

        private void TouchPad_TouchMove(object sender, TouchEventArgs e)
        {
            //当手指在触屏上持续移动时TouchMove事件触发，它来跟踪手指移动轨迹，并重新调整圆圈在<Canvas>中的位置。
            Ellipse ellipse = movingEllipses[e.TouchDevice.Id];
            TouchPoint touchPoint = e.GetTouchPoint(touchPad);
            Canvas.SetTop(ellipse, touchPoint.Bounds.Top);
            Canvas.SetLeft(ellipse, touchPoint.Bounds.Left);
        }
    }
}
