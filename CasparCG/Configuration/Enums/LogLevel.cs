namespace CasparLauncher.CasparCG.Configuration.Enums;

public enum LogLevel
{
    [Display(Description = "LogLevel_Trace", ResourceType = typeof(L))]
    _trace,
    [Display(Description = "LogLevel_Debug", ResourceType = typeof(L))]
    _debug,
    [Display(Description = "LogLevel_Info", ResourceType = typeof(L))]
    _info,
    [Display(Description = "LogLevel_Warning", ResourceType = typeof(L))]
    _warning,
    [Display(Description = "LogLevel_Error", ResourceType = typeof(L))]
    _error,
    [Display(Description = "LogLevel_Fatal", ResourceType = typeof(L))]
    _fatal
}