namespace OmniXaml.TypeLocation
{
    using System;

    public interface ITypeDirectory
    {
        Type GetTypeByFullAddress(Address address);
    }
}