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
        public void GenerateFavicon(Color backgroundColor)
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

            // Inverti i colori chiari dell'immagine
            Bitmap invertedOverlayBmp = InvertLightColors(overlayBmp);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Sovrapposizione con alpha blending
                var rect = new Rectangle(0, 0, 24, 24); // Centrare l'icona sulla bitmap 24x24
                g.DrawImage(invertedOverlayBmp, rect);
            }

            // Salva il risultato come ICO usando il MemoryStream
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);

                using (var iconStream = new FileStream("favicon.ico", FileMode.Create))
                {
                    CreateIconFromStream(ms, iconStream);
                }
            }

            // Carica e usa l'icona nella tray
            Icon icon = new Icon("favicon.ico");
            System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = icon;
            notifyIcon.Visible = true;
        }

        private Bitmap InvertLightColors(Bitmap bmp)
        {
            Bitmap invertedBmp = new Bitmap(bmp.Width, bmp.Height);

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color originalColor = bmp.GetPixel(x, y);

                    // Inverti solo i colori chiari (luminosità > 200)
                    if (originalColor.R > 200 && originalColor.G > 200 && originalColor.B > 200)
                    {
                        Color invertedColor = Color.FromArgb(
                            originalColor.A, // Mantiene l'alpha
                            255 - originalColor.R,
                            255 - originalColor.G,
                            255 - originalColor.B);
                        invertedBmp.SetPixel(x, y, invertedColor);
                    }
                    else
                    {
                        invertedBmp.SetPixel(x, y, originalColor);
                    }
                }
            }

            return invertedBmp;
        }

        private System.Drawing.Drawing2D.GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
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

        private void CreateIconFromStream(Stream inputPngStream, Stream outputIconStream)
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
