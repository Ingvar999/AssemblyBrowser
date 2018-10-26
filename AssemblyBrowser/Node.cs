using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AssemblyBrowser
{
    public class Node
    {
        public Node(string content = "")
        {
            Content = content;
            Nodes = new ObservableCollection<Node>();
        }

        public string Content { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
    }
}
