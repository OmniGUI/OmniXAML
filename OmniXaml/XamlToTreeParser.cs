namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Glass;

    public class XamlToTreeParser
    {
        private readonly Assembly assembly;
        private readonly IEnumerable<string> namespaces;

        public XamlToTreeParser(Assembly assembly, IEnumerable<string> namespaces)
        {
            this.assembly = assembly;
            this.namespaces = namespaces;
        }


        public ConstructionNode Parse(string xml)
        {
            var xm = XDocument.Load(new StringReader(xml));
            var node = xm.FirstNode;
            return ProcessNode((XElement)node);
        }

        private ConstructionNode ProcessNode(XElement node)
        {
            var type = LocateType(node.Name.LocalName);
            var directAssignments = GetAssignments(type, node).ToList();
            var nestedAssignments = ProcessInner(type, node.Nodes().Cast<XElement>()).ToList();

            return new ConstructionNode(type) { Assignments = directAssignments.Concat(nestedAssignments) };
        }

        private IEnumerable<PropertyAssignment> ProcessInner(Type type, IEnumerable<XElement> nodes)
        {
            return nodes.Select(node => ProcessProperty(type, node));
        }

        private PropertyAssignment ProcessProperty(Type type, XElement node)
        {
            var prop = node.Name;
            var name = prop.LocalName.SkipWhile(c => c != '.').Skip(1);
            var propertyName = new string(name.ToArray());

            if (string.IsNullOrEmpty(node.Value))
            {
                var children = node.Elements().Select(ProcessNode);
                return new PropertyAssignment { Property = Property.RegularProperty(type, propertyName), Children = children };
            }
            else
            {
                var value = node.Value;
                return new PropertyAssignment { Property = Property.RegularProperty(type, propertyName), SourceValue = value };
            }
        }

        private IEnumerable<PropertyAssignment> GetAssignments(Type type, XElement node)
        {
            return node.Attributes().Select(attribute => ToAssignment(type, attribute));
        }

        private PropertyAssignment ToAssignment(Type type, XAttribute attribute)
        {
            var assignment = new PropertyAssignment
            {
                Property = ResolveProperty(type, attribute),
                SourceValue = attribute.Value,
            };
            return assignment;
        }

        private Property ResolveProperty(Type type, XAttribute attribute)
        {
            var nameLocalName = attribute.Name.LocalName;
            if (nameLocalName.Contains('.'))
            {
                var dot = nameLocalName.IndexOf('.');
                var ownerName = nameLocalName.Take(dot).AsString();
                var propertyName = nameLocalName.Skip(dot + 1).AsString();
                var ownerType = LocateType(ownerName);
                return Property.FromAttached(ownerType, propertyName);
            }
            else
            {
                return Property.RegularProperty(type, nameLocalName);
            }            
        }

        private Type LocateType(string elementName)
        {
            return namespaces.Select(ns => assembly.GetType($"{ns}.{elementName}")).First(t => t != null);
        }
    }
}