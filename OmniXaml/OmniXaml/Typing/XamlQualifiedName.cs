namespace OmniXaml.Typing
{
    using System;

    public class XamlQualifiedName : XamlName
    {
        public XamlQualifiedName(string prefix, string propertyName)
            : base(prefix, propertyName)
        {
        }

        public override string ScopedName
        {
            get
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    return Prefix + ":" + PropertyName;
                }
                return PropertyName;
            }
        }

        public static bool Parse(string longName, out string prefix, out string name)
        {
            throw new NotImplementedException();
        }
    }
}