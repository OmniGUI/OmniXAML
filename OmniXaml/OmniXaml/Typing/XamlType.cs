namespace OmniXaml.Typing
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class XamlType
    {
        private readonly IXamlTypeRepository typeRepository;

        private XamlType(Type type, IXamlTypeRepository typeRepository)
        {
            this.typeRepository = typeRepository;
            UnderlyingType = type;
            Name = type.Name;
        }      

        private XamlType(string name)
        {
            Name = name;
            IsUnreachable = true;
        }

        public bool IsUnreachable { get; private set; }

        public Type UnderlyingType { get; private set; }

        public string Name { get; set; }

        public XamlType BaseType { get; set; }

        public bool IsCollection => typeof (IList).GetTypeInfo().IsAssignableFrom(this.UnderlyingType.GetTypeInfo());

        public bool IsContainer => IsCollection || IsDictionary;

        public bool IsDictionary { get; set; }

        protected bool Equals(XamlType other)
        {
            return UnderlyingType == other.UnderlyingType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((XamlType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UnderlyingType?.GetHashCode() ?? 0) * 397);
            }
        }

        public XamlMember GetMember(string name)
        {
            if (IsUnreachable)
            {
                return XamlMember.Builder.CreateUnreachable(name, this);
            }

            return XamlMember.Builder.Create(name, this, typeRepository);
        }

        public XamlMember GetAttachableMember(string name)
        {
            return XamlMember.Builder.CreateAttached(name, this, typeRepository);
        }

        public override string ToString()
        {
            return "XamlType: " + Name;
        }

        public static class Builder
        {
            public static XamlType CreateUnreachable(XamlTypeName typeName)
            {
                return new XamlType(typeName.Name);
            }

            public static XamlType Create(Type underlyingType, IXamlTypeRepository mother)
            {
                return new XamlType(underlyingType, mother);
            }

            public static XamlType CreateForBuiltInType(Type type)
            {
                return new XamlType(type, new FrameworkBuiltInTypeRepository());
            }
        }

        public bool CanAssignTo(XamlType type)
        {
            var otherUnderlyingType = type.UnderlyingType.GetTypeInfo();
            return otherUnderlyingType.IsAssignableFrom(UnderlyingType.GetTypeInfo());
        }
    }
}