namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class XamlMember
    {
        private readonly string name;

        protected XamlMember(string name)
        {
            this.name = name;
        }

        public XamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, bool isAttachable) : this(name)
        {
            IsAttachable = isAttachable;
            DeclaringType = owner;

            Type = LookupType(name, owner, xamlTypeRepository, typeFactory, isAttachable);
        }

        private XamlType LookupType(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, bool isAttachable)
        {
            if (!isAttachable)
            {
                var property = owner.UnderlyingType.GetRuntimeProperty(name);
                return XamlType.Builder.Create(property.PropertyType, xamlTypeRepository, typeFactory);
            }

            var getMethod = GetGetMethodForAttachable(owner, name);
            return XamlType.Builder.Create(getMethod.ReturnType, xamlTypeRepository, typeFactory);
        }

        private static MethodInfo GetGetMethodForAttachable(XamlType owner, string name)
        {
            return owner.UnderlyingType.GetTypeInfo().GetDeclaredMethod("Get" + name);
        }

        private static MethodInfo GetSetMethodForAttachable(XamlType owner, string name)
        {
            var runtimeMethods = owner.UnderlyingType.GetRuntimeMethods();
            return runtimeMethods.First(info =>
            {
                var nameOfSetMethod = "Set" + name;
                return info.Name == nameOfSetMethod && info.GetParameters().Length == 2;
            });
        }

        public string Name => name;

        public bool IsAttachable { get; }

        public bool IsDirective { get; set; }

        public XamlType DeclaringType { get; }

        public XamlType Type { get; set; }

        public override string ToString()
        {
            return IsDirective ? "XamlDirective:" : "XamlMember: " + Name;
        }

        public static class Builder
        {
            public static XamlMember Create(string name, XamlType parent, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory)
            {
                return new XamlMember(name, parent, xamlTypeRepository, typeFactory, false);
            }

            public static XamlMember CreateAttached(string name, XamlType parent, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory)
            {
                return new XamlMember(name, parent, xamlTypeRepository, typeFactory, true);
            }
        }

        protected bool Equals(XamlMember other)
        {
            return string.Equals(name, other.name) && IsAttachable.Equals(other.IsAttachable) && IsDirective.Equals(other.IsDirective) &&
                   Equals(DeclaringType, other.DeclaringType) && Equals(Type, other.Type);
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
            return Equals((XamlMember)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsAttachable.GetHashCode();
                hashCode = (hashCode * 397) ^ IsDirective.GetHashCode();
                hashCode = (hashCode * 397) ^ (DeclaringType != null ? DeclaringType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                return hashCode;
            }
        }

        public IXamlMemberValuePlugin XamlMemberValueConnector => LookupXamlMemberValueConnector();

        protected virtual IXamlMemberValuePlugin LookupXamlMemberValueConnector()
        {
            return new MemberValuePlugin(this);
        }

        public void SetValue(object instance, object value)
        {
            XamlMemberValueConnector.SetValue(instance, value);
        }

        public object GetValue(object instance)
        {
            return XamlMemberValueConnector.GetValue(instance);
        }
    }
}