namespace OmniXaml.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using Typing;

    internal class TestXamlTypeRepository : XamlTypeRepository
    {
        readonly ISet<Type> nameScopes = new HashSet<Type>();

        public TestXamlTypeRepository(IXamlNamespaceRegistry xamlNamespaceRegistry, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
            : base(xamlNamespaceRegistry, typeTypeFactory, featureProvider)
        {
        }

        public override XamlType GetXamlType(Type type)
        {
            var isNameScope = nameScopes.Contains(type);
            return new XamlTypeMock(type, this, TypeFactory, FeatureProvider) { IsNameScope = isNameScope };
        }

        public void ClearNameScopes()
        {           
            nameScopes.Clear();
        }

        public void EnableNameScope(Type type)
        {
            nameScopes.Add(type);
        }
    }
}