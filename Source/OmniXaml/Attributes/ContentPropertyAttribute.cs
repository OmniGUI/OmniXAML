namespace OmniXaml.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
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
