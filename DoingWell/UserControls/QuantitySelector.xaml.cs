using System;
using System.Windows;
using System.Windows.Controls;

namespace Shannan.DoingWell.UserControls
{
    public partial class QuantitySelector : UserControl
    {
        #region Public Events
        public event EventHandler QuantityChanged;
        #endregion Public Events

        public QuantitySelector()
        {
            InitializeComponent();

            Loaded += QuantitySelector_Loaded;

            PART_IncrementButton.Click += PART_IncrementButton_Click;
            PART_DecrementButton.Click += PART_DecrementButton_Click;
        }

        private void PART_IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            Quantity++;
            RefreshLayout();
            RaiseEvent();
        }

        private void PART_DecrementButton_Click(object sender, RoutedEventArgs e)
        {
            Quantity--;
            RefreshLayout();
            RaiseEvent();
        }

        private void QuantitySelector_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshLayout();
        }

        #region Public Properties

        public int Quantity
        {
            get { return (int)GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }
        public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register("Quantity", typeof(int), typeof(QuantitySelector), new PropertyMetadata(0));

        #endregion

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
