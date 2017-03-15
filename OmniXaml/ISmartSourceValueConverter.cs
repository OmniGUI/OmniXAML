namespace OmniXaml
{
    using System;

    public interface ISmartSourceValueConverter
    {
        object Convert(string strValue, Type desiredTargetType);
    }
}