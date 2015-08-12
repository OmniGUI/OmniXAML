namespace OmniXaml.Assembler
{
    using Typing;

    internal class Scope
    {
        public XamlType XamlType { get; set; }
        public XamlMemberBase Member { get; set; }
        public object Instance { get; set; }
        public bool IsPropertyValueSet { get; set; }
        public bool IsObjectFromMember { get; set; }
        public object Collection { get; set; }
        public bool WasAssignedAtCreation { get; set; }
    }
}