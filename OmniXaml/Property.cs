namespace OmniXaml
{
    using System;
    using System.Reflection;

    public class Property
    {
        private readonly MethodInfo getter;
        private readonly MethodInfo setter;
        private PropertyInfo propInfo;

        public Property(Type type, string propertyName)
        {
            propInfo = type.GetRuntimeProperty(propertyName);
            getter = propInfo.GetMethod;
            setter = propInfo.SetMethod;            
        }

        public string Name { get; set; }

        public object GetValue(object instance)
        {
            return getter.Invoke(instance, null);
        }

        public Type PropertyType => propInfo.PropertyType;

        public object SetValue(object instance, object value)
        {
            return setter.Invoke(instance, new []{ value } );
        }
    }
}