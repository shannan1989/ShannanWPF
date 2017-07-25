using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Shannan.StrawMan
{
    public partial class DBasicAnimation : SNDialog
    {
        public DBasicAnimation()
        {
            InitializeComponent();

            Width = SystemParameters.WorkArea.Width * 0.9;
            Height = SystemParameters.WorkArea.Height * 0.9;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void AnimationButton1_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation()
            {
                From = 0,                               //起始值
                To = 1,                                 //结束值
                Duration = TimeSpan.FromSeconds(3),     //动画持续时间
            };
            tbAnimation1.BeginAnimation(OpacityProperty, da);//开始动画
        }

        private void AnimationButton2_Click(object sender, RoutedEventArgs e)
        {
            //Margin属性是Thickness类型，选择ThicknessAnimation
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = new Thickness(0, 0, 0, 0);            //起始值
            ta.To = new Thickness(240, 0, 0, 0);            //结束值
            ta.Duration = TimeSpan.FromSeconds(3);          //动画持续时间
            tbAnimation2.BeginAnimation(MarginProperty, ta);//开始动画
        }

        private void KeyFramesAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimationUsingKeyFrames dakf = new DoubleAnimationUsingKeyFrames();

            dakf.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
            dakf.KeyFrames.Add(new LinearDoubleKeyFrame(240, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3))));
            dakf.KeyFrames.Add(new LinearDoubleKeyFrame(240, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(6))));
            dakf.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(9))));

            dakf.BeginTime = TimeSpan.FromSeconds(2);    //从第2秒开始动画
            dakf.RepeatBehavior = new RepeatBehavior(3); //动画重复3次

            bdKeyFrames.BeginAnimation(WidthProperty, dakf);
        }
    }
}
