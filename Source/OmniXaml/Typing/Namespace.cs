namespace OmniXaml.Typing
{
    using System;

    public abstract class Namespace
    {
        public abstract Type Get(string typeName);
        public string Name { get; protected set; }
    }
}