namespace OmniXaml
{
    using System;
    using System.Linq.Expressions;
    using Glass;
    using Glass.Core;

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

        public static Property RegularProperty<T>(Expression<Func<T, object>> propertySelector)
        {
            return RegularProperty(typeof(T), propertySelector.GetFullPropertyName());
        }

        public static Property RegularProperty(Type type, string propertyName)
        {
            return new StandardProperty(type, propertyName);
        }

        public static Property FromAttached<T>(string propertyName)
        {
            return FromAttached(typeof(T), propertyName);
        }

        public static Property FromAttached(Type type, string propertyName)
        {
            return new AttachedProperty(type, propertyName);
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