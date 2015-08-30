namespace OmniXaml
{
    using System.Collections.Generic;

    public class InstructionTreeBuilder
    {
        public IEnumerable<InstructionNode> CreateHierarchy(IEnumerable<XamlInstruction> xamlNodes)
        {
            var stream = xamlNodes.GetEnumerator();
            stream.MoveNext();
            return Create(stream);
        }

        private Sequence<InstructionNode> Create(IEnumerator<XamlInstruction> stream)
        {
            var nodes = new Sequence<InstructionNode>();

            while (IsLeading(stream.Current))
            {
                var currentNode = new InstructionNode { Leading = stream.Current };
                var continueWorking = true;
                while (stream.MoveNext() && continueWorking)
                {
                    if (IsLeading(stream.Current))
                    {
                        currentNode.Children = Create(stream);
                    }

                    var xamlNode = stream.Current;

                    if (IsTrailing(xamlNode))
                    {
                        continueWorking = false;
                        currentNode.Trailing = stream.Current;
                    }
                    else
                    {
                        currentNode.Body.Add(stream.Current);
                    }
                }

                nodes.Add(currentNode);
            }

            return nodes;
        }

        private static bool IsTrailing(XamlInstruction xamlInstruction)
        {
            return xamlInstruction.InstructionType == XamlInstructionType.EndMember || xamlInstruction.InstructionType == XamlInstructionType.EndObject;
        }

        private static bool IsLeading(XamlInstruction current)
        {
            return current.InstructionType == XamlInstructionType.StartMember || current.InstructionType == XamlInstructionType.StartObject;
        }
    }
}