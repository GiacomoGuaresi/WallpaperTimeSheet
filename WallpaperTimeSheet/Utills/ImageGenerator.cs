using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using WallpaperTimeSheet.Models;

namespace WallpaperTimeSheet.Utills
{
    public sealed class ImageGenerator
    {
        private string filePath = "";
        private Bitmap bitmap;

        public ImageGenerator(string filePath)
        {
            this.filePath = filePath;

            double ScreenWidth = double.Parse(ConfigurationManager.AppSettings["ScreenWidth"]); //System.Windows.SystemParameters.PrimaryScreenWidth;
            double ScreenHeight = double.Parse(ConfigurationManager.AppSettings["ScreenHeight"]); //System.Windows.SystemParameters.PrimaryScreenHeight;
            bitmap = new Bitmap((int)ScreenWidth, (int)ScreenHeight);
        }

        public void Draw(List<WorkDay> workDays)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                DrawBackground(g);
                DrawDays(g, workDays, 30, 1200 - 300 - 100, 1920 - 60, 300);
            }
            bitmap.Save(filePath);
        }

        private void DrawDays(Graphics g, List<WorkDay> workDays, int widgetX, int widgetY, int widgetWidth, int widgetHeight)
        {
            int index = 0;
            int margin = 15;
            int fontSize = 12;

            int totalGaps = (workDays.Count - 1) + (workDays.Count / 7);
            int barWidth = (widgetWidth - (fontSize * 2) - totalGaps * 15) / workDays.Count;
            int barHeight = widgetHeight - (fontSize * 4);

            int x = widgetX + (fontSize * 3);
            int y = widgetY + (fontSize * 2);
            int? currentMonth = null;

            int maxWorkHours = workDays.Max(wd => wd.Tasks.Values.Sum());

            //g.DrawRectangle(new Pen(Color.Purple), widgetX, widgetY, widgetWidth, widgetHeight);

            for (int i = 1; i <= maxWorkHours; i++)
            {
                int scaleHeight = (int)((double)i / maxWorkHours * barHeight);
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;
                g.DrawString(i.ToString(), new Font("Arial", fontSize), new SolidBrush(Color.White), x, y + barHeight - scaleHeight, drawFormat);
            }

            foreach (WorkDay day in workDays)
            {
                SolidBrush basicBrush = new SolidBrush(Color.FromArgb(255, 25, 25, 25));

                Rectangle rect = new Rectangle(x, y, barWidth, barHeight);
                g.FillRectangle(basicBrush, rect);

                int lastY = y + barHeight;
                foreach (KeyValuePair<WorkTask, int> task in day.Tasks)
                {
                    int taskHeight = (int)((double)task.Value / maxWorkHours * barHeight);
                    SolidBrush taskBrush = new SolidBrush(ColorsUtilis.ToColor(task.Key.Color));
                    Rectangle taskRect = new Rectangle(x, lastY - taskHeight, barWidth, taskHeight);
                    g.FillRectangle(taskBrush, taskRect);
                    lastY -= taskRect.Height;
                }

                Font drawFont = new Font("Arial", fontSize);
                if (DateTime.Now.Month == day.Date.Month && DateTime.Now.Day == day.Date.Day)
                    drawFont = new Font("Arial", fontSize, FontStyle.Underline);

                SolidBrush drawBrush = new SolidBrush(Color.Gray);
                if (DateTime.Now.Month == day.Date.Month)
                    drawBrush = new SolidBrush(Color.White);

                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;
                g.DrawString(day.Date.Day.ToString(), drawFont, drawBrush, x + barWidth / 2, y + barHeight, drawFormat);

                if (currentMonth == null || day.Date.Month != currentMonth)
                {
                    currentMonth = day.Date.Month;
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(day.Date.Month).ToUpper();
                    g.DrawString(monthName, drawFont, drawBrush, x, y - 24);
                }

                if (day.Date.DayOfWeek == DayOfWeek.Sunday)
                    x += margin;

                x += barWidth + margin;
                index++;
            }
        }

        private void DrawBackground(Graphics g)
        {
            Random random = new Random();
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    int grayValue = random.Next(30, 35); // Valori di grigio scuro
                    Color randomGray = Color.FromArgb(255, grayValue, grayValue, grayValue);
                    bitmap.SetPixel(x, y, randomGray);
                }
            }
        }
    }
}
