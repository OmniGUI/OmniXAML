namespace OmniXaml.Typing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class XamlMember
    {
        private readonly string name;

        public XamlMember(string name)
        {
            this.name = name;
        }

        private XamlMember(string name, XamlType owner, IXamlTypeRepository mother, bool isAttachable) : this(name)
        {
            IsAttachable = isAttachable;
            DeclaringType = owner;

            Type = LookupType(name, owner, mother, isAttachable);
        }

        private XamlType LookupType(string name, XamlType owner, IXamlTypeRepository mother, bool isAttachable)
        {
            if (!isAttachable)
            {
                var property = owner.UnderlyingType.GetRuntimeProperty(name);
                return XamlType.Builder.Create(property.PropertyType, mother);
            }

            var getMethod = GetGetMethodForAttachable(owner, name);
            return XamlType.Builder.Create(getMethod.ReturnType, mother);
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

        public XamlType DeclaringType { get; set; }

        public bool IsUnreachable { get; set; }

        public XamlType Type { get; set; }

        public override string ToString()
        {
            return IsDirective ? "XamlDirective:" : "XamlMember: " + Name;
        }

        public static class Builder
        {
            public static XamlMember Create(string name, XamlType parent, IXamlTypeRepository mother)
            {
                return new XamlMember(name, parent, mother, false);
            }

            public static XamlMember CreateUnreachable(string name, XamlType declaringType)
            {
                return new XamlMember(name)
                {
                    DeclaringType = declaringType,
                    IsUnreachable = true,
                };
            }

            public static XamlMember CreateAttached(string name, XamlType parent, IXamlTypeRepository mother)
            {
                return new XamlMember(name, parent, mother, true);
            }
        }

        protected bool Equals(XamlMember other)
        {
            return string.Equals(name, other.name) && IsAttachable.Equals(other.IsAttachable) && IsDirective.Equals(other.IsDirective) &&
                   Equals(DeclaringType, other.DeclaringType) && IsUnreachable.Equals(other.IsUnreachable) && Equals(Type, other.Type);
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
                hashCode = (hashCode * 397) ^ IsUnreachable.GetHashCode();
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                return hashCode;
            }
        }

        public IXamlMemberValuePlugin XamlMemberValueConnector => LookupXamlMemberValueConnector();

        private IXamlMemberValuePlugin LookupXamlMemberValueConnector()
        {
            return new DefaultMemberValuePlugin(this);
        }     
    }

    internal class DefaultMemberValuePlugin : IXamlMemberValuePlugin
    {
        private readonly XamlMember xamlMember;

        public DefaultMemberValuePlugin(XamlMember xamlMember)
        {
            this.xamlMember = xamlMember;
        }

        public object GetValue(object instance)
        {
            return ValueGetter.Invoke(instance, null);
        }

        public void SetValue(object instance, object value)
        {
            SetValueIndependent(instance, value);
        }

        private void SetValueIndependent(object instance, object value)
        {
            if (ValueSetter.IsStatic)
            {
                ValueSetter.Invoke(null, new[] { instance, value });
            }
            else
            {
                ValueSetter.Invoke(instance, new[] { value });
            }
        }

        private MethodInfo ValueSetter
        {
            get
            {
                if (xamlMember.IsAttachable)
                {
                    var underlyingType = xamlMember.DeclaringType.UnderlyingType;
                    return underlyingType.GetTypeInfo().GetDeclaredMethod("Set" + xamlMember.Name);
                }
                else
                {
                    return xamlMember.DeclaringType.UnderlyingType.GetRuntimeProperty(xamlMember.Name).SetMethod;
                }
            }
        }

        private MethodInfo ValueGetter
        {
            get
            {
                if (xamlMember.IsAttachable)
                {
                    var underlyingType = xamlMember.DeclaringType.UnderlyingType;
                    return underlyingType.GetTypeInfo().GetDeclaredMethod("Get" + xamlMember.Name);
                }
                else
                {
                    return xamlMember.DeclaringType.UnderlyingType.GetRuntimeProperty(xamlMember.Name).GetMethod;
                }
            }
        }
    }

    public interface IXamlMemberValuePlugin
    {
        void SetValue(object instance, object value);
        object GetValue(object instance);
    }
}