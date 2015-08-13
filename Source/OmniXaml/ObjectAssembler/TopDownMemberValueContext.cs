namespace OmniXaml.ObjectAssembler
{
    using System.Collections.Generic;
    using Commands;
    using Glass;
    using Typing;

    public class TopDownMemberValueContext : ITopDownMemberValueContext
    {
        private IDictionary<XamlType, object> Context { get; } = new Dictionary<XamlType, object>();
        public void SetMemberValue(XamlType member, object instance)
        {
            Context.AddOrReplace(member, instance);
        }

        public object GetMemberValue(XamlType member)
        {
            return Context[member];
        }
    }
}