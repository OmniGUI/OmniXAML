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

        public new bool IsNameScope { get; set; }

        protected override bool LookupIsNamescope()
        {
            return IsNameScope;
        }
    }
}