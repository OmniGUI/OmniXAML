namespace OmniXaml.Metadata
{
    using System;
    using System.Linq.Expressions;
    using Glass.Core;

    public class GenericMetadata<T> : Metadata
    {
        public GenericMetadata<T> WithMemberDependency(Expression<Func<T, object>> property, Expression<Func<T, object>> dependsOn)
        {
            if (PropertyDependencies == null)
            {
                PropertyDependencies = new DependencyRegistrations();
            }

            PropertyDependencies.Add(new DependencyRegistration(property.GetFullPropertyName(), dependsOn.GetFullPropertyName()));
            return this;
        }

        public GenericMetadata<T> WithContentProperty(Expression<Func<T, object>> nameOfPropertySelector)
        {
            ContentProperty = nameOfPropertySelector.GetFullPropertyName();
            return this;
        }

        public GenericMetadata<T> WithRuntimeNameProperty(Expression<Func<T, object>> nameOfPropertySelector)
        {
            RuntimePropertyName = nameOfPropertySelector.GetFullPropertyName();
            return this;
        }

        public Metadata AsNonGeneric()
        {
            return new Metadata { RuntimePropertyName = RuntimePropertyName, PropertyDependencies  = PropertyDependencies};
        }

        public Metadata WithIsNamescope(bool isNamescope)
        {
            IsNamescope = isNamescope;
            return this;
        }

        public GenericMetadata<T> WithFragmentLoader(Expression<Func<T, object>> nameOfPropertySelector, IConstructionFragmentLoader fragmentLoader)
        {
            FragmentLoaderInfo = new FragmentLoaderInfo
            {
                PropertyName = nameOfPropertySelector.GetFullPropertyName(),
                Type = typeof(T),
                Loader = fragmentLoader,
            };

            return this;
        }
    }
}