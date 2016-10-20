namespace OmniXaml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;

    public class ObjectBuilder : IObjectBuilder
    {
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;
        private readonly IInstanceLifecycleSignaler signaler;

        public ObjectBuilder(IInstanceCreator creator, ISourceValueConverter sourceValueConverter, IInstanceLifecycleSignaler signaler)
        {
            this.creator = creator;
            this.sourceValueConverter = sourceValueConverter;
            this.signaler = signaler;
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
                var values = propertyAssignment.Children.Select(node => GatedCreate(instance, property, node));

                if (IsCollection(property.PropertyType))
                {
                    AssignValuesToCollection(values.Select(o => new AssignmentTarget(instance, property, o)), instance, property);
                }
                else
                {
                    OnPropertyAssignment(new AssignmentTarget(instance, property, values.First()));                    
                }
            }
        }

        protected  virtual object GatedCreate(object instance, Property property, ConstructionNode node)
        {
            return Create(node);
        }

        protected virtual void OnPropertyAssignment(AssignmentTarget assignmentTarget)
        {
            assignmentTarget.ExecuteAssignment();
        }

        private void AssignValuesToCollection(IEnumerable<AssignmentTarget> assignments, object instance, Property property)
        {
            var valueOfProperty = property.GetValue(instance);

            foreach (var assignmentTarget in assignments)
            {
                var converted = Transform(assignmentTarget);
                Utils.UniversalAdd(valueOfProperty, converted.Value);
            }
        }

        private bool IsCollection(Type type)
        {
            if (type == typeof(string))
            {
                return false;
            }

            var typeInfo = type.GetTypeInfo();
            return typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo);
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

        protected virtual AssignmentTarget Transform(AssignmentTarget assignmentTarget)
        {
            return assignmentTarget;
        }
    }
}