using System.Collections.Generic;

namespace OmniXaml
{
    public class CreationHints
    {
        public CreationHints()
        {            
        }

        public CreationHints(IEnumerable<InjectableMember> members, IEnumerable<PositionalParameter> positionals, IEnumerable<object> children)
        {
            Members = members;
            Positionals = positionals;
            Children = children;
        }

        public IEnumerable<InjectableMember> Members { get; } = new List<InjectableMember>();
        public IEnumerable<PositionalParameter> Positionals { get; } = new List<PositionalParameter>();
        public IEnumerable<object> Children { get; } = new List<object>();
    }
}