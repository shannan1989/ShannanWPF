using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Shannan.DoingWell
{
    public static class AnimationExtension
    {
        public static void BeginDoubleAnimation(this UIElement self, DependencyProperty dp, double to, TimeSpan duration, EventHandler callback)
        {
            if (dp == null)
            {
                throw new ArgumentNullException("dp");
            }

            DoubleAnimation a = new DoubleAnimation((double)self.GetValue(dp), to, new Duration(duration), FillBehavior.HoldEnd);
            a.Completed += delegate
            {
                self.BeginAnimation(dp, null);
                self.SetValue(dp, to);
            };
            if (callback != null)
            {
                a.Completed += callback;
            }

            self.BeginAnimation(dp, a);
        }
    }
}
