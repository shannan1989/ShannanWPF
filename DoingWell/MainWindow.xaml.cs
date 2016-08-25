using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Shannan.DoingWell
{
    public partial class MainWindow : SNWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Tools.CursorGenerator.Create(new Uri("pack://application:,,,/Images/eraser.ico"), 0, 8);

            StringBuilder info = new StringBuilder();

            info.AppendLine("MachineName：" + Environment.MachineName);
            info.AppendLine("UserName：" + Environment.UserName);
            info.AppendLine("UserDomainName：" + Environment.UserDomainName);
            info.AppendLine();

            info.AppendLine("操作系统版本：（" + Environment.OSVersion + "）");
            info.AppendLine("Platform：" + Environment.OSVersion.Platform);
            info.AppendLine("Version：" + Environment.OSVersion.Version);
            info.AppendLine("VersionString：" + Environment.OSVersion.VersionString);
            info.AppendLine("ServicePack：" + Environment.OSVersion.ServicePack);
            info.AppendLine();

            if (Environment.Is64BitOperatingSystem)
                info.AppendLine("64 Bit Operating System");
            else
                info.AppendLine("32 Bit Operating System");
            info.AppendLine();

            info.AppendLine(".Net Framework版本号：（" + Environment.Version + "）");
            info.AppendLine("Major：" + Environment.Version.Major);
            info.AppendLine("MajorRevision：" + Environment.Version.MajorRevision);
            info.AppendLine("Minor：" + Environment.Version.Minor);
            info.AppendLine("MinorRevision：" + Environment.Version.MinorRevision);
            info.AppendLine("Build：" + Environment.Version.Build);
            info.AppendLine("Revision：" + Environment.Version.Revision);
            info.AppendLine();

            info.AppendLine("ProcessorCount：" + Environment.ProcessorCount);
            info.AppendLine("TickCount：" + Environment.TickCount);
            info.AppendLine("SystemPageSize：" + Environment.SystemPageSize + " bytes");
            info.AppendLine("SystemDirectory：" + Environment.SystemDirectory);
            info.AppendLine();

            info.AppendLine("Resolution：" + string.Format("{0}*{1}", SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight));
            info.AppendLine("WorkArea：" + string.Format("{0}*{1}", SystemParameters.WorkArea.Width, SystemParameters.WorkArea.Height));
            info.AppendLine();

            info.AppendLine("Drives：");
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                info.AppendLine(drive);
            }
            info.AppendLine();

            info.AppendLine("Networks：");
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                info.AppendLine(adapter.Id + "     " + adapter.Name + "     " + adapter.Description + "     " + adapter.Speed + "     " + (adapter.OperationalStatus == OperationalStatus.Up ? "Active" : "Inactive"));
            }
            info.AppendLine();

            if (NetworkInterface.GetIsNetworkAvailable())
                info.AppendLine("Network is available");
            else
                info.AppendLine("Network is not available");
            info.AppendLine();

            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer) + "-----MyComputer");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "-----UserProfile");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "-----Desktop");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "-----DesktopDirectory");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "-----MyDocuments");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.Favorites) + "-----Favorites");
            info.AppendLine();

            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "-----ApplicationData");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.Recent) + "-----Recent");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.NetworkShortcuts) + "-----NetworkShortcuts");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "-----StartMenu");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.Programs) + "-----Programs");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "-----Startup");
            info.AppendLine();

            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "-----Cookies");
            info.AppendLine(Environment.GetFolderPath(Environment.SpecialFolder.History) + "-----History");
            info.AppendLine();

            info.AppendLine("ProgramFilesX86：" + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
            info.AppendLine("ProgramFiles：" + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            info.AppendLine("Windows：" + Environment.GetFolderPath(Environment.SpecialFolder.Windows));
            info.AppendLine("System：" + Environment.GetFolderPath(Environment.SpecialFolder.System));
            info.AppendLine("SystemX86：" + Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
            info.AppendLine("Fonts：" + Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
            info.AppendLine();
            //Environment.GetFolderPath(Environment.SpecialFolder对象);
            //Environment.SpecialFolder对象提供系统保留文件夹标识，例如: Environment.SpecialFolder.Desktop表示桌面文件夹的路径

            info.AppendLine("应用程序当前目录：" + Environment.CurrentDirectory);
            info.AppendLine();

            Info.Text = info.ToString();
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            switch (tag)
            {
                case "Browser":
                    Window browserWindow = new BrowserWindow();
                    browserWindow.ShowDialog();
                    break;
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
                case "Shapes":
                    Window shapesWindow = new ShapesWindow();
                    shapesWindow.ShowDialog();
                    break;
                default:
                    break;
            }
        }
    }
}
