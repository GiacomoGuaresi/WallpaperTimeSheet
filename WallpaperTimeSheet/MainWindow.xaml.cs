using System.Drawing;
using System.IO;
using System.Windows;
using WallpaperTimeSheet.Models;
using WallpaperTimeSheet.Utills;

namespace WallpaperTimeSheet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pictureFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string filePath = Path.Combine(pictureFolder, "WTS.bmp");

            WindowsUtils wallpaper = new(filePath);
            ImageGenerator imageGenerator = new(filePath);
            CalendarUtils calendarUtils = new();

            imageGenerator.Draw(calendarUtils.Days);
            wallpaper.SetDefaultWallpaper();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            WindowsUtils.SetAccentColor(255, 0, 0);
        }
    }
}