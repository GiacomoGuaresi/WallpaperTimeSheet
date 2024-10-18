namespace WallpaperTimeSheet.Classes
{
    public class TaskColor
    {
        public string Color { get; set; }
        public string AccentColor { get; set; }

        public TaskColor(string color, string accentColor)
        {
            Color = color;
            AccentColor = accentColor;
        }
    }

    public static class TaskColors
    {
        public static readonly TaskColor Red = new TaskColor("#FF0000", "#AA0000");
        public static readonly TaskColor Green = new TaskColor("#00FF00", "#00AA00");
        public static readonly TaskColor Blue = new TaskColor("#0000FF", "#0000AA");
        public static readonly TaskColor Black = new TaskColor("#000000", "#555555");
    }
}
