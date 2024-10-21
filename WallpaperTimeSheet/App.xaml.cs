using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            CreateNotifyIcon();
            CreateTrayWindow();
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        private void CreateTrayWindow()
        {
            _trayWindow = new TrayWindow(WorkTaskData.GetAllWorkTasks());
            _trayWindow.Loaded += (s, e) =>
            {
                var screen = Screen.PrimaryScreen.WorkingArea;
                var mousePosition = Control.MousePosition;
                _trayWindow.Left = mousePosition.X - (_trayWindow.Width / 2);
                _trayWindow.Top = screen.Bottom - _trayWindow.Height;
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

        private void Exit(object sender, EventArgs e, NotifyIcon? _notifyIcon)
        {
            _notifyIcon?.Dispose();
            _trayWindow?.Close();
            Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            WorkLogData.UpsertWorkLogToDb(null, DateTime.Now);
            base.OnExit(e);
        }
    }

}
