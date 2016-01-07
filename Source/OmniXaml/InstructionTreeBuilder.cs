namespace OmniXaml
{
    using System.Collections.Generic;

    public class InstructionTreeBuilder
    {
        public IEnumerable<InstructionNode> CreateHierarchy(IEnumerable<Instruction> xamlNodes)
        {
            var stream = xamlNodes.GetEnumerator();
            stream.MoveNext();
            return Create(stream);
        }

        private Sequence<InstructionNode> Create(IEnumerator<Instruction> stream)
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

        private static bool IsTrailing(Instruction instruction)
        {
            return instruction.InstructionType == InstructionType.EndMember || instruction.InstructionType == InstructionType.EndObject;
        }

        private static bool IsLeading(Instruction current)
        {
            return current.InstructionType == InstructionType.StartMember || current.InstructionType == InstructionType.StartObject ||
                   current.InstructionType == InstructionType.GetObject;
        }
    }
}