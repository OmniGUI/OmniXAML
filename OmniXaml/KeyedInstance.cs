namespace OmniXaml
{
    public class KeyedInstance
    {
        public object Instance { get; set; }

        public KeyedInstance(object instance, object key = null)
        {
            Instance = instance;
            Key = key;
        }

        public object Key { get; set; }
    }
}