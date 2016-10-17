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

        public ObjectBuilder(IInstanceCreator creator, ISourceValueConverter sourceValueConverter)
        {
            this.creator = creator;
            this.sourceValueConverter = sourceValueConverter;
        }

        public object Create(ConstructionNode node)
        {
            var instance = CreateInstance(node);
            ApplyAssignments(instance, node.Assignments);

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
                var values = propertyAssignment.Children.Select(Create);

                if (IsCollection(property.PropertyType))
                {
                    AssignValuesToCollection(values, instance, property);
                }
                else
                {
                    AssignFirstValueToNonCollection(instance, values.First(), property);
                }
            }
        }

        protected virtual void AssignFirstValueToNonCollection(object instance, object value, Property property)
        {           
            property.SetValue(instance, value);
        }      

        private void AssignValuesToCollection(IEnumerable<object> values, object instance, Property property)
        {
            var valueOfProperty = property.GetValue(instance);

            foreach (var value in values)
            {
                Utils.UniversalAdd(valueOfProperty, value);
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
    }
}