namespace OmniXaml.Wpf
{
    using System;
    using System.Reflection;
    using System.Windows;
    using Typing;

    public class WpfXamlMemberValueConnector : MemberValuePlugin
    {
        private readonly XamlMember xamlMember;

        public WpfXamlMemberValueConnector(XamlMember xamlMember) : base(xamlMember)
        {
            this.xamlMember = xamlMember;
        }

        public override void SetValue(object instance, object value)
        {
            if (!TrySetDependencyProperty(instance, value))
            {
                base.SetValue(instance, value);
            }                      
        }

        private bool TrySetDependencyProperty(object instance, object value)
        {
            var dp = GetDependencyProperty(instance.GetType(), xamlMember.Name + "Property");
            if (dp == null)
            {
                return false;
            }

            var dependencyObject = (DependencyObject) instance;
            dependencyObject.SetValue(dp, value);
            return true;
        }

        private static DependencyProperty GetDependencyProperty(Type type, string name)
        {
            FieldInfo fieldInfo = type.GetField(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return (DependencyProperty) fieldInfo?.GetValue(null);
        }
    }
}