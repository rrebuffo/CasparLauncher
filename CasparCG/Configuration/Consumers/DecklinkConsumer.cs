namespace CasparLauncher.CasparCG.Configuration.Consumers;

public class DecklinkConsumer : FrameConsumer
{
    public ObservableCollection<DecklinkPort> Ports { get; } = [];

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

    private int _keyDevice = 2;
    public int KeyDevice
    {
        get
        {
            return _keyDevice;
        }
        set
        {
            if (_keyDevice != value)
            {
                _keyDevice = value;
                OnPropertyChanged(nameof(KeyDevice));
            }
        }
    }

    private bool _embeddedAudio = false;
    public bool EmbeddedAudio
    {
        get
        {
            return _embeddedAudio;
        }
        set
        {
            if (_embeddedAudio != value)
            {
                _embeddedAudio = value;
                OnPropertyChanged(nameof(EmbeddedAudio));
            }
        }
    }

    private DecklinkLatency _latency = DecklinkLatency._normal;
    public DecklinkLatency Latency
    {
        get
        {
            return _latency;
        }
        set
        {
            if (_latency != value)
            {
                _latency = value;
                OnPropertyChanged(nameof(Latency));
            }
        }
    }

    private DecklinkKeyer _keyer = DecklinkKeyer._external;
    public DecklinkKeyer Keyer
    {
        get
        {
            return _keyer;
        }
        set
        {
            if (_keyer != value)
            {
                _keyer = value;
                OnPropertyChanged(nameof(Keyer));
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

    private int _bufferDepth = 3;
    public int BufferDepth
    {
        get
        {
            return _bufferDepth;
        }
        set
        {
            if (_bufferDepth != value)
            {
                _bufferDepth = value;
                OnPropertyChanged(nameof(KeyDevice));
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

    private DecklinkWaitForReference _waitForReference = DecklinkWaitForReference._auto;
    public DecklinkWaitForReference WaitForReference
    {
        get
        {
            return _waitForReference;
        }
        set
        {
            if (_waitForReference != value)
            {
                _waitForReference = value;
                OnPropertyChanged(nameof(WaitForReference));
            }
        }
    }

    private int _waitForReferenceDuration = 10;
    public int WaitForReferenceDuration
    {
        get
        {
            return _waitForReferenceDuration;
        }
        set
        {
            if (_waitForReferenceDuration != value)
            {
                _waitForReferenceDuration = value;
                OnPropertyChanged(nameof(WaitForReferenceDuration));
            }
        }
    }

    private bool _hdrMetadata = false;
    public bool HdrMetadata
    {
        get
        {
            return _hdrMetadata;
        }
        set
        {
            if (_hdrMetadata != value)
            {
                _hdrMetadata = value;
                OnPropertyChanged(nameof(HdrMetadata));
            }
        }
    }

    private int _hdrMaxCll = 1000;
    public int HdrMaxCll
    {
        get
        {
            return _hdrMaxCll;
        }
        set
        {
            if (_hdrMaxCll != value)
            {
                if (value < 1) _hdrMaxCll = 1;
                else if (value > 65535) _hdrMaxCll = 65535;
                else _hdrMaxCll = value;
                OnPropertyChanged(nameof(HdrMaxCll));
            }
        }
    }

    private int _hdrMaxFall = 1000;
    public int HdrMaxFall
    {
        get
        {
            return _hdrMaxFall;
        }
        set
        {
            if (_hdrMaxFall != value)
            {
                if (value < 50) _hdrMaxFall = 50;
                else if (value > 65535) _hdrMaxFall = 65535;
                else _hdrMaxFall = value;
                OnPropertyChanged(nameof(HdrMaxFall));
            }
        }
    }

    private double _hdrMinDml = 0.005;
    public double HdrMinDml
    {
        get
        {
            return _hdrMinDml;
        }
        set
        {
            if (_hdrMinDml != value)
            {
                if (value < 0.0001) _hdrMinDml = 0.0001;
                else if (value > 6.5535) _hdrMinDml = 6.5535;
                else _hdrMinDml = value;
                OnPropertyChanged(nameof(HdrMinDml));
            }
        }
    }

    private double _hdrMaxDml = 1000;
    public double HdrMaxDml
    {
        get
        {
            return _hdrMaxDml;
        }
        set
        {
            if (_hdrMaxDml != value)
            {
                if (value < 1) _hdrMaxDml = 1;
                else if (value > 65535) _hdrMaxDml = 65535;
                else _hdrMaxDml = value;
                OnPropertyChanged(nameof(HdrMaxDml));
            }
        }
    }

    private DecklinkDefaultColorSpace _defaultColorSpace = DecklinkDefaultColorSpace._bt709;
    public DecklinkDefaultColorSpace DefaultColorSpace
    {
        get
        {
            return _defaultColorSpace;
        }
        set
        {
            if (_defaultColorSpace != value)
            {
                _defaultColorSpace = value;
                OnPropertyChanged(nameof(DefaultColorSpace));
            }
        }
    }
}