using System.Drawing;
using WallpaperTimeSheet.Models;

namespace WallpaperTimeSheet.Utills
{
    public class CalendarUtils
    {
        public List<WorkDay> Days { get; set; }

        public CalendarUtils()
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
                        { new WorkTask() { Label = "Task 1", Color = Color.Red }, rnd.Next(1, 4) },
                        { new WorkTask() { Label = "Task 2", Color = Color.Blue }, rnd.Next(1, 4) },
                        { new WorkTask() { Label = "Task 3", Color = Color.Green }, rnd.Next(1, 4) }
                    }
                });
            }
        }
    }
}
