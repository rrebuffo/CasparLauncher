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

namespace CasparLauncher
{
    /// <summary>
    /// Lógica de interacción para ConfigEditor.xaml
    /// </summary>
    public partial class ConfigEditor : Window
    {
        public ConfigEditor()
        {
            InitializeComponent();
        }

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

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            ConfigFile file = DataContext as ConfigFile;

            if (file.File == null)
            {
                var filename = ShowSaveDialog();
                if (filename != null) file.File = filename;
                else return;
            }
            else
            {
                file.SaveConfigFile(file.File);
            }
        }

        private string ShowSaveDialog()
        {
            ConfigFile file = DataContext as ConfigFile;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo de configuración |*.config";
            if (saveFileDialog.ShowDialog() == true)
            {
                file.SaveConfigFile(saveFileDialog.FileName);
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
    }
}
