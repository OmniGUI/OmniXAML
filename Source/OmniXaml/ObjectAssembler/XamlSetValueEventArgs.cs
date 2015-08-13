namespace OmniXaml.ObjectAssembler
{
    using System;
    using Typing;

    public class XamlSetValueEventArgs : EventArgs
    {
        public XamlSetValueEventArgs(MutableXamlMember member, object value)
        {
            Value = value;
            Member = member;
        }

        public MutableXamlMember Member { get; private set; }

        public object Value { get; private set; }

        public bool Handled { get; set; }
    }
}