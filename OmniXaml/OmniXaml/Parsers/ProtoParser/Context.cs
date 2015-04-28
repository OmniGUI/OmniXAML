namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.Generic;
    using Typing;

    internal class Context
    {
        private readonly Stack<Scope> stack;

        public Context()
        {
            stack = new Stack<Scope>();
            stack.Push(new Scope(null, null));
        }

        public int Depth => stack.Count - 1;

        public XamlType CurrentType => stack.Count != 0 ? stack.Peek().XamlType : null;

        public string CurrentTypeNamespace => stack.Count != 0 ? stack.Peek().Namespace : null;

        public XamlMember CurrentProperty
        {
            get
            {
                return stack.Count != 0 ? stack.Peek().XamlProperty : null;
            }
            set
            {
                stack.Peek().XamlProperty = value;
            }
        }

        public bool IsCurrentlyInsideContent
        {
            get
            {
                return stack.Count != 0 && stack.Peek().IsInsideContent;
            }
            set
            {
                stack.Peek().IsInsideContent = value;
            }
        }

        public void Push(XamlType type, string ns)
        {
            stack.Push(new Scope(type, ns));
        }

        public void Pop()
        {
            stack.Pop();
        }
    }
}