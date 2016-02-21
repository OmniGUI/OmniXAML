namespace OmniXaml.Typing
{
    using TypeConversion;

    public interface IMemberValuePlugin
    {
        void SetValue(object instance, object value, IValueContext valueContext);
        object GetValue(object instance);        
    }
}