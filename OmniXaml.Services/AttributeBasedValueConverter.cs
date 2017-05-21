namespace OmniXaml.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Zafiro.Core;

    public class AttributeBasedValueConverter<TInput, TOutput> : IValueConverter<TInput, TOutput>
    {
        private readonly IDictionary<Type, Func<TInput, ConvertContext, (bool, TOutput)>> converterFuncs = new Dictionary<Type, Func<TInput, ConvertContext, (bool, TOutput)>>();

        public AttributeBasedValueConverter(IList<Assembly> assemblies)
        {
            AddConvertersFromAssemblies(assemblies);
        }

        private void AddConvertersFromAssemblies(IList<Assembly> assemblies)
        {
            var converterEntries =
                from type in assemblies.AllExportedTypes()
                from member in type.GetRuntimeFields()
                let attr = member.GetCustomAttribute<TypeConverterMember>()
                where attr != null
                where member.IsStatic
                select new { type, member, attr.SourceType };

            foreach (var converterEntry in converterEntries)
            {
                var staticFieldValue = converterEntry.member.GetValue(null);
                var converterFunc = (Func<TInput, ConvertContext, (bool, TOutput)>)staticFieldValue;
                converterFuncs.Add(converterEntry.SourceType, converterFunc);
            }
        }

        public (bool, object) Convert(TInput input, Type desiredTargetType, ConvertContext context)
        {
            if (converterFuncs.ContainsKey(desiredTargetType))
            {
                return converterFuncs[desiredTargetType](input, context);
            }

            return (false, null);
        }
    }

 
}