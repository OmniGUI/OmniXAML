namespace OmniXaml.Tests.Model
{
    using System.Collections.Generic;
    using Zafiro.Core;

    public class VisualStateManager
    {
        private static readonly IDictionary<object, ICollection<VisualStateGroup>> GroupsByInstance = new Dictionary<object, ICollection<VisualStateGroup>>();

        public static void SetVisualStateGroups(object instance, ICollection<VisualStateGroup> groups)
        {
            GroupsByInstance.Add(instance, groups);
        }
        public static ICollection<VisualStateGroup> GetVisualStateGroups(object instance)
        {
            return GroupsByInstance.GetCreate(instance, () => new List<VisualStateGroup>());
        }
    }
}