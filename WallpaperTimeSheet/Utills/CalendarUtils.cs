using System.Drawing;
using WallpaperTimeSheet.Models;

namespace WallpaperTimeSheet.Utills
{
    public class CalendarUtils
    {   
        public CalendarUtils()
        {
            Days = new List<WorkDay>();
        }
        public List<WorkDay> Days { get; set; }

        public void MockData()
        {
            var currentDate = DateTime.Now;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var startDate = firstDayOfMonth.AddDays(1 - (int)firstDayOfMonth.DayOfWeek);
            var endDate = startDate.AddDays((7 * 6) - 1);

            // Crea i WorkDay per l'intervallo di date
            Random rnd = new Random();
            Days = new List<WorkDay>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                Days.Add(new WorkDay()
                {
                    Date = date,
                    Tasks = new Dictionary<WorkTask, int>() {
                        { new WorkTask() { Label = "Task 1", Color = ColorsUtilis.ToHex(Color.Red)}, rnd.Next(1, 4) },
                        { new WorkTask() { Label = "Task 2", Color = ColorsUtilis.ToHex(Color.Blue)}, rnd.Next(1, 4) },
                        { new WorkTask() { Label = "Task 3", Color = ColorsUtilis.ToHex(Color.Green)}, rnd.Next(1, 4) }
                    }
                });
            }
        }

        public static DateTime GetCalendarStartDate()
        {
            var currentDate = DateTime.Now;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return firstDayOfMonth.AddDays(1 - (int)firstDayOfMonth.DayOfWeek);
        }

        public static DateTime GetCalendarEndDate()
        {
            return GetCalendarStartDate().AddDays((7 * 6) - 1);
        }

        public void ConvertWorkLogToWorkDay(List<WorkLog> workLogs)
        {
            var currentDate = DateTime.Now;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var startDate = GetCalendarStartDate();
            var endDate = GetCalendarEndDate();

            Random rnd = new Random();
            var workDays = new Dictionary<DateTime, WorkDay>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                workDays.Add(date, new WorkDay()
                {
                    Date = date,
                    Tasks = new()
                });
            }



            var orderedLogs = workLogs.OrderBy(wl => wl.DateTime).ToList();
            
            for (int i = 0; i < orderedLogs.Count - 1; i++)
            {
                var currentLog = orderedLogs[i];
                var nextLog = orderedLogs[i + 1];

                var hoursWorked = (int)(nextLog.DateTime - currentLog.DateTime).TotalHours;
                if (hoursWorked <= 0) continue;

                var day = currentLog.DateTime.Date;
                if (!workDays.ContainsKey(day))
                    continue; 
                
                if (currentLog.WorkTask != null)
                {
                    var task = currentLog.WorkTask;
                    if (!workDays[day].Tasks.ContainsKey(task))
                    {
                        workDays[day].Tasks[task] = 0;
                    }
                    workDays[day].Tasks[task] += hoursWorked;
                }
            }

            var lastLog = orderedLogs.Last();
            var lastDay = lastLog.DateTime.Date;
            if (!workDays.ContainsKey(lastDay))
            {
                workDays[lastDay] = new WorkDay { Date = lastDay };
            }
            if (lastLog.WorkTask != null)
            {
                var hoursLeftInDay = 24 - lastLog.DateTime.Hour;
                if (hoursLeftInDay > 0)
                {
                    var task = lastLog.WorkTask;
                    if (!workDays[lastDay].Tasks.ContainsKey(task))
                    {
                        workDays[lastDay].Tasks[task] = 0;
                    }
                    workDays[lastDay].Tasks[task] += hoursLeftInDay;
                }
            }

            Days = workDays.Values.ToList();
        }
    }
}
