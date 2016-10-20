namespace OmniXaml
{
    public interface IInstanceLifecycleSignaler
    {
        void BeforeAssigments(object instance);
        void AfterAssigments(object instance);
    }
}