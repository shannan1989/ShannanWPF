using Awesomium.Core;
using System;
using System.Windows;

namespace Shannan.DoingWell
{
    public partial class BrowserWindow : SNWindow
    {
        public BrowserWindow()
        {
            InitializeComponent();

            Loaded += BrowserWindow_Loaded;
            ContentRendered += BrowserWindow_ContentRendered;
        }

        private void BrowserWindow_Loaded(object sender, RoutedEventArgs e)
        {
            webControl.DocumentReady += WebControl_DocumentReady;
            webControl.ShowCreatedWebView += WebControl_ShowCreatedWebView;
            webControl.TitleChanged += WebControl_TitleChanged;
            webControl.AddressChanged += WebControl_AddressChanged;
            webControl.ConsoleMessage += WebControl_ConsoleMessage;
            webControl.ShowContextMenu += WebControl_ShowContextMenu;
            webControl.NativeViewInitialized += WebControl_NativeViewInitialized;
            webControl.WindowClose += WebControl_WindowClose;
            webControl.JavascriptMessage += WebControl_JavascriptMessage;
        }

        private void BrowserWindow_ContentRendered(object sender, EventArgs e)
        {
        }

        private void WebControl_DocumentReady(object sender, DocumentReadyEventArgs e)
        {
        }

        private void WebControl_ShowCreatedWebView(object sender, ShowCreatedWebViewEventArgs e)
        {
            webControl.Source = new Uri(e.TargetURL.AbsoluteUri);
        }

        private void WebControl_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
        }

        private void WebControl_AddressChanged(object sender, UrlEventArgs e)
        {
        }

        private void WebControl_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            Title = e.Title;
        }

        private void WebControl_WindowClose(object sender, WindowCloseEventArgs e)
        {
        }

        private void WebControl_NativeViewInitialized(object sender, WebViewEventArgs e)
        {
        }

        private void WebControl_ShowContextMenu(object sender, ContextMenuEventArgs e)
        {
        }

        private void WebControl_JavascriptMessage(object sender, JavascriptMessageEventArgs e)
        {
            //<script type="text/javascript">
            //      function someFuction(sender, args) {
            //            var iValue = { someProperty: 42 };
            //            var res = OSMJIF.sendMessage('someMessage', iValue);
            //            alert(res);
            //      }
            //</script>
            //网页js需判断OSMJIF是否为undefined
            MessageBox.Show(e.Message);
            e.Result = "Processed";
        }
    }
}
