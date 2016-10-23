namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Glass.Core;
    using Metadata;
    using TypeLocation;

    public class XamlToTreeParser
    {
        private readonly ITypeDirectory typeDirectory;
        private readonly IMetadataProvider metadataProvider;
        private readonly IEnumerable<IInlineParser> inlineParsers;

        public XamlToTreeParser(ITypeDirectory typeDirectory, IMetadataProvider metadataProvider, IEnumerable<IInlineParser> inlineParsers)
        {
            this.typeDirectory = typeDirectory;
            this.metadataProvider = metadataProvider;
            this.inlineParsers = inlineParsers;
        }


        public ConstructionNode Parse(string xml)
        {
            var xm = XDocument.Load(new StringReader(xml));
            var node = xm.FirstNode;
            return ProcessNode((XElement)node);
        }

        private ConstructionNode ProcessNode(XElement node)
        {
            var type = LocateType(node.Name);
            var directAssignments = GetAssignments(type, node).ToList();
            var nestedAssignments = ProcessInnerElements(type, node.Nodes().OfType<XElement>()).ToList();

            var ctorArgs = new List<string>();

            var nodeFirstNode = node.FirstNode;
            if (nodeFirstNode != null && nodeFirstNode.NodeType == XmlNodeType.Text)
            {
                var directContent = ((XText)nodeFirstNode).Value;
                var contentProperty = metadataProvider.Get(type).ContentProperty;
                if (contentProperty == null)
                {
                    ctorArgs.Add(directContent);
                }
                else
                {
                    var property = Property.RegularProperty(type, contentProperty);
                    var assignment = new PropertyAssignment { Property = property, SourceValue = directContent };
                    var propertyAssignments = directAssignments.Concat(nestedAssignments).Concat(new[] { assignment });
                    return new ConstructionNode(type) { Assignments = propertyAssignments, };
                }
            }

            return new ConstructionNode(type) { Assignments = directAssignments.Concat(nestedAssignments), InjectableArguments = ctorArgs };
        }

        private IEnumerable<PropertyAssignment> ProcessInnerElements(Type type, IEnumerable<XElement> nodes)
        {
            return nodes.Select(
                node => IsProperty(node)
                    ? ProcessExplicityProperty(type, node)
                    : ProcessImplicityProperty(type, node));
        }

        private PropertyAssignment ProcessImplicityProperty(Type type, XElement node)
        {
            var ctorNode = ProcessNode(node);
            var contentProperty = metadataProvider.Get(type).ContentProperty;
            if (contentProperty == null)
            {
                throw new XamlParserException($"Cannot assign node. The content property of the type {type} cannot be found");
            }
            return new PropertyAssignment() {Property = Property.RegularProperty(type, contentProperty), Children = new[] {ctorNode}};
        }

        private static bool IsProperty(XElement node)
        {
            return node.Name.LocalName.Contains(".");
        }

        private PropertyAssignment ProcessExplicityProperty(Type type, XElement node)
        {
            var prop = node.Name;
            var name = prop.LocalName.SkipWhile(c => c != '.').Skip(1);
            var propertyName = new string(name.ToArray());

            var nodeFirstNode = node.FirstNode;
            if (nodeFirstNode != null && (nodeFirstNode.NodeType == XmlNodeType.Text || nodeFirstNode.NodeType == XmlNodeType.CDATA))
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
            return node
                .Attributes()
                .Where(attribute => !attribute.IsNamespaceDeclaration)
                .Select(attribute => ToAssignment(type, attribute));
        }

        private PropertyAssignment ToAssignment(Type type, XAttribute attribute)
        {
            var value = attribute.Value;
            var property = ResolveProperty(type, attribute);

            var parser = inlineParsers.FirstOrDefault(p => p.CanParse(value));
            if (parser != null)
            {
                var constructionNode = parser.Parse(value);
                return new PropertyAssignment { Property = Property.RegularProperty(type, property.PropertyName), Children = new[] { constructionNode } };
            }

            var assignment = new PropertyAssignment
            {
                Property = property,
                SourceValue = value,
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

                XName xname = attribute.Name.NamespaceName == string.Empty ? XName.Get(ownerName, attribute.Parent.Name.NamespaceName) : XName.Get(ownerName, attribute.Name.NamespaceName);
                var ownerType = LocateType(xname);
                return Property.FromAttached(ownerType, propertyName);
            }
            else
            {
                return Property.RegularProperty(type, nameLocalName);
            }
        }

        private Type LocateType(XName typeName)
        {
            return typeDirectory.GetTypeByFullAddres(new Address()
            {
                 Namespace = typeName.NamespaceName,
                 TypeName = typeName.LocalName,
            });
        }
    }
} 