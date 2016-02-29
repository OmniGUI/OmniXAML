namespace OmniXaml.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Classes;

    internal class TestListener : IInstanceLifeCycleListener
    {
        readonly IList<SetupSequence> invocationOrder = new Collection<SetupSequence>();
        private readonly Type type = typeof(ChildClass);

        public ICollection InvocationOrder => new ReadOnlyCollection<SetupSequence>(invocationOrder);

        public void OnBegin(object instance)
        {
            if (IsCorrectType(instance))
            {
                invocationOrder.Add(SetupSequence.Begin);
            }
        }

        public void OnAfterProperties(object instance)
        {
            if (IsCorrectType(instance))
            {
                invocationOrder.Add(SetupSequence.AfterSetProperties);
            }
        }

        public void OnAssociatedToParent(object instance)
        {
            if (IsCorrectType(instance))
            {
                invocationOrder.Add(SetupSequence.AfterAssociatedToParent);
            }
        }

        public void OnEnd(object instance)
        {
            if (IsCorrectType(instance))
            {
                invocationOrder.Add(SetupSequence.End);
            }
        }

        private bool IsCorrectType(object instance)
        {
            if (instance == null)
            {
                return false;
            }

            return instance.GetType() == type;
        }
    }
}