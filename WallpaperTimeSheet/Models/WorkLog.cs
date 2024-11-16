using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperTimeSheet.Models
{
    public class WorkLog
    {
        [Key]
        public DateTime DateTime { get; set; }

        [ForeignKey("WorkTask")]
        public int? WorkTaskId { get; set; }
        public virtual WorkTask? WorkTask { get; set; }


        public string toString()
        {
            return $"{DateTime} - {WorkTaskId}";
        }
    }
}
