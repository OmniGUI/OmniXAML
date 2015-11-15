namespace OmniXaml.Tests.Common
{
    using System;
    using Typing;

    public class DummyXamlType : XamlType
    {
        public DummyXamlType(Type type, IXamlTypeRepository typeRepository, ITypeFactory typeTypeFactory, ITypeFeatureProvider featureProvider)
            : base(type, typeRepository, typeTypeFactory, featureProvider)
        {
        }

        public bool IsNameScope { get; set; }

        public Action<object> ActionBeforeInstanceSetup { get; set; } = DoNothingAction;

        private static Action<object> DoNothingAction
        {
            get { return o => { }; }
        }

        public Action<object> ActionAfterInstanceSetup { get; set; } = DoNothingAction;
        public Action<object> ActionAfterAssociationToParent { get; set; } = DoNothingAction;


        public override INameScope GetNamescope(object instance)
        {
            if (IsNameScope)
            {
                return instance as INameScope;
            }

            return null;
        }

        public override void AfterInstanceSetup(object instance)
        {
            ActionAfterInstanceSetup(instance);
        }

        public override void BeforeInstanceSetup(object instance)
        {
            ActionBeforeInstanceSetup(instance);
        }

        public override void AfterAssociationToParent(object instance)
        {
            ActionAfterAssociationToParent(instance);
        }
    }
}