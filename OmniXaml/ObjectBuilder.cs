namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ambient;
    using Glass.Core;

    public class ObjectBuilder : IObjectBuilder
    {
        protected ConstructionContext ConstructionContext { get; }
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;


        public ObjectBuilder(ConstructionContext constructionContext)
        {
            ConstructionContext = constructionContext;
            creator = constructionContext.Creator;
            sourceValueConverter = constructionContext.SourceValueConverter;
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
            trackingContext.Annotator.NewInstance(instance);
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

            if (propertyAssignment.SourceValue != null)
            {
                var value = sourceValueConverter.GetCompatibleValue(property.PropertyType, propertyAssignment.SourceValue);
                property.SetValue(instance, value);
                OnAssigmentExecuted(new Assignment(instance, property, value), trackingContext);
            }
            else
            {
                if (propertyAssignment.Children.Count() == 1)
                {
                    var first = propertyAssignment.Children.First();
                    var value = CreateForChild(instance, property, first, trackingContext);
                    var converted = Transform(new Assignment(instance, property, value));

                    Assign(converted, trackingContext);
                }
                else
                {
                    foreach (var constructionNode in propertyAssignment.Children)
                    {
                        var value = Create(constructionNode, trackingContext);
                        var converted = Transform(new Assignment(instance, property, value));
                        Utils.UniversalAdd(converted.Property.GetValue(converted.Instance), converted.Value);
                    }
                }
            }
        }

        protected virtual void Assign(Assignment converted, TrackingContext trackingContext)
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

        protected virtual Assignment Transform(Assignment assignment)
        {
            return assignment;
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