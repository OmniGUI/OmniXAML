namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ambient;
    using Glass.Core;
    using Tests;

    public class ObjectBuilder : IObjectBuilder
    {
        private readonly IConverterContextFactory contextFactory;

        protected ObjectBuilderContext ObjectBuilderContext { get; }
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;


        public ObjectBuilder(IInstanceCreator creator, ObjectBuilderContext objectBuilderContext, IConverterContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            ObjectBuilderContext = objectBuilderContext;
            this.creator = creator;
            sourceValueConverter = objectBuilderContext.SourceValueConverter;
        }

        public object Create(ConstructionNode node, object instance, BuildContext buildContext)
        {
            buildContext.AmbientRegistrator.RegisterInstance(instance);
            ApplyAssignments(instance, node.Assignments, buildContext);
            return instance;
        }

        public object Create(ConstructionNode node, BuildContext buildContext)
        {
            var instance = CreateInstance(node, buildContext);
            buildContext.InstanceLifecycleSignaler.BeforeAssigments(instance);
            ApplyAssignments(instance, node.Assignments, buildContext);
            buildContext.InstanceLifecycleSignaler.AfterAssigments(instance);
            return instance;
        }

        private object CreateInstance(ConstructionNode node, BuildContext buildContext)
        {
            var instance = creator.Create(node.InstanceType, buildContext, node.InjectableArguments.Select(s => new InjectableMember(s)));
            buildContext.NamescopeAnnotator.TrackNewInstance(instance);
            buildContext.AmbientRegistrator.RegisterInstance(instance);
            if (node.Name != null)
            {
                buildContext.NamescopeAnnotator.RegisterName(node.Name, instance);
            }
            return instance;
        }

        protected virtual void ApplyAssignments(object instance, IEnumerable<MemberAssignment> propertyAssignments, BuildContext buildContext)
        {
            foreach (var propertyAssignment in propertyAssignments)
            {
                ApplyAssignment(instance, propertyAssignment, buildContext);
            }
        }

        private void ApplyAssignment(object instance, MemberAssignment propertyAssignment, BuildContext buildContext)
        {
            EnsureValidAssigment(propertyAssignment);
            var property = propertyAssignment.Member;

            if (propertyAssignment.Children.Count() == 1 || propertyAssignment.SourceValue != null)
            {
                ApplySingleAssignment(instance, propertyAssignment, buildContext, property);
            }
            else
            {
                ApplyMultiAssignment(instance, propertyAssignment, buildContext, property);
            }
        }

        private void ApplyMultiAssignment(object instance, MemberAssignment propertyAssignment, BuildContext buildContext, Member property)
        {
            foreach (var constructionNode in propertyAssignment.Children)
            {
                var value = Create(constructionNode, buildContext);
                var compatibleValue = ToCompatibleValue(new Assignment(instance, property, value), buildContext);
                Utils.UniversalAdd(compatibleValue.Member.GetValue(compatibleValue.Instance), compatibleValue.Value);
            }
        }

        private void ApplySingleAssignment(object instance, MemberAssignment propertyAssignment, BuildContext buildContext, Member property)
        {
            object value;
            if (propertyAssignment.SourceValue == null)
            {
                var first = propertyAssignment.Children.First();
                value = CreateChildProperty(instance, property, first, buildContext);
            }
            else
            {
                value = propertyAssignment.SourceValue;
            }

            var assignment = new Assignment(instance, property, value);
            var converted = ToCompatibleValue(assignment, buildContext);
            PerformAssigment(converted, buildContext);
        }

        protected virtual void PerformAssigment(Assignment converted, BuildContext buildContext)
        {
            if (converted.Member.MemberType.IsCollection())
            {
                Utils.UniversalAdd(converted.Member.GetValue(converted.Instance), converted.Value);
            }
            else
            {
                converted.ExecuteAssignment();
                OnAssigmentExecuted(converted, buildContext);
            }
        }

        protected void OnAssigmentExecuted(Assignment assignment, BuildContext buildContext)
        {
            var ambientPropertyAssignment = new AmbientMemberAssignment
            {
                Property = assignment.Member,
                Value = assignment.Value
            };

            buildContext.AmbientRegistrator.RegisterAssignment(ambientPropertyAssignment);
        }

        protected virtual Assignment ToCompatibleValue(Assignment assignment, BuildContext buildContext)
        {
            if (assignment.Value is string)
            {
                var valueContext = contextFactory.CreateConverterContext(assignment.Property.PropertyType, assignment.Value, buildContext);
                var compatibleValue = sourceValueConverter.GetCompatibleValue(valueContext);
                return assignment.ReplaceValue(compatibleValue);
            }
            else
            {
                return assignment;
            }
        }

        protected virtual object CreateChildProperty(object parent, Member property, ConstructionNode nodeToBeCreated, BuildContext buildContext)
        {
            return Create(nodeToBeCreated, buildContext);
        }

        private void EnsureValidAssigment(MemberAssignment propertyAssignment)
        {
            if (propertyAssignment.SourceValue != null && propertyAssignment.Children != null && propertyAssignment.Children.Any())
            {
                throw new InvalidOperationException("You cannot specify a Source Value and Children at the same time.");
            }
            if (propertyAssignment.SourceValue == null && !propertyAssignment.Children.Any())
            {
                throw new InvalidOperationException("Children is empty.");
            }
        }
    }
}