namespace OmniXaml
{
    public interface IObjectAssembler
    {
        void WriteNode(XamlNode node);
        void WriteNode(IXamlReader reader);

        object Result { get; }
    }
}