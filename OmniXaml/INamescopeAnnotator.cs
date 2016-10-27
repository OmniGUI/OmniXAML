namespace OmniXaml
{
    public interface INamescopeAnnotator
    {
        void NewInstance(object instance);
        void RegisterName(string name, object instance);
        Namescope GetNamescope(object instance);
        object Find(string name, object parent);
    }
}