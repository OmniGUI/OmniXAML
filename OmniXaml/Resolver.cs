namespace OmniXaml
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Glass.Core;
    using TypeLocation;

    public class Resolver : IResolver
    {
        private const string ExtensionSuffix = "Extension";
        private readonly ITypeDirectory typeDirectory;
        private static Regex s_classNameRegex = new Regex(@"(?<class>\w+\.\w+|\w+);assembly=(?<assembly>\w+\.\w+|\w+)");

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
                    ? XName.Get(ownerName, attribute.Parent.GetDefaultNamespace().NamespaceName)
                    : XName.Get(ownerName, attribute.Name.NamespaceName);

                var ownerType = LocateType(xname);
                return Member.FromAttached(ownerType, propertyName);
            }
            return Member.FromStandard(type, nameLocalName);
        }

        public Type LocateType(XName typeXName)
        {
            var ns = typeXName.NamespaceName;
            var typeName = typeXName.LocalName;

            var type = typeDirectory.GetTypeByFullAddress(new Address(ns, typeName));

            if (type == null)
            {
                var extensionType = typeDirectory.GetTypeByFullAddress(new Address(ns, typeName + ExtensionSuffix));

                if ((extensionType != null) && extensionType.IsExtension())
                    type = extensionType;
            }

            if (type == null)
                throw new TypeNotFoundException($"Cannot not find the type {typeXName}");

            return type;
        }

        public Type LocateMarkupExtension(XName typeXName)
        {
            var ns = typeXName.NamespaceName;

            var typeName = typeXName.LocalName;
            var typeNameWithSuffix = typeName + ExtensionSuffix;

            var typeLookup = new[]
            {
                typeDirectory.GetTypeByFullAddress(new Address(ns, typeName)),
                typeDirectory.GetTypeByFullAddress(new Address(ns, typeNameWithSuffix))
            };

            var candidates = (from r in typeLookup
                where r != null
                select r).ToList();

            var markupExtensions = from type in candidates
                where type.IsExtension()
                select type;

            var extensions = markupExtensions as Type[] ?? markupExtensions.ToArray();

            if (!candidates.Any())
                throw new TypeNotFoundException(
                    $@"Cannot find a Markup Extension for ""{typeXName}"". We haven't found any type that is named either {typeName} or {typeNameWithSuffix}.");


            if (candidates.Any() && !extensions.Any())
            {
                var candidatesMessage = string.Join(",", candidates.Select(type => type.Name));
                throw new TypeNotFoundException(
                    $@"Cannot find a Markup Extension for ""{typeXName}"". We found {candidates.Count}: {candidatesMessage}, but none of them is a Markup Extension.");
            }

            return extensions.First();
        }

        public Type LocateTypeForClassDirective(Type constructionType, string classDirectiveValue)
        {
            //var assembly = constructionType.GetTypeInfo().Assembly;
            var match = s_classNameRegex.Match(classDirectiveValue);

            if (!match.Success)
            {
                throw new TypeNotFoundException($"Cannot locate type {classDirectiveValue} specified in the x:Class directive.");
            }

            var className = match.Groups["class"].Value;
            var assembly = Assembly.Load(new AssemblyName(match.Groups["assembly"].Value));
            
            return assembly.GetType(className);
        }
    }
}