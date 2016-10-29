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

    public class XamlToTreeParser : IXamlToTreeParser
    {
        private readonly ITypeDirectory typeDirectory;
        private readonly IMetadataProvider metadataProvider;
        private readonly IEnumerable<IInlineParser> inlineParsers;
        private readonly AssignmentsExtractor assignmentsExtractor;

        public XamlToTreeParser(ITypeDirectory typeDirectory, IMetadataProvider metadataProvider, IEnumerable<IInlineParser> inlineParsers)
        {
            this.typeDirectory = typeDirectory;
            this.metadataProvider = metadataProvider;
            this.inlineParsers = inlineParsers;
            assignmentsExtractor = new AssignmentsExtractor(this, metadataProvider);
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
            var allAssignments = assignmentsExtractor.GetAssignments(type, node);

            var ctorArgs = GetCtorArgs(node, type);

            return new ConstructionNode(type) { Assignments = allAssignments, InjectableArguments = ctorArgs, };
        }

        private IEnumerable<string> GetCtorArgs(XContainer node, Type type)
        {
            var ctorArgs = new List<string>();

            var nodeFirstNode = node.FirstNode;
            if (nodeFirstNode != null && nodeFirstNode.NodeType == XmlNodeType.Text)
            {
                var directContent = ((XText) nodeFirstNode).Value;
                var contentProperty = metadataProvider.Get(type).ContentProperty;
                if (contentProperty == null)
                {
                    ctorArgs.Add(directContent);
                }
            }
            return ctorArgs;
        }


        private string GetName(XElement node)
        {
            var nameAttr = node.Attributes().FirstOrDefault(attribute => attribute.Name.LocalName == "Name");
            return nameAttr?.Value;
        }

        private Type LocateType(XName typeName)
        {
            return typeDirectory.GetTypeByFullAddres(new Address()
            {
                Namespace = typeName.NamespaceName,
                TypeName = typeName.LocalName,
            });
        }

        private class AssignmentsExtractor
        {
            private readonly XamlToTreeParser parser;
            private readonly IMetadataProvider metadataProvider;

            public AssignmentsExtractor(XamlToTreeParser parser, IMetadataProvider metadataProvider)
            {
                this.parser = parser;
                this.metadataProvider = metadataProvider;
            }

            public IEnumerable<PropertyAssignment> GetAssignments(Type type, XElement node)
            {
                return GetAssignmentsFromAttributes(type, node)
                    .Concat(GetAssignmentsFromContent(type, node))
                    .Concat(GetAssignmentsFromInnerElements(type, node.Nodes().OfType<XElement>()));
            }

            private IEnumerable<PropertyAssignment> GetAssignmentsFromContent(Type type, XElement node)
            {
                var nodeFirstNode = node.FirstNode;
                if (nodeFirstNode != null && nodeFirstNode.NodeType == XmlNodeType.Text && metadataProvider.Get(type).ContentProperty != null)
                {
                    var directContent = ((XText)nodeFirstNode).Value;
                    var contentProperty = metadataProvider.Get(type).ContentProperty;
                    var property = Property.RegularProperty(type, contentProperty);
                    yield return new PropertyAssignment { Property = property, SourceValue = directContent };
                }
            }

            private IEnumerable<PropertyAssignment> GetAssignmentsFromInnerElements(Type type, IEnumerable<XElement> nodes)
            {
                return nodes.Select<XElement, PropertyAssignment>(
                    node => IsProperty(node)
                        ? ProcessExplicityProperty(type, node)
                        : ProcessImplicityProperty(type, node));
            }

            private PropertyAssignment ProcessImplicityProperty(Type type, XElement node)
            {
                var constructionNode = parser.ProcessNode(node);
                var contentProperty = metadataProvider.Get(type).ContentProperty;
                if (contentProperty == null)
                {
                    throw new XamlParserException($"Cannot assign node. The content property of the type {type} cannot be found");
                }

                return new PropertyAssignment { Property = Property.RegularProperty(type, contentProperty), Children = new[] { constructionNode } };
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
                    var children = node.Elements().Select(parser.ProcessNode);
                    return new PropertyAssignment { Property = Property.RegularProperty(type, propertyName), Children = children };
                }
            }

            public IEnumerable<PropertyAssignment> GetAssignmentsFromAttributes(Type type, XElement node)
            {
                return node
                    .Attributes()
                    .Where(IsAssignment)
                    .Select(attribute => ToAssignment(type, attribute));
            }

            private static bool IsAssignment(XAttribute attribute)
            {
                return !attribute.IsNamespaceDeclaration && attribute.Name.Namespace != "special";
            }

            private PropertyAssignment ToAssignment(Type type, XAttribute attribute)
            {
                var value = attribute.Value;
                var property = ResolveProperty(type, attribute);

                var inlineParser = parser.inlineParsers.FirstOrDefault(p => p.CanParse(value));
                if (inlineParser != null)
                {
                    var constructionNode = inlineParser.Parse(value);
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
                    var ownerType = parser.LocateType(xname);
                    return Property.FromAttached(ownerType, propertyName);
                }
                else
                {
                    return Property.RegularProperty(type, nameLocalName);
                }
            }
        }
    }
}