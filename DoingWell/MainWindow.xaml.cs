using System;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace Shannan.DoingWell
{
    public partial class MainWindow : SNWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string ov = "操作系统版本：（" + Environment.OSVersion + "）" + Environment.NewLine;
            ov += "Platform：" + Environment.OSVersion.Platform + Environment.NewLine;
            ov += "Version：" + Environment.OSVersion.Version + Environment.NewLine;
            ov += "VersionString：" + Environment.OSVersion.VersionString + Environment.NewLine;
            ov += "ServicePack：" + Environment.OSVersion.ServicePack + Environment.NewLine;
            if (Environment.Is64BitOperatingSystem)
                ov += "64 Bit Operating System" + Environment.NewLine;
            else
                ov += "32 Bit Operating System" + Environment.NewLine;
            OSVersionInfo.Text = ov;

            string version = ".Net Framework版本号：（" + Environment.Version + "）" + Environment.NewLine;
            version += "Major：" + Environment.Version.Major + Environment.NewLine;
            version += "MajorRevision：" + Environment.Version.MajorRevision + Environment.NewLine;
            version += "Minor：" + Environment.Version.Minor + Environment.NewLine;
            version += "MinorRevision：" + Environment.Version.MinorRevision + Environment.NewLine;
            version += "Build：" + Environment.Version.Build + Environment.NewLine;
            version += "Revision：" + Environment.Version.Revision + Environment.NewLine;
            VersionInfo.Text = version;

            ResolutionInfo.Text = "Resolution：" + string.Format("{0}*{1}", SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight) + Environment.NewLine;

            WorkAreaInfo.Text = "WorkArea：" + string.Format("{0}*{1}", SystemParameters.WorkArea.Width, SystemParameters.WorkArea.Height) + Environment.NewLine;

            SystemDirectoryInfo.Text = "SystemDirectory：" + Environment.SystemDirectory + Environment.NewLine;

            CurrentDirectoryInfo.Text = "应用程序当前目录：" + Environment.CurrentDirectory + Environment.NewLine;

            string drivesInfo = "Drives：" + Environment.NewLine;
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                drivesInfo += drive + Environment.NewLine;
            }
            DrivesInfo.Text = drivesInfo;

            string networks = "Networks：" + Environment.NewLine;
            string currentNetwork = string.Empty;
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                networks += adapter.Name + "----" + adapter.Id + "----" + adapter.Description + "----" + adapter.Speed + Environment.NewLine;
                if (currentNetwork == string.Empty && adapter.OperationalStatus == OperationalStatus.Up)
                {
                    currentNetwork = adapter.Name;
                }
            }
            NetworksInfo.Text = networks;
            CurrentNetworksInfo.Text = "CurrentNetwork：" + currentNetwork + Environment.NewLine;

            MachineNameInfo.Text = "MachineName：" + Environment.MachineName;
            UserNameInfo.Text = "UserName：" + Environment.UserName;
            UserDomainNameInfo.Text = "UserDomainName：" + Environment.UserDomainName;
            ProcessorCountInfo.Text = "ProcessorCount：" + Environment.ProcessorCount;
            TickCountInfo.Text = "TickCount：" + Environment.TickCount;

            DesktopInfo.Text = "Desktop：" + Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //Environment.GetFolderPath(Environment.SpecialFolder对象);
            //Environment.SpecialFolder对象提供系统保留文件夹标识，例如: Environment.SpecialFolder.Desktop表示桌面文件夹的路径。
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            switch (tag)
            {
                case "VideoPlayer":
                    Window videoWindow = new VideoWindow();
                    videoWindow.ShowDialog();
                    break;
                case "DrawingBoard":
                    Window drawingWindow = new DrawingWindow();
                    drawingWindow.ShowDialog();
                    break;
                case "Controls":
                    Window controlsWindow = new ControlsWindow();
                    controlsWindow.ShowDialog();
                    break;
                default:
                    break;
            }
        }
    }
}
