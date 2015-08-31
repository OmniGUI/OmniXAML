namespace OmniXaml
{
    using System.Diagnostics;
    using Typing;

    [DebuggerDisplay("{ToString()}")]
    public struct XamlInstruction
    {
        private readonly XamlInstructionType instructionType;
        private readonly object data;
        private readonly InternalNodeType internalNodeType;

        public XamlInstruction(XamlInstructionType instructionType)
        {
            this.instructionType = instructionType;
            internalNodeType = InternalNodeType.None;
            data = null;
        }

        public XamlInstruction(XamlInstructionType instructionType, object data)
            : this(instructionType)
        {
            this.data = data;
        }

        public XamlType XamlType
        {
            get
            {
                if (InstructionType == XamlInstructionType.StartObject)
                {
                    return (XamlType)data;
                }
                return null;
            }
        }

        public XamlInstructionType InstructionType => instructionType;

        public XamlMemberBase Member
        {
            get
            {
                if (InstructionType == XamlInstructionType.StartMember)
                {
                    return (XamlMemberBase)data;
                }

                return null;
            }
        }

        public object Value => InstructionType == XamlInstructionType.Value ? data : null;

        public NamespaceDeclaration NamespaceDeclaration
        {
            get
            {
                if (InstructionType == XamlInstructionType.NamespaceDeclaration)
                {
                    return (NamespaceDeclaration)data;
                }

                return null;
            }
        }

        public bool Equals(XamlInstruction other)
        {
            return instructionType == other.instructionType && Equals(data, other.data) && internalNodeType == other.internalNodeType;
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
                var hashCode = (int) instructionType;
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
            object[] nodeType = { InstructionType };

            var str = string.Format("{0}: ", nodeType);
            switch (InstructionType)
            {
                case XamlInstructionType.None:
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
                case XamlInstructionType.StartObject:
                    {
                        str = string.Concat(str, XamlType.Name);
                        return str;
                    }
                case XamlInstructionType.GetObject:
                case XamlInstructionType.EndObject:
                case XamlInstructionType.EndMember:
                    {
                        return str;
                    }
                case XamlInstructionType.StartMember:
                    {
                        str = string.Concat(str, Member.Name);
                        return str;
                    }
                case XamlInstructionType.Value:
                    {
                        str = string.Concat(str, Value.ToString());
                        return str;
                    }
                case XamlInstructionType.NamespaceDeclaration:
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