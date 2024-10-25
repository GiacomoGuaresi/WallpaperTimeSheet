using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;
using WallpaperTimeSheet.Data;
using WallpaperTimeSheet.Models;
using WallpaperTimeSheet.Utills;
using System.IO;
using YourNamespace;
using WallpaperTimeSheet.Classes;

namespace WallpaperTimeSheet
{
    /// <summary>
    /// Logica di interazione per TrayWindow.xaml
    /// </summary>
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

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            List<WorkTask> workTasks = WorkTaskData.GetAllWorkTasks();
            Random rnd = new Random();

            DateTime moment = DateTime.Now;
            moment = new DateTime(moment.Year, moment.Month, moment.Day, 8, 0, 0);

            for (int i = 0; i < 30; i++)
            {
                foreach (WorkTask workTask in workTasks)
                {
                    WorkLog workLog = new WorkLog()
                    {
                        DateTime = moment,
                        WorkTaskId = workTask.Id
                    };
                    Trace.WriteLine(workLog.toString());
                    WorkLogData.AddWorkLogToDb(workLog);
                    moment = moment.AddHours(rnd.Next(1, 4));
                }

                //Set to null to end day 
                WorkLog workLogEndDay = new WorkLog()
                {
                    DateTime = moment
                };
                Trace.WriteLine(workLogEndDay.toString());
                WorkLogData.AddWorkLogToDb(workLogEndDay);

                moment = new DateTime(moment.Year, moment.Month, moment.Day, 8, 0, 0);
                moment = moment.AddDays(-1);
            }
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

            new Task(() => { 
                imageGenerator.Draw(workDays, SelectedWorkTask, summaries);
                wallpaper.SetDefaultWallpaper();
            }).Start();
        }

        private void ReloadWallpaper_Click(object sender, RoutedEventArgs e)
        {
            UpdateWallpaper();
        }
    }
}
