namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using AppServices.NetCore;
    using Builder;
    using Parsers.ProtoParser.SuperProtoParser;
    using Parsers.XamlNodes;
    using Typing;

    internal class Inflater
    {
        private readonly IEnumerable<Type> inflatables;
        private readonly WiringContext wiringContext;
        private readonly XamlNodeBuilder nodeBuilder;

        public Inflater(IEnumerable<Type> inflatables, WiringContext wiringContext)
        {
            this.inflatables = inflatables;
            this.wiringContext = wiringContext;
            nodeBuilder = new XamlNodeBuilder(wiringContext.TypeContext);
        }

        public IEnumerable<XamlNode> Inflate(IEnumerable<XamlNode> nodes)
        {
            var processedNodes = new Collection<XamlNode>();
            bool skipNext = false;
            foreach (var xamlNode in nodes)
            {
                var inflatable = GetInflatable(xamlNode, inflatables);
                    
                if (inflatable != null)
                {
                    var toAdd = ReadNodes(xamlNode.XamlType.UnderlyingType);
                    var croppedNodes = Crop(toAdd, xamlNode.XamlType, wiringContext.GetType(inflatable));

                    foreach (var croppedNode in croppedNodes)
                    {
                        processedNodes.Add(croppedNode);
                        skipNext = true;
                    }
                }
                else
                {
                    if (skipNext)
                    {
                        skipNext = false;
                    } else
                    {
                        processedNodes.Add(xamlNode);
                    }
                }
            }

            return processedNodes;
        }

        private IEnumerable<XamlNode> Crop(IEnumerable<XamlNode> original, XamlType newType, XamlType oldType)
        {
            var list = original.ToList();
            var nodeToReplace = list.First(node => NodeHasSameType(oldType, node));
            var id = list.IndexOf(nodeToReplace);
            list[id] = nodeBuilder.StartObject(newType.UnderlyingType);
            return list;
        }

        private static bool NodeHasSameType(XamlType oldType, XamlNode node)
        {
            var xamlType = node.XamlType;
            if (xamlType != null)
            {
                var nodeHasSameType = xamlType.Equals(oldType);
                return nodeHasSameType;
            }

            return false;
        }

        private IEnumerable<XamlNode> ReadNodes(Type underlyingType)
        {
            var resourceProvider = new NetCoreResourceProvider();
            var uriBlaBla = new NetCoreTypeToUriLocator();

            using (var stream = resourceProvider.GetStream(uriBlaBla.GetUriFor(underlyingType)))
            {
                var wiringContext = WiringContextFactory.GetContext(new TypeFactory());
                var loader = new XamlNodesPullParser(wiringContext);
                var protoParser = new SuperProtoParser(wiringContext);

                return loader.Parse(protoParser.Parse(stream)).ToList();
            }
        }

        private Type GetInflatable(XamlNode xamlNode, IEnumerable<Type> inflatables)
        {
            if (xamlNode.XamlType != null)
            {
                var matches = from inflatable in inflatables
                    where inflatable.IsAssignableFrom(xamlNode.XamlType.UnderlyingType)
                    select inflatable;

                return matches.FirstOrDefault();
            }

            return null;
        }
    }
}