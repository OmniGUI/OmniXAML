namespace OmniXaml
{
    using System;

    public class InjectableMember
    {
        public InjectableMember(object value)
        {
            Value = value;
        }

        public InjectableMember(string name, object instance)
        {            
        }

        public object Value { get; set; }
        public string Name { get; set; }
        public Type InjectionType { get; set; }
    }
}