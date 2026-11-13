# Building PSWorkshopUploader from source

This folder contains the complete source of the program shipped in this archive.
Nothing else is executed at runtime: the app only reads your mods folder, writes staging
files, and calls SteamCMD.

## Requirements

- Windows
- .NET 8 SDK (`dotnet --list-sdks` should show 8.x or newer)

## Build

```
cd source
dotnet publish WorkshopUploader.csproj -c Release -r win-x64 --self-contained true ^
  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true ^
  -p:DebugType=none -o ..\build
```

The result is `build\PSWorkshopUploader.exe` - the same program as the one in this archive.

The shipped exe is self-contained (the .NET runtime is bundled), so a rebuild can differ
byte-for-byte depending on the exact SDK version and output paths. Compare behaviour, or
build with the same SDK version, rather than expecting an identical hash.

## Files

| File | Purpose |
| --- | --- |
| `Program.cs` | Application entry point (WinForms). |
| `UploaderForm.cs` | The whole UI and the publish/backup/move logic. |
| `Settings.cs` | Saved preferences + remembered Workshop item IDs (`%APPDATA%\PSWorkshopUploader\settings.json`). |
| `Localization.cs` | UI strings for the 11 supported languages. |
| `WorkshopUploader.csproj` | Project file (net8.0-windows, WinForms). |

## What the program does with the network and the disk

- Reads `manifest.xml` from each mod folder to fill in title/description.
- Copies a selected mod folder into `%APPDATA%\PSWorkshopUploader\staging\<mod id>\content`.
- Writes `workshop_build.vdf` next to it.
- Runs `steamcmd.exe +login <user> +workshop_build_item "<vdf>" +quit` (the only network use).
- Zips the mod folder into the chosen backups folder, optionally moves the folder there.
- Opens Steam/Workshop pages in your browser only when you click those buttons.

No telemetry, no background services, no writes outside the folders listed above.
