namespace OmniXaml
{
    using System;

    public interface IInstanceCreator
    {
        object Create(Type type);
    }
}