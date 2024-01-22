﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using L = CasparLauncher.Properties.Resources;
using S = CasparLauncher.Properties.Settings;

namespace CasparLauncher
{
    class Launchpad : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        public event EventHandler<ExecutableEventArgs> ExecutableError;
        protected virtual void OnExecutableError(Executable ex) { ExecutableError?.Invoke(this, new ExecutableEventArgs(ex)); }

        public event EventHandler<ExecutableEventArgs> ExecutablePathError;
        protected virtual void OnExecutablePathError(Executable ex) { ExecutablePathError?.Invoke(this, new ExecutableEventArgs(ex)); }

        public event EventHandler<ExecutableEventArgs> ExecutableExited;
        protected virtual void OnExecutableExited(Executable ex) { ExecutableExited?.Invoke(this, new ExecutableEventArgs(ex)); }

        public ObservableCollection<Executable> Executables { get; set; } = new ObservableCollection<Executable>();
        DispatcherTimer SaveTimer = new DispatcherTimer();

        public Launchpad()
        {
            LoadSettings();
            SaveTimer.Interval = TimeSpan.FromMilliseconds(100);
            SaveTimer.Tick += SaveWindowPosition;
            Executables.CollectionChanged += Executables_CollectionChanged;
        }

        private void LoadSettings()
        {
            S.Default.Upgrade();
            _bufferLines = S.Default.BufferLines;
            _openAtLogin = S.Default.OpenAtLogin;
            PosX = S.Default.LauncherWindowPosX;
            PosY = S.Default.LauncherWindowPosY;
            Width = S.Default.LauncherWindowWidth;
            Height = S.Default.LauncherWindowHeight;
            ParseSettings(S.Default.Executables);
            InitExecutables();
        }

        private void ParseSettings(string settings)
        {
            try
            {
                XElement executables = XElement.Parse(settings);
                foreach (XElement executable in executables.Elements())
                {
                    Executable new_executable = new Executable()
                    {
                        Settings = this,
                        Path = executable.Element("path").Value,
                        Name = executable.Element("name").Value,
                        Args = executable.Element("args").Value,
                        AutoStart = bool.Parse(executable.Element("auto").Value),
                        StartupDelay = int.Parse(executable.Element("sdel").Value),
                        AllowCommands = bool.Parse(executable.Element("acmd").Value),
                        AllowMultipleInstances = bool.Parse(executable.Element("amin").Value),
                        CommandsDelay = int.Parse(executable.Element("cdel").Value)
                    };
                    foreach(XElement command in executable.Element("commands").Descendants())
                    {
                        Command new_command = new Command()
                        {
                            Value = command.Value
                        };
                        new_executable.Commands.Add(new_command);
                    }
                    Executables.Add(new_executable);
                }
            }
            catch(Exception)
            {
                LoadDefaults();
            }
        }

        private void LoadDefaults()
        {
            Executable server = new Executable()
            {
                Settings = this,
                Name = "Server",
                Path = "casparcg.exe",
                AllowCommands = true,
                AutoStart = true
            };
            Executable scanner = new Executable()
            {
                Settings = this,
                Name = "Scanner",
                Path = "scanner.exe",
                AllowCommands = false,
                AutoStart = true
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
                executable.ProcessExited += Executable_ProcessExited;
            }
        }

        private void Executable_ProcessError(object sender, ExecutableEventArgs e)
        {
            OnExecutableError(e.Executable);
        }

        private void Executable_ProcessPathError(object sender, ExecutableEventArgs e)
        {
            OnExecutablePathError(e.Executable);
        }

        private void Executable_ProcessExited(object sender, ExecutableEventArgs e)
        {
            OnExecutableExited(e.Executable);
        }

        private void Executables_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InitExecutables();
            SaveChanges();
        }

        private void ExecutableModified(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            XElement config = new XElement("executables",
                from e in Executables
                select e.Config
            );
            
            S.Default.Executables = config.ToString(SaveOptions.DisableFormatting);
            S.Default.Save();
        }

        public static int DragThreshold
        {
            get
            {
                return 10;
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
                    if (value != S.Default.BufferLines)
                    {
                        S.Default.BufferLines = value;
                        S.Default.Save();
                        S.Default.Upgrade();
                    }
                    OnPropertyChanged("BufferLines");
                }
            }
        }

        private bool _openAtLogin = false;
        public bool OpenAtLogin
        {
            get
            {
                return _openAtLogin;
            }
            set
            {
                if (_openAtLogin != value)
                {
                    _openAtLogin = value;
                    if (value != S.Default.OpenAtLogin)
                    {
                        S.Default.OpenAtLogin = value;
                        S.Default.Save();
                        S.Default.Upgrade();
                    }
                    SetStartup(value);
                    OnPropertyChanged("OpenAtLogin");
                }
            }
        }

        private Languages _lang = 0;
        public Languages SelectedLanguage
        {
            get
            {
                return _lang;
            }
            set
            {
                if (_lang != value)
                {
                    _lang = value;
                    S.Default.ForcedLanguage = (int)value;
                    S.Default.Save();
                    S.Default.Upgrade();
                    OnPropertyChanged("SelectedLanguage");
                }
            }
        }

        private double _posX = 0;
        public double PosX
        {
            get
            {
                return _posX;
            }
            set
            {
                if (_posX != value)
                {
                    _posX = value;
                    OnPropertyChanged("PosX");
                }
            }
        }

        private double _posY = 0;
        public double PosY
        {
            get
            {
                return _posY;
            }
            set
            {
                if (_posY != value)
                {
                    _posY = value;
                    OnPropertyChanged("PosY");
                }
            }
        }

        private double _width = 1000;
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    OnPropertyChanged("Width");
                }
            }
        }

        private double _height = 600;
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        public void SaveWindowPosition()
        {
            SaveTimer.Stop();
            S.Default.LauncherWindowPosX = _posX;
            S.Default.LauncherWindowPosY = _posY;
            S.Default.LauncherWindowWidth = _width;
            S.Default.LauncherWindowHeight = _height;
            S.Default.Save();
            S.Default.Upgrade();
        }

        public void SaveWindowPosition(bool delay)
        {
            SaveTimer.Stop();
            SaveTimer.Start();
        }

        private void SaveWindowPosition(object sender, EventArgs e)
        {
            SaveWindowPosition();
        }

        private void SetStartup(bool set = true)
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (set) regKey.SetValue("CasparLauncher", Process.GetCurrentProcess().MainModule.FileName);
            else regKey.DeleteValue("CasparLauncher", false);

        }

        public Executable AddExecutable(string path = null)
        {
            Executable new_executable = new Executable()
            {
                Settings = this,
                Name = L.NewExecutableName,
                Path = path,
                AllowCommands = false
            };
            Executables.Add(new_executable);
            return new_executable;
        }


    }
}
