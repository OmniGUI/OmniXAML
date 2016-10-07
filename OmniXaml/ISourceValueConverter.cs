namespace OmniXaml
{
    using System;

    public interface ISourceValueConverter
    {
        object GetCompatibleValue(Type targetType, string sourceValue);
    }
}