namespace OmniXaml.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Classes;

    public class TemplateHostingObjectAssembler : IObjectAssembler
    {
        private readonly IObjectAssembler objectAssembler;
        private bool recording;
        private IList<XamlNode> nodeList;
        private int depth = 0;

        public TemplateHostingObjectAssembler(IObjectAssembler objectAssembler)
        {
            this.objectAssembler = objectAssembler;
        }

        public void WriteNode(XamlNode node)
        {
            if (recording)
            {
                if (node.NodeType == XamlNodeType.StartObject)
                {
                    depth++;
                }
                if (node.NodeType == XamlNodeType.EndObject)
                {
                    depth--;
                    if (depth == 0)
                    {
                        recording = false;
                    }                    
                }

                nodeList.Add(node);
            }
            else
            {
                if (node.NodeType == XamlNodeType.StartObject && node.XamlType.UnderlyingType == typeof(Template))
                {
                    recording = true;
                    nodeList = new Collection<XamlNode>();
                    depth++;
                    nodeList.Add(node);
                }
                else
                {
                    objectAssembler.WriteNode(node);
                }
            }
        }

        public object Result => objectAssembler.Result;
        public IList NodeList => new ReadOnlyCollection<XamlNode>(nodeList);
    }

}