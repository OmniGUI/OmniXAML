namespace OmniXaml.Tests.Classes
{
    using System.Collections.Generic;

    public class DummyObject : INameScope
    {
        readonly IDictionary<string, object> dict = new Dictionary<string, object>();

        public void Register(string name, object scopedElement)
        {
            dict.Add(name, scopedElement);
        }

        public void Unregister(string name)
        {
            dict.Remove(name);
        }

        public object Find(string name)
        {
            return dict[name];
        }
    }
}