namespace OmniXaml.Tests.Classes
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