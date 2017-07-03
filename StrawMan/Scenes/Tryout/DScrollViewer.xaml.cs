using System.Windows;
using System.Windows.Controls;

namespace Shannan.StrawMan
{
    public partial class DScrollViewer : SNDialog
    {
        public DScrollViewer()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Loaded += delegate
            {
                scroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;

                //ScrollViewer滚动时默认以像素为单位，滚动起来看是连贯的。
                //如果以被滚动内容为单元，可以设置ScrollViewer的CanContentScroll属性为true，滚动时会对齐内容的开始位置，看上去滚动是跳跃的。
                scroller.CanContentScroll = true;

                //ScrollViewer直接支持Panning（平面拖动）操作，只要设置PanningMode属性即可。
                scroller.PanningMode = PanningMode.HorizontalFirst;

                //拖动ScrollViewer默认是有惯性的，ScrollViewer的PanningDeceleration属性是用于设置惯性运动的减速率的，默认值0.001，表现为很自然的惯性运动。
                scroller.PanningDeceleration = 0.001;

                scroller.PanningRatio = 0.5;

                //ScrollViewer中的内容滚动到边界时还会自动触发 BoundaryFeedback ，这会表现为Window窗口弹了一下，即 Window Bounce（窗体弹跳）。
                scroller.ManipulationBoundaryFeedback += (sender, e) =>
                {
                    //注释掉这行，即可看到Window Bounce（窗体弹跳）
                    e.Handled = true;
                };
            };
        }
    }
}
