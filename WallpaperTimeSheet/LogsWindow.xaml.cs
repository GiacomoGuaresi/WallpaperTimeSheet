using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.DataContext is WorkLog workLog)
            {
                string newValue = textBox.Text;

                // Prova a convertire il testo inserito (newValue) in un TimeSpan
                if (TimeSpan.TryParseExact(newValue, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan newTime))
                {

                    WorkLogData.DeleteWorkLog(workLog);

                    DateTime newDateTime = new DateTime(
                        SelectedDate.Year,
                        SelectedDate.Month,
                        SelectedDate.Day,
                        newTime.Hours,
                        newTime.Minutes,
                        0);

                    workLog.DateTime = newDateTime;
                    WorkLogData.AddWorkLogToDb(workLog);
                    UpdateList();
                }
            }
        }

        private void AddLog_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newValue = textBox.Text;

            WorkLog workLog = new WorkLog();

            // Prova a convertire il testo inserito (newValue) in un TimeSpan
            if (TimeSpan.TryParseExact(newValue, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan newTime))
            {
                DateTime newDateTime = new DateTime(
                    SelectedDate.Year,
                    SelectedDate.Month,
                    SelectedDate.Day,
                    newTime.Hours,
                    newTime.Minutes,
                    0);

                workLog.DateTime = newDateTime;
                WorkLogData.AddWorkLogToDb(workLog);
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
            System.Windows.Controls.ComboBox comboBox = sender as System.Windows.Controls.ComboBox;
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
                        workLog.WorkTask = AvailableWorkTasks.Find(m => m.Id == newValue);
                    }

                    WorkLogData.UpdateWorkLog(workLog);

                    UpdateList();
                }
            }
        }
    }
}
