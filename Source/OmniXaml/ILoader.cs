namespace OmniXaml
{
    using System.IO;

    public interface ILoader
    {
        object Load(Stream stream);
        object Load(Stream stream, object instance);
        object Load(Stream stream, LoadSettings loadSettings);
    }

    public class LoadSettings
    {
        public object RootInstance { get; set; }
        public IInstanceLifeCycleListener InstanceLifeCycleListener { get; set; }
    }

    public interface IInstanceLifeCycleListener
    {
        void OnBegin(object instance);
        void OnAfterProperites(object instance);
        void OnAssociatedToParent(object instance);
        void OnEnd(object instance);
    }
}