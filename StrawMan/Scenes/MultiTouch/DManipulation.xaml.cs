using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Shannan.StrawMan
{
    public partial class DManipulation : SNDialog
    {
        public DManipulation()
        {
            InitializeComponent();

            Width = SystemParameters.WorkArea.Width * 0.9;
            Height = SystemParameters.WorkArea.Height * 0.9;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Loaded += delegate
            {
                touchPad.ManipulationStarting += TouchPad_ManipulationStarting;
                touchPad.ManipulationDelta += TouchPad_ManipulationDelta;
                touchPad.ManipulationCompleted += TouchPad_ManipulationCompleted;
            };
        }

        private void TouchPad_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            //当触碰控件时，ManipulationStarting 事件将会自动触发
            //首先需要定义ManipulationContainer（即为touchPad<Canvas>），该容器的主要用于定义参考坐标，控件的任何操作均以参考坐标为准。
            //ManipulationModes设置可以限制哪些手势操作是程序允许的，如果没有特殊情况可设置为"All"。
            e.ManipulationContainer = touchPad;
            e.Mode = ManipulationModes.All;
        }

        private void TouchPad_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // ManipulationDelta 事件会在手势操作开始时触发，并且该手势需持续进行不得间断。
            //通过 FrameworkElement 获得接受操作的控件，将控件透明度降低到0.5。
            //创建 Matrix 用于控制控件 MatrixTransform，利用Point 获得控件中心坐标点
            //通过 ScaleAt、RotateAt、Translate 执行缩放、旋转、平移操作。
            FrameworkElement element = e.Source as FrameworkElement;
            element.Opacity = 0.5;

            Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;

            Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
            center = matrix.Transform(center);

            matrix.ScaleAt(e.DeltaManipulation.Scale.X, e.DeltaManipulation.Scale.Y, center.X, center.Y);

            matrix.RotateAt(e.DeltaManipulation.Rotation, center.X, center.Y);

            matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);

            ((MatrixTransform)element.RenderTransform).Matrix = matrix;
        }

        private void TouchPad_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            //当手指离开触摸屏即操作结束，ManipulationCompleted 事件触发，将控件透明度重新调整为1
            FrameworkElement element = e.Source as FrameworkElement;
            element.Opacity = 1;
        }
    }
}
