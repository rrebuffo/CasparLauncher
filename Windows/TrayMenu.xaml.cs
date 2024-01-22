namespace CasparLauncher;

public partial class TrayMenu : Window
{
    public TrayMenu()
    {
        InitializeComponent();
        KeyDown += TrayMenu_KeyChange;
        KeyUp += TrayMenu_KeyChange;
    }

    private void TrayMenu_KeyChange(object sender, KeyEventArgs e)
    {
        Launchpad.DisableStart = Keyboard.Modifiers == ModifierKeys.Shift;
    }

    private void StartAll(object? sender, RoutedEventArgs e)
    {
        OnStart(null);
    }

    private void StopAll(object? sender, RoutedEventArgs e)
    {
        OnStop(null);
    }

    private void RestartAll(object? sender, RoutedEventArgs e)
    {
        OnRestart(null);
    }

    private Executable? GetItemExecutable(object? item)
    {
        if (item is FrameworkElement element && element.DataContext is Executable ex) return ex;
        else return null;
    }

    private void Start_item_Click(object? sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
            OnStart(ex);
    }

    private void Stop_item_Click(object? sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
            OnStop(ex);
    }

    private void Restart_item_Click(object? sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
            OnRestart(ex);
    }

    private void Config_item_Click(object? sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
            OnConfig(ex);
    }

    private void Rebuild_item_Click(object? sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
            OnAction(ex, CasparAction.Rebuild);
    }

    private void Diag_item_Click(object? sender, RoutedEventArgs e)
    {
        if (GetItemExecutable(sender) is Executable ex)
            OnAction(ex, CasparAction.Diag);
    }

    private void Grid_item_Click(object? sender, RoutedEventArgs e)
    {

        if (GetItemExecutable(sender) is Executable ex)
            OnAction(ex, CasparAction.Grid);
    }

    private void TrayMenu_ExitItem_Click(object? sender, RoutedEventArgs e)
    {
        OnExit();
    }

    public event EventHandler? ExitExecuted;
    protected virtual void OnExit() { ExitExecuted?.Invoke(this, new()); }

    public event EventHandler<Executable?>? StartExecuted;
    protected virtual void OnStart(Executable? executable) { StartExecuted?.Invoke(this, executable); }

    public event EventHandler<Executable?>? StopExecuted;
    protected virtual void OnStop(Executable? executable) { StopExecuted?.Invoke(this, executable); }

    public event EventHandler<Executable?>? RestartExecuted;
    protected virtual void OnRestart(Executable? executable) { RestartExecuted?.Invoke(this, executable); }

    public event EventHandler<Executable>? ConfigExecuted;
    protected virtual void OnConfig(Executable executable) { ConfigExecuted?.Invoke(this, executable); }

    public event EventHandler<(Executable, CasparAction)>? ActionExecuted;
    protected virtual void OnAction(Executable executable, CasparAction action) { ActionExecuted?.Invoke(this, (executable, action)); }
}
