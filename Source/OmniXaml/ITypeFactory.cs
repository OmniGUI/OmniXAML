namespace OmniXaml
{
    using System;

    public interface ITypeFactory
    {
        // ReSharper disable once UnusedMember.Global
        object Create(Type type);
        // ReSharper disable once UnusedMember.Global
        object Create(Type type, params object[] args);
        // ReSharper disable once UnusedMember.Global
        bool CanLocate(Type type);        
    }
}