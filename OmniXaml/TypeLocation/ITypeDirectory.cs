namespace OmniXaml.Tests.Namespaces
{
    using System;
    using TypeLocation;

    public interface ITypeDirectory
    {
        Type GetTypeByFullAddress(Address address);
    }
}