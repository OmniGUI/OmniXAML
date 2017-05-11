using System;
using OmniXaml.Rework;

namespace OmniXaml.Tests
{
    public class FuncInstanceCreator : ISmartInstanceCreator
    {
        private readonly Func<CreationHints, Type, CreationResult> func;

        public FuncInstanceCreator(Func<CreationHints, Type, CreationResult> func)
        {
            this.func = func;
        }

        public CreationResult Create(Type type, CreationHints creationHints = null)
        {
            return func(creationHints, type);
        }
    }
}