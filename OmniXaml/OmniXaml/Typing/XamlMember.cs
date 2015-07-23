namespace OmniXaml.Typing
{
    using System.Linq;
    using System.Reflection;

    public abstract class XamlMemberBase
    {
        public string Name { get; }
        public XamlType XamlType { get; protected set; }
        public abstract bool IsDirective { get;  }
        public abstract bool IsAttachable { get; }

        protected XamlMemberBase(string name)
        {
            this.Name = name;
        }

        protected bool Equals(XamlMemberBase other)
        {
            return string.Equals(Name, other.Name) && XamlType.Equals(other.XamlType);
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
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((XamlMemberBase) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode()*397) ^ XamlType.GetHashCode();
            }
        }
    }

    public abstract class MutableXamlMember : XamlMemberBase
    {
        private readonly IXamlTypeRepository xamlTypeRepository;
        private readonly ITypeFactory typeFactory;

        protected MutableXamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory) : base(name)
        {
            this.xamlTypeRepository = xamlTypeRepository;
            this.typeFactory = typeFactory;

            DeclaringType = owner;
            XamlType = LookupType();
        }
        
        public XamlType DeclaringType { get; }
        
        public IXamlMemberValuePlugin XamlMemberValueConnector => LookupXamlMemberValueConnector();

        private XamlType LookupType()
        {
            if (!IsAttachable)
            {
                var property = RuntimeReflectionExtensions.GetRuntimeProperty(DeclaringType.UnderlyingType, Name);
                return XamlType.Create(property.PropertyType, xamlTypeRepository, typeFactory);
            }

            var getMethod = GetGetMethodForAttachable(DeclaringType, Name);
            return XamlType.Create(getMethod.ReturnType, xamlTypeRepository, typeFactory);
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

        public override string ToString()
        {
            return IsDirective ? "XamlDirective:" : "XamlMember: " + Name;
        }
       
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

        protected bool Equals(MutableXamlMember other)
        {
            return base.Equals(other) && Equals(DeclaringType, other.DeclaringType) && Equals(XamlType, other.XamlType);
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
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((MutableXamlMember) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ (DeclaringType != null ? DeclaringType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (XamlType?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }

    public class AttachableXamlMember : MutableXamlMember
    {
        public AttachableXamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory) : base(name, owner, xamlTypeRepository, typeFactory)
        {
        }

        public override bool IsAttachable => true;
        public override bool IsDirective => false;
    }

    public class XamlMember : MutableXamlMember
    {
        public XamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, bool isAttachable) : base(name, owner, xamlTypeRepository, typeFactory)
        {
        }

        public override bool IsAttachable => false;

        public override bool IsDirective => false;
    }
}