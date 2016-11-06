namespace Glass.Core
{
    using System.Linq;
    using System.Reflection;

    public class Utils
    {
        public static void UniversalAdd(object collection, object item)
        {
            var addMethod = collection.GetType().GetTypeInfo().ImplementedInterfaces.SelectMany(x => x.GetRuntimeMethods()).First(n => n.Name == "Add");
            if (addMethod == null || addMethod.GetParameters().Length != 1)
            {
                // handle your error
                return;
            }
            ParameterInfo parameter = addMethod.GetParameters().First();
            if (parameter.ParameterType.GetTypeInfo().IsAssignableFrom(item.GetType().GetTypeInfo()))
            {
                addMethod.Invoke(collection, new[] { item });
            }
            else
            {
                // handle your error
            }
        }
    }
}