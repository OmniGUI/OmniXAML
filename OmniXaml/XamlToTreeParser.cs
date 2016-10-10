namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    public class XamlToTreeParser
    {
        private readonly Assembly assembly;
        private readonly string ns;

        public XamlToTreeParser(Assembly assembly, string ns)
        {
            this.assembly = assembly;
            this.ns = ns;
        }


        public ConstructionNode Parse(string xml)
        {
            var xm = XDocument.Load(new StringReader(xml));
            var node = xm.FirstNode;
            return ProcessNode((XElement)node);
        }

        private ConstructionNode ProcessNode(XElement node)
        {
            var type = LocateType(node);
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
            var value = node.Value;
            
            return new PropertyAssignment { Property = Property.RegularProperty(type, propertyName), SourceValue = value}; 
        }

        private IEnumerable<PropertyAssignment> GetAssignments(Type type, XElement node)
        {
            return node.Attributes().Select(attribute => ToAssignment(type, attribute));
        }

        private PropertyAssignment ToAssignment(Type type, XAttribute attribute)
        {
            var assignment = new PropertyAssignment()
            {
                Property = Property.RegularProperty(type, attribute.Name.LocalName),
                SourceValue = attribute.Value,
            };
            return assignment;
        }

        private Type LocateType(XElement element)
        {            
            var type = assembly.GetType($"{ns}.{element.Name}");
            return type;
        }
    }
}