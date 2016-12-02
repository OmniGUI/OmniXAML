namespace OmniXaml
{
    public interface IInstanceLifecycleSignaler
    {
        void OnBegin(object instance);
        void OnEnd(object instance);
        void AfterAssociatedToParent(object instance);
    }
}