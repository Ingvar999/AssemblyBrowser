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
        public string Content { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
    }
}
