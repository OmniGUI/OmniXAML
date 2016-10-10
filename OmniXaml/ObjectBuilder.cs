namespace OmniXaml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ObjectBuilder
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
                        AssignValuesToNonCollection(instance, values, property);
                    }                    
                }                
            }
        }

        private static void AssignValuesToNonCollection(object instance, IEnumerable<object> values, Property standardProperty)
        {
            var value = values.First();
            standardProperty.SetValue(instance, value);
        }

        private void AssignValuesToCollection(IEnumerable<object> values, object instance, Property property)
        {
            var valueOfProperty = property.GetValue(instance);
            var collection = (IList) valueOfProperty;

            foreach (var value in values)
            {
                collection.Add(value);
            }
        }

        private bool IsCollection(Type propertyInfo)
        {
            var typeInfo = propertyInfo.GetTypeInfo();
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