namespace CasparLauncher.CasparCG.Configuration.Consumers;

public class FfmpegConsumer : Consumer
{
    private string _path = "";
    public string Path
    {
        get
        {
            return _path;
        }
        set
        {
            if (_path != value)
            {
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
    }

    private string _arguments = "";
    public string Arguments
    {
        get
        {
            return _arguments;
        }
        set
        {
            if (_arguments != value)
            {
                _arguments = value;
                OnPropertyChanged(nameof(Arguments));
            }
        }
    }
}