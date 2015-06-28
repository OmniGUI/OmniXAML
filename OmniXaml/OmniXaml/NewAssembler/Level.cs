namespace OmniXaml.NewAssembler
{
    using Typing;

    public class Level
    {
        public object Instance { get; set; }
        public XamlType XamlType { get; set; }
        public XamlMember XamlMember { get; set; }

        public void MeterializeCurrentType()
        {
            var instance = XamlType.CreateInstance(null);
            Instance = instance;
        }
    }
}