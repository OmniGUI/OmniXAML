namespace OmniXaml.Tests.Rework
{
    using System;
    using System.ComponentModel;

    internal class SimpleValueConverter : IStringSourceValueConverter
    {
        public (bool, object) TryConvert(string strValue, Type desiredTargetType)
        {
            var convertFrom = TypeDescriptor.GetConverter(desiredTargetType).ConvertFrom(strValue);
            return (true, convertFrom);
        }
    }
}