namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Glass;

    public class Metadata
    {
        private readonly ISet<DependencyRegistration> propertyDependencies = new HashSet<DependencyRegistration>();

        protected ISet<DependencyRegistration> PropertyDependencies => propertyDependencies;

        public void SetMemberDependency(string property, string dependsOn)
        {
            PropertyDependencies.Add(new DependencyRegistration(property, dependsOn));
        }

        public IEnumerable<string> GetMemberDependencies(string name)
        {
            return from dependency in PropertyDependencies
                where dependency.Property == name
                select dependency.DependsOn;
        }

        public string RuntimePropertyName { get; protected set; }
    }

    public struct DependencyRegistration
    {
        public string Property { get; set; }
        public string DependsOn { get; set; }

        public DependencyRegistration(string property, string dependsOn)
        {
            Property = property;
            DependsOn = dependsOn;
        }
    }

    public class Metadata<T> : Metadata
    {
        public Metadata<T> WithMemberDependency(Expression<Func<T, object>> property, Expression<Func<T, object>> dependsOn)
        {
            PropertyDependencies.Add(new DependencyRegistration(property.GetFullPropertyName(), dependsOn.GetFullPropertyName()));
            return this;
        }

        public Metadata<T> WithRuntimeNameProperty(Expression<Func<T, object>> nameOfPropertySelector)
        {
            RuntimePropertyName = nameOfPropertySelector.GetFullPropertyName();
            return this;
        }        
    }
}