using Shannan.StrawMan.UserControls;
using System;
using System.Windows;

namespace Shannan.StrawMan
{
    public partial class DUserControls : SNDialog
    {
        public DUserControls()
        {
            InitializeComponent();

            Width = SystemParameters.WorkArea.Width * 0.9;
            Height = SystemParameters.WorkArea.Height * 0.9;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Loaded += delegate
            {
                pager.CurrentPageChanged += Pager_CurrentPageChanged;
                quantitySelector.QuantityChanged += QuantitySelector_QuantityChanged;
            };
        }

        private void QuantitySelector_QuantityChanged(object sender, EventArgs e)
        {
            MessageBox.Show((sender as QuantitySelector).Quantity.ToString());
        }

        private void Pager_CurrentPageChanged(object sender, EventArgs e)
        {
            MessageBox.Show((sender as Pager).CurrentPage.ToString());
        }
    }
}
