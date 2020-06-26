using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using XH = CasparLauncher.XmlHelper;

namespace CasparLauncher
{
    public class ConfigFile : INotifyPropertyChanged
    {
        private string _file;
        public string File
        {
            get
            {
                return _file;
            }
            set
            {
                if(_file != value)
                {
                    _file = value;
                    OnPropertyChanged("File");
                    OnPropertyChanged("FileName");
                }
            }
        }
        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(_file);
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
                    OnPropertyChanged("LogLevel");
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
                    OnPropertyChanged("FlashBuffer");
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
                    OnPropertyChanged("NdiAutoLoad");
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
                    OnPropertyChanged("HtmlRemoteDebugPort");
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
                    OnPropertyChanged("HtmlEnableGpu");
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
                    OnPropertyChanged("FlashEnableProducer");
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
                    OnPropertyChanged("FfmpegThreads");
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
                    OnPropertyChanged("FfmpegDeinterlace");
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
                    OnPropertyChanged("MediaPath");
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
                    OnPropertyChanged("LogPath");
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
                    OnPropertyChanged("DataPath");
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
                    OnPropertyChanged("TemplatePath");
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
                    OnPropertyChanged("FontPath");
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
                    OnPropertyChanged("LockPass");
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
                    OnPropertyChanged("OscDefaultPort");
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
                    OnPropertyChanged("OscDisableAmcpClients");
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
                    OnPropertyChanged("AmcpPort");
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
                    OnPropertyChanged("MediaHost");
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
                    OnPropertyChanged("MediaPort");
                }
            }
        }


        public ObservableCollection<Channel> Channels { get; set; } = new ObservableCollection<Channel>();

        public ObservableCollection<OscClient> OscPredefinedClients { get; set; } = new ObservableCollection<OscClient>();
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }


        public static Dictionary<string, LogLevel> LogLevelEnumValues = new Dictionary<string, LogLevel>()
        {
            { "trace" , LogLevel._trace },
            { "debug" , LogLevel._debug },
            { "info" , LogLevel._info },
            { "warning" , LogLevel._warning },
            { "error" , LogLevel._error },
            { "fatal" , LogLevel._fatal }
        };

        public static object CheckForEnumValue(Type prop, string value)
        {
            try
            {
                string val = string.Format("_{0}", value);
                return Enum.Parse(prop, val);
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public void SaveConfigFile(string file = "casparcg.config")
        {
            XH.NewDocument(out XmlDocument x, out XmlNode configuration, "configuration");

            #region LOG LEVEL
            XH.NewTextNode(x,"log-level", E(LogLevel), configuration);
            #endregion

            #region PATHS
            XmlNode n_paths = XH.NewNode(x, "paths", configuration);
            XH.NewTextNode(x, "media-path", MediaPath, n_paths);
            XH.NewTextNode(x, "log-path", LogPath, n_paths);
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
                XH.NewTextNode(x, "video-mode", E(ch.VideoMode), n_ch);
                XmlNode n_ch_c = XH.NewNode(x, "consumers", n_ch);
                foreach (object consumer in ch.Consumers)
                {
                    if (consumer is DecklinkConsumer)
                    {
                        DecklinkConsumer c = consumer as DecklinkConsumer;
                        XmlNode dl = XH.NewNode(x, "decklink", n_ch_c);
                        XH.NewTextNode(x, "device", I(c.Device), dl);
                        XH.NewTextNode(x, "key-device", I(c.KeyDevice), dl);
                        XH.NewTextNode(x, "embedded-audio", B(c.EmbeddedAudio), dl);
                        XH.NewTextNode(x, "latency", E(c.Latency), dl);
                        XH.NewTextNode(x, "keyer", E(c.Keyer), dl);
                        XH.NewTextNode(x, "key-only", B(c.KeyOnly), dl);
                        XH.NewTextNode(x, "buffer-depth", I(c.BufferDepth), dl);
                    }
                    if (consumer is BluefishConsumer)
                    {
                        BluefishConsumer c = consumer as BluefishConsumer;
                        XmlNode bf = XH.NewNode(x, "bluefish", n_ch_c);
                        XH.NewTextNode(x, "device", I(c.Device), bf);
                        XH.NewTextNode(x, "sdi-stream", I(c.SdiStream), bf);
                        XH.NewTextNode(x, "embedded-audio", B(c.EmbeddedAudio), bf);
                        XH.NewTextNode(x, "keyer", E(c.Keyer), bf);
                        XH.NewTextNode(x, "internal-keyer-audio-source", E(c.KeyerAudio), bf);
                        XH.NewTextNode(x, "watchdog", I(c.Watchdog), bf);
                    }
                    if (consumer is SystemAudioConsumer)
                    {
                        SystemAudioConsumer c = consumer as SystemAudioConsumer;
                        XmlNode sa = XH.NewNode(x, "system-audio", n_ch_c);
                        XH.NewTextNode(x, "channel-layout", E(c.ChannelLayout), sa);
                        XH.NewTextNode(x, "latency", I(c.Latency), sa);
                    }
                    if (consumer is ScreenConsumer)
                    {
                        ScreenConsumer c = consumer as ScreenConsumer;

                        string ar;
                        switch (c.Aspect)
                        {
                            case ScreenAspectRatio._default:
                            default:
                                ar = "default";
                                break;
                            case ScreenAspectRatio._16_9:
                                ar = "16:9";
                                break;
                            case ScreenAspectRatio._4_3:
                                ar = "4:3";
                                break;
                        }
                        string cs;
                        switch (c.ColourSpace)
                        {
                            case ScreenColourSpace._rgb:
                            default:
                                cs = "RGB";
                                break;
                            case ScreenColourSpace._datavideo_full:
                                cs = "datavideo-full";
                                break;
                            case ScreenColourSpace._datavideo_limited:
                                cs = "datavideo-limited";
                                break;
                        }

                        XmlNode sc = XH.NewNode(x, "screen", n_ch_c);
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
                    }
                    if (consumer is NewtekIvgaConsumer)
                    {
                        XmlNode ni = XH.NewNode(x, "newtek-ivga", n_ch_c);
                    }
                    if (consumer is NdiConsumer)
                    {
                        NdiConsumer c = consumer as NdiConsumer;
                        XmlNode nn = XH.NewNode(x, "ndi", n_ch_c);
                        XH.NewTextNode(x, "name", c.Name, nn);
                        XH.NewTextNode(x, "allow-fields", B(c.AllowFields), nn);
                    }
                    if (consumer is FfmpegConsumer)
                    {
                        FfmpegConsumer c = consumer as FfmpegConsumer;
                        XmlNode ff = XH.NewNode(x, "ffmpeg", n_ch_c);
                        XH.NewTextNode(x, "path", c.Path, ff);
                        XH.NewTextNode(x, "args", c.Arguments, ff);
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

            #region OSC
            XmlNode n_osc = XH.NewNode(x, "osc", configuration);
            XH.NewTextNode(x, "default-port", I(OscDefaultPort), n_osc);
            XH.NewTextNode(x, "disable-send-to-amcp-clients", B(OscDisableAmcpClients), n_osc);
            XmlNode n_osc_pc = XH.NewNode(x, "predefined-clients", n_osc);
            foreach(OscClient client in OscPredefinedClients)
            {
                XmlNode cl = XH.NewNode(x, "predefined-client", n_osc_pc);
                XH.NewTextNode(x, "address", client.Address, cl);
                XH.NewTextNode(x, "port", I(client.Port), cl);
            }
            #endregion
            
            x.Save(file);
        }

        public void LoadConfigFile()
        {
            XDocument configFile;
            XElement configuration;
            try
            {
                configFile = XDocument.Load(File);
                configuration = configFile.Descendants("configuration").First();
                if (configuration is null || !configuration.Descendants().Any()) return;
            }
            catch(Exception e)
            {
                return;
            }

            foreach(XElement element in configuration.Descendants())
            {
                switch(element.Name.LocalName)
                {
                    case "log-level":
                        LogLevel = (LogLevel)CheckForEnumValue(LogLevel.GetType(), element.Value);
                        break;

                    case "paths":
                        foreach(XElement sub_element in element.Descendants())
                        {
                            switch(sub_element.Name.LocalName)
                            {
                                case "media-path":
                                    MediaPath = sub_element.Value;
                                    break;
                                case "data-path":
                                    DataPath = sub_element.Value;
                                    break;
                                case "log-path":
                                    LogPath = sub_element.Value;
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
                                        FfmpegDeinterlace = (FfmpegDeinterlace)CheckForEnumValue(FfmpegDeinterlace.GetType(), sub_element.Value);
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
                                    if(sub_element.Descendants("predefined-client").Any())
                                    {
                                        foreach (XElement osc_client in sub_element.Descendants("predefined-client"))
                                        {
                                            OscClient client = new OscClient();
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
                            foreach(XElement channel_node in element.Descendants("channel"))
                            {
                                Channel channel = new Channel();
                                if(channel_node.Descendants("video-mode").Any()) channel.VideoMode = (VideoMode)CheckForEnumValue(typeof(VideoMode), channel_node.Descendants("video-mode").First().Value);
                                if(channel_node.Descendants("consumers").Any())
                                {
                                    foreach(XElement consumer in channel_node.Descendants("consumers").First().Descendants())
                                    {
                                        switch(consumer.Name.LocalName)
                                        {
                                            case "decklink":
                                                DecklinkConsumer n_dl = new DecklinkConsumer();
                                                channel.Consumers.Add(n_dl);
                                                if (consumer.Descendants().Any())
                                                {
                                                    foreach(XElement consumer_sub in consumer.Descendants())
                                                    {
                                                        switch(consumer_sub.Name.LocalName)
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
                                                                n_dl.Latency = (DecklinkLatency)CheckForEnumValue(typeof(DecklinkLatency), consumer_sub.Value);
                                                                break;
                                                            case "keyer":
                                                                n_dl.Keyer = (DecklinkKeyer)CheckForEnumValue(typeof(DecklinkKeyer), consumer_sub.Value);
                                                                break;
                                                            case "key-only":
                                                                n_dl.KeyOnly = Convert.ToBoolean(consumer_sub.Value);
                                                                break;
                                                            case "buffer-depth":
                                                                n_dl.BufferDepth = Convert.ToInt32(consumer_sub.Value);
                                                                break;
                                                        }
                                                    }
                                                }
                                                break;
                                            case "bluefish":
                                                BluefishConsumer n_bf = new BluefishConsumer();
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
                                                                n_bf.KeyerAudio = (BluefishKeyerAudio)CheckForEnumValue(typeof(BluefishKeyerAudio), consumer_sub.Value);
                                                                break;
                                                            case "keyer":
                                                                n_bf.Keyer = (BluefishKeyer)CheckForEnumValue(typeof(BluefishKeyer), consumer_sub.Value);
                                                                break;
                                                            case "watchdog":
                                                                n_bf.Watchdog = Convert.ToInt32(consumer_sub.Value);
                                                                break;
                                                        }
                                                    }
                                                }
                                                break;
                                            case "system-audio":
                                                SystemAudioConsumer n_sa = new SystemAudioConsumer();
                                                channel.Consumers.Add(n_sa);
                                                if (consumer.Descendants().Any())
                                                {
                                                    foreach (XElement consumer_sub in consumer.Descendants())
                                                    {
                                                        switch (consumer_sub.Name.LocalName)
                                                        {
                                                            case "channel-layout":
                                                                n_sa.ChannelLayout = (SystemAudioChannelLayout)CheckForEnumValue(typeof(SystemAudioChannelLayout), consumer_sub.Value);
                                                                break;
                                                            case "latency":
                                                                n_sa.Latency = Convert.ToInt32(consumer_sub.Value);
                                                                break;
                                                        }
                                                    }
                                                }
                                                break;
                                            case "screen":
                                                ScreenConsumer n_sc = new ScreenConsumer();
                                                channel.Consumers.Add(n_sc);
                                                if (consumer.Descendants().Any())
                                                {
                                                    foreach (XElement consumer_sub in consumer.Descendants())
                                                    {
                                                        switch (consumer_sub.Name.LocalName)
                                                        {
                                                            case "device":
                                                                n_sc.Device = Convert.ToInt32(consumer_sub.Value);
                                                                break;
                                                            case "aspect-ratio":
                                                                switch (consumer_sub.Value)
                                                                {
                                                                    case "default":
                                                                    default:
                                                                        n_sc.Aspect = ScreenAspectRatio._default;
                                                                        break;
                                                                    case "4:3":
                                                                        n_sc.Aspect = ScreenAspectRatio._4_3;
                                                                        break;
                                                                    case "16:9":
                                                                        n_sc.Aspect = ScreenAspectRatio._16_9;
                                                                        break;
                                                                }
                                                                break;
                                                            case "stretch":
                                                                n_sc.Stretch = (ScreenStretch)CheckForEnumValue(typeof(ScreenStretch), consumer_sub.Value);
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
                                                                switch(consumer_sub.Value)
                                                                {
                                                                    case "RGB":
                                                                    default:
                                                                        n_sc.ColourSpace = ScreenColourSpace._rgb;
                                                                        break;
                                                                    case "datavideo-full":
                                                                        n_sc.ColourSpace = ScreenColourSpace._datavideo_full;
                                                                        break;
                                                                    case "datavideo-limited":
                                                                        n_sc.ColourSpace = ScreenColourSpace._datavideo_limited;
                                                                        break;
                                                                }
                                                                break;
                                                        }
                                                    }
                                                }
                                                break;
                                            case "newtek-ivga":
                                                NewtekIvgaConsumer n_ni = new NewtekIvgaConsumer();
                                                channel.Consumers.Add(n_ni);
                                                break;
                                            case "ndi":
                                                NdiConsumer n_nn = new NdiConsumer();
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
                                                FfmpegConsumer n_ff = new FfmpegConsumer();
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
                                        }
                                    }
                                }
                                Channels.Add(channel);
                            }
                        }
                        break;
                }
            }
            
        }

        public static string E(object v)
        {
            
            return Enum.GetName(v.GetType(), v).Substring(1);
        }

        public static string B(bool v)
        {
            return v.ToString().ToLower();
        }

        public static string I(int v)
        {
            return v.ToString();
        }


    }

    public class Channel : INotifyPropertyChanged
    {
        private VideoMode _videoMode = VideoMode._PAL;
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
                    OnPropertyChanged("VideoMode");
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
                    OnPropertyChanged("SelectedConsumer");
                }
            }
        }

        public ObservableCollection<object> Consumers { get; set; } = new ObservableCollection<object>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }
    
    public class DecklinkConsumer : INotifyPropertyChanged
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
                    OnPropertyChanged("Device");
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
                    OnPropertyChanged("KeyDevice");
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
                    OnPropertyChanged("EmbeddedAudio");
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
                    OnPropertyChanged("Latency");
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
                    OnPropertyChanged("Keyer");
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
                    OnPropertyChanged("KeyOnly");
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
                    OnPropertyChanged("KeyDevice");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class BluefishConsumer : INotifyPropertyChanged
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
                    OnPropertyChanged("Device");
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
                    OnPropertyChanged("SdiStream");
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
                    OnPropertyChanged("EmbeddedAudio");
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
                    OnPropertyChanged("Keyer");
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
                    OnPropertyChanged("KeyerAudio");
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class ScreenConsumer : INotifyPropertyChanged
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
                    OnPropertyChanged("Device");
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
                    OnPropertyChanged("Aspect");
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
                    OnPropertyChanged("Stretch");
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
                    OnPropertyChanged("Windowed");
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
                    OnPropertyChanged("KeyOnly");
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
                    OnPropertyChanged("Vsync");
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
                    OnPropertyChanged("Borderless");
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
                    OnPropertyChanged("Interactive");
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
                    OnPropertyChanged("AlwaysOn");
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
                    OnPropertyChanged("X");
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
                    OnPropertyChanged("Y");
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
                    OnPropertyChanged("Width");
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
                    OnPropertyChanged("Height");
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
                    OnPropertyChanged("SbsKey");
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
                    OnPropertyChanged("ColourSpace");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class SystemAudioConsumer : INotifyPropertyChanged
    {
        private SystemAudioChannelLayout _channelLayout = SystemAudioChannelLayout._stereo;
        public SystemAudioChannelLayout ChannelLayout
        {
            get
            {
                return _channelLayout;
            }
            set
            {
                if (_channelLayout != value)
                {
                    _channelLayout = value;
                    OnPropertyChanged("ChannelLayout");
                }
            }
        }

        private int _latency = 200;
        public int Latency
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
                    OnPropertyChanged("Latency");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class NewtekIvgaConsumer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class NdiConsumer : INotifyPropertyChanged
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
                    OnPropertyChanged("Name");
                }
            }
        }

        private bool _allowFields = false;
        public bool AllowFields
        {
            get
            {
                return _allowFields;
            }
            set
            {
                if (_allowFields != value)
                {
                    _allowFields = value;
                    OnPropertyChanged("AllowFields");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class FfmpegConsumer : INotifyPropertyChanged
    {
        private string _path = "";
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged("Path");
                }
            }
        }

        private string _arguments = "";
        public string Arguments
        {
            get
            {
                return _arguments;
            }
            set
            {
                if (_arguments != value)
                {
                    _arguments = value;
                    OnPropertyChanged("Arguments");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public class OscClient : INotifyPropertyChanged
    {
        private int _port = 5253;
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
                    OnPropertyChanged("Port");
                }
            }
        }

        private string _address = "127.0.0.1";
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged("Address");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
    }

    public enum LogLevel
    {
        [Description("Trace")]
        _trace,

        [Description("Debug")]
        _debug,

        [Description("Info")]
        _info,

        [Description("Warning")]
        _warning,

        [Description("Error")]
        _error,

        [Description("Fatal")]
        _fatal
    }

    public enum FfmpegDeinterlace
    {
        [Description("None")]
        _none,

        [Description("Interlaced")]
        _interlaced,

        [Description("All")]
        _all
    }

    public enum VideoMode
    {
        [Description("PAL")]
        _PAL,

        [Description("NTSC")]
        _NTSC,

        [Description("576p25")]
        _576p2500,

        [Description("720p23.98")]
        _720p2398,

        [Description("720p24")]
        _720p2400,

        [Description("720p25")]
        _720p2500,

        [Description("720p50")]
        _720p5000,

        [Description("720p29.97")]
        _720p2997,

        [Description("720p59.94")]
        _720p5994,

        [Description("720p30")]
        _720p3000,

        [Description("720p60")]
        _720p6000,

        [Description("1080p23.98")]
        _1080p2398,

        [Description("1080p24")]
        _1080p2400,

        [Description("1080i50")]
        _1080i5000,

        [Description("1080i59.94")]
        _1080i5994,

        [Description("1080i60")]
        _1080i6000,

        [Description("1080p25")]
        _1080p2500,

        [Description("1080p29.97")]
        _1080p2997,

        [Description("1080p30")]
        _1080p3000,

        [Description("1080p50")]
        _1080p5000,

        [Description("1080p59.94")]
        _1080p5994,

        [Description("1080p60")]
        _1080p6000,
        [Description("1556p23.98")]
        _1556p2398,

        [Description("1556p24")]
        _1556p2400,

        [Description("1556p25")]
        _1556p2500,

        [Description("DCI 1080p23.98")]
        _dci1080p2398,

        [Description("DCI 1080p24")]
        _dci1080p2400,

        [Description("DCI 1080p25")]
        _dci1080p2500,

        [Description("2160p23.98")]
        _2160p2398,

        [Description("2160p24")]
        _2160p2400,

        [Description("2160p25")]
        _2160p2500,

        [Description("2160p29.97")]
        _2160p2997,

        [Description("2160p30")]
        _2160p3000,

        [Description("2160p50")]
        _2160p5000,

        [Description("2160p59.94")]
        _2160p5994,

        [Description("2160p60")]
        _2160p6000,

        [Description("DCI 2160p23.98")]
        _dci2160p2398,

        [Description("DCI 2160p24")]
        _dci2160p2400,

        [Description("DCI 2160p25")]
        _dci2160p2500
    }

    public enum DecklinkLatency
    {
        [Description("Normal")]
        _normal,

        [Description("Low")]
        _low,

        [Description("Default")]
        _default
    }

    public enum DecklinkKeyer
    {
        [Description("External")]
        _external,

        [Description("External (Separate Device)")]
        _external_separate_device,

        [Description("Internal")]
        _internal,

        [Description("Default")]
        _default
    }

    public enum BluefishKeyer
    {
        [Description("External")]
        _external,

        [Description("Internal")]
        _internal,

        [Description("Disabled")]
        _disabled
    }

    public enum BluefishKeyerAudio
    {
        [Description("Video Output Channel")]
        _videooutputchannel,

        [Description("SDI Video Input")]
        _sdivideoinput
    }

    public enum SystemAudioChannelLayout
    {
        [Description("Mono")]
        _mono,

        [Description("Stereo")]
        _stereo,

        [Description("Matrix")]
        _matrix
    }

    public enum ScreenAspectRatio
    {
        [Description("Default")]
        _default,

        [Description("4:3")]
        _4_3,

        [Description("16:9")]
        _16_9,
    }

    public enum ScreenStretch
    {
        [Description("None")]
        _none,

        [Description("Fill")]
        _fill,

        [Description("Uniform")]
        _uniform,

        [Description("Uniform to Fill")]
        _uniform_to_fill
    }

    public enum ScreenColourSpace
    {
        [Description("RGB")]
        _rgb,

        [Description("Datavideo (Full)")]
        _datavideo_full,

        [Description("Datavideo (Limited)")]
        _datavideo_limited
    }
    
}
