namespace CasparLauncher.CasparCG.Configuration;

public class VideoMode : INotifyPropertyChanged
{
    private string _display = "";
    virtual public string Display
    {
        get
        {
            return _display;
        }
        set
        {
            if (_display != value)
            {
                _display = value;
                OnPropertyChanged(nameof(Display));
            }
        }
    }

    private string _id = "";
    public string Id
    {
        get
        {
            return _id;
        }
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Display));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}

