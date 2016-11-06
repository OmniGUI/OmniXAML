using Glass.Core;
using System;
using System.Linq;
using System.Reflection;

namespace OmniXaml
{
    internal class AttachedEvent : Property
    {
        private readonly object eventObject;
        private readonly MethodInfo addHandlerMethod;
        private readonly MethodInfo raiseEventMethod;

        public AttachedEvent(Type owner, string propertyName) : base(owner, propertyName)
        {
            eventObject = owner.GetRuntimeField($"{propertyName}Event").GetValue(null);
            addHandlerMethod = owner.GetRuntimeMethods()
                    .Where(method => method.Name == "AddHandler")
                    .OrderBy(method => method.GetParameters().Length)
                    .First();

            raiseEventMethod = owner.GetRuntimeMethods().First(method => method.Name == "RaiseEvent");
        }

        public override bool IsEvent => true;

        public override Type PropertyType =>
            addHandlerMethod.GetParameters()[1].ParameterType; // The second parameter is always the callback

        public override object GetValue(object instance)
        {
            return (Action<object>)(args => raiseEventMethod.Invoke(instance, new[] { args }));
        }

        public override void SetValue(object instance, object value)
        {
            var addHandlerDelegate = Utils.GetDelegateWithDefaultParameterValuesBound(instance, addHandlerMethod);
            addHandlerDelegate.DynamicInvoke(eventObject, value);
        }
    }
}