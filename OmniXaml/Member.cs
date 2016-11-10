namespace OmniXaml
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Glass;
    using Glass.Core;
    using System.Linq;

    public abstract class Member
    {
        public Type Owner { get; }
        public string MemberName { get; }

        public Member(Type owner, string memberName)
        {
            Owner = owner;
            MemberName = memberName;
        }

        public abstract object GetValue(object instance);
        public abstract void SetValue(object instance, object value);
        public abstract Type MemberType { get; }

        public static Member FromStandard<T>(Expression<Func<T, object>> propertySelector)
        {
            return FromStandard(typeof(T), propertySelector.GetFullPropertyName());
        }

        public static Member FromStandard(Type type, string member)
        {
            if (type.GetRuntimeProperty(member) != null)
            {
                return new StandardProperty(type, member);
            }
            else if(type.GetRuntimeEvent(member) != null)
            {
                return new StandardEvent(type, member);
            }

            throw new XamlParserException($"No supported member {member} found on type {type}");
        }

        public static Member FromAttached<T>(string memberName)
        {
            return FromAttached(typeof(T), memberName);
        }

        public static Member FromAttached(Type type, string memberName)
        {
            // If there is a method that looks like an attached property accessor, then assume this is an attached property
            if (type.GetRuntimeMethods().Any(info => info.Name == $"Get{memberName}"))
            {
                return new AttachedProperty(type, memberName);
            }
            else
            {
                return new AttachedEvent(type, memberName);
            }
        }

        public override string ToString()
        {
            return $"{Owner.Name}.{MemberName}";
        }

        protected bool Equals(Member other)
        {
            return Owner == other.Owner && string.Equals(MemberName, other.MemberName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Member) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Owner?.GetHashCode() ?? 0)*397) ^ (MemberName != null ? MemberName.GetHashCode() : 0);
            }
        }
    }
}