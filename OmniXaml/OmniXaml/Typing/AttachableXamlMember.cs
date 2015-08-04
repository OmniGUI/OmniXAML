namespace OmniXaml.Typing
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class AttachableXamlMember : MutableXamlMember
    {
        private readonly MethodInfo setter;
        private readonly MethodInfo getter;

        public AttachableXamlMember(string name,
            MethodInfo getter,
            MethodInfo setter,
            IXamlTypeRepository xamlTypeRepository,
            ITypeFeatureProvider featureProvider) : base(name, xamlTypeRepository.GetXamlType(getter.DeclaringType), xamlTypeRepository, featureProvider)
        {
            this.getter = getter;
            this.setter = setter;
            XamlType = LookupType();
        }

        public override bool IsAttachable => true;
        public override bool IsDirective => false;

        public override MethodInfo Getter => getter;

        public override MethodInfo Setter => setter;

        private XamlType LookupType()
        {
            return TypeRepository.GetXamlType(getter.ReturnType);
        }

        private static MethodInfo GetGetMethodForAttachable(XamlType owner, string name)
        {
            return owner.UnderlyingType.GetTypeInfo().GetDeclaredMethod("Get" + name);
        }

        private static MethodInfo GetSetMethodForAttachable(XamlType owner, string name)
        {
            var runtimeMethods = owner.UnderlyingType.GetRuntimeMethods();
            return runtimeMethods.First(info =>
            {
                var nameOfSetMethod = "Set" + name;
                return info.Name == nameOfSetMethod && info.GetParameters().Length == 2;
            });
        }
    }
}