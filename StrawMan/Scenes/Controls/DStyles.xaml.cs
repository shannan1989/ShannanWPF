using System.Windows;

namespace Shannan.StrawMan
{
    public partial class DStyles : SNDialog
    {
        public DStyles()
        {
            InitializeComponent();

            Width = SystemParameters.WorkArea.Width * 0.9;
            Height = SystemParameters.WorkArea.Height * 0.9;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
