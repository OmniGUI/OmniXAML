namespace OmniXaml.Wpf
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Markup;
    using System.Xaml;
    using System.Xaml.Schema;
    using Typing;
    using XamlMember = System.Xaml.XamlMember;

    internal class XamlMemberAdapter : XamlMember, IProvideValueTarget
    {
        private readonly XamlMember member;

        public XamlMemberAdapter(XamlMember member, XamlSchemaContext context, string propName, MethodInfo getter, MethodInfo setter)
            : base(attachablePropertyName: propName, getter: getter, setter: setter, schemaContext: context)
        {
            this.member = member;
        }

        public XamlMemberAdapter(XamlMember member, XamlSchemaContext context) : base(GetPropertyInfo(member), context)
        {
            this.member = member;
        }

        private static PropertyInfo GetPropertyInfo(XamlMember member)
        {
            return member.DeclaringType.UnderlyingType.GetProperty(member.Name);
        }

        public object TargetObject
        {
            get { throw new NotSupportedException(); }
        }

        public object TargetProperty => GetDependencyProperty(member.DeclaringType.UnderlyingType, member.Name);

        private static DependencyProperty GetDependencyProperty(Type type, string name)
        {
            var dpPropName = name + "Property";
            var fieldInfo = type.GetField(dpPropName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return (DependencyProperty)fieldInfo?.GetValue(null);
        }

        protected override XamlMemberInvoker LookupInvoker()
        {
            return new XamlMemberInvokerAdapter(member);
        }
    }
}