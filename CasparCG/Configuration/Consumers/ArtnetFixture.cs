namespace CasparLauncher.CasparCG.Configuration.Consumers;

public class ArtnetFixture : INotifyPropertyChanged
{
    private ArtnetFixtureType _type = ArtnetFixtureType._RGBW;
    public ArtnetFixtureType Type
    {
        get
        {
            return _type;
        }
        set
        {
            if (_type != value)
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
    }

    private int _startAddress = 1;
    public int StartAddress
    {
        get
        {
            return _startAddress;
        }
        set
        {
            if (_startAddress != value)
            {
                _startAddress = value;
                OnPropertyChanged(nameof(StartAddress));
            }
        }
    }

    private int _fixtureCount = 10;
    public int FixtureCount
    {
        get
        {
            return _fixtureCount;
        }
        set
        {
            if (_fixtureCount != value)
            {
                _fixtureCount = value;
                OnPropertyChanged(nameof(FixtureCount));
            }
        }
    }

    private int _fixtureChannels = 6;
    public int FixtureChannels
    {
        get
        {
            return _fixtureChannels;
        }
        set
        {
            if (_fixtureChannels != value)
            {
                _fixtureChannels = value;
                OnPropertyChanged(nameof(FixtureChannels));
            }
        }
    }

    private int _x = 960;
    public int X
    {
        get
        {
            return _x;
        }
        set
        {
            if (_x != value)
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }
    }

    private int _y = 540;
    public int Y
    {
        get
        {
            return _y;
        }
        set
        {
            if (_y != value)
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
    }

    private int _width = 100;
    public int Width
    {
        get
        {
            return _width;
        }
        set
        {
            if (_width != value)
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }
    }

    private int _height = 100;
    public int Height
    {
        get
        {
            return _height;
        }
        set
        {
            if (_height != value)
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
    }

    private int _rotation = 0;
    public int Rotation
    {
        get
        {
            return _rotation;
        }
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                OnPropertyChanged(nameof(Rotation));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}
