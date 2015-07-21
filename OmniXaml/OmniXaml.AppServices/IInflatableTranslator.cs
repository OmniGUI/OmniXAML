namespace OmniXaml.AppServices
{
    using System;
    using System.IO;

    public interface IInflatableTranslator
    {
        Type GetTypeFor(Uri uri);
        Stream GetStream(Type type);
    }
}