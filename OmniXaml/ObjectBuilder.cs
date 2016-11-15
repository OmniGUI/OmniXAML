namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ambient;
    using Glass.Core;

    public class ObjectBuilder : IObjectBuilder
    {
        private readonly IConverterContextFactory contextFactory;
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;


        public ObjectBuilder(IInstanceCreator creator, ObjectBuilderContext objectBuilderContext, IConverterContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
            ObjectBuilderContext = objectBuilderContext;
            this.creator = creator;
            sourceValueConverter = objectBuilderContext.SourceValueConverter;
        }

        protected ObjectBuilderContext ObjectBuilderContext { get; }

        public object Create(ConstructionNode node, object instance, BuildContext buildContext)
        {
            buildContext.AmbientRegistrator.RegisterInstance(instance);
            ApplyAssignments(instance, node.Assignments, buildContext);
            CreateChildren(instance, node.Children, buildContext);
            return instance;
        }

        public object Create(ConstructionNode node, BuildContext buildContext)
        {
            var instance = CreateInstance(node, buildContext);
            buildContext.InstanceLifecycleSignaler.BeforeAssigments(instance);
            ApplyAssignments(instance, node.Assignments, buildContext);
            CreateChildren(instance, node.Children, buildContext);
            buildContext.InstanceLifecycleSignaler.AfterAssigments(instance);
            return instance;
        }

        private void CreateChildren(object parent, IEnumerable<ConstructionNode> children, BuildContext buildContext)
        {
            foreach (var constructionNode in children)
            {
                var child = Create(constructionNode, buildContext);
                var association = new ChildAssociation(parent, child, constructionNode.Key);

                Associate(association);
            }
        }

        private static void Associate(ChildAssociation childAssociation)
        {
            if (childAssociation.Key == null)
            {
                Utils.UniversalAdd(childAssociation.Parent, childAssociation.Child);                
            }
            else
            {
                Utils.UniversalAddToDictionary(childAssociation.Parent, childAssociation.Child, childAssociation.Key);
            }
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

            if ((propertyAssignment.Children.Count() == 1) || (propertyAssignment.SourceValue != null))
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

                var parent = compatibleValue.Member.GetValue(compatibleValue.Instance);
                var pendingAdd = new ChildAssociation(parent, compatibleValue.Value, constructionNode.Key);

                Associate(pendingAdd);
            }
        }

        private void ApplySingleAssignment(object instance, MemberAssignment propertyAssignment, BuildContext buildContext, Member property)
        {
            object value;
            string key = null;
            if (propertyAssignment.SourceValue == null)
            {
                var first = propertyAssignment.Children.First();
                key = first.Key;
                value = CreateChildProperty(instance, property, first, buildContext);
            }
            else
            {
                value = propertyAssignment.SourceValue;
            }

            var assignment = new Assignment(instance, property, value);
            var converted = ToCompatibleValue(assignment, buildContext);
            PerformAssigment(converted, buildContext, key);
        }

        protected virtual void PerformAssigment(Assignment converted, BuildContext buildContext, string key)
        {
            if (converted.Member.MemberType.IsCollection())
            {
                var parent = converted.Member.GetValue(converted.Instance);
                var child = converted.Value;
                var pendingAdd = new ChildAssociation(parent, child, key);
                Associate(pendingAdd);
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
                var valueContext = contextFactory.CreateConverterContext(assignment.Member.MemberType, assignment.Value, buildContext);
                var compatibleValue = sourceValueConverter.GetCompatibleValue(valueContext);
                return assignment.ReplaceValue(compatibleValue);
            }

            return assignment;
        }

        protected virtual object CreateChildProperty(object parent, Member property, ConstructionNode nodeToBeCreated, BuildContext buildContext)
        {
            return Create(nodeToBeCreated, buildContext);
        }

        private void EnsureValidAssigment(MemberAssignment propertyAssignment)
        {
            if ((propertyAssignment.SourceValue != null) && (propertyAssignment.Children != null) && propertyAssignment.Children.Any())
            {
                throw new InvalidOperationException("You cannot specify a Source Value and Children at the same time.");                
            }

            if ((propertyAssignment.SourceValue == null) && !propertyAssignment.Children.Any())
            {
                throw new InvalidOperationException("Children is empty.");
            }
        }
    }
}