namespace OmniXaml
{
    using System;
    using System.Linq.Expressions;
    using Glass;
    using Glass.Core;

    public abstract class Property
    {
        protected Type Owner { get; }
        protected string PropertyName { get; }

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
    }
}