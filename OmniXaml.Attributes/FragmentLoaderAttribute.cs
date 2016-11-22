namespace OmniXaml.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class FragmentLoaderAttribute : Attribute
    {
        public string PropertyName { get; set; }
        public Type FragmentLoader { get; set; }
        public Type Type { get; set; }
    }
}