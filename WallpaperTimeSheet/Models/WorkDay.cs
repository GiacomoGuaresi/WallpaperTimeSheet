using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperTimeSheet.Models
{
    public class WorkDay
    {
        public required DateTime Date { get; set; }

        public Dictionary<WorkTask, int> Tasks { get; set; }

        public WorkDay() {
            Tasks = new Dictionary<WorkTask, int>();
        }
    }
}
