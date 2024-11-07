using System;
using System.Collections.Generic;
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

namespace WallpaperTimeSheet
{
    /// <summary>
    /// Logica di interazione per LogsWindow.xaml
    /// </summary>
    public partial class LogsWindow : Window
    {
        public List<WorkLog> workLogs { get; private set; } = new List<WorkLog>();
        private WorkLog selectedLog = null;
        public DateTime SelectedDate { get; private set; } = DateTime.Now;
        public LogsWindow()
        {
            InitializeComponent();
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

        private void LogLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
