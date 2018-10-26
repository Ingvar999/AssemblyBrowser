using System;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections.ObjectModel;
using AssemblyInfoGetterLib;

namespace AssemblyBrowser
{
    public class AssemblyReader : INotifyPropertyChanged
    {
        public OpenCommand OpenCommand { get; private set; }
        private ObservableCollection<Node> root;
        private IInfoGetter assemblyInfoGetter;

        public AssemblyReader(ObservableCollection<Node> root, IInfoGetter infoGetter)
        {
            this.root = root;
            assemblyInfoGetter = infoGetter;
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
                root.Clear();
                root.Add(assemblyInfoGetter.GetFileInfo(dialog.FileName));
                OnPropertyChanged();
            }
        }
    }
}
