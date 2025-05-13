using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using WallpaperTimeSheet.Classes;
using WallpaperTimeSheet.Data;
using WallpaperTimeSheet.Models;
using WallpaperTimeSheet.Utills;

namespace WallpaperTimeSheet
{
    public partial class StartupPromptWindow : Window
    {
        public string SelectedActivity { get; private set; }

        private DispatcherTimer _topmostTimer;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        private List<WorkTask> WorkTasks { get; set; }
        public static WorkTask? SelectedWorkTask { get; set; }

        private const int SW_RESTORE = 9;
        public StartupPromptWindow(List<WorkTask> workTasks)
        {
            WorkTasks = workTasks;
            InitializeComponent();

            workTasks.Sort((a, b) => a.Label.CompareTo(b.Label));

            WorkTaskSelector.Items.Add(WorkTaskData.None.Label);
            foreach (WorkTask workTask in WorkTasks)
                WorkTaskSelector.Items.Add(workTask.Label);

            SelectedWorkTask = WorkLogData.GetLastWorkLog()?.WorkTask ?? WorkTaskData.None;

            WorkTaskSelector.SelectedItem = SelectedWorkTask.Label;

            Loaded += (s, e) =>
            {
                StartTopmostEnforcer();
            };
        }

        private void WorkTaskSelector_Change(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedWorkTask = WorkTasks.Find(workTask => workTask.Label == WorkTaskSelector.SelectedItem.ToString());
            WorkLogData.UpsertWorkLogToDb(SelectedWorkTask?.Id, DateTime.Now);
        }
        
        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void StartTopmostEnforcer()
        {
            _topmostTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _topmostTimer.Tick += (s, e) => ForceTopmost();
            _topmostTimer.Start();
        }

        private void ForceTopmost()
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            if (IsIconic(hwnd))
            {
                ShowWindow(hwnd, SW_RESTORE);
            }

            // SetForegroundWindow lo forza in cima
            SetForegroundWindow(hwnd);

            // Rinforza Topmost in caso venga ignorato
            this.Topmost = false;
            this.Topmost = true;
        }
    }
}
