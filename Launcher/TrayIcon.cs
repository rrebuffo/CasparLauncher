using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace CasparLauncher.Launcher;

public partial class TrayIcon
{
    private const int GWL_EX_STYLE = -20;
    private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;

    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial int GetWindowLongA(IntPtr hWnd, int nIndex);

    [LibraryImport("user32.dll")]
    private static partial int SetWindowLongA(IntPtr hWnd, int nIndex, int dwNewLong);

    private readonly WF.NotifyIcon Icon = new();
    private readonly TrayMenu Menu = new();
    
    static bool IsColorLight(Color clr) => ((5 * clr.G) + (2 * clr.R) + clr.B) > (8 * 128);

    public TrayIcon()
    {
    }

    private void SystemEvents_DisplaySettingsChanged(object? sender, EventArgs e)
    {
        SetTrayIcon();
    }

    internal void SetupTray()
    {
        SetEventHandlers();
        SetTrayIcon();
        SetTrayMenu();
    }

    private void SetTrayIcon()
    {
        HideIcon();
        int iconsize = 16;
        using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
        {
            float dpiX = graphics.DpiX;
            float dpiY = graphics.DpiY;
            double size = 16 * (graphics.DpiX / 96);
            if (size >= 20) iconsize = 20;
            if (size >= 24) iconsize = 24;
            if (size >= 32) iconsize = 32;
            if (size >= 48) iconsize = 256;
            //size = 256;
        }
        string iconpath = App.IsSystemThemeLight() ? "NotifyIconLight.ico" : "NotifyIconDark.ico";
        Stream IconStream = Application.GetResourceStream(new Uri($@"pack://application:,,,/Resources/{iconpath}")).Stream;
        Icon.Icon = new Icon(IconStream, new System.Drawing.Size(iconsize, iconsize));
        Icon.Text = L.AppTitle;
        Icon.BalloonTipClicked += Icon_BalloonTipClicked;
        IconStream.Dispose();
        ShowIcon();
    }

    private void SetTrayMenu()
    {
        Menu.DataContext = App.Launchpad;
        Menu.Show();
        IntPtr handle = new WindowInteropHelper(Menu).Handle;
        _ = SetWindowLongA(handle, GWL_EX_STYLE, (GetWindowLongA(handle, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
    }

    internal void ShowIcon() => Icon.Visible = true;
    internal void HideIcon() => Icon.Visible = false;

    private void SetEventHandlers()
    {
        SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;

        Icon.MouseDown += Icon_MouseDown;
        Icon.MouseUp += Icon_MouseUp;

        Menu.ActionExecuted += Menu_ActionExecuted;
        Menu.ConfigExecuted += Menu_ConfigExecuted;
        Menu.StartExecuted += Menu_StartExecuted;
        Menu.StopExecuted += Menu_StopExecuted;
        Menu.RestartExecuted += Menu_RestartExecuted;
        Menu.ExitExecuted += Menu_ExitExecuted;

        App.Launchpad.ExecutableError += Settings_ExecutableError;
        App.Launchpad.ExecutablePathError += Settings_ExecutablePathError;
        App.Launchpad.ExecutableStarted += Launchpad_ExecutableStarted;
        App.Launchpad.ExecutableStopped += Settings_ExecutableExited;
    }

    private void Menu_ActionExecuted(object? sender, (Executable, CasparAction) e)
    {
        switch (e.Item2)
        {
            case CasparAction.Diag:
                e.Item1.OpenDiag();
                break;
            case CasparAction.Rebuild:
                e.Item1.RebuildMediaDb();
                break;
            case CasparAction.Grid:
                e.Item1.OpenGrid();
                break;
        }
    }
    private void Menu_ConfigExecuted(object? sender, Executable executable)
    {
        Window? currentWindow = null;
        foreach (Window window in Application.Current.Windows)
        {
            if (window.DataContext == executable) currentWindow = window;
        }
        if(currentWindow is not null)
        {
            currentWindow.Topmost = true;
            currentWindow.Activate();
            currentWindow.Topmost = false;
        }
        else
        {
            ExecutableOptions executableOptions = new()
            {
                DataContext = executable,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = Menu
            };
            executableOptions.Show();
        }
    }

    private void Menu_StartExecuted(object? sender, Executable? e)
    {
        if (e is not null) Launchpad.Start(e);
        else Launchpad.StartAll();

    }

    private void Menu_StopExecuted(object? sender, Executable? e)
    {
        if (e is not null) Launchpad.Stop(e);
        else Launchpad.StopAll();
    }

    private void Menu_RestartExecuted(object? sender, Executable? e)
    {
        if (e is not null) Launchpad.Restart(e);
        else Launchpad.RestartAll();
    }

    private void Menu_ExitExecuted(object? sender, EventArgs e)
    {
        App.Shutdown(true);
    }


    private void Icon_MouseDown(object? sender, WF.MouseEventArgs e)
    {
        Launchpad.DisableStart = Keyboard.Modifiers == ModifierKeys.Shift;
        if (e.Button == WF.MouseButtons.Right)
        {
            Menu.Activate();
            Menu.TrayContextMenu.PlacementTarget = Menu;
            Menu.TrayContextMenu.IsOpen = true;
        }
    }

    private void Icon_MouseUp(object? sender, WF.MouseEventArgs e)
    {
        Launchpad.DisableStart = Keyboard.Modifiers == ModifierKeys.Shift;
        if (e.Button == WF.MouseButtons.Left)
        {
            if (Application.Current is App app) app.ShowWindow();
        }
    }

    private bool LastBalloonWasError = false;

    private void Icon_BalloonTipClicked(object? sender, EventArgs e)
    {
        if (Application.Current is App app) app.ShowWindow(LastBalloonWasError ? 0 : null);
        LastBalloonWasError = false;
    }

    private void Settings_ExecutablePathError(object? sender, ExecutableEventArgs e)
    {
        LastBalloonWasError = true;
        Icon.ShowBalloonTip(3000, string.Format("{0}: {1}", e.Executable.Name, L.ExecutableNotFoundWarningCaption), L.ExecutableNotFoundWarningMessage, WF.ToolTipIcon.Warning);
    }

    private void Settings_ExecutableError(object? sender, ExecutableEventArgs e)
    {
        LastBalloonWasError = true;
        if (Icon is null) return;
        Icon.ShowBalloonTip(3000, e.Executable.Name, L.ExecutableErrorMessage, WF.ToolTipIcon.Error);
    }

    private void Launchpad_ExecutableStarted(object? sender, ExecutableEventArgs e)
    {
        LastBalloonWasError = false;
        if (Icon is null) return;
        Icon.ShowBalloonTip(3000, e.Executable.Name, L.ExecutableStartedMessage, WF.ToolTipIcon.Info);
    }

    private void Settings_ExecutableExited(object? sender, ExecutableEventArgs e)
    {
        LastBalloonWasError = false;
        if (Icon is null) return;
        Icon.ShowBalloonTip(3000, e.Executable.Name, L.ExecutableStoppedMessage, WF.ToolTipIcon.Info);
    }
}