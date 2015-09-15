namespace Glass.ChangeTracking
{
    using System.Reflection;

    public class Property
    {
        public Property(object owner, string propertyName)
        {
            Owner = owner;
            PropertyName = propertyName;
            PropertyInfo = Owner.GetType().GetRuntimeProperty(propertyName);
        }

        protected string PropertyName { get; }

        protected object Owner { get; }

        private PropertyInfo PropertyInfo { get; }

        public object Value
        {
            get { return PropertyInfo.GetValue(Owner); }
            set { PropertyInfo.SetValue(Owner, value); }
        }
    }
}