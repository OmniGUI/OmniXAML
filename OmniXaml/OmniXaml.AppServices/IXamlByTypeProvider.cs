namespace OmniXaml.AppServices
{
    using System;

    public interface IXamlByTypeProvider
    {
        Uri GetUriFor(Type type);
    }
}