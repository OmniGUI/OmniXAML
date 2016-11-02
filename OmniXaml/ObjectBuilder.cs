namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ambient;
    using Glass.Core;

    public class ObjectBuilder : IObjectBuilder
    {
        protected StaticContext StaticContext { get; }
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;


        public ObjectBuilder(StaticContext staticContext)
        {
            StaticContext = staticContext;
            creator = staticContext.Creator;
            sourceValueConverter = staticContext.SourceValueConverter;
        }

        public object Create(ConstructionNode node, object instance, TrackingContext trackingContext)
        {
            ApplyAssignments(instance, node.Assignments, trackingContext);
            return instance;
        }

        public object Create(ConstructionNode node, TrackingContext trackingContext)
        {
            var instance = CreateInstance(node, trackingContext);
            trackingContext.InstanceLifecycleSignaler.BeforeAssigments(instance);
            ApplyAssignments(instance, node.Assignments, trackingContext);
            trackingContext.InstanceLifecycleSignaler.AfterAssigments(instance);
            return instance;
        }

        private object CreateInstance(ConstructionNode node, TrackingContext trackingContext)
        {
            var instance = creator.Create(node.InstanceType);
            trackingContext.Annotator.TrackNewInstance(instance);
            trackingContext.AmbientRegistrator.RegisterInstance(instance);
            if (node.Name != null)
            {
                trackingContext.Annotator.RegisterName(node.Name, instance);
            }
            return instance;
        }

        private void ApplyAssignments(object instance, IEnumerable<PropertyAssignment> propertyAssignments, TrackingContext trackingContext)
        {
            foreach (var propertyAssignment in propertyAssignments)
            {
                ApplyAssignment(instance, propertyAssignment, trackingContext);
            }
        }

        private void ApplyAssignment(object instance, PropertyAssignment propertyAssignment, TrackingContext trackingContext)
        {
            EnsureValidAssigment(propertyAssignment);
            var property = propertyAssignment.Property;

            if (propertyAssignment.Children.Count() == 1 || propertyAssignment.SourceValue != null)
            {
                ApplySingleAssignment(instance, propertyAssignment, trackingContext, property);
            }
            else
            {
                ApplyMultiAssignment(instance, propertyAssignment, trackingContext, property);
            }
        }

        private void ApplyMultiAssignment(object instance, PropertyAssignment propertyAssignment, TrackingContext trackingContext, Property property)
        {
            foreach (var constructionNode in propertyAssignment.Children)
            {
                var value = Create(constructionNode, trackingContext);
                var compatibleValue = ToCompatibleValue(new Assignment(instance, property, value), trackingContext);
                Utils.UniversalAdd(compatibleValue.Property.GetValue(compatibleValue.Instance), compatibleValue.Value);
            }
        }

        private void ApplySingleAssignment(object instance, PropertyAssignment propertyAssignment, TrackingContext trackingContext, Property property)
        {
            object value;
            if (propertyAssignment.SourceValue == null)
            {
                var first = propertyAssignment.Children.First();
                value = CreateForChild(instance, property, first, trackingContext);
            }
            else
            {
                value = propertyAssignment.SourceValue;
            }

            var assignment = new Assignment(instance, property, value);
            var converted = ToCompatibleValue(assignment, trackingContext);
            PerformAssigment(converted, trackingContext);
        }

        protected virtual void PerformAssigment(Assignment converted, TrackingContext trackingContext)
        {
            if (converted.Property.PropertyType.IsCollection())
            {
                Utils.UniversalAdd(converted.Property.GetValue(converted.Instance), converted.Value);
            }
            else
            {
                converted.ExecuteAssignment();
                OnAssigmentExecuted(converted, trackingContext);
            }
        }

        protected void OnAssigmentExecuted(Assignment assignment, TrackingContext trackingContext)
        {
            var ambientPropertyAssignment = new AmbientPropertyAssignment
            {
                Property = assignment.Property,
                Value = assignment.Value
            };

            trackingContext.AmbientRegistrator.RegisterAssignment(ambientPropertyAssignment);
        }

        protected virtual Assignment ToCompatibleValue(Assignment assignment, TrackingContext trackingContext)
        {
            if (assignment.Value is string)
            {
                var compatibleValue = sourceValueConverter.GetCompatibleValue(new SuperContext(trackingContext, StaticContext), assignment);
                return assignment.ReplaceValue(compatibleValue);
            }
            else
            {
                return assignment;
            }
        }

        protected virtual object CreateForChild(object instance, Property property, ConstructionNode node, TrackingContext trackingContext)
        {
            return Create(node, trackingContext);
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