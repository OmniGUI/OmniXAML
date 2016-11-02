namespace OmniXaml
{
    using System.Collections.Generic;

    public class Namescope
    {
        private readonly object hostingInstance;
        private readonly IDictionary<string, object> dict = new Dictionary<string, object>();

        public Namescope(object hostingInstance)
        {
            this.hostingInstance = hostingInstance;
        }   

        public ICollection<Namescope> Children { get; set; } = new List<Namescope>();

        public void Register(string name, object instance)
        {
            dict.Add(name, instance);
        }

        public object Find(string name)
        {
            return dict[name];
        }
    }
}