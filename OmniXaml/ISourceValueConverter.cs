namespace OmniXaml
{
    using System;

    public interface ISourceValueConverter
    {
        object GetCompatibleValue(SuperContext superContext, Type targetType, string sourceValue);
    }
}