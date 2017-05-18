using System.Reflection;

namespace OmniXaml.Tests.Model
{
    public class ReferenceTarget
    {
        public object Instance { get; set; }
        public string PropertyName { get; set; }
        public object Value => Instance.GetType().GetRuntimeProperty(PropertyName).GetValue(Instance);
    }
}