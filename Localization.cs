using System;
using System.Collections.Generic;
using System.Globalization;

namespace PSWorkshopUploader
{
	/// <summary>
	/// UI strings for every locale the game ships: en, de, fr, es, it, pl, ptBR, ru, ja, ko, zh.
	/// Missing keys fall back to English.
	/// </summary>
	internal static class L
	{
		public static readonly (string Code, string Name)[] Languages =
		{
			("en", "English"),
			("de", "Deutsch"),
			("fr", "Français"),
			("es", "Español"),
			("it", "Italiano"),
			("pl", "Polski"),
			("ptBR", "Português (Brasil)"),
			("ru", "Русский"),
			("ja", "日本語"),
			("ko", "한국어"),
			("zh", "简体中文")
		};

		private static string _current = "en";

		public static string Current => _current;

		public static void Set(string code)
		{
			_current = Tables.ContainsKey(code) ? code : "en";
		}

		public static string T(string key, params object[] args)
		{
			Dictionary<string, string> table = Tables[_current];
			string value;
			if (!table.TryGetValue(key, out value) && !En.TryGetValue(key, out value))
			{
				value = key;
			}
			return args != null && args.Length > 0 ? string.Format(CultureInfo.InvariantCulture, value, args) : value;
		}

		/// <summary>Detects a supported locale from the OS, used on first start.</summary>
		public static string DetectFromSystem()
		{
			string name = CultureInfo.CurrentUICulture.Name.ToLowerInvariant();
			if (name.StartsWith("pt")) return "ptBR";
			if (name.StartsWith("zh")) return "zh";
			if (name.StartsWith("ja")) return "ja";
			if (name.StartsWith("ko")) return "ko";
			if (name.StartsWith("ru")) return "ru";
			if (name.StartsWith("de")) return "de";
			if (name.StartsWith("fr")) return "fr";
			if (name.StartsWith("es")) return "es";
			if (name.StartsWith("it")) return "it";
			if (name.StartsWith("pl")) return "pl";
			return "en";
		}

		private static readonly Dictionary<string, string> En = new()
		{
			["app.title"] = "Probably Stolen - Workshop Uploader",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Mods folder",
			["grp.list"] = "3. Mods to publish",
			["grp.settings"] = "4. Upload settings",
			["grp.log"] = "5. Log",
			["steam.checking"] = "Checking...",
			["steam.found"] = "SteamCMD found: {0}",
			["steam.missing"] = "SteamCMD not found. Install it with the download button, or point the tool at steamcmd.exe.",
			["btn.locate"] = "Locate...",
			["btn.download"] = "Download SteamCMD",
			["btn.browse"] = "Browse...",
			["btn.refresh"] = "Refresh list",
			["btn.signin"] = "Sign in to SteamCMD",
			["btn.publish"] = "Publish selected",
			["btn.myworkshop"] = "My Workshop",
			["btn.help"] = "Help",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "Mods folder",
			["btn.saveprefs"] = "Save preferences",
			["btn.close"] = "Close",
			["lbl.user"] = "Steam user",
			["lbl.visibility"] = "Visibility",
			["lbl.backups"] = "Backups",
			["lbl.tags"] = "Workshop tags",
			["lbl.tagshint"] = "Comma separated, e.g. Mod, Quality of Life, UI",
			["lbl.language"] = "Language",
			["chk.dryrun"] = "Dry run (prepare the files, do not upload)",
			["chk.move"] = "After a successful upload, move the local mod folder into the backups folder (a zip backup is always made first)",
			["vis.public"] = "0 - Public",
			["vis.friends"] = "1 - Friends only",
			["vis.private"] = "2 - Private",
			["vis.unlisted"] = "3 - Unlisted",
			["log.modsMissing"] = "Mods folder not set or missing.",
			["log.foundMods"] = "Found {0} mod(s) in {1}.",
			["log.prefsSaved"] = "Preferences saved.",
			["status.prefsSaved"] = "Preferences saved.",
			["log.downloading"] = "Downloading SteamCMD from Valve...",
			["status.downloading"] = "Downloading SteamCMD...",
			["log.downloaded"] = "Downloaded {0} KB, extracting...",
			["log.prepareSteam"] = "Preparing SteamCMD (first run updates itself, this can take a few minutes)...",
			["status.prepareSteam"] = "SteamCMD first run...",
			["log.steamPrepared"] = "SteamCMD preparation finished (exit code {0}).",
			["log.signInOpened"] = "A SteamCMD window opened. Type your password (and Steam Guard code) there; it closes by itself once signed in.",
			["log.opening"] = "Opening {0}",
			["log.staged"] = "Staged {0} file(s).",
			["log.previewGenerated"] = "No preview image found; generated a simple one (you can replace it on the Workshop page).",
			["log.previewUsing"] = "Using preview image: {0}",
			["log.preparedNew"] = "Prepared a new Workshop item.",
			["log.preparedUpdate"] = "Prepared an update of item {0}.",
			["log.dryRun"] = "Dry run: skipped the upload, backup and move.",
			["status.preparing"] = "Preparing {0}...",
			["status.uploading"] = "Uploading {0}...",
			["log.item"] = "Workshop item: {0}",
			["log.keepId"] = "Upload finished; keeping the known item id {0}.",
			["log.noId"] = "Upload finished but SteamCMD did not report an item id. Check your Workshop items page and set the id next time.",
			["log.backup"] = "Backup: {0}",
			["log.moved"] = "Moved the local mod folder to {0}",
			["log.error"] = "ERROR: {0}",
			["log.failedMod"] = "FAILED: {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "Sign in",
			["msg.publishTitle"] = "Publish",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Workshop upload",
			["msg.noSteamPublish"] = "SteamCMD is not installed yet. Use 'Download SteamCMD' or 'Locate...' first.",
			["msg.noSteamSignin"] = "SteamCMD is not installed yet.",
			["msg.noUser"] = "Enter your Steam user name first.",
			["msg.noMods"] = "Tick at least one mod in the list.",
			["msg.noBackups"] = "Choose a backups folder.",
			["msg.newItems"] = "{0} of the selected mod(s) will be published as NEW Workshop items.\n\nContinue?",
			["msg.moveConfirm"] = "After each successful upload the local mod folder will be MOVED into:\n{0}\n\nA zip backup is made first. Continue?",
			["msg.steamReady"] = "SteamCMD is ready.\n\nNext: click 'Sign in to SteamCMD' once and enter your password and Steam Guard code in the window that opens.",
			["msg.steamFailed"] = "Could not set up SteamCMD:\n\n{0}\n\nYou can also download it manually from:\n{1}\nThen use 'Locate...' to select steamcmd.exe.",
			["msg.myWorkshopEmpty"] = "Enter your Steam user name first.\n\nThe address is always steamcommunity.com/id/ + your vanity name + /myworkshopfiles/.\nThe 'id' part is fixed; only the name decides the page.\nA full profile URL or a 17-digit SteamID64 also works.",
			["msg.dryDone"] = "Dry run finished: the upload files were prepared, nothing was published.",
			["msg.done"] = "Upload finished.",
			["msg.nextSteps"] = "Next steps:\n1. Subscribe to the Workshop item on Steam.\n2. Once it shows up in the game's mod list, remove the local copy\n   (a zip backup was created in the backups folder).\n3. Restart the game.",
			["line.failed"] = "- {0}: FAILED (see the log)",
			["line.item"] = "- {0}: item {1}",
			["line.prepared"] = "- {0}: prepared",
			["line.preparedNoId"] = "- {0}: prepared (no item id reported)",
			["err.auth"] = "SteamCMD could not sign in. Click 'Sign in to SteamCMD' once, then publish again.",
			["err.exit"] = "SteamCMD exited with code {0}. See the log above.",
			["err.timeout"] = "SteamCMD did not finish within 25 minutes and was closed.",
			["err.extract"] = "steamcmd.exe was not found after extracting. Extract the zip manually and use 'Locate...'.",
			["tip.steamcmd"] = "The steamcmd.exe that performs the upload. Detected automatically; use Locate or Download if it is missing.",
			["tip.locate"] = "Point the tool at an existing steamcmd.exe.",
			["tip.download"] = "Download SteamCMD from Valve and prepare it (first run updates it, which can take a few minutes).",
			["tip.mods"] = "Probably Stolen's Mods folder, usually <Steam library>\\steamapps\\common\\Probably Stolen\\Mods.",
			["tip.list"] = "Tick every mod you want to publish. The title and description are read from its manifest.xml.",
			["tip.user"] = "Your Steam account name, used by SteamCMD. For My Workshop use your profile vanity name, a full profile URL, or your 17-digit SteamID64.",
			["tip.visibility"] = "Private is the safe default for a first upload. You can make the item public on its Workshop page.",
			["tip.backups"] = "Where zip backups (and moved local mod folders) are stored.",
			["tip.tags"] = "Comma separated Workshop tags, e.g. Mod, Quality of Life, UI. Unknown tags are ignored by Steam.",
			["tip.dryrun"] = "Stages the files and writes the upload manifest without sending anything to Steam.",
			["tip.move"] = "After a successful upload, move the local mod folder into the backups folder so it does not load twice. The zip backup is made first.",
			["tip.saveprefs"] = "Store these fields now. They are also saved when the window closes.",
			["tip.myworkshop"] = "Open your Workshop items page so you can edit the published item.",
			["tip.help"] = "Step-by-step explanation of every field.",
			["tip.publish"] = "Upload the ticked mods with SteamCMD. Existing items are updated, new ones are created.",
			["tip.signin"] = "One-time SteamCMD sign-in. Type your password and Steam Guard code in the console window that opens.",
			["help.title"] = "Help - how to publish a mod",
			["help.body"] =
				"STEP 1 - STEAMCMD\n" +
				"The tool finds steamcmd.exe automatically; otherwise use Download or Locate.\n" +
				"Click \"Sign in to SteamCMD\" once and enter your password and Steam Guard code in the console.\n" +
				"SteamCMD remembers the session, so later uploads normally do not ask again.\n\n" +
				"STEP 2 - MODS FOLDER\n" +
				"Select ...\\Probably Stolen\\Mods and close the game first so the DLLs are not locked.\n\n" +
				"STEP 3 - MODS TO PUBLISH\n" +
				"Tick the mods to upload. Title and description come from manifest.xml (Name, ModVersion, Description).\n\n" +
				"STEP 4 - SETTINGS\n" +
				"Steam user: your account name for SteamCMD. \"My Workshop\" opens\n" +
				"steamcommunity.com/id/<name>/myworkshopfiles/ - \"/id/\" is a fixed part of the address;\n" +
				"only the name decides the page. A full profile URL or a 17-digit SteamID64 also works.\n" +
				"Visibility: Private by default, change it on the Workshop page later.\n" +
				"Backups: where the zip backups (and moved mod folders) are stored.\n" +
				"Tags: comma separated, e.g. Mod, Quality of Life, UI.\n" +
				"Dry run: prepares everything without uploading.\n" +
				"Move after upload: moves the local folder into the backups folder after a successful upload (zip first).\n" +
				"Save preferences: stores all fields now.\n\n" +
				"STEP 5 - PUBLISH\n" +
				"Publish selected runs steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit.\n" +
				"The first upload creates a new item and remembers its ID; later uploads update it.\n\n" +
				"STEP 6 - AFTER UPLOADING\n" +
				"1. Click \"My Workshop\" and subscribe to the item.\n" +
				"2. Set the visibility you want.\n" +
				"3. Remove the local copy from Mods (or use the move option).\n" +
				"4. Restart the game.\n\n" +
				"TROUBLESHOOTING\n" +
				"- Asked for a password again: sign in again; the session expired.\n" +
				"- No item id reported: check \"My Workshop\"; later uploads keep the known id.\n" +
				"- Upload fails while the game is open: close the game (DLLs are locked).\n" +
				"- Unknown tags are ignored by Steam; use the game's existing tags."
		};

		private static readonly Dictionary<string, string> De = new()
		{
			["app.title"] = "Probably Stolen - Workshop-Uploader",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Mods-Ordner",
			["grp.list"] = "3. Zu veröffentlichende Mods",
			["grp.settings"] = "4. Upload-Einstellungen",
			["grp.log"] = "5. Protokoll",
			["steam.checking"] = "Wird geprüft...",
			["steam.found"] = "SteamCMD gefunden: {0}",
			["steam.missing"] = "SteamCMD nicht gefunden. Installiere es über die Schaltfläche oder wähle steamcmd.exe aus.",
			["btn.locate"] = "Suchen...",
			["btn.download"] = "SteamCMD herunterladen",
			["btn.browse"] = "Durchsuchen...",
			["btn.refresh"] = "Liste aktualisieren",
			["btn.signin"] = "Bei SteamCMD anmelden",
			["btn.publish"] = "Auswahl veröffentlichen",
			["btn.myworkshop"] = "Mein Workshop",
			["btn.help"] = "Hilfe",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "Mods-Ordner",
			["btn.saveprefs"] = "Einstellungen speichern",
			["btn.close"] = "Schließen",
			["lbl.user"] = "Steam-Name",
			["lbl.visibility"] = "Sichtbarkeit",
			["lbl.backups"] = "Sicherungen",
			["lbl.tags"] = "Workshop-Tags",
			["lbl.tagshint"] = "Kommagetrennt, z. B. Mod, Quality of Life, UI",
			["lbl.language"] = "Sprache",
			["chk.dryrun"] = "Testlauf (Dateien vorbereiten, nichts hochladen)",
			["chk.move"] = "Nach erfolgreichem Upload den lokalen Mod-Ordner in den Sicherungsordner verschieben (vorher wird immer ein ZIP erstellt)",
			["vis.public"] = "0 - Öffentlich",
			["vis.friends"] = "1 - Nur Freunde",
			["vis.private"] = "2 - Privat",
			["vis.unlisted"] = "3 - Nicht gelistet",
			["log.modsMissing"] = "Mods-Ordner nicht gesetzt oder fehlt.",
			["log.foundMods"] = "{0} Mod(s) in {1} gefunden.",
			["log.prefsSaved"] = "Einstellungen gespeichert.",
			["status.prefsSaved"] = "Einstellungen gespeichert.",
			["log.downloading"] = "SteamCMD wird von Valve heruntergeladen...",
			["status.downloading"] = "SteamCMD wird heruntergeladen...",
			["log.downloaded"] = "{0} KB heruntergeladen, wird entpackt...",
			["log.prepareSteam"] = "SteamCMD wird vorbereitet (erster Start lädt Updates, das kann einige Minuten dauern)...",
			["status.prepareSteam"] = "Erster SteamCMD-Start...",
			["log.steamPrepared"] = "SteamCMD ist vorbereitet (Exit-Code {0}).",
			["log.signInOpened"] = "Ein SteamCMD-Fenster wurde geöffnet. Gib dort Passwort und Steam-Guard-Code ein; es schließt sich nach der Anmeldung selbst.",
			["log.opening"] = "Öffne {0}",
			["log.staged"] = "{0} Datei(en) vorbereitet.",
			["log.previewGenerated"] = "Kein Vorschaubild gefunden; ein einfaches wurde erzeugt (später auf der Workshop-Seite ersetzbar).",
			["log.previewUsing"] = "Vorschaubild: {0}",
			["log.preparedNew"] = "Neues Workshop-Item vorbereitet.",
			["log.preparedUpdate"] = "Aktualisierung von Item {0} vorbereitet.",
			["log.dryRun"] = "Testlauf: Upload, Sicherung und Verschieben übersprungen.",
			["status.preparing"] = "{0} wird vorbereitet...",
			["status.uploading"] = "{0} wird hochgeladen...",
			["log.item"] = "Workshop-Item: {0}",
			["log.keepId"] = "Upload beendet; bekannte Item-ID {0} bleibt erhalten.",
			["log.noId"] = "Upload beendet, aber SteamCMD hat keine Item-ID gemeldet. Prüfe deine Workshop-Seite und trage die ID beim nächsten Mal ein.",
			["log.backup"] = "Sicherung: {0}",
			["log.moved"] = "Lokaler Mod-Ordner verschoben nach {0}",
			["log.error"] = "FEHLER: {0}",
			["log.failedMod"] = "FEHLGESCHLAGEN: {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "Anmelden",
			["msg.publishTitle"] = "Veröffentlichen",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Workshop-Upload",
			["msg.noSteamPublish"] = "SteamCMD ist noch nicht installiert. Nutze zuerst 'SteamCMD herunterladen' oder 'Suchen...'.",
			["msg.noSteamSignin"] = "SteamCMD ist noch nicht installiert.",
			["msg.noUser"] = "Gib zuerst deinen Steam-Namen ein.",
			["msg.noMods"] = "Wähle mindestens einen Mod in der Liste aus.",
			["msg.noBackups"] = "Wähle einen Sicherungsordner.",
			["msg.newItems"] = "{0} der ausgewählten Mods werden als NEUE Workshop-Items veröffentlicht.\n\nFortfahren?",
			["msg.moveConfirm"] = "Nach jedem erfolgreichen Upload wird der lokale Mod-Ordner verschoben nach:\n{0}\n\nVorher wird ein ZIP erstellt. Fortfahren?",
			["msg.steamReady"] = "SteamCMD ist bereit.\n\nNächster Schritt: einmal 'Bei SteamCMD anmelden' klicken und Passwort sowie Steam-Guard-Code im Fenster eingeben.",
			["msg.steamFailed"] = "SteamCMD konnte nicht eingerichtet werden:\n\n{0}\n\nDu kannst es auch manuell herunterladen:\n{1}\nDanach mit 'Suchen...' steamcmd.exe auswählen.",
			["msg.myWorkshopEmpty"] = "Gib zuerst deinen Steam-Namen ein.\n\nDie Adresse lautet immer steamcommunity.com/id/ + dein Profilname + /myworkshopfiles/.\nDer Teil 'id' ist fest; nur der Name bestimmt die Seite.\nEine vollständige Profil-URL oder eine 17-stellige SteamID64 funktioniert ebenfalls.",
			["msg.dryDone"] = "Testlauf beendet: Die Upload-Dateien wurden vorbereitet, nichts veröffentlicht.",
			["msg.done"] = "Upload beendet.",
			["msg.nextSteps"] = "Nächste Schritte:\n1. Das Workshop-Item auf Steam abonnieren.\n2. Sobald es in der Mod-Liste des Spiels erscheint, die lokale Kopie entfernen\n   (im Sicherungsordner liegt ein ZIP).\n3. Das Spiel neu starten.",
			["line.failed"] = "- {0}: FEHLGESCHLAGEN (siehe Protokoll)",
			["line.item"] = "- {0}: Item {1}",
			["line.prepared"] = "- {0}: vorbereitet",
			["line.preparedNoId"] = "- {0}: vorbereitet (keine Item-ID gemeldet)",
			["err.auth"] = "SteamCMD konnte sich nicht anmelden. Einmal 'Bei SteamCMD anmelden' klicken und erneut veröffentlichen.",
			["err.exit"] = "SteamCMD wurde mit Code {0} beendet. Siehe Protokoll oben.",
			["err.timeout"] = "SteamCMD war nach 25 Minuten nicht fertig und wurde geschlossen.",
			["err.extract"] = "steamcmd.exe wurde nach dem Entpacken nicht gefunden. Entpacke das ZIP manuell und nutze 'Suchen...'.",
			["tip.steamcmd"] = "Die steamcmd.exe, die den Upload ausführt. Wird automatisch gesucht; sonst Suchen oder Herunterladen.",
			["tip.locate"] = "Auf eine vorhandene steamcmd.exe verweisen.",
			["tip.download"] = "SteamCMD von Valve herunterladen und vorbereiten (der erste Start lädt Updates, das kann dauern).",
			["tip.mods"] = "Der Mods-Ordner von Probably Stolen, meist <Steam-Bibliothek>\\steamapps\\common\\Probably Stolen\\Mods.",
			["tip.list"] = "Alle Mods anhaken, die veröffentlicht werden sollen. Titel und Beschreibung kommen aus manifest.xml.",
			["tip.user"] = "Dein Steam-Kontoname für SteamCMD. Für 'Mein Workshop' den Profilnamen, eine vollständige Profil-URL oder die 17-stellige SteamID64 verwenden.",
			["tip.visibility"] = "Privat ist für den ersten Upload am sichersten. Öffentlich lässt sich später auf der Workshop-Seite einstellen.",
			["tip.backups"] = "Ablage für ZIP-Sicherungen (und verschobene Mod-Ordner).",
			["tip.tags"] = "Kommagetrennte Workshop-Tags, z. B. Mod, Quality of Life, UI. Unbekannte Tags ignoriert Steam.",
			["tip.dryrun"] = "Bereitet Dateien und Upload-Datei vor, ohne etwas an Steam zu senden.",
			["tip.move"] = "Verschiebt den lokalen Mod-Ordner nach erfolgreichem Upload in die Sicherungen. Das ZIP wird vorher erstellt.",
			["tip.saveprefs"] = "Diese Felder jetzt speichern. Beim Schließen werden sie ebenfalls gespeichert.",
			["tip.myworkshop"] = "Deine Workshop-Item-Seite öffnen, um das veröffentlichte Item zu bearbeiten.",
			["tip.help"] = "Schritt-für-Schritt-Erklärung aller Felder.",
			["tip.publish"] = "Die angehakten Mods mit SteamCMD hochladen. Vorhandene Items werden aktualisiert, neue erstellt.",
			["tip.signin"] = "Einmalige Anmeldung bei SteamCMD. Passwort und Steam-Guard-Code im Konsolenfenster eingeben.",
			["help.title"] = "Hilfe - einen Mod veröffentlichen",
			["help.body"] =
				"SCHRITT 1 - STEAMCMD\n" +
				"Das Tool findet steamcmd.exe automatisch; sonst Herunterladen oder Suchen verwenden.\n" +
				"Einmal \"Bei SteamCMD anmelden\" klicken und Passwort sowie Steam-Guard-Code in der Konsole eingeben.\n" +
				"SteamCMD merkt sich die Sitzung, spätere Uploads fragen normalerweise nicht mehr.\n\n" +
				"SCHRITT 2 - MODS-ORDNER\n" +
				"...\\Probably Stolen\\Mods auswählen und das Spiel vorher schließen, damit die DLLs nicht gesperrt sind.\n\n" +
				"SCHRITT 3 - MODS ZUM VERÖFFENTLICHEN\n" +
				"Mods anhaken. Titel und Beschreibung stammen aus manifest.xml (Name, ModVersion, Description).\n\n" +
				"SCHRITT 4 - EINSTELLUNGEN\n" +
				"Steam-Name: dein Kontoname für SteamCMD. \"Mein Workshop\" öffnet\n" +
				"steamcommunity.com/id/<name>/myworkshopfiles/ - \"/id/\" ist fester Teil der Adresse;\n" +
				"nur der Name bestimmt die Seite. Eine vollständige Profil-URL oder 17-stellige SteamID64 geht auch.\n" +
				"Sichtbarkeit: standardmäßig Privat, später auf der Workshop-Seite änderbar.\n" +
				"Sicherungen: Ablage für ZIP-Sicherungen (und verschobene Mod-Ordner).\n" +
				"Tags: kommagetrennt, z. B. Mod, Quality of Life, UI.\n" +
				"Testlauf: bereitet alles vor, ohne hochzuladen.\n" +
				"Nach Upload verschieben: verschiebt den lokalen Ordner nach erfolgreichem Upload (ZIP zuerst).\n" +
				"Einstellungen speichern: speichert alle Felder sofort.\n\n" +
				"SCHRITT 5 - VERÖFFENTLICHEN\n" +
				"\"Auswahl veröffentlichen\" führt steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit aus.\n" +
				"Der erste Upload erstellt ein neues Item und merkt sich die ID; spätere Uploads aktualisieren es.\n\n" +
				"SCHRITT 6 - NACH DEM UPLOAD\n" +
				"1. \"Mein Workshop\" öffnen und das Item abonnieren.\n" +
				"2. Gewünschte Sichtbarkeit einstellen.\n" +
				"3. Lokale Kopie aus Mods entfernen (oder Verschieben nutzen).\n" +
				"4. Spiel neu starten.\n\n" +
				"FEHLERBEHEBUNG\n" +
				"- Erneut nach Passwort gefragt: noch einmal anmelden, die Sitzung ist abgelaufen.\n" +
				"- Keine Item-ID gemeldet: \"Mein Workshop\" prüfen; spätere Uploads behalten die ID.\n" +
				"- Upload scheitert bei laufendem Spiel: Spiel schließen (DLLs gesperrt).\n" +
				"- Unbekannte Tags werden von Steam ignoriert."
		};

		private static readonly Dictionary<string, string> Fr = new()
		{
			["app.title"] = "Probably Stolen - Uploader Workshop",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Dossier des mods",
			["grp.list"] = "3. Mods à publier",
			["grp.settings"] = "4. Réglages d'envoi",
			["grp.log"] = "5. Journal",
			["steam.checking"] = "Vérification...",
			["steam.found"] = "SteamCMD trouvé : {0}",
			["steam.missing"] = "SteamCMD introuvable. Installez-le avec le bouton de téléchargement ou indiquez steamcmd.exe.",
			["btn.locate"] = "Parcourir...",
			["btn.download"] = "Télécharger SteamCMD",
			["btn.browse"] = "Parcourir...",
			["btn.refresh"] = "Actualiser la liste",
			["btn.signin"] = "Se connecter à SteamCMD",
			["btn.publish"] = "Publier la sélection",
			["btn.myworkshop"] = "Mon Workshop",
			["btn.help"] = "Aide",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "Dossier des mods",
			["btn.saveprefs"] = "Enregistrer les réglages",
			["btn.close"] = "Fermer",
			["lbl.user"] = "Compte Steam",
			["lbl.visibility"] = "Visibilité",
			["lbl.backups"] = "Sauvegardes",
			["lbl.tags"] = "Tags Workshop",
			["lbl.tagshint"] = "Séparés par des virgules, ex. Mod, Quality of Life, UI",
			["lbl.language"] = "Langue",
			["chk.dryrun"] = "Simulation (préparer les fichiers, ne rien envoyer)",
			["chk.move"] = "Après un envoi réussi, déplacer le dossier local du mod dans le dossier de sauvegarde (une archive ZIP est toujours créée avant)",
			["vis.public"] = "0 - Public",
			["vis.friends"] = "1 - Amis uniquement",
			["vis.private"] = "2 - Privé",
			["vis.unlisted"] = "3 - Non répertorié",
			["log.modsMissing"] = "Dossier des mods non défini ou introuvable.",
			["log.foundMods"] = "{0} mod(s) trouvé(s) dans {1}.",
			["log.prefsSaved"] = "Réglages enregistrés.",
			["status.prefsSaved"] = "Réglages enregistrés.",
			["log.downloading"] = "Téléchargement de SteamCMD depuis Valve...",
			["status.downloading"] = "Téléchargement de SteamCMD...",
			["log.downloaded"] = "{0} Ko téléchargés, extraction...",
			["log.prepareSteam"] = "Préparation de SteamCMD (le premier lancement se met à jour, cela peut prendre quelques minutes)...",
			["status.prepareSteam"] = "Premier lancement de SteamCMD...",
			["log.steamPrepared"] = "SteamCMD est prêt (code de sortie {0}).",
			["log.signInOpened"] = "Une fenêtre SteamCMD s'est ouverte. Saisissez votre mot de passe et le code Steam Guard ; elle se ferme après la connexion.",
			["log.opening"] = "Ouverture de {0}",
			["log.staged"] = "{0} fichier(s) préparé(s).",
			["log.previewGenerated"] = "Aucune image d'aperçu trouvée ; une image simple a été générée (remplaçable sur la page Workshop).",
			["log.previewUsing"] = "Image d'aperçu : {0}",
			["log.preparedNew"] = "Nouvel élément Workshop préparé.",
			["log.preparedUpdate"] = "Mise à jour de l'élément {0} préparée.",
			["log.dryRun"] = "Simulation : envoi, sauvegarde et déplacement ignorés.",
			["status.preparing"] = "Préparation de {0}...",
			["status.uploading"] = "Envoi de {0}...",
			["log.item"] = "Élément Workshop : {0}",
			["log.keepId"] = "Envoi terminé ; identifiant connu {0} conservé.",
			["log.noId"] = "Envoi terminé mais SteamCMD n'a pas signalé d'identifiant. Vérifiez votre page Workshop et renseignez-le la prochaine fois.",
			["log.backup"] = "Sauvegarde : {0}",
			["log.moved"] = "Dossier local du mod déplacé vers {0}",
			["log.error"] = "ERREUR : {0}",
			["log.failedMod"] = "ÉCHEC : {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "Connexion",
			["msg.publishTitle"] = "Publier",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Envoi Workshop",
			["msg.noSteamPublish"] = "SteamCMD n'est pas encore installé. Utilisez d'abord 'Télécharger SteamCMD' ou 'Parcourir...'.",
			["msg.noSteamSignin"] = "SteamCMD n'est pas encore installé.",
			["msg.noUser"] = "Saisissez d'abord votre nom de compte Steam.",
			["msg.noMods"] = "Cochez au moins un mod dans la liste.",
			["msg.noBackups"] = "Choisissez un dossier de sauvegarde.",
			["msg.newItems"] = "{0} des mods sélectionnés seront publiés comme NOUVEAUX éléments Workshop.\n\nContinuer ?",
			["msg.moveConfirm"] = "Après chaque envoi réussi, le dossier local du mod sera DÉPLACÉ vers :\n{0}\n\nUne archive ZIP est créée avant. Continuer ?",
			["msg.steamReady"] = "SteamCMD est prêt.\n\nEnsuite : cliquez une fois sur 'Se connecter à SteamCMD' et saisissez votre mot de passe et le code Steam Guard dans la fenêtre.",
			["msg.steamFailed"] = "Impossible de configurer SteamCMD :\n\n{0}\n\nVous pouvez aussi le télécharger manuellement :\n{1}\nPuis utilisez 'Parcourir...' pour choisir steamcmd.exe.",
			["msg.myWorkshopEmpty"] = "Saisissez d'abord votre nom de compte Steam.\n\nL'adresse est toujours steamcommunity.com/id/ + votre nom de profil + /myworkshopfiles/.\nLa partie 'id' est fixe ; seul le nom détermine la page.\nUne URL de profil complète ou un SteamID64 à 17 chiffres fonctionne aussi.",
			["msg.dryDone"] = "Simulation terminée : les fichiers ont été préparés, rien n'a été publié.",
			["msg.done"] = "Envoi terminé.",
			["msg.nextSteps"] = "Étapes suivantes :\n1. Abonnez-vous à l'élément Workshop sur Steam.\n2. Quand il apparaît dans la liste des mods du jeu, supprimez la copie locale\n   (une archive ZIP a été créée dans le dossier de sauvegarde).\n3. Redémarrez le jeu.",
			["line.failed"] = "- {0} : ÉCHEC (voir le journal)",
			["line.item"] = "- {0} : élément {1}",
			["line.prepared"] = "- {0} : préparé",
			["line.preparedNoId"] = "- {0} : préparé (aucun identifiant signalé)",
			["err.auth"] = "SteamCMD n'a pas pu se connecter. Cliquez une fois sur 'Se connecter à SteamCMD' puis relancez l'envoi.",
			["err.exit"] = "SteamCMD s'est arrêté avec le code {0}. Voir le journal.",
			["err.timeout"] = "SteamCMD n'a pas terminé en 25 minutes et a été fermé.",
			["err.extract"] = "steamcmd.exe est introuvable après l'extraction. Extrayez le ZIP manuellement et utilisez 'Parcourir...'.",
			["tip.steamcmd"] = "Le steamcmd.exe qui effectue l'envoi. Détecté automatiquement ; utilisez Parcourir ou Télécharger sinon.",
			["tip.locate"] = "Indiquer un steamcmd.exe existant.",
			["tip.download"] = "Télécharger SteamCMD depuis Valve et le préparer (le premier lancement se met à jour, cela peut prendre du temps).",
			["tip.mods"] = "Le dossier Mods de Probably Stolen, en général <bibliothèque Steam>\\steamapps\\common\\Probably Stolen\\Mods.",
			["tip.list"] = "Cochez les mods à publier. Le titre et la description viennent de manifest.xml.",
			["tip.user"] = "Votre nom de compte Steam pour SteamCMD. Pour Mon Workshop, utilisez votre nom de profil, une URL complète ou votre SteamID64.",
			["tip.visibility"] = "Privé est le choix sûr pour un premier envoi. Vous pourrez le rendre public sur la page Workshop.",
			["tip.backups"] = "Emplacement des archives ZIP (et des dossiers de mods déplacés).",
			["tip.tags"] = "Tags Workshop séparés par des virgules, ex. Mod, Quality of Life, UI. Les tags inconnus sont ignorés.",
			["tip.dryrun"] = "Prépare les fichiers et le manifeste d'envoi sans rien envoyer à Steam.",
			["tip.move"] = "Déplace le dossier local dans les sauvegardes après un envoi réussi. L'archive ZIP est créée avant.",
			["tip.saveprefs"] = "Enregistre ces champs maintenant. Ils sont aussi sauvegardés à la fermeture.",
			["tip.myworkshop"] = "Ouvre votre page d'éléments Workshop pour modifier l'élément publié.",
			["tip.help"] = "Explication pas à pas de chaque champ.",
			["tip.publish"] = "Envoie les mods cochés avec SteamCMD. Les éléments existants sont mis à jour, les nouveaux créés.",
			["tip.signin"] = "Connexion unique à SteamCMD. Saisissez mot de passe et code Steam Guard dans la console.",
			["help.title"] = "Aide - publier un mod",
			["help.body"] =
				"ÉTAPE 1 - STEAMCMD\n" +
				"L'outil trouve steamcmd.exe automatiquement ; sinon utilisez Télécharger ou Parcourir.\n" +
				"Cliquez une fois sur \"Se connecter à SteamCMD\" et saisissez mot de passe et code Steam Guard dans la console.\n" +
				"SteamCMD retient la session, les envois suivants ne demandent normalement plus rien.\n\n" +
				"ÉTAPE 2 - DOSSIER DES MODS\n" +
				"Choisissez ...\\Probably Stolen\\Mods et fermez le jeu pour que les DLL ne soient pas verrouillées.\n\n" +
				"ÉTAPE 3 - MODS À PUBLIER\n" +
				"Cochez les mods. Le titre et la description viennent de manifest.xml (Name, ModVersion, Description).\n\n" +
				"ÉTAPE 4 - RÉGLAGES\n" +
				"Compte Steam : votre nom de compte pour SteamCMD. \"Mon Workshop\" ouvre\n" +
				"steamcommunity.com/id/<nom>/myworkshopfiles/ - \"/id/\" fait partie fixe de l'adresse ;\n" +
				"seul le nom détermine la page. Une URL de profil complète ou un SteamID64 fonctionne aussi.\n" +
				"Visibilité : Privé par défaut, modifiable ensuite sur la page Workshop.\n" +
				"Sauvegardes : emplacement des ZIP (et des dossiers déplacés).\n" +
				"Tags : séparés par des virgules, ex. Mod, Quality of Life, UI.\n" +
				"Simulation : prépare tout sans envoyer.\n" +
				"Déplacer après envoi : déplace le dossier local après un envoi réussi (ZIP d'abord).\n" +
				"Enregistrer les réglages : enregistre tous les champs immédiatement.\n\n" +
				"ÉTAPE 5 - PUBLIER\n" +
				"Publier la sélection lance steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit.\n" +
				"Le premier envoi crée un élément et retient son identifiant ; les suivants le mettent à jour.\n\n" +
				"ÉTAPE 6 - APRÈS L'ENVOI\n" +
				"1. Ouvrez \"Mon Workshop\" et abonnez-vous à l'élément.\n" +
				"2. Choisissez la visibilité voulue.\n" +
				"3. Supprimez la copie locale de Mods (ou utilisez le déplacement).\n" +
				"4. Redémarrez le jeu.\n\n" +
				"DÉPANNAGE\n" +
				"- Mot de passe redemandé : reconnectez-vous, la session a expiré.\n" +
				"- Aucun identifiant signalé : vérifiez \"Mon Workshop\" ; les envois suivants gardent l'identifiant.\n" +
				"- Échec avec le jeu ouvert : fermez le jeu (DLL verrouillées).\n" +
				"- Les tags inconnus sont ignorés par Steam."
		};

		private static readonly Dictionary<string, string> Es = new()
		{
			["app.title"] = "Probably Stolen - Uploader de Workshop",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Carpeta de mods",
			["grp.list"] = "3. Mods para publicar",
			["grp.settings"] = "4. Ajustes de subida",
			["grp.log"] = "5. Registro",
			["steam.checking"] = "Comprobando...",
			["steam.found"] = "SteamCMD encontrado: {0}",
			["steam.missing"] = "SteamCMD no encontrado. Instálalo con el botón de descarga o indica steamcmd.exe.",
			["btn.locate"] = "Buscar...",
			["btn.download"] = "Descargar SteamCMD",
			["btn.browse"] = "Examinar...",
			["btn.refresh"] = "Actualizar lista",
			["btn.signin"] = "Iniciar sesión en SteamCMD",
			["btn.publish"] = "Publicar selección",
			["btn.myworkshop"] = "Mi Workshop",
			["btn.help"] = "Ayuda",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "Carpeta de mods",
			["btn.saveprefs"] = "Guardar preferencias",
			["btn.close"] = "Cerrar",
			["lbl.user"] = "Usuario de Steam",
			["lbl.visibility"] = "Visibilidad",
			["lbl.backups"] = "Copias",
			["lbl.tags"] = "Etiquetas",
			["lbl.tagshint"] = "Separadas por comas, p. ej. Mod, Quality of Life, UI",
			["lbl.language"] = "Idioma",
			["chk.dryrun"] = "Simulación (preparar los archivos, sin subir)",
			["chk.move"] = "Tras una subida correcta, mover la carpeta local del mod a la carpeta de copias (siempre se crea antes un ZIP)",
			["vis.public"] = "0 - Público",
			["vis.friends"] = "1 - Solo amigos",
			["vis.private"] = "2 - Privado",
			["vis.unlisted"] = "3 - No listado",
			["log.modsMissing"] = "Carpeta de mods no definida o inexistente.",
			["log.foundMods"] = "Se encontraron {0} mod(s) en {1}.",
			["log.prefsSaved"] = "Preferencias guardadas.",
			["status.prefsSaved"] = "Preferencias guardadas.",
			["log.downloading"] = "Descargando SteamCMD de Valve...",
			["status.downloading"] = "Descargando SteamCMD...",
			["log.downloaded"] = "Descargados {0} KB, extrayendo...",
			["log.prepareSteam"] = "Preparando SteamCMD (la primera ejecución se actualiza, puede tardar varios minutos)...",
			["status.prepareSteam"] = "Primera ejecución de SteamCMD...",
			["log.steamPrepared"] = "SteamCMD preparado (código de salida {0}).",
			["log.signInOpened"] = "Se abrió una ventana de SteamCMD. Escribe allí tu contraseña y el código Steam Guard; se cierra sola al iniciar sesión.",
			["log.opening"] = "Abriendo {0}",
			["log.staged"] = "{0} archivo(s) preparado(s).",
			["log.previewGenerated"] = "No hay imagen de vista previa; se generó una simple (puedes cambiarla en la página de Workshop).",
			["log.previewUsing"] = "Usando vista previa: {0}",
			["log.preparedNew"] = "Nuevo elemento de Workshop preparado.",
			["log.preparedUpdate"] = "Actualización del elemento {0} preparada.",
			["log.dryRun"] = "Simulación: se omitieron la subida, la copia y el movimiento.",
			["status.preparing"] = "Preparando {0}...",
			["status.uploading"] = "Subiendo {0}...",
			["log.item"] = "Elemento de Workshop: {0}",
			["log.keepId"] = "Subida terminada; se mantiene el id conocido {0}.",
			["log.noId"] = "La subida terminó pero SteamCMD no informó de un id. Revisa tu página de Workshop y ponlo la próxima vez.",
			["log.backup"] = "Copia: {0}",
			["log.moved"] = "Carpeta local del mod movida a {0}",
			["log.error"] = "ERROR: {0}",
			["log.failedMod"] = "FALLÓ: {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "Iniciar sesión",
			["msg.publishTitle"] = "Publicar",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Subida a Workshop",
			["msg.noSteamPublish"] = "SteamCMD aún no está instalado. Usa primero 'Descargar SteamCMD' o 'Buscar...'.",
			["msg.noSteamSignin"] = "SteamCMD aún no está instalado.",
			["msg.noUser"] = "Escribe primero tu nombre de usuario de Steam.",
			["msg.noMods"] = "Marca al menos un mod de la lista.",
			["msg.noBackups"] = "Elige una carpeta de copias.",
			["msg.newItems"] = "{0} de los mods seleccionados se publicarán como NUEVOS elementos de Workshop.\n\n¿Continuar?",
			["msg.moveConfirm"] = "Tras cada subida correcta, la carpeta local del mod se MOVERÁ a:\n{0}\n\nAntes se crea un ZIP. ¿Continuar?",
			["msg.steamReady"] = "SteamCMD está listo.\n\nSiguiente: pulsa una vez 'Iniciar sesión en SteamCMD' y escribe tu contraseña y el código Steam Guard en la ventana.",
			["msg.steamFailed"] = "No se pudo preparar SteamCMD:\n\n{0}\n\nTambién puedes descargarlo manualmente:\n{1}\nLuego usa 'Buscar...' para elegir steamcmd.exe.",
			["msg.myWorkshopEmpty"] = "Escribe primero tu nombre de usuario de Steam.\n\nLa dirección siempre es steamcommunity.com/id/ + tu nombre de perfil + /myworkshopfiles/.\nLa parte 'id' es fija; solo el nombre decide la página.\nTambién sirve una URL de perfil completa o un SteamID64 de 17 dígitos.",
			["msg.dryDone"] = "Simulación terminada: se prepararon los archivos, no se publicó nada.",
			["msg.done"] = "Subida terminada.",
			["msg.nextSteps"] = "Siguientes pasos:\n1. Suscríbete al elemento de Workshop en Steam.\n2. Cuando aparezca en la lista de mods del juego, borra la copia local\n   (se creó un ZIP en la carpeta de copias).\n3. Reinicia el juego.",
			["line.failed"] = "- {0}: FALLÓ (ver registro)",
			["line.item"] = "- {0}: elemento {1}",
			["line.prepared"] = "- {0}: preparado",
			["line.preparedNoId"] = "- {0}: preparado (sin id informado)",
			["err.auth"] = "SteamCMD no pudo iniciar sesión. Pulsa una vez 'Iniciar sesión en SteamCMD' y vuelve a publicar.",
			["err.exit"] = "SteamCMD terminó con el código {0}. Ver el registro.",
			["err.timeout"] = "SteamCMD no terminó en 25 minutos y se cerró.",
			["err.extract"] = "No se encontró steamcmd.exe tras extraer. Extrae el ZIP manualmente y usa 'Buscar...'.",
			["tip.steamcmd"] = "El steamcmd.exe que hace la subida. Se detecta solo; usa Buscar o Descargar si falta.",
			["tip.locate"] = "Indicar un steamcmd.exe existente.",
			["tip.download"] = "Descargar SteamCMD de Valve y prepararlo (la primera ejecución se actualiza y puede tardar).",
			["tip.mods"] = "La carpeta Mods de Probably Stolen, normalmente <biblioteca de Steam>\\steamapps\\common\\Probably Stolen\\Mods.",
			["tip.list"] = "Marca los mods que quieras publicar. El título y la descripción salen de manifest.xml.",
			["tip.user"] = "Tu nombre de cuenta de Steam para SteamCMD. Para Mi Workshop usa tu nombre de perfil, una URL completa o tu SteamID64.",
			["tip.visibility"] = "Privado es lo seguro para la primera subida. Puedes hacerlo público en la página de Workshop.",
			["tip.backups"] = "Dónde se guardan los ZIP (y las carpetas de mods movidas).",
			["tip.tags"] = "Etiquetas separadas por comas, p. ej. Mod, Quality of Life, UI. Las desconocidas se ignoran.",
			["tip.dryrun"] = "Prepara los archivos y el manifiesto de subida sin enviar nada a Steam.",
			["tip.move"] = "Mueve la carpeta local a las copias tras una subida correcta. El ZIP se crea antes.",
			["tip.saveprefs"] = "Guarda estos campos ahora. También se guardan al cerrar.",
			["tip.myworkshop"] = "Abre tu página de elementos de Workshop para editar lo publicado.",
			["tip.help"] = "Explicación paso a paso de cada campo.",
			["tip.publish"] = "Sube los mods marcados con SteamCMD. Los existentes se actualizan y los nuevos se crean.",
			["tip.signin"] = "Inicio de sesión único en SteamCMD. Escribe contraseña y código Steam Guard en la consola.",
			["help.title"] = "Ayuda - cómo publicar un mod",
			["help.body"] =
				"PASO 1 - STEAMCMD\n" +
				"La herramienta encuentra steamcmd.exe automáticamente; si no, usa Descargar o Buscar.\n" +
				"Pulsa una vez \"Iniciar sesión en SteamCMD\" y escribe tu contraseña y el código Steam Guard en la consola.\n" +
				"SteamCMD recuerda la sesión, así que las siguientes subidas no suelen pedir nada.\n\n" +
				"PASO 2 - CARPETA DE MODS\n" +
				"Elige ...\\Probably Stolen\\Mods y cierra el juego para que las DLL no estén bloqueadas.\n\n" +
				"PASO 3 - MODS PARA PUBLICAR\n" +
				"Marca los mods. El título y la descripción vienen de manifest.xml (Name, ModVersion, Description).\n\n" +
				"PASO 4 - AJUSTES\n" +
				"Usuario de Steam: tu nombre de cuenta para SteamCMD. \"Mi Workshop\" abre\n" +
				"steamcommunity.com/id/<nombre>/myworkshopfiles/ - \"/id/\" es una parte fija de la dirección;\n" +
				"solo el nombre decide la página. También sirve una URL completa o un SteamID64 de 17 dígitos.\n" +
				"Visibilidad: Privado por defecto, cambiable luego en la página de Workshop.\n" +
				"Copias: dónde se guardan los ZIP (y las carpetas movidas).\n" +
				"Etiquetas: separadas por comas, p. ej. Mod, Quality of Life, UI.\n" +
				"Simulación: prepara todo sin subir.\n" +
				"Mover tras subir: mueve la carpeta local tras una subida correcta (ZIP primero).\n" +
				"Guardar preferencias: guarda todos los campos ahora.\n\n" +
				"PASO 5 - PUBLICAR\n" +
				"Publicar selección ejecuta steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit.\n" +
				"La primera subida crea un elemento y recuerda su ID; las siguientes lo actualizan.\n\n" +
				"PASO 6 - TRAS LA SUBIDA\n" +
				"1. Abre \"Mi Workshop\" y suscríbete al elemento.\n" +
				"2. Pon la visibilidad que quieras.\n" +
				"3. Borra la copia local de Mods (o usa la opción de mover).\n" +
				"4. Reinicia el juego.\n\n" +
				"SOLUCIÓN DE PROBLEMAS\n" +
				"- Pide contraseña otra vez: vuelve a iniciar sesión, la sesión caducó.\n" +
				"- Sin id informado: revisa \"Mi Workshop\"; las siguientes subidas conservan el id.\n" +
				"- Falla con el juego abierto: ciérralo (DLL bloqueadas).\n" +
				"- Las etiquetas desconocidas las ignora Steam."
		};

		private static readonly Dictionary<string, string> It = new()
		{
			["app.title"] = "Probably Stolen - Uploader Workshop",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Cartella mod",
			["grp.list"] = "3. Mod da pubblicare",
			["grp.settings"] = "4. Impostazioni di upload",
			["grp.log"] = "5. Registro",
			["steam.checking"] = "Verifica...",
			["steam.found"] = "SteamCMD trovato: {0}",
			["steam.missing"] = "SteamCMD non trovato. Installalo con il pulsante di download o indica steamcmd.exe.",
			["btn.locate"] = "Sfoglia...",
			["btn.download"] = "Scarica SteamCMD",
			["btn.browse"] = "Sfoglia...",
			["btn.refresh"] = "Aggiorna elenco",
			["btn.signin"] = "Accedi a SteamCMD",
			["btn.publish"] = "Pubblica selezione",
			["btn.myworkshop"] = "Il mio Workshop",
			["btn.help"] = "Aiuto",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "Cartella mod",
			["btn.saveprefs"] = "Salva preferenze",
			["btn.close"] = "Chiudi",
			["lbl.user"] = "Utente Steam",
			["lbl.visibility"] = "Visibilità",
			["lbl.backups"] = "Backup",
			["lbl.tags"] = "Tag Workshop",
			["lbl.tagshint"] = "Separati da virgole, es. Mod, Quality of Life, UI",
			["lbl.language"] = "Lingua",
			["chk.dryrun"] = "Prova (prepara i file, non caricare)",
			["chk.move"] = "Dopo un upload riuscito sposta la cartella locale del mod nella cartella backup (prima viene sempre creato uno ZIP)",
			["vis.public"] = "0 - Pubblico",
			["vis.friends"] = "1 - Solo amici",
			["vis.private"] = "2 - Privato",
			["vis.unlisted"] = "3 - Non elencato",
			["log.modsMissing"] = "Cartella mod non impostata o mancante.",
			["log.foundMods"] = "Trovati {0} mod in {1}.",
			["log.prefsSaved"] = "Preferenze salvate.",
			["status.prefsSaved"] = "Preferenze salvate.",
			["log.downloading"] = "Download di SteamCMD da Valve...",
			["status.downloading"] = "Download di SteamCMD...",
			["log.downloaded"] = "Scaricati {0} KB, estrazione...",
			["log.prepareSteam"] = "Preparazione di SteamCMD (il primo avvio si aggiorna, può richiedere qualche minuto)...",
			["status.prepareSteam"] = "Primo avvio di SteamCMD...",
			["log.steamPrepared"] = "SteamCMD pronto (codice di uscita {0}).",
			["log.signInOpened"] = "Si è aperta una finestra SteamCMD. Inserisci lì password e codice Steam Guard; si chiude da sola dopo l'accesso.",
			["log.opening"] = "Apertura di {0}",
			["log.staged"] = "{0} file preparati.",
			["log.previewGenerated"] = "Nessuna immagine di anteprima; ne è stata generata una semplice (sostituibile nella pagina Workshop).",
			["log.previewUsing"] = "Anteprima: {0}",
			["log.preparedNew"] = "Nuovo elemento Workshop preparato.",
			["log.preparedUpdate"] = "Aggiornamento dell'elemento {0} preparato.",
			["log.dryRun"] = "Prova: upload, backup e spostamento saltati.",
			["status.preparing"] = "Preparazione di {0}...",
			["status.uploading"] = "Caricamento di {0}...",
			["log.item"] = "Elemento Workshop: {0}",
			["log.keepId"] = "Upload terminato; mantenuto l'id noto {0}.",
			["log.noId"] = "Upload terminato ma SteamCMD non ha riportato un id. Controlla la pagina Workshop e inseriscilo la prossima volta.",
			["log.backup"] = "Backup: {0}",
			["log.moved"] = "Cartella locale del mod spostata in {0}",
			["log.error"] = "ERRORE: {0}",
			["log.failedMod"] = "FALLITO: {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "Accesso",
			["msg.publishTitle"] = "Pubblica",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Upload Workshop",
			["msg.noSteamPublish"] = "SteamCMD non è ancora installato. Usa prima 'Scarica SteamCMD' o 'Sfoglia...'.",
			["msg.noSteamSignin"] = "SteamCMD non è ancora installato.",
			["msg.noUser"] = "Inserisci prima il tuo nome utente Steam.",
			["msg.noMods"] = "Seleziona almeno un mod nell'elenco.",
			["msg.noBackups"] = "Scegli una cartella per i backup.",
			["msg.newItems"] = "{0} dei mod selezionati saranno pubblicati come NUOVI elementi Workshop.\n\nContinuare?",
			["msg.moveConfirm"] = "Dopo ogni upload riuscito la cartella locale del mod sarà SPOSTATA in:\n{0}\n\nPrima viene creato uno ZIP. Continuare?",
			["msg.steamReady"] = "SteamCMD è pronto.\n\nPoi: premi una volta 'Accedi a SteamCMD' e inserisci password e codice Steam Guard nella finestra.",
			["msg.steamFailed"] = "Impossibile preparare SteamCMD:\n\n{0}\n\nPuoi anche scaricarlo manualmente:\n{1}\nPoi usa 'Sfoglia...' per scegliere steamcmd.exe.",
			["msg.myWorkshopEmpty"] = "Inserisci prima il tuo nome utente Steam.\n\nL'indirizzo è sempre steamcommunity.com/id/ + il tuo nome profilo + /myworkshopfiles/.\nLa parte 'id' è fissa; solo il nome decide la pagina.\nFunziona anche un URL profilo completo o uno SteamID64 di 17 cifre.",
			["msg.dryDone"] = "Prova terminata: i file sono stati preparati, nulla è stato pubblicato.",
			["msg.done"] = "Upload terminato.",
			["msg.nextSteps"] = "Passi successivi:\n1. Iscriviti all'elemento Workshop su Steam.\n2. Quando compare nell'elenco mod del gioco, rimuovi la copia locale\n   (uno ZIP è stato creato nella cartella backup).\n3. Riavvia il gioco.",
			["line.failed"] = "- {0}: FALLITO (vedi registro)",
			["line.item"] = "- {0}: elemento {1}",
			["line.prepared"] = "- {0}: preparato",
			["line.preparedNoId"] = "- {0}: preparato (nessun id riportato)",
			["err.auth"] = "SteamCMD non è riuscito ad accedere. Premi una volta 'Accedi a SteamCMD' e ripubblica.",
			["err.exit"] = "SteamCMD è terminato con codice {0}. Vedi il registro.",
			["err.timeout"] = "SteamCMD non ha finito in 25 minuti ed è stato chiuso.",
			["err.extract"] = "steamcmd.exe non trovato dopo l'estrazione. Estrai lo ZIP manualmente e usa 'Sfoglia...'.",
			["tip.steamcmd"] = "Il steamcmd.exe che esegue l'upload. Rilevato automaticamente; usa Sfoglia o Scarica se manca.",
			["tip.locate"] = "Indica un steamcmd.exe esistente.",
			["tip.download"] = "Scarica SteamCMD da Valve e lo prepara (il primo avvio si aggiorna, può richiedere tempo).",
			["tip.mods"] = "La cartella Mods di Probably Stolen, di solito <libreria Steam>\\steamapps\\common\\Probably Stolen\\Mods.",
			["tip.list"] = "Seleziona i mod da pubblicare. Titolo e descrizione vengono da manifest.xml.",
			["tip.user"] = "Il tuo nome account Steam per SteamCMD. Per Il mio Workshop usa il nome profilo, un URL completo o lo SteamID64.",
			["tip.visibility"] = "Privato è la scelta sicura per il primo upload. Puoi renderlo pubblico nella pagina Workshop.",
			["tip.backups"] = "Dove vengono salvati gli ZIP (e le cartelle mod spostate).",
			["tip.tags"] = "Tag separati da virgole, es. Mod, Quality of Life, UI. Quelli sconosciuti sono ignorati.",
			["tip.dryrun"] = "Prepara file e manifesto di upload senza inviare nulla a Steam.",
			["tip.move"] = "Sposta la cartella locale nei backup dopo un upload riuscito. Lo ZIP viene creato prima.",
			["tip.saveprefs"] = "Salva questi campi ora. Vengono salvati anche alla chiusura.",
			["tip.myworkshop"] = "Apre la pagina dei tuoi elementi Workshop per modificare quello pubblicato.",
			["tip.help"] = "Spiegazione passo passo di ogni campo.",
			["tip.publish"] = "Carica i mod selezionati con SteamCMD. Gli elementi esistenti sono aggiornati, i nuovi creati.",
			["tip.signin"] = "Accesso una tantum a SteamCMD. Inserisci password e codice Steam Guard nella console.",
			["help.title"] = "Aiuto - come pubblicare un mod",
			["help.body"] =
				"PASSO 1 - STEAMCMD\n" +
				"Il tool trova steamcmd.exe automaticamente; altrimenti usa Scarica o Sfoglia.\n" +
				"Premi una volta \"Accedi a SteamCMD\" e inserisci password e codice Steam Guard nella console.\n" +
				"SteamCMD ricorda la sessione, i prossimi upload di solito non chiedono più nulla.\n\n" +
				"PASSO 2 - CARTELLA MOD\n" +
				"Scegli ...\\Probably Stolen\\Mods e chiudi il gioco per non bloccare le DLL.\n\n" +
				"PASSO 3 - MOD DA PUBBLICARE\n" +
				"Seleziona i mod. Titolo e descrizione vengono da manifest.xml (Name, ModVersion, Description).\n\n" +
				"PASSO 4 - IMPOSTAZIONI\n" +
				"Utente Steam: il tuo nome account per SteamCMD. \"Il mio Workshop\" apre\n" +
				"steamcommunity.com/id/<nome>/myworkshopfiles/ - \"/id/\" è una parte fissa dell'indirizzo;\n" +
				"solo il nome decide la pagina. Funziona anche un URL completo o uno SteamID64.\n" +
				"Visibilità: Privato per impostazione predefinita, modificabile nella pagina Workshop.\n" +
				"Backup: dove vengono salvati gli ZIP (e le cartelle spostate).\n" +
				"Tag: separati da virgole, es. Mod, Quality of Life, UI.\n" +
				"Prova: prepara tutto senza caricare.\n" +
				"Sposta dopo l'upload: sposta la cartella locale dopo un upload riuscito (ZIP prima).\n" +
				"Salva preferenze: salva subito tutti i campi.\n\n" +
				"PASSO 5 - PUBBLICA\n" +
				"Pubblica selezione esegue steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit.\n" +
				"Il primo upload crea un elemento e ne ricorda l'ID; i successivi lo aggiornano.\n\n" +
				"PASSO 6 - DOPO L'UPLOAD\n" +
				"1. Apri \"Il mio Workshop\" e iscriviti all'elemento.\n" +
				"2. Imposta la visibilità desiderata.\n" +
				"3. Rimuovi la copia locale da Mods (o usa lo spostamento).\n" +
				"4. Riavvia il gioco.\n\n" +
				"RISOLUZIONE PROBLEMI\n" +
				"- Chiede di nuovo la password: accedi di nuovo, la sessione è scaduta.\n" +
				"- Nessun id riportato: controlla \"Il mio Workshop\"; gli upload successivi mantengono l'id.\n" +
				"- Fallisce con il gioco aperto: chiudi il gioco (DLL bloccate).\n" +
				"- I tag sconosciuti sono ignorati da Steam."
		};

		private static readonly Dictionary<string, string> Pl = new()
		{
			["app.title"] = "Probably Stolen - Uploader Workshop",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Folder modów",
			["grp.list"] = "3. Mody do publikacji",
			["grp.settings"] = "4. Ustawienia wysyłania",
			["grp.log"] = "5. Log",
			["steam.checking"] = "Sprawdzanie...",
			["steam.found"] = "Znaleziono SteamCMD: {0}",
			["steam.missing"] = "Nie znaleziono SteamCMD. Zainstaluj je przyciskiem pobierania lub wskaż steamcmd.exe.",
			["btn.locate"] = "Wybierz...",
			["btn.download"] = "Pobierz SteamCMD",
			["btn.browse"] = "Przeglądaj...",
			["btn.refresh"] = "Odśwież listę",
			["btn.signin"] = "Zaloguj do SteamCMD",
			["btn.publish"] = "Publikuj wybrane",
			["btn.myworkshop"] = "Mój Workshop",
			["btn.help"] = "Pomoc",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "Folder modów",
			["btn.saveprefs"] = "Zapisz ustawienia",
			["btn.close"] = "Zamknij",
			["lbl.user"] = "Użytkownik Steam",
			["lbl.visibility"] = "Widoczność",
			["lbl.backups"] = "Kopie",
			["lbl.tags"] = "Tagi Workshop",
			["lbl.tagshint"] = "Oddzielone przecinkami, np. Mod, Quality of Life, UI",
			["lbl.language"] = "Język",
			["chk.dryrun"] = "Próba (przygotuj pliki, nie wysyłaj)",
			["chk.move"] = "Po udanym wysłaniu przenieś lokalny folder moda do folderu kopii (najpierw zawsze powstaje ZIP)",
			["vis.public"] = "0 - Publiczny",
			["vis.friends"] = "1 - Tylko znajomi",
			["vis.private"] = "2 - Prywatny",
			["vis.unlisted"] = "3 - Niepubliczny",
			["log.modsMissing"] = "Folder modów nie ustawiony lub nie istnieje.",
			["log.foundMods"] = "Znaleziono {0} modów w {1}.",
			["log.prefsSaved"] = "Ustawienia zapisane.",
			["status.prefsSaved"] = "Ustawienia zapisane.",
			["log.downloading"] = "Pobieranie SteamCMD od Valve...",
			["status.downloading"] = "Pobieranie SteamCMD...",
			["log.downloaded"] = "Pobrano {0} KB, rozpakowywanie...",
			["log.prepareSteam"] = "Przygotowanie SteamCMD (pierwsze uruchomienie się aktualizuje, może potrwać kilka minut)...",
			["status.prepareSteam"] = "Pierwsze uruchomienie SteamCMD...",
			["log.steamPrepared"] = "SteamCMD gotowe (kod wyjścia {0}).",
			["log.signInOpened"] = "Otwarto okno SteamCMD. Wpisz tam hasło i kod Steam Guard; zamknie się samo po zalogowaniu.",
			["log.opening"] = "Otwieranie {0}",
			["log.staged"] = "Przygotowano plików: {0}.",
			["log.previewGenerated"] = "Brak obrazu podglądu; wygenerowano prosty (można zmienić na stronie Workshop).",
			["log.previewUsing"] = "Podgląd: {0}",
			["log.preparedNew"] = "Przygotowano nowy element Workshop.",
			["log.preparedUpdate"] = "Przygotowano aktualizację elementu {0}.",
			["log.dryRun"] = "Próba: pominięto wysyłanie, kopię i przenoszenie.",
			["status.preparing"] = "Przygotowanie {0}...",
			["status.uploading"] = "Wysyłanie {0}...",
			["log.item"] = "Element Workshop: {0}",
			["log.keepId"] = "Wysyłanie zakończone; zachowano znane id {0}.",
			["log.noId"] = "Wysyłanie zakończone, ale SteamCMD nie podało id. Sprawdź stronę Workshop i wpisz je następnym razem.",
			["log.backup"] = "Kopia: {0}",
			["log.moved"] = "Przeniesiono lokalny folder moda do {0}",
			["log.error"] = "BŁĄD: {0}",
			["log.failedMod"] = "NIEUDANE: {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "Logowanie",
			["msg.publishTitle"] = "Publikacja",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Wysyłanie do Workshop",
			["msg.noSteamPublish"] = "SteamCMD nie jest zainstalowane. Użyj najpierw 'Pobierz SteamCMD' lub 'Wybierz...'.",
			["msg.noSteamSignin"] = "SteamCMD nie jest jeszcze zainstalowane.",
			["msg.noUser"] = "Najpierw wpisz nazwę konta Steam.",
			["msg.noMods"] = "Zaznacz co najmniej jeden mod na liście.",
			["msg.noBackups"] = "Wybierz folder kopii.",
			["msg.newItems"] = "{0} z wybranych modów zostanie opublikowanych jako NOWE elementy Workshop.\n\nKontynuować?",
			["msg.moveConfirm"] = "Po każdym udanym wysłaniu lokalny folder moda zostanie PRZENIESIONY do:\n{0}\n\nNajpierw powstaje ZIP. Kontynuować?",
			["msg.steamReady"] = "SteamCMD jest gotowe.\n\nDalej: kliknij raz 'Zaloguj do SteamCMD' i wpisz hasło oraz kod Steam Guard w oknie.",
			["msg.steamFailed"] = "Nie udało się przygotować SteamCMD:\n\n{0}\n\nMożesz też pobrać je ręcznie:\n{1}\nPotem użyj 'Wybierz...', aby wskazać steamcmd.exe.",
			["msg.myWorkshopEmpty"] = "Najpierw wpisz nazwę konta Steam.\n\nAdres to zawsze steamcommunity.com/id/ + nazwa profilu + /myworkshopfiles/.\nCzęść 'id' jest stała; tylko nazwa decyduje o stronie.\nDziała też pełny URL profilu lub 17-cyfrowy SteamID64.",
			["msg.dryDone"] = "Próba zakończona: pliki przygotowano, nic nie opublikowano.",
			["msg.done"] = "Wysyłanie zakończone.",
			["msg.nextSteps"] = "Następne kroki:\n1. Subskrybuj element Workshop na Steam.\n2. Gdy pojawi się na liście modów gry, usuń lokalną kopię\n   (ZIP jest w folderze kopii).\n3. Uruchom grę ponownie.",
			["line.failed"] = "- {0}: NIEUDANE (zobacz log)",
			["line.item"] = "- {0}: element {1}",
			["line.prepared"] = "- {0}: przygotowano",
			["line.preparedNoId"] = "- {0}: przygotowano (brak id)",
			["err.auth"] = "SteamCMD nie mogło się zalogować. Kliknij raz 'Zaloguj do SteamCMD' i opublikuj ponownie.",
			["err.exit"] = "SteamCMD zakończyło się kodem {0}. Zobacz log.",
			["err.timeout"] = "SteamCMD nie zakończyło pracy w 25 minut i zostało zamknięte.",
			["err.extract"] = "Nie znaleziono steamcmd.exe po rozpakowaniu. Rozpakuj ZIP ręcznie i użyj 'Wybierz...'.",
			["tip.steamcmd"] = "steamcmd.exe, które wykonuje wysyłanie. Wykrywane automatycznie; użyj Wybierz lub Pobierz, jeśli brak.",
			["tip.locate"] = "Wskaż istniejące steamcmd.exe.",
			["tip.download"] = "Pobierz SteamCMD od Valve i przygotuj je (pierwsze uruchomienie się aktualizuje, może potrwać).",
			["tip.mods"] = "Folder Mods gry Probably Stolen, zwykle <biblioteka Steam>\\steamapps\\common\\Probably Stolen\\Mods.",
			["tip.list"] = "Zaznacz mody do publikacji. Tytuł i opis pochodzą z manifest.xml.",
			["tip.user"] = "Nazwa konta Steam dla SteamCMD. Do Mojego Workshop użyj nazwy profilu, pełnego URL lub SteamID64.",
			["tip.visibility"] = "Prywatny to bezpieczny wybór na pierwsze wysłanie. Publiczny ustawisz na stronie Workshop.",
			["tip.backups"] = "Gdzie trafiają ZIP-y (i przeniesione foldery modów).",
			["tip.tags"] = "Tagi oddzielone przecinkami, np. Mod, Quality of Life, UI. Nieznane są ignorowane.",
			["tip.dryrun"] = "Przygotowuje pliki i manifest wysyłania, nic nie wysyłając do Steam.",
			["tip.move"] = "Przenosi lokalny folder do kopii po udanym wysłaniu. ZIP powstaje wcześniej.",
			["tip.saveprefs"] = "Zapisz te pola teraz. Są też zapisywane przy zamykaniu.",
			["tip.myworkshop"] = "Otwiera stronę twoich elementów Workshop, aby edytować opublikowany.",
			["tip.help"] = "Objaśnienie każdego pola krok po kroku.",
			["tip.publish"] = "Wysyła zaznaczone mody przez SteamCMD. Istniejące są aktualizowane, nowe tworzone.",
			["tip.signin"] = "Jednorazowe logowanie do SteamCMD. Wpisz hasło i kod Steam Guard w konsoli.",
			["help.title"] = "Pomoc - jak opublikować mod",
			["help.body"] =
				"KROK 1 - STEAMCMD\n" +
				"Narzędzie samo znajdzie steamcmd.exe; inaczej użyj Pobierz lub Wybierz.\n" +
				"Kliknij raz \"Zaloguj do SteamCMD\" i wpisz hasło oraz kod Steam Guard w konsoli.\n" +
				"SteamCMD pamięta sesję, więc kolejne wysyłania zwykle nie pytają ponownie.\n\n" +
				"KROK 2 - FOLDER MODÓW\n" +
				"Wybierz ...\\Probably Stolen\\Mods i zamknij grę, aby DLL nie były zablokowane.\n\n" +
				"KROK 3 - MODY DO PUBLIKACJI\n" +
				"Zaznacz mody. Tytuł i opis pochodzą z manifest.xml (Name, ModVersion, Description).\n\n" +
				"KROK 4 - USTAWIENIA\n" +
				"Użytkownik Steam: nazwa konta dla SteamCMD. \"Mój Workshop\" otwiera\n" +
				"steamcommunity.com/id/<nazwa>/myworkshopfiles/ - \"/id/\" to stała część adresu;\n" +
				"tylko nazwa decyduje o stronie. Działa też pełny URL lub SteamID64.\n" +
				"Widoczność: domyślnie Prywatny, zmienisz na stronie Workshop.\n" +
				"Kopie: gdzie zapisywane są ZIP-y (i przeniesione foldery).\n" +
				"Tagi: oddzielone przecinkami, np. Mod, Quality of Life, UI.\n" +
				"Próba: przygotowuje wszystko bez wysyłania.\n" +
				"Przenieś po wysłaniu: przenosi lokalny folder po udanym wysłaniu (najpierw ZIP).\n" +
				"Zapisz ustawienia: zapisuje wszystkie pola od razu.\n\n" +
				"KROK 5 - PUBLIKACJA\n" +
				"Publikuj wybrane uruchamia steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit.\n" +
				"Pierwsze wysłanie tworzy element i zapamiętuje ID; kolejne go aktualizują.\n\n" +
				"KROK 6 - PO WYSŁANIU\n" +
				"1. Otwórz \"Mój Workshop\" i subskrybuj element.\n" +
				"2. Ustaw widoczność.\n" +
				"3. Usuń lokalną kopię z Mods (lub użyj przenoszenia).\n" +
				"4. Uruchom grę ponownie.\n\n" +
				"ROZWIĄZYWANIE PROBLEMÓW\n" +
				"- Znów pyta o hasło: zaloguj się ponownie, sesja wygasła.\n" +
				"- Brak id: sprawdź \"Mój Workshop\"; kolejne wysyłania zachowają id.\n" +
				"- Błąd przy otwartej grze: zamknij grę (DLL zablokowane).\n" +
				"- Nieznane tagi Steam ignoruje."
		};

		private static readonly Dictionary<string, string> PtBr = new()
		{
			["app.title"] = "Probably Stolen - Uploader da Workshop",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Pasta de mods",
			["grp.list"] = "3. Mods para publicar",
			["grp.settings"] = "4. Configurações de envio",
			["grp.log"] = "5. Registro",
			["steam.checking"] = "Verificando...",
			["steam.found"] = "SteamCMD encontrado: {0}",
			["steam.missing"] = "SteamCMD não encontrado. Instale pelo botão de download ou indique o steamcmd.exe.",
			["btn.locate"] = "Procurar...",
			["btn.download"] = "Baixar SteamCMD",
			["btn.browse"] = "Procurar...",
			["btn.refresh"] = "Atualizar lista",
			["btn.signin"] = "Entrar no SteamCMD",
			["btn.publish"] = "Publicar seleção",
			["btn.myworkshop"] = "Minha Workshop",
			["btn.help"] = "Ajuda",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "Pasta de mods",
			["btn.saveprefs"] = "Salvar preferências",
			["btn.close"] = "Fechar",
			["lbl.user"] = "Usuário Steam",
			["lbl.visibility"] = "Visibilidade",
			["lbl.backups"] = "Backups",
			["lbl.tags"] = "Tags da Workshop",
			["lbl.tagshint"] = "Separadas por vírgula, ex. Mod, Quality of Life, UI",
			["lbl.language"] = "Idioma",
			["chk.dryrun"] = "Simulação (preparar os arquivos, sem enviar)",
			["chk.move"] = "Após um envio bem-sucedido, mover a pasta local do mod para a pasta de backups (um ZIP é sempre criado antes)",
			["vis.public"] = "0 - Público",
			["vis.friends"] = "1 - Apenas amigos",
			["vis.private"] = "2 - Privado",
			["vis.unlisted"] = "3 - Não listado",
			["log.modsMissing"] = "Pasta de mods não definida ou inexistente.",
			["log.foundMods"] = "Encontrados {0} mod(s) em {1}.",
			["log.prefsSaved"] = "Preferências salvas.",
			["status.prefsSaved"] = "Preferências salvas.",
			["log.downloading"] = "Baixando SteamCMD da Valve...",
			["status.downloading"] = "Baixando SteamCMD...",
			["log.downloaded"] = "Baixados {0} KB, extraindo...",
			["log.prepareSteam"] = "Preparando o SteamCMD (a primeira execução se atualiza, pode levar alguns minutos)...",
			["status.prepareSteam"] = "Primeira execução do SteamCMD...",
			["log.steamPrepared"] = "SteamCMD pronto (código de saída {0}).",
			["log.signInOpened"] = "Abriu uma janela do SteamCMD. Digite a senha e o código Steam Guard; ela fecha sozinha após entrar.",
			["log.opening"] = "Abrindo {0}",
			["log.staged"] = "{0} arquivo(s) preparado(s).",
			["log.previewGenerated"] = "Nenhuma imagem de prévia encontrada; uma simples foi gerada (pode trocar na página da Workshop).",
			["log.previewUsing"] = "Usando prévia: {0}",
			["log.preparedNew"] = "Novo item da Workshop preparado.",
			["log.preparedUpdate"] = "Atualização do item {0} preparada.",
			["log.dryRun"] = "Simulação: envio, backup e movimentação ignorados.",
			["status.preparing"] = "Preparando {0}...",
			["status.uploading"] = "Enviando {0}...",
			["log.item"] = "Item da Workshop: {0}",
			["log.keepId"] = "Envio concluído; mantendo o id conhecido {0}.",
			["log.noId"] = "O envio terminou, mas o SteamCMD não informou um id. Verifique sua página da Workshop e informe-o na próxima vez.",
			["log.backup"] = "Backup: {0}",
			["log.moved"] = "Pasta local do mod movida para {0}",
			["log.error"] = "ERRO: {0}",
			["log.failedMod"] = "FALHOU: {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "Entrar",
			["msg.publishTitle"] = "Publicar",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Envio para a Workshop",
			["msg.noSteamPublish"] = "O SteamCMD ainda não está instalado. Use 'Baixar SteamCMD' ou 'Procurar...' primeiro.",
			["msg.noSteamSignin"] = "O SteamCMD ainda não está instalado.",
			["msg.noUser"] = "Digite primeiro seu nome de usuário Steam.",
			["msg.noMods"] = "Marque pelo menos um mod na lista.",
			["msg.noBackups"] = "Escolha uma pasta de backups.",
			["msg.newItems"] = "{0} dos mods selecionados serão publicados como NOVOS itens da Workshop.\n\nContinuar?",
			["msg.moveConfirm"] = "Após cada envio bem-sucedido, a pasta local do mod será MOVIDA para:\n{0}\n\nUm ZIP é criado antes. Continuar?",
			["msg.steamReady"] = "O SteamCMD está pronto.\n\nA seguir: clique uma vez em 'Entrar no SteamCMD' e digite a senha e o código Steam Guard na janela.",
			["msg.steamFailed"] = "Não foi possível preparar o SteamCMD:\n\n{0}\n\nVocê também pode baixá-lo manualmente:\n{1}\nDepois use 'Procurar...' para escolher steamcmd.exe.",
			["msg.myWorkshopEmpty"] = "Digite primeiro seu nome de usuário Steam.\n\nO endereço é sempre steamcommunity.com/id/ + seu nome de perfil + /myworkshopfiles/.\nA parte 'id' é fixa; só o nome decide a página.\nUma URL de perfil completa ou um SteamID64 de 17 dígitos também funciona.",
			["msg.dryDone"] = "Simulação concluída: os arquivos foram preparados, nada foi publicado.",
			["msg.done"] = "Envio concluído.",
			["msg.nextSteps"] = "Próximos passos:\n1. Inscreva-se no item da Workshop no Steam.\n2. Quando ele aparecer na lista de mods do jogo, remova a cópia local\n   (um ZIP foi criado na pasta de backups).\n3. Reinicie o jogo.",
			["line.failed"] = "- {0}: FALHOU (ver registro)",
			["line.item"] = "- {0}: item {1}",
			["line.prepared"] = "- {0}: preparado",
			["line.preparedNoId"] = "- {0}: preparado (sem id informado)",
			["err.auth"] = "O SteamCMD não conseguiu entrar. Clique uma vez em 'Entrar no SteamCMD' e publique de novo.",
			["err.exit"] = "O SteamCMD terminou com código {0}. Veja o registro.",
			["err.timeout"] = "O SteamCMD não terminou em 25 minutos e foi fechado.",
			["err.extract"] = "steamcmd.exe não encontrado após extrair. Extraia o ZIP manualmente e use 'Procurar...'.",
			["tip.steamcmd"] = "O steamcmd.exe que faz o envio. Detectado automaticamente; use Procurar ou Baixar se faltar.",
			["tip.locate"] = "Indicar um steamcmd.exe existente.",
			["tip.download"] = "Baixar o SteamCMD da Valve e prepará-lo (a primeira execução se atualiza, pode demorar).",
			["tip.mods"] = "A pasta Mods do Probably Stolen, geralmente <biblioteca Steam>\\steamapps\\common\\Probably Stolen\\Mods.",
			["tip.list"] = "Marque os mods a publicar. Título e descrição vêm do manifest.xml.",
			["tip.user"] = "Seu nome de conta Steam para o SteamCMD. Para Minha Workshop use o nome de perfil, uma URL completa ou o SteamID64.",
			["tip.visibility"] = "Privado é o padrão seguro para o primeiro envio. Você pode torná-lo público na página da Workshop.",
			["tip.backups"] = "Onde ficam os ZIPs (e as pastas de mods movidas).",
			["tip.tags"] = "Tags separadas por vírgula, ex. Mod, Quality of Life, UI. Tags desconhecidas são ignoradas.",
			["tip.dryrun"] = "Prepara os arquivos e o manifesto de envio sem mandar nada ao Steam.",
			["tip.move"] = "Move a pasta local para os backups após um envio bem-sucedido. O ZIP é criado antes.",
			["tip.saveprefs"] = "Salva estes campos agora. Também são salvos ao fechar.",
			["tip.myworkshop"] = "Abre a página dos seus itens da Workshop para editar o publicado.",
			["tip.help"] = "Explicação passo a passo de cada campo.",
			["tip.publish"] = "Envia os mods marcados com o SteamCMD. Itens existentes são atualizados, novos são criados.",
			["tip.signin"] = "Login único no SteamCMD. Digite a senha e o código Steam Guard no console.",
			["help.title"] = "Ajuda - como publicar um mod",
			["help.body"] =
				"PASSO 1 - STEAMCMD\n" +
				"A ferramenta encontra o steamcmd.exe automaticamente; senão use Baixar ou Procurar.\n" +
				"Clique uma vez em \"Entrar no SteamCMD\" e digite a senha e o código Steam Guard no console.\n" +
				"O SteamCMD lembra a sessão, então os próximos envios normalmente não pedem nada.\n\n" +
				"PASSO 2 - PASTA DE MODS\n" +
				"Escolha ...\\Probably Stolen\\Mods e feche o jogo para as DLLs não ficarem bloqueadas.\n\n" +
				"PASSO 3 - MODS PARA PUBLICAR\n" +
				"Marque os mods. Título e descrição vêm do manifest.xml (Name, ModVersion, Description).\n\n" +
				"PASSO 4 - CONFIGURAÇÕES\n" +
				"Usuário Steam: seu nome de conta para o SteamCMD. \"Minha Workshop\" abre\n" +
				"steamcommunity.com/id/<nome>/myworkshopfiles/ - \"/id/\" é parte fixa do endereço;\n" +
				"só o nome decide a página. Uma URL completa ou um SteamID64 também funciona.\n" +
				"Visibilidade: Privado por padrão, alterável na página da Workshop.\n" +
				"Backups: onde ficam os ZIPs (e as pastas movidas).\n" +
				"Tags: separadas por vírgula, ex. Mod, Quality of Life, UI.\n" +
				"Simulação: prepara tudo sem enviar.\n" +
				"Mover após envio: move a pasta local após um envio bem-sucedido (ZIP primeiro).\n" +
				"Salvar preferências: salva todos os campos agora.\n\n" +
				"PASSO 5 - PUBLICAR\n" +
				"Publicar seleção executa steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit.\n" +
				"O primeiro envio cria um item e guarda o ID; os seguintes o atualizam.\n\n" +
				"PASSO 6 - APÓS O ENVIO\n" +
				"1. Abra \"Minha Workshop\" e inscreva-se no item.\n" +
				"2. Defina a visibilidade desejada.\n" +
				"3. Remova a cópia local de Mods (ou use a opção de mover).\n" +
				"4. Reinicie o jogo.\n\n" +
				"SOLUÇÃO DE PROBLEMAS\n" +
				"- Pediu senha de novo: entre novamente, a sessão expirou.\n" +
				"- Sem id informado: verifique \"Minha Workshop\"; os próximos envios mantêm o id.\n" +
				"- Falha com o jogo aberto: feche o jogo (DLLs bloqueadas).\n" +
				"- Tags desconhecidas são ignoradas pelo Steam."
		};

		private static readonly Dictionary<string, string> Ru = new()
		{
			["app.title"] = "Probably Stolen - загрузка в Workshop",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Папка модов",
			["grp.list"] = "3. Моды для публикации",
			["grp.settings"] = "4. Настройки загрузки",
			["grp.log"] = "5. Журнал",
			["steam.checking"] = "Проверка...",
			["steam.found"] = "SteamCMD найден: {0}",
			["steam.missing"] = "SteamCMD не найден. Установите его кнопкой загрузки или укажите steamcmd.exe.",
			["btn.locate"] = "Выбрать...",
			["btn.download"] = "Скачать SteamCMD",
			["btn.browse"] = "Обзор...",
			["btn.refresh"] = "Обновить список",
			["btn.signin"] = "Войти в SteamCMD",
			["btn.publish"] = "Опубликовать выбранное",
			["btn.myworkshop"] = "Мой Workshop",
			["btn.help"] = "Справка",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "Папка модов",
			["btn.saveprefs"] = "Сохранить настройки",
			["btn.close"] = "Закрыть",
			["lbl.user"] = "Пользователь Steam",
			["lbl.visibility"] = "Видимость",
			["lbl.backups"] = "Резервные копии",
			["lbl.tags"] = "Теги Workshop",
			["lbl.tagshint"] = "Через запятую, напр. Mod, Quality of Life, UI",
			["lbl.language"] = "Язык",
			["chk.dryrun"] = "Пробный запуск (подготовить файлы, не загружать)",
			["chk.move"] = "После успешной загрузки переместить локальную папку мода в папку резервных копий (ZIP создаётся всегда заранее)",
			["vis.public"] = "0 - Публичный",
			["vis.friends"] = "1 - Только друзья",
			["vis.private"] = "2 - Приватный",
			["vis.unlisted"] = "3 - Скрытый",
			["log.modsMissing"] = "Папка модов не задана или не существует.",
			["log.foundMods"] = "Найдено модов: {0} в {1}.",
			["log.prefsSaved"] = "Настройки сохранены.",
			["status.prefsSaved"] = "Настройки сохранены.",
			["log.downloading"] = "Загрузка SteamCMD с серверов Valve...",
			["status.downloading"] = "Загрузка SteamCMD...",
			["log.downloaded"] = "Загружено {0} КБ, распаковка...",
			["log.prepareSteam"] = "Подготовка SteamCMD (при первом запуске он обновляется, это может занять несколько минут)...",
			["status.prepareSteam"] = "Первый запуск SteamCMD...",
			["log.steamPrepared"] = "SteamCMD готов (код выхода {0}).",
			["log.signInOpened"] = "Открылось окно SteamCMD. Введите там пароль и код Steam Guard; оно закроется само после входа.",
			["log.opening"] = "Открытие {0}",
			["log.staged"] = "Подготовлено файлов: {0}.",
			["log.previewGenerated"] = "Изображение предпросмотра не найдено; создано простое (можно заменить на странице Workshop).",
			["log.previewUsing"] = "Предпросмотр: {0}",
			["log.preparedNew"] = "Подготовлен новый элемент Workshop.",
			["log.preparedUpdate"] = "Подготовлено обновление элемента {0}.",
			["log.dryRun"] = "Пробный запуск: загрузка, копия и перемещение пропущены.",
			["status.preparing"] = "Подготовка {0}...",
			["status.uploading"] = "Загрузка {0}...",
			["log.item"] = "Элемент Workshop: {0}",
			["log.keepId"] = "Загрузка завершена; сохранён известный id {0}.",
			["log.noId"] = "Загрузка завершена, но SteamCMD не сообщил id. Проверьте страницу Workshop и укажите его в следующий раз.",
			["log.backup"] = "Резервная копия: {0}",
			["log.moved"] = "Локальная папка мода перемещена в {0}",
			["log.error"] = "ОШИБКА: {0}",
			["log.failedMod"] = "НЕУДАЧА: {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "Вход",
			["msg.publishTitle"] = "Публикация",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Загрузка в Workshop",
			["msg.noSteamPublish"] = "SteamCMD ещё не установлен. Сначала нажмите 'Скачать SteamCMD' или 'Выбрать...'.",
			["msg.noSteamSignin"] = "SteamCMD ещё не установлен.",
			["msg.noUser"] = "Сначала введите имя учётной записи Steam.",
			["msg.noMods"] = "Отметьте хотя бы один мод в списке.",
			["msg.noBackups"] = "Выберите папку для резервных копий.",
			["msg.newItems"] = "{0} из выбранных модов будут опубликованы как НОВЫЕ элементы Workshop.\n\nПродолжить?",
			["msg.moveConfirm"] = "После каждой успешной загрузки локальная папка мода будет ПЕРЕМЕЩЕНА в:\n{0}\n\nСначала создаётся ZIP. Продолжить?",
			["msg.steamReady"] = "SteamCMD готов.\n\nДалее: нажмите один раз 'Войти в SteamCMD' и введите пароль и код Steam Guard в открывшемся окне.",
			["msg.steamFailed"] = "Не удалось подготовить SteamCMD:\n\n{0}\n\nМожно скачать вручную:\n{1}\nЗатем нажмите 'Выбрать...' и укажите steamcmd.exe.",
			["msg.myWorkshopEmpty"] = "Сначала введите имя учётной записи Steam.\n\nАдрес всегда steamcommunity.com/id/ + ваше имя профиля + /myworkshopfiles/.\nЧасть 'id' фиксирована; страницу определяет только имя.\nПолный URL профиля или 17-значный SteamID64 тоже подойдёт.",
			["msg.dryDone"] = "Пробный запуск завершён: файлы подготовлены, ничего не опубликовано.",
			["msg.done"] = "Загрузка завершена.",
			["msg.nextSteps"] = "Далее:\n1. Подпишитесь на элемент Workshop в Steam.\n2. Когда он появится в списке модов игры, удалите локальную копию\n   (ZIP создан в папке резервных копий).\n3. Перезапустите игру.",
			["line.failed"] = "- {0}: НЕУДАЧА (см. журнал)",
			["line.item"] = "- {0}: элемент {1}",
			["line.prepared"] = "- {0}: подготовлено",
			["line.preparedNoId"] = "- {0}: подготовлено (id не сообщён)",
			["err.auth"] = "SteamCMD не смог войти. Нажмите один раз 'Войти в SteamCMD' и повторите публикацию.",
			["err.exit"] = "SteamCMD завершился с кодом {0}. См. журнал.",
			["err.timeout"] = "SteamCMD не завершился за 25 минут и был закрыт.",
			["err.extract"] = "steamcmd.exe не найден после распаковки. Распакуйте ZIP вручную и нажмите 'Выбрать...'.",
			["tip.steamcmd"] = "steamcmd.exe, выполняющий загрузку. Определяется автоматически; если нет — Выбрать или Скачать.",
			["tip.locate"] = "Указать существующий steamcmd.exe.",
			["tip.download"] = "Скачать SteamCMD от Valve и подготовить (первый запуск обновляется, может занять время).",
			["tip.mods"] = "Папка Mods игры Probably Stolen, обычно <библиотека Steam>\\steamapps\\common\\Probably Stolen\\Mods.",
			["tip.list"] = "Отметьте моды для публикации. Название и описание берутся из manifest.xml.",
			["tip.user"] = "Имя учётной записи Steam для SteamCMD. Для 'Мой Workshop' укажите имя профиля, полный URL или SteamID64.",
			["tip.visibility"] = "Приватный — безопасный вариант для первой загрузки. Публичный можно включить на странице Workshop.",
			["tip.backups"] = "Куда сохраняются ZIP-копии (и перемещённые папки модов).",
			["tip.tags"] = "Теги через запятую, напр. Mod, Quality of Life, UI. Неизвестные Steam игнорирует.",
			["tip.dryrun"] = "Готовит файлы и манифест загрузки, ничего не отправляя в Steam.",
			["tip.move"] = "Перемещает локальную папку в резервные копии после успешной загрузки. ZIP создаётся раньше.",
			["tip.saveprefs"] = "Сохранить эти поля сейчас. Они также сохраняются при закрытии.",
			["tip.myworkshop"] = "Открывает страницу ваших элементов Workshop для редактирования опубликованного.",
			["tip.help"] = "Пошаговое описание каждого поля.",
			["tip.publish"] = "Загружает отмеченные моды через SteamCMD. Существующие обновляются, новые создаются.",
			["tip.signin"] = "Однократный вход в SteamCMD. Введите пароль и код Steam Guard в консоли.",
			["help.title"] = "Справка - как опубликовать мод",
			["help.body"] =
				"ШАГ 1 - STEAMCMD\n" +
				"Программа находит steamcmd.exe сама; иначе используйте Скачать или Выбрать.\n" +
				"Нажмите один раз \"Войти в SteamCMD\" и введите пароль и код Steam Guard в консоли.\n" +
				"SteamCMD запоминает сеанс, поэтому следующие загрузки обычно не спрашивают снова.\n\n" +
				"ШАГ 2 - ПАПКА МОДОВ\n" +
				"Выберите ...\\Probably Stolen\\Mods и закройте игру, чтобы DLL не были заблокированы.\n\n" +
				"ШАГ 3 - МОДЫ ДЛЯ ПУБЛИКАЦИИ\n" +
				"Отметьте моды. Название и описание берутся из manifest.xml (Name, ModVersion, Description).\n\n" +
				"ШАГ 4 - НАСТРОЙКИ\n" +
				"Пользователь Steam: имя учётной записи для SteamCMD. \"Мой Workshop\" открывает\n" +
				"steamcommunity.com/id/<имя>/myworkshopfiles/ - \"/id/\" это фиксированная часть адреса;\n" +
				"страницу определяет только имя. Полный URL профиля или SteamID64 тоже подойдёт.\n" +
				"Видимость: по умолчанию Приватный, меняется на странице Workshop.\n" +
				"Резервные копии: куда сохраняются ZIP (и перемещённые папки).\n" +
				"Теги: через запятую, напр. Mod, Quality of Life, UI.\n" +
				"Пробный запуск: готовит всё без загрузки.\n" +
				"Переместить после загрузки: переносит локальную папку после успешной загрузки (ZIP заранее).\n" +
				"Сохранить настройки: сохраняет все поля сразу.\n\n" +
				"ШАГ 5 - ПУБЛИКАЦИЯ\n" +
				"Опубликовать выбранное выполняет steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit.\n" +
				"Первая загрузка создаёт элемент и запоминает его ID; следующие его обновляют.\n\n" +
				"ШАГ 6 - ПОСЛЕ ЗАГРУЗКИ\n" +
				"1. Откройте \"Мой Workshop\" и подпишитесь на элемент.\n" +
				"2. Установите нужную видимость.\n" +
				"3. Удалите локальную копию из Mods (или используйте перемещение).\n" +
				"4. Перезапустите игру.\n\n" +
				"РЕШЕНИЕ ПРОБЛЕМ\n" +
				"- Снова просит пароль: войдите заново, сеанс истёк.\n" +
				"- Нет id: проверьте \"Мой Workshop\"; следующие загрузки сохранят id.\n" +
				"- Ошибка при открытой игре: закройте игру (DLL заблокированы).\n" +
				"- Неизвестные теги Steam игнорирует."
		};

		private static readonly Dictionary<string, string> Ja = new()
		{
			["app.title"] = "Probably Stolen - Workshop アップローダー",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Mods フォルダー",
			["grp.list"] = "3. 公開する Mod",
			["grp.settings"] = "4. アップロード設定",
			["grp.log"] = "5. ログ",
			["steam.checking"] = "確認中...",
			["steam.found"] = "SteamCMD が見つかりました: {0}",
			["steam.missing"] = "SteamCMD が見つかりません。ダウンロードボタンで導入するか、steamcmd.exe を指定してください。",
			["btn.locate"] = "参照...",
			["btn.download"] = "SteamCMD をダウンロード",
			["btn.browse"] = "参照...",
			["btn.refresh"] = "一覧を更新",
			["btn.signin"] = "SteamCMD にサインイン",
			["btn.publish"] = "選択を公開",
			["btn.myworkshop"] = "マイ Workshop",
			["btn.help"] = "ヘルプ",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "Mods フォルダー",
			["btn.saveprefs"] = "設定を保存",
			["btn.close"] = "閉じる",
			["lbl.user"] = "Steam ユーザー",
			["lbl.visibility"] = "公開範囲",
			["lbl.backups"] = "バックアップ",
			["lbl.tags"] = "Workshop タグ",
			["lbl.tagshint"] = "カンマ区切り（例: Mod, Quality of Life, UI）",
			["lbl.language"] = "言語",
			["chk.dryrun"] = "テスト実行（ファイルを準備するだけでアップロードしない）",
			["chk.move"] = "アップロード成功後、ローカルの Mod フォルダーをバックアップ先へ移動する（ZIP は必ず先に作成）",
			["vis.public"] = "0 - 公開",
			["vis.friends"] = "1 - フレンドのみ",
			["vis.private"] = "2 - 非公開",
			["vis.unlisted"] = "3 - 非表示",
			["log.modsMissing"] = "Mods フォルダーが未設定か存在しません。",
			["log.foundMods"] = "{1} に {0} 件の Mod が見つかりました。",
			["log.prefsSaved"] = "設定を保存しました。",
			["status.prefsSaved"] = "設定を保存しました。",
			["log.downloading"] = "Valve から SteamCMD をダウンロード中...",
			["status.downloading"] = "SteamCMD をダウンロード中...",
			["log.downloaded"] = "{0} KB をダウンロードしました。展開中...",
			["log.prepareSteam"] = "SteamCMD を準備中（初回は自己更新のため数分かかることがあります）...",
			["status.prepareSteam"] = "SteamCMD 初回起動...",
			["log.steamPrepared"] = "SteamCMD の準備が完了しました（終了コード {0}）。",
			["log.signInOpened"] = "SteamCMD のウィンドウが開きました。パスワードと Steam Guard コードを入力してください。サインイン後に自動で閉じます。",
			["log.opening"] = "{0} を開いています",
			["log.staged"] = "{0} 個のファイルを準備しました。",
			["log.previewGenerated"] = "プレビュー画像がないため簡易画像を生成しました（Workshop ページで差し替え可能）。",
			["log.previewUsing"] = "プレビュー画像: {0}",
			["log.preparedNew"] = "新しい Workshop アイテムを準備しました。",
			["log.preparedUpdate"] = "アイテム {0} の更新を準備しました。",
			["log.dryRun"] = "テスト実行: アップロード・バックアップ・移動を省略しました。",
			["status.preparing"] = "{0} を準備中...",
			["status.uploading"] = "{0} をアップロード中...",
			["log.item"] = "Workshop アイテム: {0}",
			["log.keepId"] = "アップロード完了。既知の ID {0} を維持します。",
			["log.noId"] = "アップロードは完了しましたが、SteamCMD が ID を報告しませんでした。Workshop ページを確認し、次回入力してください。",
			["log.backup"] = "バックアップ: {0}",
			["log.moved"] = "ローカルの Mod フォルダーを {0} に移動しました",
			["log.error"] = "エラー: {0}",
			["log.failedMod"] = "失敗: {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "サインイン",
			["msg.publishTitle"] = "公開",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Workshop アップロード",
			["msg.noSteamPublish"] = "SteamCMD がまだインストールされていません。先に「SteamCMD をダウンロード」または「参照...」を使用してください。",
			["msg.noSteamSignin"] = "SteamCMD がまだインストールされていません。",
			["msg.noUser"] = "先に Steam ユーザー名を入力してください。",
			["msg.noMods"] = "一覧から少なくとも 1 つの Mod を選択してください。",
			["msg.noBackups"] = "バックアップ先フォルダーを選択してください。",
			["msg.newItems"] = "選択した Mod のうち {0} 件が新しい Workshop アイテムとして公開されます。\n\n続行しますか？",
			["msg.moveConfirm"] = "アップロード成功ごとに、ローカルの Mod フォルダーを次へ移動します:\n{0}\n\nZIP は先に作成されます。続行しますか？",
			["msg.steamReady"] = "SteamCMD の準備ができました。\n\n次に「SteamCMD にサインイン」を 1 回押し、開いたウィンドウでパスワードと Steam Guard コードを入力してください。",
			["msg.steamFailed"] = "SteamCMD を準備できませんでした:\n\n{0}\n\n手動でダウンロードすることもできます:\n{1}\nその後「参照...」で steamcmd.exe を選択してください。",
			["msg.myWorkshopEmpty"] = "先に Steam ユーザー名を入力してください。\n\nアドレスは常に steamcommunity.com/id/ + プロフィール名 + /myworkshopfiles/ です。\n「id」の部分は固定で、名前だけがページを決めます。\n完全なプロフィール URL や 17 桁の SteamID64 でも開けます。",
			["msg.dryDone"] = "テスト実行が完了しました。ファイルは準備され、何も公開されていません。",
			["msg.done"] = "アップロードが完了しました。",
			["msg.nextSteps"] = "次の手順:\n1. Steam で Workshop アイテムを購読します。\n2. ゲームの Mod 一覧に表示されたら、ローカルコピーを削除します\n   （バックアップ先に ZIP があります）。\n3. ゲームを再起動します。",
			["line.failed"] = "- {0}: 失敗（ログ参照）",
			["line.item"] = "- {0}: アイテム {1}",
			["line.prepared"] = "- {0}: 準備完了",
			["line.preparedNoId"] = "- {0}: 準備完了（ID 未報告）",
			["err.auth"] = "SteamCMD がサインインできませんでした。「SteamCMD にサインイン」を 1 回押してから再実行してください。",
			["err.exit"] = "SteamCMD がコード {0} で終了しました。ログを確認してください。",
			["err.timeout"] = "SteamCMD が 25 分以内に終了しなかったため閉じました。",
			["err.extract"] = "展開後に steamcmd.exe が見つかりません。ZIP を手動で展開し「参照...」を使用してください。",
			["tip.steamcmd"] = "アップロードを行う steamcmd.exe。自動検出されます。無い場合は参照またはダウンロードを使用。",
			["tip.locate"] = "既存の steamcmd.exe を指定します。",
			["tip.download"] = "Valve から SteamCMD をダウンロードして準備します（初回は更新のため時間がかかります）。",
			["tip.mods"] = "Probably Stolen の Mods フォルダー。通常は <Steam ライブラリ>\\steamapps\\common\\Probably Stolen\\Mods です。",
			["tip.list"] = "公開する Mod を選択します。タイトルと説明は manifest.xml から読み取ります。",
			["tip.user"] = "SteamCMD に使う Steam アカウント名。マイ Workshop にはプロフィール名、完全な URL、17 桁の SteamID64 を使用します。",
			["tip.visibility"] = "初回は非公開が安全です。Workshop ページで後から公開できます。",
			["tip.backups"] = "ZIP バックアップ（および移動した Mod フォルダー）の保存先。",
			["tip.tags"] = "カンマ区切りの Workshop タグ（例: Mod, Quality of Life, UI）。不明なタグは無視されます。",
			["tip.dryrun"] = "Steam に送信せず、ファイルとアップロード用ファイルだけを準備します。",
			["tip.move"] = "アップロード成功後にローカルフォルダーをバックアップへ移動します。ZIP は先に作成されます。",
			["tip.saveprefs"] = "現在の内容を保存します。ウィンドウを閉じるときにも保存されます。",
			["tip.myworkshop"] = "公開したアイテムを編集するため、Workshop アイテムページを開きます。",
			["tip.help"] = "各項目の手順付き説明。",
			["tip.publish"] = "選択した Mod を SteamCMD でアップロードします。既存は更新、新規は作成されます。",
			["tip.signin"] = "SteamCMD への 1 回限りのサインイン。コンソールにパスワードと Steam Guard コードを入力します。",
			["help.title"] = "ヘルプ - Mod の公開方法",
			["help.body"] =
				"手順 1 - STEAMCMD\n" +
				"steamcmd.exe は自動で検出されます。無い場合はダウンロードまたは参照を使用します。\n" +
				"「SteamCMD にサインイン」を 1 回押し、コンソールにパスワードと Steam Guard コードを入力します。\n" +
				"SteamCMD はセッションを記憶するため、次回以降は通常何も聞かれません。\n\n" +
				"手順 2 - MODS フォルダー\n" +
				"...\\Probably Stolen\\Mods を選び、DLL がロックされないようゲームを終了しておきます。\n\n" +
				"手順 3 - 公開する MOD\n" +
				"Mod を選択します。タイトルと説明は manifest.xml（Name、ModVersion、Description）から取得します。\n\n" +
				"手順 4 - 設定\n" +
				"Steam ユーザー: SteamCMD 用のアカウント名。「マイ Workshop」は\n" +
				"steamcommunity.com/id/<名前>/myworkshopfiles/ を開きます。「/id/」は固定部分で、\n" +
				"名前だけがページを決めます。完全な URL や 17 桁の SteamID64 も使えます。\n" +
				"公開範囲: 既定は非公開。後で Workshop ページで変更できます。\n" +
				"バックアップ: ZIP（と移動したフォルダー）の保存先。\n" +
				"タグ: カンマ区切り（例: Mod, Quality of Life, UI）。\n" +
				"テスト実行: アップロードせずに準備だけ行います。\n" +
				"アップロード後に移動: 成功後にローカルフォルダーをバックアップへ移動します（ZIP 先）。\n" +
				"設定を保存: すべての項目をすぐ保存します。\n\n" +
				"手順 5 - 公開\n" +
				"「選択を公開」は steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit を実行します。\n" +
				"初回はアイテムを作成して ID を記憶し、次回以降はそれを更新します。\n\n" +
				"手順 6 - アップロード後\n" +
				"1. 「マイ Workshop」を開いてアイテムを購読します。\n" +
				"2. 希望の公開範囲を設定します。\n" +
				"3. Mods からローカルコピーを削除します（移動オプションでも可）。\n" +
				"4. ゲームを再起動します。\n\n" +
				"トラブルシューティング\n" +
				"- パスワードを再度聞かれる: セッション切れです。再サインインしてください。\n" +
				"- ID が報告されない: 「マイ Workshop」を確認。次回以降は ID を保持します。\n" +
				"- ゲーム起動中に失敗: ゲームを終了してください（DLL ロック）。\n" +
				"- 不明なタグは Steam に無視されます。"
		};

		private static readonly Dictionary<string, string> Ko = new()
		{
			["app.title"] = "Probably Stolen - Workshop 업로더",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. 모드 폴더",
			["grp.list"] = "3. 게시할 모드",
			["grp.settings"] = "4. 업로드 설정",
			["grp.log"] = "5. 로그",
			["steam.checking"] = "확인 중...",
			["steam.found"] = "SteamCMD 찾음: {0}",
			["steam.missing"] = "SteamCMD를 찾을 수 없습니다. 다운로드 버튼으로 설치하거나 steamcmd.exe를 지정하세요.",
			["btn.locate"] = "찾아보기...",
			["btn.download"] = "SteamCMD 다운로드",
			["btn.browse"] = "찾아보기...",
			["btn.refresh"] = "목록 새로고침",
			["btn.signin"] = "SteamCMD 로그인",
			["btn.publish"] = "선택 항목 게시",
			["btn.myworkshop"] = "내 Workshop",
			["btn.help"] = "도움말",
			["btn.hub"] = "Workshop",
			["btn.modsfolder"] = "모드 폴더",
			["btn.saveprefs"] = "설정 저장",
			["btn.close"] = "닫기",
			["lbl.user"] = "Steam 사용자",
			["lbl.visibility"] = "공개 범위",
			["lbl.backups"] = "백업",
			["lbl.tags"] = "Workshop 태그",
			["lbl.tagshint"] = "쉼표로 구분, 예: Mod, Quality of Life, UI",
			["lbl.language"] = "언어",
			["chk.dryrun"] = "테스트 실행 (파일만 준비하고 업로드하지 않음)",
			["chk.move"] = "업로드 성공 후 로컬 모드 폴더를 백업 폴더로 이동 (ZIP은 항상 먼저 생성)",
			["vis.public"] = "0 - 공개",
			["vis.friends"] = "1 - 친구만",
			["vis.private"] = "2 - 비공개",
			["vis.unlisted"] = "3 - 목록 비공개",
			["log.modsMissing"] = "모드 폴더가 설정되지 않았거나 없습니다.",
			["log.foundMods"] = "{1}에서 모드 {0}개를 찾았습니다.",
			["log.prefsSaved"] = "설정이 저장되었습니다.",
			["status.prefsSaved"] = "설정이 저장되었습니다.",
			["log.downloading"] = "Valve에서 SteamCMD를 다운로드하는 중...",
			["status.downloading"] = "SteamCMD 다운로드 중...",
			["log.downloaded"] = "{0} KB 다운로드 완료, 압축 해제 중...",
			["log.prepareSteam"] = "SteamCMD 준비 중 (첫 실행은 자체 업데이트로 몇 분 걸릴 수 있습니다)...",
			["status.prepareSteam"] = "SteamCMD 첫 실행...",
			["log.steamPrepared"] = "SteamCMD 준비 완료 (종료 코드 {0}).",
			["log.signInOpened"] = "SteamCMD 창이 열렸습니다. 비밀번호와 Steam Guard 코드를 입력하세요. 로그인 후 자동으로 닫힙니다.",
			["log.opening"] = "{0} 여는 중",
			["log.staged"] = "파일 {0}개를 준비했습니다.",
			["log.previewGenerated"] = "미리보기 이미지가 없어 간단한 이미지를 생성했습니다 (Workshop 페이지에서 교체 가능).",
			["log.previewUsing"] = "미리보기 이미지: {0}",
			["log.preparedNew"] = "새 Workshop 항목을 준비했습니다.",
			["log.preparedUpdate"] = "항목 {0}의 업데이트를 준비했습니다.",
			["log.dryRun"] = "테스트 실행: 업로드, 백업, 이동을 건너뛰었습니다.",
			["status.preparing"] = "{0} 준비 중...",
			["status.uploading"] = "{0} 업로드 중...",
			["log.item"] = "Workshop 항목: {0}",
			["log.keepId"] = "업로드 완료. 기존 ID {0}을 유지합니다.",
			["log.noId"] = "업로드는 끝났지만 SteamCMD가 ID를 보고하지 않았습니다. Workshop 페이지를 확인하고 다음에 입력하세요.",
			["log.backup"] = "백업: {0}",
			["log.moved"] = "로컬 모드 폴더를 {0}으로 이동했습니다",
			["log.error"] = "오류: {0}",
			["log.failedMod"] = "실패: {0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "로그인",
			["msg.publishTitle"] = "게시",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "Workshop 업로드",
			["msg.noSteamPublish"] = "SteamCMD가 아직 설치되지 않았습니다. 먼저 'SteamCMD 다운로드' 또는 '찾아보기...'를 사용하세요.",
			["msg.noSteamSignin"] = "SteamCMD가 아직 설치되지 않았습니다.",
			["msg.noUser"] = "먼저 Steam 사용자 이름을 입력하세요.",
			["msg.noMods"] = "목록에서 모드를 하나 이상 선택하세요.",
			["msg.noBackups"] = "백업 폴더를 선택하세요.",
			["msg.newItems"] = "선택한 모드 중 {0}개가 새 Workshop 항목으로 게시됩니다.\n\n계속할까요?",
			["msg.moveConfirm"] = "업로드에 성공할 때마다 로컬 모드 폴더를 다음으로 이동합니다:\n{0}\n\nZIP은 먼저 생성됩니다. 계속할까요?",
			["msg.steamReady"] = "SteamCMD가 준비되었습니다.\n\n다음: 'SteamCMD 로그인'을 한 번 누르고 열린 창에서 비밀번호와 Steam Guard 코드를 입력하세요.",
			["msg.steamFailed"] = "SteamCMD를 준비하지 못했습니다:\n\n{0}\n\n직접 다운로드할 수도 있습니다:\n{1}\n그런 다음 '찾아보기...'로 steamcmd.exe를 선택하세요.",
			["msg.myWorkshopEmpty"] = "먼저 Steam 사용자 이름을 입력하세요.\n\n주소는 항상 steamcommunity.com/id/ + 프로필 이름 + /myworkshopfiles/ 입니다.\n'id' 부분은 고정이며 이름만 페이지를 결정합니다.\n전체 프로필 URL이나 17자리 SteamID64도 됩니다.",
			["msg.dryDone"] = "테스트 실행 완료: 파일만 준비되었고 아무것도 게시되지 않았습니다.",
			["msg.done"] = "업로드가 완료되었습니다.",
			["msg.nextSteps"] = "다음 단계:\n1. Steam에서 Workshop 항목을 구독하세요.\n2. 게임 모드 목록에 나타나면 로컬 복사본을 삭제하세요\n   (백업 폴더에 ZIP이 있습니다).\n3. 게임을 다시 시작하세요.",
			["line.failed"] = "- {0}: 실패 (로그 참고)",
			["line.item"] = "- {0}: 항목 {1}",
			["line.prepared"] = "- {0}: 준비됨",
			["line.preparedNoId"] = "- {0}: 준비됨 (ID 보고 없음)",
			["err.auth"] = "SteamCMD가 로그인하지 못했습니다. 'SteamCMD 로그인'을 한 번 누르고 다시 게시하세요.",
			["err.exit"] = "SteamCMD가 코드 {0}으로 종료되었습니다. 로그를 확인하세요.",
			["err.timeout"] = "SteamCMD가 25분 안에 끝나지 않아 종료했습니다.",
			["err.extract"] = "압축 해제 후 steamcmd.exe를 찾을 수 없습니다. ZIP을 직접 풀고 '찾아보기...'를 사용하세요.",
			["tip.steamcmd"] = "업로드를 수행하는 steamcmd.exe입니다. 자동으로 찾으며, 없으면 찾아보기나 다운로드를 사용하세요.",
			["tip.locate"] = "기존 steamcmd.exe를 지정합니다.",
			["tip.download"] = "Valve에서 SteamCMD를 받아 준비합니다 (첫 실행은 업데이트로 시간이 걸릴 수 있습니다).",
			["tip.mods"] = "Probably Stolen의 Mods 폴더, 보통 <Steam 라이브러리>\\steamapps\\common\\Probably Stolen\\Mods.",
			["tip.list"] = "게시할 모드를 선택하세요. 제목과 설명은 manifest.xml에서 읽습니다.",
			["tip.user"] = "SteamCMD에 사용할 Steam 계정 이름. 내 Workshop에는 프로필 이름, 전체 URL 또는 17자리 SteamID64를 사용하세요.",
			["tip.visibility"] = "첫 업로드에는 비공개가 안전합니다. Workshop 페이지에서 공개로 바꿀 수 있습니다.",
			["tip.backups"] = "ZIP 백업(및 이동한 모드 폴더)이 저장되는 곳입니다.",
			["tip.tags"] = "쉼표로 구분한 Workshop 태그, 예: Mod, Quality of Life, UI. 알 수 없는 태그는 무시됩니다.",
			["tip.dryrun"] = "Steam에 아무것도 보내지 않고 파일과 업로드 파일만 준비합니다.",
			["tip.move"] = "업로드 성공 후 로컬 폴더를 백업으로 이동합니다. ZIP은 먼저 생성됩니다.",
			["tip.saveprefs"] = "지금 이 항목들을 저장합니다. 창을 닫을 때도 저장됩니다.",
			["tip.myworkshop"] = "게시한 항목을 편집할 수 있도록 Workshop 항목 페이지를 엽니다.",
			["tip.help"] = "각 항목에 대한 단계별 설명.",
			["tip.publish"] = "선택한 모드를 SteamCMD로 업로드합니다. 기존 항목은 업데이트되고 새 항목은 생성됩니다.",
			["tip.signin"] = "SteamCMD 일회 로그인. 콘솔에 비밀번호와 Steam Guard 코드를 입력하세요.",
			["help.title"] = "도움말 - 모드 게시 방법",
			["help.body"] =
				"1단계 - STEAMCMD\n" +
				"steamcmd.exe는 자동으로 찾습니다. 없으면 다운로드 또는 찾아보기를 사용하세요.\n" +
				"'SteamCMD 로그인'을 한 번 누르고 콘솔에 비밀번호와 Steam Guard 코드를 입력하세요.\n" +
				"SteamCMD는 세션을 기억하므로 이후 업로드에서는 보통 다시 묻지 않습니다.\n\n" +
				"2단계 - 모드 폴더\n" +
				"...\\Probably Stolen\\Mods를 선택하고 DLL이 잠기지 않도록 게임을 먼저 종료하세요.\n\n" +
				"3단계 - 게시할 모드\n" +
				"모드를 선택하세요. 제목과 설명은 manifest.xml(Name, ModVersion, Description)에서 가져옵니다.\n\n" +
				"4단계 - 설정\n" +
				"Steam 사용자: SteamCMD용 계정 이름. '내 Workshop'은\n" +
				"steamcommunity.com/id/<이름>/myworkshopfiles/ 를 엽니다. '/id/'는 고정 부분이며\n" +
				"이름만 페이지를 결정합니다. 전체 URL이나 17자리 SteamID64도 됩니다.\n" +
				"공개 범위: 기본은 비공개이며 Workshop 페이지에서 변경할 수 있습니다.\n" +
				"백업: ZIP(및 이동한 폴더) 저장 위치.\n" +
				"태그: 쉼표로 구분, 예: Mod, Quality of Life, UI.\n" +
				"테스트 실행: 업로드 없이 준비만 합니다.\n" +
				"업로드 후 이동: 성공 후 로컬 폴더를 백업으로 이동합니다 (ZIP 먼저).\n" +
				"설정 저장: 모든 항목을 즉시 저장합니다.\n\n" +
				"5단계 - 게시\n" +
				"'선택 항목 게시'는 steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit 를 실행합니다.\n" +
				"첫 업로드는 항목을 만들고 ID를 기억하며, 이후 업로드는 이를 업데이트합니다.\n\n" +
				"6단계 - 업로드 후\n" +
				"1. '내 Workshop'을 열고 항목을 구독하세요.\n" +
				"2. 원하는 공개 범위를 설정하세요.\n" +
				"3. Mods에서 로컬 복사본을 삭제하세요 (이동 옵션 사용 가능).\n" +
				"4. 게임을 다시 시작하세요.\n\n" +
				"문제 해결\n" +
				"- 비밀번호를 다시 묻는 경우: 세션이 만료되었으니 다시 로그인하세요.\n" +
				"- ID가 보고되지 않는 경우: '내 Workshop'을 확인하세요. 이후 업로드는 ID를 유지합니다.\n" +
				"- 게임이 켜진 상태에서 실패: 게임을 종료하세요 (DLL 잠김).\n" +
				"- 알 수 없는 태그는 Steam이 무시합니다."
		};

		private static readonly Dictionary<string, string> Zh = new()
		{
			["app.title"] = "Probably Stolen - 创意工坊上传工具",
			["grp.steam"] = "1. SteamCMD",
			["grp.mods"] = "2. Mods 文件夹",
			["grp.list"] = "3. 要发布的 Mod",
			["grp.settings"] = "4. 上传设置",
			["grp.log"] = "5. 日志",
			["steam.checking"] = "正在检查...",
			["steam.found"] = "已找到 SteamCMD：{0}",
			["steam.missing"] = "未找到 SteamCMD。请用下载按钮安装，或指定 steamcmd.exe。",
			["btn.locate"] = "浏览...",
			["btn.download"] = "下载 SteamCMD",
			["btn.browse"] = "浏览...",
			["btn.refresh"] = "刷新列表",
			["btn.signin"] = "登录 SteamCMD",
			["btn.publish"] = "发布所选",
			["btn.myworkshop"] = "我的创意工坊",
			["btn.help"] = "帮助",
			["btn.hub"] = "创意工坊",
			["btn.modsfolder"] = "Mods 文件夹",
			["btn.saveprefs"] = "保存偏好",
			["btn.close"] = "关闭",
			["lbl.user"] = "Steam 用户",
			["lbl.visibility"] = "可见性",
			["lbl.backups"] = "备份",
			["lbl.tags"] = "创意工坊标签",
			["lbl.tagshint"] = "用逗号分隔，例如 Mod, Quality of Life, UI",
			["lbl.language"] = "语言",
			["chk.dryrun"] = "试运行（只准备文件，不上传）",
			["chk.move"] = "上传成功后，把本地 Mod 文件夹移动到备份文件夹（始终会先创建 ZIP）",
			["vis.public"] = "0 - 公开",
			["vis.friends"] = "1 - 仅好友",
			["vis.private"] = "2 - 私有",
			["vis.unlisted"] = "3 - 不列出",
			["log.modsMissing"] = "未设置 Mods 文件夹或文件夹不存在。",
			["log.foundMods"] = "在 {1} 中找到 {0} 个 Mod。",
			["log.prefsSaved"] = "偏好已保存。",
			["status.prefsSaved"] = "偏好已保存。",
			["log.downloading"] = "正在从 Valve 下载 SteamCMD...",
			["status.downloading"] = "正在下载 SteamCMD...",
			["log.downloaded"] = "已下载 {0} KB，正在解压...",
			["log.prepareSteam"] = "正在准备 SteamCMD（首次运行会自我更新，可能需要几分钟）...",
			["status.prepareSteam"] = "SteamCMD 首次运行...",
			["log.steamPrepared"] = "SteamCMD 准备完成（退出代码 {0}）。",
			["log.signInOpened"] = "已打开 SteamCMD 窗口。请在其中输入密码和 Steam 令牌代码；登录后会自动关闭。",
			["log.opening"] = "正在打开 {0}",
			["log.staged"] = "已准备 {0} 个文件。",
			["log.previewGenerated"] = "未找到预览图，已生成简单预览（可在创意工坊页面替换）。",
			["log.previewUsing"] = "使用预览图：{0}",
			["log.preparedNew"] = "已准备新的创意工坊项目。",
			["log.preparedUpdate"] = "已准备更新项目 {0}。",
			["log.dryRun"] = "试运行：已跳过上传、备份和移动。",
			["status.preparing"] = "正在准备 {0}...",
			["status.uploading"] = "正在上传 {0}...",
			["log.item"] = "创意工坊项目：{0}",
			["log.keepId"] = "上传完成；保留已知 ID {0}。",
			["log.noId"] = "上传完成，但 SteamCMD 未返回项目 ID。请检查创意工坊页面并在下次填写。",
			["log.backup"] = "备份：{0}",
			["log.moved"] = "已将本地 Mod 文件夹移动到 {0}",
			["log.error"] = "错误：{0}",
			["log.failedMod"] = "失败：{0} - {1}",
			["log.header"] = "=== {0} v{1} ({2}) ===",
			["msg.signinTitle"] = "登录",
			["msg.publishTitle"] = "发布",
			["msg.steamTitle"] = "SteamCMD",
			["msg.summaryTitle"] = "创意工坊上传",
			["msg.noSteamPublish"] = "尚未安装 SteamCMD。请先使用“下载 SteamCMD”或“浏览...”。",
			["msg.noSteamSignin"] = "尚未安装 SteamCMD。",
			["msg.noUser"] = "请先输入 Steam 用户名。",
			["msg.noMods"] = "请至少勾选一个 Mod。",
			["msg.noBackups"] = "请选择备份文件夹。",
			["msg.newItems"] = "所选 Mod 中有 {0} 个将作为新的创意工坊项目发布。\n\n继续？",
			["msg.moveConfirm"] = "每次成功上传后，本地 Mod 文件夹将被移动到：\n{0}\n\n会先创建 ZIP 备份。继续？",
			["msg.steamReady"] = "SteamCMD 已就绪。\n\n接下来：点击一次“登录 SteamCMD”，在打开的窗口中输入密码和 Steam 令牌代码。",
			["msg.steamFailed"] = "无法准备 SteamCMD：\n\n{0}\n\n也可以手动下载：\n{1}\n然后用“浏览...”选择 steamcmd.exe。",
			["msg.myWorkshopEmpty"] = "请先输入 Steam 用户名。\n\n地址始终是 steamcommunity.com/id/ + 你的个人资料名称 + /myworkshopfiles/。\n其中“id”是固定部分；只有名称决定打开哪个页面。\n也可以使用完整的个人资料 URL 或 17 位 SteamID64。",
			["msg.dryDone"] = "试运行完成：已准备上传文件，未发布任何内容。",
			["msg.done"] = "上传完成。",
			["msg.nextSteps"] = "后续步骤：\n1. 在 Steam 上订阅该创意工坊项目。\n2. 当它出现在游戏的 Mod 列表中后，删除本地副本\n   （备份文件夹中有 ZIP）。\n3. 重启游戏。",
			["line.failed"] = "- {0}：失败（见日志）",
			["line.item"] = "- {0}：项目 {1}",
			["line.prepared"] = "- {0}：已准备",
			["line.preparedNoId"] = "- {0}：已准备（未返回 ID）",
			["err.auth"] = "SteamCMD 无法登录。请点击一次“登录 SteamCMD”后重新发布。",
			["err.exit"] = "SteamCMD 以代码 {0} 退出。请查看日志。",
			["err.timeout"] = "SteamCMD 在 25 分钟内未完成，已关闭。",
			["err.extract"] = "解压后未找到 steamcmd.exe。请手动解压 ZIP 并使用“浏览...”。",
			["tip.steamcmd"] = "执行上传的 steamcmd.exe。会自动检测；若缺失请用浏览或下载。",
			["tip.locate"] = "指定已有的 steamcmd.exe。",
			["tip.download"] = "从 Valve 下载并准备 SteamCMD（首次运行会更新，可能需要一些时间）。",
			["tip.mods"] = "Probably Stolen 的 Mods 文件夹，通常为 <Steam 库>\\steamapps\\common\\Probably Stolen\\Mods。",
			["tip.list"] = "勾选要发布的 Mod。标题和描述来自 manifest.xml。",
			["tip.user"] = "用于 SteamCMD 的 Steam 账号名。使用“我的创意工坊”时请填个人资料名称、完整 URL 或 17 位 SteamID64。",
			["tip.visibility"] = "首次上传建议使用私有。之后可在创意工坊页面改为公开。",
			["tip.backups"] = "ZIP 备份（以及被移动的 Mod 文件夹）的存放位置。",
			["tip.tags"] = "用逗号分隔的创意工坊标签，例如 Mod, Quality of Life, UI。未知标签会被 Steam 忽略。",
			["tip.dryrun"] = "准备文件和上传清单，但不向 Steam 发送任何内容。",
			["tip.move"] = "上传成功后把本地文件夹移动到备份中。ZIP 会先创建。",
			["tip.saveprefs"] = "立即保存这些字段。关闭窗口时也会保存。",
			["tip.myworkshop"] = "打开你的创意工坊项目页面以编辑已发布内容。",
			["tip.help"] = "每个字段的分步说明。",
			["tip.publish"] = "用 SteamCMD 上传勾选的 Mod。已有项目会更新，新项目会创建。",
			["tip.signin"] = "一次性的 SteamCMD 登录。在控制台输入密码和 Steam 令牌代码。",
			["help.title"] = "帮助 - 如何发布 Mod",
			["help.body"] =
				"第 1 步 - STEAMCMD\n" +
				"工具会自动查找 steamcmd.exe；找不到时使用下载或浏览。\n" +
				"点击一次“登录 SteamCMD”，在控制台输入密码和 Steam 令牌代码。\n" +
				"SteamCMD 会记住会话，之后上传通常不再询问。\n\n" +
				"第 2 步 - MODS 文件夹\n" +
				"选择 ...\\Probably Stolen\\Mods，并先关闭游戏以免 DLL 被占用。\n\n" +
				"第 3 步 - 要发布的 MOD\n" +
				"勾选要上传的 Mod。标题和描述来自 manifest.xml（Name、ModVersion、Description）。\n\n" +
				"第 4 步 - 设置\n" +
				"Steam 用户：SteamCMD 使用的账号名。“我的创意工坊”会打开\n" +
				"steamcommunity.com/id/<名称>/myworkshopfiles/ —— “/id/”是地址的固定部分，\n" +
				"只有名称决定页面。完整的个人资料 URL 或 17 位 SteamID64 也可以。\n" +
				"可见性：默认私有，之后可在创意工坊页面修改。\n" +
				"备份：ZIP（以及移动的文件夹）的存放位置。\n" +
				"标签：用逗号分隔，例如 Mod, Quality of Life, UI。\n" +
				"试运行：只准备，不上传。\n" +
				"上传后移动：成功后将本地文件夹移动到备份（先创建 ZIP）。\n" +
				"保存偏好：立即保存所有字段。\n\n" +
				"第 5 步 - 发布\n" +
				"“发布所选”执行 steamcmd +login <user> +workshop_build_item \"<vdf>\" +quit。\n" +
				"首次上传会创建项目并记住其 ID；之后的上传会更新它。\n\n" +
				"第 6 步 - 上传之后\n" +
				"1. 打开“我的创意工坊”并订阅该项目。\n" +
				"2. 设置想要的可见性。\n" +
				"3. 从 Mods 中删除本地副本（或使用移动选项）。\n" +
				"4. 重启游戏。\n\n" +
				"故障排除\n" +
				"- 再次要求密码：会话已过期，请重新登录。\n" +
				"- 未返回 ID：检查“我的创意工坊”；之后的上传会保留该 ID。\n" +
				"- 游戏开启时上传失败：请关闭游戏（DLL 被占用）。\n" +
				"- 未知标签会被 Steam 忽略。"
		};

		private static readonly Dictionary<string, Dictionary<string, string>> Tables = new()
		{
			["en"] = En,
			["de"] = De,
			["fr"] = Fr,
			["es"] = Es,
			["it"] = It,
			["pl"] = Pl,
			["ptBR"] = PtBr,
			["ru"] = Ru,
			["ja"] = Ja,
			["ko"] = Ko,
			["zh"] = Zh
		};
	}
}
