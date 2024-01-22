namespace CasparLauncher.CasparCG.Configuration.Consumers;

public class SystemAudioConsumer : Consumer
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
                OnPropertyChanged(nameof(ChannelLayout));
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
                OnPropertyChanged(nameof(Latency));
            }
        }
    }
}
