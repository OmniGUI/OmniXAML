namespace OmniXaml.Tests.Classes.Another
{
    using System.Collections.Generic;

    public class Foreigner
    {
        static readonly IDictionary<object, object> AttachedProperties = new Dictionary<object, object>();

        public static void SetProperty(object instance, object value)
        {
            AttachedProperties.Add(instance, value);
        }

        public static object GetProperty(object instance)
        {
            return AttachedProperties[instance];
        }
    }
}