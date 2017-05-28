namespace OmniXaml
{
    using System;
    using System.Reflection;

    internal class StandardProperty : Member
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

        public override Type MemberType => propertyType;

        public override void SetValue(object instance, object value)
        {
            if (setter == null)
            {
                throw new InvalidOperationException($"Attempt to write to read-only property ({MemberName}) from type {Owner}");
            }

            setter.Invoke(instance, new []{ value } );
        }      
    }
}