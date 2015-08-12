namespace OmniXaml.Typing
{
    public interface IXamlMemberValuePlugin
    {
        void SetValue(object instance, object value);
        object GetValue(object instance);
    }
}