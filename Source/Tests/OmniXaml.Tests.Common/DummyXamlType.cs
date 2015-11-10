namespace OmniXaml.Tests.Common
{
    using System;
    using Typing;

    internal class DummyXamlType : XamlType
    {
        public DummyXamlType(Type type, IXamlTypeRepository typeRepository, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
            : base(type, typeRepository, typeTypeFactory, featureProvider)
        {
        }

        public bool IsNameScope { get; set; }


        public override INameScope GetNamescope(object instance)
        {
            if (IsNameScope)
            {
                return instance as INameScope;
            }

            return null;
        }
    }
}