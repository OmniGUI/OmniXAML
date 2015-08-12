namespace OmniXaml.Builder
{
    using System;

    public class ContentPropertyDefinition
    {
        private readonly Type ownerType;
        private readonly string name;

        public ContentPropertyDefinition(Type ownerType, string name)
        {
            this.ownerType = ownerType;
            this.name = name;
        }

        public Type OwnerType => ownerType;

        public string Name => name;
    }
}