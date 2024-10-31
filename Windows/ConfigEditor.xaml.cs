namespace CasparLauncher;

public partial class ConfigEditor : DialogWindow
{
    public ConfigEditor()
    {
        InitializeComponent();
        DataContextChanged += ConfigEditor_DataContextChanged;
    }

    private void ConfigEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is null && e.NewValue is ConfigFile file)
        {
            file.Channels.CollectionChanged -= Channels_CollectionChanged;
            file.Channels.CollectionChanged += Channels_CollectionChanged;
            Debug.WriteLine("Initialized");
        }
    }

    private void Channels_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(ChannelsUpdated));
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        if (null != Owner)
        {
            Owner.Activate();
        }
    }

    private void AddVideoMode(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        file.VideoModes.Add(new CustomVideoMode() { Id = L.ConfigWindowVideoModesNewModeId });
    }

    private void RemVideoMode(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        if (VideoModesList.SelectedIndex >= 0)
        {
            CustomVideoMode target = (CustomVideoMode)VideoModesList.SelectedItem;

            foreach (Channel channel in file.Channels)
                if (channel.VideoMode == target)
                    channel.VideoMode = ConfigFile.DefaultVideoModes.First();

            file.VideoModes.Remove(target);
        }
    }

    private void AddChannel(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        file.Channels.Add(new Channel() { VideoMode = ConfigFile.DefaultVideoModes.First() });
    }

    private void RemChannel(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        if (ChannelList.SelectedIndex >= 0)
        {
            file.Channels.Remove((Channel)ChannelList.SelectedItem);
        }
    }

    private void AddConsumer(object sender, RoutedEventArgs e)
    {
        Consumer consumer;
        switch (((FrameworkElement)e.OriginalSource).Name)
        {
            case "AddDecklinkMenuItem":
                consumer = new DecklinkConsumer();
                break;
            case "AddBluefishMenuItem":
                consumer = new BluefishConsumer();
                break;
            case "AddScreenMenuItem":
                consumer = new ScreenConsumer();
                break;
            case "AddSystemAudioMenuItem":
                consumer = new SystemAudioConsumer();
                break;
            case "AddIvgaMenuItem":
                consumer = new NewtekIvgaConsumer();
                break;
            case "AddNdiMenuItem":
                consumer = new NdiConsumer();
                break;
            case "AddFfmpegMenuItem":
                consumer = new FfmpegConsumer();
                break;
            case "AddArtnetMenuItem":
                consumer = new ArtnetConsumer();
                break;
            default:
                return;
        }
        if (ChannelList.SelectedIndex >= 0)
        {
            ((Channel)ChannelList.SelectedItem).Consumers.Add(consumer);
        }
    }

    private void RemConsumer(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;

        if (ChannelList.SelectedIndex >= 0 && ConsumerList.SelectedIndex >= 0)
        {
            file.Channels[ChannelList.SelectedIndex].Consumers.Remove(ConsumerList.SelectedItem);
        }
    }


    private void SaveFile(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;

        if (file.File is null)
        {
            if (ShowSaveDialog() is string filename) file.File = filename;
            else return;
        }

        try
        {
            file.SaveConfigFile(file.File);
        }
        catch (IOException)
        {
            MessageBox.Show(L.ConfigWindowStatusMessageSaveIOError, L.OpenConfigEditorErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{L.ConfigWindowStatusMessageSaveError}\n{ex.GetType()}", L.OpenConfigEditorErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        Close();
    }

    private static string? ShowSaveDialog()
    {
        SaveFileDialog saveFileDialog = new() { Filter = "Archivo de configuración |*.config" };
        if (saveFileDialog.ShowDialog() == true) return saveFileDialog.FileName;
        else return null;
    }

    private static string? SelectFolder(string? path = null, string? caption = null, bool newFolder = false)
    {
        WF.FolderBrowserDialog Browser = new() { ShowNewFolderButton = newFolder };
        if (string.IsNullOrEmpty(path)) path = Environment.CurrentDirectory;
        if (!Path.IsPathRooted(path)) path = Path.Combine(Environment.CurrentDirectory, path);
        if (Directory.Exists(path)) Browser.SelectedPath = path;
        else Browser.SelectedPath = "";
        Browser.UseDescriptionForTitle = true;
        if (caption is not null) Browser.Description = caption;
        if (Browser.ShowDialog() == WF.DialogResult.OK)
        {
            string folder = Browser.SelectedPath;
            if (Directory.Exists(folder))
            {
                return folder;
            }
        }
        return null;
    }

    private void PickMediaPathButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        if (SelectFolder(file.MediaPath, string.Format(L.BrowseForPathDialogCaption,"media"), true) is string newFolder) file.MediaPath = newFolder;
    }

    private void PickDataPathButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        if (SelectFolder(file.DataPath, string.Format(L.BrowseForPathDialogCaption, "data"), true) is string newFolder) file.DataPath = newFolder;
    }

    private void PickTemplatePathButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        if (SelectFolder(file.TemplatePath, string.Format(L.BrowseForPathDialogCaption, "template"), true) is string newFolder) file.TemplatePath = newFolder;
    }

    private void PickFontPathButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        if (SelectFolder(file.FontPath, string.Format(L.BrowseForPathDialogCaption, "font"), true) is string newFolder) file.FontPath = newFolder;
    }

    private void PickLogPathButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        if (SelectFolder(file.LogPath, string.Format(L.BrowseForPathDialogCaption, "log"), true) is string newFolder) file.LogPath = newFolder;
    }

    private void PickHtmlCachePathButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ConfigFile file) return;
        if (SelectFolder(file.HtmlCachePath, string.Format(L.BrowseForPathDialogCaption, "HTML cache"), true) is string newFolder) file.HtmlCachePath = newFolder;
    }

    #region Drag & Drop

    private bool move = false;
    public bool Move
    {
        get
        {
            return move;
        }
        set
        {
            if (move != value)
            {
                move = value;
                OnPropertyChanged(nameof(Move));
            }
        }
    }

    private void ChannelItem_Drop(object sender, DragEventArgs e)
    {
        e.Handled = false;
        if (sender is ListBoxItem item
            && item.DataContext is Channel target
            && e.Data.GetData(typeof(List<object>)) is List<object> droppedItems)
        {
            foreach (object dropped in droppedItems)
            {
                if (dropped is Consumer consumer
                    && GetConsumerChannel(consumer) is Channel origin
                    && origin != target)
                {
                    origin.Consumers.Remove(consumer);
                    target.Consumers.Add(consumer);
                    Debug.WriteLine(item);
                }
            }
        }
    }

    private Channel? GetConsumerChannel(Consumer consumer)
    {
        if (DataContext is ConfigFile file)
        {
            return file.Channels.FirstOrDefault(c => c.Consumers.Contains(consumer));
        }
        return null;
    }

    #endregion

    private void AddDecklinkPort(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is DecklinkConsumer decklink)
        {
            decklink.Ports.Add(new());
        }
    }

    private void RemDecklinkPort(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is DecklinkPort port
            && element.FindParent<ItemsControl>() is ItemsControl parent && parent.DataContext is DecklinkConsumer decklink)
        {
            decklink.Ports.Remove(port);
        }
    }

    private void AddArtnetFixture(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is ArtnetConsumer artnet)
        {
            artnet.Fixtures.Add(new());
        }
    }

    private void RemArtnetFixture(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.DataContext is ArtnetFixture fixture
            && element.FindParent<ItemsControl>() is ItemsControl parent && parent.DataContext is ArtnetConsumer artnet)
        {
            artnet.Fixtures.Remove(fixture);
        }
    }

    private object? _selectedPort;
    public object? SelectedPort
    {
        get
        {
            return _selectedPort;
        }
        set
        {
            if (_selectedPort != value)
            {
                _selectedPort = value;
                OnPropertyChanged(nameof(SelectedPort));
            }
        }
    }

    public bool ChannelsUpdated
    {
        get
        {
            return false;
        }
        set
        {
            OnPropertyChanged(nameof(ChannelsUpdated));
        }
    }
}
