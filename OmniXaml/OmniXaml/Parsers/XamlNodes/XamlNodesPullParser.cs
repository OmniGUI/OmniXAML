namespace OmniXaml.Parsers.XamlNodes
{
    using System;
    using System.Collections.Generic;
    using MarkupExtensions;
    using ProtoParser;
    using Sprache;
    using Typing;

    public class XamlNodesPullParser
    {
        private readonly WiringContext wiringContext;
        private IEnumerator<ProtoXamlNode> nodeStream;
        private bool EndOfStream { get; set; }

        public XamlNodesPullParser(WiringContext wiringContext)
        {
            this.wiringContext = wiringContext;
        }

        public IEnumerable<XamlNode> Parse(IEnumerable<ProtoXamlNode> protoNodes)
        {
            nodeStream = protoNodes.GetEnumerator();
            SetNextNode();

            foreach (var prefix in ParsePrefixDefinitions()) yield return prefix;
            foreach (var element in ParseElements()) yield return element;
        }

        private IEnumerable<XamlNode> ParseElements(XamlMember hostingProperty = null)
        {
            SkipTextNodes();
            if (hostingProperty != null)
            {
                yield return Inject.StartOfMember(hostingProperty);
            }

            while (CurrentNodeIsElement && !EndOfStream)
            {
                switch (nodeStream.Current.NodeType)
                {
                    case NodeType.Element:
                        foreach (var xamlNode in ParseNonEmptyElement()) yield return xamlNode;

                        break;
                    case NodeType.EmptyElement:
                        foreach (var xamlNode in ParseEmptyElement()) yield return xamlNode;
                        break;
                }

                // There may be text nodes after each element. Skip all of them.
                SkipTextNodes();
            }

            if (hostingProperty != null)
            {
                yield return Inject.EndOfMember();
            }
        }

        private bool CurrentNodeIsElement => CurrentNodeType == NodeType.Element || CurrentNodeType == NodeType.EmptyElement;

        private void SkipTextNodes()
        {
            while (CurrentNodeType == NodeType.Text)
            {
                SetNextNode();
            }
        }

        private NodeType CurrentNodeType => nodeStream.Current?.NodeType ?? NodeType.None;

        private IEnumerable<XamlNode> ParseEmptyElement()
        {
            yield return Inject.StartOfObject(nodeStream.Current.XamlType);

            SetNextNode();

            foreach (var member in ParseMembersOfObject()) yield return member;

            yield return Inject.EndOfObject();

            if (CurrentNodeType == NodeType.Text)
            {
                SetNextNode();
            }            
        }

        private IEnumerable<XamlNode> ParseNonEmptyElement()
        {
            yield return Inject.StartOfObject(nodeStream.Current.XamlType);
            var parentType = nodeStream.Current.XamlType;

            SetNextNode();

            foreach (var xamlNode in ParseMembersOfObject()) { yield return xamlNode; }
            foreach (var xamlNode in ParseContentPropertyIfAny(parentType)) { yield return xamlNode; }

            SkipTextNodes();

            foreach (var xamlNode in ParseNestedProperties(parentType)) { yield return xamlNode; }

            yield return Inject.EndOfObject();

            ReadEndTag();
        }

        private void ReadEndTag()
        {
            SkipTextNodes();

            if (CurrentNodeType != NodeType.EndTag)
            {
                throw new XamlParseException("Expected End Tag");
            }

            SetNextNode();
        }

        private XamlMember GetContentProperty(XamlType parentType)
        {
            var propertyName = wiringContext.ContentPropertyProvider.GetContentPropertyName(parentType.UnderlyingType);

            if (propertyName == null)
            {
                return null;
            }

            var member = wiringContext.TypeContext.GetXamlType(parentType.UnderlyingType).GetMember(propertyName);
            return member;
        }

        private bool IsNestedPropertyImplicit => CurrentNodeType != NodeType.PropertyElement && CurrentNodeType != NodeType.EmptyPropertyElement &&
                                                 CurrentNodeType != NodeType.EndTag;

        private IEnumerable<XamlNode> ParseNestedProperties(XamlType parentType)
        {
            while (CurrentNodeType == NodeType.PropertyElement || CurrentNodeType == NodeType.EmptyPropertyElement)
            {
                var member = nodeStream.Current.PropertyElement;
                if (member.Type.IsCollection)
                {
                    SetNextNode();
                    foreach (var xamlNode in ParseCollectionInsideThisProperty(member))
                    {
                        yield return xamlNode;
                    }
                }
                else
                {
                    foreach (var xamlNode in ParseNestedProperty(member))
                    {
                        yield return xamlNode;
                    }
                }

                // After and EndTag, there could be a ContentProperty! so we consider parsing it.
                if (CurrentNodeType == NodeType.EndTag)
                {
                    SetNextNode();
                    foreach (var xamlNode in ParseContentPropertyIfAny(parentType)) yield return xamlNode;
                }
            }
        }

        private IEnumerable<XamlNode> ParseContentPropertyIfAny(XamlType parentType)
        {
            if (IsNestedPropertyImplicit)
            {
                var contentProperty = GetContentProperty(parentType);
                if (contentProperty == null)
                {
                    throw new InvalidOperationException($"Cannot get the content property for the type {parentType}");
                }

                if (contentProperty.Type.IsCollection)
                {
                    foreach (var xamlNode in ParseCollectionInsideThisProperty(contentProperty)) { yield return xamlNode; }
                }
                else
                {
                    foreach (var xamlNode in ParseElements(contentProperty)) { yield return xamlNode; }
                }
            }
        }

        private void SetNextNode()
        {
            if (EndOfStream)
            {
                throw new XamlParseException("The end of the stream has already been reached!");
            }

            EndOfStream = !nodeStream.MoveNext();           
        }

        private IEnumerable<XamlNode> ParseCollectionInsideThisProperty(XamlMember member)
        {
            yield return Inject.StartOfMember(member);
            yield return Inject.GetObject();
            yield return Inject.Items();

            foreach (var xamlNode in ParseElements()) yield return xamlNode;

            yield return Inject.EndOfMember();
            yield return Inject.EndOfObject();
            yield return Inject.EndOfMember();
        }

        private IEnumerable<XamlNode> ParseNestedProperty(XamlMember member)
        {
            yield return Inject.StartOfMember(member);

            SetNextNode();

            foreach (var xamlNode in ParseInnerContentOfNestedProperty()) yield return xamlNode;

            yield return Inject.EndOfMember();
        }

        private IEnumerable<XamlNode> ParseInnerContentOfNestedProperty()
        {
            if (CurrentNodeType == NodeType.Text)
            {
                yield return Inject.Value(nodeStream.Current.Text);
            }
            else
            {
                foreach (var xamlNode in ParseElements())
                {
                    yield return xamlNode;
                }
            }
        }

        private IEnumerable<XamlNode> ParseMembersOfObject()
        {
            while (CurrentNodeType == NodeType.Attribute && !EndOfStream)
            {
                var protoXamlNode = nodeStream.Current;
                var valueOfMember = protoXamlNode.PropertyAttributeText;

                yield return Inject.StartOfMember(protoXamlNode.PropertyAttribute);

                if (IsMarkupExtension(valueOfMember))
                {
                    foreach (var xamlNode in ParseMarkupExtension(valueOfMember)) yield return xamlNode;
                }
                else
                {
                    yield return Inject.Value(valueOfMember);
                }

                yield return Inject.EndOfMember();

                SetNextNode();
            }
        }

        private IEnumerable<XamlNode> ParseMarkupExtension(string valueOfMember)
        {
            var tree = MarkupExtensionParser.MarkupExtension.Parse(valueOfMember);
            var markupExtensionConverter = new MarkupExtensionNodeToXamlNodesConverter(wiringContext);
            return markupExtensionConverter.Convert(tree);
        }

        private IEnumerable<XamlNode> ParsePrefixDefinitions()
        {
            while (CurrentNodeType == NodeType.PrefixDefinition)
            {
                var protoXamlNode = nodeStream.Current;
                yield return Inject.PrefixDefinitionOfNamespace(protoXamlNode);
                SetNextNode();
            }
        }

        private static bool IsMarkupExtension(string text)
        {
            return text.Length > 3 && text.StartsWith("{") && text.EndsWith("}");
        }
    }
}