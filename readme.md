## Introduction

wallicons is console application to set the desktop walpaper, save the desktop icon locations and restore the desktop icon locations, intended to allow per wallpaper custom desktop icon layouts on Windows 8 and above.

## Download

Compiled downloads are not available.

## Compiling

To clone and run this application, you'll need [Git](https://git-scm.com) and [.NET](https://dotnet.microsoft.com/) installed on your computer. From your command line:

```
# Clone this repository
$ git clone https://github.com/btigi/asciiz

# Go into the repository
$ cd src

# Build  the app
$ dotnet build
```

## Usage

wallicons is a command line application and should be run from a terminal session. The application has several commands:

wallpaper --file [filename] --monitor [id]

Set `file` as the desktop wallpaper on the monitor specified by `monitor` parameter. Monitor ids are 1-indexed, matching the monitor ids in the Windows Display settings app. A monitor id of 0 is treated as 'full desktop' and spans all monitors.

save
--file [filename]

Save the current desktop icon layout to `file`

restore
--file [filename]

Restore the current desktop icon layout from `file`

Usage examples:

 ```wallicons wallpaper --file C:\wallpaper.jpg```

 ```wallicons save --file C:\icon-positions.json```

 ```wallicons restore --file C:\icon-positions.json```

## Licencing

wallicons is licenced under CC BY-SA 4.0 Full licence details are available in licence.md