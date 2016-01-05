namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TypeConversion;
    using Typing;

    public class TypeFeatureProviderBuilder
    {
        private ITypeConverterProvider converterProvider;

        public ITypeFeatureProvider Build()
        {
            return new TypeFeatureProvider(converterProvider);           
        }

        public TypeFeatureProviderBuilder FromAttributes(IEnumerable<Type> types)
        {
            var listOfTypes = types as IList<Type> ?? types.ToList();
            
            converterProvider = new TypeConverterProvider().FillFromAttributes(listOfTypes);
            
            return this;
        }
    }
}
