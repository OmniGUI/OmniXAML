namespace OmniXaml.ObjectAssembler.Commands
{
    using Typing;

    public interface ITopDownValueContext
    {
        void Add(object instance, XamlType xamlType);
        object GetLastInstance(XamlType xamlType);
    }
}