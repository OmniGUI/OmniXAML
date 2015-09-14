namespace OmniXaml.ObjectAssembler.Commands
{
    using Typing;

    public interface ITopDownValueContext
    {
        void SetInstanceValue(XamlType xamlType, object instance);
        object GetLastInstance(XamlType xamlType);
    }
}