namespace OmniXaml.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ContentPropertyAttribute : Attribute
    {
        public string Name { get; }

        public ContentPropertyAttribute()
        {
        }

        public ContentPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
