using System;

namespace AberratioReader
{
    public interface ITypeProvider
    {
        Type GetType(string typeName, string clrNamespace, string assemblyName);
    }
}