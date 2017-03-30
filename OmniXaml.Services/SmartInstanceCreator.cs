namespace OmniXaml.Services
{
    using System;
    using Rework;

    public class SmartInstanceCreator : ISmartInstanceCreator
    {
        public SmartInstanceCreator(IStringSourceValueConverter sourceValueConverter)
        {            
        }

        public CreationResult Create(Type type, CreationHints creationHints = null)
        {
            return new CreationResult(Activator.CreateInstance(type));
        }
    }
}