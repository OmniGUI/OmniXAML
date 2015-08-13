namespace OmniXaml
{
    using System;

    public class XamlParsingException : Exception
    {
        public XamlParsingException(string message) : base(message)
        {
        }

        // ReSharper disable once UnusedMember.Global
        public XamlParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}