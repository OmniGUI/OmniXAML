using System;
using System.Collections.Generic;

namespace OmniXaml
{
    public class InjectableMember
    {
        public IEnumerable<object> Values { get; set; }
        public string Name { get; set; }
        public Type InjectionType { get; set; }
    }
}