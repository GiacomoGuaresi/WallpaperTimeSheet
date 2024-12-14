using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperTimeSheet.Classes
{
    class FaviconGenerator
    {
        public static void GenerateFavicon(Color backgroundColor)
        {
            // Crea un bitmap 24x24
            Bitmap bmp = new Bitmap(24, 24);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Disegna il quadrato con angoli stondati
                using (SolidBrush brush = new SolidBrush(backgroundColor))
                {
                    var rect = new Rectangle(0, 0, 24, 24);
                    var path = RoundedRectangle(rect, 5); // Angoli stondati di raggio 5px
                    g.FillPath(brush, path);
                }
            }


            // Sovrapporre l'icona
            string currentDirectory = System.Reflection.Assembly.GetEntryAssembly().Location;
            currentDirectory = Path.GetDirectoryName(currentDirectory);
            Icon overlayIcon = new Icon(Path.Combine(currentDirectory,"Icons/ic_fluent_briefcase_24_filled.ico"));
            Bitmap overlayBmp = overlayIcon.ToBitmap();

            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Sovrapposizione con alpha blending
                var rect = new Rectangle(0, 0, 24, 24); // Centrare l'icona sulla bitmap 24x24
                g.DrawImage(overlayBmp, rect);
            }

            // Salva il risultato come ICO usando il MemoryStream
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                using (var iconStream = new FileStream(Path.Combine(currentDirectory, "activeTaskIcon.ico"), FileMode.Create))
                {
                    CreateIconFromStream(ms, iconStream);
                }
            }

            // Carica e usa l'icona nella tray
            WallpaperTimeSheet.App.CahngeNoriIcon(Path.Combine(currentDirectory, "activeTaskIcon.ico"));
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void CreateIconFromStream(Stream inputPngStream, Stream outputIconStream)
        {
            // Usa un PNG per creare un file ICO valido
            BinaryWriter iconWriter = new BinaryWriter(outputIconStream);
            iconWriter.Write((short)0);   // idReserved
            iconWriter.Write((short)1);   // idType
            iconWriter.Write((short)1);   // idCount (1 immagine)

            // Scrivi il formato ICO
            iconWriter.Write((byte)16);  // bWidth
            iconWriter.Write((byte)16);  // bHeight
            iconWriter.Write((byte)0);   // bColorCount
            iconWriter.Write((byte)0);   // bReserved
            iconWriter.Write((short)1);  // wPlanes
            iconWriter.Write((short)32); // wBitCount
            iconWriter.Write((int)inputPngStream.Length);  // dwBytesInRes
            iconWriter.Write(22); // dwImageOffset (dove inizia l'immagine)

            // Scrivi i dati PNG nel file ICO
            inputPngStream.CopyTo(outputIconStream);
            iconWriter.Flush();
        }
    }
}
