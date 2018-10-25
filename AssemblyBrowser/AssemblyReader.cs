using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.IO;

namespace AssemblyBrowser
{
    public class AssemblyReader : INotifyPropertyChanged
    {
        public OpenCommand OpenCommand { get; private set; }
        private IEnumerable sourceView;

        public AssemblyReader(TreeView sourceView)
        {
            this.sourceView = sourceView;
            OpenCommand = new OpenCommand(OpenHandler);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void OpenHandler(object param)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "(*.dll)|*.dll";
            if (dialog.ShowDialog() == true)
            {
                Node assemblyInfo = GetAssemblyInfo(dialog.FileName);
                sourceView = (IEnumerable)assemblyInfo;
                OnPropertyChanged();
            }
        }

        private Node GetAssemblyInfo(string fileName)
        {
            Node info = new Node();
            info.Content = fileName;
            return info;
        }
    }
}
