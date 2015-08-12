namespace OmniXaml
{
    using System.Diagnostics;
    using Typing;

    [DebuggerDisplay("{ToString()}")]
    public struct XamlInstruction
    {
        private readonly XamlNodeType nodeType;
        private readonly object data;
        private readonly InternalNodeType internalNodeType;

        public XamlInstruction(XamlNodeType nodeType)
        {
            this.nodeType = nodeType;
            internalNodeType = InternalNodeType.None;
            data = null;
        }

        public XamlInstruction(XamlNodeType nodeType, object data)
            : this(nodeType)
        {
            this.data = data;
        }

        public XamlType XamlType
        {
            get
            {
                if (NodeType == XamlNodeType.StartObject)
                {
                    return (XamlType)data;
                }
                return null;
            }
        }

        public XamlNodeType NodeType => nodeType;

        public XamlMemberBase Member
        {
            get
            {
                if (NodeType == XamlNodeType.StartMember)
                {
                    return (XamlMemberBase)data;
                }

                return null;
            }
        }

        public object Value => NodeType == XamlNodeType.Value ? data : null;

        public NamespaceDeclaration NamespaceDeclaration
        {
            get
            {
                if (NodeType == XamlNodeType.NamespaceDeclaration)
                {
                    return (NamespaceDeclaration)data;
                }

                return null;
            }
        }

        public bool Equals(XamlInstruction other)
        {
            return nodeType == other.nodeType && Equals(data, other.data) && internalNodeType == other.internalNodeType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is XamlInstruction && Equals((XamlInstruction) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) nodeType;
                hashCode = (hashCode*397) ^ (data?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (int) internalNodeType;
                return hashCode;
            }
        }

        public enum InternalNodeType
        {
            EndOfAttributes,
            StartOfStream,
            EndOfStream,
            None
        }

        public override string ToString()
        {
            object[] nodeType = { NodeType };

            var str = string.Format("{0}: ", nodeType);
            switch (NodeType)
            {
                case XamlNodeType.None:
                    {
                        switch (internalNodeType)
                        {
                            case InternalNodeType.StartOfStream:
                                {
                                    str = string.Concat(str, "Start Of Stream");
                                    return str;
                                }
                            case InternalNodeType.EndOfStream:
                                {
                                    str = string.Concat(str, "End Of Stream");
                                    return str;
                                }
                            case InternalNodeType.EndOfAttributes:
                                {
                                    str = string.Concat(str, "End Of Attributes");
                                    return str;
                                }
                            default:
                                {
                                    return str;
                                }
                        }
                    }
                case XamlNodeType.StartObject:
                    {
                        str = string.Concat(str, XamlType.Name);
                        return str;
                    }
                case XamlNodeType.GetObject:
                case XamlNodeType.EndObject:
                case XamlNodeType.EndMember:
                    {
                        return str;
                    }
                case XamlNodeType.StartMember:
                    {
                        str = string.Concat(str, Member.Name);
                        return str;
                    }
                case XamlNodeType.Value:
                    {
                        str = string.Concat(str, Value.ToString());
                        return str;
                    }
                case XamlNodeType.NamespaceDeclaration:
                    {
                        str = string.Concat(str, NamespaceDeclaration.ToString());
                        return str;
                    }
                default:
                    {
                        return str;
                    }
            }
        }
    }
}