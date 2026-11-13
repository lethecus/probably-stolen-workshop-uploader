using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PSWorkshopUploader
{
	internal sealed class UploaderForm : Form
	{
		private const string AppId = "4348910";
		private const string WorkshopHub = "https://steamcommunity.com/app/4348910/workshop/";
		private const string SteamCmdDownload = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";

		private static readonly Regex IdRegex = new Regex(@"(?i)(?:published\s*file\s*id|publishedfileid|publishfileid)\s*[:\-]?\s*(\d{5,})", RegexOptions.Compiled);
		private static readonly Regex PathRegex = new Regex("\"path\"\\s+\"([^\"]+)\"", RegexOptions.Compiled);

		private readonly AppSettings _settings;
		private readonly List<ModInfo> _mods = new List<ModInfo>();

		private readonly GroupBox _grpSteam = new GroupBox();
		private readonly GroupBox _grpModsFolder = new GroupBox();
		private readonly GroupBox _grpList = new GroupBox();
		private readonly GroupBox _grpSettings = new GroupBox();
		private readonly GroupBox _grpLog = new GroupBox();

		private readonly Label _lblSteamStatus = new Label();
		private readonly TextBox _txtSteamCmd = new TextBox();
		private readonly Button _btnLocateSteam = new Button();
		private readonly Button _btnDownloadSteam = new Button();

		private readonly TextBox _txtMods = new TextBox();
		private readonly Button _btnBrowseMods = new Button();
		private readonly Button _btnRefresh = new Button();

		private readonly CheckedListBox _lstMods = new CheckedListBox();

		private readonly Label _lblUser = new Label();
		private readonly Label _lblVisibility = new Label();
		private readonly Label _lblBackups = new Label();
		private readonly Label _lblTags = new Label();
		private readonly Label _lblTagsHint = new Label();
		private readonly Label _lblLanguage = new Label();
		private readonly TextBox _txtUser = new TextBox();
		private readonly ComboBox _cboVisibility = new ComboBox();
		private readonly ComboBox _cboLanguage = new ComboBox();
		private readonly TextBox _txtBackups = new TextBox();
		private readonly Button _btnBrowseBackups = new Button();
		private readonly TextBox _txtTags = new TextBox();
		private readonly CheckBox _chkMove = new CheckBox();
		private readonly CheckBox _chkDryRun = new CheckBox();
		private readonly Button _btnSavePrefs = new Button();

		private readonly RichTextBox _txtLog = new RichTextBox();
		private readonly Label _lblProgress = new Label();
		private readonly Button _btnSignIn = new Button();
		private readonly Button _btnPublish = new Button();
		private readonly Button _btnHub = new Button();
		private readonly Button _btnOpenMods = new Button();
		private readonly Button _btnMyWorkshop = new Button();
		private readonly Button _btnHelp = new Button();
		private readonly ToolTip _tips = new ToolTip();
		private readonly System.Windows.Forms.Timer _flashTimer = new System.Windows.Forms.Timer();

		private string _lastParsedId = "";
		private bool _authProblem;
		private bool _initializing = true;
		private bool _visibilityTouched;

		private string StagingRoot => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PSWorkshopUploader", "staging");

		public UploaderForm()
		{
			_settings = AppSettings.Load();
			L.Set(string.IsNullOrEmpty(_settings.Language) ? L.DetectFromSystem() : _settings.Language);
			BuildUi();

			_txtSteamCmd.Text = _settings.SteamCmdPath;
			_txtMods.Text = _settings.ModsFolder;
			_txtUser.Text = _settings.SteamUser;
			_txtTags.Text = _settings.Tags;
			_txtBackups.Text = _settings.BackupFolder;
			_chkMove.Checked = _settings.MoveAfterUpload;
			_cboVisibility.SelectedIndex = _settings.VisibilityChosen ? Math.Clamp(_settings.Visibility, 0, 3) : 2;
			_visibilityTouched = _settings.VisibilityChosen;

			if (_txtBackups.Text.Length == 0)
			{
				_txtBackups.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Probably Stolen Mod Backups");
			}

			ApplyLanguage();
			_initializing = false;

			Shown += (s, e) =>
			{
				if (_txtSteamCmd.Text.Length == 0)
				{
					string found = DetectSteamCmd();
					if (found.Length > 0)
					{
						_txtSteamCmd.Text = found;
					}
				}
				UpdateSteamStatus();
				if (_txtMods.Text.Length == 0)
				{
					string mods = DetectModsFolder();
					if (mods.Length > 0)
					{
						_txtMods.Text = mods;
					}
				}
				RefreshMods();
			};
		}

		// ---------------------------------------------------------------- UI

		private void BuildUi()
		{
			ClientSize = new Size(1020, 812);
			MinimumSize = new Size(900, 760);
			StartPosition = FormStartPosition.CenterScreen;
			Font = new Font("Segoe UI", 9f);

			_grpSteam.SetBounds(12, 10, 996, 92);
			_grpSteam.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			_lblSteamStatus.SetBounds(14, 22, 960, 20);
			_lblSteamStatus.Text = "Checking...";
			_txtSteamCmd.SetBounds(14, 48, 700, 24);
			_txtSteamCmd.ReadOnly = true;
			_btnLocateSteam.SetBounds(722, 46, 100, 28);
			_btnLocateSteam.Click += BtnLocateSteam_Click;
			_btnDownloadSteam.SetBounds(828, 46, 152, 28);
			_btnDownloadSteam.Click += BtnDownloadSteam_Click;
			_grpSteam.Controls.AddRange(new Control[] { _lblSteamStatus, _txtSteamCmd, _btnLocateSteam, _btnDownloadSteam });

			_grpModsFolder.SetBounds(12, 110, 996, 72);
			_grpModsFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			_txtMods.SetBounds(14, 30, 700, 24);
			_btnBrowseMods.SetBounds(722, 28, 120, 28);
			_btnBrowseMods.Click += BtnBrowseMods_Click;
			_btnRefresh.SetBounds(848, 28, 132, 28);
			_btnRefresh.Click += (s, e) => RefreshMods();
			_grpModsFolder.Controls.AddRange(new Control[] { _txtMods, _btnBrowseMods, _btnRefresh });

			_grpList.SetBounds(12, 190, 996, 196);
			_grpList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			_lstMods.SetBounds(14, 24, 966, 156);
			_lstMods.CheckOnClick = true;
			_lstMods.IntegralHeight = false;
			_grpList.Controls.Add(_lstMods);

			_grpSettings.SetBounds(12, 394, 996, 182);
			_grpSettings.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			_lblUser.SetBounds(14, 26, 88, 20);
			_txtUser.SetBounds(106, 24, 200, 24);
			_lblVisibility.SetBounds(326, 26, 64, 20);
			_cboVisibility.SetBounds(394, 24, 150, 24);
			_cboVisibility.DropDownStyle = ComboBoxStyle.DropDownList;
			// Placeholders; ApplyLanguage() fills in the localized labels.
			_cboVisibility.Items.AddRange(new object[] { "0", "1", "2", "3" });
			_cboVisibility.SelectedIndexChanged += (s, e) =>
			{
				if (!_initializing)
				{
					_visibilityTouched = true;
				}
			};
			_lblBackups.SetBounds(566, 26, 60, 20);
			_txtBackups.SetBounds(626, 24, 240, 24);
			_btnBrowseBackups.SetBounds(872, 22, 108, 28);
			_btnBrowseBackups.Click += BtnBrowseBackups_Click;

			_lblTags.SetBounds(14, 58, 88, 20);
			_txtTags.SetBounds(106, 56, 520, 24);
			_lblTagsHint.SetBounds(634, 58, 340, 20);
			_lblTagsHint.ForeColor = SystemColors.GrayText;

			_chkDryRun.SetBounds(14, 88, 380, 22);
			_chkMove.SetBounds(14, 118, 840, 22);

			_btnSavePrefs.SetBounds(870, 84, 112, 28);
			_btnSavePrefs.Click += (s, e) => SavePreferences();

			_lblLanguage.SetBounds(14, 148, 88, 20);
			_cboLanguage.SetBounds(106, 146, 220, 24);
			_cboLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
			foreach ((string code, string name) in L.Languages)
			{
				_cboLanguage.Items.Add(name);
			}
			_cboLanguage.SelectedIndexChanged += (s, e) =>
			{
				if (_initializing || _cboLanguage.SelectedIndex < 0)
				{
					return;
				}
				L.Set(L.Languages[_cboLanguage.SelectedIndex].Code);
				ApplyLanguage();
				SaveSettingsFromUi();
			};

			_grpSettings.Controls.AddRange(new Control[] { _lblUser, _txtUser, _lblVisibility, _cboVisibility, _lblBackups, _txtBackups, _btnBrowseBackups, _lblTags, _txtTags, _lblTagsHint, _chkDryRun, _chkMove, _btnSavePrefs, _lblLanguage, _cboLanguage });

			_grpLog.SetBounds(12, 584, 996, 166);
			_grpLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			_txtLog.SetBounds(14, 22, 966, 132);
			_txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			_txtLog.ReadOnly = true;
			_txtLog.BackColor = Color.FromArgb(24, 24, 28);
			_txtLog.ForeColor = Color.Gainsboro;
			_txtLog.Font = new Font("Consolas", 9f);
			_txtLog.DetectUrls = true;
			_txtLog.LinkClicked += (s, e) => OpenUrl(e.LinkText);
			_grpLog.Controls.Add(_txtLog);

			_btnSignIn.SetBounds(12, 762, 150, 36);
			_btnSignIn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			_btnSignIn.Click += BtnSignIn_Click;

			_btnPublish.SetBounds(170, 762, 160, 36);
			_btnPublish.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			_btnPublish.Click += BtnPublish_Click;

			_btnMyWorkshop.SetBounds(338, 762, 130, 36);
			_btnMyWorkshop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			_btnMyWorkshop.Click += (s, e) => OpenMyWorkshop();

			_btnHelp.SetBounds(476, 762, 70, 36);
			_btnHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			_btnHelp.Click += (s, e) => ShowHelp();

			_lblProgress.SetBounds(554, 770, 232, 20);
			_lblProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

			_btnHub.SetBounds(794, 762, 100, 36);
			_btnHub.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			_btnHub.Click += (s, e) => OpenUrl(WorkshopHub);

			_btnOpenMods.SetBounds(902, 762, 106, 36);
			_btnOpenMods.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			_btnOpenMods.Click += (s, e) => OpenFolder(_txtMods.Text.Trim());

			Controls.AddRange(new Control[] { _grpSteam, _grpModsFolder, _grpList, _grpSettings, _grpLog, _btnSignIn, _btnPublish, _btnMyWorkshop, _btnHelp, _lblProgress, _btnHub, _btnOpenMods });

			_flashTimer.Interval = 2500;
			_flashTimer.Tick += (s, e) =>
			{
				_flashTimer.Stop();
				if (_btnPublish.Enabled)
				{
					_lblProgress.Text = "";
				}
			};

			_tips.AutoPopDelay = 15000;
			_tips.InitialDelay = 400;
			_tips.ReshowDelay = 100;

			FormClosing += (s, e) => SaveSettingsFromUi();
		}

		private void ApplyLanguage()
		{
			Text = L.T("app.title");
			_grpSteam.Text = L.T("grp.steam");
			_grpModsFolder.Text = L.T("grp.mods");
			_grpList.Text = L.T("grp.list");
			_grpSettings.Text = L.T("grp.settings");
			_grpLog.Text = L.T("grp.log");

			_btnLocateSteam.Text = L.T("btn.locate");
			_btnDownloadSteam.Text = L.T("btn.download");
			_btnBrowseMods.Text = L.T("btn.browse");
			_btnRefresh.Text = L.T("btn.refresh");
			_btnSignIn.Text = L.T("btn.signin");
			_btnPublish.Text = L.T("btn.publish");
			_btnMyWorkshop.Text = L.T("btn.myworkshop");
			_btnHelp.Text = L.T("btn.help");
			_btnHub.Text = L.T("btn.hub");
			_btnOpenMods.Text = L.T("btn.modsfolder");
			_btnSavePrefs.Text = L.T("btn.saveprefs");
			_btnBrowseBackups.Text = L.T("btn.browse");

			_lblUser.Text = L.T("lbl.user");
			_lblVisibility.Text = L.T("lbl.visibility");
			_lblBackups.Text = L.T("lbl.backups");
			_lblTags.Text = L.T("lbl.tags");
			_lblTagsHint.Text = L.T("lbl.tagshint");
			_lblLanguage.Text = L.T("lbl.language");

			_chkDryRun.Text = L.T("chk.dryrun");
			_chkMove.Text = L.T("chk.move");

			int visibility = Math.Max(0, _cboVisibility.SelectedIndex);
			bool wasInitializing = _initializing;
			_initializing = true;
			_cboVisibility.Items.Clear();
			_cboVisibility.Items.AddRange(new object[]
			{
				L.T("vis.public"),
				L.T("vis.friends"),
				L.T("vis.private"),
				L.T("vis.unlisted")
			});
			_cboVisibility.SelectedIndex = visibility;
			_cboLanguage.SelectedIndex = Array.FindIndex(L.Languages, l => l.Code == L.Current);
			_initializing = wasInitializing;

			_tips.SetToolTip(_txtSteamCmd, L.T("tip.steamcmd"));
			_tips.SetToolTip(_btnLocateSteam, L.T("tip.locate"));
			_tips.SetToolTip(_btnDownloadSteam, L.T("tip.download"));
			_tips.SetToolTip(_txtMods, L.T("tip.mods"));
			_tips.SetToolTip(_lstMods, L.T("tip.list"));
			_tips.SetToolTip(_txtUser, L.T("tip.user"));
			_tips.SetToolTip(_cboVisibility, L.T("tip.visibility"));
			_tips.SetToolTip(_txtBackups, L.T("tip.backups"));
			_tips.SetToolTip(_txtTags, L.T("tip.tags"));
			_tips.SetToolTip(_chkDryRun, L.T("tip.dryrun"));
			_tips.SetToolTip(_chkMove, L.T("tip.move"));
			_tips.SetToolTip(_btnSavePrefs, L.T("tip.saveprefs"));
			_tips.SetToolTip(_btnMyWorkshop, L.T("tip.myworkshop"));
			_tips.SetToolTip(_btnHelp, L.T("tip.help"));
			_tips.SetToolTip(_btnPublish, L.T("tip.publish"));
			_tips.SetToolTip(_btnSignIn, L.T("tip.signin"));
			_tips.SetToolTip(_cboLanguage, L.T("lbl.language"));

			UpdateSteamStatus();
		}

		// ---------------------------------------------------------------- helpers

		private void Log(string line)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action<string>(Log), line);
				return;
			}
			_txtLog.AppendText(line + Environment.NewLine);
			_txtLog.SelectionStart = _txtLog.TextLength;
			_txtLog.ScrollToCaret();
		}

		private void SetProgress(string text)
		{
			if (InvokeRequired)
			{
				BeginInvoke(new Action<string>(SetProgress), text);
				return;
			}
			_lblProgress.Text = text;
		}

		private void SetBusy(bool busy)
		{
			_btnPublish.Enabled = !busy;
			_btnSignIn.Enabled = !busy;
			_btnDownloadSteam.Enabled = !busy;
			_btnLocateSteam.Enabled = !busy;
			_btnBrowseMods.Enabled = !busy;
			_btnRefresh.Enabled = !busy;
			_btnBrowseBackups.Enabled = !busy;
			_btnSavePrefs.Enabled = !busy;
			_lstMods.Enabled = !busy;
			Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
			if (!busy)
			{
				SetProgress("");
			}
		}

		private void SaveSettingsFromUi()
		{
			_settings.SteamCmdPath = _txtSteamCmd.Text.Trim();
			_settings.ModsFolder = _txtMods.Text.Trim();
			_settings.SteamUser = _txtUser.Text.Trim();
			_settings.Tags = _txtTags.Text.Trim();
			_settings.Visibility = Math.Max(0, _cboVisibility.SelectedIndex);
			_settings.VisibilityChosen = _settings.VisibilityChosen || _visibilityTouched;
			_settings.BackupFolder = _txtBackups.Text.Trim();
			_settings.MoveAfterUpload = _chkMove.Checked;
			_settings.Language = L.Current;
			_settings.Save();
		}

		private void SavePreferences()
		{
			SaveSettingsFromUi();
			Log(L.T("log.prefsSaved"));
			FlashProgress(L.T("status.prefsSaved"));
		}

		private void FlashProgress(string text)
		{
			SetProgress(text);
			_flashTimer.Stop();
			_flashTimer.Start();
		}

		private void OpenMyWorkshop()
		{
			string input = _txtUser.Text.Trim();
			if (input.Length == 0)
			{
				MessageBox.Show(this, L.T("msg.myWorkshopEmpty"), L.T("btn.myworkshop"), MessageBoxButtons.OK, MessageBoxIcon.Information);
				_txtUser.Focus();
				return;
			}
			string url;
			if (input.StartsWith("http", StringComparison.OrdinalIgnoreCase))
			{
				url = input.TrimEnd('/') + "/myworkshopfiles/";
			}
			else
			{
				string name = input.TrimStart('@', '/');
				if (name.StartsWith("id/", StringComparison.OrdinalIgnoreCase))
				{
					name = name.Substring(3);
				}
				else if (name.StartsWith("profiles/", StringComparison.OrdinalIgnoreCase))
				{
					name = name.Substring("profiles/".Length);
				}
				url = name.Length >= 17 && name.All(char.IsDigit)
					? "https://steamcommunity.com/profiles/" + name + "/myworkshopfiles/"
					: "https://steamcommunity.com/id/" + name + "/myworkshopfiles/";
			}
			Log(L.T("log.opening", url));
			OpenUrl(url);
		}

		private void ShowHelp()
		{
			using var form = new Form
			{
				Text = L.T("help.title"),
				ClientSize = new Size(780, 680),
				StartPosition = FormStartPosition.CenterParent,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				MinimizeBox = false,
				MaximizeBox = false,
				ShowInTaskbar = false,
				Font = Font
			};
			var box = new TextBox
			{
				Multiline = true,
				ReadOnly = true,
				ScrollBars = ScrollBars.Vertical,
				Bounds = new Rectangle(12, 12, 756, 620),
				BackColor = Color.White,
				Font = new Font("Consolas", 9f),
				Text = L.T("help.body").Replace("\n", Environment.NewLine)
			};
			box.Select(0, 0);
			var close = new Button
			{
				Text = L.T("btn.close"),
				Bounds = new Rectangle(660, 640, 108, 28),
				DialogResult = DialogResult.OK
			};
			form.Controls.Add(box);
			form.Controls.Add(close);
			form.AcceptButton = close;
			form.CancelButton = close;
			form.ShowDialog(this);
		}

		private void UpdateSteamStatus()
		{
			string path = _txtSteamCmd.Text.Trim();
			if (path.Length > 0 && File.Exists(path))
			{
				_lblSteamStatus.Text = L.T("steam.found", path);
				_lblSteamStatus.ForeColor = Color.ForestGreen;
			}
			else
			{
				_lblSteamStatus.Text = L.T("steam.missing");
				_lblSteamStatus.ForeColor = Color.Firebrick;
			}
		}

		private static string DetectSteamCmd()
		{
			var candidates = new List<string>();
			string pathVariable = Environment.GetEnvironmentVariable("PATH") ?? "";
			foreach (string part in pathVariable.Split(';'))
			{
				string trimmed = part.Trim();
				if (trimmed.Length > 0)
				{
					candidates.Add(Path.Combine(trimmed, "steamcmd.exe"));
				}
			}
			candidates.Add(@"C:\steamcmd\steamcmd.exe");
			candidates.Add(@"D:\steamcmd\steamcmd.exe");
			candidates.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam", "steamcmd", "steamcmd.exe"));
			candidates.Add(Path.Combine(AppContext.BaseDirectory, "steamcmd", "steamcmd.exe"));
			candidates.Add(Path.Combine(AppContext.BaseDirectory, "steamcmd.exe"));
			foreach (string candidate in candidates)
			{
				try
				{
					if (File.Exists(candidate))
					{
						return candidate;
					}
				}
				catch
				{
					// ignore malformed PATH entries
				}
			}
			return "";
		}

		private static string DetectModsFolder()
		{
			string[] steamRoots = { @"C:\Program Files (x86)\Steam", @"C:\Program Files\Steam" };
			foreach (string root in steamRoots)
			{
				string libraryFile = Path.Combine(root, "steamapps", "libraryfolders.vdf");
				if (!File.Exists(libraryFile))
				{
					continue;
				}
				try
				{
					foreach (Match match in PathRegex.Matches(File.ReadAllText(libraryFile)))
					{
						string library = match.Groups[1].Value.Replace("\\\\", "\\");
						string mods = Path.Combine(library, "steamapps", "common", "Probably Stolen", "Mods");
						if (Directory.Exists(mods))
						{
							return mods;
						}
					}
				}
				catch
				{
					// ignore unreadable libraries
				}
			}
			return "";
		}

		private static ModInfo ReadMod(string folder)
		{
			string manifestPath = Path.Combine(folder, "manifest.xml");
			if (!File.Exists(manifestPath))
			{
				return null;
			}
			try
			{
				XDocument document = XDocument.Load(manifestPath);
				XElement root = document.Root;
				if (root == null)
				{
					return null;
				}
				string id = root.Element("ID")?.Value?.Trim();
				string name = root.Element("Name")?.Value?.Trim();
				if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
				{
					return null;
				}
				string description = root.Element("Description")?.Value?.Trim();
				return new ModInfo
				{
					Folder = folder,
					Id = id,
					Name = name,
					Version = root.Element("ModVersion")?.Value?.Trim() ?? "1.0",
					Author = root.Element("Author")?.Value?.Trim() ?? "",
					Description = string.IsNullOrEmpty(description) ? "A mod for Probably Stolen." : description
				};
			}
			catch
			{
				return null;
			}
		}

		private void RefreshMods()
		{
			string folder = _txtMods.Text.Trim();
			_mods.Clear();
			_lstMods.Items.Clear();
			if (folder.Length == 0 || !Directory.Exists(folder))
			{
				Log(L.T("log.modsMissing"));
				return;
			}
			foreach (string directory in Directory.GetDirectories(folder).OrderBy(d => Path.GetFileName(d), StringComparer.OrdinalIgnoreCase))
			{
				ModInfo mod = ReadMod(directory);
				if (mod != null)
				{
					_mods.Add(mod);
					_lstMods.Items.Add(mod);
				}
			}
			Log(L.T("log.foundMods", _mods.Count, folder));
		}

		private List<ModInfo> CheckedMods()
		{
			return _lstMods.CheckedItems.Cast<ModInfo>().ToList();
		}

		// ---------------------------------------------------------------- steamcmd

		private void BtnLocateSteam_Click(object sender, EventArgs e)
		{
			using var dialog = new OpenFileDialog { Filter = "steamcmd.exe|steamcmd.exe|Executables (*.exe)|*.exe", Title = L.T("btn.locate") };
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				_txtSteamCmd.Text = dialog.FileName;
				UpdateSteamStatus();
				SaveSettingsFromUi();
			}
		}

		private async void BtnDownloadSteam_Click(object sender, EventArgs e)
		{
			using var dialog = new FolderBrowserDialog
			{
				Description = L.T("tip.download"),
				UseDescriptionForTitle = true
			};
			if (dialog.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}
			string target = dialog.SelectedPath;
			if (!string.Equals(Path.GetFileName(target.TrimEnd('\\', '/')), "steamcmd", StringComparison.OrdinalIgnoreCase))
			{
				target = Path.Combine(target, "steamcmd");
			}
			Directory.CreateDirectory(target);
			string exe = Path.Combine(target, "steamcmd.exe");

			SetBusy(true);
			try
			{
				if (!File.Exists(exe))
				{
					Log(L.T("log.downloading"));
					SetProgress(L.T("status.downloading"));
					using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };
					byte[] data = await client.GetByteArrayAsync(SteamCmdDownload);
					string zip = Path.Combine(Path.GetTempPath(), "steamcmd.zip");
					File.WriteAllBytes(zip, data);
					Log(L.T("log.downloaded", data.Length / 1024));
					ZipFile.ExtractToDirectory(zip, target, true);
					try
					{
						File.Delete(zip);
					}
					catch
					{
						// not important
					}
				}
				if (!File.Exists(exe))
				{
					throw new Exception(L.T("err.extract"));
				}
				_txtSteamCmd.Text = exe;
				UpdateSteamStatus();
				SaveSettingsFromUi();

				Log(L.T("log.prepareSteam"));
				SetProgress(L.T("status.prepareSteam"));
				int exitCode = await RunProcessAsync(exe, "+quit", target, Log, TimeSpan.FromMinutes(20));
				Log(L.T("log.steamPrepared", exitCode));
				MessageBox.Show(this, L.T("msg.steamReady"), L.T("msg.steamTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception exception)
			{
				Log(L.T("log.error", exception.Message));
				MessageBox.Show(this, L.T("msg.steamFailed", exception.Message, SteamCmdDownload), L.T("msg.steamTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally
			{
				SetBusy(false);
			}
		}

		private void BtnSignIn_Click(object sender, EventArgs e)
		{
			string exe = _txtSteamCmd.Text.Trim();
			if (!File.Exists(exe))
			{
				MessageBox.Show(this, L.T("msg.noSteamSignin"), L.T("msg.signinTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			string user = _txtUser.Text.Trim();
			if (user.Length == 0)
			{
				MessageBox.Show(this, L.T("msg.noUser"), L.T("msg.signinTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				_txtUser.Focus();
				return;
			}
			SaveSettingsFromUi();
			try
			{
				Process.Start(new ProcessStartInfo(exe, "+login " + user + " +quit")
				{
					UseShellExecute = true,
					WorkingDirectory = Path.GetDirectoryName(exe) ?? AppContext.BaseDirectory
				});
				Log(L.T("log.signInOpened"));
			}
			catch (Exception exception)
			{
				Log(L.T("log.error", exception.Message));
			}
		}

		// ---------------------------------------------------------------- publish

		private async void BtnPublish_Click(object sender, EventArgs e)
		{
			string exe = _txtSteamCmd.Text.Trim();
			if (!File.Exists(exe))
			{
				MessageBox.Show(this, L.T("msg.noSteamPublish"), L.T("msg.publishTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			string user = _txtUser.Text.Trim();
			if (user.Length == 0)
			{
				MessageBox.Show(this, L.T("msg.noUser"), L.T("msg.publishTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				_txtUser.Focus();
				return;
			}
			List<ModInfo> mods = CheckedMods();
			if (mods.Count == 0)
			{
				MessageBox.Show(this, L.T("msg.noMods"), L.T("msg.publishTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			string backups = _txtBackups.Text.Trim();
			if (backups.Length == 0)
			{
				MessageBox.Show(this, L.T("msg.noBackups"), L.T("msg.publishTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			Directory.CreateDirectory(backups);

			bool dryRun = _chkDryRun.Checked;
			bool move = _chkMove.Checked;

			if (!dryRun)
			{
				int newItems = mods.Count(m => !_settings.PublishedIds.ContainsKey(m.Id));
				if (newItems > 0)
				{
					DialogResult answer = MessageBox.Show(this, L.T("msg.newItems", newItems), L.T("msg.publishTitle"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
					if (answer != DialogResult.OK)
					{
						return;
					}
				}
				if (move)
				{
					DialogResult answer = MessageBox.Show(this, L.T("msg.moveConfirm", backups), L.T("msg.publishTitle"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
					if (answer != DialogResult.OK)
					{
						return;
					}
				}
			}

			SaveSettingsFromUi();
			SetBusy(true);
			var results = new List<(ModInfo Mod, string Id, bool Ok)>();
			try
			{
				foreach (ModInfo mod in mods)
				{
					try
					{
						string id = await PublishOneAsync(exe, user, mod, backups, move, dryRun);
						results.Add((mod, id, true));
					}
					catch (Exception exception)
					{
						Log(L.T("log.failedMod", mod.Name, exception.Message));
						results.Add((mod, "", false));
					}
				}
			}
			finally
			{
				SetBusy(false);
				RefreshMods();
			}
			ShowSummary(results, dryRun);
		}

		private async Task<string> PublishOneAsync(string steamCmd, string user, ModInfo mod, string backupFolder, bool move, bool dryRun)
		{
			Log("");
			Log(L.T("log.header", mod.Name, mod.Version, mod.Id));
			SetProgress(L.T("status.preparing", mod.Name));

			string staging = Path.Combine(StagingRoot, mod.Id);
			if (Directory.Exists(staging))
			{
				Directory.Delete(staging, true);
			}
			string content = Path.Combine(staging, "content");
			CopyDirectory(mod.Folder, content);
			Log(L.T("log.staged", Directory.GetFiles(content, "*", SearchOption.AllDirectories).Length));

			string preview = FindPreview(mod.Folder);
			if (preview == null)
			{
				preview = Path.Combine(staging, "preview.png");
				CreatePreview(preview, mod.Name, mod.Version);
				Log(L.T("log.previewGenerated"));
			}
			else
			{
				Log(L.T("log.previewUsing", preview));
			}

			string existingId = _settings.PublishedIds.TryGetValue(mod.Id, out string saved) ? saved : "0";
			string vdf = Path.Combine(staging, "workshop_build.vdf");
			File.WriteAllText(vdf, BuildVdf(content, preview, mod, existingId, ParseVisibility(), ParseTags()), new UTF8Encoding(false));
			Log(existingId == "0" ? L.T("log.preparedNew") : L.T("log.preparedUpdate", existingId));

			if (dryRun)
			{
				Log(L.T("log.dryRun"));
				return "";
			}

			SetProgress(L.T("status.uploading", mod.Name));
			string newId = await RunUploadAsync(steamCmd, user, vdf);
			if (newId.Length > 0)
			{
				_settings.PublishedIds[mod.Id] = newId;
				_settings.Save();
				Log(L.T("log.item", "https://steamcommunity.com/sharedfiles/filedetails/?id=" + newId));
			}
			else if (existingId != "0")
			{
				newId = existingId;
				Log(L.T("log.keepId", existingId));
			}
			else
			{
				Log(L.T("log.noId"));
			}

			string zip = CreateBackup(mod, backupFolder);
			Log(L.T("log.backup", zip));
			if (newId.Length > 0)
			{
				try
				{
					File.WriteAllText(Path.Combine(backupFolder, mod.Id + "_workshop_id.txt"),
						"Workshop item id: " + newId + Environment.NewLine +
						"https://steamcommunity.com/sharedfiles/filedetails/?id=" + newId + Environment.NewLine);
				}
				catch
				{
					// optional convenience file
				}
			}

			if (move)
			{
				try
				{
					MoveModToBackup(mod, backupFolder);
				}
				catch (Exception moveError)
				{
					// The upload already succeeded; a locked DLL (game running) must not turn this into a failure.
					Log("Could not move the local mod folder (is the game running?): " + moveError.Message);
					Log("The mod is uploaded; move or delete " + mod.Folder + " manually.");
				}
			}
			return newId;
		}

		private async Task<string> RunUploadAsync(string steamCmd, string user, string vdf)
		{
			_lastParsedId = "";
			_authProblem = false;
			var startInfo = new ProcessStartInfo(steamCmd, "+login " + user + " +workshop_build_item \"" + vdf + "\" +quit")
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = Path.GetDirectoryName(steamCmd) ?? AppContext.BaseDirectory
			};
			using var process = new Process { StartInfo = startInfo };
			process.OutputDataReceived += (s, e) => HandleSteamLine(e.Data);
			process.ErrorDataReceived += (s, e) => HandleSteamLine(e.Data);
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			Task exitTask = process.WaitForExitAsync();
			Task timeout = Task.Delay(TimeSpan.FromMinutes(25));
			if (await Task.WhenAny(exitTask, timeout) != exitTask)
			{
				try
				{
					process.Kill(true);
				}
				catch
				{
					// ignore
				}
				throw new Exception(L.T("err.timeout"));
			}
			await exitTask;

			if (_lastParsedId.Length == 0 && _authProblem)
			{
				throw new Exception(L.T("err.auth"));
			}
			if (_lastParsedId.Length == 0 && process.ExitCode != 0)
			{
				throw new Exception(L.T("err.exit", process.ExitCode));
			}
			return _lastParsedId;
		}

		private void HandleSteamLine(string line)
		{
			if (line == null)
			{
				return;
			}
			Log("  " + line);
			Match match = IdRegex.Match(line);
			if (match.Success)
			{
				_lastParsedId = match.Groups[1].Value;
			}
			if (line.IndexOf("Invalid Password", StringComparison.OrdinalIgnoreCase) >= 0 ||
				line.IndexOf("Steam Guard", StringComparison.OrdinalIgnoreCase) >= 0 ||
				line.IndexOf("Account Logon Denied", StringComparison.OrdinalIgnoreCase) >= 0 ||
				line.IndexOf("FAILED (", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				_authProblem = true;
			}
		}

		// ---------------------------------------------------------------- files

		private static string BuildVdf(string contentFolder, string previewFile, ModInfo mod, string publishedId, int visibility, string[] tags)
		{
			var builder = new StringBuilder();
			builder.AppendLine("\"workshopitem\"");
			builder.AppendLine("{");
			AppendKey(builder, "appid", AppId);
			AppendKey(builder, "publishedfileid", publishedId);
			AppendKey(builder, "contentfolder", contentFolder);
			AppendKey(builder, "previewfile", previewFile);
			AppendKey(builder, "visibility", Math.Clamp(visibility, 0, 3).ToString());
			AppendKey(builder, "title", mod.Name);
			AppendKey(builder, "description", mod.Description);
			AppendKey(builder, "changenote", "Version " + mod.Version);

			if (tags.Length > 0)
			{
				builder.AppendLine("\t\"tags\"");
				builder.AppendLine("\t{");
				for (int i = 0; i < tags.Length; i++)
				{
					AppendKey(builder, i.ToString(), tags[i], "\t\t");
				}
				builder.AppendLine("\t}");
			}
			builder.AppendLine("}");
			return builder.ToString();
		}

		private string[] ParseTags()
		{
			return (_txtTags.Text ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(t => t.Trim())
				.Where(t => t.Length > 0)
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToArray();
		}

		private int ParseVisibility()
		{
			return _cboVisibility.SelectedIndex < 0 ? 2 : _cboVisibility.SelectedIndex;
		}

		private static void AppendKey(StringBuilder builder, string key, string value, string indent = "\t")
		{
			builder.Append(indent).Append('"').Append(Escape(key)).Append("\"\t\"").Append(Escape(value ?? "")).AppendLine("\"");
		}

		private static string Escape(string value)
		{
			return (value ?? "").Replace("\\", "\\\\").Replace("\"", "\\\"");
		}

		private static void CopyDirectory(string source, string destination)
		{
			Directory.CreateDirectory(destination);
			foreach (string file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
			{
				string relative = Path.GetRelativePath(source, file);
				string target = Path.Combine(destination, relative);
				Directory.CreateDirectory(Path.GetDirectoryName(target));
				File.Copy(file, target, true);
			}
		}

		private static string FindPreview(string folder)
		{
			string[] names = { "preview.png", "preview.jpg", "preview.jpeg", "icon.png", "thumbnail.png" };
			foreach (string name in names)
			{
				string candidate = Path.Combine(folder, name);
				if (File.Exists(candidate))
				{
					return candidate;
				}
			}
			string previewFolder = Path.Combine(folder, "preview");
			if (Directory.Exists(previewFolder))
			{
				string found = Directory.GetFiles(previewFolder)
					.FirstOrDefault(f => Regex.IsMatch(f, @"\.(png|jpe?g)$", RegexOptions.IgnoreCase));
				if (found != null)
				{
					return found;
				}
			}
			return null;
		}

		private static void CreatePreview(string path, string title, string version)
		{
			using var bitmap = new Bitmap(512, 512);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.Clear(Color.FromArgb(22, 22, 28));
				using var accent = new SolidBrush(Color.FromArgb(120, 200, 160));
				graphics.FillRectangle(accent, 0, 0, 512, 8);
				graphics.FillRectangle(accent, 0, 504, 512, 8);
				using var titleFont = new Font("Segoe UI", 34, FontStyle.Bold, GraphicsUnit.Pixel);
				using var subFont = new Font("Segoe UI", 18, FontStyle.Regular, GraphicsUnit.Pixel);
				using var white = new SolidBrush(Color.White);
				using var grey = new SolidBrush(Color.FromArgb(170, 172, 180));
				var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
				graphics.DrawString(title, titleFont, white, new RectangleF(36, 120, 440, 240), format);
				graphics.DrawString("Probably Stolen mod  -  v" + version, subFont, grey, new RectangleF(36, 372, 440, 40), format);
			}
			bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
		}

		private static string CreateBackup(ModInfo mod, string backupFolder)
		{
			string stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
			string zip = Path.Combine(backupFolder, mod.Id + "_v" + mod.Version + "_" + stamp + ".zip");
			ZipFile.CreateFromDirectory(mod.Folder, zip, CompressionLevel.Optimal, true);
			return zip;
		}

		private void MoveModToBackup(ModInfo mod, string backupFolder)
		{
			string target = Path.Combine(backupFolder, Path.GetFileName(mod.Folder));
			if (Directory.Exists(target))
			{
				target = Path.Combine(backupFolder, Path.GetFileName(mod.Folder) + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss"));
			}
			Directory.Move(mod.Folder, target);
			Log(L.T("log.moved", target));
		}

		private void ShowSummary(List<(ModInfo Mod, string Id, bool Ok)> results, bool dryRun)
		{
			var builder = new StringBuilder();
			builder.AppendLine(dryRun ? L.T("msg.dryDone") : L.T("msg.done"));
			builder.AppendLine();
			foreach ((ModInfo mod, string id, bool ok) in results)
			{
				if (!ok)
				{
					builder.AppendLine(L.T("line.failed", mod.Name));
				}
				else if (id.Length > 0)
				{
					builder.AppendLine(L.T("line.item", mod.Name, id));
				}
				else
				{
					builder.AppendLine(dryRun ? L.T("line.prepared", mod.Name) : L.T("line.preparedNoId", mod.Name));
				}
			}
			if (!dryRun)
			{
				builder.AppendLine();
				builder.AppendLine(L.T("msg.nextSteps"));
			}
			MessageBox.Show(this, builder.ToString(), L.T("msg.summaryTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		// ---------------------------------------------------------------- misc

		private static async Task<int> RunProcessAsync(string exe, string arguments, string workingDirectory, Action<string> onLine, TimeSpan timeout)
		{
			var startInfo = new ProcessStartInfo(exe, arguments)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = workingDirectory
			};
			using var process = new Process { StartInfo = startInfo };
			process.OutputDataReceived += (s, e) => { if (e.Data != null) onLine(e.Data); };
			process.ErrorDataReceived += (s, e) => { if (e.Data != null) onLine(e.Data); };
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			Task exitTask = process.WaitForExitAsync();
			if (await Task.WhenAny(exitTask, Task.Delay(timeout)) != exitTask)
			{
				try
				{
					process.Kill(true);
				}
				catch
				{
					// ignore
				}
				return -1;
			}
			await exitTask;
			return process.ExitCode;
		}

		private static void OpenUrl(string url)
		{
			try
			{
				Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
			}
			catch
			{
				// ignore
			}
		}

		private void OpenFolder(string folder)
		{
			if (folder.Length == 0 || !Directory.Exists(folder))
			{
				return;
			}
			try
			{
				Process.Start(new ProcessStartInfo("explorer.exe", "\"" + folder + "\"") { UseShellExecute = true });
			}
			catch
			{
				// ignore
			}
		}

		// ---------------------------------------------------------------- browse

		private void BtnBrowseMods_Click(object sender, EventArgs e)
		{
			using var dialog = new FolderBrowserDialog { Description = L.T("tip.mods"), UseDescriptionForTitle = true };
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				_txtMods.Text = dialog.SelectedPath;
				SaveSettingsFromUi();
				RefreshMods();
			}
		}

		private void BtnBrowseBackups_Click(object sender, EventArgs e)
		{
			using var dialog = new FolderBrowserDialog { Description = L.T("tip.backups"), UseDescriptionForTitle = true };
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				_txtBackups.Text = dialog.SelectedPath;
				SaveSettingsFromUi();
			}
		}
	}
}
