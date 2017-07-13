namespace Shannan.StrawMan
{
    public class SNDialog : WindowBase
    {
        public SNDialog()
        {
            ShowInTaskbar = false;

            PreviewMouseDoubleClick += (sender, e) =>
            {
                e.Handled = true;
            };
        }
    }
}
