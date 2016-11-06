namespace OmniXaml
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Glass;
    using Glass.Core;
    using System.Linq;

    public abstract class Property
    {
        public Type Owner { get; }
        public string PropertyName { get; }

        public Property(Type owner, string propertyName)
        {
            Owner = owner;
            PropertyName = propertyName;
        }

        public abstract object GetValue(object instance);
        public abstract void SetValue(object instance, object value);
        public abstract Type PropertyType { get; }

        public static Property FromStandard<T>(Expression<Func<T, object>> propertySelector)
        {
            return FromStandard(typeof(T), propertySelector.GetFullPropertyName());
        }

        public static Property FromStandard(Type type, string propertyOrEventName)
        {
            if (type.GetRuntimeProperty(propertyOrEventName) != null)
            {
                return new StandardProperty(type, propertyOrEventName);
            }
            else if(type.GetRuntimeEvent(propertyOrEventName) != null)
            {
                return new StandardEvent(type, propertyOrEventName);
            }

            throw new XamlParserException($"No property or event named {propertyOrEventName} found on type {type}");
        }

        public static Property FromAttached<T>(string propertyName)
        {
            return FromAttached(typeof(T), propertyName);
        }

        public static Property FromAttached(Type type, string propertyOrEventName)
        {
            // If there is a method that looks like an attached property accessor, then assume this is an attached property
            if (type.GetRuntimeMethods().Any(info => info.Name == $"Get{propertyOrEventName}"))
            {
                return new AttachedProperty(type, propertyOrEventName);
            }
            else
            {
                return new AttachedEvent(type, propertyOrEventName);
            }
        }

        public override string ToString()
        {
            return $"{Owner.Name}.{PropertyName}";
        }

        protected bool Equals(Property other)
        {
            return Owner == other.Owner && string.Equals(PropertyName, other.PropertyName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Property) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Owner?.GetHashCode() ?? 0)*397) ^ (PropertyName != null ? PropertyName.GetHashCode() : 0);
            }
        }
    }
}