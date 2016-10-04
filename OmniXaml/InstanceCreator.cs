namespace OmniXaml
{
    using System;

    public class InstanceCreator : IInstanceCreator
    {
        public object Create(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}