namespace OmniXaml.Wpf
{
    using System;
    using System.Reflection;
    using System.Windows;

    public static class Extensions
    {
        public static DependencyProperty GetDependencyProperty(this Type type, string name)
        {
            var dpPropName = name + "Property";
            FieldInfo fieldInfo = type.GetField(dpPropName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return (DependencyProperty)fieldInfo?.GetValue(null);
        }
    }
}