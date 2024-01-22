namespace CasparLauncher;

public partial class StatusWindow : AppWindow
{
    public StatusWindow()
    {
        InitializeComponent();
        Loaded += StatusWindow_Initialized;
    }

    private void StatusWindow_Initialized(object? sender, EventArgs e)
    {
        if (State == WindowState.Maximized) WindowState = WindowState.Maximized;
        KeyUp += StatusWindow_KeyUp;
        KeyDown += StatusWindow_KeyDown;
    }

    #region PROPERTIES

    private WindowState? _state;
    public WindowState State
    {
        get
        {
            _state ??= (WindowState)S.GetOrSetValue("/ui/state", (int)WindowState.Normal);
            return _state ?? (int)WindowState.Normal;
        }
        set
        {
            if (IsLoaded && _state != value && value != WindowState.Minimized)
            {
                _state = value;
                S.Set("/ui/state", (int)value);
                OnPropertyChanged(nameof(State));
            }
        }
    }

    private int? _posX;
    public int PosX
    {
        get
        {
            _posX ??= S.GetOrSetValue("/ui/pos-x", 0);
            return _posX ?? 0;
        }
        set
        {
            if (WindowState == WindowState.Normal && _posX != value)
            {
                _posX = value;
                S.Set("/ui/pos-x", value);
                OnPropertyChanged(nameof(PosX));
            }
        }
    }

    private int? _posY;
    public int PosY
    {
        get
        {
            _posY ??= S.GetOrSetValue("/ui/pos-y", 0);
            return _posY ?? 0;
        }
        set
        {
            if (WindowState == WindowState.Normal && _posY != value)
            {
                _posY = value;
                S.Set("/ui/pos-y", value);
                OnPropertyChanged(nameof(PosY));
            }
        }
    }

    private int? _width;
    public int WindowWidth
    {
        get
        {
            _width ??= S.GetOrSetValue("/ui/width", 700);
            return _width ?? 0;
        }
        set
        {
            if (WindowState == WindowState.Normal && _width != value)
            {
                _width = value;
                S.Set("/ui/width", value);
                OnPropertyChanged(nameof(WindowWidth));
            }
        }
    }

    private int? _height;
    public int WindowHeight
    {
        get
        {
            _height ??= S.GetOrSetValue("/ui/height", 400);
            return _height ?? 0;
        }
        set
        {
            if (WindowState == WindowState.Normal && _height != value)
            {
                _height = value;
                S.Set("/ui/height", value);
                OnPropertyChanged(nameof(WindowHeight));
            }
        }
    }

    #endregion

    #region Event Handlers

    private void ForcedLanguage_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is Languages language)
        {
            App.Launchpad.ForcedLanguage = language;
        }
    }

    private void StatusWindow_KeyDown(object sender, KeyEventArgs e)
    {
        Launchpad.DisableStart = Keyboard.Modifiers == ModifierKeys.Shift;
    }

    private void StatusWindow_KeyUp(object sender, KeyEventArgs e)
    {
        Launchpad.DisableStart = Keyboard.Modifiers == ModifierKeys.Shift;
        IInputElement focused = FocusManager.GetFocusedElement(this);
        if (focused is TextBox target
            && target.Name == "ConsoleCommandTextBox"
            && target.DataContext is Executable ex
            && ex.AllowCommands)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
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

    private void StartAll(object sender, RoutedEventArgs e)
    {
        Launchpad.StartAll();
    }

    private void StopAll(object sender, RoutedEventArgs e)
    {
        Launchpad.StopAll();
    }

    private void RestartAll(object sender, RoutedEventArgs e)
    {
        Launchpad.RestartAll();
    }

    private static Executable? GetItemExecutable(object item)
    {
        if (item is FrameworkElement element && element.DataContext is Executable executable)
        return executable;
        else return null;
    }

    private void ExecutableButton_Click(object sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is not Executable ex) return;
        if (ex.Exists) ex.IsSelected = true;
        else ConfigTab.IsSelected = true;
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
        {
            Launchpad.Start(ex);
        }
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
        {
            Launchpad.Stop(ex);
        }
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
        {
            Launchpad.Restart(ex);
        }
    }

    private void RebuildMedia_Click(object sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
        {
            Launchpad.RebuildMediaDb(ex);
        }
    }

    private void OpenDiag_Click(object sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
        {
            Launchpad.OpenDiag(ex);
        }
    }

    private void OpenGrid_Click(object sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
        {
            Launchpad.OpenGrid(ex);
        }
    }

    private void ConsoleOutputDataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (e.ExtentHeightChange > 0 && e.OriginalSource is ScrollViewer view)
        {
            if (view.ScrollableHeight <= view.VerticalOffset + e.ExtentHeightChange + 20)
            {
                view.ScrollToVerticalOffset(view.ScrollableHeight);
            }
        }
    }

    private void AddExecutableButton_Click(object sender, RoutedEventArgs e)
    {
        OpenExecutableConfig(App.Launchpad.AddExecutable());
    }

    private void ExecutableConfig_Click(object sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is not Executable ex) return;
        OpenExecutableConfig(ex);
    }

    private void OpenExecutableConfig(Executable executable)
    {
        ExecutableOptions executableOptions = new()
        {
            DataContext = executable,
            Owner = this
        };
        executableOptions.ShowDialog();
    }

    #endregion

    private void UpdateDataGridColumnWidth(DataGrid target)
    {
        target.Columns[0].Width = 0;
        target.UpdateLayout();
        target.Columns[0].Width = App.Launchpad.CropConsole ? new(1, DataGridLengthUnitType.Star) : new(1, DataGridLengthUnitType.Auto);
    }

    private void ConsoleOutputDataGrid_Initialized(object sender, EventArgs e)
    {
        if (sender is DataGrid target) UpdateDataGridColumnWidth(target);
    }

    private void ConsoleOutputDataGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is DataGrid target) UpdateDataGridColumnWidth(target);
    }

    private void CropConsoleLines_Checked(object sender, RoutedEventArgs e)
    {
        if (VisualTreeHelpers.FindChild<DataGrid>(ExecutablesTabbedPanel) is DataGrid currentConsole)
        {
            currentConsole.Visibility = Visibility.Collapsed;
            ExecutablesTabbedPanel.UpdateLayout();
            currentConsole.Visibility = Visibility.Visible;
            UpdateDataGridColumnWidth(currentConsole);
        }
    }

    private void ReportIssue_Click(object sender, RoutedEventArgs e)
    {
        LinkHelper.Open("https://github.com/rrebuffo/CasparLauncher/issues");
        e.Handled = true;
    }

    private void CasparCGProject_Click(object sender, RoutedEventArgs e)
    {
        LinkHelper.Open("https://casparcg.com/");
        e.Handled = true;
    }

    private void About_Click(object sender, RoutedEventArgs e)
    {
        AboutWindow about = new();
        about.Owner = this;
        about.ShowDialog();
    }
}
