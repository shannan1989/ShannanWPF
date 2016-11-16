using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Shannan.DoingWell.Controls
{
    [TemplateVisualState(Name = "Large", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Small", GroupName = "SizeStates")]
    [TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
    [TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
    public class ProgressRing : Control
    {
        private List<Action> _deferredActions = new List<Action>();

        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));

            VisibilityProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(
                                                                new PropertyChangedCallback(
                                                                    (ringObject, e) =>
                                                                    {
                                                                        if (e.NewValue != e.OldValue)
                                                                        {
                                                                            ProgressRing ring = ringObject as ProgressRing;
                                                                            //auto set IsActive to false if we're hiding it.
                                                                            if ((Visibility)e.NewValue != Visibility.Visible)
                                                                            {
                                                                                //sets the value without overriding it's binding (if any).
                                                                                ring.SetCurrentValue(ProgressRing.IsActiveProperty, false);
                                                                            }
                                                                            else
                                                                            {
                                                                                // don't forget to re-activate
                                                                                ring.IsActive = true;
                                                                            }
                                                                        }
                                                                    }
                                                                )));
        }

        public ProgressRing()
        {
            SizeChanged += ProgressRing_SizeChanged;
        }

        private void ProgressRing_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BindableWidth = ActualWidth;
        }

        public override void OnApplyTemplate()
        {
            //make sure the states get updated
            UpdateLargeState();
            UpdateActiveState();
            base.OnApplyTemplate();

            if (_deferredActions != null)
            {
                foreach (Action action in _deferredActions)
                {
                    action();
                }
            }
            _deferredActions = null;
        }

        #region dependency properties

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, IsActiveChanged));

        private static void IsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProgressRing ring = d as ProgressRing;
            if (ring == null)
            {
                return;
            }
            ring.UpdateActiveState();
        }

        public bool IsLarge
        {
            get { return (bool)GetValue(IsLargeProperty); }
            set { SetValue(IsLargeProperty, value); }
        }

        public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register("IsLarge", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, IsLargeChanged));

        private static void IsLargeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProgressRing ring = d as ProgressRing;
            if (ring == null)
            {
                return;
            }
            ring.UpdateLargeState();
        }

        public double BindableWidth
        {
            get { return (double)GetValue(BindableWidthProperty); }
            private set { SetValue(BindableWidthProperty, value); }
        }

        public static readonly DependencyProperty BindableWidthProperty = DependencyProperty.Register("BindableWidth", typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double), BindableWidthChanged));

        private static void BindableWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProgressRing ring = d as ProgressRing;
            if (ring == null)
            {
                return;
            }

            Action action = new Action(() =>
            {
                ring.SetEllipseDiameter((double)e.NewValue);
                ring.SetEllipseOffset((double)e.NewValue);
                ring.SetMaxSideLength((double)e.NewValue);
            });
            if (ring._deferredActions != null)
            {
                ring._deferredActions.Add(action);
            }
            else
            {
                action();
            }
        }

        public double MaxSideLength
        {
            get { return (double)GetValue(MaxSideLengthProperty); }
            private set { SetValue(MaxSideLengthProperty, value); }
        }

        public static readonly DependencyProperty MaxSideLengthProperty = DependencyProperty.Register("MaxSideLength", typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double)));

        public double EllipseDiameter
        {
            get { return (double)GetValue(EllipseDiameterProperty); }
            private set { SetValue(EllipseDiameterProperty, value); }
        }

        public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(ProgressRing), new PropertyMetadata(default(double)));

        public double EllipseDiameterScale
        {
            get { return (double)GetValue(EllipseDiameterScaleProperty); }
            set { SetValue(EllipseDiameterScaleProperty, value); }
        }

        public static readonly DependencyProperty EllipseDiameterScaleProperty = DependencyProperty.Register("EllipseDiameterScale", typeof(double), typeof(ProgressRing), new PropertyMetadata(1D));

        public Thickness EllipseOffset
        {
            get { return (Thickness)GetValue(EllipseOffsetProperty); }
            private set { SetValue(EllipseOffsetProperty, value); }
        }

        public static readonly DependencyProperty EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(ProgressRing), new PropertyMetadata(default(Thickness)));

        #endregion dependency properties

        #region private methods

        private void UpdateActiveState()
        {
            Action action;
            if (IsActive)
            {
                action = () => VisualStateManager.GoToState(this, "Active", true);
            }
            else
            {
                action = () => VisualStateManager.GoToState(this, "Inactive", true);
            }

            if (_deferredActions != null)
            {
                _deferredActions.Add(action);
            }
            else
            {
                action();
            }
        }

        private void UpdateLargeState()
        {
            Action action;
            if (IsLarge)
            {
                action = () => VisualStateManager.GoToState(this, "Large", true);
            }
            else
            {
                action = () => VisualStateManager.GoToState(this, "Small", true);
            }

            if (_deferredActions != null)
            {
                _deferredActions.Add(action);
            }
            else
            {
                action();
            }
        }

        private void SetMaxSideLength(double width)
        {
            MaxSideLength = Math.Max(width, 20);
        }

        private void SetEllipseDiameter(double width)
        {
            EllipseDiameter = (width / 8) * EllipseDiameterScale;
        }

        private void SetEllipseOffset(double width)
        {
            EllipseOffset = new Thickness(0, width / 2, 0, 0);
        }

        #endregion private methods
    }
}
