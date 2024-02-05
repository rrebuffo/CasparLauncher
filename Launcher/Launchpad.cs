namespace CasparLauncher.Launcher;

public class Launchpad : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new(propertyName)); }

    public event EventHandler<ExecutableEventArgs>? ExecutableError;
    protected virtual void OnExecutableError(Executable ex) { ExecutableError?.Invoke(this, new(ex)); }

    public event EventHandler<ExecutableEventArgs>? ExecutablePathError;
    protected virtual void OnExecutablePathError(Executable ex) { ExecutablePathError?.Invoke(this, new(ex)); }

    public event EventHandler<ExecutableEventArgs>? ExecutableStarted;
    protected virtual void OnExecutableStarted(Executable ex) { ExecutableStarted?.Invoke(this, new(ex)); }

    public event EventHandler<ExecutableEventArgs>? ExecutableStopped;
    protected virtual void OnExecutableStopped(Executable ex) { ExecutableStopped?.Invoke(this, new(ex)); }

    public Launchpad()
    {
        LoadSettings();
        SaveChanges();
        PropertyChanged += Launchpad_PropertyChanged;
        Executables.CollectionChanged += Executables_CollectionChanged;
    }

    private void Launchpad_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        SaveChanges();
    }

    private void LoadSettings()
    {
        S.Init("casparlauncher");
        S.WriteEnabled = true;
        S.Load();
        ParseSettings();
        if (Keyboard.Modifiers == ModifierKeys.Shift) DisableStart = true;
        InitExecutables();
    }

    #region LAUNCHER SETTINGS

    public ObservableCollection<Executable> Executables { get; set; } = [];

    public static bool DisableStart { get; set; } = false;
    public static bool Exiting { get; set; } = false;

    private Languages? _forcedLanguage;
    public Languages ForcedLanguage
    {
        get
        {
            _forcedLanguage ??= (Languages)S.GetOrSetValue("/ui/language", (int)Languages.none);
            return _forcedLanguage ?? 0;
        }
        set
        {
            if (_forcedLanguage != value)
            {
                _forcedLanguage = value;
                S.Set("/ui/language", (int)value);
                OnPropertyChanged(nameof(ForcedLanguage));
                App.SetLanguageDictionary();
                LocalizationHelper.Instance.CurrentCulture = L.Culture;
            }
        }
    }

    private bool? _darkMode;
    public bool DarkMode
    {
        get
        {
            _darkMode ??= S.GetOrSetValue("/ui/dark", true);
            return _darkMode ?? true;
        }
        set
        {
            if (_darkMode != value)
            {
                _darkMode = value;
                App.SetDarkMode(value);
                S.Set("/ui/dark", value);
                OnPropertyChanged(nameof(DarkMode));
            }
        }
    }

    private bool? _openAtLogin;
    public bool OpenAtLogin
    {
        get
        {
            _openAtLogin ??= S.GetOrSetValue("/ui/open-at-login", false);
            return _openAtLogin ?? false;
        }
        set
        {
            if (_openAtLogin != value)
            {
                _openAtLogin = value;
                SetStartup(value);
                S.Set("/ui/open-at-login", value);
                OnPropertyChanged(nameof(OpenAtLogin));
            }
        }
    }

    private bool? _notifyStart;
    public bool NotifyStart
    {
        get
        {
            _notifyStart ??= S.GetOrSetValue("/ui/notify-start", true);
            return _notifyStart ?? true;
        }
        set
        {
            if (_notifyStart != value)
            {
                _notifyStart = value;
                SetStartup(value);
                S.Set("/ui/notify-start", value);
                OnPropertyChanged(nameof(NotifyStart));
            }
        }
    }

    private bool? _notifyStop;
    public bool NotifyStop
    {
        get
        {
            _notifyStop ??= S.GetOrSetValue("/ui/notify-stop", true);
            return _notifyStop ?? true;
        }
        set
        {
            if (_notifyStop != value)
            {
                _notifyStop = value;
                SetStartup(value);
                S.Set("/ui/notify-stop", value);
                OnPropertyChanged(nameof(NotifyStop));
            }
        }
    }

    private bool? _notifyError;
    public bool NotifyError
    {
        get
        {
            _notifyError ??= S.GetOrSetValue("/ui/notify-error", true);
            return _notifyError ?? true;
        }
        set
        {
            if (_notifyError != value)
            {
                _notifyError = value;
                SetStartup(value);
                S.Set("/ui/notify-error", value);
                OnPropertyChanged(nameof(NotifyError));
            }
        }
    }

    private int? _selectedTab;
    public int SelectedTab
    {
        get
        {
            _selectedTab ??= S.GetOrSetValue("/ui/tab", 0);
            return _selectedTab ?? 0;
        }
        set
        {
            if (_selectedTab != value)
            {
                _selectedTab = value;
                S.Set("/ui/tab", value);
                OnPropertyChanged(nameof(SelectedTab));
            }
        }
    }

    private bool? _stylizeConsole;
    public bool StylizeConsole
    {
        get
        {
            _stylizeConsole ??= S.GetOrSetValue("/ui/console-stylize", true);
            return _stylizeConsole ?? true;
        }
        set
        {
            _stylizeConsole = value;
            S.Set("/ui/console-stylize", value);
            OnPropertyChanged(nameof(StylizeConsole));
        }
    }

    private bool? _cropConsole;
    public bool CropConsole
    {
        get
        {
            _cropConsole ??= S.GetOrSetValue("/ui/console-crop", true);
            return _cropConsole ?? true;
        }
        set
        {
            _cropConsole = value;
            S.Set("/ui/console-crop", value);
            OnPropertyChanged(nameof(CropConsole));
            OnPropertyChanged(nameof(CropConsoleWidth));
        }
    }

    public DataGridLength CropConsoleWidth
    {
        get => CropConsole ? new(10d, DataGridLengthUnitType.Star) : new(10d, DataGridLengthUnitType.Auto);
        set { }
    }

    #endregion

    #region PRIVATE METHODS

    private void ParseSettings()
    {
        try
        {
            if (S.GetOrSet("/executables") is not SettingItem executables) return;

            if (executables.Settings.Count == 0) LoadDefaults();
            else
            {
                foreach (SettingItem executable in executables.Settings.Values)
                {
                    var path = executable.FullPath;
                    Executable new_executable = new()
                    {
                        Path = S.GetOrSetValue(path + "path", ""),
                        Name = S.GetOrSetValue(path + "name", ""),
                        Args = S.GetOrSetValue(path + "args", ""),
                        AutoStart = S.GetOrSetValue(path + "autostart", false),
                        StartupDelay = S.GetOrSetValue(path + "sdel", 0),
                        AllowCommands = S.GetOrSetValue(path + "acmd", false),
                        AllowMultipleInstances = S.GetOrSetValue(path + "amin", false),
                        KillOnlyCurrentPath = S.GetOrSetValue(path + "kocp", false),
                        CommandsDelay = S.GetOrSetValue(path + "cdel", 0),
                        BufferLines = S.GetOrSetValue(path + "buff", 1000),
                        SuppressEmptyLines = S.GetOrSetValue(path + "seln", false),
                        ConfigFile = S.GetOrSetValue(path + "config", ""),
                        ScannerPort = S.GetOrSetValue(path + "sprt", 8000),
                    };

                    if ((new_executable.IsServer || new_executable.IsScanner)
                        && string.IsNullOrEmpty(new_executable.ConfigFile))
                        new_executable.ConfigFile = S.GetOrSetValue(path + "config", "casparcg.config");

                    if (S.GetOrSet(path + "commands") is SettingItem commands && commands.Values.Count > 0)
                    {
                        foreach (SettingValue command in commands.Values)
                        {
                            if (command.Value is not null)
                            {
                                Command new_command = new()
                                {
                                    Value = command.Value
                                };
                                new_executable.Commands.Add(new_command);
                            }
                        }
                    }
                    Executables.Add(new_executable);
                }
            }
        }
        catch(Exception)
        {
            LoadDefaults();
        }
    }

    private void LoadDefaults()
    {
        Executable server = new()
        {
            Name = "Server",
            Path = "casparcg.exe",
            AllowCommands = true,
            ConfigFile = "casparcg.config"
        };
        Executable scanner = new()
        {
            Name = "Scanner",
            Path = "scanner.exe",
            ConfigFile = "casparcg.config"
        };
        Executables.Add(server);
        Executables.Add(scanner);
    }

    private void InitExecutables()
    {
        foreach (Executable executable in Executables)
        {
            executable.Modified -= ExecutableModified;
            executable.ProcessError -= Executable_ProcessError;
            executable.ProcessPathError -= Executable_ProcessPathError;
            executable.ProcessExited -= Executable_ProcessExited;

            executable.Modified += ExecutableModified;
            executable.ProcessError += Executable_ProcessError;
            executable.ProcessPathError += Executable_ProcessPathError;
            executable.ProcessStarted += Executable_ProcessStarted;
            executable.ProcessExited += Executable_ProcessExited;
        }
    }

    private void Executable_ProcessError(object? sender, ExecutableEventArgs e)
    {
        if (NotifyError) OnExecutableError(e.Executable);
        if (e.Exception is not null)
        {
            Debug.WriteLine(e.Exception?.Message);
            Debug.WriteLine(e.Exception?.Data);
            Debug.WriteLine(e.Exception?.StackTrace);
        }
    }

    private void Executable_ProcessPathError(object? sender, ExecutableEventArgs e)
    {
        if (NotifyError && !Exiting) OnExecutablePathError(e.Executable);
    }

    private void Executable_ProcessStarted(object? sender, ExecutableEventArgs e)
    {
        if (NotifyStart && !Exiting) OnExecutableStarted(e.Executable);
    }

    private void Executable_ProcessExited(object? sender, ExecutableEventArgs e)
    {
        if (NotifyStop && !Exiting) OnExecutableStopped(e.Executable);
    }

    private void Executables_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        InitExecutables();
        SaveChanges();
    }

    private void ExecutableModified(object? sender, EventArgs e)
    {
        SaveChanges();
    }

    private void SaveChanges()
    {
        if (S.GetOrSet("/executables") is not SettingItem executables) return;
        executables.Settings.Clear();
        var count = 0;
        foreach (Executable executable in Executables)
        {
            count++;
            string path = $"{executables.FullPath}{count}";
            if (S.GetOrSet(path) is SettingItem setting)
            {
                string s = setting.FullPath;
                S.Set(s + "path", executable.Path??"");
                S.Set(s + "name", executable.Name);
                S.Set(s + "args", executable.Args);
                S.Set(s + "autostart", executable.AutoStart);
                S.Set(s + "sdel", executable.StartupDelay);
                S.Set(s + "acmd", executable.AllowCommands);
                S.Set(s + "amin", executable.AllowMultipleInstances);
                S.Set(s + "kocp", executable.KillOnlyCurrentPath);
                S.Set(s + "cdel", executable.CommandsDelay);
                S.Set(s + "config", executable.ConfigFile);
                S.Set(s + "sprt", executable.ScannerPort);
                S.Set(s + "buff", executable.BufferLines);
                S.Set(s + "seln", executable.SuppressEmptyLines);

                if (executable.Commands.Count > 0)
                {
                    SettingItem? commands = S.GetOrSet(s + "commands");
                    if (commands is not null)
                    {
                        if (commands.Values.Count > 0) commands.Values.Clear();
                        foreach (Command command in executable.Commands)
                            commands.AddValue(command.Value);
                    }
                }
            }
        }
        S.Save();
    }

    private static void SetStartup(bool set = true)
    {
        if (Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true) is RegistryKey regKey)
        {
            if (set) regKey.SetValue("CasparLauncher", Process.GetCurrentProcess().MainModule?.FileName ?? "");
            else regKey.DeleteValue("CasparLauncher", false);
        }

    }

    #endregion

    #region PUBLIC METHODS

    internal Executable AddExecutable(string? path = null)
    {
        Executable new_executable = new()
        {
            Name = L.NewExecutableName,
            Path = path,
            AllowCommands = false
        };
        Executables.Add(new_executable);
        return new_executable;
    }

    internal bool RemoveExecutable(Executable ex)
    {
        if (!ex.Exists)
        {
            Executables.Remove(ex);
            return true;
        }
        MessageBoxResult remove = MessageBox.Show(L.DeleteExecutablePromptMessage, L.DeleteExecutablePromptCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        switch (remove)
        {
            case MessageBoxResult.Yes:
                Executables.Remove(ex);
                return true;
            case MessageBoxResult.No:
            default:
                return false;
        }
    }

    internal static void StartAll(bool startup = false)
    {
        foreach (Executable ex in App.Launchpad.Executables.Where(ex => (!startup || ex.AutoStart) && !ex.IsRunning)) ex.Start();
    }

    internal static void StopAll()
    {
        foreach (Executable ex in App.Launchpad.Executables.Where(ex => ex.IsRunning)) ex.Stop();
    }

    internal static void RestartAll()
    {
        foreach (Executable ex in App.Launchpad.Executables.Where(ex => ex.IsRunning)) ex.Process?.Kill();
    }

    internal static void Start(Executable executable)
    {
        executable.Start();
    }

    internal static void Stop(Executable executable)
    {
        executable.Stop();
    }

    internal static void Restart(Executable executable)
    {
        if (executable.IsRunning) executable.Process?.Kill();
    }

    internal static void RebuildMediaDb(Executable executable)
    {
        if (executable.IsScanner)
            executable.RebuildMediaDb();
    }

    internal static void OpenDiag(Executable executable)
    {
        if (executable.IsServer)
            executable.OpenDiag();
    }

    internal static void OpenGrid(Executable executable)
    {
        if (executable.IsServer)
            executable.OpenGrid();
    }

    #endregion
}
