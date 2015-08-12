namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Markup;
    using Typing;

    public class WpfXamlMember : XamlMember
    {
        public WpfXamlMember(string name, XamlType declaringType, IXamlTypeRepository xamlTypeRepository, ITypeFeatureProvider typeFeatureProvider)
            : base(name, declaringType, xamlTypeRepository, typeFeatureProvider)
        {
        }

        protected override IXamlMemberValuePlugin LookupXamlMemberValueConnector()
        {
            return new MemberValuePlugin(this);
        }

        protected override IEnumerable<XamlMember> LookupDependencies()
        {
            var underlyingType = DeclaringType.UnderlyingType;
            var dependsOnAttributes = underlyingType.GetRuntimeProperty(Name)?.GetCustomAttributes<DependsOnAttribute>();
            return dependsOnAttributes.Select(attr => DeclaringType.GetMember(attr.Name));
        }

        public override bool IsDirective => false;
        public override bool IsAttachable => false;
    }
}