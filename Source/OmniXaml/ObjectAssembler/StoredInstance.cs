namespace OmniXaml.ObjectAssembler
{
    using Typing;

    public class StoredInstance
    {
        public StoredInstance(object instance, XamlType xamlType)
        {
            Instance = instance;
            XamlType = xamlType;
        }

        public object Instance { get; private set; }
        public XamlType XamlType { get; private set; }
    }
}