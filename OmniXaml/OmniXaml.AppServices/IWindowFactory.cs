namespace OmniXaml.AppServices
{
    using System;

    public interface IWindowFactory
    {
        T Create<T>(Uri uri);
        object Create(Type type, Uri uri);
    }
}