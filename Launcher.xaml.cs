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
using l = CasparLauncher.Properties;
using S = CasparLauncher.Properties.Settings;

namespace CasparLauncher
{
    public partial class Launcher : Window, INotifyPropertyChanged
    {
        public Launcher()
        {
            SetupEventHandlers();
            InitializeComponent();
            LoadSettings();
            SetupTray();
            StartExecutables();
        }

        private Settings Settings { get; set; }
        private Executable CasparExecutable;
        private Executable ScannerExecutable;
        private WF.NotifyIcon TrayIcon;
        private WF.ContextMenu TrayMenu;
        private WindowState PreviousState = WindowState.Normal;
        private ObservableCollection<string> ServerStartupCommands { get; set; } = new ObservableCollection<string>();
        
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
        }

        private void LoadSettings()
        {
            Settings = new Settings();
            if (Settings.Executables.Where(e => e.IsServer).Any()) CasparExecutable = Settings.Executables.Where(e => e.IsServer).First();
            if (Settings.Executables.Where(e => e.IsScanner).Any()) ScannerExecutable = Settings.Executables.Where(e => e.IsScanner).First();
            DataContext = Settings;
            Settings.SelectedLanguage = (Languages)S.Default.ForcedLanguage;
        }

        private void StartExecutables()
        {
            foreach (Executable executable in Settings.Executables) if (executable.AutoStart) executable.Start(true);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            bool wasCodeClosed = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
            if (!wasCodeClosed)
            {
                MessageBoxResult close = MessageBox.Show(l.Resources.ClosePromptMessage, l.Resources.ClosePromptCaption, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (close)
                {
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.No:
                    default:
                        e.Cancel = true;
                        return;
                }
            }

            S.Default.Save();
            S.Default.Upgrade();

            TrayIcon.Visible = false;

            foreach(Executable executable in Settings.Executables) if (executable.Running) executable.Stop();

            base.OnClosing(e);
        }

        #endregion

        #region TRAY ICON

        private void SetupTray()
        {
            TrayIcon = new WF.NotifyIcon();
            Stream IconStream = Application.GetResourceStream(new Uri(@"pack://application:,,,/Resources/NotifyIcon.ico")).Stream;
            TrayIcon.Icon = new Icon(IconStream);
            IconStream.Dispose();

            TrayIcon.MouseUp += TrayIcon_MouseUp;

            TrayIcon.Visible = true;

            TrayMenu = new WF.ContextMenu();
            WF.MenuItem TrayMenu_DiagWindow = new WF.MenuItem();
            WF.MenuItem TrayMenu_RebuildMedia = new WF.MenuItem();
            WF.MenuItem TrayMenu_CasparItem = new WF.MenuItem();
            WF.MenuItem TrayMenu_ScannerItem = new WF.MenuItem();
            WF.MenuItem TrayMenu_ExitItem = new WF.MenuItem();


            TrayMenu.MenuItems.Add(TrayMenu_DiagWindow);
            TrayMenu.MenuItems.Add(TrayMenu_RebuildMedia);
            TrayMenu.MenuItems.Add("-");
            TrayMenu.MenuItems.Add(TrayMenu_CasparItem);
            TrayMenu.MenuItems.Add(TrayMenu_ScannerItem);
            TrayMenu.MenuItems.Add("-");
            TrayMenu.MenuItems.Add(TrayMenu_ExitItem);

            // RebuildDB Menu Item
            TrayMenu_RebuildMedia.Index = 0;
            TrayMenu_RebuildMedia.Text = l.Resources.ContextMenuRebuildMediaDbItemHeader;
            TrayMenu_RebuildMedia.Click += TrayMenu_RebuildMedia_Click;

            // Diag Menu Item
            TrayMenu_DiagWindow.Index = 1;
            TrayMenu_DiagWindow.Text = l.Resources.ContextMenuDiagItemHeader;
            TrayMenu_DiagWindow.Click += TrayMenu_DiagWindow_Click;

            // Caspar Menu Item
            TrayMenu_CasparItem.Index = 3;
            TrayMenu_CasparItem.Text = l.Resources.ContextMenuCasparItemHeader;
            TrayMenu_CasparItem.Click += TrayMenu_CasparItem_Click;

            // Scanner Menu Item
            TrayMenu_ScannerItem.Index = 4;
            TrayMenu_ScannerItem.Text = l.Resources.ContextMenuScannerItemHeader;
            TrayMenu_ScannerItem.Click += TrayMenu_ScannerItem_Click;

            // Exit Menu Item
            TrayMenu_ExitItem.Index = 6;
            TrayMenu_ExitItem.Text = l.Resources.ContextMenuExitItemHeader;
            TrayMenu_ExitItem.Click += TrayMenu_ExitItem_Click;

            TrayIcon.ContextMenu = TrayMenu;
        }

        private void TrayMenu_RebuildMedia_Click(object sender, EventArgs e)
        {
            RebuildMediaDb();
        }

        private void TrayMenu_DiagWindow_Click(object sender, EventArgs e)
        {
            OpenDiag();
        }

        private void TrayMenu_ExitItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TrayMenu_ScannerItem_Click(object sender, EventArgs e)
        {
            if (ScannerExecutable != null && ScannerExecutable.Running) ScannerExecutable.Process.Kill();
        }

        private void TrayMenu_CasparItem_Click(object sender, EventArgs e)
        {
            if (CasparExecutable != null && CasparExecutable.Running) CasparExecutable.Process.Kill();
        }

        private void TrayIcon_MouseUp(object sender, WF.MouseEventArgs e)
        {
            if (e.Button != WF.MouseButtons.Left) return;
            WindowState = PreviousState;
        }
        #endregion

        #region SPECIAL COMMANDS

        private void OpenDiag()
        {
            if (CasparExecutable is null || !CasparExecutable.Running) return;
            CasparExecutable.Write("DIAG");
        }

        private void ClearScannerDatabases()
        {
            if (ScannerExecutable is null || !ScannerExecutable.Exists || ScannerExecutable.Running) return;
            
            string scanner_dir = System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(ScannerExecutable.Path));
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

        private void RebuildMediaDb()
        {
            if (ScannerExecutable is null || !ScannerExecutable.Exists) return;
            if (ScannerExecutable.Running)
            {
                ScannerExecutable.Stop();
                ScannerExecutable.ProcessExited += RebuildAndRestart;
            }
            else
            {
                ClearScannerDatabases();
            }
        }

        private void RebuildAndRestart(object sender, EventArgs e)
        {
            ScannerExecutable.ProcessExited -= RebuildAndRestart;
            ClearScannerDatabases();
            ScannerExecutable.Start();
        }

        #endregion
        
        #region UI METHODS

        // Status TabItem UI Handlers

        private void ExecutableButton_Click(object sender, RoutedEventArgs e)
        {
            Executable exec = ((FrameworkElement)sender).DataContext as Executable;
            exec.IsSelected = true;
        }

        // Executables TabItem UI Handlers
        private void ScrollToEnd(object target)
        {
            ((TextBox)target).ScrollToEnd();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Executable executable = ((FrameworkElement)sender).DataContext as Executable;
            executable.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Executable executable = ((FrameworkElement)sender).DataContext as Executable;
            executable.Stop();
        }

        private void RebuildMedia_Click(object sender, RoutedEventArgs e)
        {
            RebuildMediaDb();
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
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = false,
                Filter = l.Resources.FileDialogFilterDescription + " (*.EXE)|*.exe"
            };
            Executable executable;
            if (openFileDialog.ShowDialog() == true)
            {
                executable = Settings.AddExecutable(openFileDialog.FileName);
            }
            else
            {
                executable = Settings.AddExecutable();
            }
            OpenExecutableOptions(executable);
        }

        private void ExecutableConfig_Click(object sender, RoutedEventArgs e)
        {
            Executable executable = ((FrameworkElement)sender).DataContext as Executable;
            ExecutableOptions executableOptions = new ExecutableOptions()
            {
                DataContext = executable,
                Owner = this
            };
            executableOptions.ShowDialog();
        }

        private void ExecutableRemove_Click(object sender, RoutedEventArgs e)
        {
            Executable executable = ((FrameworkElement)sender).DataContext as Executable;

            MessageBoxResult remove = MessageBox.Show(l.Resources.DeleteExecutablePromptMessage, l.Resources.DeleteExecutablePromptCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (remove)
            {
                case MessageBoxResult.Yes:
                    Settings.Executables.Remove(executable);
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
                ShowInTaskbar = false;
            }
            else
            {
                ShowInTaskbar = true;
                Activate();
            }
        }

        public void ActivateInstance()
        {
            WindowState = WindowState.Normal;
            Topmost = true;
            Activate();
            Topmost = false;
            Focus();
        }

        private void Launcher_Initialized(object sender, EventArgs e)
        {
            PreviousState = WindowState;
            WindowState = WindowState.Minimized;
        }

        private void Launcher_Activated(object sender, EventArgs e)
        {
            OnPropertyChanged("IsShiftDown");
        }

        private void Launcher_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) IsShiftDown = true;
        }

        private void Launcher_KeyUp(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyUp(Key.LeftShift) || Keyboard.IsKeyUp(Key.RightShift)) IsShiftDown = false;

            IInputElement focused = FocusManager.GetFocusedElement(this);
            if (focused is TextBox)
            {
                TextBox target = focused as TextBox;
                if (target.Name == "ConsoleCommandTextBox")
                {
                    object context = target.DataContext;
                    Executable executable = (context is Executable) ? context as Executable : null;
                    if (executable is null || !executable.AllowCommands) return;

                    if (e.Key == Key.Enter)
                    {
                        executable.Write();
                    }
                    if (e.Key == Key.Up)
                    {
                        executable.PreviousHistoryCommand();
                        target.CaretIndex = target.Text.Length;
                    }
                    if (e.Key == Key.Down)
                    {
                        executable.NextHistoryCommand();
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
                        Filter = l.Resources.ConfigFileDialogFilterDescription + "|*.config"
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

        private void OpenExecutableOptions(Executable executable)
        {
            ExecutableOptions executableOptions = new ExecutableOptions()
            {
                DataContext = executable,
                Owner = this
            };
            executableOptions.ShowDialog();
        }

        #endregion
    }

}
