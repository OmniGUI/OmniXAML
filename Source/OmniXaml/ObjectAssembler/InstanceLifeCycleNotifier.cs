using OmniXaml.Typing;

namespace OmniXaml.ObjectAssembler
{
    internal static class InstanceLifeCycleNotifier
    {
        public static void NotifyBeginningOfSetup(XamlType xamlType, object instance)
        {
            xamlType.BeforeInstanceSetup(instance);
        }

        public static void NotifyEndOfSetup(XamlType xamlType, object instance)
        {
            xamlType.AfterInstanceSetup(instance);
        }

        public static void NotifyAssociatedToParent(XamlType xamlType, object instance)
        {
            xamlType.AfterAssociationToParent(instance);
        }
    }
}