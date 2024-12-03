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
    public class Config
    {
        [Key]
        public string key { get; set; }

        public string value { get; set; }
    }
}
