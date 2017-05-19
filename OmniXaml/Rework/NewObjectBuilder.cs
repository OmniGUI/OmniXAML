using System.Reactive.Subjects;

namespace OmniXaml.Rework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Zafiro.Core;

    public class NewObjectBuilder : INewObjectBuilder
    {
        private readonly ISmartInstanceCreator instanceCreator;
        private readonly IStringSourceValueConverter sourceValueConverter;
        private readonly IValuePipeline pipeline;
        public ISubject<NodeInflation> NodeInflated { get; } = new ReplaySubject<NodeInflation>();

        public NewObjectBuilder(ISmartInstanceCreator instanceCreator, IStringSourceValueConverter sourceValueConverter, IValuePipeline pipeline)
        {
            this.instanceCreator = instanceCreator;
            this.sourceValueConverter = sourceValueConverter;
            this.pipeline = pipeline;
        }

        public object Inflate(ConstructionNode constructionNode)
        {
            var inflateAssignments = InflateAssignments(constructionNode.Assignments);

            IEnumerable<NewInjectableMember> members = GetInjectables(inflateAssignments);

            var positionals =
                from positional in constructionNode.PositionalParameters
                select new PositionalParameter(positional);

            var children = InflateChildren(constructionNode).ToList();

            var creationResult = instanceCreator.Create(constructionNode.InstanceType, new CreationHints(members, positionals, children));

            var unusedMembers = GetMembersNotUsedInConstruction(creationResult.UsedHints.Members, inflateAssignments);

            AssignNonInjectableDependencies(creationResult.Instance, unusedMembers, children);

            NodeInflated.OnNext(new NodeInflation(creationResult.Instance, constructionNode));

            return creationResult.Instance;
        }

        private IEnumerable<object> InflateChildren(ConstructionNode constructionNode)
        {
            return from c in constructionNode.Children
                   let i = Inflate(c)
                   select i;
        }

        private IEnumerable<NewInjectableMember> GetInjectables(IEnumerable<InflatedAssignment> inflatedAssignments)
        {
            var injectableMembers = from child in inflatedAssignments
                                    select new NewInjectableMember()
                                    {
                                        Values = child.Instances,
                                        Name = child.Member.MemberName,
                                        InjectionType = child.Member.MemberType,
                                    };

            var injectables = injectableMembers;
            return injectables;
        }

        private IEnumerable<InflatedAssignment> GetMembersNotUsedInConstruction(IEnumerable<NewInjectableMember> creationResultInjectedMembers, IEnumerable<InflatedAssignment> inflatedAssignments)
        {
            var assigned = from injected in creationResultInjectedMembers
                           join assignment in inflatedAssignments on injected.Name equals assignment.Member.MemberName
                           select assignment;

            return inflatedAssignments.Except(assigned);
        }

        private void AssignNonInjectableDependencies(object parent, IEnumerable<InflatedAssignment> inflatedAssignments, List<object> children)
        {
            void AssignChildren()
            {
                if (parent.GetType().IsCollection())
                {
                    foreach (var child in children)
                    {
                        Associate(parent, child);
                    }
                }
            }

            void AssignMembers()
            {
                foreach (var assignment in inflatedAssignments)
                {
                    if (assignment.Member.MemberType.IsCollection())
                    {
                        var hoster = assignment.Member.GetValue(parent);
                        foreach (var inst in assignment.Instances)
                        {
                            Associate(hoster, inst);
                        }
                    }
                    else
                    {
                        SetMember(parent, assignment.Member, assignment.Instances.First());
                    }
                }
            }

            AssignChildren();
            AssignMembers();
        }

        private void SetMember(object parent, Member member, object value)
        {
            var mutableUnit = new MutablePipelineUnit(null, value);
            pipeline.Handle(parent, member, mutableUnit);
            if (mutableUnit.Handled)
            {
                return;
            }

            member.SetValue(parent, mutableUnit.Value);
        }

        private void Associate(object parent, object child)
        {
            Collection.UniversalAdd(parent, child);
        }

        private IEnumerable<InflatedAssignment> InflateAssignments(IEnumerable<MemberAssignment> assignments)
        {
            return InflateAssignmentsFromChildren(assignments).Concat(AssignmentsFromSourceValues(assignments));
        }

        private IEnumerable<InflatedAssignment> InflateAssignmentsFromChildren(IEnumerable<MemberAssignment> assignments)
        {
            var instantiatedAssignments =
                from a in assignments
                where a.Values.Any()
                let children = from child in a.Values select Inflate(child)
                from ct in a.Values
                let inflate = Inflate(ct)
                select new InflatedAssignment
                {
                    Instances = children,
                    Member = a.Member,
                };

            return instantiatedAssignments;
        }

        private IEnumerable<InflatedAssignment> AssignmentsFromSourceValues(IEnumerable<MemberAssignment> assignments)
        {
            var instantiatedAssignments =
                from a in assignments
                where a.SourceValue != null
                let value = a.SourceValue
                let compatibleValue = GetCompatibleValue(value, a.Member.MemberType)
                select new InflatedAssignment
                {
                    Instances = new[] { compatibleValue },
                    Member = a.Member,
                };

            return instantiatedAssignments;
        }

        private object GetCompatibleValue(string strValue, Type desiredTargetType)
        {
            return sourceValueConverter.TryConvert(strValue, desiredTargetType).Item2;
        }
    }
}