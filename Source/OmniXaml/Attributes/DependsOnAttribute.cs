namespace OmniXaml.Attributes
{
    using System;

    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}