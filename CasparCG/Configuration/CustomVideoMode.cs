namespace CasparLauncher.CasparCG.Configuration;

public class CustomVideoMode : VideoMode
{
    public CustomVideoMode()
    {

    }

    public override string Display => Id;

    private int _width = 1;
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

    private int _height = 1;
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

    private int _timeScale = 60000;
    public int TimeScale
    {
        get
        {
            return _timeScale;
        }
        set
        {
            if (_timeScale != value)
            {
                _timeScale = value;
                OnPropertyChanged(nameof(TimeScale));
            }
        }
    }

    private int _duration = 1000;
    public int Duration
    {
        get
        {
            return _duration;
        }
        set
        {
            if (_duration != value)
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }
    }

    private int _cadence = 0;
    public int Cadence
    {
        get
        {
            return _cadence;
        }
        set
        {
            if (_cadence != value)
            {
                _cadence = value;
                OnPropertyChanged(nameof(Cadence));
            }
        }
    }

}
