namespace OmniXaml.ObjectAssembler
{
    using System;
    using Typing;

    public class XamlSetValueEventArgs : EventArgs
    {
        public XamlSetValueEventArgs(MutableMember member, object value)
        {
            Value = value;
            Member = member;
        }

        public MutableMember Member { get; private set; }

        public object Value { get; private set; }

        public bool Handled { get; set; }
    }
}