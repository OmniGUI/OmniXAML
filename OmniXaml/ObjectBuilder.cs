namespace OmniXaml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Glass.Core;
    using Metadata;

    public class ObjectBuilder : IObjectBuilder
    {
        private readonly IInstanceCreator creator;
        private readonly ISourceValueConverter sourceValueConverter;
        private readonly IInstanceLifecycleSignaler signaler;
        private readonly IMetadataProvider metadataProvider;

        public ObjectBuilder(IInstanceCreator creator, ISourceValueConverter sourceValueConverter, IInstanceLifecycleSignaler signaler, IMetadataProvider metadataProvider)
        {
            this.creator = creator;
            this.sourceValueConverter = sourceValueConverter;
            this.signaler = signaler;
            this.metadataProvider = metadataProvider;
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

        protected Assignment Transform(Assignment assignment)
        {
            var me = assignment.Value as IMarkupExtension;
            if (me != null)
            {
                var value = me.GetValue(null);
                assignment = assignment.ChangeValue(value);
            }

            return assignment;
        }

        protected object CreateForChild(object instance, Property property, ConstructionNode node)
        {
            var metadata = metadataProvider.Get(instance.GetType());
            var fragmentLoaderInfo = metadata.FragmentLoaderInfo;

            if (fragmentLoaderInfo != null && instance.GetType() == fragmentLoaderInfo.Type && property.PropertyName == fragmentLoaderInfo.PropertyName)
            {
                return fragmentLoaderInfo.Loader.Load(node, this);
            }
            else
            {
                return Create(node);
            }
        }

        //protected virtual object CreateForChild(object instance, Property property, ConstructionNode node)
        //{
        //    return Create(node);
        //}

        //protected virtual void OnPropertyAssignment(Assignment assignment)
        //{
        //    assignment.ExecuteAssignment();
        //}

        private void AssignValuesToCollection(IEnumerable<Assignment> assignments, object instance, Property property)
        {
            var valueOfProperty = property.GetValue(instance);

            foreach (var assignmentTarget in assignments)
            {
                var converted = Transform(assignmentTarget);
                Utils.UniversalAdd(valueOfProperty, converted.Value);
            }
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