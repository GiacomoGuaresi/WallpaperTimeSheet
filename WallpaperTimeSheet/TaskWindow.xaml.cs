using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WallpaperTimeSheet.Classes;
using WallpaperTimeSheet.Data;
using WallpaperTimeSheet.Models;
using WallpaperTimeSheet.Utills;
using System.Linq;

namespace WallpaperTimeSheet
{
    public partial class TaskWindow : Window
    {
        public List<WorkTask> WorkTasks { get; private set; } = new List<WorkTask>();
        private System.Windows.Controls.Button SelectedColorButton = null;
        public TaskColor? SelectedColor { get; private set; }
        private WorkTask SelectedTask = null;

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
                var button = new System.Windows.Controls.Button
                {
                    Background = new SolidColorBrush(ColorsUtilis.ToWindowsMediaColor(color.HexColor)),
                    Margin = new Thickness(2),
                    Width = 30,
                    Height = 30,
                    Tag = color
                };

                button.Click += ColorButton_Click;

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

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateColorButtons(sender);
        }

        private void UpdateColorButtons(object sender)
        {
            if (sender is System.Windows.Controls.Button clickedButton)
            {
                if (SelectedColorButton != null)
                {
                    SelectedColorButton.BorderThickness = new Thickness(0);
                }

                clickedButton.BorderThickness = new Thickness(3);
                clickedButton.BorderBrush = System.Windows.Media.Brushes.Black;

                SelectedColorButton = clickedButton;
                SelectedColor = (TaskColor?)clickedButton.Tag;
            }
        }

        public void UpdateList()
        {
            using var context = new AppDbContext();
            WorkTasks = context.WorkTasks.OrderBy(task => task.Label).ToList();
            TaskLists.ItemsSource = WorkTasks;
        }

        private void TaskLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteButton.IsEnabled = TaskLists.SelectedItem != null;
            UpdateButton.IsEnabled = TaskLists.SelectedItem != null;

            if (TaskLists.SelectedItem != null)
            {
                SelectedTask = (WorkTask)TaskLists.SelectedItem;
                label.Text = SelectedTask.Label;

                var color = SelectedTask.Color;
                SelectColorButton(TaskColors.GetColorByHex(color));
            }
        }

        private void SelectColorButton(TaskColor? color)
        {
            if (color == null) return;

            foreach (System.Windows.Controls.Button button in ColorButtonGrid.Children)
            {
                if (button.Tag is TaskColor btnColor && btnColor.HexColor == color.HexColor)
                {
                    UpdateColorButtons(button);
                    break;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (label.Text == "" || SelectedColor == null)
            {
                System.Windows.MessageBox.Show("Please fill in all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using var context = new AppDbContext();

            if (WorkTasks.Exists(task => task.Label == label.Text))
            {
                System.Windows.MessageBox.Show("Task already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            WorkTask workTask = new WorkTask
            {
                Label = label.Text,
                Color = SelectedColor.HexColor
            };

            context.WorkTasks.Add(workTask);
            context.SaveChanges();

            UpdateList();
            ResetForm();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (TaskLists.SelectedItem != null)
            {
                using var context = new AppDbContext();

                context.Remove(TaskLists.SelectedItem);
                context.SaveChanges();

                UpdateList();
                ResetForm();
            }
        }

        private void ResetForm()
        {
            label.Text = "";
            DeselectSelectedButton();
            SaveButton.Content = "Salva";
        }

        private void DeselectSelectedButton()
        {
            if (SelectedColorButton != null)
            {
                SelectedColorButton.BorderThickness = new Thickness(0);
                SelectedColorButton = null;
            }
            SelectedColor = null;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (label.Text == "" || SelectedColor == null)
            {
                System.Windows.MessageBox.Show("Please fill in all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using var context = new AppDbContext();

            if(SelectedTask == null)
            {
                System.Windows.MessageBox.Show("Task not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SelectedTask.Label = label.Text;
            SelectedTask.Color = SelectedColor.HexColor;
            
            context.Update(SelectedTask);
            context.SaveChanges();
            
            SelectedTask = null;
            UpdateList();
            ResetForm();
        }
    }
}
