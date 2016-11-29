namespace OmniXaml
{
    public interface IInstanceLifecycleSignaler
    {
        void OnBegin(object instance);
        void EndEnd(object instance);
        void AfterAssociatedToParent(object instance);
    }
}