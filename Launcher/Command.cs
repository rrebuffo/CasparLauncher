namespace CasparLauncher.Launcher;

public class Command : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

    public Command()
    {

    }

    private string _value = "";
    public string Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (_value != value)
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
    }
}
