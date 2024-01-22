namespace CasparLauncher.Launcher;

public class ExecutableEventArgs : EventArgs
{
    public Executable Executable { get; private set; }
    public Exception? Exception { get; private set; }

    public ExecutableEventArgs(Executable executable)
    {
        Executable = executable;
    }
    public ExecutableEventArgs(Executable executable, Exception exception)
    {
        Executable = executable;
        Exception = exception;
    }
}