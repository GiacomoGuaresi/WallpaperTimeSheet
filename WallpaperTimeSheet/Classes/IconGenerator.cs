using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace YourNamespace
{
    public class IconGenerator
    {
        private readonly string savePath;
        private readonly string iconBasePath;

        public IconGenerator(string savePath, string iconBasePath)
        {
            this.savePath = savePath;
            this.iconBasePath = iconBasePath;
        }

        /// <summary>
        /// Genera un'icona .ico da un'icona PNG bianca trasparente, cambiando il colore.
        /// </summary>
        /// <param name="pngFilePath">Il percorso del file PNG originale</param>
        /// <param name="color">Il colore che sostituirà il bianco nell'icona</param>
        public void GenerateIcon(Color color)
        {
            if (!File.Exists(iconBasePath))
                throw new FileNotFoundException("Il file PNG non esiste", iconBasePath);

            using (Bitmap originalBitmap = new Bitmap(iconBasePath))
            {
                Bitmap coloredBitmap = ChangeImageColor(originalBitmap, color);
                SaveAsIcon(coloredBitmap, Path.Combine(savePath));
            }
        }

        /// <summary>
        /// Cambia il colore dell'immagine bianca trasparente con il colore specificato.
        /// </summary>
        private Bitmap ChangeImageColor(Bitmap originalBitmap, Color newColor)
        {
            Bitmap coloredBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);
            for (int x = 0; x < originalBitmap.Width; x++)
            {
                for (int y = 0; y < originalBitmap.Height; y++)
                {
                    Color originalColor = originalBitmap.GetPixel(x, y);
                    int alpha = originalColor.A;
                    Color recoloredPixel = Color.FromArgb(alpha, newColor.R, newColor.G, newColor.B);
                    coloredBitmap.SetPixel(x, y, recoloredPixel);
                }
            }

            return coloredBitmap;
        }

        /// <summary>
        /// Converte e salva un'immagine Bitmap come file ICO a 32-bit con trasparenza.
        /// </summary>
        private void SaveAsIcon(Bitmap bitmap, string filePath)
        {
            // Assicurati che il bitmap abbia un formato a 32-bit per supportare la trasparenza RGBA
            Bitmap bitmapWithAlpha = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bitmapWithAlpha))
            {
                g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            }

            // Ottieni l'icona dal Bitmap e salvala come file ICO
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                Icon icon = Icon.FromHandle(bitmapWithAlpha.GetHicon());
                icon.Save(fs);
            }

            // Libera le risorse GDI
            DestroyIcon(bitmapWithAlpha.GetHicon());
        }

        /// <summary>
        /// Funzione per distruggere l'handle dell'icona ed evitare perdite di memoria.
        /// </summary>
        /// <param name="hIcon">Handle dell'icona da distruggere.</param>
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr hIcon);

    }
}
