namespace OmniXaml
{
    using System;

    public class XamlParseException : Exception
    {
        public XamlParseException(string message) : base(message)
        {
        }

        // ReSharper disable once UnusedMember.Global
        public XamlParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}