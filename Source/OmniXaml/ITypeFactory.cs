namespace OmniXaml
{
    using System;

    public interface ITypeFactory
    {
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once UnusedMember.Global
        object Create(Type type, params object[] args);
        // ReSharper disable once UnusedMember.Global
        bool CanCreate(Type type);        
    }
}