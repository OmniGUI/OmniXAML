namespace OmniXaml
{
    using System;
    using System.Xml.Linq;

    public interface IResolver
    {
        Member ResolveProperty(Type type, XElement element);
        Member ResolveProperty(Type type, XAttribute attribute);
        Type LocateType(XName typeXName);
        Type LocateMarkupExtension(XName typeXName);
        Type LocateTypeForClassDirective(Type constructionType, string classDirectiveValue);
    }
}