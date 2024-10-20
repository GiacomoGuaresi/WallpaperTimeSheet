using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperTimeSheet.Models;

namespace WallpaperTimeSheet.Data
{
    public class WorkTaskData
    {
        public static void AddWorkTaskToDb(WorkTask workTask)
        {
            using (var db = new AppDbContext())
            {
                db.WorkTasks.Add(workTask);
                db.SaveChanges();
            }
        }

        public static List<WorkTask> GetAllWorkTasks()
        {
            using (var db = new AppDbContext())
            {
                return db.WorkTasks.ToList();
            }
        }

        public static readonly WorkTask None = new WorkTask
        {
            Id = 0,
            Label = "Nessuna",
            Color = "#333333"
        };
    }
}
