using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WallpaperTimeSheet.Classes;
using WallpaperTimeSheet.Models;
using WallpaperTimeSheet.Utills;

namespace WallpaperTimeSheet
{
    /// <summary>
    /// Logica di interazione per TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public List<WorkTask> workTasks { get; private set; }
        
        public TaskWindow()
        {
            InitializeComponent();
            UpdateList();
            GenerateColorButtons();
        }

        private void GenerateColorButtons()
        {
            var colors = new TaskColor[]
            {
                TaskColors.GialloOro, TaskColors.Oro, TaskColors.ArancioBrillante, TaskColors.ArancioneScuro,
                TaskColors.Ruggine, TaskColors.RugginePallido, TaskColors.RossoMattone, TaskColors.RossoModerno,
                TaskColors.RossoPallido, TaskColors.Rosso, TaskColors.RosaBrillante, TaskColors.Rosa,
                TaskColors.PrugnaChiaro, TaskColors.Prugna, TaskColors.OrchideaChiaro, TaskColors.Orchidea,
                TaskColors.Blu, TaskColors.BluScuro, TaskColors.Viola, TaskColors.ViolaScuro, TaskColors.IrisPastello,
                TaskColors.IrisPrimaverile, TaskColors.RossoViolaChiaro, TaskColors.RossoViola,
                TaskColors.BluFreddoBrillante, TaskColors.BluFreddo, TaskColors.SpumaMarina, TaskColors.VerdeAcqua,
                TaskColors.VerdeMentaChiaro, TaskColors.VerdeMentaScuro, TaskColors.VerdeErba, TaskColors.VerdeSport,
                TaskColors.Grigio, TaskColors.MarroneGrigio, TaskColors.BluAcciaio, TaskColors.BluMetallico,
                TaskColors.VerdeMuschioPallido, TaskColors.VerdeMuschioScuro, TaskColors.VerdePrato, TaskColors.Verde,
                TaskColors.GrigioPlumbeo, TaskColors.GrigioTemporale, TaskColors.GrigioBlu, TaskColors.GrigioScuro,
                TaskColors.VerdeNinfea, TaskColors.Salvia, TaskColors.MimeticoDeserto, TaskColors.Mimetico
            };

            int columns = ColorButtonGrid.ColumnDefinitions.Count;
            int row = 0, column = 0;

            foreach (var color in colors)
            {
                System.Windows.Controls.Button button = new System.Windows.Controls.Button
                {
                    Background = new SolidColorBrush(ColorsUtilis.ToWindowsMediaColor(color.HexColor)),
                    Margin = new Thickness(2),
                    Width = 30,
                    Height = 30
                };

                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);
                ColorButtonGrid.Children.Add(button);

                column++;
                if (column >= columns)
                {
                    column = 0;
                    row++;
                }
            }
        }


        public void UpdateList()
        {
            using (AppDbContext context = new AppDbContext())
            {
                workTasks = context.WorkTasks.ToList();
                workTasks.Sort((a, b) => a.Label.CompareTo(b.Label));
                TaskLists.ItemsSource = workTasks;
            }
        }
    }
}
