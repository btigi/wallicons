using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace wallicons
{
    public class WallpaperManager
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style
        {
            Centered,
            Tiled,
            Stretched,
            Fit,
            Fill,
            Span
        }

        public void SetWallpaper(string filename, Style style)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (key != null)
            {
                if (style == Style.Centered)
                {
                    key.SetValue(@"WallpaperStyle", Convert.ToString(0));
                    key.SetValue(@"TileWallpaper", Convert.ToString(0));
                }
                if (style == Style.Tiled)
                {
                    key.SetValue(@"WallpaperStyle", Convert.ToString(1));
                    key.SetValue(@"TileWallpaper", Convert.ToString(1));
                }
                if (style == Style.Stretched)
                {
                    key.SetValue(@"WallpaperStyle", Convert.ToString(2));
                    key.SetValue(@"TileWallpaper", Convert.ToString(0));
                }
                if (style == Style.Fit)
                {
                    key.SetValue(@"WallpaperStyle", Convert.ToString(3));
                    key.SetValue(@"TileWallpaper", Convert.ToString(0));
                }
                if (style == Style.Fill)
                {
                    key.SetValue(@"WallpaperStyle", Convert.ToString(4));
                    key.SetValue(@"TileWallpaper", Convert.ToString(0));
                }
                if (style == Style.Span)
                {
                    key.SetValue(@"WallpaperStyle", Convert.ToString(5));
                    key.SetValue(@"TileWallpaper", Convert.ToString(0));
                }

                _ = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filename, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
        }
    }
}