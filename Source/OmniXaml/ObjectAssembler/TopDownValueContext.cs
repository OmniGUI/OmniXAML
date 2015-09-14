namespace OmniXaml.ObjectAssembler
{
    using System.Collections.Generic;
    using Commands;
    using Glass;
    using Typing;

    public class TopDownValueContext : ITopDownValueContext
    {
        private IDictionary<XamlType, object> Context { get; } = new Dictionary<XamlType, object>();
        public void SetInstanceValue(XamlType xamlType, object instance)
        {
            Context.AddOrReplace(xamlType, instance);
        }

        public object GetLastInstance(XamlType xamlType)
        {
            return Context[xamlType];
        }
    }
}