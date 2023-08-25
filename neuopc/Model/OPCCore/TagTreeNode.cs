using System.Collections.Generic;

namespace neuopc
{
    public class TagTreeNode
    {
        public string Name
        {
            get;
            set;
        }

        private List<TagTreeNode> _nodes = new List<TagTreeNode>();

        public List<TagTreeNode> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        public TagTreeNode Parent
        {
            get;
            private set;
        }

        public bool IsLeaf { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TagTreeNode"/> has child nodes.
        /// </summary>
        public bool HasChildNodes => _nodes.Count > 0;

        public string FullPath
        {
            get
            {
                string res = Name;
                TagTreeNode parent = this.Parent;
                while (parent != null)
                {
                    res = string.Format("{0}/{1}", parent.Name, res);
                    parent = parent.Parent;
                }
                return res;
            }
        }


        public string NodeId { get; set; }        

        public TagTreeNode()
        { }

        public TagTreeNode(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Adds a new child node with the specified name to the current node.
        /// </summary>
        /// <param name="name">The name of the new child node.</param>
        /// <returns>The newly created child node.</returns>
        public TagTreeNode AddNode(string name)
        {
            TagTreeNode node = new TagTreeNode(name)
            {
                Parent = this
            };
            this._nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Finds a node with the specified name in the current node's child nodes.
        /// </summary>
        /// <param name="name">The name of the node to find.</param>
        /// <returns>The node with the specified name, or null if the node is not found.</returns>
        public TagTreeNode FindNode(string name)
        {
            foreach (TagTreeNode node in this._nodes)
            {
                if (node.Name == name)
                {
                    return node;
                }

                // If the node has child nodes, recursively search those nodes
                if (node.HasChildNodes)
                {
                    TagTreeNode found = node.FindNode(name);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a node in the tree by its path.
        /// </summary>
        /// <param name="path">The path of the node to find.</param>
        /// <returns>The node with the specified path, or null if it was not found.</returns>
        public TagTreeNode FindNodeByPath(string path)
        {
            foreach (TagTreeNode node in this._nodes)
            {
                if (node.FullPath == path)
                {
                    return node;
                }

                // If the node has child nodes, recursively search those nodes
                if (node.HasChildNodes)
                {
                    TagTreeNode found = node.FindNodeByPath(path);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
            return null;
        }
    }
}
