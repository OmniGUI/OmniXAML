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
                if (node.NodeType == XamlNodeType.StartMember)
                {
                    depth++;
                }

                if (node.NodeType == XamlNodeType.EndMember)
                {
                    depth--;
                    if (depth == 0)
                    {
                        recording = false;
                    }                    
                }

                if (depth>0)
                {
                    nodeList.Add(node);
                }
            }
            else
            {
                if (node.NodeType == XamlNodeType.StartMember && node.Member.Name == "Content")
                {
                    recording = true;
                    nodeList = new Collection<XamlNode>();
                    depth++;
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