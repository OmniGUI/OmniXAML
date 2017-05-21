namespace OmniXaml
{
    using System;

    public interface IValueConverter<TInput, TOutput>
    {
        (bool, object) Convert(TInput strValue, Type desiredTargetType, ConvertContext context = null);
    }
}