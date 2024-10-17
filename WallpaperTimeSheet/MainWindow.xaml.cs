using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using WallpaperTimeSheet.Data;
using WallpaperTimeSheet.Models;
using WallpaperTimeSheet.Utills;

namespace WallpaperTimeSheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pictureFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string filePath = Path.Combine(pictureFolder, "WTS.bmp");

            WindowsUtils wallpaper = new(filePath);
            ImageGenerator imageGenerator = new(filePath);
            CalendarUtils calendarUtils = new();

            List<WorkLog> workLogs = WorkLogData.GetWorkLogsFromDate(CalendarUtils.GetCalendarStartDate());
            calendarUtils.ConvertWorkLogToWorkDay(workLogs);

            imageGenerator.Draw(calendarUtils.Days);
            wallpaper.SetDefaultWallpaper();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            WindowsUtils.SetAccentColor(255, 0, 0);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            List<WorkLog> workLogs = WorkLogData.GetWorkLogsFromDate( DateTime.Now.AddDays(-7));
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            List<WorkTask> workTasks = WorkTaskData.GetAllWorkTasks();
            Random rnd = new Random();

            DateTime moment = DateTime.Now;
            moment = new DateTime(moment.Year, moment.Month, moment.Day, 8, 0, 0);

            for (int i = 0; i < 30; i++)
            {
                foreach(WorkTask workTask in workTasks)
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
    }
}