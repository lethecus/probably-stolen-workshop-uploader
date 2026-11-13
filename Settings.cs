using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PSWorkshopUploader
{
	internal sealed class AppSettings
	{
		public string SteamCmdPath { get; set; } = "";
		public string ModsFolder { get; set; } = "";
		public string SteamUser { get; set; } = "";
		public string Tags { get; set; } = "Mod, Quality of Life";
		public int Visibility { get; set; } = 2;
		public bool VisibilityChosen { get; set; } = false;
		public string BackupFolder { get; set; } = "";
		public bool MoveAfterUpload { get; set; } = false;
		public string Language { get; set; } = "";
		public Dictionary<string, string> PublishedIds { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		private static string SettingsPath
		{
			get
			{
				string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PSWorkshopUploader");
				Directory.CreateDirectory(dir);
				return Path.Combine(dir, "settings.json");
			}
		}

		public static AppSettings Load()
		{
			try
			{
				if (File.Exists(SettingsPath))
				{
					AppSettings loaded = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsPath));
					if (loaded != null)
					{
						loaded.PublishedIds ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
						return loaded;
					}
				}
			}
			catch
			{
				// A broken settings file must never stop the tool from starting.
			}
			return new AppSettings();
		}

		public void Save()
		{
			try
			{
				File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
			}
			catch
			{
				// Ignore: settings are a convenience, not a requirement.
			}
		}
	}

	internal sealed class ModInfo
	{
		public string Folder { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public string Version { get; set; }
		public string Author { get; set; }
		public string Description { get; set; }

		public string Display => $"{Name}  v{Version}   [{Id}]";

		public override string ToString() => Display;
	}
}
