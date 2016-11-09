namespace OmniXaml
{
    using System;

    public interface IConverterContextFactory
    {
        ConverterValueContext CreateConverterContext(Type type, object value, BuildContext buildContext);
    }
}