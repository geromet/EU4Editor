using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal record Node
    {
        public Node()
        {

        }
        public Node(string name)
        {
            Name = name;
        }
        public Node(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public Node(string name, string value, Stack<Node> nodes) : this(name, value)
        {
            Name = name;
            Value = value;
            Nodes = nodes;
        }
        public bool InnerNodes { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Stack<Node> Nodes { get; set; } = new();

    }
}
