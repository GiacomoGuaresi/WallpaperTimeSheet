using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperTimeSheet.Utills
{
    public static class ColorsUtilis
    {
        public static String ToHex(System.Drawing.Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

        public static String ToRGB(System.Drawing.Color c) => $"RGB({c.R},{c.G},{c.B})";

        public static Color ToColor(string hex) => ColorTranslator.FromHtml(hex);
    }
}
