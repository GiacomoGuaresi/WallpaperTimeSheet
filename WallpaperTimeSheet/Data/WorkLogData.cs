using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WallpaperTimeSheet.Models;

namespace WallpaperTimeSheet.Data
{
    public static class WorkLogData
    {
        public static void AddWorkLogToDb(WorkLog workLog)
        {
            using (var db = new AppDbContext())
            {
                db.WorkLogs.Add(workLog);
                db.SaveChanges();
            }
        }

        public static List<WorkLog> GetAllWorkLog()
        {
            using (var db = new AppDbContext())
            {
                return db.WorkLogs
                    .Include(wl => wl.WorkTask)
                    .ToList();
            }
        }

        public static List<WorkLog> GetWorkLogsFromDate(DateTime dateTime)
        {
            using (var db = new AppDbContext())
            {
                return db.WorkLogs
                    .Where(wl => wl.DateTime.Date >= dateTime.Date)
                    .Include(wl => wl.WorkTask)
                    .ToList();
            }
        }
    }
}
