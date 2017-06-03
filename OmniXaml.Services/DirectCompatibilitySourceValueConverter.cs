namespace OmniXaml.Services
{
    using System;
    using Zafiro.Core;

    public class DirectCompatibilitySourceValueConverter : IStringSourceValueConverter
    {
        public (bool, object) Convert(string strValue, Type desiredTargetType, ConvertContext context)
        {
            if (typeof(string).IsAssignableFrom(desiredTargetType))
            {
                return (true, strValue);
            }

            return (false, null);
        }
    }
}