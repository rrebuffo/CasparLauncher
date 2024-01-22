namespace CasparLauncher.CasparCG.Configuration.Consumers;

public class NdiConsumer : Consumer
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
                OnPropertyChanged(nameof(Name));
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
                OnPropertyChanged(nameof(AllowFields));
            }
        }
    }
}