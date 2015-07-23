namespace OmniXaml.Parsers.ProtoParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Typing;

    public class ProtoParser
    {
        private readonly Queue<ProtoParserNode> nodes = new Queue<ProtoParserNode>();
        private readonly Context stack;
        private readonly ITypeContext typingCore;
        private ProtoParserNode currentNode;
        private TextBuffer textBuffer;
        private XmlReader xmlReader;

        public ProtoParser(ITypeContext typingCore)
        {
            this.typingCore = typingCore;
            stack = new Context();
        }

        private XamlType Type => currentNode.Type;

        private bool HasTextInBuffer
        {
            get
            {
                if (textBuffer != null)
                {
                    return !textBuffer.IsEmpty;
                }

                return false;
            }
        }

        private TextBuffer TextBuffer => textBuffer ?? (textBuffer = new TextBuffer());
        private NodeType NodeType => currentNode.NodeType;
        private string Namespace => currentNode.TypeNamespace;
        private string Prefix => currentNode.Prefix;
        private MutableXamlMember PropertyAttribute => currentNode.PropertyAttribute;
        private TextBuffer PropertyAttributeText => currentNode.PropertyAttributeText;
        private XamlMember PropertyElement => currentNode.PropertyElement;

        private void Read()
        {
            FillQueue();
            currentNode = nodes.Dequeue();
        }

        private void FillQueue()
        {
            if (nodes.Count != 0)
            {
                return;
            }

            ReadXmlFromReader();
        }

        private void ReadXmlFromReader()
        {
            while (nodes.Count == 0)
            {
                if (xmlReader.Read())
                {
                    ProcessCurrentXmlNode();
                }
                else
                {
                    ReadNone();
                }
            }
        }

        private void ReadNone()
        {
            nodes.Enqueue(new ProtoParserNode {NodeType = NodeType.None});
        }

        private void ProcessCurrentXmlNode()
        {
            switch (xmlReader.NodeType)
            {
                case XmlNodeType.None:
                    ReadNone();
                    break;
                case XmlNodeType.Element:
                    ReadElement();
                    break;
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                    ReadText();
                    break;
                case XmlNodeType.Whitespace:
                case XmlNodeType.SignificantWhitespace:
                    ReadWhitespace();
                    break;
                case XmlNodeType.EndElement:
                    ReadEndElement();
                    break;
            }
        }

        private void EnqueueAnyText()
        {
            if (HasTextInBuffer)
            {
                EnqueueTextNode();
            }

            ClearBufferedText();
        }

        private void EnqueueTextNode()
        {
            if (stack.Depth == 0 && TextBuffer.IsWhiteSpaceOnly)
            {
                return;
            }

            nodes.Enqueue(new ProtoParserNode {NodeType = NodeType.Text, TextContent = TextBuffer});
        }

        private void ClearBufferedText()
        {
            textBuffer = null;
        }

        private void ReadEndElement()
        {
            EnqueueAnyText();

            if (stack.CurrentProperty != null)
            {
                stack.CurrentProperty = null;
                stack.IsCurrentlyInsideContent = false;
            }
            else
            {
                stack.Pop();
            }

            nodes.Enqueue(new ProtoParserNode {NodeType = NodeType.EndTag});
        }

        private void ReadWhitespace()
        {
            TextBuffer.Append(xmlReader.Value, !stack.IsCurrentlyInsideContent);
        }

        private void ReadText()
        {
            TextBuffer.Append(xmlReader.Value, !stack.IsCurrentlyInsideContent);
            stack.IsCurrentlyInsideContent = true;
        }

        private void ReadElement()
        {
            EnqueueAnyText();

            var isEmptyElement = xmlReader.IsEmptyElement;
            var prefix = xmlReader.Prefix;
            var localName = xmlReader.LocalName;

            if (!XamlName.ContainsDot(localName))
            {
                ReadObjectElement(new XamlQualifiedName(prefix, localName), isEmptyElement);
            }
            else
            {
                var name = PropertyLocator.Parse(xmlReader.Name, xmlReader.NamespaceURI);
                if (stack.CurrentType == null)
                {
                    throw new XamlParseException("Property element with no parent!");
                }

                ReadPropertyElement(name, stack.CurrentType, isEmptyElement);
            }
        }

        private void ReadObjectElement(XamlName name, bool isEmptyTag)
        {
            var attributes = EnqueuePrefixDefinitionsAndGetTheRestOfAttributes();


            var node = new ProtoParserNode {Prefix = name.Prefix, IsEmptyTag = isEmptyTag};

            var namespaceUri = xmlReader.NamespaceURI;
            node.TypeNamespace = namespaceUri;

            var xamlAttributes = ReadObjectElementObject(namespaceUri, name.PropertyName, node, attributes);
            nodes.Enqueue(node);

            EnqueueAttributes(xamlAttributes);
        }

        private List<UnboundAttribute> EnqueuePrefixDefinitionsAndGetTheRestOfAttributes()
        {
            var allAttributes = ReadAllAttributes()
                .ToLookup(attribute => attribute.Type == AttributeType.Namespace);

            var withoutPrefixDeclarations = allAttributes[false].ToList();

            EnqueuePrefixDefinitions(allAttributes[true]);
            return withoutPrefixDeclarations;
        }

        private void EnqueuePrefixDefinitions(IEnumerable<UnboundAttribute> prefixDefinition)
        {
            foreach (var xamlBareAttribute in prefixDefinition)
            {
                EnqueuePrefixDefinition(xamlBareAttribute);
            }
        }

        private void EnqueueAttributes(IEnumerable<XamlAttribute> attributes)
        {
            foreach (var xamlBareAttribute in attributes)
            {
                EnqueueAttribute(xamlBareAttribute);
            }
        }

        private void EnqueueAttribute(XamlAttribute attribute)
        {
            var nodeForAttribute = new ProtoParserNode();
            switch (attribute.Type)
            {
                case AttributeType.CtorDirective:
                case AttributeType.Name:
                case AttributeType.Directive:
                    nodeForAttribute.NodeType = NodeType.Directive;
                    break;
                case AttributeType.Property:
                    nodeForAttribute.NodeType = NodeType.Attribute;
                    break;
                case AttributeType.AttachableProperty:
                    nodeForAttribute.NodeType = NodeType.Attribute;
                    break;
                default:
                    throw new ProtoParserException("The type of the attribute is unknown");
            }

            var property = attribute.Property;
            var convertCrlFtoLf = property == null;

            nodeForAttribute.PropertyAttribute = property;
            var xamlText = new TextBuffer();
            xamlText.Append(attribute.Value, false, convertCrlFtoLf);
            nodeForAttribute.PropertyAttributeText = xamlText;
            nodeForAttribute.Prefix = attribute.Locator.Prefix;

            nodes.Enqueue(nodeForAttribute);
        }

        private List<UnboundAttribute> ReadAllAttributes()
        {
            var allAttributes = new List<UnboundAttribute>();

            if (xmlReader.MoveToFirstAttribute())
            {
                do
                {
                    var name = xmlReader.Name;
                    var val = xmlReader.Value;
                    var propName = PropertyLocator.Parse(name);

                    if (propName == null)
                    {
                        throw new XamlParseException("InvalidXamlMemberName");
                    }

                    var attr = new UnboundAttribute(propName, val);
                    allAttributes.Add(attr);
                } while (xmlReader.MoveToNextAttribute());

                xmlReader.MoveToElement();
            }

            return allAttributes;
        }

        private void EnqueuePrefixDefinition(UnboundAttribute attr)
        {
            var xmlNsPrefixDefined = attr.XmlNsPrefixDefined;
            var xmlNsUriDefined = attr.XmlNsUriDefined;

            nodes.Enqueue(
                new ProtoParserNode
                {
                    NodeType = NodeType.PrefixDefinition,
                    Prefix = xmlNsPrefixDefined,
                    TypeNamespace = xmlNsUriDefined
                });
        }

        private IEnumerable<XamlAttribute> ReadObjectElementObject(string xmlns, string name, ProtoParserNode node, List<UnboundAttribute> xamlBareAttributes)
        {
            var xamlTypeName = new XamlTypeName(xmlns, name);
            node.Type = typingCore.GetWithFullAddress(xamlTypeName);

            var attributes = GetAttributes(xamlBareAttributes, node.Type).ToList();

            if (stack.Depth > 0)
            {
                stack.IsCurrentlyInsideContent = true;
            }

            if (!node.IsEmptyTag)
            {
                node.NodeType = NodeType.Element;
                stack.Push(node.Type, node.TypeNamespace);
            }
            else
            {
                node.NodeType = NodeType.EmptyElement;
            }

            return attributes;
        }

        private void ReadPropertyElement(PropertyLocator locator, XamlType tagType, bool isEmptyTag)
        {
            var xamlBareAttributes = EnqueuePrefixDefinitionsAndGetTheRestOfAttributes();

            var namespaceUri = xmlReader.NamespaceURI;
            var tagIsRoot = stack.Depth == 1;
            var dottedProperty = GetDottedProperty(tagType, locator);

            var node = new ProtoParserNode
            {
                Prefix = locator.Prefix,
                TypeNamespace = namespaceUri,
                IsEmptyTag = isEmptyTag
            };

            var fullyFledgedAttributes = GetAttributes(xamlBareAttributes, node.Type).ToList();

            if (stack.Depth > 0)
            {
                stack.IsCurrentlyInsideContent = false;
            }

            node.PropertyElement = dottedProperty;

            if (!node.IsEmptyTag)
            {
                stack.CurrentProperty = node.PropertyElement;
                node.NodeType = NodeType.PropertyElement;
            }
            else
            {
                node.NodeType = NodeType.EmptyPropertyElement;
            }

            nodes.Enqueue(node);

            EnqueueAttributes(fullyFledgedAttributes);
        }

        private XamlMember GetDottedProperty(XamlType tagType, PropertyLocator propLocator)
        {
            if (tagType == null)
            {
                throw new ArgumentNullException(nameof(tagType));
            }

            var xamlNamespace = ResolveXamlNameNs(propLocator);
            if (xamlNamespace == null)
            {
                throw new XamlParseException("PrefixNotFound");
            }

            var xamlTypeName = new XamlTypeName(xamlNamespace, propLocator.Owner.PropertyName);
            var xamlType = typingCore.GetWithFullAddress(xamlTypeName);
            return xamlType.GetMember(propLocator.PropertyName);
        }

        private static string ResolveXamlNameNs(XamlName name)
        {
            return name.Namespace;
        }

        private IEnumerable<XamlAttribute> GetAttributes(IEnumerable<UnboundAttribute> unboundAttributes, XamlType xamlType)
        {
            if (xamlType != null)
            {
                foreach (var xamlAttribute in unboundAttributes)
                {
                    yield return new XamlAttribute(xamlAttribute, xamlType, typingCore);
                }
            }
        }

        public IEnumerable<ProtoXamlNode> Parse(string xml)
        {
            using (xmlReader = XmlReader.Create(new StringReader(xml)))
            {
                foreach (var node in ExtractStates())
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<ProtoXamlNode> ExtractStates()
        {
            do
            {
                Read();
                yield return ExtractState();
            } while (NodeType != NodeType.None);
        }

        private ProtoXamlNode ExtractState()
        {
            return new ProtoXamlNode
            {
                XamlType = Type,
                Namespace = Namespace,
                NodeType = NodeType,
                PropertyAttribute = PropertyAttribute,
                Prefix = Prefix,
                PropertyElement = PropertyElement,
                PropertyAttributeText = PropertyAttributeText?.Text
            };
        }
    }
}