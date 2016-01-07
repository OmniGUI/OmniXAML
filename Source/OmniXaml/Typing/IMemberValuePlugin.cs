namespace OmniXaml.Typing
{
    public interface IMemberValuePlugin
    {
        void SetValue(object instance, object value);
        object GetValue(object instance);
    }
}