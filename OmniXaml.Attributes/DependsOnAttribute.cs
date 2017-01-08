namespace OmniXaml.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(string nameOfDependency)
        {
            Dependency = nameOfDependency;
        }
        public string Dependency { get; private set; }
    }
}