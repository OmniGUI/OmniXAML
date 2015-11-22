namespace OmniXaml.ObjectAssembler
{
    internal class InstanceLifeCycleNotifier
    {
        private readonly IObjectAssembler objectAssembler;

        public InstanceLifeCycleNotifier(IObjectAssembler objectAssembler)
        {
            this.objectAssembler = objectAssembler;
        }

        public void NotifyBegin(object instance)
        {
            objectAssembler.InstanceLifeCycleHandler.OnBegin(instance);
        }

        public void NotifyAfterProperties(object instance)
        {
            objectAssembler.InstanceLifeCycleHandler.AfterProperties(instance);
        }

        public void NotifyAssociatedToParent(object instance)
        {
            objectAssembler.InstanceLifeCycleHandler.OnAssociatedToParent(instance);
        }

        public void NotifyEnd(object instance)
        {
            objectAssembler.InstanceLifeCycleHandler.OnEnd(instance);
        }
    }
}