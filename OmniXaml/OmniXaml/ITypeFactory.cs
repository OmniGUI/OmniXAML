namespace OmniXaml
{
    using System;
    using System.Collections.Generic;

    public interface ITypeFactory
    {
        // ReSharper disable once UnusedMember.Global
        object Create(Type type);
        // ReSharper disable once UnusedMember.Global
        object Create(Type type, object[] args);
        // ReSharper disable once UnusedMember.Global
        bool CanLocate(Type type);
        // ReSharper disable once UnusedMember.Global
        IList<Type> LookupUserInjectableParameters(Type type, int parameterCount);
    }
}