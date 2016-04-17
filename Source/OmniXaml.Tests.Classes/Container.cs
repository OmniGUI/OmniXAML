namespace OmniXaml.Tests.Classes
{
    using System.Collections.Generic;

    public class Container
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

        public static void SetCollection(object instance, CustomCollection value)
        {
            AttachedProperties.Add(instance, value);
        }

        public static CustomCollection GetCollection(object instance)
        {
            object col;
            var success = AttachedProperties.TryGetValue(instance, out col);
            if (success)
            {
                return (CustomCollection) col;
            }
            else
            {
                
                var customCollection = new CustomCollection();

                AttachedProperties[instance] = customCollection;
                return customCollection;
            }
        }
    }
}