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
using WallpaperTimeSheet.Classes;
using WallpaperTimeSheet.Utills;

namespace WallpaperTimeSheet
{
    /// <summary>
    /// Logica di interazione per SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();

            if(WindowsUtils.CheckIfInStartup())
            {
                RunAtStartupCheckBox.IsChecked = true;
            }
        }

        private void RunAtStartupCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(RunAtStartupCheckBox.IsChecked == true)
            {
                WindowsUtils.AddToStartup();
            }
            else
            {
                WindowsUtils.RemoveFromStartup();
            }
        }

        private void CheckTrayColor_Click(object sender, RoutedEventArgs e)
        {            
            System.Drawing.Color color = ColorsUtilis.ToColor("#"+hexColor.Text);
            FaviconGenerator.GenerateFavicon(color);
        }
    }
}
