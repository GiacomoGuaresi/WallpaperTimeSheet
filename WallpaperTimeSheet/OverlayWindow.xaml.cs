using System.Windows;
using System.Windows.Media.Media3D;

namespace WallpaperTimeSheet
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow(Rect bounds)
        {
            InitializeComponent();
            Left = bounds.Left;
            Top = bounds.Top;
            Width = bounds.Width;
            Height = bounds.Height;
        }
    }
}
