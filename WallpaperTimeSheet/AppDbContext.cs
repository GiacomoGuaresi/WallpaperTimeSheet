using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallpaperTimeSheet.Models;

namespace WallpaperTimeSheet
{
    internal class AppDbContext : DbContext
    {
        public DbSet<WorkTask> WorkTasks { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }

        public string path = Path.Combine(SpecialDirectories.MyDocuments, "WallpaperTimeSheet.db");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
            optionsBuilder.UseSqlite($"Data Source={path}");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
