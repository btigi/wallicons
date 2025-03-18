namespace wallicons
{
    public class WallpaperManager
    {
        public void SetWallpaper(string filename, int monitor)
        {

            // Windows displays monitors as 1-indexed, but the API is 0-indexed
            monitor = monitor - 1;

            var desktopWallpaper = (IDesktopWallpaper)new DesktopWallpaperClass();
            var monitorCount = desktopWallpaper.GetMonitorDevicePathCount();

            if (monitor == -1)
            {
                desktopWallpaper.SetPosition(DESKTOP_WALLPAPER_POSITION.DWPOS_SPAN);
                desktopWallpaper.SetWallpaper(null, filename);
                return;
            }
            else
            {
                desktopWallpaper.SetPosition(DESKTOP_WALLPAPER_POSITION.DWPOS_FILL);
                desktopWallpaper.GetMonitorDevicePathAt((uint)monitor, out var currentMonitorId);
                desktopWallpaper.SetWallpaper(currentMonitorId, filename);
            }
        }
    }
}