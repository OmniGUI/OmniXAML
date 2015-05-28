namespace OmniXaml
{
    public interface IObjectAssembler
    {
        void WriteNode(XamlNode node);

        object Result { get; }
    }
}