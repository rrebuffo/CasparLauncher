namespace CasparLauncher.CasparCG.Configuration.Consumers;

public abstract class Consumer : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
}