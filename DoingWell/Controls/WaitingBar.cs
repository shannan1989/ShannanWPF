using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Shannan.DoingWell.Controls
{
    public class WaitingBar : ProgressBar
    {
        private LinkedList<DateTime> _thread;
        private ProgressBar _bar;
        private System.Timers.Timer _timer;

        public WaitingBar() : base()
        {
            _thread = new LinkedList<DateTime>();
            _bar = new ProgressBar();
            _timer = new System.Timers.Timer(100);
            _timer.AutoReset = true;
            _timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                if (_bar.Visibility == Visibility.Hidden)
                {
                    if (_thread.First != null && (_thread.First.Value - DateTime.Now).TotalMilliseconds > 300)
                    {
                        _bar.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (_thread.Last == null || (_thread.Last.Value - DateTime.Now).TotalSeconds > 30)
                    {
                        _bar.Visibility = Visibility.Hidden;
                        _thread.Clear();
                    }
                }
            };
        }

        public void Start()
        {
            _thread.AddLast(DateTime.Now);
        }

        public void Finished()
        {
            _thread.RemoveFirst();
        }

    }
}
