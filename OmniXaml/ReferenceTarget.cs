using System.Reflection;

namespace OmniXaml
{
    public class ReferenceTarget
    {
        public object Instance { get; set; }
        public string PropertyName { get; set; }
        public object Value => Instance.GetType().GetRuntimeProperty(PropertyName).GetValue(Instance);
    }
}