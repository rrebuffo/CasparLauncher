namespace CasparLauncher.CasparCG.Configuration;

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
                OnPropertyChanged(nameof(Port));
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
                OnPropertyChanged(nameof(Address));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}