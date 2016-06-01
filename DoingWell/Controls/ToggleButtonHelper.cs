using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Shannan.DoingWell.Controls
{
    class ToggleButtonHelper : DependencyObject
    {

        public static FlowDirection GetContentDirection(DependencyObject obj)
        {
            return (FlowDirection)obj.GetValue(ContentDirectionProperty);
        }

        public static void SetContentDirection(DependencyObject obj, FlowDirection value)
        {
            obj.SetValue(ContentDirectionProperty, value);
        }

        public static readonly DependencyProperty ContentDirectionProperty =
            DependencyProperty.RegisterAttached("ContentDirection", typeof(FlowDirection), typeof(ToggleButtonHelper), new FrameworkPropertyMetadata(FlowDirection.LeftToRight, ContentDirectionPropertyChanged));

        private static void ContentDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tb = d as ToggleButton;
            if (null == tb)
            {
                throw new InvalidOperationException("The property 'ContentDirection' may only be set on ToggleButton elements.");
            }
        }

    }
}
