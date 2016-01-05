namespace OmniXaml.Typing
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class NameAttribute : Attribute
    {
        public string Name { get; set; }
    }
}