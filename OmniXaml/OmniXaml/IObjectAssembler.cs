namespace OmniXaml
{
    public interface IObjectAssembler
    {
        object Result { get; }

        void WriteNode(XamlNode node);

        void OverrideInstance(object instance);
    }
}