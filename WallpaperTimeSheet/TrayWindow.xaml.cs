using System.Diagnostics;
using System.Windows;
using WallpaperTimeSheet.Data;
using WallpaperTimeSheet.Models;
using WallpaperTimeSheet.Utills;

namespace WallpaperTimeSheet
{
    public partial class TrayWindow : Window
    {
        private List<WorkTask> WorkTasks { get; set; }
        public static WorkTask? SelectedWorkTask { get; set; }

        public TrayWindow(List<WorkTask> workTasks)
        {
            WorkTasks = workTasks;
            InitializeComponent();

            workTasks.Sort((a, b) => a.Label.CompareTo(b.Label));

            WorkTaskSelector.Items.Add(WorkTaskData.None.Label);
            foreach (WorkTask workTask in WorkTasks)
                WorkTaskSelector.Items.Add(workTask.Label);

            SelectedWorkTask = WorkLogData.GetLastWorkLog()?.WorkTask ?? WorkTaskData.None;

            WorkTaskSelector.SelectedItem = SelectedWorkTask.Label;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void WorkTaskSelector_Change(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedWorkTask = WorkTasks.Find(workTask => workTask.Label == WorkTaskSelector.SelectedItem.ToString());
            WorkLogData.PurgeWorkLogAfterHour(DateTime.Now);
            WorkLogData.UpsertWorkLogToDb(SelectedWorkTask?.Id, DateTime.Now);
            ConfigData.UpsertData("LastUpdateExecution", DateTime.Now);
            UpdateWallpaper();
        }
 
        public static void UpdateWallpaper()
        {
            string pictureFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string filePath = System.IO.Path.Combine(pictureFolder, "WTS.bmp");

            WindowsUtils wallpaper = new(filePath);
            ImageGenerator imageGenerator = new(filePath);

            List<WorkLog> workLogs = WorkLogData.GetWorkLogsFromDate(CalendarUtils.GetCalendarStartDate());
            List<WorkDay> workDays = CalendarUtils.ConvertWorkLogToWorkDay(workLogs);

            List<TaskSummary> summaries = CalendarUtils.ConvertWorkDaysToTaskSummary(workDays);


            DateTime startOfToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            List<WorkLog> workLogsToday = WorkLogData.GetWorkLogsFromDate(startOfToday);

            new Task(() => { 
                imageGenerator.Draw(workDays, SelectedWorkTask, summaries, workLogsToday);
                wallpaper.SetDefaultWallpaper();
            }).Start();
        }

        private void ReloadWallpaper_Click(object sender, RoutedEventArgs e)
        {
            UpdateWallpaper();
        }

        private void OpenTaskWindow_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow taskWindow = new TaskWindow();
            taskWindow.Show();
        }

        private void OpenLogWindow_Click(object sender, RoutedEventArgs e)
        {
            LogsWindow logsWindow = new LogsWindow();
            logsWindow.Show();
        }

        private void OpenSettingWindow_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.Show();
        }
    }
}
