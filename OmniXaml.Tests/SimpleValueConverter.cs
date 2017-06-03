using System;
using System.ComponentModel;

namespace OmniXaml.Tests
{
    internal class SimpleValueConverter : IStringSourceValueConverter
    {
        public (bool, object) Convert(string strValue, Type desiredTargetType, ConvertContext context)
        {
            var convertFrom = TypeDescriptor.GetConverter(desiredTargetType).ConvertFrom(strValue);
            return (true, convertFrom);
        }
    }
}