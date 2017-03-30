namespace OmniXaml.Services
{
    using System;
    using Zafiro.Core;

    public class DirectCompatibilitySourceValueConverter : IStringSourceValueConverter
    {
        public (bool, object) TryConvert(string strValue, Type desiredTargetType)
        {
            if (typeof(string).IsAssignableFrom(desiredTargetType))
            {
                return (true, strValue);
            }

            return (false, null);
        }
    }
}