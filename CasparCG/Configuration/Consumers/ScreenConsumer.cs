namespace CasparLauncher.CasparCG.Configuration.Consumers;

public class ScreenConsumer : Consumer
{
    private string _name = "";
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

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

    private ScreenAspectRatio _aspect = ScreenAspectRatio._default;
    public ScreenAspectRatio Aspect
    {
        get
        {
            return _aspect;
        }
        set
        {
            if (_aspect != value)
            {
                _aspect = value;
                OnPropertyChanged(nameof(Aspect));
            }
        }
    }

    private ScreenStretch _stretch = ScreenStretch._fill;
    public ScreenStretch Stretch
    {
        get
        {
            return _stretch;
        }
        set
        {
            if (_stretch != value)
            {
                _stretch = value;
                OnPropertyChanged(nameof(Stretch));
            }
        }
    }

    private bool _windowed = true;
    public bool Windowed
    {
        get
        {
            return _windowed;
        }
        set
        {
            if (_windowed != value)
            {
                _windowed = value;
                OnPropertyChanged(nameof(Windowed));
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

    private bool _vsync = false;
    public bool Vsync
    {
        get
        {
            return _vsync;
        }
        set
        {
            if (_vsync != value)
            {
                _vsync = value;
                OnPropertyChanged(nameof(Vsync));
            }
        }
    }

    private bool _borderless = false;
    public bool Borderless
    {
        get
        {
            return _borderless;
        }
        set
        {
            if (_borderless != value)
            {
                _borderless = value;
                OnPropertyChanged(nameof(Borderless));
            }
        }
    }

    private bool _interactive = true;
    public bool Interactive
    {
        get
        {
            return _interactive;
        }
        set
        {
            if (_interactive != value)
            {
                _interactive = value;
                OnPropertyChanged(nameof(Interactive));
            }
        }
    }

    private bool _alwaysOn = false;
    public bool AlwaysOn
    {
        get
        {
            return _alwaysOn;
        }
        set
        {
            if (_alwaysOn != value)
            {
                _alwaysOn = value;
                OnPropertyChanged(nameof(AlwaysOn));
            }
        }
    }

    private int _x = 0;
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

    private int _y = 0;
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

    private int _width = 0;
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

    private int _height = 0;
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

    private bool _sbsKey = false;
    public bool SbsKey
    {
        get
        {
            return _sbsKey;
        }
        set
        {
            if (_sbsKey != value)
            {
                _sbsKey = value;
                OnPropertyChanged(nameof(SbsKey));
            }
        }
    }

    private ScreenColourSpace _colourSpace = ScreenColourSpace._rgb;
    public ScreenColourSpace ColourSpace
    {
        get
        {
            return _colourSpace;
        }
        set
        {
            if (_colourSpace != value)
            {
                _colourSpace = value;
                OnPropertyChanged(nameof(ColourSpace));
            }
        }
    }
}