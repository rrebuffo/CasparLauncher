using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WF = System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.IO;
using l = CasparLauncher.Properties.Resources;
using System.ComponentModel;

namespace CasparLauncher
{
    /// <summary>
    /// Lógica de interacción para ConfigEditor.xaml
    /// </summary>
    public partial class ConfigEditor : Window, INotifyPropertyChanged
    {
        public ConfigEditor()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (null != Owner)
            {
                Owner.Activate();
            }
        }

        private void AddChannel(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;
            file.Channels.Add(new Channel());
        }

        private void RemChannel(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;

            if (ChannelList.SelectedIndex>=0)
            {
                file.Channels.Remove((Channel)ChannelList.SelectedItem);
            }
        }

        private void AddConsumer(object sender, RoutedEventArgs e)
        {
            object consumer;
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
                default:
                    return;
            }
            if(ChannelList.SelectedIndex>=0)
            {
                ((Channel)ChannelList.SelectedItem).Consumers.Add(consumer);
            }
        }

        private void RemConsumer(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;

            if (ChannelList.SelectedIndex >=0 && ConsumerList.SelectedIndex >= 0)
            {
                file.Channels[ChannelList.SelectedIndex].Consumers.Remove(ConsumerList.SelectedItem);
            }
        }

        #region Status Message

        DispatcherTimer ShowStatusTimer = new DispatcherTimer();
        DoubleAnimation fade = new DoubleAnimation();
        Storyboard status = new Storyboard();

        private void fadeInStatus()
        {
            //FadeIn
            fade.From = 0.0;
            fade.To = 1.0;
            fade.Duration = TimeSpan.FromMilliseconds(500);
            fade.AutoReverse = false;
            status.Children.Add(fade);
            Storyboard.SetTargetName(fade, StatusText.Name);
            Storyboard.SetTargetProperty(fade, new PropertyPath(TextBlock.OpacityProperty));
            status.Begin(this);
        }

        private void fadeOutStatus()
        {
            //FadeIn
            fade.From = 1.0;
            fade.To = 0.0;
            fade.Duration = TimeSpan.FromMilliseconds(500);
            fade.AutoReverse = false;
            status.Children.Add(fade);
            status.Completed += StatusFaded;
            Storyboard.SetTargetName(fade, StatusText.Name);
            Storyboard.SetTargetProperty(fade, new PropertyPath(TextBlock.OpacityProperty));
            status.Begin(this);
        }

        private void StatusFaded(object sender, EventArgs e)
        {
            status.Completed -= StatusFaded;
            StatusText.Text = "";
        }

        private void ShowStatusMessage(string message)
        {
            StatusText.Text = message;
            fadeInStatus();
            ShowStatusTimer.Interval = TimeSpan.FromMilliseconds(5000);
            ShowStatusTimer.Tick += RemoveStatusMessage;
            ShowStatusTimer.Start();
        }

        private void RemoveStatusMessage(object sender, EventArgs e)
        {
            ShowStatusTimer.Stop();
            fadeOutStatus();
        }

        #endregion


        private void SaveFile(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;

            if (file.File == null)
            {
                var filename = ShowSaveDialog();
                if (filename != null) file.File = filename;
                else return;
            }

            try
            {
                file.SaveConfigFile(file.File);
            }
            catch(IOException)
            {
                ShowStatusMessage(l.ConfigWindowStatusMessageSaveIOError); // IO error
                return;
            }
            catch(Exception ex)
            {
                ShowStatusMessage($"{l.ConfigWindowStatusMessageSaveError} ({ex.GetType()})"); // Unknown error
                return;
            }

            ShowStatusMessage(l.ConfigWindowStatusMessageSaveSuccess); // Success
        }

        private string ShowSaveDialog()
        {
            ConfigFile file = DataContext as ConfigFile;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo de configuración |*.config";
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        private string SelectFolder(string path = null, bool newFolder = false)
        {
            WF.FolderBrowserDialog Browser = new WF.FolderBrowserDialog();
            Browser.ShowNewFolderButton = newFolder;
            if (path != null && System.IO.Directory.Exists(path)) Browser.SelectedPath = path;
            else Browser.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            WF.DialogResult Result = Browser.ShowDialog();
            if (Result == WF.DialogResult.OK)
            {
                string folder = Browser.SelectedPath;
                if (System.IO.Directory.Exists(folder))
                {
                    return folder;
                }
            }
            return null;
        }

        private void PickMediaPathButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;
            string newFolder = SelectFolder(file.MediaPath, true);
            if (newFolder != null) file.MediaPath = newFolder;
        }

        private void PickDataPathButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;
            string newFolder = SelectFolder(file.DataPath, true);
            if (newFolder != null) file.DataPath = newFolder;
        }

        private void PickTemplatePathButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;
            string newFolder = SelectFolder(file.TemplatePath, true);
            if (newFolder != null) file.TemplatePath = newFolder;
        }

        private void PickFontPathButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;
            string newFolder = SelectFolder(file.FontPath, true);
            if (newFolder != null) file.FontPath = newFolder;
        }

        private void PickLogPathButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;
            string newFolder = SelectFolder(file.LogPath, true);
            if (newFolder != null) file.LogPath = newFolder;
        }

        #region Drag & Drop

        private void TopDropBorder_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            ((Border)sender).Opacity = 0;
            MoveChannel(DraggedItem.DataContext as Channel, ((Border)sender).DataContext as Channel, false);
        }

        private void BotDropBorder_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            ((Border)sender).Opacity = 0;
            MoveChannel(DraggedItem.DataContext as Channel, ((Border)sender).DataContext as Channel, true);
        }

        private void MoveChannel(Channel origin, Channel target, bool bottom)
        {
            Drag = false;
            if (origin == target) return;
            ConfigFile file = DataContext as ConfigFile;
            file.Channels.Remove(origin);
            file.Channels.Insert(file.Channels.IndexOf(target) + (bottom?1:0), origin);
            ChannelsUpdated = true;
            DraggedItem = null;
        }

        private Point StartPoint;

        private bool drag = false;
        public bool Drag
        {
            get
            {
                return drag;
            }
            set
            {
                if (drag != value)
                {
                    drag = value;
                    OnPropertyChanged("Drag");
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
                OnPropertyChanged("ChannelsUpdated");
            }
        }

        private ListBoxItem DraggedItem;

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                DraggedItem = sender as ListBoxItem;
                this.MouseMove += HandleDrag;
                StartPoint = e.GetPosition(this);
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DraggedItem = null;
            MouseMove -= HandleDrag;
            Drag = false;
        }

        private void HandleMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                (sender as ListBox).ReleaseMouseCapture();
        }

        private void HandleDrag(object sender, MouseEventArgs e)
        {
            if (DraggedItem is null)
            {
                ((Border)sender).Opacity = 0;
                Drag = false;
                MouseMove -= HandleDrag;
                return;
            }
            if (Math.Abs(e.GetPosition(this).X - StartPoint.X) > Settings.DragThreshold || Math.Abs(e.GetPosition(this).Y - StartPoint.Y) > Settings.DragThreshold)
            {
                Drag = true;
                DragDrop.DoDragDrop(DraggedItem, DraggedItem.DataContext, DragDropEffects.Move);
                MouseMove -= HandleDrag;
            }
        }

        private void Border_DragEnter(object sender, DragEventArgs e)
        {
            ((Border)sender).Opacity = 1;
        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            ((Border)sender).Opacity = 0;
        }

        #endregion

    }
}
