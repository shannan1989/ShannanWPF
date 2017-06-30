using System;
using System.Windows;
using System.Windows.Controls;

namespace Shannan.StrawMan.UserControls
{
    public partial class QuantitySelector : UserControl
    {
        #region Public Events

        public event EventHandler QuantityChanged;

        #endregion Public Events

        public QuantitySelector()
        {
            InitializeComponent();

            Loaded += delegate
            {
                RefreshLayout();
            };

            PART_IncrementButton.Click += delegate
            {
                Quantity++;
                RefreshLayout();
                RaiseEvent();
            };
            PART_DecrementButton.Click += delegate
            {
                Quantity--;
                RefreshLayout();
                RaiseEvent();
            };
        }

        #region Public Properties

        public int Quantity
        {
            get { return (int)GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }

        public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register("Quantity", typeof(int), typeof(QuantitySelector), new PropertyMetadata(0));

        #endregion Public Properties

        private void RefreshLayout()
        {
            PART_DecrementButton.IsEnabled = Quantity > 0;
        }

        private void RaiseEvent()
        {
            QuantityChanged?.Invoke(this, new EventArgs());
        }
    }
}
