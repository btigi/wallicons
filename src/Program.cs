using System.CommandLine;
using System.Text.Json;
using wallicons;

var rootCommand = new RootCommand("wallicon");

var fileOption = new Option<string>(
    name: "--file",
    description: "The file to set as the desktop wallpaper.")
    {
        IsRequired = true
    };
var monitorOption = new Option<int>(
    name: "--monitor",
    description: "The monitor to set the wallpaper on.",
    getDefaultValue: () => 0);
var wallpaperCommand = new Command("wallpaper", "Set the desktop wallpaper") { fileOption, monitorOption };
rootCommand.AddCommand(wallpaperCommand);
wallpaperCommand.SetHandler(SetWallpaper, fileOption, monitorOption);

var saveOption = new Option<string>(
    name: "--file",
    description: "The file to save the desktop icon positions to.")
    {
        IsRequired = true
    };
var saveCommand = new Command("save", "Save the position of the desktop icons") { saveOption };
rootCommand.AddCommand(saveCommand);
saveCommand.SetHandler(SaveIconPositions, saveOption);

var restoreOption = new Option<string>(
    name: "--file",
    description: "The file to restore the desktop icon positions from.")
    {
        IsRequired = true
    };
var restoreCommand = new Command("restore", "Restore the position of the desktop icons") { restoreOption };
rootCommand.AddCommand(restoreCommand);
restoreCommand.SetHandler(RestoreIconPositions, restoreOption);

return rootCommand.InvokeAsync(args).Result;


static void SetWallpaper(string wallpaper, int monitor)
{
    Console.WriteLine($"Setting wallpaper to {wallpaper} on monitor {monitor}");
    if (!String.IsNullOrEmpty(wallpaper))
    {
        var wallpaperManager = new WallpaperManager();
        wallpaperManager.SetWallpaper(wallpaper, monitor);
    }
}

static void SaveIconPositions(string filename)
{
    var iconManager = new IconManager();

    if (!String.IsNullOrEmpty(filename))
    {
        var iconPositions = iconManager.GetIconPositions();
        var serializedIconPositions = JsonSerializer.Serialize(iconPositions);
        File.WriteAllText(filename, serializedIconPositions);
    }
}
static void RestoreIconPositions(string filename)
{
    var iconManager = new IconManager();

    if (!String.IsNullOrEmpty(filename))
    {
        var serializedIconPositions = File.ReadAllText(filename);
        var deserializedIconPositions = JsonSerializer.Deserialize<List<IconPosition>>(serializedIconPositions);
        iconManager.SetIconPositions(deserializedIconPositions);
    }
}