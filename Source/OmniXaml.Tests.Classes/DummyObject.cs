namespace OmniXaml.Tests.Classes
{
    using System.Collections.Generic;
    using Typing;

    public class DummyObject : INameScope
    {
        readonly IDictionary<string, object> namescopeRegistrations = new Dictionary<string, object>();
        private string name;
        public IList<string> NamesHistory { get; } = new List<string>();

        [Name]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NamesHistory.Add(name);
            }
        }

        public void Register(string name, object scopedElement)
        {
            namescopeRegistrations.Add(name, scopedElement);
        }

        public void Unregister(string name)
        {
            namescopeRegistrations.Remove(name);
        }

        public object Find(string name)
        {
            return namescopeRegistrations[name];
        }
    }
}