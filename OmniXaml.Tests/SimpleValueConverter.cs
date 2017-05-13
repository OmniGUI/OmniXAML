using System;
using System.ComponentModel;
using OmniXaml.ReworkPhases;

namespace OmniXaml.Tests
{
    internal class SimpleValueConverter : IStringSourceValueConverter
    {
        public (bool, object) TryConvert(string strValue, Type desiredTargetType, ConvertContext context)
        {
            var convertFrom = TypeDescriptor.GetConverter(desiredTargetType).ConvertFrom(strValue);
            return (true, convertFrom);
        }
    }
}