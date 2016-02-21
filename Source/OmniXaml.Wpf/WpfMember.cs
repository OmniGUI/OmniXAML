namespace OmniXaml.Wpf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Markup;
    using Typing;

    public class WpfMember : Member
    {
        public WpfMember(string name, XamlType declaringType, ITypeRepository typeRepository, ITypeFeatureProvider typeFeatureProvider)
            : base(name, declaringType, typeRepository, typeFeatureProvider)
        {
        }

        protected override IMemberValuePlugin LookupXamlMemberValueConnector()
        {
            return new MemberValuePlugin(this);
        }

        protected override IEnumerable<Member> LookupDependencies()
        {
            var underlyingType = DeclaringType.UnderlyingType;
            var dependsOnAttributes = underlyingType.GetRuntimeProperty(Name)?.GetCustomAttributes<DependsOnAttribute>();
            return dependsOnAttributes.Select(attr => DeclaringType.GetMember(attr.Name));
        }

        public override bool IsDirective => false;
        public override bool IsAttachable => false;
    }
}