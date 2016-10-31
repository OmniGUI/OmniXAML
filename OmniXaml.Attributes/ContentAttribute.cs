namespace OmniXaml.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ContentAttribute : Attribute
    {
        public string Name { get; set; }
    }
}