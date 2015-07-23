namespace OmniXaml.Typing
{
    using System;

    public class XamlDirective : XamlMemberBase
    {
        public XamlDirective(string name) : base(name)
        {
            XamlType = XamlType.CreateForBuiltInType(typeof(object));
        }

        public XamlDirective(string name, XamlType xamlType) : base(name)
        {
            XamlType = xamlType;
        }

        public override bool IsDirective => true;
        public override bool IsAttachable => false;
    }
}