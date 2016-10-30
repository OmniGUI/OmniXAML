namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Core;

    public class ObjectBuilder : IObjectBuilder
    {
        protected ConstructionContext ConstructionContext { get; }
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;
        private readonly IInstanceLifecycleSignaler signaler;


        public ObjectBuilder(ConstructionContext constructionContext)
        {
            ConstructionContext = constructionContext;
            creator = constructionContext.Creator;
            sourceValueConverter = constructionContext.SourceValueConverter;
            signaler = constructionContext.Signaler;
        }

        public object Create(ConstructionNode node, object instance, CreationContext creationContext)
        {
            ApplyAssignments(instance, node.Assignments, creationContext);            
            return instance;
        }

        public object Create(ConstructionNode node, CreationContext creationContext)
        {
            var instance = CreateInstance(node, creationContext);
            signaler.BeforeAssigments(instance);
            ApplyAssignments(instance, node.Assignments, creationContext);
            signaler.AfterAssigments(instance);
            return instance;
        }

        private object CreateInstance(ConstructionNode node, CreationContext creationContext)
        {
            var instance = creator.Create(node.InstanceType);
            creationContext.Annotator.NewInstance(instance);
            if (node.Name != null)
            {
                creationContext.Annotator.RegisterName(node.Name, instance);
            }
            return instance;
        }

        private void ApplyAssignments(object instance, IEnumerable<PropertyAssignment> propertyAssignments, CreationContext creationContext)
        {
            foreach (var propertyAssignment in propertyAssignments)
            {
                ApplyAssignment(instance, propertyAssignment, creationContext);
            }
        }

        private void ApplyAssignment(object instance, PropertyAssignment propertyAssignment, CreationContext creationContext)
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
                    var value = CreateForChild(instance, property, first, creationContext);
                    var converted = Transform(new Assignment(instance, property, value));

                    Assign(converted);
                }
                else
                {
                    foreach (var constructionNode in propertyAssignment.Children)
                    {
                        var value = Create(constructionNode, creationContext);
                        var converted = Transform(new Assignment(instance, property, value));
                        Utils.UniversalAdd(converted.Property.GetValue(converted.Instance), converted.Value);
                    }
                }
            }
        }

        protected virtual void Assign(Assignment converted)
        {
            if (converted.Property.PropertyType.IsCollection())
            {
                Utils.UniversalAdd(converted.Property.GetValue(converted.Instance), converted.Value);
            }
            else
            {
                converted.ExecuteAssignment();
            }
        }

        protected virtual Assignment Transform(Assignment assignment)
        {
            return assignment;
        }

        protected virtual object CreateForChild(object instance, Property property, ConstructionNode node, CreationContext creationContext)
        {
            return Create(node, creationContext);
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

    public interface IAmbientRegistrator
    {
    }
}