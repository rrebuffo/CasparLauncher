using System.Reflection;
using System.Windows.Media.Imaging;
using IconConverter = BaseUISupport.Converters.IconConverter;

namespace CasparLauncher;

public partial class AboutWindow : DialogWindow
{
    public AboutWindow()
    {
        InitializeComponent();
    }

    public BitmapImage? AppIcon => IconConverter.GetJumboImageFromResource($@"pack://application:,,,/Resources/AppIcon.ico");

    public string Version
    {
        get
        {
            FileVersionInfo? version = Process.GetCurrentProcess().MainModule?.FileVersionInfo;
            return version is not null ? string.Format(L.AboutWindowVersion, version.ProductVersion) : "";
        }
    }

    private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
        LinkHelper.Open(e.Uri.ToString());
        e.Handled = true;
    }
}
