namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Glass;
    using Glass.Core;

    public class XamlToTreeParser
    {
        private readonly IContentPropertyProvider contentPropertyProvider;
        private readonly ITypeDirectory typeDirectory;

        public XamlToTreeParser(IContentPropertyProvider contentPropertyProvider, ITypeDirectory typeDirectory)
        {
            this.contentPropertyProvider = contentPropertyProvider;
            this.typeDirectory = typeDirectory;
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
            var nestedAssignments = ProcessInnerElements(type, node.Nodes().OfType<XElement>()).ToList();

            var ctorArgs = new List<string>();

            var nodeFirstNode = node.FirstNode;
            if (nodeFirstNode != null && nodeFirstNode.NodeType == XmlNodeType.Text)
            {
                var directContent = ((XText)nodeFirstNode).Value;
                var contentProperty = contentPropertyProvider.GetContentProperty(type);
                if (contentProperty == null)
                {
                    ctorArgs.Add(directContent);
                }
                else
                {
                    var assignment = new PropertyAssignment() { Property = Property.RegularProperty(type, contentProperty), SourceValue = directContent };
                    return new ConstructionNode(type) { Assignments = directAssignments.Concat(nestedAssignments).Concat(new[] { assignment }), };
                }
            }

            return new ConstructionNode(type) { Assignments = directAssignments.Concat(nestedAssignments), InjectableArguments = ctorArgs };
        }

        private IEnumerable<PropertyAssignment> ProcessInnerElements(Type type, IEnumerable<XElement> nodes)
        {

            return nodes.Select(node =>
            {
                if (IsProperty(node))
                {
                    return ProcessProperty(type, node);
                }
                else
                {
                    var ctorNode = this.ProcessNode(node);
                    return new PropertyAssignment() { Property = Property.RegularProperty(type, contentPropertyProvider.GetContentProperty(type)), Children = new[] { ctorNode } };
                }
            });
        }

        private static bool IsProperty(XElement node)
        {
            return node.Name.LocalName.Contains(".");
        }

        private PropertyAssignment ProcessProperty(Type type, XElement node)
        {
            var prop = node.Name;
            var name = prop.LocalName.SkipWhile(c => c != '.').Skip(1);
            var propertyName = new string(name.ToArray());

            var nodeFirstNode = node.FirstNode;
            if (nodeFirstNode != null && nodeFirstNode.NodeType == XmlNodeType.Text)
            {
                var value = ((XText)nodeFirstNode).Value;
                return new PropertyAssignment { Property = Property.RegularProperty(type, propertyName), SourceValue = value };                
            }
            else
            {
                var children = node.Elements().Select(ProcessNode);
                return new PropertyAssignment { Property = Property.RegularProperty(type, propertyName), Children = children };
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

        private Type LocateType(string typeName)
        {
            return typeDirectory.GetTypeByPrefix(string.Empty, typeName);
        }
    }
}