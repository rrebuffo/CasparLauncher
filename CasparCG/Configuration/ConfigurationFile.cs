using System.ComponentModel;
using System.Xml;
using System.Xml.Linq;
using XH = BaseUISupport.Helpers.XmlHelper;

namespace CasparLauncher.CasparCG.Configuration;

public class ConfigFile : INotifyPropertyChanged
{
    static ConfigFile()
    {
        DefaultVideoModes.Add(new VideoMode() { Display = "PAL", Id = "PAL" });
        DefaultVideoModes.Add(new VideoMode() { Display = "NTSC", Id = "NTSC" });
        DefaultVideoModes.Add(new VideoMode() { Display = "576p25", Id = "576p2500" });
        DefaultVideoModes.Add(new VideoMode() { Display = "720p23.98", Id = "720p2398" });
        DefaultVideoModes.Add(new VideoMode() { Display = "720p24", Id = "720p2400" });
        DefaultVideoModes.Add(new VideoMode() { Display = "720p25", Id = "720p2500" });
        DefaultVideoModes.Add(new VideoMode() { Display = "720p50", Id = "720p5000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "720p29.97", Id = "720p2997" });
        DefaultVideoModes.Add(new VideoMode() { Display = "720p59.94", Id = "720p5994" });
        DefaultVideoModes.Add(new VideoMode() { Display = "720p30", Id = "720p3000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "720p60", Id = "720p6000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080p23.98", Id = "1080p2398" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080p24", Id = "1080p2400" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080i50", Id = "1080i5000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080i59.94", Id = "1080i5994" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080i60", Id = "1080i6000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080p25", Id = "1080p2500" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080p29.97", Id = "1080p2997" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080p30", Id = "1080p3000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080p50", Id = "1080p5000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080p59.94", Id = "1080p5994" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1080p60", Id = "1080p6000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1556p23.98", Id = "1556p2398" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1556p24", Id = "1556p2400" });
        DefaultVideoModes.Add(new VideoMode() { Display = "1556p25", Id = "1556p2500" });
        DefaultVideoModes.Add(new VideoMode() { Display = "DCI 1080p23.98", Id = "dci1080p2398" });
        DefaultVideoModes.Add(new VideoMode() { Display = "DCI 1080p24", Id = "dci1080p2400" });
        DefaultVideoModes.Add(new VideoMode() { Display = "DCI 1080p25", Id = "dci1080p2500" });
        DefaultVideoModes.Add(new VideoMode() { Display = "2160p23.98", Id = "2160p2398" });
        DefaultVideoModes.Add(new VideoMode() { Display = "2160p24", Id = "2160p2400" });
        DefaultVideoModes.Add(new VideoMode() { Display = "2160p25", Id = "2160p2500" });
        DefaultVideoModes.Add(new VideoMode() { Display = "2160p29.97", Id = "2160p2997" });
        DefaultVideoModes.Add(new VideoMode() { Display = "2160p30", Id = "2160p3000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "2160p50", Id = "2160p5000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "2160p59.94", Id = "2160p5994" });
        DefaultVideoModes.Add(new VideoMode() { Display = "2160p60", Id = "2160p6000" });
        DefaultVideoModes.Add(new VideoMode() { Display = "DCI 2160p23.98", Id = "dci2160p2398" });
        DefaultVideoModes.Add(new VideoMode() { Display = "DCI 2160p24", Id = "dci2160p2400" });
        DefaultVideoModes.Add(new VideoMode() { Display = "DCI 2160p25", Id = "dci2160p2500" });
    }

    public ConfigFile()
    {
        
    }

    private string? _file;
    public string? File
    {
        get
        {
            return _file;
        }
        set
        {
            if (_file != value)
            {
                _file = value;
                OnPropertyChanged(nameof(File));
                OnPropertyChanged(nameof(FileName));
            }
        }
    }
    public string? FileName
    {
        get
        {
            return Path.GetFileName(_file);
        }
    }


    private LogLevel _logLevel = LogLevel._info;
    public LogLevel LogLevel
    {
        get
        {
            return _logLevel;
        }
        set
        {
            if (_logLevel != value)
            {
                _logLevel = value;
                OnPropertyChanged(nameof(LogLevel));
            }
        }
    }

    private int _flashBuffer = 0;
    public int FlashBuffer
    {
        get
        {
            return _flashBuffer;
        }
        set
        {
            if (_flashBuffer != value)
            {
                _flashBuffer = value;
                OnPropertyChanged(nameof(FlashBuffer));
            }
        }
    }

    private bool _ndiAutoLoad = false;
    public bool NdiAutoLoad
    {
        get
        {
            return _ndiAutoLoad;
        }
        set
        {
            if (_ndiAutoLoad != value)
            {
                _ndiAutoLoad = value;
                OnPropertyChanged(nameof(NdiAutoLoad));
            }
        }
    }

    private int _htmlRemoteDebugPort = 0;
    public int HtmlRemoteDebugPort
    {
        get
        {
            return _htmlRemoteDebugPort;
        }
        set
        {
            if (_htmlRemoteDebugPort != value)
            {
                _htmlRemoteDebugPort = value;
                OnPropertyChanged(nameof(HtmlRemoteDebugPort));
            }
        }
    }

    private bool _htmlEnableGpu = false;
    public bool HtmlEnableGpu
    {
        get
        {
            return _htmlEnableGpu;
        }
        set
        {
            if (_htmlEnableGpu != value)
            {
                _htmlEnableGpu = value;
                OnPropertyChanged(nameof(HtmlEnableGpu));
            }
        }
    }

    private string? _htmlCachePath = null;
    public string? HtmlCachePath
    {
        get
        {
            return _htmlCachePath;
        }
        set
        {
            if (_htmlCachePath != value)
            {
                if (value == "") _htmlCachePath = null;
                else _htmlCachePath = value;
                OnPropertyChanged(nameof(HtmlCachePath));
            }
        }
    }

    private HtmlAngleGraphicsBackend _htmlAngleBackend = HtmlAngleGraphicsBackend._gl;
    public HtmlAngleGraphicsBackend HtmlAngleBackend
    {
        get
        {
            return _htmlAngleBackend;
        }
        set
        {
            if (_htmlAngleBackend != value)
            {
                _htmlAngleBackend = value;
                OnPropertyChanged(nameof(HtmlAngleBackend));
            }
        }
    }

    private bool _flashEnableProducer = false;
    public bool FlashEnableProducer
    {
        get
        {
            return _flashEnableProducer;
        }
        set
        {
            if (_flashEnableProducer != value)
            {
                _flashEnableProducer = value;
                OnPropertyChanged(nameof(FlashEnableProducer));
            }
        }
    }

    private int _ffmpegThreads = 4;
    public int FfmpegThreads
    {
        get
        {
            return _ffmpegThreads;
        }
        set
        {
            if (_ffmpegThreads != value)
            {
                _ffmpegThreads = value;
                OnPropertyChanged(nameof(FfmpegThreads));
            }
        }
    }

    private string _defaultAudioOutputDevice = "";
    public string DefaultAudioOutputDevice
    {
        get
        {
            return _defaultAudioOutputDevice;
        }
        set
        {
            if (_defaultAudioOutputDevice != value)
            {
                _defaultAudioOutputDevice = value;
                OnPropertyChanged(nameof(DefaultAudioOutputDevice));
            }
        }
    }


    private FfmpegDeinterlace _ffmpegDeinterlace = FfmpegDeinterlace._interlaced;
    public FfmpegDeinterlace FfmpegDeinterlace
    {
        get
        {
            return _ffmpegDeinterlace;
        }
        set
        {
            if (_ffmpegDeinterlace != value)
            {
                _ffmpegDeinterlace = value;
                OnPropertyChanged(nameof(FfmpegDeinterlace));
            }
        }
    }

    private string _mediaPath = "media/";
    public string MediaPath
    {
        get
        {
            return _mediaPath;
        }
        set
        {
            if (_mediaPath != value)
            {
                _mediaPath = value;
                OnPropertyChanged(nameof(MediaPath));
            }
        }
    }

    private string _logPath = "log/";
    public string LogPath
    {
        get
        {
            return _logPath;
        }
        set
        {
            if (_logPath != value)
            {
                _logPath = value;
                OnPropertyChanged(nameof(LogPath));
            }
        }
    }

    private bool _logToFile = true;
    public bool LogToFile
    {
        get
        {
            return _logToFile;
        }
        set
        {
            if (_logToFile != value)
            {
                _logToFile = value;
                OnPropertyChanged(nameof(LogToFile));
            }
        }
    }

    private bool _logDisableColumnAlignment = false;
    public bool LogDisableColumnAlignment
    {
        get
        {
            return _logDisableColumnAlignment;
        }
        set
        {
            if (_logDisableColumnAlignment != value)
            {
                _logDisableColumnAlignment = value;
                OnPropertyChanged(nameof(LogDisableColumnAlignment));
            }
        }
    }

    private string _dataPath = "data/";
    public string DataPath
    {
        get
        {
            return _dataPath;
        }
        set
        {
            if (_dataPath != value)
            {
                _dataPath = value;
                OnPropertyChanged(nameof(DataPath));
            }
        }
    }

    private string _templatePath = "template/";
    public string TemplatePath
    {
        get
        {
            return _templatePath;
        }
        set
        {
            if (_templatePath != value)
            {
                _templatePath = value;
                OnPropertyChanged(nameof(TemplatePath));
            }
        }
    }

    private string _fontPath = "font/";
    public string FontPath
    {
        get
        {
            return _fontPath;
        }
        set
        {
            if (_fontPath != value)
            {
                _fontPath = value;
                OnPropertyChanged(nameof(FontPath));
            }
        }
    }

    private string _lockPass = "secret";
    public string LockPass
    {
        get
        {
            return _lockPass;
        }
        set
        {
            if (_lockPass != value)
            {
                _lockPass = value;
                OnPropertyChanged(nameof(LockPass));
            }
        }
    }


    private int _oscDefaultPort = 6250;
    public int OscDefaultPort
    {
        get
        {
            return _oscDefaultPort;
        }
        set
        {
            if (_oscDefaultPort != value)
            {
                _oscDefaultPort = value;
                OnPropertyChanged(nameof(OscDefaultPort));
            }
        }
    }

    private bool _oscDisableAmcpClients = false;
    public bool OscDisableAmcpClients
    {
        get
        {
            return _oscDisableAmcpClients;
        }
        set
        {
            if (_oscDisableAmcpClients != value)
            {
                _oscDisableAmcpClients = value;
                OnPropertyChanged(nameof(OscDisableAmcpClients));
            }
        }
    }

    private int _amcpPort = 5250;
    public int AmcpPort
    {
        get
        {
            return _amcpPort;
        }
        set
        {
            if (_amcpPort != value)
            {
                _amcpPort = value;
                OnPropertyChanged(nameof(AmcpPort));
            }
        }
    }

    private string _mediaHost = "localhost";
    public string MediaHost
    {
        get
        {
            return _mediaHost;
        }
        set
        {
            if (_mediaHost != value)
            {
                _mediaHost = value;
                OnPropertyChanged(nameof(MediaHost));
            }
        }
    }

    private int _mediaPort = 8000;
    public int MediaPort
    {
        get
        {
            return _mediaPort;
        }
        set
        {
            if (_mediaPort != value)
            {
                _mediaPort = value;
                OnPropertyChanged(nameof(MediaPort));
            }
        }
    }


    private List<string> _comments = new();
    public List<string> Comments
    {
        get
        {
            return _comments;
        }
        set
        {
            if (_comments != value)
            {
                _comments = value;
                OnPropertyChanged(nameof(Comments));
            }
        }
    }

    public static ObservableCollection<VideoMode> DefaultVideoModes { get; set; } = [];
    public ObservableCollection<CustomVideoMode> VideoModes { get; set; } = [];
    public ObservableCollection<Channel> Channels { get; set; } = [];

    public ObservableCollection<OscClient> OscPredefinedClients { get; set; } = [];

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }


    public static Dictionary<string, LogLevel> LogLevelEnumValues { get; } = new()
    {
        { "trace" , LogLevel._trace },
        { "debug" , LogLevel._debug },
        { "info" , LogLevel._info },
        { "warning" , LogLevel._warning },
        { "error" , LogLevel._error },
        { "fatal" , LogLevel._fatal }
    };

    public static T? CheckForEnumValue<T>(string value)
    {
        try
        {
            string val = string.Format("_{0}", value);
            return (T)Enum.Parse(typeof(T), val) ?? default;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Data);
            Debug.WriteLine(e.Message);
            Debug.WriteLine(e.StackTrace);
            return default;
        }

    }

    private static void CheckConsumerSubregion(XmlDocument x, FrameConsumer consumer, XmlNode node)
    {
        if (consumer.SubregionEnable)
        {
            XmlNode n_sub = XH.NewNode(x, "subregion", node);
            XH.NewTextNode(x, "src-x", I(consumer.SourceX), n_sub);
            XH.NewTextNode(x, "src-y", I(consumer.SourceY), n_sub);
            XH.NewTextNode(x, "dest-x", I(consumer.DestinationX), n_sub);
            XH.NewTextNode(x, "dest-y", I(consumer.DestinationY), n_sub);
            XH.NewTextNode(x, "width", I(consumer.SubregionWidth), n_sub);
            XH.NewTextNode(x, "height", I(consumer.SubregionHeight), n_sub);
        }
    }

    private static void CheckPortSubregion(XmlDocument x, DecklinkPort port, XmlNode node)
    {
        if (port.SubregionEnable)
        {
            XmlNode n_sub = XH.NewNode(x, "subregion", node);
            XH.NewTextNode(x, "src-x", I(port.SourceX), n_sub);
            XH.NewTextNode(x, "src-y", I(port.SourceY), n_sub);
            XH.NewTextNode(x, "dest-x", I(port.DestinationX), n_sub);
            XH.NewTextNode(x, "dest-y", I(port.DestinationY), n_sub);
            XH.NewTextNode(x, "width", I(port.SubregionWidth), n_sub);
            XH.NewTextNode(x, "height", I(port.SubregionHeight), n_sub);
        }
    }

    public void SaveConfigFile(string file = "casparcg.config")
    {
        XH.NewDocument(out XmlDocument x, out XmlNode? configuration, "configuration");

        #region LOG
        XH.NewTextNode(x, "log-level", E(LogLevel), configuration);
        XH.NewTextNode(x, "log-align-columns", B(!LogDisableColumnAlignment), configuration);
        #endregion

        #region PATHS
        XmlNode n_paths = XH.NewNode(x, "paths", configuration);
        XH.NewTextNode(x, "media-path", MediaPath, n_paths);
        XmlNode n_logpath = XH.NewTextNode(x, "log-path", LogPath, n_paths);
        if (LogToFile == false)
        {
            XH.NewAttribute(x, "disable", "true", n_logpath);
        }
        XH.NewTextNode(x, "data-path", DataPath, n_paths);
        XH.NewTextNode(x, "font-path", FontPath, n_paths);
        XH.NewTextNode(x, "template-path", TemplatePath, n_paths);
        #endregion

        #region LOCK CLEAR PHRASE
        XH.NewTextNode(x, "lock-clear-phrase", LockPass, configuration);
        #endregion

        #region CHANNELS
        XmlNode n_channels = XH.NewNode(x, "channels", configuration);

        foreach (Channel ch in Channels)
        {
            XmlNode n_ch = XH.NewNode(x, "channel", n_channels);
            XH.NewTextNode(x, "video-mode", ch.VideoMode.Id, n_ch);
            if (ch.Producers.Any())
            {
                XmlNode n_ch_p = XH.NewNode(x, "producers", n_ch);
                foreach (Producer producer in ch.Producers)
                {
                    XmlNode n_prod = XH.NewTextNode(x, "producer", producer.Media, n_ch_p);
                    XmlNode n_prod_media = XH.NewAttribute(x, "id", I(producer.Layer), n_prod);
                }
            }
            XmlNode n_ch_c = XH.NewNode(x, "consumers", n_ch);
            foreach (object consumer in ch.Consumers)
            {
                switch (consumer)
                {
                    case DecklinkConsumer c:
                        XmlNode dl = XH.NewNode(x, "decklink", n_ch_c);
                        XH.NewTextNode(x, "device", I(c.Device), dl);
                        XH.NewTextNode(x, "key-device", I(c.KeyDevice), dl);
                        XH.NewTextNode(x, "embedded-audio", B(c.EmbeddedAudio), dl);
                        XH.NewTextNode(x, "latency", E(c.Latency), dl);
                        XH.NewTextNode(x, "keyer", E(c.Keyer), dl);
                        XH.NewTextNode(x, "key-only", B(c.KeyOnly), dl);
                        XH.NewTextNode(x, "buffer-depth", I(c.BufferDepth), dl);
                        if (c.VideoMode != "") XH.NewTextNode(x, "video-mode", c.VideoMode, dl);
                        XH.NewTextNode(x, "wait-for-reference", E(c.WaitForReference), dl);
                        XH.NewTextNode(x, "wait-for-reference-duration", I(c.WaitForReferenceDuration), dl);
                        CheckConsumerSubregion(x, c, dl);
                        if (c.Ports.Any())
                        {
                            XmlNode dl_p = XH.NewNode(x, "ports", dl);
                            foreach (DecklinkPort p in c.Ports)
                            {
                                XmlNode n_dl_p = XH.NewNode(x, "port", dl_p);
                                XH.NewTextNode(x, "device", I(p.Device), n_dl_p);
                                XH.NewTextNode(x, "key-only", B(p.KeyOnly), n_dl_p);
                                if (p.VideoMode != "") XH.NewTextNode(x, "video-mode", p.VideoMode, n_dl_p);
                                CheckPortSubregion(x, p, n_dl_p);
                            }
                        }
                        break;
                    case BluefishConsumer c:
                        XmlNode bf = XH.NewNode(x, "bluefish", n_ch_c);
                        XH.NewTextNode(x, "device", I(c.Device), bf);
                        XH.NewTextNode(x, "sdi-stream", I(c.SdiStream), bf);
                        XH.NewTextNode(x, "embedded-audio", B(c.EmbeddedAudio), bf);
                        XH.NewTextNode(x, "keyer", E(c.Keyer), bf);
                        XH.NewTextNode(x, "internal-keyer-audio-source", E(c.KeyerAudio), bf);
                        XH.NewTextNode(x, "watchdog", I(c.Watchdog), bf);
                        XH.NewTextNode(x, "uhd-mode", E(c.UhdMode), bf);
                        break;
                    case SystemAudioConsumer c:
                        XmlNode sa = XH.NewNode(x, "system-audio", n_ch_c);
                        XH.NewTextNode(x, "channel-layout", E(c.ChannelLayout), sa);
                        XH.NewTextNode(x, "latency", I(c.Latency), sa);
                        break;
                    case ScreenConsumer c:
                        string ar = c.Aspect switch
                        {
                            ScreenAspectRatio._16_9 => "16:9",
                            ScreenAspectRatio._4_3 => "4:3",
                            _ => "default",
                        };
                        string cs = c.ColourSpace switch
                        {
                            ScreenColourSpace._datavideo_full => "datavideo-full",
                            ScreenColourSpace._datavideo_limited => "datavideo-limited",
                            _ => "RGB",
                        };
                        XmlNode sc = XH.NewNode(x, "screen", n_ch_c);
                        if (!string.IsNullOrEmpty(c.Name)) XH.NewTextNode(x, "name", c.Name, sc);
                        XH.NewTextNode(x, "device", I(c.Device), sc);
                        XH.NewTextNode(x, "aspect-ratio", ar, sc);
                        XH.NewTextNode(x, "stretch", E(c.Stretch), sc);
                        XH.NewTextNode(x, "windowed", B(c.Windowed), sc);
                        XH.NewTextNode(x, "key-only", B(c.KeyOnly), sc);
                        XH.NewTextNode(x, "vsync", B(c.Vsync), sc);
                        XH.NewTextNode(x, "borderless", B(c.Borderless), sc);
                        XH.NewTextNode(x, "interactive", B(c.Interactive), sc);
                        XH.NewTextNode(x, "always-on-top", B(c.AlwaysOn), sc);
                        XH.NewTextNode(x, "x", I(c.X), sc);
                        XH.NewTextNode(x, "y", I(c.Y), sc);
                        XH.NewTextNode(x, "width", I(c.Width), sc);
                        XH.NewTextNode(x, "height", I(c.Height), sc);
                        XH.NewTextNode(x, "sbs-key", B(c.SbsKey), sc);
                        XH.NewTextNode(x, "colour-space", cs, sc);
                        break;
                    case NewtekIvgaConsumer c:
                        XmlNode ni = XH.NewNode(x, "newtek-ivga", n_ch_c);
                        break;
                    case NdiConsumer c:
                        XmlNode nn = XH.NewNode(x, "ndi", n_ch_c);
                        XH.NewTextNode(x, "name", c.Name, nn);
                        XH.NewTextNode(x, "allow-fields", B(c.AllowFields), nn);
                        break;
                    case FfmpegConsumer c:
                        XmlNode ff = XH.NewNode(x, "ffmpeg", n_ch_c);
                        XH.NewTextNode(x, "path", c.Path, ff);
                        XH.NewTextNode(x, "args", c.Arguments, ff);
                        break;
                    case ArtnetConsumer c:
                        XmlNode an = XH.NewNode(x, "artnet", n_ch_c);
                        XH.NewTextNode(x, "universe", I(c.Universe), an);
                        XH.NewTextNode(x, "host", c.Host, an);
                        XH.NewTextNode(x, "port", I(c.Port), an);
                        XH.NewTextNode(x, "refresh-rate", I(c.RefreshRate), an);
                        if (c.Fixtures.Any())
                        {
                            XmlNode an_f = XH.NewNode(x, "fixtures", an);
                            foreach (ArtnetFixture f in c.Fixtures)
                            {
                                XmlNode n_an_f = XH.NewNode(x, "fixture", an_f);
                                XH.NewTextNode(x, "type", E(f.Type), n_an_f);
                                XH.NewTextNode(x, "start-address", I(f.StartAddress), n_an_f);
                                XH.NewTextNode(x, "fixture-count", I(f.FixtureCount), n_an_f);
                                XH.NewTextNode(x, "fixture-channels", I(f.FixtureChannels), n_an_f);
                                XH.NewTextNode(x, "x", I(f.X), n_an_f);
                                XH.NewTextNode(x, "y", I(f.Y), n_an_f);
                                XH.NewTextNode(x, "width", I(f.Width), n_an_f);
                                XH.NewTextNode(x, "height", I(f.Height), n_an_f);
                                XH.NewTextNode(x, "rotation", I(f.Rotation), n_an_f);
                            }
                        }
                        break;
                }
            }
        }
        #endregion

        #region CONTROLLERS
        XmlNode n_cntr = XH.NewNode(x, "controllers", configuration);
        XmlNode n_cntr_tcp = XH.NewNode(x, "tcp", n_cntr);
        XH.NewTextNode(x, "port", I(AmcpPort), n_cntr_tcp);
        XH.NewTextNode(x, "protocol", "AMCP", n_cntr_tcp);
        #endregion

        #region AMCP
        XmlNode n_amcp = XH.NewNode(x, "amcp", configuration);
        XmlNode n_amcp_ms = XH.NewNode(x, "media-server", n_amcp);
        XH.NewTextNode(x, "host", MediaHost, n_amcp_ms);
        XH.NewTextNode(x, "port", I(MediaPort), n_amcp_ms);
        #endregion

        #region FLASH
        XmlNode n_flash = XH.NewNode(x, "flash", configuration);
        string v_flash_bd = (FlashBuffer > 0) ? I(FlashBuffer) : "auto";
        XH.NewTextNode(x, "buffer-depth", v_flash_bd, n_flash);
        XH.NewTextNode(x, "enabled", B(FlashEnableProducer), n_flash);
        #endregion

        #region HTML
        XmlNode n_html = XH.NewNode(x, "html", configuration);
        XH.NewTextNode(x, "remote-debugging-port", I(HtmlRemoteDebugPort), n_html);
        XH.NewTextNode(x, "enable-gpu", B(HtmlEnableGpu), n_html);
        XH.NewTextNode(x, "angle-backend", E(HtmlAngleBackend), n_html);
        if (HtmlCachePath is not null) XH.NewTextNode(x, "cache-path", HtmlCachePath, n_html);
        #endregion

        #region FFmpeg
        XmlNode n_ffmpeg = XH.NewNode(x, "ffmpeg", configuration);
        XmlNode n_ffmpeg_producer = XH.NewNode(x, "producer", n_ffmpeg);
        XH.NewTextNode(x, "auto-deinterlace", E(FfmpegDeinterlace), n_ffmpeg_producer);
        XH.NewTextNode(x, "threads", I(FfmpegThreads), n_ffmpeg_producer);
        #endregion

        #region NDI
        XmlNode n_ndi = XH.NewNode(x, "ndi", configuration);
        XH.NewTextNode(x, "auto-load", B(NdiAutoLoad), n_ndi);
        #endregion

        #region AUDIO
        if (!string.IsNullOrEmpty(DefaultAudioOutputDevice))
        {
            XmlNode n_audio = XH.NewNode(x, "system-audio", configuration);
            XmlNode n_audio_producer = XH.NewNode(x, "producer", n_audio);
            XH.NewTextNode(x, "default-device-name", DefaultAudioOutputDevice, n_audio_producer);
        }
        #endregion

        #region OSC
        XmlNode n_osc = XH.NewNode(x, "osc", configuration);
        XH.NewTextNode(x, "default-port", I(OscDefaultPort), n_osc);
        XH.NewTextNode(x, "disable-send-to-amcp-clients", B(OscDisableAmcpClients), n_osc);
        XmlNode n_osc_pc = XH.NewNode(x, "predefined-clients", n_osc);
        foreach (OscClient client in OscPredefinedClients)
        {
            XmlNode cl = XH.NewNode(x, "predefined-client", n_osc_pc);
            XH.NewTextNode(x, "address", client.Address, cl);
            XH.NewTextNode(x, "port", I(client.Port), cl);
        }
        #endregion

        #region VIDEO MODES
        if (VideoModes.Any())
        {
            XmlNode n_video_modes = XH.NewNode(x, "video-modes", configuration);
            foreach (CustomVideoMode mode in VideoModes)
            {
                XmlNode n_mode = XH.NewNode(x, "video-mode", n_video_modes);
                XH.NewTextNode(x, "id", mode.Id, n_mode);
                XH.NewTextNode(x, "width", I(mode.Width), n_mode);
                XH.NewTextNode(x, "height", I(mode.Height), n_mode);
                XH.NewTextNode(x, "time-scale", I(mode.TimeScale), n_mode);
                XH.NewTextNode(x, "duration", I(mode.Duration), n_mode);
                XH.NewTextNode(x, "cadence", I(mode.Cadence), n_mode);
            }
        }
        #endregion

        #region COMMENTS

        foreach(string comment in Comments)
        {
            var newComment = x.CreateComment(comment);
            x.AppendChild(newComment);
        }

        #endregion

        x.Save(file);
    }

    public void LoadConfigFile()
    {
        if (File is null) return;
        XDocument configFile;
        XElement configuration;

        configFile = XDocument.Load(File);
        configuration = configFile.Descendants("configuration").First();
        if (configuration is null || !configuration.Descendants().Any()) throw new Exception();

        if (configuration.Descendants("video-modes").Any()
            && configuration.Descendants("video-modes").First() is XElement modesElement
            && modesElement.Descendants("video-mode").Any())
        {
            foreach (XElement videoMode in modesElement.Descendants("video-mode"))
            {
                CustomVideoMode newVideoMode = new();
                foreach (XElement sub_element in videoMode.Descendants())
                {
                    switch (sub_element.Name.LocalName)
                    {
                        case "id":
                            newVideoMode.Id = sub_element.Value;
                            break;
                        case "width":
                            newVideoMode.Width = Convert.ToInt32(sub_element.Value);
                            break;
                        case "height":
                            newVideoMode.Height = Convert.ToInt32(sub_element.Value);
                            break;
                        case "time-scale":
                            newVideoMode.TimeScale = Convert.ToInt32(sub_element.Value);
                            break;
                        case "duration":
                            newVideoMode.Duration = Convert.ToInt32(sub_element.Value);
                            break;
                        case "cadence":
                            newVideoMode.Cadence = Convert.ToInt32(sub_element.Value);
                            break;
                    }
                }
                VideoModes.Add(newVideoMode);
            }
        }

        foreach (XElement element in configuration.Descendants())
        {
            switch (element.Name.LocalName)
            {
                case "log-level":
                    LogLevel = CheckForEnumValue<LogLevel>(element.Value);;
                    break;
                case "log-align-columns":
                    LogDisableColumnAlignment = !Convert.ToBoolean(element.Value);
                    break;

                case "paths":
                    foreach (XElement sub_element in element.Descendants())
                    {
                        switch (sub_element.Name.LocalName)
                        {
                            case "media-path":
                                MediaPath = sub_element.Value;
                                break;
                            case "data-path":
                                DataPath = sub_element.Value;
                                break;
                            case "log-path":
                                LogPath = sub_element.Value;
                                if (sub_element.HasAttributes && sub_element.Attribute("disable") != null && sub_element.Attribute("disable").Value == "true")
                                    LogToFile = false;
                                else
                                    LogToFile = true;
                                break;
                            case "template-path":
                                TemplatePath = sub_element.Value;
                                break;
                            case "font-path":
                                FontPath = sub_element.Value;
                                break;
                        }
                    }
                    break;

                case "lock-clear-phrase":
                    LockPass = element.Value;
                    break;

                case "flash":
                    foreach (XElement sub_element in element.Descendants())
                    {
                        switch (sub_element.Name.LocalName)
                        {
                            case "buffer-depth":
                                FlashBuffer = (sub_element.Value == "auto") ? 0 : Convert.ToInt32(sub_element.Value);
                                break;
                            case "enabled":
                                FlashEnableProducer = Convert.ToBoolean(sub_element.Value);
                                break;
                        }
                    }
                    break;

                case "html":
                    HtmlCachePath = null;
                    foreach (XElement sub_element in element.Descendants())
                    {
                        switch (sub_element.Name.LocalName)
                        {
                            case "remote-debugging-port":
                                HtmlRemoteDebugPort = Convert.ToInt32(sub_element.Value);
                                break;
                            case "enable-gpu":
                                HtmlEnableGpu = Convert.ToBoolean(sub_element.Value);
                                break;
                            case "angle-backend":
                                HtmlAngleBackend = CheckForEnumValue<HtmlAngleGraphicsBackend>(sub_element.Value);;
                                break;
                            case "cache-path":
                                HtmlCachePath = sub_element.Value;
                                break;
                        }
                    }
                    break;

                case "ffmpeg":
                    if (element.Descendants("producer").Any())
                    {
                        foreach (XElement sub_element in element.Descendants("producer").Descendants())
                        {
                            switch (sub_element.Name.LocalName)
                            {
                                case "auto-deinterlace":
                                    FfmpegDeinterlace = CheckForEnumValue<FfmpegDeinterlace>(sub_element.Value);;
                                    break;
                                case "threads":
                                    FfmpegThreads = Convert.ToInt32(sub_element.Value);
                                    break;
                            }
                        }
                    }
                    break;

                case "controllers":
                    if (element.Descendants("tcp").Any())
                    {
                        foreach (XElement sub_element in element.Descendants("tcp").Descendants())
                        {
                            switch (sub_element.Name.LocalName)
                            {
                                case "port":
                                    AmcpPort = Convert.ToInt32(sub_element.Value);
                                    break;
                                case "protocol":
                                    break;
                            }
                        }
                    }
                    break;

                case "amcp":
                    if (element.Descendants("media-server").Any())
                    {
                        foreach (XElement sub_element in element.Descendants("media-server").Descendants())
                        {
                            switch (sub_element.Name.LocalName)
                            {
                                case "host":
                                    MediaHost = sub_element.Value;
                                    break;
                                case "port":
                                    MediaPort = Convert.ToInt32(sub_element.Value);
                                    break;
                            }
                        }
                    }
                    break;

                case "ndi":
                    if (element.Descendants("auto-load").Any())
                    {
                        NdiAutoLoad = Convert.ToBoolean(element.Descendants("auto-load").First().Value);
                    }
                    break;

                case "system-audio":
                    if (element.Descendants("producer").Any() && element.Descendants("producer").First().Descendants("default-device-name").Any())
                    {
                        DefaultAudioOutputDevice = element.Descendants("producer").First().Descendants("default-device-name").First().Value;
                    }
                    break;

                case "osc":
                    foreach (XElement sub_element in element.Descendants())
                    {
                        switch (sub_element.Name.LocalName)
                        {
                            case "default-port":
                                OscDefaultPort = Convert.ToInt32(sub_element.Value);
                                break;
                            case "disable-send-to-amcp-clients":
                                OscDisableAmcpClients = Convert.ToBoolean(sub_element.Value);
                                break;
                            case "predefined-clients":
                                if (sub_element.Descendants("predefined-client").Any())
                                {
                                    foreach (XElement osc_client in sub_element.Descendants("predefined-client"))
                                    {
                                        OscClient client = new();
                                        foreach (XElement osc_client_node in osc_client.Descendants())
                                        {
                                            switch (osc_client_node.Name.LocalName)
                                            {
                                                case "address":
                                                    client.Address = osc_client_node.Value;
                                                    break;
                                                case "port":
                                                    client.Port = Convert.ToInt32(osc_client_node.Value);
                                                    break;
                                            }
                                        }
                                        OscPredefinedClients.Add(client);
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case "channels":
                    if (element.Descendants("channel").Any())
                    {
                        foreach (XElement channel_node in element.Descendants("channel"))
                        {
                            Channel channel = new();
                            if (channel_node.Descendants("video-mode").Any())
                            {
                                string channel_format = channel_node.Descendants("video-mode").First().Value;
                                VideoMode? targetVideoMode = null;
                                if (VideoModes.Any()) targetVideoMode = VideoModes.FirstOrDefault(m => m.Id == channel_format);
                                targetVideoMode ??= DefaultVideoModes.FirstOrDefault(m => m.Id == channel_format);
                                targetVideoMode ??= DefaultVideoModes.First();
                                channel.VideoMode = targetVideoMode;
                            }
                            if (channel_node.Descendants("producers").Any())
                            {
                                foreach(XElement producer in channel_node.Descendants("producers").First().Descendants())
                                {
                                    Debug.WriteLine(producer.ToString());
                                    if (producer.Attribute("id") is XAttribute id)
                                    {
                                        channel.Producers.Add(new()
                                        {
                                            Layer = int.Parse(id.Value),
                                            Media = producer.Value
                                        });
                                    }
                                }
                            }
                            if (channel_node.Descendants("consumers").Any())
                            {
                                foreach (XElement consumer in channel_node.Descendants("consumers").First().Descendants())
                                {
                                    switch (consumer.Name.LocalName)
                                    {
                                        case "decklink":
                                            DecklinkConsumer n_dl = new();
                                            channel.Consumers.Add(n_dl);
                                            if (consumer.Descendants().Any())
                                            {
                                                foreach (XElement consumer_sub in consumer.Descendants())
                                                {
                                                    switch (consumer_sub.Name.LocalName)
                                                    {
                                                        case "device":
                                                            n_dl.Device = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "key-device":
                                                            n_dl.KeyDevice = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "embedded-audio":
                                                            n_dl.EmbeddedAudio = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "latency":
                                                            n_dl.Latency = CheckForEnumValue<DecklinkLatency>(consumer_sub.Value);;
                                                            break;
                                                        case "keyer":
                                                            n_dl.Keyer = CheckForEnumValue<DecklinkKeyer>(consumer_sub.Value);;
                                                            break;
                                                        case "key-only":
                                                            n_dl.KeyOnly = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "buffer-depth":
                                                            n_dl.BufferDepth = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "video-mode":
                                                            string decklink_format = consumer_sub.Value;
                                                            VideoMode? decklinkVideoMode = null;
                                                            decklinkVideoMode ??= DefaultVideoModes.FirstOrDefault(m => m.Id == consumer_sub.Value);
                                                            decklinkVideoMode ??= DefaultVideoModes.First();
                                                            n_dl.VideoMode = decklinkVideoMode.Id;
                                                            break;
                                                        case "wait-for-reference":
                                                            n_dl.WaitForReference = CheckForEnumValue<DecklinkWaitForReference>(consumer_sub.Value);;
                                                            break;
                                                        case "wait-for-reference-duration":
                                                            n_dl.WaitForReferenceDuration = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "ports":
                                                            if (consumer_sub.Descendants("port").Any())
                                                            {
                                                                foreach (XElement port in consumer_sub.Descendants("port"))
                                                                {
                                                                    DecklinkPort n_dl_p = new();
                                                                    if (port.Descendants().Any())
                                                                    {
                                                                        foreach(XElement port_sub in port.Descendants())
                                                                        {
                                                                            switch (port_sub.Name.LocalName)
                                                                            {
                                                                                case "device":
                                                                                    n_dl_p.Device = Convert.ToInt32(port_sub.Value);
                                                                                    break;
                                                                                case "key-only":
                                                                                    n_dl_p.KeyOnly = Convert.ToBoolean(port_sub.Value);
                                                                                    break;
                                                                                case "video-mode":
                                                                                    string port_format = port_sub.Value;
                                                                                    VideoMode? portVideoMode = null;
                                                                                    portVideoMode ??= DefaultVideoModes.FirstOrDefault(m => m.Id == port_sub.Value);
                                                                                    portVideoMode ??= DefaultVideoModes.First();
                                                                                    n_dl_p.VideoMode = portVideoMode.Id;
                                                                                    break;
                                                                            }
                                                                        }
                                                                        CheckPortSubregion(n_dl_p, port);
                                                                        n_dl.Ports.Add(n_dl_p);
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                                CheckConsumerSubregion(n_dl, consumer);
                                            }
                                            break;
                                        case "bluefish":
                                            BluefishConsumer n_bf = new();
                                            channel.Consumers.Add(n_bf);
                                            if (consumer.Descendants().Any())
                                            {
                                                foreach (XElement consumer_sub in consumer.Descendants())
                                                {
                                                    switch (consumer_sub.Name.LocalName)
                                                    {
                                                        case "device":
                                                            n_bf.Device = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "sdi-stream":
                                                            n_bf.SdiStream = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "embedded-audio":
                                                            n_bf.EmbeddedAudio = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "internal-keyer-audio-source":
                                                            n_bf.KeyerAudio = CheckForEnumValue<BluefishKeyerAudio>(consumer_sub.Value);;
                                                            break;
                                                        case "keyer":
                                                            n_bf.Keyer = CheckForEnumValue<BluefishKeyer>(consumer_sub.Value);;
                                                            break;
                                                        case "watchdog":
                                                            n_bf.Watchdog = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "uhd-mode":
                                                            n_bf.UhdMode = CheckForEnumValue<BluefishUhdMode>(consumer_sub.Value);;
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case "system-audio":
                                            SystemAudioConsumer n_sa = new();
                                            channel.Consumers.Add(n_sa);
                                            if (consumer.Descendants().Any())
                                            {
                                                foreach (XElement consumer_sub in consumer.Descendants())
                                                {
                                                    switch (consumer_sub.Name.LocalName)
                                                    {
                                                        case "channel-layout":
                                                            n_sa.ChannelLayout = CheckForEnumValue<SystemAudioChannelLayout>(consumer_sub.Value);;
                                                            break;
                                                        case "latency":
                                                            n_sa.Latency = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case "screen":
                                            ScreenConsumer n_sc = new();
                                            channel.Consumers.Add(n_sc);
                                            if (consumer.Descendants().Any())
                                            {
                                                foreach (XElement consumer_sub in consumer.Descendants())
                                                {
                                                    switch (consumer_sub.Name.LocalName)
                                                    {
                                                        case "name":
                                                            n_sc.Name = consumer_sub.Value;
                                                            break;
                                                        case "device":
                                                            n_sc.Device = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "aspect-ratio":
                                                            n_sc.Aspect = consumer_sub.Value switch
                                                            {
                                                                "4:3" => ScreenAspectRatio._4_3,
                                                                "16:9" => ScreenAspectRatio._16_9,
                                                                _ => ScreenAspectRatio._default,
                                                            };
                                                            break;
                                                        case "stretch":
                                                            n_sc.Stretch = CheckForEnumValue<ScreenStretch>(consumer_sub.Value);;
                                                            break;
                                                        case "windowed":
                                                            n_sc.Windowed = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "key-only":
                                                            n_sc.KeyOnly = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "vsync":
                                                            n_sc.Vsync = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "borderless":
                                                            n_sc.Borderless = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "interactive":
                                                            n_sc.Interactive = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "always-on-top":
                                                            n_sc.AlwaysOn = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "x":
                                                            n_sc.X = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "y":
                                                            n_sc.Y = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "width":
                                                            n_sc.Width = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "height":
                                                            n_sc.Height = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "sbs-key":
                                                            n_sc.SbsKey = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                        case "colour-space":
                                                            n_sc.ColourSpace = consumer_sub.Value switch
                                                            {
                                                                "datavideo-full" => ScreenColourSpace._datavideo_full,
                                                                "datavideo-limited" => ScreenColourSpace._datavideo_limited,
                                                                _ => ScreenColourSpace._rgb,
                                                            };
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case "newtek-ivga":
                                            NewtekIvgaConsumer n_ni = new();
                                            channel.Consumers.Add(n_ni);
                                            break;
                                        case "ndi":
                                            NdiConsumer n_nn = new();
                                            channel.Consumers.Add(n_nn);
                                            if (consumer.Descendants().Any())
                                            {
                                                foreach (XElement consumer_sub in consumer.Descendants())
                                                {
                                                    switch (consumer_sub.Name.LocalName)
                                                    {
                                                        case "name":
                                                            n_nn.Name = consumer_sub.Value;
                                                            break;
                                                        case "allow-fields":
                                                            n_nn.AllowFields = Convert.ToBoolean(consumer_sub.Value);
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case "ffmpeg":
                                            FfmpegConsumer n_ff = new();
                                            channel.Consumers.Add(n_ff);
                                            if (consumer.Descendants().Any())
                                            {
                                                foreach (XElement consumer_sub in consumer.Descendants())
                                                {
                                                    switch (consumer_sub.Name.LocalName)
                                                    {
                                                        case "path":
                                                            n_ff.Path = consumer_sub.Value;
                                                            break;
                                                        case "args":
                                                            n_ff.Arguments = consumer_sub.Value;
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case "artnet":
                                            ArtnetConsumer n_an = new();
                                            channel.Consumers.Add(n_an);
                                            if (consumer.Descendants().Any())
                                            {
                                                foreach (XElement consumer_sub in consumer.Descendants())
                                                {
                                                    switch (consumer_sub.Name.LocalName)
                                                    {
                                                        case "universe":
                                                            n_an.Universe = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "host":
                                                            n_an.Host = consumer_sub.Value;
                                                            break;
                                                        case "port":
                                                            n_an.Port = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "refresh-rate":
                                                            n_an.RefreshRate = Convert.ToInt32(consumer_sub.Value);
                                                            break;
                                                        case "fixtures":
                                                            if (consumer_sub.Descendants("fixture").Any())
                                                            {
                                                                foreach (XElement fixture in consumer_sub.Descendants("fixture"))
                                                                {
                                                                    ArtnetFixture n_an_f = new();
                                                                    if (fixture.Descendants().Any())
                                                                    {
                                                                        foreach (XElement fixture_sub in fixture.Descendants())
                                                                        {
                                                                            switch (fixture_sub.Name.LocalName)
                                                                            {
                                                                                case "type":
                                                                                    n_an_f.Type = CheckForEnumValue<ArtnetFixtureType>(fixture_sub.Value);
                                                                                    break;
                                                                                case "start-address":
                                                                                    n_an_f.StartAddress = Convert.ToInt32(fixture_sub.Value);
                                                                                    break;
                                                                                case "fixture-count":
                                                                                    n_an_f.FixtureCount = Convert.ToInt32(fixture_sub.Value);
                                                                                    break;
                                                                                case "fixture-channels":
                                                                                    n_an_f.FixtureChannels = Convert.ToInt32(fixture_sub.Value);
                                                                                    break;
                                                                                case "x":
                                                                                    n_an_f.X = Convert.ToInt32(fixture_sub.Value);
                                                                                    break;
                                                                                case "y":
                                                                                    n_an_f.Y = Convert.ToInt32(fixture_sub.Value);
                                                                                    break;
                                                                                case "width":
                                                                                    n_an_f.Width = Convert.ToInt32(fixture_sub.Value);
                                                                                    break;
                                                                                case "height":
                                                                                    n_an_f.Height = Convert.ToInt32(fixture_sub.Value);
                                                                                    break;
                                                                                case "rotation":
                                                                                    n_an_f.Rotation = Convert.ToInt32(fixture_sub.Value);
                                                                                    break;
                                                                            }
                                                                        }
                                                                        n_an.Fixtures.Add(n_an_f);
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                            Channels.Add(channel);
                        }
                    }
                    break;
            }
        }

        if (configFile.Nodes().Where(n => n is XComment) is IEnumerable<XNode> comments && comments.Any())
        {
            Comments.Clear();
            foreach (XComment comment in comments)
            {
                Comments.Add(comment.Value);
            }
        }

    }

    public static string E(object v)
    {

        return Enum.GetName(v.GetType(), v)?[1..] ?? "";
    }

    public static string B(bool v)
    {
        return v.ToString().ToLower();
    }

    public static string I(int v)
    {
        return v.ToString();
    }

    private void CheckConsumerSubregion(FrameConsumer consumer, XElement node)
    {
        if (node.Element("subregion") is XElement subregion && subregion.Descendants().Any())
        {
            consumer.SubregionEnable = true;
            foreach (XElement sub in subregion.Descendants())
            {
                switch (sub.Name.LocalName)
                {
                    case "width":
                        consumer.SubregionWidth = Convert.ToInt32(sub.Value);
                        break;
                    case "height":
                        consumer.SubregionHeight = Convert.ToInt32(sub.Value);
                        break;
                    case "src-x":
                        consumer.SourceX = Convert.ToInt32(sub.Value);
                        break;
                    case "src-y":
                        consumer.SourceY = Convert.ToInt32(sub.Value);
                        break;
                    case "dest-x":
                        consumer.DestinationX = Convert.ToInt32(sub.Value);
                        break;
                    case "dest-y":
                        consumer.DestinationY = Convert.ToInt32(sub.Value);
                        break;
                }
            }
        }
    }

    private void CheckPortSubregion(DecklinkPort port, XElement node)
    {
        if (node.Element("subregion") is XElement subregion && subregion.Descendants().Any())
        {
            port.SubregionEnable = true;
            foreach (XElement sub in subregion.Descendants())
            {
                switch (sub.Name.LocalName)
                {
                    case "width":
                        port.SubregionWidth = Convert.ToInt32(sub.Value);
                        break;
                    case "height":
                        port.SubregionHeight = Convert.ToInt32(sub.Value);
                        break;
                    case "src-x":
                        port.SourceX = Convert.ToInt32(sub.Value);
                        break;
                    case "src-y":
                        port.SourceY = Convert.ToInt32(sub.Value);
                        break;
                    case "dest-x":
                        port.DestinationX = Convert.ToInt32(sub.Value);
                        break;
                    case "dest-y":
                        port.DestinationY = Convert.ToInt32(sub.Value);
                        break;
                }
            }
        }
    }

}