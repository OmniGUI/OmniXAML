using System;
using Zafiro.Core;

namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;
    using System.Linq;
    using Rework;

    public class NodeAssembler : INodeAssembler
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly IStringSourceValueConverter converter;
        private readonly IMemberAssigmentApplier assigmentApplier;

        public NodeAssembler(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier assigmentApplier)
        {
            this.instanceCreator = instanceCreator;
            this.converter = converter;
            this.assigmentApplier = assigmentApplier;
        }

        public void Assemble(ConstructionNode node, ConstructionNode parent = null)
        {
            node.Parent = parent;

            if (node.IsCreated)
            {
                return;
            }

            if (node.SourceValue != null)
            {
                CreateLeafNode(node);
            }
            else
            {
                CreateIntermediateNode(node);
            }

        }

        private void CreateIntermediateNode(ConstructionNode node)
        {
            foreach (var a in node.Assignments)
            {
                InflateAssignment(a, node);
            }

            foreach (var c in node.Children)
            {
                Assemble(c, node);
            }

            if (CanBeCreated(node))
            {
                CreateInstance(node);
                ApplyAssignments(node);
                AttachChildren(node);
            }
        }

        private void AttachChildren(ConstructionNode node)
        {
            var children = node.Children.Select(c => c.Instance).ToList();

            foreach (var c in children)
            {
                Collection.UniversalAdd(node.Instance, c);
            }
        }

        private void ApplyAssignments(ConstructionNode node)
        {
            foreach (var assignment in node.Assignments)
            {
                assigmentApplier.ExecuteAssignment(assignment, node.Instance);
            }
        }

        private void InflateAssignment(MemberAssignment memberAssignment, ConstructionNode node)
        {
            if (memberAssignment.SourceValue != null)
            {
                InflateFromSourceValue(memberAssignment, node);
            }
            else
            {
                InflateFromChildren(memberAssignment, node);
            }
        }

        private void CreateInstance(ConstructionNode node)
        {
            var positionalParameters = from n in node.PositionalParameters select new PositionalParameter(n);
            var creationHints = new CreationHints(new List<InjectableMember>(), positionalParameters, new List<object>());
            var instance = instanceCreator.Create(node.ActualInstanceType, creationHints).Instance;
            node.Instance = instance;
            node.IsCreated = true;
        }

        private static bool CanBeCreated(ConstructionNode node)
        {
            var allAsignmentsCreated = node.Assignments.All(assignment => assignment.Values.All(c => c.IsCreated));
            var allChildrenCreated = node.Children.All(c => c.IsCreated);
            return allAsignmentsCreated && allChildrenCreated;
        }

        private void CreateLeafNode(ConstructionNode node)
        {
            var tryConvert = converter.TryConvert(node.SourceValue, node.ActualInstanceType, new ConvertContext() { Node = node });

            node.Instance = tryConvert.Item2;
            node.IsCreated = tryConvert.Item1;
            node.SourceValue = node.SourceValue;
            node.InstanceType = node.ActualInstanceType;
        }

        private void InflateFromChildren(MemberAssignment a, ConstructionNode constructionNode)
        {
            foreach (var node in a.Values)
            {
                Assemble(node, constructionNode);
            }
        }

        private void InflateFromSourceValue(MemberAssignment a, ConstructionNode node)
        {
            var conversionResult = converter.TryConvert(a.SourceValue, a.Member.MemberType, new ConvertContext() { Node = node, });

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