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
