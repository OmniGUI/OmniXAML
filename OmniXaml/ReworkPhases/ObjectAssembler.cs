using System;
using Zafiro.Core;

namespace OmniXaml.ReworkPhases
{
    using System.Collections.Generic;
    using System.Linq;
    using Rework;

    public class ObjectAssembler : IObjectAssembler
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly IStringSourceValueConverter converter;
        private readonly IMemberAssigmentApplier assigmentApplier;

        public ObjectAssembler(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter converter, IMemberAssigmentApplier assigmentApplier)
        {
            this.instanceCreator = instanceCreator;
            this.converter = converter;
            this.assigmentApplier = assigmentApplier;
        }

        public void Assemble(ConstructionNode node)
        {
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
                InflateAssignment(a);
            }

            foreach (var c in node.Children)
            {
                Assemble(c);
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

        private void InflateAssignment(MemberAssignment memberAssignment)
        {
            if (memberAssignment.SourceValue != null)
            {
                InflateFromSourceValue(memberAssignment);
            }
            else
            {
                InflateFromChildren(memberAssignment);
            }
        }

        private void CreateInstance(ConstructionNode node)
        {
            var positionalParameters = from n in node.PositionalParameters select new PositionalParameter(n);
            var creationHints = new CreationHints(new List<NewInjectableMember>(), positionalParameters, new List<object>());
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
            var tryConvert = converter.TryConvert(node.SourceValue, node.ActualInstanceType);

            node.Instance = tryConvert.Item2;
            node.IsCreated = tryConvert.Item1;
            node.SourceValue = node.SourceValue;
            node.InstanceType = node.ActualInstanceType;
        }

        private void InflateFromChildren(MemberAssignment a)
        {
            foreach (var node in a.Values)
            {
                Assemble(node);
            }            
        }

        private void InflateFromSourceValue(MemberAssignment a)
        {
            var conversionResult = converter.TryConvert(a.SourceValue, a.Member.MemberType);

            a.Values = new List<ConstructionNode>
            {
                new ConstructionNode(a.Member.MemberType)
                {
                    Instance = conversionResult.Item2,
                    IsCreated = conversionResult.Item1,
                }
            };
        }
    }
}