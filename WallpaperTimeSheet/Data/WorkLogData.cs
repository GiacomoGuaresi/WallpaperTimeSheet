﻿using Microsoft.EntityFrameworkCore;
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

        public static void UpdateWorkLog(WorkLog workLog)
        {
            using (var db = new AppDbContext())
            {
                db.WorkLogs.Update(workLog);
                db.SaveChanges();
            }
        }

        public static void DeleteWorkLog(WorkLog workLog)
        {
            using (var db = new AppDbContext())
            {
                db.WorkLogs.Remove(workLog);
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
        public static WorkLog GetLastWorkLog()
        {
            using (var db = new AppDbContext())
            {
                return db.WorkLogs
                    .Include(wl => wl.WorkTask)
                    .OrderByDescending(wl => wl.DateTime)
                    .FirstOrDefault();
            }
        }

        public static void UpsertWorkLogToDb(int? workTaskId, DateTime dateTime)
        {
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);

            using (var db = new AppDbContext())
            {
                var existingWorkLog = db.WorkLogs.FirstOrDefault(wl => wl.DateTime == dateTime);

                if (existingWorkLog != null)
                {
                    existingWorkLog.WorkTaskId = workTaskId;
                    db.WorkLogs.Update(existingWorkLog);
                }
                else
                {
                    var newWorkLog = new WorkLog
                    {
                        DateTime = dateTime,
                        WorkTaskId = workTaskId
                    };
                    db.WorkLogs.Add(newWorkLog);
                }

                db.SaveChanges();
            }
        }


    }
}
