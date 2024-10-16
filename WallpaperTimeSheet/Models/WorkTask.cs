using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperTimeSheet.Models
{
    public class WorkTask
    {
        public required string Label { get; set; }
        public required Color Color { get; set; }
    }
}
