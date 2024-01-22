namespace CasparLauncher.CasparCG.Configuration;

public class Producer : INotifyPropertyChanged
{
    public Producer()
    {

    }

    private string _media = "";
    public string Media
    {
        get
        {
            return _media;
        }
        set
        {
            if (_media != value)
            {
                _media = value;
                OnPropertyChanged(nameof(Media));
            }
        }
    }

    private int _layer = 1;
    public int Layer
    {
        get
        {
            return _layer;
        }
        set
        {
            if (_layer != value)
            {
                _layer = value;
                OnPropertyChanged(nameof(Layer));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}
