namespace CasparLauncher.CasparCG.Configuration.Consumers;

public class BluefishConsumer : Consumer
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

    private int _sdiStream = 1;
    public int SdiStream
    {
        get
        {
            return _sdiStream;
        }
        set
        {
            if (_sdiStream != value)
            {
                _sdiStream = value;
                OnPropertyChanged(nameof(SdiStream));
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

    private BluefishKeyer _keyer = BluefishKeyer._disabled;
    public BluefishKeyer Keyer
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

    private BluefishKeyerAudio _keyerAudio = BluefishKeyerAudio._videooutputchannel;
    public BluefishKeyerAudio KeyerAudio
    {
        get
        {
            return _keyerAudio;
        }
        set
        {
            if (_keyerAudio != value)
            {
                _keyerAudio = value;
                OnPropertyChanged(nameof(KeyerAudio));
            }
        }
    }

    private int _watchdog = 2;
    public int Watchdog
    {
        get
        {
            return _watchdog;
        }
        set
        {
            if (_watchdog != value)
            {
                _watchdog = value;
                OnPropertyChanged("KeyDevice");
            }
        }
    }

    private BluefishUhdMode _uhdMode = BluefishUhdMode._0;
    public BluefishUhdMode UhdMode
    {
        get
        {
            return _uhdMode;
        }
        set
        {
            if (_uhdMode != value)
            {
                _uhdMode = value;
                OnPropertyChanged(nameof(UhdMode));
            }
        }
    }
}