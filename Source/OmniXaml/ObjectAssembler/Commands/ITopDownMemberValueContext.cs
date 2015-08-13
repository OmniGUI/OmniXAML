namespace OmniXaml.ObjectAssembler.Commands
{
    using Typing;

    public interface ITopDownMemberValueContext
    {
        void SetMemberValue(XamlType member, object instance);
        object GetMemberValue(XamlType type);
    }
}