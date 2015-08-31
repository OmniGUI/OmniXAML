namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Typing;

    public class XamlProtoInstructionParser : IProtoParser
    {
        private readonly IWiringContext wiringContext;
        private readonly ProtoInstructionBuilder instructionBuilder;
        private IXmlReader reader;

        public XamlProtoInstructionParser(IWiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
            instructionBuilder = new ProtoInstructionBuilder(wiringContext.TypeContext);
        }

        public IEnumerable<ProtoXamlInstruction> Parse(Stream stream)
        {
            reader = new XmlCompatibilityReader(stream);
            reader.Read();

            return ParseElement();
        }

        private IEnumerable<ProtoXamlInstruction> ParseEmptyElement(XamlType xamlType, NamespaceDeclaration namespaceDeclaration, AttributeFeed attributes)
        {
            var emptyElement = instructionBuilder.EmptyElement(xamlType.UnderlyingType, namespaceDeclaration);
            return CommonNodesOfElement(xamlType, emptyElement, attributes);
        }

        private IEnumerable<ProtoXamlInstruction> CommonNodesOfElement(XamlType owner, ProtoXamlInstruction elementToInject, AttributeFeed attributeFeed)
        {
            var attributes = attributeFeed;

            foreach (var instruction in attributes.PrefixRegistrations.Select(ConvertAttributeToNsPrefixDefinition)) yield return instruction;
            
            yield return elementToInject;

            foreach (var instruction in attributes.Directives.Select(ConvertDirective)) yield return instruction;
            foreach (var instruction in attributes.RawAttributes.Select(a => ConvertAttributeToNode(owner, a))) yield return instruction;
        }

        private IEnumerable<ProtoXamlInstruction> ParseExpandedElement(XamlType xamlType, NamespaceDeclaration namespaceDeclaration, AttributeFeed attributes)
        {
            var element = instructionBuilder.NonEmptyElement(xamlType.UnderlyingType, namespaceDeclaration);
            foreach (var instruction in CommonNodesOfElement(xamlType, element, attributes)) yield return instruction;

            reader.Read();

            foreach (var instruction in ParseInnerTextIfAny()) yield return instruction; 
            foreach (var instruction in ParseNestedElements(xamlType)) yield return instruction;

            yield return instructionBuilder.EndTag();
        }

        private IEnumerable<ProtoXamlInstruction> ParseNestedElements(XamlType xamlType)
        {
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                SkipWhitespaces();

                var isPropertyElement = reader.LocalName.Contains(".");
                if (isPropertyElement)
                {
                    foreach (var instruction in ParseNestedProperty(xamlType)) yield return instruction;

                    reader.Read();
                }
                else
                {
                    foreach (var instruction in ParseChildren()) yield return instruction;
                }
            }
        }

        private IEnumerable<ProtoXamlInstruction> ParseNestedProperty(XamlType xamlType)
        {
            var propertyLocator = PropertyLocator.Parse(reader.LocalName);
            var namespaceDeclaration = new NamespaceDeclaration(reader.Namespace, reader.Prefix);
            yield return instructionBuilder.NonEmptyPropertyElement(xamlType.UnderlyingType, propertyLocator.PropertyName, namespaceDeclaration);

            reader.Read();

            foreach (var p in ParseInnerTextIfAny()) yield return p;

            SkipWhitespaces();

            if (reader.NodeType != XmlNodeType.EndElement)
            {
                foreach (var instruction in ParseChildren()) { yield return instruction; }
            }

            yield return instructionBuilder.EndTag();
        }

        private IEnumerable<ProtoXamlInstruction> ParseInnerTextIfAny()
        {
            if (reader.NodeType == XmlNodeType.Text)
            {
                yield return instructionBuilder.Text(reader.Value);
                reader.Read();
            }
        }

        private IEnumerable<ProtoXamlInstruction> ParseChildren()
        {
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                foreach (var instruction in ParseElement()) { yield return instruction; }

                yield return instructionBuilder.Text();

                reader.Read();
                SkipWhitespaces();
            }
        }

        private void AssertValidElement()
        {
            if (!(reader.NodeType == XmlNodeType.Element && !reader.LocalName.Contains(".")))
            {
                throw new XamlParseException("The root should be an element.");
            }
        }

        private ProtoXamlInstruction ConvertAttributeToNode(XamlType containingType, UnprocessedAttribute rawAttribute)
        {
            MutableXamlMember member;

            if (rawAttribute.Locator.IsDotted)
            {
                var ownerName = rawAttribute.Locator.Owner.PropertyName;
                var ownerPrefix = rawAttribute.Locator.Owner.Prefix;

                var owner = wiringContext.TypeContext.GetByPrefix(ownerPrefix, ownerName);

                member = owner.GetAttachableMember(rawAttribute.Locator.PropertyName);
            }
            else
            {
                member = containingType.GetMember(rawAttribute.Locator.PropertyName);
            }

            var namespaceDeclaration = new NamespaceDeclaration(rawAttribute.Locator.Namespace, rawAttribute.Locator.Prefix);
            return instructionBuilder.Attribute(member, rawAttribute.Value, namespaceDeclaration);
        }

        private AttributeFeed GetAttributes()
        {
            AttributeReader attributeReader = new AttributeReader(reader);
            return attributeReader.Load();
        }

        private IEnumerable<ProtoXamlInstruction> ParseElement()
        {
            SkipWhitespaces();

            AssertValidElement();

            var attributes = GetAttributes();

            var prefixRegistrations = attributes.PrefixRegistrations.ToList();

            RegisterPrefixes(prefixRegistrations);

            var prefix = reader.Prefix;
            var ns = reader.Namespace;
            var namespaceDeclaration = new NamespaceDeclaration(ns, prefix);

            var childType = wiringContext.TypeContext.GetByPrefix(namespaceDeclaration.Prefix, reader.LocalName);
            

            if (reader.IsEmptyElement)
            {                
                foreach (var instruction in ParseEmptyElement(childType, namespaceDeclaration, attributes)) yield return instruction;
            }
            else
            {
                foreach (var instruction in ParseExpandedElement(childType, namespaceDeclaration, attributes)) yield return instruction;
            }
        }

        private void RegisterPrefixes(IEnumerable<NsPrefix> prefixRegistrations)
        {
            foreach (var prefixRegistration in prefixRegistrations)
            {
                var registration = new PrefixRegistration(prefixRegistration.Prefix, prefixRegistration.Namespace);
                wiringContext.TypeContext.RegisterPrefix(registration);
            }
        }

        private void SkipWhitespaces()
        {
            while (reader.NodeType == XmlNodeType.Whitespace)
            {
                reader.Read();
            }
        }

        private ProtoXamlInstruction ConvertDirective(RawDirective directive)
        {
            if (directive.Locator.PropertyName == "Key")
            {
                return instructionBuilder.Attribute(
                    CoreTypes.Key,
                    directive.Value,
                    new NamespaceDeclaration("http://schemas.microsoft.com/winfx/2006/xaml", "x"));
            }
            if (directive.Locator.PropertyName == "Name")
            {
                return instructionBuilder.Attribute(
                    CoreTypes.Name,
                    directive.Value,
                    new NamespaceDeclaration(null, "x"));
            }

            throw new XamlParseException($"Cannot handle the directive {directive.Locator.PropertyName}");
        }

        private ProtoXamlInstruction ConvertAttributeToNsPrefixDefinition(NsPrefix prefix)
        {
            return instructionBuilder.NamespacePrefixDeclaration(prefix.Prefix, prefix.Namespace);
        }
    }
}