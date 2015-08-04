namespace OmniXaml.Typing
{
    using System.Linq;
    using System.Reflection;

    public class AttachableXamlMember : MutableXamlMember
    {
        private readonly ITypeFeatureProvider featureProvider;

        public AttachableXamlMember(string name, XamlType owner, IXamlTypeRepository xamlTypeRepository, ITypeFactory typeFactory, ITypeFeatureProvider featureProvider)
            : base(name, owner, xamlTypeRepository, typeFactory)
        {
            this.featureProvider = featureProvider;
        }

        public override bool IsAttachable => true;
        public override bool IsDirective => false;
        protected override XamlType LookupType()
        {
            var getMethod = GetGetMethodForAttachable(DeclaringType, Name);            
            return XamlType.Create(getMethod.ReturnType, TypeRepository, TypeFactory, featureProvider);
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