namespace OmniXaml.DefaultLoader
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(string nameOfDependency)
        {
            DependentPropertyName = nameOfDependency;
        }
        public string DependentPropertyName { get; set; }
    }
}