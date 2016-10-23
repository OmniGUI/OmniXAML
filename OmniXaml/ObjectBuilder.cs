namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Core;
    using Metadata;

    public class ObjectBuilder : IObjectBuilder
    {
        protected ConstructionContext ConstructionContext { get; }
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;
        private readonly IInstanceLifecycleSignaler signaler;
        private readonly IMetadataProvider metadataProvider;



        public ObjectBuilder(ConstructionContext constructionContext)
        {
            ConstructionContext = constructionContext;
            creator = constructionContext.Creator;
            sourceValueConverter = constructionContext.SourceValueConverter;
            signaler = constructionContext.Signaler;
            metadataProvider = constructionContext.MetadataProvider;
        }

        public object Create(ConstructionNode node)
        {

            var instance = CreateInstance(node);
            signaler.BeforeAssigments(instance);
            ApplyAssignments(instance, node.Assignments);
            signaler.AfterAssigments(instance);
            return instance;
        }

        private object CreateInstance(ConstructionNode node)
        {
            return creator.Create(node.InstanceType);
        }

        private void ApplyAssignments(object instance, IEnumerable<PropertyAssignment> propertyAssignments)
        {
            foreach (var propertyAssignment in propertyAssignments)
            {
                ApplyAssignment(instance, propertyAssignment);
            }
        }

        private void ApplyAssignment(object instance, PropertyAssignment propertyAssignment)
        {
            EnsureValidAssigment(propertyAssignment);
            var property = propertyAssignment.Property;

            if (propertyAssignment.SourceValue != null)
            {
                var value = sourceValueConverter.GetCompatibleValue(property.PropertyType, propertyAssignment.SourceValue);
                property.SetValue(instance, value);
            }
            else
            {
                if (propertyAssignment.Children.Count() == 1)
                {
                    var first = propertyAssignment.Children.First();
                    var value = CreateForChild(instance, property, first);
                    var converted = Transform(new Assignment(instance, property, value));

                    Assign(converted);
                }
                else
                {
                    foreach (var constructionNode in propertyAssignment.Children)
                    {
                        var value = Create(constructionNode);
                        var converted = Transform(new Assignment(instance, property, value));
                        Utils.UniversalAdd(converted.Property.GetValue(converted.Instance), converted.Value);
                    }
                }
            }
        }

        protected virtual void Assign(Assignment converted)
        {
            converted.ExecuteAssignment();
        }

        protected virtual Assignment Transform(Assignment assignment)
        {
            return assignment;
        }

        protected virtual object CreateForChild(object instance, Property property, ConstructionNode node)
        {
            return Create(node);
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