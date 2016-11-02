namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class InstanceCreator : IInstanceCreator
    {
        public object Create(Type type, IEnumerable<InjectableMember> injectableMembers = null)
        {
            var ctor = SelectCtor(type);
            return ctor.Invoke(null);
        }

        private static ConstructorInfo SelectCtor(Type type)
        {
            return GetParameterlessCtor(type) ?? FirstCtorWithMostArguments(type);
        }

        private static ConstructorInfo FirstCtorWithMostArguments(Type type)
        {
            var ctorsByNumOfArgs = from ctor in type.GetTypeInfo().DeclaredConstructors
                where ctor.IsPublic
                orderby ctor.GetParameters().Length descending
                select ctor;

            return ctorsByNumOfArgs.First();
        }

        private static ConstructorInfo GetParameterlessCtor(Type type)
        {
            var parameterlessCtors = from ctor in type.GetTypeInfo().DeclaredConstructors
                where ctor.IsPublic && !ctor.GetParameters().Any()
                select ctor;

            return parameterlessCtors.FirstOrDefault();
        }
    }
}