namespace OmniXaml
{
    using System;

    public interface ISmartSourceValueConverter
    {
        (bool, object) TryConvert(string strValue, Type desiredTargetType);
    }
}