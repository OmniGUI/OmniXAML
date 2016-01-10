namespace OmniXaml
{
    public interface IInstanceLifeCycleListener
    {
        void OnBegin(object instance);
        void OnAfterProperites(object instance);
        void OnAssociatedToParent(object instance);
        void OnEnd(object instance);
    }
}