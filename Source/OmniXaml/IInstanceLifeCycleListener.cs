namespace OmniXaml
{
    public interface IInstanceLifeCycleListener
    {
        void OnBegin(object instance);
        void OnAfterProperties(object instance);
        void OnAssociatedToParent(object instance);
        void OnEnd(object instance);
    }
}