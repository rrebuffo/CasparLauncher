namespace CasparLauncher.CasparCG.Configuration.Consumers;

public class DecklinkPort : INotifyPropertyChanged
{
    private int _device = 1;
    public int Device
    {
        get
        {
            return _device;
        }
        set
        {
            if (_device != value)
            {
                _device = value;
                OnPropertyChanged(nameof(Device));
            }
        }
    }

    private bool _keyOnly = false;
    public bool KeyOnly
    {
        get
        {
            return _keyOnly;
        }
        set
        {
            if (_keyOnly != value)
            {
                _keyOnly = value;
                OnPropertyChanged(nameof(KeyOnly));
            }
        }
    }

    private string _videoMode = "";
    public string VideoMode
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


    private bool _subregionEnable = false;
    public bool SubregionEnable
    {
        get
        {
            return _subregionEnable;
        }
        set
        {
            if (_subregionEnable != value)
            {
                _subregionEnable = value;
                OnPropertyChanged(nameof(SubregionEnable));
            }
        }
    }

    #region SUBREGION

    private int _sourceX = 0;
    public int SourceX
    {
        get
        {
            return _sourceX;
        }
        set
        {
            if (_sourceX != value)
            {
                _sourceX = value;
                OnPropertyChanged(nameof(SourceX));
            }
        }
    }

    private int _sourceY = 0;
    public int SourceY
    {
        get
        {
            return _sourceY;
        }
        set
        {
            if (_sourceY != value)
            {
                _sourceY = value;
                OnPropertyChanged(nameof(SourceY));
            }
        }
    }

    private int _destinationX = 0;
    public int DestinationX
    {
        get
        {
            return _destinationX;
        }
        set
        {
            if (_destinationX != value)
            {
                _destinationX = value;
                OnPropertyChanged(nameof(DestinationX));
            }
        }
    }

    private int _destinationY = 0;
    public int DestinationY
    {
        get
        {
            return _destinationY;
        }
        set
        {
            if (_destinationY != value)
            {
                _destinationY = value;
                OnPropertyChanged(nameof(DestinationY));
            }
        }
    }

    private int _subregionWidth = 0;
    public int SubregionWidth
    {
        get
        {
            return _subregionWidth;
        }
        set
        {
            if (_subregionWidth != value)
            {
                _subregionWidth = value;
                OnPropertyChanged(nameof(SubregionWidth));
            }
        }
    }

    private int _subregionHeight = 0;
    public int SubregionHeight
    {
        get
        {
            return _subregionHeight;
        }
        set
        {
            if (_subregionHeight != value)
            {
                _subregionHeight = value;
                OnPropertyChanged(nameof(SubregionHeight));
            }
        }
    }

    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}
