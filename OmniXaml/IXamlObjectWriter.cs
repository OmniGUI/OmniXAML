namespace OmniXaml
{
    public interface IXamlObjectWriter
    {
        void WriteNode(IXamlReader reader);

        object Result { get; }
    }
}