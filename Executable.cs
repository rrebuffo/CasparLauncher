using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Linq;

namespace CasparLauncher
{
    class Executable : INotifyPropertyChanged
    {

        public Settings Settings { get; set; }
        private DispatcherTimer StartupTimer = new DispatcherTimer();
        private DispatcherTimer CommandsTimer = new DispatcherTimer();
        private int CurrentCommandIndex = 0;

        public Executable()
        {
            History.Add("");
            StartupTimer.Tick += StartupTimer_Tick;
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

        public bool IsServer
        {
            get
            {
                if (!Exists) return false;
                return System.IO.Path.GetFileNameWithoutExtension(_path).ToLower() == "casparcg";
            }
        }

        public bool IsScanner
        {
            get
            {
                if (!Exists) return false;
                return System.IO.Path.GetFileNameWithoutExtension(_path).ToLower() == "scanner";
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

        private string _output = "";
        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                if (_output != value)
                {
                    _output = value;
                    OnPropertyChanged("Output");
                }
            }
        }

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
                if (_path is null) return false;
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
            catch (Exception e)
            {
                return false;
            }
        }

        private void KillAllRunningInstances(string file)
        {
            foreach (Process p in Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(file)))
            {
                p.Kill();
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
                OnProcessError();
                return;
            }

            Enabled = true;
            Process.Start();
            Process.StandardInput.AutoFlush = true;
            Process.OutputDataReceived += Process_OutputDataReceived;
            Process.Exited += Process_Exited;
            Process.EnableRaisingEvents = true;
            Process.BeginOutputReadLine();
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
            LinkedList<string> buffer = new LinkedList<string>(Output.Split('\n'));
            if (buffer.Count > Settings.BufferLines) buffer.RemoveFirst();
            buffer.AddLast(e.Data);
            Output = string.Join("\n", buffer);
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

            for(var i=0; i<History.Count;i++)
            {
                Debug.WriteLine($"{i.ToString("dd")}: {History[i]}");
            }
        }

        public void Write(string command)
        {
            SendCommand(command);
        }

        #endregion

        #region EVENTS

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        public event EventHandler ProcessError;
        protected virtual void OnProcessError() { ProcessError?.Invoke(this, new EventArgs()); }

        public event EventHandler ProcessExited;
        protected virtual void OnProcessExited() { ProcessExited?.Invoke(this, new EventArgs()); }

        public event EventHandler Modified;
        protected virtual void OnModified() { Modified?.Invoke(this, new EventArgs()); }

        #endregion
    }
}
