using System;

namespace OmniXaml.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TypeConverterMember : Attribute
    {
        public Type SourceType { get; }

        public TypeConverterMember(Type sourceType)
        {
            SourceType = sourceType;
        } 
    }
}