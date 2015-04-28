namespace OmniXaml
{
    public interface IObjectAssembler
    {
        void WriteNode(IXamlReader reader);

        object Result { get; }
    }
}