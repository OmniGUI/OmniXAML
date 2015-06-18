namespace OmniXaml.Typing
{
    using System.Reflection;

    public class DefaultMemberValuePlugin : IXamlMemberValuePlugin
    {
        private readonly XamlMember xamlMember;

        public DefaultMemberValuePlugin(XamlMember xamlMember)
        {
            this.xamlMember = xamlMember;
        }

        public virtual object GetValue(object instance)
        {
            return ValueGetter.Invoke(instance, null);
        }

        public virtual void SetValue(object instance, object value)
        {
            SetValueIndependent(instance, value);
        }

        private void SetValueIndependent(object instance, object value)
        {
            if (ValueSetter.IsStatic)
            {
                ValueSetter.Invoke(null, new[] { instance, value });
            }
            else
            {
                ValueSetter.Invoke(instance, new[] { value });
            }
        }

        private MethodInfo ValueSetter
        {
            get
            {
                if (xamlMember.IsAttachable)
                {
                    var underlyingType = xamlMember.DeclaringType.UnderlyingType;
                    return underlyingType.GetTypeInfo().GetDeclaredMethod("Set" + xamlMember.Name);
                }
                else
                {
                    return xamlMember.DeclaringType.UnderlyingType.GetRuntimeProperty(xamlMember.Name).SetMethod;
                }
            }
        }

        private MethodInfo ValueGetter
        {
            get
            {
                if (xamlMember.IsAttachable)
                {
                    var underlyingType = xamlMember.DeclaringType.UnderlyingType;
                    return underlyingType.GetTypeInfo().GetDeclaredMethod("Get" + xamlMember.Name);
                }
                else
                {
                    return xamlMember.DeclaringType.UnderlyingType.GetRuntimeProperty(xamlMember.Name).GetMethod;
                }
            }
        }
    }
}