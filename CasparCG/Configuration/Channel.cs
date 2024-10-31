namespace CasparLauncher.CasparCG.Configuration;

public class Channel : INotifyPropertyChanged
{
    private VideoMode _videoMode;
    public VideoMode VideoMode
    {
        get
        {
            return _videoMode;
        }
        set
        {
            if (_videoMode != value)
            {
                _videoMode = value;
                OnPropertyChanged(nameof(VideoMode));
            }
        }
    }

    private ChannelColorDepth? _colorDepth = null;
    public ChannelColorDepth? ColorDepth
    {
        get
        {
            return _colorDepth;
        }
        set
        {
            if (_colorDepth != value)
            {
                _colorDepth = value;
                OnPropertyChanged(nameof(ColorDepth));
            }
        }
    }

    private ChannelColorSpace? _colorSpace = null;
    public ChannelColorSpace? ColorSpace
    {
        get
        {
            return _colorSpace;
        }
        set
        {
            if (_colorSpace != value)
            {
                _colorSpace = value;
                OnPropertyChanged(nameof(ColorSpace));
            }
        }
    }

    private int _selectedConsumer = 0;
    public int SelectedConsumer
    {
        get
        {
            return _selectedConsumer;
        }
        set
        {
            if (_selectedConsumer != value)
            {
                _selectedConsumer = value;
                OnPropertyChanged(nameof(SelectedConsumer));
            }
        }
    }

    public Channel()
    {
        _videoMode = ConfigFile.DefaultVideoModes.First();
    }

    public ObservableCollection<Producer> Producers { get; set; } = [];
    public ObservableCollection<object> Consumers { get; set; } = [];

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}
