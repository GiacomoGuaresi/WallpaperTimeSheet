using System;
using System.Drawing;

namespace WallpaperTimeSheet.Utills
{
    public static class ColorsUtilis
    {
        public static string ToHex(Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

        public static string ToRGB(Color c) => $"RGB({c.R},{c.G},{c.B})";

        public static Color ToColor(string hex) => ColorTranslator.FromHtml(hex);
    }
}
