namespace OmniXaml.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}