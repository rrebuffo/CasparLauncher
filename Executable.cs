using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace CasparLauncher
{
    class Executable : INotifyPropertyChanged
    {
        const string LOG_LEVEL_START = "Logging [";
        const string LOG_LEVEL_END = "] or higher severity to log";

        public Settings Settings { get; set; }
        private DispatcherTimer StartupTimer = new DispatcherTimer();
        private DispatcherTimer CommandsTimer = new DispatcherTimer();
        private DispatcherTimer UptimeTimer = new DispatcherTimer();
        private int CurrentCommandIndex = 0;
        private DateTime StartupTime;

        public Executable()
        {
            History.Add("");
            StartupTimer.Tick += StartupTimer_Tick;
            UptimeTimer.Tick += UptimeTimer_Tick;
            PropertyChanged += Executable_PropertyChanged;
            Commands.CollectionChanged += Commands_CollectionChanged;
        }

        private void Commands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach(Command c in Commands)
            {
                c.PropertyChanged -= CommandChanged;
                c.PropertyChanged += CommandChanged;
            }
            OnModified();
        }

        private void CommandChanged(object sender, PropertyChangedEventArgs e)
        {
            OnModified();
        }

        private void Executable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Path" ||
                e.PropertyName == "Name" ||
                e.PropertyName == "Args" ||
                e.PropertyName == "AutoStart" ||
                e.PropertyName == "StartupDelay" ||
                e.PropertyName == "CommandsDelay" ||
                e.PropertyName == "AllowCommands" ||
                e.PropertyName == "AllowMultipleInstances")
            {
                OnModified();
            }
        }

        private void UptimeTimer_Tick(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Uptime));
        }

        public XElement Config
        {
            get
            {
                XElement exec_ex = new XElement("executable",
                    new XElement("path", Path),
                    new XElement("name", Name),
                    new XElement("args", Args),
                    new XElement("auto", AutoStart.ToString()),
                    new XElement("sdel", StartupDelay.ToString()),
                    new XElement("acmd", AllowCommands.ToString()),
                    new XElement("amin", AllowMultipleInstances.ToString()),
                    new XElement("cdel", CommandsDelay.ToString()),
                    new XElement("commands",
                    from c in Commands
                    select new XElement("cmd", c.Value))
                );
                return exec_ex;
            }
        }

        #region PROPERTIES

        private string _path = null;
        public string Path
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
                    OnPropertyChanged("Path");
                    OnPropertyChanged("Exists");
                    OnPropertyChanged("Icon");
                    OnPropertyChanged("IsServer");
                    OnPropertyChanged("IsScanner");
                }
            }
        }

        private bool? _isServer;
        public bool IsServer
        {
            get
            {
                if(_isServer.HasValue) return _isServer.Value;
                if (!Exists) return false;
                _isServer = System.IO.Path.GetFileNameWithoutExtension(_path).ToLower() == "casparcg";
                return _isServer.Value;
            }
        }

        private bool? _isScanner;
        public bool IsScanner
        {
            get
            {
                if (_isScanner.HasValue) return _isScanner.Value;
                if (!Exists) return false;
                _isScanner = System.IO.Path.GetFileNameWithoutExtension(_path).ToLower() == "scanner";
                return _isServer.Value;
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
                    OnPropertyChanged("IsSelected");
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
                    OnPropertyChanged("Enabled");
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
                    OnPropertyChanged("Name");
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
                    OnPropertyChanged("Args");
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
                    OnPropertyChanged("AutoStart");
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
                    OnPropertyChanged("StartupDelay");
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
                    OnPropertyChanged("CommandsDelay");
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
                    OnPropertyChanged("AllowCommands");
                }
            }
        }

        private bool _running = false;
        public bool Running
        {
            get
            {
                return _running;
            }
            set
            {
                if (_running != value)
                {
                    _running = value;
                    OnPropertyChanged("Running");
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
                    OnPropertyChanged("AllowMultipleInstances");
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
                        OnPropertyChanged("CurrentCommand");
                    }
                    else
                    {
                        _currentcommand = value;
                        History[History.Count-1] = value;
                        CurrentHistoryIndex = 0;
                        OnPropertyChanged("CurrentCommand");
                    }
                }
                else
                {
                    if (_currentcommand != value)
                    {
                        _currentcommand = value;
                        History[History.Count - 1] = value;
                        OnPropertyChanged("CurrentCommand");
                    }
                }
            }
        }

        public ObservableCollection<LogLine> Output { get; set; } = new ObservableCollection<LogLine>();

        private Process _process = null;
        public Process Process
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
                    OnPropertyChanged("Process");
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

        public Icon Icon
        {
            get
            {
                if (!Exists) return null;
                return Icon.ExtractAssociatedIcon(_path);
            }
        }

        public TimeSpan Uptime => DateTime.Now - StartupTime;

        public ObservableCollection<Command> Commands { get; set; } = new ObservableCollection<Command>();

        public ObservableCollection<string> History { get; set; } = new ObservableCollection<string>();

        #endregion

        #region PRIVATE METHODS

        private bool IsRunning(string file)
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

        private void KillAllRunningInstances(string file)
        {
            foreach (Process p in Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(file)))
            {
                KillTreeOf(p.Id);
            }
        }

        private void KillTreeOf(int id)
        {
            KillChildsOf((uint)id);
            try
            {
                if (Process.GetProcessById(id) is Process p)
                {
                    if (!p.HasExited)
                    {
                        Process.GetProcessById(id).Kill();
                    }
                }
            }
            catch (ArgumentException) { }
        }

        private void KillChildsOf(uint id)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new SelectQuery($"SELECT * FROM Win32_Process where ParentProcessId={id}")))
            {
                foreach (var child in searcher.Get())
                {
                    uint pid = (uint)child["ProcessId"];
                    KillChildsOf(pid);
                    Process.GetProcessById((int)pid).Kill();
                }
            }
        }

        private void Setup()
        {
            if (!Exists)
            {
                throw (new Exception("ExecutablePathNotFound"));
            }
            if (!AllowMultipleInstances && IsRunning(_path)) KillAllRunningInstances(_path);

            ProcessStartInfo info = new ProcessStartInfo();
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.WorkingDirectory = System.IO.Path.GetDirectoryName(_path);
            info.FileName = System.IO.Path.Combine(_path);
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.Environment.Add("ERRORLEVEL", "0");

            if (!string.IsNullOrEmpty(Args)) info.Arguments = Args;

            if (Process != null) Process.Dispose();
            Process = new Process();
            Process.StartInfo = info;
        }

        private void StopProcess()
        {
            if (!Enabled) return;
            Enabled = false;
            Process.Kill();
        }

        private void StartProcess()
        {
            try
            {
                Setup();
            }
            catch (Exception e)
            {
                if (e.Message == "ExecutablePathNotFound") OnProcessPathError();
                else
                {
                    OnProcessError();
                }
                return;
            }

            Enabled = true;
            Process.Start();
            Process.StandardInput.AutoFlush = true;
            Process.OutputDataReceived += Process_OutputDataReceived;
            Process.Exited += Process_Exited;
            Process.EnableRaisingEvents = true;
            Process.BeginOutputReadLine();
            StartupTime = DateTime.Now;
            OnPropertyChanged(nameof(Uptime));
            UptimeTimer.Interval = TimeSpan.FromSeconds(.1);
            UptimeTimer.Start();
            Running = true;

            if(AllowCommands && Commands.Any()) SendStartupCommands();
        }

        private void StartupTimer_Tick(object sender, EventArgs e)
        {
            StartupTimer.Stop();
            StartProcess();
        }

        private void SendStartupCommands()
        {
            if(CommandsDelay>0)
            {
                CurrentCommandIndex = 0;
                CommandsTimer.Interval = TimeSpan.FromMilliseconds(CommandsDelay);
                CommandsTimer.Tick += SendStartupCommand;
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

        private void SendStartupCommand(object sender, EventArgs e)
        {
            if(CurrentCommandIndex >= Commands.Count())
            {
                CommandsTimer.Tick -= SendStartupCommand;
                CommandsTimer.Stop();
                return;
            }
            SendCommand(Commands[CurrentCommandIndex].Value);
            CurrentCommandIndex++;
        }

        private void SendCommand(string command)
        {
            Process.StandardInput.WriteLine(command);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (Application.Current is null) return;
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (Output.Count == Settings.BufferLines) Output.RemoveAt(0);
                var line = new LogLine(e.Data, IsServer);
                try
                {
                    if (IsServer && line.Data is string data && line.DirectOutput && data.Length > LOG_LEVEL_START.Length + LOG_LEVEL_END.Length)
                    {
                        int start = LOG_LEVEL_START.Length;
                        int end = data.IndexOf(LOG_LEVEL_END, start);

                        if (data.IndexOf(LOG_LEVEL_START) == 0 && end > 0)
                        {
                            var level = LogLine.GetLevel(data.Substring(start, end - start));
                            CurrentLogLevel = level;
                        }
                    }
                }
                catch (Exception) {}
                
                Output.Add(line);
            }));
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            Process process = sender as Process;
            if (process != null)
            {
                if (process.ExitCode == 0) Enabled = false;
                Debug.WriteLine($"Process exit code: {process.ExitCode}");
            }
            Running = false;
            OnProcessExited();
            if (!Enabled) return;
            Thread.Sleep(100);
            StartProcess();
        }

        private void ChangeCasparLogLevel(LogLevel newLevel)
        {
            if(IsServer && Running) SendCommand($"LOG LEVEL {newLevel.ToString().Substring(1)}");
        }

        #endregion

        #region PUBLIC METHODS

        public void Start(bool auto = false)
        {
            if (auto && StartupDelay > 0)
            {
                StartupTimer.Interval = TimeSpan.FromMilliseconds(StartupDelay);
                StartupTimer.Start();
            }
            else
            {
                StartProcess();
            }
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

        #region EVENTS

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        public event EventHandler<ExecutableEventArgs> ProcessError;
        protected virtual void OnProcessError() { ProcessError?.Invoke(this, new ExecutableEventArgs(this)); }

        public event EventHandler<ExecutableEventArgs> ProcessPathError;
        protected virtual void OnProcessPathError() { ProcessPathError?.Invoke(this, new ExecutableEventArgs(this)); }

        public event EventHandler<ExecutableEventArgs> ProcessExited;
        protected virtual void OnProcessExited() { ProcessExited?.Invoke(this, new ExecutableEventArgs(this)); }

        public event EventHandler Modified;
        protected virtual void OnModified() { Modified?.Invoke(this, new EventArgs()); }

        #endregion
    }

    public class LogLine
    {
        public string Data { get; private set; } = "";
        public string Message { get; private set; } = "";
        public LogLevel Level { get; private set; } = LogLevel._info;
        public string Timestamp { get; private set; } = "";
        public bool DirectOutput { get; private set; } = true;
        private static Regex find = new Regex(@"^\[([0-9]{4}-[0-9]{2}-[0-9]{2}\s[0-9]{2}:[0-9]{2}:[0-9]{2}\.[0-9]+)\]\s\[(.+?)\]\s*(.*)");

        public LogLine(string data, bool serverFormat)
        {
            Data = data;
            if (string.IsNullOrEmpty(data) || !serverFormat) return;
            bool has_content = data.Length > 36;

            string to_match = has_content ? data.Substring(0, 36) : data;
            Match linedata = find.Match(to_match);
            if(linedata.Groups.Count>1)
            {
                Timestamp = linedata.Groups[1].Value;
                Level = GetLevel(linedata.Groups[2].Value);
                Message = has_content ? data.Substring(36) : "";
                DirectOutput = false;
            }
        }

        public static LogLevel GetLevel(string value)
        {
            var level = LogLevel._info;
            switch (value)
            {
                case "fatal":
                    level = LogLevel._fatal;
                    break;
                case "error":
                    level = LogLevel._error;
                    break;
                case "warning":
                    level = LogLevel._warning;
                    break;
                case "info":
                    level = LogLevel._info;
                    break;
                case "debug":
                    level = LogLevel._debug;
                    break;
                case "trace":
                    level = LogLevel._trace;
                    break;
            }
            return level;
        }
    }

    class ExecutableEventArgs : EventArgs
    {
        public Executable Executable { get; private set; }

        public ExecutableEventArgs(Executable executable)
        {
            Executable = executable;
        }
    }
}
