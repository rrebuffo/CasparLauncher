using System.Text;

namespace CasparLauncher;

public partial class ExecutableOptions : DialogWindow
{
    public ObservableCollection<string> ConfigFiles { get; set; } = [];

    public string CustomPath
    {
        get
        {
            return Executable.ConfigFile ?? "";
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                Executable.ConfigFile = value;
            }
        }
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CustomPath));
        Keyboard.ClearFocus();
    }

    public ExecutableOptions()
    {
        InitializeComponent();
        DataContextChanged += ExecutableOptions_DataContextChanged;
        if (DataContext is Executable executable)
        {
            FindConfigFiles();
            executable.PropertyChanged += PathChanged;
        }
    }

    private void PathChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Executable.Path))
        {
            FindConfigFiles();
        }
    }

    private void ExecutableOptions_DataContextChanged(object? sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is Executable old)
        {
            old.PropertyChanged -= PathChanged;
        }
        if (e.NewValue is Executable current)
        {
            current.PropertyChanged += PathChanged;
        }
        FindConfigFiles();
    }

    private Executable Executable
    {
        get
        {
            return DataContext as Executable;
        }
    }

    private void FindConfigFiles()
    {
        try
        {
            if (Executable is null) return;
            ConfigFiles.Clear();
            var path = Executable.Path;
            if (string.IsNullOrEmpty(path)) path = Environment.CurrentDirectory;
            if (!Path.IsPathRooted(path)) path = Path.Combine(Environment.CurrentDirectory, path);
            if (Path.GetDirectoryName(path) is string folder)
            {
                IEnumerable<string> files = Directory.GetFiles(folder)
                    .Select(f => Path.GetFileName(f).ToLower())
                    .Where(c => c.EndsWith(".config"));
                foreach (string file in files) ConfigFiles.Add(file);

                if (DataContext is Executable executable && (executable.IsServer || executable.IsScanner))
                {
                    if (string.IsNullOrEmpty(executable.ConfigFile)
                        && ConfigFiles.Contains("casparcg.config"))
                    {
                        executable.ConfigFile = "casparcg.config";
                    }
                    if (!ConfigFiles.Contains(executable.ConfigFile))
                    {
                        ConfigFiles.Add(executable.ConfigFile);
                    }
                }
            }
        }
        catch { }
    }

    private string? ShowSaveDialog(string filter, string? folder = null)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = filter,
            InitialDirectory = folder
        };
        return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
    }

    private string? ShowOpenDialog(string filter, string? folder = null)
    {
        OpenFileDialog openFileDialog = new()
        {
            Multiselect = false,
            Filter = filter,
            InitialDirectory = folder
        };
        return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
    }

    private void EditConfig(string path)
    {
        ConfigFile file = new();

        if (!string.IsNullOrEmpty(path) && File.Exists(path)) file.File = path;
        else file.File = ShowOpenDialog(L.ConfigFileDialogFilterDescription + "|*.config");
        if (file.File is null) return;

        try
        {
            file.LoadConfigFile();
        }
        catch (IOException)
        {
            MessageBox.Show(L.OpenConfigEditorIOError, L.OpenConfigEditorErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        catch (Exception)
        {
            MessageBox.Show(L.OpenConfigEditorFileError, L.OpenConfigEditorErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        ConfigEditor configWindow = new();
        configWindow.Owner = this;
        configWindow.DataContext = file;
        configWindow.ShowDialog();
    }

    private void NewConfig(bool clone = false)
    {
        if (DataContext is Executable executable)
        {
            string? folder = Path.GetDirectoryName(executable.Path);
            if (folder is not null && ShowSaveDialog(L.ConfigFileDialogFilterDescription + "|*.config", folder) is string filename)
            {
                try
                {
                    ConfigFile file = new();
                    if (clone)
                    {
                        try
                        {
                            file.File = Path.Combine(folder,executable.ConfigFile);
                            file.LoadConfigFile();
                            file.File = filename;
                        }
                        catch (IOException)
                        {
                            MessageBox.Show(L.OpenConfigEditorIOError, L.OpenConfigEditorErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(L.OpenConfigEditorFileError, L.OpenConfigEditorErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    file.SaveConfigFile(filename);
                }
                catch (IOException)
                {
                    MessageBox.Show(L.CreateConfigFileIOError, L.CreateConfigFileErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (File.Exists(filename))
                {
                    EditConfig(filename);
                    FindConfigFiles();
                    executable.ConfigFile = Path.GetFileName(filename);
                }
            }
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        if (Owner != null)
        {
            Owner.Activate();
        }
    }

    private void ChangeLocation_Click(object? sender, RoutedEventArgs e)
    {
        if (Executable is null) return;
        
        if (ShowOpenDialog(L.FileDialogFilterDescription + " (*.EXE)|*.exe") is string path)
        {
            Executable.Path = path;
        }
        else
        {
            return;
        }
    }

    private void Copy(object? sender, ExecutedRoutedEventArgs e)
    {
        if (sender is DataGrid commands_dg)
        {
            StringBuilder commandList = new();
            var first = false;
            foreach (Command c in commands_dg.SelectedItems)
            {
                if (!first) first = true;
                else commandList.Append('\n');
                commandList.Append(c.Value);
            }
            Clipboard.SetData(DataFormats.UnicodeText, commandList.ToString());
        }
    }

    private void CanCopy(object? sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void CanPaste(object? sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
        e.Handled = true;
    }

    private void Paste(object? sender, ExecutedRoutedEventArgs e)
    {
        try
        {
            List<string> values = [];
            var data = "";
            if (Clipboard.ContainsData(DataFormats.CommaSeparatedValue) || Clipboard.ContainsData(DataFormats.UnicodeText))
            {
                if (Clipboard.ContainsData(DataFormats.CommaSeparatedValue)) data = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                else if (Clipboard.ContainsData(DataFormats.UnicodeText)) data = (string)Clipboard.GetData(DataFormats.UnicodeText);
                else return;
                data = data.Replace("\r\n", "\n");
                values.AddRange(data.Split('\n'));
            }
            foreach (string value in values) if (value != "") Executable?.Commands.Add(new Command() { Value = value });
        }
        catch { }
    }

    private void ExportCommands_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*",
            FileName = Environment.MachineName + "_CasparLauncher_Commands.txt",
            Title = L.ExportCommandsDialogTitle
        };
        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                File.WriteAllLines(
                    saveFileDialog.FileName,
                    Executable.Commands.Select(x => x.Value).ToArray()
                );
            }
            catch { }
        }
    }
    private void ImportCommands_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Title = L.ImportCommandsDialogTitle,
            Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
            FilterIndex = 2,
            RestoreDirectory = true
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string filePath = openFileDialog.FileName;
            if (string.IsNullOrEmpty(filePath)) return;
            IEnumerable<string> lines = [];
            try
            {
                lines = File.ReadLines(filePath);
            }
            catch { return; }
            Executable.Commands.Clear();
            foreach (string line in lines)
            {
                Debug.WriteLine(line);
                Executable.Commands.Add(new()
                {
                    Value = line
                });
            }
        }
    }

    private void EditConfigButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Executable executable)
        {
            string? folder = Path.GetDirectoryName(executable.Path);
            string? file = executable.ConfigFile;
            if (folder is not null && file is not null)
            {
                string? path = Path.Combine(folder, file);
                EditConfig(path);
            }
        }
    }

    private void RemoveConfigButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Executable executable)
        {
            string? folder = Path.GetDirectoryName(executable.Path);
            string? file = executable.ConfigFile;
            if (folder is not null && file is not null)
            {
                string? path = Path.Combine(folder, file);
                if (File.Exists(path))
                {
                    MessageBoxResult result = MessageBox.Show(string.Format(L.ExecutableConfigWindowDeleteFilePromptMessage,file), L.ExecutableConfigWindowDeleteFilePromptCaption, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            File.Delete(path);
                        }
                        catch (IOException)
                        {
                            MessageBox.Show(L.DeleteConfigFileIOError, L.DeleteConfigFileErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(L.DeleteConfigFileError, L.DeleteConfigFileErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        FindConfigFiles();
                    }
                }
            }
            else
            {
                FindConfigFiles();
            }
        }
    }

    private void NewConfigButton_Click(object sender, RoutedEventArgs e)
    {
        NewConfig();
    }

    private void CloneConfigButton_Click(object sender, RoutedEventArgs e)
    {
        NewConfig(true);
    }

    private void RemoveExecutable_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is Executable ex)
        {
            bool deleted = App.Launchpad.RemoveExecutable(ex);
            if (deleted) Close();
        }
    }
}
