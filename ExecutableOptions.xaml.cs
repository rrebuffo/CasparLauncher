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
using L = CasparLauncher.Properties.Resources;

namespace CasparLauncher
{
    /// <summary>
    /// Lógica de interacción para ConfigEditor.xaml
    /// </summary>
    public partial class ExecutableOptions : Window
    {

        public ExecutableOptions()
        {
            InitializeComponent();
        }

        private Executable Executable
        {
            get
            {
                return DataContext as Executable;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (Owner != null)
            {
                Owner.Activate();
            }
        }

        private void AddCommand(object sender, RoutedEventArgs e)
        {
            
        }

        private void RemoveCommand(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeLocation_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = false,
                Filter = L.FileDialogFilterDescription + " (*.EXE)|*.exe"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Executable.Path = openFileDialog.FileName;
            }
            else
            {
                return;
            }
        }
    }
}
