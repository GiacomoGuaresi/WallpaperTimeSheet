using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        public void Draw(List<WorkDay> workDays, WorkTask activeTask, List<TaskSummary> taskSummaries, List<WorkLog> workLogs)
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

                DrawTaskSummary(
                    g,
                    taskSummaries,
                    int.Parse(ConfigurationManager.AppSettings["TaskSummaryWidgetX"]),
                    int.Parse(ConfigurationManager.AppSettings["TaskSummaryWidgetY"]),
                    int.Parse(ConfigurationManager.AppSettings["TaskSummaryWidgetWidth"]),
                    int.Parse(ConfigurationManager.AppSettings["TaskSummaryWidgetHeight"]),
                    new Font("Arial", int.Parse(ConfigurationManager.AppSettings["TaskSummaryWidgetFontSize"])));

                DrawLog(
                    g,
                    workLogs,
                    int.Parse(ConfigurationManager.AppSettings["LogWidgetX"]),
                    int.Parse(ConfigurationManager.AppSettings["LogWidgetY"]),
                    int.Parse(ConfigurationManager.AppSettings["LogWidgetWidth"]),
                    int.Parse(ConfigurationManager.AppSettings["LogWidgetHeight"]),
                    new Font("Arial", int.Parse(ConfigurationManager.AppSettings["LogWidgetFontSize"])));
            }
            bitmap.Save(filePath);
        }

        private void DrawCalendar(Graphics g, List<WorkDay> workDays, int widgetX, int widgetY, int widgetWidth, int widgetHeight, Font font)
        {
            int index = 0;
            int margin = 15;

            int totalGaps = (workDays.Count - 1) + (workDays.Count / 7);
            int barWidth = (widgetWidth - ((int)font.Size * 2) - totalGaps * 15) / workDays.Count;
            int barHeight = widgetHeight - ((int)font.Size * 6);

            int x = widgetX + ((int)font.Size * 3);
            int y = widgetY + ((int)font.Size * 2);
            int? currentMonth = null;

            int maxWorkHours = workDays.Max(wd => wd.Tasks.Values.Sum());
            int totalHoursOnWeek = 0;

            //Pen debugPen = new Pen(Color.Purple);
            //g.DrawRectangle(debugPen, widgetX, widgetY, widgetWidth, widgetHeight);

            for (int i = 1; i <= maxWorkHours; i++)
            {
                int scaleHeight = (int)((double)i / maxWorkHours * barHeight);
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;
                g.DrawString(i.ToString(), font, new SolidBrush(Color.White), x, y + barHeight - scaleHeight, drawFormat);

                var dashedPen = new Pen(Color.Gray);
                float[] dashValues = { 2, 5, 2, 5 };
                dashedPen.DashPattern = dashValues;
                g.DrawLine(
                    dashedPen, 
                    x,
                    y + barHeight - scaleHeight + font.Size, 
                    x + widgetWidth - (((int)font.Size * 3) * 2),
                    y + barHeight - scaleHeight + font.Size);
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
                    totalHoursOnWeek += task.Value; 
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
                {
                    Point startingPoint = new Point(
                        x - ((barWidth + margin) * 6), 
                        y + barHeight + (int)(font.Size * 2.5));
                    Point endPoint = new Point(
                        x + barWidth, 
                        y + barHeight + (int)(font.Size * 2.5));

                    Point middlePoint = new Point(
                        startingPoint.X + ((endPoint.X - startingPoint.X) / 2),
                        y + barHeight + (int)font.Size * 2 + 10);

                    Pen linePen = new Pen(Color.Gray);
                    Brush textBrush = new SolidBrush(Color.Gray);
                    g.DrawLine(linePen, startingPoint, endPoint); 
                    g.DrawString("Totale ore: " + totalHoursOnWeek.ToString(), drawFont, textBrush, middlePoint, drawFormat);
                    totalHoursOnWeek = 0; 
                    x += margin;
                }

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

        private void DrawTaskSummary(Graphics g, List<TaskSummary> taskSummaries, int widgetX, int widgetY, int widgetWidth, int widgetHeight, Font font)
        {
            //Pen debugPen = new Pen(Color.Purple);
            //g.DrawRectangle(debugPen, widgetX, widgetY, widgetWidth, widgetHeight);
            int y = widgetY;
            int x = widgetX;
            foreach (TaskSummary taskSummary in taskSummaries)
            {
                SolidBrush borderBrush = new SolidBrush(ColorsUtilis.ToColor(taskSummary.task.Color));
                Rectangle rect = new Rectangle(x, y, 8, (int)(font.Size * 3.5));
                g.FillRoundedRectangle(borderBrush, rect, 4);

                Font titleFont = new Font(font, FontStyle.Bold);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                g.DrawString(taskSummary.task.Label, titleFont, borderBrush, x + 10, y);
                g.DrawString("Ore questo mese: " + taskSummary.TotalHours, font, whiteBrush, x + 10, y + (titleFont.Size * 2));
                y += (int)(font.Size * 5);
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

        private void DrawLog(Graphics g, List<WorkLog> workLogs, int widgetX, int widgetY, int widgetWidth, int widgetHeight, Font font)
        {
            //print all log in column 
            int y = widgetY;
            int x = widgetX;

            workLogs.Sort((a, b) => b.DateTime.CompareTo(a.DateTime));

            g.DrawLine(new Pen(Color.White), x, y, x, y + (font.Size * workLogs.Count * 2) - font.Size);

            foreach (WorkLog workLog in workLogs)
            {
                string logText = workLog.DateTime.ToString("HH:mm");
                logText += " - ";

                SolidBrush brush = new SolidBrush(ColorsUtilis.ToColor(WorkTaskData.None.Color));
                if (workLog.WorkTaskId == null)
                {
                    logText += WorkTaskData.None.Label;
                }
                else
                {
                    brush = new SolidBrush(ColorsUtilis.ToColor(workLog.WorkTask.Color));
                    logText += workLog.WorkTask.Label;
                }

                g.DrawString(logText, font, new SolidBrush(Color.White), x + 10, y);
                g.FillCircle(brush, x, y + font.Size, 7);

                y += (int)font.Size * 2;
            }
        }
    }
}
