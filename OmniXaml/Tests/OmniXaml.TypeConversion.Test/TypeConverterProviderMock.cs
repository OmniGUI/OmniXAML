using OmniXaml.TypeConversion;

namespace Perspex.Markup.Test
{
    using System;
    using System.Collections.Generic;

    public class TypeConverterProviderMock : ITypeConverterProvider
    {
        private ITypeConverter assignedConverter;


        public ITypeConverter GetTypeConverter(Type getType)
        {
            return assignedConverter;
        }

        public void Register(Type type, ITypeConverter converter)
        {
            assignedConverter = converter;
        }

        public void AddCatalog(IDictionary<Type, ITypeConverter> typeConverters)
        {
            throw new NotImplementedException();
        }
    }
}