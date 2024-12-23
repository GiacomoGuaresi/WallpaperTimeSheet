﻿using System.Globalization;
using System.Windows;
using System.Windows.Controls; 
using System.Windows.Input;
using WallpaperTimeSheet.Data;
using WallpaperTimeSheet.Models;
using TextBox = System.Windows.Controls.TextBox;

namespace WallpaperTimeSheet
{
    /// <summary>
    /// Logica di interazione per LogsWindow.xaml
    /// </summary>
    public partial class LogsWindow : Window
    {
        public List<WorkLog> workLogs { get; private set; } = new List<WorkLog>();
        public DateTime SelectedDate { get; private set; } = DateTime.Now;

        public List<ComboBoxItem> tasksComboBox = new List<ComboBoxItem>();
        public List<WorkTask> AvailableWorkTasks { get; private set; }

        public LogsWindow()
        {
            InitializeComponent();
            AvailableWorkTasks = WorkTaskData.GetAllWorkTasks();
            AvailableWorkTasks.Add(WorkTaskData.None);
            DataContext = this;
            UpdateList();
        }

        private void UpdateList()
        {
            workLogs = WorkLogData.GetWorkLogsOfDay(SelectedDate);
            workLogs.Sort((x, y) => y.DateTime.CompareTo(x.DateTime));
            LogLists.ItemsSource = workLogs;
            DateLabel.Content = SelectedDate.ToString("dd/MM/yyyy");
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate = SelectedDate.AddDays(-1);
            UpdateList();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate = SelectedDate.AddDays(1);
            UpdateList();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox != null && textBox.DataContext is WorkLog workLog)
            {
                string newValue = textBox.Text;

                // Prova a convertire il testo inserito (newValue) in un TimeSpan
                if (TimeSpan.TryParseExact(newValue, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan newTime))
                {
                    WorkLogData.DeleteWorkLog(workLog);
                    WorkLogData.UpsertWorkLogToDb(workLog.WorkTaskId, new DateTime(
                            workLog.DateTime.Year,
                            workLog.DateTime.Month,
                            workLog.DateTime.Day,
                            newTime.Hours,
                            newTime.Minutes,
                            0), false);

                    ConfigData.UpsertData("LastUpdateExecution", DateTime.Now);

                    UpdateList();
                }
            }
        }

        private void AddLog_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox? textBox = sender as TextBox;
            if (textBox == null)
                return;

            string newValue = textBox.Text;

            WorkLog workLog = new WorkLog();

            // Prova a convertire il testo inserito (newValue) in un TimeSpan
            if (TimeSpan.TryParseExact(newValue, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan newTime))
            {
                WorkLogData.UpsertWorkLogToDb(null, 
                    new DateTime(
                        SelectedDate.Year,
                        SelectedDate.Month,
                        SelectedDate.Day,
                        newTime.Hours,
                        newTime.Minutes,
                        0), false);

                ConfigData.UpsertData("LastUpdateExecution", DateTime.Now);

                UpdateList();

                textBox.Text = ""; 
            }
        }

        private void AddLog_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AddLog_LostFocus(sender, e);
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TextBox_LostFocus(sender, e);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox? comboBox = sender as System.Windows.Controls.ComboBox;
            if (comboBox != null && comboBox.DataContext is WorkLog workLog && comboBox.SelectedValue != null)
            {
                int newValue = (int)comboBox.SelectedValue;
                if (workLog.WorkTaskId != newValue)
                {
                    if(newValue == 0)
                    {
                        workLog.WorkTaskId = null;
                        workLog.WorkTask = null;
                    }
                    else
                    {
                        workLog.WorkTaskId = newValue;
                        workLog.WorkTask = AvailableWorkTasks.Find(m => m.Id == newValue) ?? throw new InvalidOperationException("WorkTask not found");
                    }

                    WorkLogData.UpsertWorkLogToDb(workLog.WorkTaskId, workLog.DateTime, false);

                    ConfigData.UpsertData("LastUpdateExecution", DateTime.Now);

                    UpdateList();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button? button = sender as System.Windows.Controls.Button;
            if (button != null && button.DataContext is WorkLog workLog)
            {
                WorkLogData.DeleteWorkLog(workLog);
                UpdateList();
            }
        }
    }
}
