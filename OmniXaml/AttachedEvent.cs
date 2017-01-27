namespace OmniXaml
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Zafiro.Core;

    internal class AttachedEvent : Member
    {
        private readonly Func<object, Delegate> addHandlerDelegate;
        private readonly MethodInfo addHandlerMethod;
        private readonly object eventObject;
        private readonly MethodInfo raiseEventMethod;

        public AttachedEvent(Type owner, string propertyName) : base(owner, propertyName)
        {
            eventObject = owner.GetRuntimeField($"{propertyName}Event").GetValue(null);

            addHandlerMethod = owner.GetRuntimeMethods()
                .Where(method => method.Name == "AddHandler")
                .OrderBy(method => method.GetParameters().Length)
                .First();
            addHandlerDelegate = addHandlerMethod.GetDelegateWithDefaultParameterValuesBound();

            raiseEventMethod = owner.GetRuntimeMethods().First(method => method.Name == "RaiseEvent");
        }

        public override Type MemberType =>
            addHandlerMethod.GetParameters()[1].ParameterType; // The second parameter is always the callback

        public override object GetValue(object instance)
        {
            return (Action<object>) (args => raiseEventMethod.Invoke(instance, new[] {args}));
        }

        public override void SetValue(object instance, object value)
        {
            addHandlerDelegate(instance).DynamicInvoke(eventObject, value);
        }
    }
}