namespace OmniXaml.Tests.Common
{
    using System;
    using Typing;

    internal class XamlTypeMock : XamlType
    {
        public XamlTypeMock(Type type, ITypeRepository typeRepository, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
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