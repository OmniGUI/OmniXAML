namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ambient;
    using Glass.Core;

    public class ObjectBuilder : IObjectBuilder
    {
        private readonly Func<Assignment, ObjectBuilderContext, BuildContext, ValueContext> createValueContext;
        protected ObjectBuilderContext ObjectBuilderContext { get; }
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;


        public ObjectBuilder(ObjectBuilderContext objectBuilderContext, Func<Assignment, ObjectBuilderContext, BuildContext, ValueContext> createValueContext)
        {
            this.createValueContext = createValueContext;
            ObjectBuilderContext = objectBuilderContext;
            creator = objectBuilderContext.Creator;
            sourceValueConverter = objectBuilderContext.SourceValueConverter;
        }

        public object Create(ConstructionNode node, object instance, BuildContext buildContext)
        {
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
            var instance = creator.Create(node.InstanceType);
            buildContext.NamescopeAnnotator.TrackNewInstance(instance);
            buildContext.AmbientRegistrator.RegisterInstance(instance);
            if (node.Name != null)
            {
                buildContext.NamescopeAnnotator.RegisterName(node.Name, instance);
            }
            return instance;
        }

        protected virtual void ApplyAssignments(object instance, IEnumerable<PropertyAssignment> propertyAssignments, BuildContext buildContext)
        {
            foreach (var propertyAssignment in propertyAssignments)
            {
                ApplyAssignment(instance, propertyAssignment, buildContext);
            }
        }

        private void ApplyAssignment(object instance, PropertyAssignment propertyAssignment, BuildContext buildContext)
        {
            EnsureValidAssigment(propertyAssignment);
            var property = propertyAssignment.Property;

            if (propertyAssignment.Children.Count() == 1 || propertyAssignment.SourceValue != null)
            {
                ApplySingleAssignment(instance, propertyAssignment, buildContext, property);
            }
            else
            {
                ApplyMultiAssignment(instance, propertyAssignment, buildContext, property);
            }
        }

        private void ApplyMultiAssignment(object instance, PropertyAssignment propertyAssignment, BuildContext buildContext, Property property)
        {
            foreach (var constructionNode in propertyAssignment.Children)
            {
                var value = Create(constructionNode, buildContext);
                var compatibleValue = ToCompatibleValue(new Assignment(instance, property, value), buildContext);
                Utils.UniversalAdd(compatibleValue.Property.GetValue(compatibleValue.Instance), compatibleValue.Value);
            }
        }

        private void ApplySingleAssignment(object instance, PropertyAssignment propertyAssignment, BuildContext buildContext, Property property)
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
            if (converted.Property.PropertyType.IsCollection())
            {
                Utils.UniversalAdd(converted.Property.GetValue(converted.Instance), converted.Value);
            }
            else
            {
                converted.ExecuteAssignment();
                OnAssigmentExecuted(converted, buildContext);
            }
        }

        protected void OnAssigmentExecuted(Assignment assignment, BuildContext buildContext)
        {
            var ambientPropertyAssignment = new AmbientPropertyAssignment
            {
                Property = assignment.Property,
                Value = assignment.Value
            };

            buildContext.AmbientRegistrator.RegisterAssignment(ambientPropertyAssignment);
        }

        protected virtual Assignment ToCompatibleValue(Assignment assignment, BuildContext buildContext)
        {
            if (assignment.Value is string)
            {
                var valueContext = createValueContext(assignment, ObjectBuilderContext, buildContext);
                var compatibleValue = sourceValueConverter.GetCompatibleValue(valueContext);
                return assignment.ReplaceValue(compatibleValue);
            }
            else
            {
                return assignment;
            }
        }

        protected virtual object CreateChildProperty(object parent, Property property, ConstructionNode nodeToBeCreated, BuildContext buildContext)
        {
            return Create(nodeToBeCreated, buildContext);
        }

        private void EnsureValidAssigment(PropertyAssignment propertyAssignment)
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