namespace OmniXaml.Wpf
{
    using System.Windows.Controls;
    using System.Xaml;

    public class RootObjectProvider : IRootObjectProvider
    {
        public RootObjectProvider()
        {
            RootObject = new Button();
        }

        public object RootObject { get; }
    }
}