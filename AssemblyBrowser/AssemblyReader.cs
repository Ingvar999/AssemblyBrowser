using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Reflection;
using System.Collections.ObjectModel;

namespace AssemblyBrowser
{
    public class AssemblyReader : INotifyPropertyChanged
    {
        public OpenCommand OpenCommand { get; private set; }
        private ObservableCollection<Node> root;

        public AssemblyReader(ObservableCollection<Node> root)
        {
            this.root = root;
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
                root.Add(GetAssemblyInfo(dialog.FileName));
                OnPropertyChanged();
            }
        }

        private Node GetAssemblyInfo(string fileName)
        {
            Assembly assembly = Assembly.LoadFrom(fileName);
            Node info = new Node(assembly.FullName);
            foreach (var type in assembly.GetTypes())
            {
                GetTypeInfo(SearchNamespaceEntry(info.Nodes, type.ToString()), type);
            }
            return info;
        }

        private ObservableCollection<Node> SearchNamespaceEntry(ObservableCollection<Node> tree, string typeName)
        {
            int dotIndex = typeName.IndexOf('.');
            if (dotIndex == -1)
            {
                return tree;
            }
            Node match = null;
            string namespaceName = typeName.Substring(0, dotIndex);
            foreach (var node in tree)
            {
                if (node.Content == namespaceName)
                {
                    match = node;
                    break;
                }
            }
            if (match == null)
            {
                match = new Node(namespaceName);
                tree.Add(match);
            }
            return SearchNamespaceEntry(match.Nodes, typeName.Substring(dotIndex + 1));
        }

        private void GetTypeInfo(ObservableCollection<Node> tree, Type type)
        {
            Node typeInfo = new Node(type.Name);
            tree.Add(typeInfo);
            FieldInfo[] fields = type.GetFields();
            if (fields.Length > 0)
            {
                Node fieldsInfo = new Node("Fields");
                typeInfo.Nodes.Add(fieldsInfo);
                foreach(var field in fields)
                {
                    Node fieldInfo = new Node(field.ToString());
                    fieldsInfo.Nodes.Add(fieldInfo);
                }
            }
        }
    }
}
