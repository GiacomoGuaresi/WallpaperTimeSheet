# WallpaperTimeSheet

**WallpaperTimeSheet** is a Windows C# application designed to automatically track the hours spent on your PC working on different tasks or projects. The program allows you to select the currently running task via an icon in the system tray, and generates a desktop wallpaper that shows a graphical summary of your working hours, broken down by task and day in the current month.

## Features

- **Task selection from the tray**: select the task or project you are working on directly from the icon in the system tray.
- **Automatic counting of hours**: the program automatically records the time spent on the selected task.
- **Dynamic desktop background**: WallpaperTimeSheet generates a desktop wallpaper with a summary graph of the hours spent in the current month, broken down by task and day.
- The calendar shows up to 6 weeks, similar to the Windows calendar, with each task colored according to the user configuration.
- Summary of hours spent on each task.
- Display of the currently selected task.

## Technologies used

- **C#**
- **SQLite** for saving data locally.
- **Windows API** for managing the tray icon and desktop wallpaper.

## Requirements

- Windows operating system
- .NET Framework 4.7.2 or later

## Instructions for use

1. Start the program and select the current task from the tray icon.
2. The program will automatically start counting the hours spent on that activity.
3. The desktop background will be updated in real time with the graph of the hours spent on each task.
4. You can change tasks at any time by selecting it again from the tray.

## WIP (Work in Progress)

WallpaperTimeSheet is still under development, and the following features are being implemented:

- **Installer**: an installer will be released to facilitate installation.

## Screenshot

![Image 2024-12-14 142358](https://github.com/user-attachments/assets/5af3f994-1a36-4b08-8d6f-3402df31142e)

![Image 2024-12-14 142257](https://github.com/user-attachments/assets/e9c21f79-40e8-484c-afd8-155f6743df74)

## License

GNU GENERAL PUBLIC LICENSE
