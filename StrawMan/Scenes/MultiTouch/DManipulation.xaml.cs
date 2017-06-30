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
                touchPad.ManipulationInertiaStarting += TouchPad_ManipulationInertiaStarting;
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

        private void TouchPad_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            //移动过程中如果将手指移开屏幕则控件会立刻停止，根据这种情况 WPF 提供另外一种惯性效果（Inertia）。
            //通过它可以使UI 单元移动的更加符合物理特性、更为实际和流畅
            //如下代码分别对TranslationBehavior、ExpansionBehavior、RotationBehavior 进行设置，使其具备惯性特征。

            e.TranslationBehavior = new InertiaTranslationBehavior();
            e.TranslationBehavior.InitialVelocity = e.InitialVelocities.LinearVelocity;
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            e.ExpansionBehavior = new InertiaExpansionBehavior();
            e.ExpansionBehavior.InitialVelocity = e.InitialVelocities.ExpansionVelocity;
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / 1000.0 * 1000.0;

            e.RotationBehavior = new InertiaRotationBehavior();
            e.RotationBehavior.InitialVelocity = e.InitialVelocities.AngularVelocity;
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);
        }
    }
}
