namespace OmniXaml.Assembler
{
    using System;
    using Typing;

    public class XamlSetValueEventArgs : EventArgs
    {
        public XamlSetValueEventArgs(XamlMember member, object value)
        {
            Value = value;
            Member = member;
        }

        public XamlMember Member { get; private set; }

        public object Value { get; private set; }

        public bool Handled { get; set; }
    }
}