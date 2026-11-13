# Probably Stolen - Workshop Uploader

A small standalone Windows tool that publishes local mod folders to the Steam Workshop
(App ID **4348910**) using SteamCMD.

## Where it is

- Ready-to-run program: `dist\PSWorkshopUploader.exe` (self-contained, no .NET install needed)
- Source: `WorkshopUploader.csproj`, `Program.cs`, `UploaderForm.cs`, `Settings.cs`

Rebuild:

```
dotnet publish WorkshopUploader.csproj -c Release -r win-x64 --self-contained true ^
  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o dist
```

## What it does

1. **SteamCMD** - finds `steamcmd.exe` (PATH, `C:\steamcmd`, `D:\steamcmd`, Steam folder) or offers
   a one-click download from Valve. If it is missing, it tells you and links the official zip.
2. **Mods folder** - remembers the last one and auto-detects it from Steam's `libraryfolders.vdf`.
3. **Mod list** - every subfolder with a `manifest.xml`, showing name / version / ID.
4. **Publish** - for each ticked mod it:
   - copies the folder into a staging area,
   - uses `preview.png` / `preview.jpg` / `icon.png` if present, otherwise draws a simple placeholder,
   - writes `workshop_build.vdf` (App ID, title, description, tags, visibility, item ID),
   - runs `steamcmd +login <user> +workshop_build_item "<vdf>" +quit`,
   - reads the published file ID from SteamCMD's output and remembers it for future updates.
5. **Backup** - zips the local mod folder (timestamped) into the backups folder and writes
   `<mod id>_workshop_id.txt` next to it.
6. **Move (optional)** - if "move the local mod folder into the backups folder" is ticked, the
   folder is moved out of `Mods` after a successful upload (the zip is made first).

Buttons along the bottom:

- **Sign in to SteamCMD** - one-time console sign-in (password + Steam Guard).
- **Publish selected** - uploads the ticked mods.
- **My Workshop** - opens `https://steamcommunity.com/id/<your name>/myworkshopfiles/`.
  `/id/` is a fixed part of that address - only the name decides which page opens.
  A full profile URL or a 17-digit SteamID64 works too.
- **Help** - a step-by-step explanation of every field (localized).
- **Workshop** - the game's Workshop hub.
- **Mods folder** - opens the mods folder in Explorer.

Visibility starts as **Private** (2) and only remembers another value once you change it
explicitly. **Save preferences** stores the current fields; they are also saved on close.

## Languages

The interface is translated into every locale the game ships: English, Deutsch, Français,
Español, Italiano, Polski, Português (Brasil), Русский, 日本語, 한국어, 简体中文.
The language selector sits at the bottom of the upload settings; on first start the tool picks
the Windows language when it is supported, otherwise English. The choice is saved.

Use **Dry run** first: it stages the files and writes the VDF without uploading, so you can
inspect exactly what would be published.

## First-time checklist

1. Close Probably Stolen (the game locks DLLs, and moving folders works best when it is off).
2. Fix your `manifest.xml` files first - the tool reads them:
   - `<Name>`, `<ModVersion>`, `<Author>`, `<Description>`
   - `<GameVersions><Version>1.0.0.14</Version></GameVersions>`
   - every DLL listed under `<Assemblies>`, and `<Capabilities/>` when none are needed
3. Start the tool, confirm SteamCMD and the Mods folder are detected.
4. Enter your Steam user name, pick tags and visibility.
5. Click **Sign in to SteamCMD** once. A console opens: type your password / Steam Guard code
   there. SteamCMD caches the session (`ssfn*` files), so later uploads normally do not ask again -
   unless Steam decides the session is stale or the IP changed.
6. Tick the mods, run **Dry run** to check the staging output, then publish for real.
7. When the item is listed: subscribe to it, remove the local copy from `Mods`
   (the zip backup is your safety net), restart the game.

## Notes

- Uploading requires a Steam account that owns Probably Stolen.
- Tags must be ones the game's workshop uses (`Mod`, `Quality of Life`, `UI`, `Gameplay`,
  `Utilities`, `Items`, `Content`, `Cosmetic`, `Translation`, `Music`, `Audio`, `Tools`, ...).
  Tags, preview and description can all be edited later on the item's Workshop page.
- Steam processes new items asynchronously; the page may take a minute to appear.
- Settings and remembered item IDs: `%APPDATA%\PSWorkshopUploader\settings.json`
- Staging area: `%APPDATA%\PSWorkshopUploader\staging\<mod id>\`
- Default backups folder: `Documents\Probably Stolen Mod Backups`

## Safety

The tool never deletes a mod. The zip backup is always created before the optional move, and the
move only happens after a successful upload.
