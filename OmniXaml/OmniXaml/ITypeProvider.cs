namespace OmniXaml
{
    using System;

    public interface ITypeProvider
    {
        Type GetType(string typeName, string clrNamespace, string assemblyName);
    }
}