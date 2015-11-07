namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TypeConversion;

    public class TypeFeatureProviderBuilder
    {
        private IContentPropertyProvider contentPropertyProvider;
        private ITypeConverterProvider converterProvider;
        private IRuntimeNameProvider runtimeNamePropertyProvider;

        public ITypeFeatureProvider Build()
        {
            return new TypeFeatureProvider(contentPropertyProvider, converterProvider, runtimeNamePropertyProvider);           
        }

        public TypeFeatureProviderBuilder FromAttributes(IEnumerable<Type> types)
        {
            var listOfTypes = types as IList<Type> ?? types.ToList();

            contentPropertyProvider = ContentPropertyProvider.FromAttributes(listOfTypes);
            converterProvider = TypeConverterProvider.FromAttributes(listOfTypes);
            runtimeNamePropertyProvider = RuntimeNamePropertyProvider.FromAttributes(listOfTypes);

            return this;
        }
    }
}
