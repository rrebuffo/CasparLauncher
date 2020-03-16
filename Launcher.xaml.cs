using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using WF = System.Windows.Forms;
using l = CasparLauncher.Properties.Resources;
using S = CasparLauncher.Properties.Settings;

namespace CasparLauncher
{
    public partial class Launcher : Window, INotifyPropertyChanged
    {
        public Launcher()
        {
            Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
            SetupEventHandlers();
            InitializeComponent();
            LoadSettings();
            StartExecutables();
        }

        private Settings Settings { get; set; }
        private Executable CasparExecutable;
        private WF.NotifyIcon TrayIcon = new WF.NotifyIcon();
        private WindowState PreviousState = WindowState.Normal;
        private bool InTray = false;
        
        public bool IsShiftDown
        {
            get
            {
                return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            }
            set
            {
                OnPropertyChanged("IsShiftDown");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        #region STARTUP AND SHUTDOWN

        private void SetupEventHandlers()
        {
            StateChanged += Launcher_StateChanged;
            Loaded += Launcher_Initialized;
            KeyDown += Launcher_KeyDown;
            KeyUp += Launcher_KeyUp;
            Activated += Launcher_Activated;
            SizeChanged += Launcher_SizeChanged;
            LocationChanged += Launcher_LocationChanged;
        }

        private void LoadSettings()
        {
            Settings = new Settings();
            if (Settings.Executables.Where(e => e.IsServer).Any()) CasparExecutable = Settings.Executables.Where(e => e.IsServer).First();
            DataContext = Settings;
            PreviousState = S.Default.LauncherWindowState;
            WindowState = PreviousState;
            Settings.SelectedLanguage = (Languages)S.Default.ForcedLanguage;
            Settings.ExecutableError += Settings_ExecutableError;
            Settings.ExecutablePathError += Settings_ExecutablePathError;
            Settings.ExecutableExited += Settings_ExecutableExited;
        }

        private void Settings_ExecutablePathError(object sender, ExecutableEventArgs e)
        {
            TrayIcon.ShowBalloonTip(3000, string.Format("{0}: {1}",e.Executable.Name,l.ExecutableNotFoundWarningCaption), l.ExecutableNotFoundWarningMessage, WF.ToolTipIcon.Warning);
        }

        private void Settings_ExecutableError(object sender, ExecutableEventArgs e)
        {
            if (TrayIcon is null) return;
            TrayIcon.ShowBalloonTip(3000, e.Executable.Name, l.ExecutableErrorMessage, WF.ToolTipIcon.Error);
        }

        private void Settings_ExecutableExited(object sender, ExecutableEventArgs e)
        {
            if (TrayIcon is null) return;
            TrayIcon.ShowBalloonTip(3000, e.Executable.Name, l.ExecutableStoppedMessage, WF.ToolTipIcon.Info);
        }

        private void StartExecutables()
        {
            foreach (Executable ex in Settings.Executables) if (ex.AutoStart && !IsShiftDown) ex.Start(true);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            bool wasCodeClosed = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
            if (!wasCodeClosed)
            {
                e.Cancel = true;
                MinimizeToTray();
            }

            base.OnClosing(e);
        }

        public void Shutdown(bool prompt = false)
        {
            if(prompt)
            {
                MessageBoxResult close = MessageBox.Show(l.ClosePromptMessage, l.ClosePromptCaption, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (close)
                {
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.No:
                    default:
                        return;
                }
            }

            S.Default.Save();
            S.Default.Upgrade();

            TrayIcon.Visible = false;

            foreach(Executable ex in Settings.Executables) if (ex.Running) ex.Stop();

            Application.Current.Shutdown();
        }

        private bool IsSystemThemeLight()
        {
            string keypath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            string value = "SystemUsesLightTheme";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keypath))
            {
                object obj = key?.GetValue(value);
                if (obj is null) return false;
                return (int)obj > 0;
            }
        }

        private void LauncherLoaded(object sender, RoutedEventArgs e)
        {
            SetupTray();
        }

        #endregion

        #region TRAY ICON

        private void SetupTray()
        {
            SetTrayIcon();
            TrayIcon.MouseDown += TrayIcon_MouseDown;
            TrayIcon.MouseUp += TrayIcon_MouseUp;
            TrayIcon.Visible = true;
        }

        private void SetTrayIcon()
        {
            double size = 16 * PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;
            int iconsize = 16;
            if (size >= 20) iconsize = 20;
            if (size >= 24) iconsize = 24;
            if (size >= 32) iconsize = 32;
            if (size >= 48) iconsize = 256;
            string iconpath = IsSystemThemeLight() ? "NotifyIconLight.ico" : "NotifyIconDark.ico";
            Stream IconStream = Application.GetResourceStream(new Uri($@"pack://application:,,,/Resources/{iconpath}")).Stream;
            TrayIcon.Icon = new Icon(IconStream, new System.Drawing.Size(iconsize, iconsize));
            IconStream.Dispose();
        }

        private void StartAll(object sender, RoutedEventArgs e)
        {
            foreach(Executable ex in Settings.Executables.Where(ex => !ex.Running)) ex.Start();
        }

        private void StopAll(object sender, RoutedEventArgs e)
        {
            foreach (Executable ex in Settings.Executables.Where(ex => ex.Running)) ex.Stop();
        }

        private void RestartAll(object sender, RoutedEventArgs e)
        {
            foreach (Executable ex in Settings.Executables.Where(ex => ex.Running)) ex.Process.Kill();
        }

        private Executable GetItemExecutable(object item)
        {
            object ex = ((FrameworkElement)item).DataContext;
            if (ex is Executable) return ex as Executable;
            else return null;
        }

        private void Start_item_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            ex.Start();
        }

        private void Stop_item_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            ex.Stop();
        }

        private void Restart_item_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            ex.Process.Kill();
        }

        private void Config_item_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            OpenExecutableConfig(ex);
        }

        private void Rebuild_item_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            RebuildMediaDb(ex);
        }

        private void Diag_item_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            OpenDiag(ex);
        }

        private void TrayMenu_ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Shutdown(true);
        }

        private void TrayIcon_MouseDown(object sender, WF.MouseEventArgs e)
        {
            if (e.Button != WF.MouseButtons.Right) return;
            TrayMenu.IsOpen = true;
            Activate();
        }

        private void TrayIcon_MouseUp(object sender, WF.MouseEventArgs e)
        {
            if (e.Button != WF.MouseButtons.Left) return;
            ActivateInstance();
        }
        #endregion

        #region SPECIAL COMMANDS

        private void OpenDiag(Executable ex)
        {
            if (ex is null || !ex.Running) return;
            ex.Write("DIAG");
        }

        private void ClearScannerDatabases(Executable ex)
        {
            if (ex is null || !ex.Exists || ex.Running) return;

            string scanner_dir;
            try
            {
                scanner_dir = System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(ex.Path));
            }
            catch(Exception)
            {
                scanner_dir = AppDomain.CurrentDomain.BaseDirectory;
            }
            string media_dir = System.IO.Path.Combine(scanner_dir, ".\\_media\\");
            string pad_dir = System.IO.Path.Combine(scanner_dir, ".\\pouch__all_dbs__\\");
            if (Directory.Exists(scanner_dir) && Directory.Exists(media_dir) && Directory.Exists(pad_dir))
            {
                try
                {
                    Directory.Delete(media_dir,true);
                    Directory.Delete(pad_dir,true);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void RebuildMediaDb(Executable ex)
        {
            if (ex is null || !ex.Exists) return;
            if (ex.Running)
            {
                ex.Stop();
                ex.ProcessExited += RebuildAndRestart;
            }
            else
            {
                ClearScannerDatabases(ex);
            }
        }

        private void RebuildAndRestart(object sender, EventArgs e)
        {
            Executable ex = sender as Executable;
            ex.ProcessExited -= RebuildAndRestart;
            ClearScannerDatabases(ex);
            ex.Start();
        }

        #endregion
        
        #region UI METHODS

        // Status TabItem UI Handlers

        private void ExecutableButton_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            if (ex.Exists) ex.IsSelected = true;
            else ConfigTab.IsSelected = true;
        }

        // Executables TabItem UI Handlers
        private void ScrollToEnd(object target)
        {
            ((TextBox)target).ScrollToEnd();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            ex.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            ex.Stop();
        }

        private void RebuildMedia_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            RebuildMediaDb(ex);
        }

        private void OpenDiag_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            OpenDiag(ex);
        }

        private void ConsoleOutputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ScrollToEnd(sender);
        }

        private void ConsoleOutputTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollToEnd(sender);
        }


        // Config TabItem UI Handlers

        private void AddExecutableButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = false,
                Filter = l.Resources.FileDialogFilterDescription + " (*.EXE)|*.exe"
            };
            Executable ex;
            if (openFileDialog.ShowDialog() == true)
            {
                ex = Settings.AddExecutable(openFileDialog.FileName);
            }
            else
            {
                ex = Settings.AddExecutable();
            }
            */
            OpenExecutableConfig(Settings.AddExecutable());
        }

        private void ExecutableConfig_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = ((FrameworkElement)sender).DataContext as Executable;
            OpenExecutableConfig(ex);
        }

        private void OpenExecutableConfig(Executable ex)
        {
            ExecutableOptions executableOptions = new ExecutableOptions()
            {
                DataContext = ex,
                Owner = this
            };
            executableOptions.ShowDialog();
        }

        private void ExecutableRemove_Click(object sender, RoutedEventArgs e)
        {
            Executable ex = GetItemExecutable(sender);
            if (ex is null) return;
            if (!ex.Exists)
            {
                Settings.Executables.Remove(ex);
                return;
            }
            MessageBoxResult remove = MessageBox.Show(l.DeleteExecutablePromptMessage, l.DeleteExecutablePromptCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (remove)
            {
                case MessageBoxResult.Yes:
                    Settings.Executables.Remove(ex);
                    break;
                case MessageBoxResult.No:
                default:
                    return;
            }
        }

        private void NewConfigButton_Click(object sender, RoutedEventArgs e)
        {
            EditConfig();
        }

        private void EditConfigButton_Click(object sender, RoutedEventArgs e)
        {
            string configPath = "";
            if (!IsShiftDown && CasparExecutable != null && CasparExecutable.Exists) configPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(CasparExecutable.Path), "casparcg.config");
            EditConfig(configPath);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        #endregion

        #region APP WINDOW METHODS

        private void Launcher_StateChanged(object sender, EventArgs e)
        {
            if(WindowState == WindowState.Minimized)
            {
                if(InTray) ShowInTaskbar = false;
            }
            else
            {
                ShowInTaskbar = true;
                S.Default.LauncherWindowState = WindowState;
                S.Default.Save();
                S.Default.Upgrade();
                Activate();
            }
        }

        private void Launcher_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Normal) Settings.SaveWindowPosition(true);
        }

        private void Launcher_LocationChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal) Settings.SaveWindowPosition(true);
        }

        public void ActivateInstance()
        {
            InTray = false;
            if(WindowState == WindowState.Minimized) WindowState = PreviousState;
            Topmost = true;
            Activate();
            Topmost = false;
            Focus();
        }

        private void MinimizeToTray()
        {
            InTray = true;
            PreviousState = WindowState;
            WindowState = WindowState.Minimized;
        }

        private void Launcher_Initialized(object sender, EventArgs e)
        {
            MinimizeToTray();
        }

        private void Launcher_Activated(object sender, EventArgs e)
        {
            OnPropertyChanged("IsShiftDown");
        }

        private void LauncherWindow_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            SetTrayIcon();
        }

        private void Launcher_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) IsShiftDown = true;
        }

        private void Launcher_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyUp(Key.LeftShift) && Keyboard.IsKeyUp(Key.RightShift)) IsShiftDown = false;

            IInputElement focused = FocusManager.GetFocusedElement(this);
            if (focused is TextBox)
            {
                TextBox target = focused as TextBox;
                if (target.Name == "ConsoleCommandTextBox")
                {
                    object context = target.DataContext;
                    Executable ex = (context is Executable) ? context as Executable : null;
                    if (ex is null || !ex.AllowCommands) return;

                    if (e.Key == Key.Enter)
                    {
                        ex.Write();
                    }
                    if (e.Key == Key.Up)
                    {
                        ex.PreviousHistoryCommand();
                        target.CaretIndex = target.Text.Length;
                    }
                    if (e.Key == Key.Down)
                    {
                        ex.NextHistoryCommand();
                        target.CaretIndex = target.Text.Length;
                    }
                }
            }
        }

        #endregion

        #region CONFIG METHODS

        private void EditConfig(string path = null)
        {
            ConfigFile file = new ConfigFile();

            if(path != null)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    file.File = path;
                }
                else
                {
                    Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
                    {
                        Multiselect = false,
                        Filter = l.ConfigFileDialogFilterDescription + "|*.config"
                    };

                    if (openFileDialog.ShowDialog() == true)
                    {
                        file.File = openFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
                if (file.File is null) return;
                file.LoadConfigFile();
            }

            ConfigEditor configWindow = new ConfigEditor();
            configWindow.Owner = this;
            configWindow.DataContext = file;
            configWindow.ShowDialog();
        }

        #endregion
    }

}
