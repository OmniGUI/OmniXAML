namespace OmniXaml
{
    using System;

    public interface ISourceValueConverter
    {
        object GetCompatibleValue(ValueContext valueContext);
    }
}