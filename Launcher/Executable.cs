using System.Management;
using System.Text;
using System.Windows.Threading;
using System.Collections.Specialized;

namespace CasparLauncher.Launcher;

public class Executable : INotifyPropertyChanged
{
    const string LOG_LEVEL_START = "Logging [";
    const string LOG_LEVEL_END = "] or higher severity to log";

    private readonly Timer StartupTimer = new();
    private readonly Timer CommandsTimer = new();
    private readonly DispatcherTimer UptimeTimer = new(DispatcherPriority.Normal, Application.Current.Dispatcher);
    private int CurrentCommandIndex = 0;
    private DateTime StartupTime;
    private Encoding CurrentEncoding = Encoding.ASCII;
    private Thread? ExecutableThread;

    public Executable()
    {
        History.Add("");
        StartupTimer.Elapsed += StartupTimer_Elapsed;
        StartupTimer.AutoReset = false;
        CommandsTimer.Elapsed += SendStartupCommand;
        CommandsTimer.AutoReset = true;
        UptimeTimer.Tick += UptimeTimer_Tick;
        PropertyChanged += Executable_PropertyChanged;
        Commands.CollectionChanged += Commands_CollectionChanged;
    }

    private void Commands_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach(Command c in Commands)
        {
            c.PropertyChanged -= CommandChanged;
            c.PropertyChanged += CommandChanged;
        }
        OnModified();
    }

    private void CommandChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnModified();
    }

    private void Executable_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (
            e.PropertyName == nameof(Path) ||
            e.PropertyName == nameof(Name) ||
            e.PropertyName == nameof(Args) ||
            e.PropertyName == nameof(AutoStart) ||
            e.PropertyName == nameof(StartupDelay) ||
            e.PropertyName == nameof(CommandsDelay) ||
            e.PropertyName == nameof(AllowCommands) ||
            e.PropertyName == nameof(AllowMultipleInstances) ||
            e.PropertyName == nameof(ConfigFile) ||
            e.PropertyName == nameof(BufferLines) ||
            e.PropertyName == nameof(SuppressEmptyLines)
        ) {
            OnModified();
        }
    }

    private void UptimeTimer_Tick(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(Uptime));
    }

    #region PROPERTIES

    private string? _path = null;
    public string? Path
    {
        get
        {
            return _path;
        }
        set
        {
            if (_path != value)
            {
                _path = value;
                IsServer = false;
                IsScanner = false;
                OnPropertyChanged(nameof(Path));
                OnPropertyChanged(nameof(Exists));
                OnPropertyChanged(nameof(Icon));
                OnPropertyChanged(nameof(IsServer));
                OnPropertyChanged(nameof(IsScanner));
            }
        }
    }

    private bool? _isServer;
    public bool IsServer
    {
        get
        {
            if(_isServer.HasValue) return _isServer.Value;
            if (_path is null || !Exists) return false;
            _isServer = System.IO.Path.GetFileNameWithoutExtension(_path).ToLower() == "casparcg";
            return _isServer.Value;
        }
        private set
        {
            _isServer = null;
            _ = IsServer;
        }
    }

    private bool? _isScanner;
    public bool IsScanner
    {
        get
        {
            if (_isScanner.HasValue) return _isScanner.Value;
            if (_path is null || !Exists) return false;
            _isScanner = System.IO.Path.GetFileNameWithoutExtension(_path).ToLower() == "scanner";
            return _isScanner ?? false;
        }
        private set
        {
            _isScanner = null;
            _ = IsScanner;
        }
    }

    private bool _isselected = false;
    public bool IsSelected
    {
        get
        {
            return _isselected;
        }
        set
        {
            if (_isselected != value)
            {
                _isselected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    private bool _enabled = false;
    public bool Enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }
    }

    private string _name = "Executable";
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    private string _configFile = "";
    public string ConfigFile
    {
        get
        {
            return _configFile;
        }
        set
        {
            if (_configFile != value)
            {
                _configFile = value;
                OnPropertyChanged(nameof(ConfigFile));
            }
        }
    }

    private string _args = "";
    public string Args
    {
        get
        {
            return _args;
        }
        set
        {
            if (_args != value)
            {
                _args = value;
                OnPropertyChanged(nameof(Args));
            }
        }
    }

    private bool _autostart = true;
    public bool AutoStart
    {
        get
        {
            return _autostart;
        }
        set
        {
            if (_autostart != value)
            {
                _autostart = value;
                OnPropertyChanged(nameof(AutoStart));
            }
        }
    }

    private int _startupdelay = 0;
    public int StartupDelay
    {
        get
        {
            return _startupdelay;
        }
        set
        {
            if (_startupdelay != value)
            {
                _startupdelay = value;
                OnPropertyChanged(nameof(StartupDelay));
            }
        }
    }

    private int _commandsdelay = 5;
    public int CommandsDelay
    {
        get
        {
            return _commandsdelay;
        }
        set
        {
            if (_commandsdelay != value)
            {
                _commandsdelay = value;
                OnPropertyChanged(nameof(CommandsDelay));
            }
        }
    }

    private bool _allowcommands = false;
    public bool AllowCommands
    {
        get
        {
            return _allowcommands;
        }
        set
        {
            if (_allowcommands != value)
            {
                _allowcommands = value;
                OnPropertyChanged(nameof(AllowCommands));
            }
        }
    }

    private int _bufferLines = 1000;
    public int BufferLines
    {
        get
        {
            return _bufferLines;
        }
        set
        {
            if (_bufferLines != value)
            {
                _bufferLines = value;
                OnPropertyChanged(nameof(BufferLines));
            }
        }
    }

    private bool _suppressEmptyLines = false;
    public bool SuppressEmptyLines
    {
        get
        {
            return _suppressEmptyLines;
        }
        set
        {
            if (_suppressEmptyLines != value)
            {
                _suppressEmptyLines = value;
                OnPropertyChanged(nameof(SuppressEmptyLines));
            }
        }
    }

    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            if (_isRunning != value)
            {
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }
    }

    private LogLevel _currentLogLevel = LogLevel._warning;
    public LogLevel CurrentLogLevel
    {
        get
        {
            return _currentLogLevel;
        }
        set
        {
            if (_currentLogLevel != value)
            {
                _currentLogLevel = value;
                OnPropertyChanged(nameof(CurrentLogLevel));
                if(_logLevelIsSet) ChangeCasparLogLevel(value);
                _logLevelIsSet = true;
            }
        }
    }

    private bool _logLevelIsSet = false;

    private bool _allowMultipleInstances = false;
    public bool AllowMultipleInstances
    {
        get
        {
            return _allowMultipleInstances;
        }
        set
        {
            if (_allowMultipleInstances != value)
            {
                _allowMultipleInstances = value;
                OnPropertyChanged(nameof(AllowMultipleInstances));
            }
        }
    }

    private bool _killOnlyCurrentPath = false;
    public bool KillOnlyCurrentPath
    {
        get
        {
            return _killOnlyCurrentPath;
        }
        set
        {
            if (_killOnlyCurrentPath != value)
            {
                _killOnlyCurrentPath = value;
                OnPropertyChanged(nameof(KillOnlyCurrentPath));
            }
        }
    }

    private int HistoryIndex
    {
        get
        {
            if (CurrentHistoryIndex >= History.Count) CurrentHistoryIndex = History.Count - 1;
            if (CurrentHistoryIndex < 0) CurrentHistoryIndex = 0;
            return History.Count - (CurrentHistoryIndex + 1);
        }
    }

    public int CurrentHistoryIndex { get; set; } = 0;

    private string _currentcommand = "";
    public string CurrentCommand
    {
        get
        {
            return _currentcommand;
        }
        set
        {
            if(CurrentHistoryIndex>0)
            {
                if (value == History[HistoryIndex])
                {
                    _currentcommand = value;
                    OnPropertyChanged(nameof(CurrentCommand));
                }
                else
                {
                    _currentcommand = value;
                    History[^1] = value;
                    CurrentHistoryIndex = 0;
                    OnPropertyChanged(nameof(CurrentCommand));
                }
            }
            else
            {
                if (_currentcommand != value)
                {
                    _currentcommand = value;
                    History[^1] = value;
                    OnPropertyChanged(nameof(CurrentCommand));
                }
            }
        }
    }

    public ObservableCollection<LogLine> Output { get; set; } = [];

    private Process? _process = null;
    public Process? Process
    {
        get
        {
            return _process;
        }
        set
        {
            if (_process != value)
            {
                _process = value;
                OnPropertyChanged(nameof(Process));
            }
        }
    }

    public bool Exists
    {
        get
        {
            if (string.IsNullOrEmpty(_path)) return false;
            string path = System.IO.Path.GetFullPath(_path);
            return (Directory.Exists(System.IO.Path.GetDirectoryName(path)) && File.Exists(path));
        }
    }

    public Icon? Icon
    {
        get
        {
            if (_path is null || !Exists) return null;
            return Icon.ExtractAssociatedIcon(_path);
        }
    }

    public TimeSpan Uptime => DateTime.Now - StartupTime;

    public ObservableCollection<Command> Commands { get; set; } = [];

    public ObservableCollection<string> History { get; set; } = [];

    #endregion

    #region STATIC METHODS

    private static bool CheckIfRunning(string file)
    {
        try
        {
            return Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(file)).Any();
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static void KillAllRunningInstances(string file, bool allInstances = false)
    {
        try
        {
            string absolutePath = System.IO.Path.GetFullPath(file);
            string processName = System.IO.Path.GetFileName(file);
            List<Process> runningProcesses = [];
            Dictionary<int,uint> parentProcesses = [];
            int count = 0;
            DateTime startTime = DateTime.Now;
            TimeSpan timeout = TimeSpan.FromSeconds(15);
            foreach (Process process in Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(file)))
            {
                if ((allInstances && process.ProcessName == processName) || process.MainModule?.FileName == absolutePath)
                {
                    parentProcesses.Add(process.Id, GetProcessParent(process));
                    runningProcesses.Add(process);
                    process.Exited += (s, e) => { count--; };
                    count++;
                }
            }
            foreach (Process parent in runningProcesses.Where(p => !parentProcesses.ContainsKey((int)parentProcesses[p.Id])))
            {
                if (!parent.HasExited) parent.Kill(true);
            }
            while (count > 0 && DateTime.Now - startTime < timeout)
            {
                foreach(Process runningProcess in runningProcesses.Where(p => p is not null && !p.HasExited))
                {
                    runningProcess.Kill();
                }
                Thread.Sleep(10);
            }
        }
        catch { }
    }

    private static uint GetProcessParent(Process process)
    {
        using ManagementObjectSearcher searcher = new(new SelectQuery($"SELECT * FROM Win32_Process where ProcessId={process.Id}"));
        foreach (var item in searcher.Get()) return(uint)item["ParentProcessId"];
        return 0;
    }

    private static List<Process> GetProcessChildren(Process process)
    {
        List<Process> children = [];
        using ManagementObjectSearcher searcher = new(new SelectQuery($"SELECT * FROM Win32_Process where ParentProcessId={process.Id}"));
        foreach (var item in searcher.Get()) children.Add(Process.GetProcessById((int)item["ProcessId"]));
        return children;
    }

    private static Encoding GetEncodingByVersion(string path)
    {
        try
        {
            if (path is not null)
            {
                FileVersionInfo version = FileVersionInfo.GetVersionInfo(path);
                float versionNumber = float.Parse(version.FileMajorPart + "." + version.FileMinorPart, System.Globalization.CultureInfo.InvariantCulture);
                if (versionNumber >= 2.4f) return Encoding.UTF8;
                else return Encoding.ASCII;
            }
        }
        catch (Exception) { }
        return Encoding.UTF8;
    }

    #endregion

    #region PRIVATE METHODS

    private void Setup()
    {
        if (_path is null || !Exists)
        {
            throw (new Exception("ExecutablePathNotFound"));
        }
        if (!AllowMultipleInstances && CheckIfRunning(_path))
        {
            KillAllRunningInstances(_path);
            Thread.Sleep(100);
        }
        if (IsServer) CurrentEncoding = GetEncodingByVersion(_path);
        ProcessStartInfo info = new();
        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        info.WorkingDirectory = System.IO.Path.GetDirectoryName(_path);
        info.FileName = System.IO.Path.Combine(_path);
        info.RedirectStandardOutput = true;
        info.RedirectStandardInput = true;
        info.StandardOutputEncoding = CurrentEncoding;
        info.Environment.Add("ERRORLEVEL", "0");
        StringBuilder arguments = new();
        if ((IsServer || IsScanner)
            && !string.IsNullOrEmpty(ConfigFile)
            && ConfigFile != "casparcg.config")
            arguments.Append($@"""{ConfigFile}"" ");
        if (!string.IsNullOrEmpty(Args)) arguments.Append(Args);
        if (arguments.Length > 0) info.Arguments = arguments.ToString();

        if (Process is not null) Process.Dispose();
        Process = new()
        {
            StartInfo = info
        };
    }

    private void StopProcess()
    {
        if (!Enabled) return;
        Enabled = false;
        Process?.Kill();
    }

    private void StartProcess()
    {
        if (Launchpad.DisableStart) return;
        try
        {
            Setup();
            Enabled = true;
            if (Process is null) return;
            Process.Start();
            Process.OutputDataReceived += Process_OutputDataReceived;
            Process.ErrorDataReceived += Process_OutputDataReceived;
            Process.Exited += Process_Exited;
            Process.EnableRaisingEvents = true;
            Process.BeginOutputReadLine();
        }
        catch (Exception e)
        {
            if (e.Message == "ExecutablePathNotFound") OnProcessPathError();
            else OnProcessError(e);
            return;
        }
        StartupTime = DateTime.Now;
        OnPropertyChanged(nameof(Uptime));
        UptimeTimer.Interval = TimeSpan.FromSeconds(.2);
        UptimeTimer.Start();
        IsRunning = true;
        OnProcessStarted();
        if (AllowCommands && Commands.Any()) SendStartupCommands();
    }

    private void StartupTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        StartProcess();
    }

    private void SendStartupCommands()
    {
        if(CommandsDelay>0)
        {
            CurrentCommandIndex = 0;
            CommandsTimer.Interval = CommandsDelay;
            CommandsTimer.Start();
        }
        else
        {
            foreach(Command c in Commands)
            {
                SendCommand(c.Value);
            }
        }
    }

    private void SendStartupCommand(object? sender, EventArgs e)
    {
        if(CurrentCommandIndex >= Commands.Count)
        {
            CommandsTimer.Stop();
            return;
        }
        SendCommand(Commands[CurrentCommandIndex].Value);
        CurrentCommandIndex++;
    }

    private void SendCommand(string command)
    {
        if (Process is null || !IsRunning) return;
        try
        {
            if (IsServer && CurrentEncoding == Encoding.UTF8)
            {
            
                    byte[] data = Encoding.Unicode.GetBytes(command + Process.StandardInput.NewLine);
                    Process.StandardInput.BaseStream.Write(data, 0, data.Length);
                    Process.StandardInput.BaseStream.Flush();
            
            }
            else
            {
                Process.StandardInput.WriteLine(command);
            }
        }
        catch { }
    }

    private void Process_OutputDataReceived(object? sender, DataReceivedEventArgs e)
    {
        if (Application.Current is null || e.Data is null) return;
        bool remove_line = false;
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            if (BufferLines > 0 && Output.Count == BufferLines) remove_line = true;
            var line = new LogLine(e.Data, IsServer);
            try
            {
                if (IsServer && line.Data is string data && line.DirectOutput && data.Length > LOG_LEVEL_START.Length + LOG_LEVEL_END.Length)
                {
                    int start = LOG_LEVEL_START.Length;
                    int end = data.IndexOf(LOG_LEVEL_END, start);

                    if (data.IndexOf(LOG_LEVEL_START) == 0 && end > 0)
                    {
                        var level = LogLine.GetLevel(data[start..end]);
                        CurrentLogLevel = level;
                    }
                }
            }
            catch (Exception) {}
            if(SuppressEmptyLines)
            {
                if (line.DirectOutput && line.Data?.Length == 0) return;
                if (!line.DirectOutput && line.Message?.Length == 0) return;
            }
            if (remove_line) Output.RemoveAt(0);
            Output.Add(line);
        });
    }

    private void Process_Exited(object? sender, EventArgs e)
    {
        if (sender is Process process)
        {
            if (process.ExitCode == 0) Enabled = false;
            Debug.WriteLine($"Process exit code: {process.ExitCode}");
        }
        IsRunning = false;
        OnProcessExited();
        if (!Enabled) return;
        Thread.Sleep(100);
        StartProcess();
    }

    private void ChangeCasparLogLevel(LogLevel newLevel)
    {
        if(IsServer && IsRunning) SendCommand($"LOG LEVEL {newLevel.ToString()[1..]}");
    }

    #endregion

    #region PUBLIC METHODS

    public void Start(bool auto = false)
    {
        ExecutableThread = new(() =>
        {
            if (auto && StartupDelay > 0)
            {
                StartupTimer.Interval = StartupDelay;
                StartupTimer.Start();
            }
            else
            {
                StartProcess();
            }
        });
        ExecutableThread.Start();
    }

    public void Stop()
    {
        StopProcess();
    }

    public string PreviousHistoryCommand()
    {
        CurrentHistoryIndex++;
        return CurrentCommand = History[HistoryIndex];
    }

    public string NextHistoryCommand()
    {
        CurrentHistoryIndex--;
        return CurrentCommand = History[HistoryIndex];
    }

    public void Write()
    {
        if (CurrentCommand.Length == 0) return;
        Write(CurrentCommand);
        if (CurrentHistoryIndex == 0) History.Add("");
        else CurrentHistoryIndex = 0;
        CurrentCommand = "";
    }

    public void Write(string command)
    {
        SendCommand(command);
    }

    #endregion

    #region SPECIAL COMMANDS

    internal void OpenDiag()
    {
        if (!IsRunning) return;
        Write("DIAG");
    }

    internal void OpenGrid()
    {
        if (!IsRunning) return;
        Write("CHANNEL_GRID");
    }

    internal void ClearScannerDatabases()
    {
        if (!Exists || IsRunning) return;

        string scanner_dir;
        try
        {
            if (System.IO.Path.GetDirectoryName(Path) is string relative_dir)
                scanner_dir = System.IO.Path.GetFullPath(relative_dir);
            else
                throw new Exception("Path is relative");
        }
        catch (Exception)
        {
            scanner_dir = AppDomain.CurrentDomain.BaseDirectory;
        }
        string media_dir = System.IO.Path.Combine(scanner_dir, ".\\_media\\");
        string pad_dir = System.IO.Path.Combine(scanner_dir, ".\\pouch__all_dbs__\\");
        if (Directory.Exists(scanner_dir))
        {
            try
            {
                if (Directory.Exists(media_dir)) Directory.Delete(media_dir, true);
                if (Directory.Exists(pad_dir)) Directory.Delete(pad_dir, true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    internal void RebuildMediaDb()
    {
        if (!Exists) return;
        if (IsRunning)
        {
            Stop();
            ProcessExited += RebuildAndRestart;
        }
        else
        {
            ClearScannerDatabases();
        }
    }

    private void RebuildAndRestart(object? sender, EventArgs e)
    {
        ProcessExited -= RebuildAndRestart;
        ClearScannerDatabases();
        Start();
    }

    #endregion

    #region EVENTS

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new(propertyName)); }

    public event EventHandler<ExecutableEventArgs>? ProcessError;
    protected virtual void OnProcessError(Exception exception) { ProcessError?.Invoke(this, new(this, exception)); }

    public event EventHandler<ExecutableEventArgs>? ProcessPathError;
    protected virtual void OnProcessPathError() { ProcessPathError?.Invoke(this, new(this)); }

    public event EventHandler<ExecutableEventArgs>? ProcessExited;
    protected virtual void OnProcessExited() { ProcessExited?.Invoke(this, new(this)); }

    public event EventHandler<ExecutableEventArgs>? ProcessStarted;
    protected virtual void OnProcessStarted() { ProcessStarted?.Invoke(this, new(this)); }

    public event EventHandler? Modified;
    protected virtual void OnModified() { Modified?.Invoke(this, new()); }

    #endregion
}