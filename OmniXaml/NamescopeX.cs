namespace OmniXaml
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class NamescopeX
    {
        readonly IDictionary<string, object> namedInstances = new Dictionary<string, object>();

        public void Add(string name, object instance)
        {
            namedInstances.Add(name, instance);
        }

        public object Get(string name)
        {
            if (namedInstances.TryGetValue(name, out var instance))
            {
                return instance;
            }

            var gets = from ns in Namescopes
                let get = ns.Get(name)
                where get != null
                select get;

            return gets.FirstOrDefault();

        }

        public NamescopeX AddNamescope()
        {
            var namescopeX = new NamescopeX();
            this.Namescopes.Add(namescopeX);
            return namescopeX;
        }

        public Collection<NamescopeX> Namescopes { get; set; } = new Collection<NamescopeX>();
    }
}