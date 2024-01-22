namespace CasparLauncher.CasparCG.Configuration.Consumers;

public abstract class FrameConsumer : Consumer
{
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
}