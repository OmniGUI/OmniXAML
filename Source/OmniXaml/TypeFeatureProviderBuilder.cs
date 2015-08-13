namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using TypeConversion;

    public class TypeFeatureProviderBuilder
    {
        private IContentPropertyProvider contentPropertyProvider;
        private ITypeConverterProvider converterProvider;

        public ITypeFeatureProvider Build()
        {
            return new TypeFeatureProvider(contentPropertyProvider, converterProvider);           
        }

        public TypeFeatureProviderBuilder FromAttributes(IEnumerable<Type> types)
        {
            contentPropertyProvider = ContentPropertyProvider.FromAttributes(types);
            converterProvider = TypeConverterProvider.FromAttributes(types);
            return this;
        }
    }
}
