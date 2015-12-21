namespace OmniXaml.Tests.Classes
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

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
            var succes = AttachedProperties.TryGetValue(instance, out col);
            if (succes)
            {
                return (CustomCollection)AttachedProperties[instance];
            }
            else
            {
                return new CustomCollection();
            }
        }
    }
}