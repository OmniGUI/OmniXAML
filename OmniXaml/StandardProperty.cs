namespace OmniXaml
{
    using System;
    using System.Reflection;

    public class StandardProperty : Property
    {
        private readonly MethodInfo getter;
        private readonly MethodInfo setter;
        private readonly Type propertyType;

        public StandardProperty(Type owner, string propertyName) : base(owner, propertyName)
        {
            var propInfo = owner.GetRuntimeProperty(propertyName);

            getter = propInfo.GetMethod;
            setter = propInfo.SetMethod;
            propertyType = propInfo.PropertyType;
        }

        public override object GetValue(object instance)
        {
            return getter.Invoke(instance, null);
        }

        public override Type PropertyType => propertyType;

        public override void SetValue(object instance, object value)
        {
            setter.Invoke(instance, new []{ value } );
        }      
    }
}