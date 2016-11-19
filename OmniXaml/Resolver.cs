namespace OmniXaml
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Glass.Core;
    using TypeLocation;

    public class Resolver : IResolver
    {
        private readonly ITypeDirectory typeDirectory;

        public Resolver(ITypeDirectory typeDirectory)
        {
            this.typeDirectory = typeDirectory;
        }

        public Member ResolveProperty(Type type, XElement element)
        {
            var nameLocalName = element.Name.LocalName;


            var dot = nameLocalName.IndexOf('.');
            var ownerName = nameLocalName.Take(dot).AsString();
            var propertyName = nameLocalName.Skip(dot + 1).AsString();

            var ownerType = LocateType(XName.Get(ownerName, element.Name.NamespaceName));

            if (ownerType == type)
                return Member.FromStandard(ownerType, propertyName);

            return Member.FromAttached(ownerType, propertyName);
        }

        public Member ResolveProperty(Type type, XAttribute attribute)
        {
            var nameLocalName = attribute.Name.LocalName;
            if (nameLocalName.Contains('.'))
            {
                var dot = nameLocalName.IndexOf('.');
                var ownerName = nameLocalName.Take(dot).AsString();
                var propertyName = nameLocalName.Skip(dot + 1).AsString();

                var xname = attribute.Name.NamespaceName == string.Empty
                    ? XName.Get(ownerName, attribute.Parent.Name.NamespaceName)
                    : XName.Get(ownerName, attribute.Name.NamespaceName);
                var ownerType = LocateType(xname);
                return Member.FromAttached(ownerType, propertyName);
            }
            return Member.FromStandard(type, nameLocalName);
        }

        public Type LocateType(XName typeName)
        {
            return typeDirectory.GetTypeByFullAddress(
                new Address
                {
                    Namespace = typeName.NamespaceName,
                    TypeName = typeName.LocalName
                });
        }
    }
}