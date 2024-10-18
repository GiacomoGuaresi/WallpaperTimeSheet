using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

public sealed class WindowsUtils
{
    const int SPI_SETDESKWALLPAPER = 20;
    const int SPIF_UPDATEINIFILE = 0x01;
    const int SPIF_SENDWININICHANGE = 0x02;
    private string filePath = "";

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out IntPtr lpdwResult);

    private const uint WM_SETTINGCHANGE = 0x001A;
    private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xFFFF);

    public enum Style : int
    {
        Fill,
        Fit,
        Span,
        Stretch,
        Tile,
        Center
    }

    public WindowsUtils(string filePath)
    {
        this.filePath = filePath;
    }

    private void SetWallpaperWithStyle(string tempPath, Style style)
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
        if (style == Style.Fill)
        {
            key.SetValue(@"WallpaperStyle", 10.ToString());
            key.SetValue(@"TileWallpaper", 0.ToString());
        }
        if (style == Style.Fit)
        {
            key.SetValue(@"WallpaperStyle", 6.ToString());
            key.SetValue(@"TileWallpaper", 0.ToString());
        }
        if (style == Style.Span) // Windows 8 or newer only!
        {
            key.SetValue(@"WallpaperStyle", 22.ToString());
            key.SetValue(@"TileWallpaper", 0.ToString());
        }
        if (style == Style.Stretch)
        {
            key.SetValue(@"WallpaperStyle", 2.ToString());
            key.SetValue(@"TileWallpaper", 0.ToString());
        }
        if (style == Style.Tile)
        {
            key.SetValue(@"WallpaperStyle", 0.ToString());
            key.SetValue(@"TileWallpaper", 1.ToString());
        }
        if (style == Style.Center)
        {
            key.SetValue(@"WallpaperStyle", 0.ToString());
            key.SetValue(@"TileWallpaper", 0.ToString());
        }

        SystemParametersInfo(SPI_SETDESKWALLPAPER,
            0,
            tempPath,
            SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
    }

    public void SetDefaultWallpaper()
    {
        SetWallpaperWithStyle(filePath, Style.Fill);
    }

    public static void SetAccentColor(int r, int g, int b)
    {
        // Calcolo del valore ARGB (Alpha, Red, Green, Blue) del colore
        int argbColor = (255 << 24) | (r << 16) | (g << 8) | b;

        // Imposta il colore di accento personalizzato
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Accent", true))
        {
            if (key != null)
            {
                key.SetValue("AccentColorMenu", argbColor, RegistryValueKind.DWord);
            }
        }

        // Disattiva la colorazione automatica per usare il colore personalizzato
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM", true))
        {
            if (key != null)
            {
                key.SetValue("AutoColorization", 0, RegistryValueKind.DWord);
            }
        }

        // Abilita il colore su Start e barra delle applicazioni
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true))
        {
            if (key != null)
            {
                key.SetValue("ColorPrevalence", 1, RegistryValueKind.DWord); // Abilita colore su Start e barra delle applicazioni
            }
        }

        // Riavvia Esplora risorse per applicare le modifiche
        RestartExplorer();
    }

    public static void EnableAutoAccentColor()
    {
        // Attiva la colorazione automatica basata sullo sfondo
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM", true))
        {
            if (key != null)
            {
                key.SetValue("AutoColorization", 1, RegistryValueKind.DWord);
            }
        }

        // Riavvia Esplora risorse per applicare le modifiche
        RestartExplorer();
    }

    public static void RestartExplorer()
    {
        // Trova e chiudi tutti i processi explorer.exe
        foreach (Process proc in Process.GetProcessesByName("explorer"))
        {
            proc.Kill();
        }

        // Riavvia explorer.exe
        Process.Start("explorer.exe");
    }


}
