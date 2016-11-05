using System;
using System.Reflection;

namespace OmniXaml
{
    internal class StandardEvent : Property
    {
        private EventInfo eventInfo;

        public StandardEvent(Type owner, string eventName) : base(owner, eventName)
        {
            eventInfo = owner.GetRuntimeEvent(eventName);
        }

        public override Type PropertyType => eventInfo.EventHandlerType;

        public override bool IsEvent => true;

        public override object GetValue(object instance)
        {
            return eventInfo.RaiseMethod.CreateDelegate(eventInfo.EventHandlerType, instance);
        }

        public override void SetValue(object instance, object value)
        {
            eventInfo.AddEventHandler(instance, (Delegate)value);
        }
    }
}