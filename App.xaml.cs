namespace CasparLauncher;

public partial class App : Application
{
    private static readonly CultureInfo DefaultCulture = Thread.CurrentThread.CurrentCulture;
    public static TrayIcon Tray { get; } = new();
    public static Launchpad Launchpad { get; } = new();
    public StatusWindow? Window { get; set; }

    public static System.Drawing.Icon? AppIcon
    {
        get => Environment.ProcessPath is null ? null : System.Drawing.Icon.ExtractAssociatedIcon(Environment.ProcessPath);
    }

    public App() : base()
    {
        AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);
        LocalizationHelper.Init(L.ResourceManager);
        if (Process.GetCurrentProcess() is Process p &&
            p.MainModule is ProcessModule m &&
            m.FileName is string f &&
            Path.GetDirectoryName(f) is string d)
        {
            Directory.SetCurrentDirectory(d);
        }
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        SetLanguageDictionary();
        _ = AppInitHelper.SingleInstanceCheck("CasparLauncher");
        AppInitHelper.RequestActivate += ActivateSingleInstance;
        SetDarkMode(Launchpad.DarkMode);
        Tray.SetupTray();
        Launchpad.StartAll(true);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Launchpad.Exiting = true;
        Tray.HideIcon();
        base.OnExit(e);
    }

    public static void Shutdown(bool prompt = false)
    {
        if (prompt)
        {
            MessageBoxResult close = MessageBox.Show(L.ClosePromptMessage, L.ClosePromptCaption, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (close)
            {
                case MessageBoxResult.Yes:
                    break;
                case MessageBoxResult.No:
                default:
                    return;
            }
        }
        Launchpad.StopAll();
        Current.Shutdown();
    }

    private void ActivateSingleInstance(object? sender, EventArgs e)
    {
        Dispatcher.BeginInvoke((() =>
        {
            ShowWindow();
        }));
    }

    internal void ShowWindow(int? tab = null)
    {
        if (Window is not null && Window.IsLoaded)
        {
            Window.Topmost = true;
            if (tab is int index) Launchpad.SelectedTab = index;
            Window.Activate();
            Window.Topmost = false;
        }
        else
        {
            Window = new()
            {
                DataContext = Launchpad
            };
            if (tab is int index) Launchpad.SelectedTab = index;
            Window.Show();
        }
    }

    public static void SetLanguageDictionary()
    {
        try
        {
            L.Culture = Launchpad.ForcedLanguage switch
            {
                Languages.en => new CultureInfo("en-US"),
                Languages.es => new CultureInfo("es-ES"),
                _ => DefaultCulture.ToString() switch
                {
                    "es-ES" or "es-MX" or "es-AR" => new("es-ES"),
                    _ => new("en-US"),
                },
            };
            Thread.CurrentThread.CurrentCulture = L.Culture;
            Thread.CurrentThread.CurrentUICulture = L.Culture;
            Thread.CurrentThread.CurrentCulture.ClearCachedData();
            Thread.CurrentThread.CurrentUICulture.ClearCachedData();
            L.Culture.ClearCachedData();
            LocalizationHelper.Instance.CurrentCulture = L.Culture;
        }
        catch (Exception)
        {
            L.Culture = new("en-US");
        }
    }


    const string THEMES_KEYPATH = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    const string THEMES_VALUEPATH = "SystemUsesLightTheme";

    public static void SetDarkMode(bool enabled)
    {
        Current.Resources.MergedDictionaries[0].Source = enabled ? new Uri("pack://application:,,,/BaseUISupport;component/Styles/DarkColors.xaml", UriKind.RelativeOrAbsolute) : new Uri("pack://application:,,,/BaseUISupport;component/Styles/LightColors.xaml", UriKind.RelativeOrAbsolute);
    }

    public static bool IsSystemThemeLight()
    {
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(THEMES_KEYPATH);
        if (key is not null && key.GetValue(THEMES_VALUEPATH) is object obj) return (int)obj > 0;
        else return false;
    }
}
