using System.Timers;
using System.Windows;
using WallpaperTimeSheet.Data;

namespace WallpaperTimeSheet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private NotifyIcon? _notifyIcon;
        private TrayWindow? _trayWindow;
        private System.Timers.Timer _timer;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CreateNotifyIcon();
            CreateTrayWindow();
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            ScheduleTask();
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

        private void CreateNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("Icons/ic_fluent_briefcase_24_filled.ico"),
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
            DateTime now = DateTime.Now;
            DateTime nextRun = new DateTime(now.Year, now.Month, now.Day, now.Hour, 1, 0).AddHours(now.Minute >= 1 ? 1 : 0);
            double intervalToNextRun = (nextRun - now).TotalMilliseconds;

            _timer = new System.Timers.Timer(intervalToNextRun);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = false;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TrayWindow.UpdateWallpaper();

            _timer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            _timer.Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            WorkLogData.PurgeWorkLogAfterHour(DateTime.Now);
            WorkLogData.UpsertWorkLogToDb(null, DateTime.Now, true);
            
            _notifyIcon?.Dispose();
            _trayWindow?.Close();
            _timer?.Dispose();

            base.OnExit(e);
        }
    }

}
