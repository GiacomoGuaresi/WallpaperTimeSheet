using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Windows.Controls;
using WallpaperTimeSheet.Data;
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

            double ScreenWidth = double.Parse(ConfigurationManager.AppSettings["ScreenWidth"]);
            double ScreenHeight = double.Parse(ConfigurationManager.AppSettings["ScreenHeight"]);
            bitmap = new Bitmap((int)ScreenWidth, (int)ScreenHeight);
        }

        public void Draw(List<WorkDay> workDays, WorkTask activeTask)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                DrawBackground(g);

                DrawCalendar(
                    g,
                    workDays,
                    int.Parse(ConfigurationManager.AppSettings["CalendarWidgetX"]),
                    int.Parse(ConfigurationManager.AppSettings["CalendarWidgetY"]),
                    int.Parse(ConfigurationManager.AppSettings["CalendarWidgetWidth"]),
                    int.Parse(ConfigurationManager.AppSettings["CalendarWidgetHeight"]),
                    new Font("Arial", int.Parse(ConfigurationManager.AppSettings["CalendarWidgetFontSize"])));

                DrawActiveTask(
                    g,
                    activeTask,
                    int.Parse(ConfigurationManager.AppSettings["ActiveTaskWidgetX"]),
                    int.Parse(ConfigurationManager.AppSettings["ActiveTaskWidgetY"]),
                    int.Parse(ConfigurationManager.AppSettings["ActiveTaskWidgetWidth"]),
                    int.Parse(ConfigurationManager.AppSettings["ActiveTaskWidgetHeight"]),
                    new Font("Arial", int.Parse(ConfigurationManager.AppSettings["ActiveTaskWidgetFontSize"])));
            }
            bitmap.Save(filePath);
        }

        private void DrawCalendar(Graphics g, List<WorkDay> workDays, int widgetX, int widgetY, int widgetWidth, int widgetHeight, Font font)
        {
            int index = 0;
            int margin = 15;

            int totalGaps = (workDays.Count - 1) + (workDays.Count / 7);
            int barWidth = (widgetWidth - ((int)font.Size * 2) - totalGaps * 15) / workDays.Count;
            int barHeight = widgetHeight - ((int)font.Size * 4);

            int x = widgetX + ((int)font.Size * 3);
            int y = widgetY + ((int)font.Size * 2);
            int? currentMonth = null;

            int maxWorkHours = workDays.Max(wd => wd.Tasks.Values.Sum());


            for (int i = 1; i <= maxWorkHours; i++)
            {
                int scaleHeight = (int)((double)i / maxWorkHours * barHeight);
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;
                g.DrawString(i.ToString(), font, new SolidBrush(Color.White), x, y + barHeight - scaleHeight, drawFormat);
            }

            foreach (WorkDay day in workDays)
            {
                SolidBrush basicBrush = new SolidBrush(Color.FromArgb(255, 25, 25, 25));

                Rectangle rect = new Rectangle(x, y, barWidth, barHeight);
                g.FillRoundedRectangle(basicBrush, rect, barWidth / 8);

                int lastY = y + barHeight;
                foreach (KeyValuePair<WorkTask, int> task in day.Tasks)
                {
                    int taskHeight = (int)((double)task.Value / maxWorkHours * barHeight);
                    SolidBrush taskBrush = new SolidBrush(ColorsUtilis.ToColor(task.Key.Color));
                    Rectangle taskRect = new Rectangle(x, lastY - taskHeight + 2, barWidth, taskHeight - 2);
                    g.FillRoundedRectangle(taskBrush, taskRect, barWidth / 8);
                    lastY -= taskHeight;
                }

                Font drawFont = font;
                if (DateTime.Now.Month == day.Date.Month && DateTime.Now.Day == day.Date.Day)
                    drawFont = new Font(font, FontStyle.Underline);

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
                    g.DrawString(monthName, drawFont, drawBrush, x, y - font.Size * 2);
                }

                if (day.Date.DayOfWeek == DayOfWeek.Sunday)
                    x += margin;

                x += barWidth + margin;
                index++;
            }
        }

        private void DrawActiveTask(Graphics g, WorkTask task, int widgetX, int widgetY, int widgetWidth, int widgetHeight, Font font)
        {
            if(task == null)
            {
                task = WorkTaskData.None;
            }

            Brush brush = new SolidBrush(ColorsUtilis.ToColor(task.Color));
            Rectangle rect = new Rectangle(widgetX, widgetY, widgetWidth, widgetHeight + 50);
            g.FillRectangle(brush, rect);
            StringFormat drawFormat = new StringFormat();
            drawFormat.LineAlignment = StringAlignment.Center;

            Rectangle textRect = new Rectangle(widgetX + 30, widgetY, widgetWidth - 60, widgetHeight);
            g.DrawString("Attività in corso: " + task.Label, font, new SolidBrush(Color.White), textRect, drawFormat);
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
