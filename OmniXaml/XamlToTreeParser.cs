namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Metadata;
    using TypeLocation;

    public class XamlToTreeParser : IXamlToTreeParser
    {
        private readonly AssignmentsExtractor assignmentsExtractor;
        private readonly IMetadataProvider metadataProvider;
        private readonly ITypeDirectory typeDirectory;

        public XamlToTreeParser(ITypeDirectory typeDirectory, IMetadataProvider metadataProvider, IEnumerable<IInlineParser> inlineParsers)
        {
            this.typeDirectory = typeDirectory;
            this.metadataProvider = metadataProvider;
            assignmentsExtractor = new AssignmentsExtractor(metadataProvider, typeDirectory, inlineParsers, ProcessNode);
        }


        public ConstructionNode Parse(string xml)
        {
            var xm = XDocument.Load(new StringReader(xml));
            var node = xm.FirstNode;
            return ProcessNode((XElement) node);
        }

        private ConstructionNode ProcessNode(XElement node)
        {
            var type = LocateType(node.Name);
            var allAssignments = assignmentsExtractor.GetAssignments(type, node);

            var ctorArgs = GetCtorArgs(node, type);

            return new ConstructionNode(type) {Assignments = allAssignments, InjectableArguments = ctorArgs};
        }

        private Type LocateType(XName typeName)
        {
            return typeDirectory.GetTypeByFullAddres(
                new Address
                {
                    Namespace = typeName.NamespaceName,
                    TypeName = typeName.LocalName
                });
        }

        private IEnumerable<string> GetCtorArgs(XContainer node, Type type)
        {
            var ctorArgs = new List<string>();

            var nodeFirstNode = node.FirstNode;
            if ((nodeFirstNode != null) && (nodeFirstNode.NodeType == XmlNodeType.Text))
            {
                var directContent = ((XText) nodeFirstNode).Value;
                var contentProperty = metadataProvider.Get(type).ContentProperty;
                if (contentProperty == null)
                    ctorArgs.Add(directContent);
            }
            return ctorArgs;
        }


        private string GetName(XElement node)
        {
            var nameAttr = node.Attributes().FirstOrDefault(attribute => attribute.Name.LocalName == "Name");
            return nameAttr?.Value;
        }
    }
}