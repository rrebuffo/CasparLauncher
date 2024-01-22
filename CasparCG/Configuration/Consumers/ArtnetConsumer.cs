namespace CasparLauncher.CasparCG.Configuration.Consumers;

public class ArtnetConsumer : Consumer
{
    public ObservableCollection<ArtnetFixture> Fixtures { get; } = [];

    private int _universe = 1;
    public int Universe
    {
        get
        {
            return _universe;
        }
        set
        {
            if (_universe != value)
            {
                _universe = value;
                OnPropertyChanged(nameof(Universe));
            }
        }
    }

    private string _host = "127.0.0.1";
    public string Host
    {
        get
        {
            return _host;
        }
        set
        {
            if (_host != value)
            {
                _host = value;
                OnPropertyChanged(nameof(Host));
            }
        }
    }

    private int _port = 6454;
    public int Port
    {
        get
        {
            return _port;
        }
        set
        {
            if (_port != value)
            {
                _port = value;
                OnPropertyChanged(nameof(Port));
            }
        }
    }

    private int _refreshRate = 30;
    public int RefreshRate
    {
        get
        {
            return _refreshRate;
        }
        set
        {
            if (_refreshRate != value)
            {
                _refreshRate = value;
                OnPropertyChanged(nameof(RefreshRate));
            }
        }
    }
}