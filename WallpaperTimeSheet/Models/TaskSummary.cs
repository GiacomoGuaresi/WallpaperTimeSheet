using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperTimeSheet.Models
{
    public class TaskSummary
    {
        public WorkTask task { get; set; }
        public int TotalHours { get; set; }
    }
}
