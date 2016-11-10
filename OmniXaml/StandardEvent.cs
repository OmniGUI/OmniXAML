using System;
using System.Reflection;

namespace OmniXaml
{
    internal class StandardEvent : Member
    {
        private EventInfo eventInfo;

        public StandardEvent(Type owner, string eventName) : base(owner, eventName)
        {
            eventInfo = owner.GetRuntimeEvent(eventName);
        }

        public override Type MemberType => eventInfo.EventHandlerType;

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