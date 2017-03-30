namespace OmniXaml
{
    using System.Collections.Generic;
    using Rework;

    public class CreationResult
    {
        public CreationResult(object instance) : this(instance, new CreationHints())
        {            
        }

        public CreationResult(object instance, CreationHints usedHints)
        {
            Instance = instance;
            UsedHints = usedHints;
        }

        public object Instance { get; }
        public CreationHints UsedHints { get; }
    }
}