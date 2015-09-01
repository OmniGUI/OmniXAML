namespace OmniXaml.Services
{
    using System;
    using System.IO;

    public interface IInflatableTranslator
    {
        Type GetTypeFor(Uri uri);
        Stream GetInflationSourceStream(Type type);
    }
}