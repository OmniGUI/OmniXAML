namespace OmniXaml.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Zafiro.Core;

    public class AttributeBasedSmartSourceValueConverter<TInput, TOutput> : ISmartSourceValueConverter<TInput, TOutput>
    {
        private readonly IDictionary<Type, Func<TInput, TOutput>> converterFuncs = new Dictionary<Type, Func<TInput, TOutput>>();

        public AttributeBasedSmartSourceValueConverter(IList<Assembly> assemblies)
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
                var converterFunc = (Func<TInput, TOutput>)staticFieldValue;
                converterFuncs.Add(converterEntry.SourceType, converterFunc);
            }
        }

        public (bool, object) TryConvert(TInput input, Type desiredTargetType)
        {
            if (converterFuncs.ContainsKey(desiredTargetType))
            {
                return (true, converterFuncs[desiredTargetType](input));
            }

            return (false, null);
        }
    }
}