using System;
using System.Collections.Generic;
using System.Linq;
using Opc.Da;
using neulib;

namespace neuclient
{
    public class DaBrowse
    {
        private static IEnumerable<Node> AllNode(Server server, Opc.ItemIdentifier id = null, List<Node> nodes = null)
        {
            if (null == nodes)
            {
                nodes = new List<Node>();
            }

            BrowseElement[] elements;
            var filters = new BrowseFilters
            {
                BrowseFilter = browseFilter.all
            };

            try
            {
                elements = server.Browse(id, filters, out BrowsePosition position);
            }
            catch (Exception)
            {
                throw;
            }

            if (null != elements)
            {
                var list = elements.Select(x => new Node
                {
                    Name = x.Name,
                    ItemName = x.ItemName,
                    ItemPath = x.ItemPath,
                    IsItem = x.IsItem
                }).ToList();
                nodes.AddRange(list);

                foreach (var element in elements)
                {
                    if (element.HasChildren)
                    {
                        id = new Opc.ItemIdentifier(element.ItemPath, element.ItemName);
                        AllNode(server, id, nodes);
                    }
                }
            }

            return nodes;
        }

        /// <summary>
        /// Retrieves all nodes from the specified server that match the given list of tags.
        /// </summary>
        /// <param name="server">The OPC server to browse.</param>
        /// <param name="nodesToGet">The list of tags to retrieve nodes for.</param>
        /// <returns>An IEnumerable of Node objects representing the nodes that match the given tags.</returns>
        public static IEnumerable<Node> AllNode(Server server, List<Tag> nodesToGet)
        {
            var nodes = new List<Node>();
            BrowseElement[] elements;
            var filters = new BrowseFilters
            {
                BrowseFilter = browseFilter.all
            };

            try
            {
                foreach (var node in nodesToGet)
                {
                    var id = new Opc.ItemIdentifier(node.Path, node.Id);
                    elements = server.Browse(id, filters, out BrowsePosition position);
                    if (null != elements)
                    {
                        var nodeToReturn = elements.Select(x => new Node
                        {
                            Name = x.Name,
                            ItemName = x.ItemName,
                            ItemPath = x.ItemPath,
                            IsItem = x.IsItem,
                            HasChildren = x.HasChildren
                        }).FirstOrDefault();
                        if (nodeToReturn != null)
                        {
                            nodes.Add(nodeToReturn);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return nodes;
        }

        public static IEnumerable<Node> RootNodes(Server server)
        {
            var nodes = new List<Node>();

            BrowseElement[] elements;
            var filters = new BrowseFilters
            {
                BrowseFilter = browseFilter.branch
            };

            try
            {
                elements = server.Browse(null, filters, out BrowsePosition position);
            }
            catch (Exception)
            {
                throw;
            }

            if (null != elements)
            {
                nodes.AddRange(elements.Select(x => new Node
                {
                    Name = x.Name,
                    ItemName = x.ItemName,
                    ItemPath = x.ItemPath,
                    IsItem = x.IsItem,
                    HasChildren = x.HasChildren
                }));
            }

            return nodes;
        }

        public static IEnumerable<Node> ChildNodes(Server server, Opc.ItemIdentifier id)
        {
            var nodes = new List<Node>();

            BrowseElement[] elements;
            var filters = new BrowseFilters
            {
                BrowseFilter = browseFilter.all
            };

            try
            {
                elements = server.Browse(id, filters, out BrowsePosition position);
            }
            catch (Exception)
            {
                throw;
            }

            if (null != elements)
            {
                nodes.AddRange(elements.Select(x => new Node
                {
                    Name = x.Name,
                    ItemName = x.ItemName,
                    ItemPath = x.ItemPath,
                    IsItem = x.IsItem,
                    HasChildren = x.HasChildren
                }));
            }

            return nodes;
        }

        public static Type GetDataType(Server server, string tag, string path)
        {
            var item = new Opc.Da.Item { ItemName = tag, ItemPath = path };
            ItemProperty result;

            try
            {
                var propertyCollection = server.GetProperties(new Opc.ItemIdentifier[] { item }, new[] { new PropertyID(1) }, true)[0];
                result = propertyCollection[0];
            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.WriteLine($"GetProperties Exception {tag}");
                return null;
            }

            return (Type)result.Value;
        }


        public static IEnumerable<Node> AllItemNode(Server server, List<Tag> nodesToGet = null)
        {
            IEnumerable<Node> nodes = null;
            if (null != nodesToGet || nodesToGet.Count > 0)            
            {
                nodes = nodesToGet.Select(x => new Node
                {
                    Name = x.Name,
                    ItemName = x.Id,
                    ItemPath = x.Path,
                    IsItem = true,
                    HasChildren = false,
                    Type = GetDataType(server, x.Id, x.Path),
                });
                return nodes;
            }

            nodes = AllNode(server);
            var items = nodes.Where(x => x.IsItem);
            foreach (var item in items)
            {
                item.Type = GetDataType(server, item.ItemName, item.ItemPath);
            }

            return items;
        }


        public static IEnumerable<Node> AllItemRootNode(Server server)
        {
            var nodes = RootNodes(server);
            var items = nodes.Where(x => !x.IsItem);
            foreach (var item in items)
            {
                item.Type = GetDataType(server, item.ItemName, item.ItemPath);
            }
            return items;
        }

        public static IEnumerable<Node> AllItemChildNode(Server server, Node node)
        {
            var id = new Opc.ItemIdentifier(node.ItemPath, node.ItemName);
            var nodes = ChildNodes(server, id);
            if (null == nodes)
            {
                // Try again with only with itemname
                id = new Opc.ItemIdentifier(node.ItemName);
                nodes = ChildNodes(server, id);
            }
            var items = nodes.Where(x => x.IsItem || x.HasChildren);
            foreach (var item in items)
            {
                item.Type = GetDataType(server, item.ItemName, item.ItemPath);
            }
            return items;
        }
    }
}
