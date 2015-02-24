using System;
using System.Collections.Generic;

namespace AberratioReader
{
    public interface ITypeFactory
    {
        object Create(Type type);
        object Create(Type type, object[] args);
        bool CanLocate(Type type);
        IList<Type> LookupUserInjectableParameters(Type type, int parameterCount);
    }
}