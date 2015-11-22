namespace OmniXaml
{
    using System;

    public class InstanceLifeCycleHandler
    {
        public Action<object> OnBegin { get; set; } = NoAction;
        public Action<object> AfterProperties { get; set; } = NoAction;
        public Action<object> OnAssociatedToParent { get; set; } = NoAction;
        public Action<object> OnEnd { get; set; } = NoAction;

        private static void NoAction(object obj)
        {
        }
    }
}