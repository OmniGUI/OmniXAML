using System.Collections.Generic;
using System.Linq;
using Zafiro.Core;

namespace OmniXaml
{
    public class NodeAssembler : INodeAssembler
    {
        public IInstanceCreator InstanceCreator { get; }
        public IStringSourceValueConverter Converter { get; }
        public IMemberAssigmentApplier AssigmentApplier { get; }

        public NodeAssembler(IInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier assigmentApplier)
        {
            this.InstanceCreator = instanceCreator;
            this.Converter = converter;
            this.AssigmentApplier = assigmentApplier;
        }

        public void Assemble(ConstructionNode node, INodeToObjectBuilder nodeToObjectBuilder, ConstructionNode parent = null, BuilderContext context = null)
        {
            node.Parent = parent;

            if (node.IsCreated)
            {
                return;
            }

            if (node.SourceValue != null)
            {
                AssembleLeafNode(node, context);
            }
            else
            {
                CreateIntermediateNode(node, nodeToObjectBuilder, context);
            }

        }

        private void CreateIntermediateNode(ConstructionNode node, INodeToObjectBuilder nodeToObjectBuilder, BuilderContext context)
        {
            foreach (var a in node.Assignments)
            {
                AssembleAssignment(a, node, nodeToObjectBuilder, context);
            }

            foreach (var c in node.Children)
            {
                Assemble(c, nodeToObjectBuilder, node, context);
            }

            if (CanBeCreated(node))
            {
                CreateInstance(node);
                ApplyAssignments(node, nodeToObjectBuilder, context);
                AttachChildren(node);
            }
        }

        private static void AttachChildren(ConstructionNode node)
        {
            var children = node.Children.Select(c => c.Instance).ToList();

            foreach (var c in children)
            {
                Collection.UniversalAdd(node.Instance, c);
            }
        }

        private void ApplyAssignments(ConstructionNode node, INodeToObjectBuilder builder, BuilderContext context)
        {
            foreach (var assignment in node.Assignments)
            {
                var nodeAssignment = new NodeAssignment(assignment, node.Instance);
                AssigmentApplier.ExecuteAssignment(nodeAssignment, builder, context);
            }
        }

        private void AssembleAssignment(MemberAssignment memberAssignment, ConstructionNode node, INodeToObjectBuilder nodeToObjectBuilder, BuilderContext context)
        {
            if (memberAssignment.SourceValue != null)
            {
                AssembleFromSourceValue(memberAssignment, node, context);
            }
            else
            {
                AssembleFromChildren(memberAssignment, node, nodeToObjectBuilder, context);
            }
        }

        protected void CreateInstance(ConstructionNode node)
        {
            var positionalParameters = from n in node.PositionalParameters select new PositionalParameter(n);
            var creationHints = new CreationHints(new List<InjectableMember>(), positionalParameters, new List<object>());
            var instance = InstanceCreator.Create(node.ActualInstanceType, creationHints).Instance;
            node.Instance = instance;
            node.IsCreated = true;
        }

        private static bool CanBeCreated(ConstructionNode node)
        {
            var allAsignmentsCreated = node.Assignments.All(assignment => assignment.Values.All(c => c.IsCreated));
            var allChildrenCreated = node.Children.All(c => c.IsCreated);
            return allAsignmentsCreated && allChildrenCreated;
        }

        private void AssembleLeafNode(ConstructionNode node, BuilderContext context)
        {
            var tryConvert = Converter.Convert(node.SourceValue, node.ActualInstanceType, new ConvertContext() { Node = node });

            node.Instance = tryConvert.Item2;
            node.IsCreated = tryConvert.Item1;
            node.SourceValue = node.SourceValue;
            node.InstanceType = node.ActualInstanceType;
        }

        protected virtual void AssembleFromChildren(MemberAssignment a, ConstructionNode constructionNode, INodeToObjectBuilder nodeToObjectBuilder, BuilderContext context)
        {
            foreach (var node in a.Values)
            {
                Assemble(node, nodeToObjectBuilder, constructionNode, context);
            }
        }

        private void AssembleFromSourceValue(MemberAssignment a, ConstructionNode node, BuilderContext context)
        {
            var conversionResult = Converter.Convert(a.SourceValue, a.Member.MemberType, new ConvertContext() { Node = node, BuilderContext = context });

            a.Values = new List<ConstructionNode>
            {
                new ConstructionNode(a.Member.MemberType)
                {
                    Instance = conversionResult.Item2,
                    IsCreated = conversionResult.Item1,
                    Parent = node,
                }
            };
        }
    }
}