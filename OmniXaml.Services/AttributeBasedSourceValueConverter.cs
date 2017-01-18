using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glass.Core;
using OmniXaml.Attributes;

namespace OmniXaml.Services
{
    public class AttributeBasedSourceValueConverter : ISourceValueConverter
    {
        private readonly Dictionary<Type, Func<ConverterValueContext, object>> standardConverters = new Dictionary<Type, Func<ConverterValueContext, object>>();

        public AttributeBasedSourceValueConverter(IList<Assembly> assemblies)
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
                select new {type, member, attr.SourceType};

            foreach (var converterEntry in converterEntries)
            {
                var staticFieldValue = converterEntry.member.GetValue(null);
                var converterFunc = (Func<ConverterValueContext, object>) staticFieldValue;
                standardConverters.Add(converterEntry.SourceType, converterFunc);
            }
        }

        public object GetCompatibleValue(ConverterValueContext valueContext)
        {
            var targetType = valueContext.TargetType;
            var sourceValue = valueContext.Value as string;

            if (sourceValue == null)
            {
                return valueContext.Value;
            }


            Func<ConverterValueContext, object> converter;
            if (standardConverters.TryGetValue(targetType, out converter))
            {
                return converter(valueContext);
            }

            object result;

            if (BuiltInConversionParser.TryParse(targetType, sourceValue, out result))
            {
                return result;
            }

            var delegateParent = valueContext.BuildContext.AmbientRegistrator.Instances.FirstOrDefault();
            if (DelegateParser.TryParse(sourceValue, targetType, delegateParent, out result))
            {
                return result;
            }

            return valueContext.Value;
        }        
    }
}