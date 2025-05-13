using System.IO;
using System.Windows;
using WallpaperTimeSheet.Data;
using WallpaperTimeSheet.Models;

namespace WallpaperTimeSheet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static NotifyIcon? _notifyIcon;
        private static TrayWindow? _trayWindow;
        private static System.Windows.Forms.Timer? _timer;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //check if there is a worklog that is not closed and close it
            WorkLog lastWorklog = WorkLogData.GetLastWorkLog();
            if (lastWorklog.WorkTaskId != null)
            {
                string lastUpdateExecutionStr = ConfigData.GetData("LastUpdateExecution");
                if(lastUpdateExecutionStr != null)
                {
                    DateTime lastUpdateExecution = DateTime.Parse(lastUpdateExecutionStr);
                    WorkLogData.UpsertWorkLogToDb(null, lastUpdateExecution, false);
                }   
            }

            CreateNotifyIcon();
            CreateTrayWindow();
            ScheduleTask();
            OpenStartupPromptWindow();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
        }

        private void CreateTrayWindow()
        {
            _trayWindow = new TrayWindow(WorkTaskData.GetAllWorkTasks());
            _trayWindow.Loaded += (s, e) =>
            {
                var mousePosition = Control.MousePosition;

                var currentScreen = Screen.FromPoint(mousePosition);
                var workingArea = currentScreen.WorkingArea;

                float scaleFactor = currentScreen.Bounds.Width / (float)SystemParameters.PrimaryScreenWidth;

                _trayWindow.Left = (mousePosition.X / scaleFactor) - (_trayWindow.Width / 2);
                _trayWindow.Top = (workingArea.Bottom / scaleFactor) - _trayWindow.Height;

                if (_trayWindow.Left < workingArea.Left / scaleFactor)
                    _trayWindow.Left = workingArea.Left / scaleFactor;
                else if (_trayWindow.Left + _trayWindow.Width > workingArea.Right / scaleFactor)
                    _trayWindow.Left = (workingArea.Right / scaleFactor) - _trayWindow.Width;

                _trayWindow.Show();
            };

            _trayWindow.Deactivated += (s, e) => _trayWindow.Hide();
        }

        public static void CahngeNoriIcon(string newIconPath)
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Icon = new System.Drawing.Icon(newIconPath);
            }
        }

        private void CreateNotifyIcon()
        {
            string currentDirectory = System.Reflection.Assembly.GetEntryAssembly().Location;
            currentDirectory = Path.GetDirectoryName(currentDirectory);
            _notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon(Path.Combine(currentDirectory, "Icons/ic_fluent_briefcase_off_24_filled.ico")),
                Visible = true
            };
            _notifyIcon.Click += NotifyIcon_Click;
        }

        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            if (_trayWindow?.IsVisible == true)
            {
                _trayWindow.Hide();
            }
            else
            {
                _trayWindow?.Show();
                _trayWindow?.Activate();
            }
        }

        private NotifyIcon? Get_notifyIcon()
        {
            return _notifyIcon;
        }

        private void ScheduleTask()
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += new EventHandler(Timer_Elapsed);
            _timer.Interval = 1000 * 60 * 15;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            ConfigData.UpsertData("LastUpdateExecution", DateTime.Now);
            TrayWindow.UpdateWallpaper();
        }

        private void OnProcessExit(object? sender, EventArgs e)
        {
            setWorkLogToNone();
        }

        private void setWorkLogToNone()
        {
            ConfigData.UpsertData("LastUpdateExecution", DateTime.Now);

            WorkLogData.PurgeWorkLogAfterHour(DateTime.Now);
            WorkLogData.UpsertWorkLogToDb(null, DateTime.Now, true);

            _notifyIcon?.Dispose();
            _timer?.Dispose();
        }

        private void OpenStartupPromptWindow()
        {
            var overlays = Screen.AllScreens.Select(screen =>
            {
                var bounds = screen.Bounds;
                var rect = new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                var overlay = new OverlayWindow(rect);
                overlay.Show();
                return overlay;
            }).ToList();

            var prompt = new StartupPromptWindow(WorkTaskData.GetAllWorkTasks());
            prompt.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            prompt.Topmost = true;

            // forza il focus e la visibilità
            prompt.Loaded += (s, ev) =>
            {
                prompt.Activate(); // forza focus
                prompt.Topmost = true; // ribadisce Topmost
            };

            prompt.ShowDialog();

            // Chiudi tutti gli overlay
            foreach (var overlay in overlays)
                overlay.Close();
            WorkTask? SelectedWorkTask = WorkLogData.GetLastWorkLog()?.WorkTask ?? WorkTaskData.None;
            _trayWindow.WorkTaskSelector.SelectedItem = SelectedWorkTask.Label;
        }
    }
}
