namespace OmniXaml
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    public class XamlToTreeParser
    {
        public XamlToTreeParser()
        {            
        }


        public void Parse(string xml)
        {
            XDocument xm = XDocument.Load(new StringReader(xml));
            var node = xm.FirstNode;
            ProcessNode((XElement) node);            
        }

        private ContructionNode ProcessNode(XElement node)
        {
            var type = LocateType(node);

            return new ContructionNode(type);
        }

        private Type LocateType(XElement element)
        {
            var ass = Assembly.Load(new AssemblyName("OmniXaml.Tests"));
            var type= ass.GetType($"OmniXaml.Tests.Model.{element.Name}");
            return type;
        }
    }
}