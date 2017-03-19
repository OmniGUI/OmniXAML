namespace OmniXaml.Rework
{
    using System;
    using System.Collections.Generic;

    public class CreationHints
    {
        public CreationHints()
        {            
        }

        public CreationHints(IEnumerable<NewInjectableMember> members, IEnumerable<PositionalParameter> positionals, IEnumerable<object> children)
        {
            Members = members;
            Positionals = positionals;
            Children = children;
        }

        public IEnumerable<NewInjectableMember> Members { get; } = new List<NewInjectableMember>();
        public IEnumerable<PositionalParameter> Positionals { get; } = new List<PositionalParameter>();
        public IEnumerable<object> Children { get; } = new List<object>();
    }

    public class NewInjectableMember
    {
        public IEnumerable<object> Values { get; set; }
        public string Name { get; set; }
        public Type InjectionType { get; set; }
    }
}