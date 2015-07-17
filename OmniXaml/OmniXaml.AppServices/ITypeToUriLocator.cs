namespace OmniXaml.AppServices
{
    using System;

    public interface ITypeToUriLocator
    {
        Uri GetUriFor(Type type);
        Type GetTypeFor(Uri uri);
    }
}