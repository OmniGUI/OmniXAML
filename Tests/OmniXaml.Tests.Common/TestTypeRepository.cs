namespace OmniXaml.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using Typing;

    internal class TestTypeRepository : TypeRepository
    {
        readonly ISet<Type> nameScopes = new HashSet<Type>();

        public TestTypeRepository(INamespaceRegistry namespaceRegistry, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
            : base(namespaceRegistry, typeTypeFactory, featureProvider)
        {
        }

        public override XamlType GetByType(Type type)
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