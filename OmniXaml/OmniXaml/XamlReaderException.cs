namespace OmniXaml
{
    using System;

    public class XamlReaderException : Exception
    {
        public XamlReaderException(string message) : base(message)
        {
        }

        public XamlReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}