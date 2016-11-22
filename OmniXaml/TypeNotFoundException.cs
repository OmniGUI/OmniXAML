namespace OmniXaml
{
    using System;

    public class TypeNotFoundException : Exception
    {
        public TypeNotFoundException(string message) : base(message)
        {            
        }
    }
}