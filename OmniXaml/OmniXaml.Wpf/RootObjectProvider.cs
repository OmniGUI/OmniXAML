namespace OmniXaml.Wpf
{
    using System.Xaml;

    public class RootObjectProvider : IRootObjectProvider
    {
        public RootObjectProvider()
        {
            RootObject = null;
        }

        public object RootObject { get; }
    }
}