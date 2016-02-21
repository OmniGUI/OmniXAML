namespace OmniXaml
{
    using System.Diagnostics;
    using Typing;

    [DebuggerDisplay("{ToString()}")]
    public struct Instruction
    {
        private readonly InstructionType instructionType;
        private readonly object data;
        private readonly InternalNodeType internalNodeType;

        public Instruction(InstructionType instructionType)
        {
            this.instructionType = instructionType;
            internalNodeType = InternalNodeType.None;
            data = null;
        }

        public Instruction(InstructionType instructionType, object data)
            : this(instructionType)
        {
            this.data = data;
        }

        public XamlType XamlType
        {
            get
            {
                if (InstructionType == OmniXaml.InstructionType.StartObject)
                {
                    return (XamlType)data;
                }
                return null;
            }
        }

        public InstructionType InstructionType => instructionType;

        public MemberBase Member
        {
            get
            {
                if (InstructionType == InstructionType.StartMember)
                {
                    return (MemberBase)data;
                }

                return null;
            }
        }

        public object Value => InstructionType == InstructionType.Value ? data : null;

        public NamespaceDeclaration NamespaceDeclaration
        {
            get
            {
                if (InstructionType == InstructionType.NamespaceDeclaration)
                {
                    return (NamespaceDeclaration)data;
                }

                return null;
            }
        }

        public bool Equals(Instruction other)
        {
            return instructionType == other.instructionType && Equals(data, other.data) && internalNodeType == other.internalNodeType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Instruction && Equals((Instruction) obj);
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
                case InstructionType.None:
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
                case InstructionType.StartObject:
                    {
                        str = string.Concat(str, XamlType.Name);
                        return str;
                    }
                case InstructionType.GetObject:
                case InstructionType.EndObject:
                case InstructionType.EndMember:
                    {
                        return str;
                    }
                case InstructionType.StartMember:
                    {
                        str = string.Concat(str, Member.ToString());
                        return str;
                    }
                case InstructionType.Value:
                    {
                        str = string.Concat(str, Value.ToString());
                        return str;
                    }
                case InstructionType.NamespaceDeclaration:
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